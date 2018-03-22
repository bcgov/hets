import React from 'react';

import { connect } from 'react-redux';

import { Well, PageHeader, Row, Col, Button, Form } from 'react-bootstrap';

import _ from 'lodash';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import store from '../store';

import MultiDropdown from '../components/MultiDropdown.jsx';

var Home = React.createClass({
  propTypes: {
    currentUser: React.PropTypes.object,
    owners: React.PropTypes.object,
    unapprovedOwners: React.PropTypes.object,
    unapprovedEquipment: React.PropTypes.object,
    districtEquipmentTypes: React.PropTypes.object,
    localAreas: React.PropTypes.object,
    router: React.PropTypes.object,
  },

  getInitialState() {
    return {
      selectedEquipmentTypesIds: [],
      selectedLocalAreasIds: [],
    };
  },

  componentDidMount() {
    Api.searchOwners({status: Constant.OWNER_STATUS_CODE_PENDING});
    Api.searchEquipmentList({status: Constant.EQUIPMENT_STATUS_CODE_PENDING});
    Api.getDistrictEquipmentTypes(this.props.currentUser.district.id);
  },

  goToUnapprovedOwners() {
    var search = {
      statusCode: Constant.OWNER_STATUS_CODE_PENDING,
    };
    store.dispatch({ type: Action.UPDATE_OWNERS_SEARCH, owners: search });
    this.props.router.push({ pathname: Constant.OWNERS_PATHNAME });
  },

  goToUnapprovedEquipment() {
    var search = {
      statusCode: Constant.EQUIPMENT_STATUS_CODE_PENDING,
    };
    store.dispatch({ type: Action.UPDATE_EQUIPMENT_LIST_SEARCH, equipmentList: search });
    this.props.router.push({ pathname: Constant.EQUIPMENT_PATHNAME });
  },

  updateState(state) {
    this.setState(state);
  },

  getRotationList(e) {
    e.preventDefault();
    Api.equipmentSeniorityListPdf(this.state.selectedLocalAreasIds, this.state.selectedEquipmentTypesIds).then(response => {
      var blob = new Blob([response], {type: 'image/pdf'});
      if (window.navigator.msSaveBlob) {
        blob = window.navigator.msSaveBlob([response], 'equipment_rotation_list.pdf');
      }
      //Create a link element, hide it, direct 
      //it towards the blob, and then 'click' it programatically
      let a = document.createElement('a');
      a.style = 'display: none';
      document.body.appendChild(a);
      //Create a DOMString representing the blob 
      //and point the link element towards it
      let url = window.URL.createObjectURL(blob);
      a.href = url;
      a.download = 'equipmentRotationList.pdf';
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

    var districtEquipmentTypes = _.chain(this.props.districtEquipmentTypes.data)
      .filter(type => type.district.id == this.props.currentUser.district.id)
      .sortBy('districtEquipmentName')
      .value();

    return <div id="home">
      <PageHeader>{this.props.currentUser.fullName}<br/>{this.props.currentUser.districtName} District</PageHeader>
      <Well>
        <Row>
          <Col md={6} className="btn-container">
            <Button onClick={ this.goToUnapprovedOwners }>Unapproved owners ({Object.keys(this.props.unapprovedOwners.data).length})</Button>
            <Button onClick={ this.goToUnapprovedEquipment }>Unapproved equipment ({Object.keys(this.props.unapprovedEquipment.data).length})</Button>          
          </Col>
          <Col md={6} className="btn-container">
            <Form onSubmit={ this.getRotationList }>
              <MultiDropdown id="selectedEquipmentTypesIds" className="fixed-width" placeholder="Equipment Types" fieldName="districtEquipmentName"
                items={ districtEquipmentTypes } updateState={ this.updateState} showMaxItems={ 2 } />
              <MultiDropdown id="selectedLocalAreasIds" className="fixed-width small" placeholder="Local Areas"
                items={ localAreas } updateState={ this.updateState } showMaxItems={ 2 } />
              <Button id="submit-button" bsStyle="primary" type="submit">Get Rotation List</Button>
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
    search: state.search.owners,
    unapprovedOwners: state.models.owners,
    unapprovedEquipment: state.models.equipmentList,
    districtEquipmentTypes: state.lookups.districtEquipmentTypes,
    localAreas: state.lookups.localAreas,
  };
}

export default connect(mapStateToProps)(Home);
