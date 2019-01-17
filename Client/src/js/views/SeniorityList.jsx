import React from 'react';

import { connect } from 'react-redux';

import { Well, PageHeader, Row, Col, Button, Form } from 'react-bootstrap';

import _ from 'lodash';

import * as Api from '../api';

import MultiDropdown from '../components/MultiDropdown.jsx';

var SeniorityList = React.createClass({
  propTypes: {
    currentUser: React.PropTypes.object,
    districtEquipmentTypes: React.PropTypes.object,
    localAreas: React.PropTypes.object,
  },

  getInitialState() {
    return {
      selectedEquipmentTypeIds: [],
      selectedLocalAreaIds: [],
    };
  },

  componentDidMount() {
    this.fetch();
  },

  fetch() {
    Api.getDistrictEquipmentTypes(this.props.currentUser.district.id);
  },

  updateState(state) {
    this.setState(state);
  },
  
  onLocalAreasChanged() {
    this.setState({ selectedEquipmentTypeIds: [] });
  },

  getFilteredEquipmentTypes(localAreaIds) {
    return _.chain(this.props.districtEquipmentTypes)
      .filter(type => type.equipmentCount > 0 && localAreaIds.length === 0 || _.filter(type.localAreas, localArea => _.includes(localAreaIds, localArea.id) && localArea.equipmentCount > 0).length > 0)
      .sortBy('districtEquipmentName')
      .value();
  },

  getRotationList(e) {
    e.preventDefault();
    Api.equipmentSeniorityListPdf(this.state.selectedLocalAreaIds, this.state.selectedEquipmentTypeIds).then(response => {
      var blob = new Blob([response], {type: 'image/pdf'});
      if (window.navigator.msSaveBlob) {
        // ie11
        window.navigator.msSaveBlob(blob, 'seniority_list.pdf');
        return;
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
      a.download = 'seniority_list.pdf';
      //programatically click the link to trigger the download
      a.click();
      //release the reference to the file by revoking the Object URL
      window.URL.revokeObjectURL(url);
    });
  },

  render() {
    var localAreas = _.chain(this.props.localAreas)
      .sortBy('name')
      .value();
    
    var districtEquipmentTypes = this.getFilteredEquipmentTypes(this.state.selectedLocalAreaIds);

    return <div id="seniority-list">
      <PageHeader>Seniority List</PageHeader>
      <Well>
        <Row>
          <Col md={12} className="btn-container">
            <Form className="rotation-list-form" onSubmit={ this.getRotationList }>
              <MultiDropdown id="selectedLocalAreaIds" className="fixed-width small" placeholder="Local Areas" items={ localAreas }
                selectedIds={ this.state.selectedLocalAreaIds } updateState={ this.updateState } onChange={ this.onLocalAreasChanged } showMaxItems={ 2 } />
              <MultiDropdown id="selectedEquipmentTypeIds" className="fixed-width" placeholder="Equipment Types" fieldName="districtEquipmentName"
                items={ districtEquipmentTypes } selectedIds={ this.state.selectedEquipmentTypeIds } updateState={ this.updateState } showMaxItems={ 2 } />
              <Button id="submit-button" bsStyle="primary" type="submit">Seniority List</Button>
            </Form>
          </Col>
        </Row>
      </Well>
    </div>;
  },
});


function mapStateToProps(state) {
  return {
    currentUser: state.user,
    districtEquipmentTypes: state.lookups.districtEquipmentTypes.data,
    localAreas: state.lookups.localAreas,
  };
}

export default connect(mapStateToProps)(SeniorityList);
