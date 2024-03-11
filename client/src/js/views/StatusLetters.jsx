import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { Row, Col, ButtonToolbar, Button } from 'react-bootstrap';
import _ from 'lodash';
import { saveAs } from 'file-saver';

import * as Api from '../api';
import * as Constant from '../constants';

import PageHeader from '../components/ui/PageHeader.jsx';
import SearchBar from '../components/ui/SearchBar.jsx';
import MultiDropdown from '../components/MultiDropdown.jsx';

import { formatDateTimeUTCToLocal } from '../utils/date';

class StatusLetters extends React.Component {
  static propTypes = {
    localAreas: PropTypes.object,
    owners: PropTypes.object,
  };

  constructor(props) {
    super(props);

    this.state = {
      localAreaIds: [],
      ownerIds: [],
    };
  }

  componentDidMount() {
    this.props.dispatch(Api.getOwnersLite());
  }

  updateState = (state, callback) => {
    this.setState(state, () => {
      if (callback) {
        callback();
      }
    });
  };

  getStatusLetters = async () => {
    const filename = 'StatusLetters-' + formatDateTimeUTCToLocal(new Date(), Constant.DATE_TIME_FILENAME) + '.docx';
    try {
      const res = await this.props.dispatch(Api.getStatusLettersDoc({
        localAreas: this.state.localAreaIds,
        owners: this.state.ownerIds,
      }));
      saveAs(res, filename);
    } catch(error) {
      console.log(error);
    }
  };

  getMailingLabel = async () => {
    const filename = 'MailingLabels-' + formatDateTimeUTCToLocal(new Date(), Constant.DATE_TIME_FILENAME) + '.docx';
    try {
      const res = await this.props.dispatch(Api.getMailingLabelsDoc({
        localAreas: this.state.localAreaIds,
        owners: this.state.ownerIds,
      }));
      saveAs(res, filename);
    } catch(error) {
      console.log(error);
    }
  };

  matchesLocalAreaFilter = (localAreaId) => {
    if (this.state.localAreaIds.length === 0) {
      return true;
    }

    return _.includes(this.state.localAreaIds, localAreaId);
  };

  updateLocalAreaState = (state) => {
    this.updateState(state, this.filterSelectedOwners);
  };

  filterSelectedOwners = () => {
    var acceptableOwnerIds = _.map(this.getFilteredOwners(), 'id');
    var ownerIds = _.intersection(this.state.ownerIds, acceptableOwnerIds);
    this.updateState({ ownerIds: ownerIds });
  };

  getFilteredOwners = () => {
    return _.chain(this.props.owners.data)
      .filter((x) => this.matchesLocalAreaFilter(x.localAreaId))
      .sortBy('organizationName')
      .value();
  };

  render() {
    var localAreas = _.sortBy(this.props.localAreas, 'name');
    var owners = this.getFilteredOwners();

    return (
      <div id="status-letters">
        <PageHeader>Status Letters</PageHeader>
        <SearchBar>
          <Row>
            <Col md={12} id="filters">
              <ButtonToolbar>
                <MultiDropdown
                  id="localAreaIds"
                  placeholder="Local Areas"
                  items={localAreas}
                  selectedIds={this.state.localAreaIds}
                  updateState={this.updateLocalAreaState}
                  showMaxItems={2}
                />
                <MultiDropdown
                  id="ownerIds"
                  placeholder="Companies"
                  fieldName="organizationName"
                  items={owners}
                  disabled={!this.props.owners.loaded}
                  selectedIds={this.state.ownerIds}
                  updateState={this.updateState}
                  showMaxItems={2}
                />
                <Button onClick={this.getStatusLetters} variant="primary">
                  Status Letters
                </Button>
                <Button onClick={this.getMailingLabel} variant="primary">
                  Mailing Labels
                </Button>
              </ButtonToolbar>
            </Col>
          </Row>
        </SearchBar>
      </div>
    );
  }
}

const mapStateToProps = (state) => ({
  localAreas: state.lookups.localAreas,
  owners: state.lookups.owners.lite,
});

const mapDispatchToProps = (dispatch) => ({ dispatch });

export default connect(mapStateToProps, mapDispatchToProps)(StatusLetters);
