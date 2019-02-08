import React from 'react';

import { connect } from 'react-redux';
import { Link } from 'react-router';

import { Well, PageHeader, Row, Col, Button, Alert, ButtonGroup, Glyphicon, HelpBlock } from 'react-bootstrap';

import _ from 'lodash';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import DeleteButton from '../components/DeleteButton.jsx';
import EditButton from '../components/EditButton.jsx';
import SortTable from '../components/SortTable.jsx';
import Spinner from '../components/Spinner.jsx';
import store from '../store';

var Home = React.createClass({
  propTypes: {
    currentUser: React.PropTypes.object,
    owners: React.PropTypes.object,
    unapprovedOwners: React.PropTypes.object,
    unapprovedEquipment: React.PropTypes.object,
    hiredEquipment: React.PropTypes.object,
    blockedRotationLists: React.PropTypes.object,
    rentalAgreement: React.PropTypes.object,
    rentalAgreements: React.PropTypes.object,
    blankRentalAgreements: React.PropTypes.object,
    uiBlankRentalAgreements: React.PropTypes.object,
    router: React.PropTypes.object,
  },

  getInitialState() {
    return {
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
    Api.getHiredEquipment();
    Api.getBlockedRotationLists();
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

  goToHiredEquipment() {
    // update search parameters
    store.dispatch({ type: Action.UPDATE_EQUIPMENT_LIST_SEARCH, equipmentList: { statusCode: Constant.EQUIPMENT_STATUS_CODE_APPROVED, hired: true  } });
    
    // perform search
    Api.searchEquipmentList({ status: Constant.EQUIPMENT_STATUS_CODE_APPROVED, hired: true  });

    // navigate to search page
    this.props.router.push({ pathname: Constant.EQUIPMENT_PATHNAME });
  },

  goToBlockedRotationLists() {
    // update search parameters
    store.dispatch({ type: Action.UPDATE_RENTAL_REQUESTS_SEARCH, rentalRequests: { statusCode: Constant.RENTAL_REQUEST_STATUS_CODE_IN_PROGRESS  } });
    
    // perform search
    Api.searchRentalRequests({ status: Constant.RENTAL_REQUEST_STATUS_CODE_IN_PROGRESS  });

    // navigate to search page
    this.props.router.push({ pathname: Constant.RENTAL_REQUESTS_PATHNAME });
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
    return <div id="home">
      <PageHeader>{this.props.currentUser.fullName}<br/>{this.props.currentUser.districtName} District</PageHeader>
      <Well>
        <h3>Summary</h3>
        <Row>
          <Col md={12} className="btn-container">
            <Button onClick={ this.goToUnapprovedOwners }>Unapproved owners { this.props.unapprovedOwners.loaded && `(${ Object.keys(this.props.unapprovedOwners.data).length })` }</Button>
            <Button onClick={ this.goToUnapprovedEquipment }>Unapproved equipment { this.props.unapprovedEquipment.loaded && `(${ Object.keys(this.props.unapprovedEquipment.data).length })` }</Button>
            <Button onClick={ this.goToHiredEquipment }>Currently hired equipment { this.props.hiredEquipment.loaded && `(${ Object.keys(this.props.hiredEquipment.data).length })` }</Button>
            <Button onClick={ this.goToBlockedRotationLists }>Blocked rotation lists { this.props.blockedRotationLists.loaded && `(${ Object.keys(this.props.blockedRotationLists.data).length })` }</Button>
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
    hiredEquipment: state.models.hiredEquipmentList,
    blockedRotationLists: state.models.blockedRotationLists,
    rentalAgreement: state.models.rentalAgreement,
    rentalAgreements: state.models.rentalAgreements,
    blankRentalAgreements: state.lookups.blankRentalAgreements,
    uiBlankRentalAgreements: state.ui.blankRentalAgreements,
  };
}

export default connect(mapStateToProps)(Home);
