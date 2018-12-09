import React from 'react';

import { connect } from 'react-redux';
import { Link } from 'react-router';

import { Well, PageHeader, Row, Col, Button, Form, Alert, ButtonGroup, Glyphicon, HelpBlock } from 'react-bootstrap';

import _ from 'lodash';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import DeleteButton from '../components/DeleteButton.jsx';
import EditButton from '../components/EditButton.jsx';
import SortTable from '../components/SortTable.jsx';
import Spinner from '../components/Spinner.jsx';
import store from '../store';

import MultiDropdown from '../components/MultiDropdown.jsx';

var Home = React.createClass({
  propTypes: {
    currentUser: React.PropTypes.object,
    owners: React.PropTypes.object,
    unapprovedOwners: React.PropTypes.object,
    unapprovedEquipment: React.PropTypes.object,
    districtEquipmentTypes: React.PropTypes.object,
    rentalAgreement: React.PropTypes.object,
    rentalAgreements: React.PropTypes.object,
    localAreas: React.PropTypes.object,
    blankRentalAgreements: React.PropTypes.object,
    uiBlankRentalAgreements: React.PropTypes.object,
    router: React.PropTypes.object,
  },

  getInitialState() {
    return {
      selectedEquipmentTypesIds: [],
      selectedLocalAreasIds: [],

      uiBlankRentalAgreements : {
        sortField: this.props.uiBlankRentalAgreements.sortField || 'agreementNumber',
        sortDesc: this.props.uiBlankRentalAgreements.sortDesc === false,
      },
    };
  },

  componentDidMount() {
    this.fetch();
  },

  fetch() {
    Api.getUnapprovedOwners();
    Api.getUnapprovedEquipment();
    Api.getDistrictEquipmentTypes(this.props.currentUser.district.id);
    this.fetchBlankRentalAgreements();
  },

  fetchBlankRentalAgreements() {
    Api.getBlankRentalAgreements();
  },

  goToUnapprovedOwners() {
    var unapprovedStatus = Constant.OWNER_STATUS_CODE_PENDING;

    // update search parameters
    store.dispatch({ type: Action.UPDATE_OWNERS_SEARCH, owners: { statusCode: unapprovedStatus } });
    
    // perform search
    Api.searchOwners({ status: unapprovedStatus });
    
    // navigate to search page
    this.props.router.push({ pathname: Constant.OWNERS_PATHNAME });
  },

  goToUnapprovedEquipment() {
    var unapprovedStatus = Constant.EQUIPMENT_STATUS_CODE_PENDING;
    
    // update search parameters
    store.dispatch({ type: Action.UPDATE_EQUIPMENT_LIST_SEARCH, equipmentList: { statusCode: Constant.EQUIPMENT_STATUS_CODE_PENDING } });
    
    // perform search
    Api.searchEquipmentList({ status: unapprovedStatus });

    // navigate to search page
    this.props.router.push({ pathname: Constant.EQUIPMENT_PATHNAME });
  },

  createRentalAgreement() {
    Api.addBlankRentalAgreement().then(() => {
      // navigate to newly created agreement
      this.props.router.push({
        pathname: `${ Constant.RENTAL_AGREEMENTS_PATHNAME }/${ this.props.rentalAgreement.id }`,
      });
    });
  },

  editRentalAgreement(id) {
    this.props.router.push({
      pathname: `${ Constant.RENTAL_AGREEMENTS_PATHNAME }/${ id }`,
    });
  },

  deleteRentalAgreement(id) {
    Api.deleteBlankRentalAgreement(id).then(() => {
      this.fetchBlankRentalAgreements();
    });
  },

  updateRentalAgreementsUIState(state, callback) {
    this.setState({ uiBlankRentalAgreements: { ...this.state.uiBlankRentalAgreements, ...state }}, () => {
      store.dispatch({ type: Action.UPDATE_BLANK_RENTAL_AGREEMENTS_UI, blankRentalAgreements: this.state.uiBlankRentalAgreements });
      if (callback) { callback(); }
    });
  },

  updateState(state) {
    this.setState(state);
  },

  getRotationList(e) {
    e.preventDefault();
    Api.equipmentSeniorityListPdf(this.state.selectedLocalAreasIds, this.state.selectedEquipmentTypesIds).then(response => {
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

  renderAgreements() {
    if (!this.props.currentUser.hasPermission(Constant.PERMISSION_ADMIN)) {
      return null;
    }

    var agreements = this.props.blankRentalAgreements.data;
    var agreementCount = Object.keys(agreements).length;
    
    return <Well>
      <h3>Unassociated Rental Agreements</h3>
      <p>(These equipment have not yet been hired against a rental request in the application.)</p>
      {(() => {
        if (this.props.blankRentalAgreements.loading ) { return <div className="spinner-container"><Spinner/></div>; }

        var addRentalAgreementButton = null;
        if (agreementCount < Constant.MAX_UNASSOCIATED_RENTAL_AGREEMENTS) {
          addRentalAgreementButton = <Button title="Add Rental Agreement" onClick={ this.createRentalAgreement } bsSize="small"><Glyphicon glyph="plus" />&nbsp;<strong>Add</strong></Button>;
        }
        
        if (!agreements || agreementCount === 0) { return <Alert bsStyle="success">No unassociated rental agreements { addRentalAgreementButton }</Alert>; }
      
        // assign sequence numbers
        var sortedAgreements = _.sortBy(agreements, 'id');
        _.map(sortedAgreements, (agreement, index) => { agreement.sequence = index + 1; });

        // sort by selected column
        sortedAgreements = _.sortBy(agreements, this.state.uiBlankRentalAgreements.sortField);
        if (this.state.uiBlankRentalAgreements.sortDesc) {
          _.reverse(sortedAgreements);
        }

        var headers = [
          { field: 'sequence',        title: '#'                },
          { field: 'agreementNumber', title: 'Rental Agreement' },
          { field: 'projectName',     title: 'Project'          },
          { field: 'equipmentCode',   title: 'Equipment ID'     },
          { field: 'add', style: { textAlign: 'right' }, node: addRentalAgreementButton },
        ];

        return <SortTable id="rental-agreement-list" sortField={ this.state.uiBlankRentalAgreements.sortField } sortDesc={ this.state.uiBlankRentalAgreements.sortDesc } onSort={ this.updateRentalAgreementsUIState } headers={ headers }>
          {
            _.map(sortedAgreements, (agreement) => {
              return <tr key={ agreement.id }>
                <td>{ agreement.sequence } </td>
                <td>{ agreement.number }</td>
                {(() => {
                  if (!_.isEmpty(agreement.project)) { return <td><Link to={ `${ Constant.PROJECTS_PATHNAME }/${ agreement.projectId }` }>{ agreement.project.name }</Link></td>; }
                  
                  return <td></td>;
                })()}
                {(() => {
                  if (!_.isEmpty(agreement.equipment)) { return <td><Link to={ `${ Constant.EQUIPMENT_PATHNAME }/${ agreement.equipmentId }` }>{ agreement.equipment.equipmentCode }</Link></td>; }
                  
                  return <td></td>;
                })()}
                <td style={{ textAlign: 'right' }}>
                  <ButtonGroup>
                    <DeleteButton name="Rental Agreement" hide={ !agreement.canDelete } onConfirm={ this.deleteRentalAgreement.bind(this, agreement.id) } />
                    <EditButton name="Rental Agreement" view={ !agreement.canEdit } onClick={ this.editRentalAgreement.bind(this, agreement.id) } />
                  </ButtonGroup>
                </td>
              </tr>;
            })
          }
        </SortTable>;
      })()}
      { agreementCount >= Constant.MAX_UNASSOCIATED_RENTAL_AGREEMENTS && <HelpBlock>The maximum number of unassociated rental agreements has been reached.</HelpBlock> }
    </Well>;
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
            <Button onClick={ this.goToUnapprovedOwners }>Unapproved owners { this.props.unapprovedOwners.loaded && `(${ Object.keys(this.props.unapprovedOwners.data).length })` }</Button>
            <Button onClick={ this.goToUnapprovedEquipment }>Unapproved equipment { this.props.unapprovedEquipment.loaded && `(${ Object.keys(this.props.unapprovedEquipment.data).length })` }</Button>          
          </Col>
          <Col md={6} className="btn-container">
            <Form className="rotation-list-form" onSubmit={ this.getRotationList }>
              <MultiDropdown id="selectedEquipmentTypesIds" className="fixed-width" placeholder="Equipment Types" fieldName="districtEquipmentName"
                items={ districtEquipmentTypes } updateState={ this.updateState} showMaxItems={ 2 } />
              <MultiDropdown id="selectedLocalAreasIds" className="fixed-width small" placeholder="Local Areas"
                items={ localAreas } updateState={ this.updateState } showMaxItems={ 2 } />
              <Button id="submit-button" bsStyle="primary" type="submit">Seniority List</Button>
            </Form>
          </Col>
        </Row>
      </Well>
      { this.renderAgreements() }
    </div>;
  },
});


function mapStateToProps(state) {
  return {
    currentUser: state.user,
    search: state.search.owners,
    unapprovedOwners: state.models.unapprovedOwners,
    unapprovedEquipment: state.models.unapprovedEquipmentList,
    rentalAgreement: state.models.rentalAgreement,
    rentalAgreements: state.models.rentalAgreements,
    districtEquipmentTypes: state.lookups.districtEquipmentTypes,
    localAreas: state.lookups.localAreas,
    blankRentalAgreements: state.lookups.blankRentalAgreements,
    uiBlankRentalAgreements: state.ui.blankRentalAgreements,
  };
}

export default connect(mapStateToProps)(Home);
