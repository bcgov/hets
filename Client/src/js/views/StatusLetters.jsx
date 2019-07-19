import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { Row, Col, ButtonToolbar, Button } from 'react-bootstrap';
import _ from 'lodash';

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
    Api.getOwnersLite();
  }

  updateState = (state, callback) => {
    this.setState(state, () => {
      if (callback) { callback(); }
    });
  };

  downloadFile = (promise, filename, mimeType) => {
    promise.then((response) => {
      var blob;
      if (window.navigator.msSaveBlob) {
        blob = window.navigator.msSaveBlob(response, filename);
      } else {
        blob = new Blob([response], {type: mimeType});
      }
      //Create a link element, hide it, direct
      //it towards the blob, and then 'click' it programatically
      let a = document.createElement('a');
      a.style.cssText = 'display: none';
      document.body.appendChild(a);
      //Create a DOMString representing the blob
      //and point the link element towards it
      let url = window.URL.createObjectURL(blob);
      a.href = url;
      a.download = filename;
      //programatically click the link to trigger the download
      a.click();
      //release the reference to the file by revoking the Object URL
      window.URL.revokeObjectURL(url);
    });
  };

  getStatusLetters = () => {
    const promise = Api.getStatusLettersDoc({ localAreas: this.state.localAreaIds, owners: this.state.ownerIds });
    const filename = 'StatusLetters-' + formatDateTimeUTCToLocal(new Date(), Constant.DATE_TIME_FILENAME) + '.docx';
    const mimeType = 'application/vnd.openxmlformats-officedocument.wordprocessingml.document';

    this.downloadFile(promise, filename, mimeType);
  };

  getMailingLabel = () => {
    const promise = Api.getMailingLabelsDoc({ localAreas: this.state.localAreaIds, owners: this.state.ownerIds });
    const filename = 'MailingLabels-' + formatDateTimeUTCToLocal(new Date(), Constant.DATE_TIME_FILENAME) + '.docx';
    const mimeType = 'application/vnd.openxmlformats-officedocument.wordprocessingml.document';

    this.downloadFile(promise, filename, mimeType);
  };

  matchesLocalAreaFilter = (localAreaId) => {
    if (this.state.localAreaIds.length == 0) {
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
      .filter(x => this.matchesLocalAreaFilter(x.localAreaId))
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
                  showMaxItems={2} />
                <MultiDropdown
                  id="ownerIds"
                  placeholder="Companies"
                  fieldName="organizationName"
                  items={owners}
                  disabled={!this.props.owners.loaded}
                  selectedIds={this.state.ownerIds}
                  updateState={this.updateState}
                  showMaxItems={2} />
                <Button onClick={ this.getStatusLetters } bsStyle="primary">Status Letters</Button>
                <Button onClick={ this.getMailingLabel } bsStyle="primary">Mailing Labels</Button>
              </ButtonToolbar>
            </Col>
          </Row>
        </SearchBar>
      </div>
    );
  }
}

function mapStateToProps(state) {
  return {
    localAreas: state.lookups.localAreas,
    owners: state.lookups.owners.lite,
  };
}

export default connect(mapStateToProps)(StatusLetters);
