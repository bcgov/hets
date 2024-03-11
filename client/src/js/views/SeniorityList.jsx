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

class SeniorityList extends React.Component {
  static propTypes = {
    currentUser: PropTypes.object,
    districtEquipmentTypes: PropTypes.object,
    localAreas: PropTypes.object,
  };

  constructor(props) {
    super(props);

    this.state = {
      selectedEquipmentTypeIds: [],
      selectedLocalAreaIds: [],
    };
  }

  componentDidMount() {
    this.fetch();
  }

  fetch = () => {
    this.props.dispatch(Api.getDistrictEquipmentTypes());
  };

  updateState = (state) => {
    this.setState(state);
  };

  onLocalAreasChanged = () => {
    this.setState({ selectedEquipmentTypeIds: [] });
  };

  getFilteredEquipmentTypes = (localAreaIds) => {
    return _.chain(this.props.districtEquipmentTypes.data)
      .filter(
        (type) =>
          (type.equipmentCount > 0 && localAreaIds.length === 0) ||
          _.filter(
            type.localAreas,
            (localArea) => _.includes(localAreaIds, localArea.id) && localArea.equipmentCount > 0
          ).length > 0
      )
      .sortBy('districtEquipmentName')
      .value();
  };

  getRotationList = async (counterCopy) => {
    const filename =
      'SeniorityList-' +
      formatDateTimeUTCToLocal(new Date(), Constant.DATE_TIME_FILENAME) +
      (counterCopy ? '-(CounterCopy)' : '') +
      '.docx';

    try {
      const res = await this.props.dispatch(
        Api.equipmentSeniorityListDoc(this.state.selectedLocalAreaIds, this.state.selectedEquipmentTypeIds, counterCopy));

      saveAs(res, filename);
    } catch(error) {
      console.log(error);
    }
  };

  render() {
    var localAreas = _.chain(this.props.localAreas).sortBy('name').value();

    var districtEquipmentTypes = this.getFilteredEquipmentTypes(this.state.selectedLocalAreaIds);

    return (
      <div id="seniority-list">
        <PageHeader>Seniority List</PageHeader>
        <SearchBar>
          <Row>
            <Col md={12} id="filters">
              <ButtonToolbar className="btn-container">
                <MultiDropdown
                  id="selectedLocalAreaIds"
                  className="fixed-width small"
                  placeholder="Local Areas"
                  items={localAreas}
                  selectedIds={this.state.selectedLocalAreaIds}
                  updateState={this.updateState}
                  onChange={this.onLocalAreasChanged}
                  showMaxItems={2}
                />
                <MultiDropdown
                  id="selectedEquipmentTypeIds"
                  className="fixed-width"
                  placeholder="Equipment Types"
                  fieldName="districtEquipmentName"
                  items={districtEquipmentTypes}
                  disabled={!this.props.districtEquipmentTypes.loaded}
                  selectedIds={this.state.selectedEquipmentTypeIds}
                  updateState={this.updateState}
                  showMaxItems={2}
                />
                <Button onClick={() => this.getRotationList(false)} variant="primary">
                  Seniority List
                </Button>
                <Button onClick={() => this.getRotationList(true)} variant="primary">
                  Seniority List (Counter Copy)
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
  currentUser: state.user,
  districtEquipmentTypes: state.lookups.districtEquipmentTypes,
  localAreas: state.lookups.localAreas,
});

const mapDispatchToProps = (dispatch) => ({ dispatch });

export default connect(mapStateToProps, mapDispatchToProps)(SeniorityList);
