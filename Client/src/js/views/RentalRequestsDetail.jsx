import React from 'react';

import { connect } from 'react-redux';

import { Grid, Well, Row, Col } from 'react-bootstrap';
import { Alert, Button, ButtonGroup, Glyphicon, Label } from 'react-bootstrap';
import { Link } from 'react-router';
import { LinkContainer } from 'react-router-bootstrap';

import _ from 'lodash';
import Promise from 'bluebird';

import HireOfferEditDialog from './dialogs/HireOfferEditDialog.jsx';
import RentalRequestsEditDialog from './dialogs/RentalRequestsEditDialog.jsx';
import DocumentsListDialog from './dialogs/DocumentsListDialog.jsx';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import store from '../store';

import CheckboxControl from '../components/CheckboxControl.jsx';
import ColDisplay from '../components/ColDisplay.jsx';
import Spinner from '../components/Spinner.jsx';
import TableControl from '../components/TableControl.jsx';
import Unimplemented from '../components/Unimplemented.jsx';

import { formatDateTime } from '../utils/date';
import { concat } from '../utils/string';

/*

TODO:
* Print / Notes / Docs / Contacts (TBD) / History / Request Status List / Clone / Request Attachments

*/

var RentalRequestsDetail = React.createClass({
  propTypes: {
    rentalRequest: React.PropTypes.object,
    rentalAgreement: React.PropTypes.object,
    notes: React.PropTypes.object,
    attachments: React.PropTypes.object,
    documents: React.PropTypes.object,
    history: React.PropTypes.object,
    params: React.PropTypes.object,
    ui: React.PropTypes.object,
    router: React.PropTypes.object,
  },

  getInitialState() {
    return {
      loading: false,
      loadingHistory: false,

      showEditDialog: false,
      showHireOfferDialog: false,
      showContactDialog: false,

      showHiredEquipment: false,

      contact: {},
      rotationListHireOffer: {},

      isNew: this.props.params.rentalRequestId == 0,
    };
  },

  componentDidMount() {
    this.fetch();
  },

  fetch() {
    this.setState({ loading: true });

    var rentalRequestsPromise = Api.getRentalRequest(this.props.params.rentalRequestId);
    var documentsPromise = Api.getRentalRequestDocuments(this.props.params.rentalRequestId);

    return Promise.all([rentalRequestsPromise, documentsPromise]).finally(() => {
      this.setState({ loading: false });
    });
  },

  updateState(state, callback) {
    this.setState(state, callback);
  },

  updateContactsUIState(state, callback) {
    this.setState({ ui: { ...this.state.ui, ...state }}, () => {
      store.dispatch({ type: Action.UPDATE_PROJECT_CONTACTS_UI, projectContacts: this.state.ui });
      if (callback) { callback(); }
    });
  },

  showNotes() {

  },

  showDocuments() {
    this.setState({ showDocumentsDialog: true });
  },

  closeDocumentsDialog() {
    this.setState({ showDocumentsDialog: false });
  },

  addNote() {

  },

  addDocument() {

  },

  openEditDialog() {
    this.setState({ showEditDialog: true });
  },

  closeEditDialog() {
    this.setState({ showEditDialog: false });
  },

  saveEdit(rentalRequest) {
    Api.updateRentalRequest(rentalRequest).finally(() => {
      this.closeEditDialog();
    });
  },

  openHireOfferDialog(hireOffer) {
    this.setState({
      rotationListHireOffer: hireOffer,
      showHireOfferDialog: true,
    });
  },

  closeHireOfferDialog() {
    this.setState({ showHireOfferDialog: false });
  },

  saveHireOffer(hireOffer) {
    Api.updateRentalRequestRotationList(hireOffer, this.props.rentalRequest).finally(() => {
      this.fetch();
      this.closeHireOfferDialog();
    });
  },

  saveNewRentalAgreement(rentalRequestRotationList) {
    var rentalRequest = this.props.rentalRequest;

    var newAgreement = {
      equipment: { id: rentalRequestRotationList.equipment.id },
      project: { id: rentalRequest.project.id },
      estimateHours: rentalRequest.expectedHours,
      estimateStartWork: rentalRequest.expectedStartDate,
    };

    Api.addRentalAgreement(newAgreement).then(() => {
      // Update rotation list entry to reference the newly created agreement
      return Api.updateRentalRequestRotationList({...rentalRequestRotationList, rentalAgreement: { id: this.props.rentalAgreement.id }}, rentalRequest);
    }).finally(() => {
      // Open it up
      this.props.router.push({ pathname: `${Constant.RENTAL_AGREEMENTS_PATHNAME}/${this.props.rentalAgreement.id}` });
    });
  },

  openContactDialog(contact) {
    this.setState({
      contact: contact,
      showContactDialog: true,
    });
  },

  closeContactDialog() {
    this.setState({ showContactDialog: false });
  },

  saveContact() {
    // TODO Save contact
  },

  print() {
    window.print();
  },

  addRequest() {

  },

  cloneRequest() {
    // TODO
  },

  render() {
    var rentalRequest = this.props.rentalRequest;

    return <div id="rental-requests-detail">
      <Row id="rental-requests-top">
        <Col md={10}>
          <Label bsStyle={ rentalRequest.isActive ? 'success' : rentalRequest.isCancelled ? 'danger' : 'default' }>{ rentalRequest.status }</Label>
          <Unimplemented>
            <Button title="Clone" onClick={ this.cloneRequest }>Clone</Button>
          </Unimplemented>
          <Unimplemented>
            <Button title="Notes" onClick={ this.showNotes }>Notes ({ Object.keys(this.props.notes).length })</Button>
          </Unimplemented>
          <Button title="Documents" onClick={ this.showDocuments }>Documents ({ Object.keys(this.props.documents).length })</Button>
        </Col>
        <Col md={2}>
          <div className="pull-right">
            <LinkContainer to={{ pathname: Constant.RENTAL_REQUESTS_PATHNAME }}>
              <Button title="Return to List"><Glyphicon glyph="arrow-left" /> Return to List</Button>
            </LinkContainer>
          </div>
        </Col>
      </Row>

      {(() => {
        if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

        return <Grid fluid id="rental-requests-header">
          <Row>
            <ColDisplay md={12} labelProps={{ md: 1 }} label={ <h1>Project:</h1> }><h1><small>{ rentalRequest.projectName }</small></h1></ColDisplay>
          </Row>
        </Grid>;
      })()}

      <Well className="request-information">
        <h3>Request Information <span className="pull-right">
          <Button title="Edit Rental Request" bsSize="small" onClick={ this.openEditDialog }><Glyphicon glyph="pencil" /></Button>
        </span></h3>
        {(() => {
          if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

          var numRequestAttachments = Object.keys(rentalRequest.rentalRequestAttachment || []).length;
          var requestAttachments = (rentalRequest.rentalRequestAttachments || []).join(', ');

          return <Grid fluid id="rental-requests-data">
            <Row>
              <Col md={6}>
                <ColDisplay md={12} labelProps={{ md: 4 }} label="Project">{ rentalRequest.projectName }</ColDisplay>
                <ColDisplay md={12} labelProps={{ md: 4 }} label={ rentalRequest.primaryContactRole || 'Primary Contact' }>
                  <Unimplemented>
                    <Button bsStyle="link" title="Show Contact" onClick={ this.openContactDialog.bind(this, rentalRequest.primaryContact) }>
                      { concat(rentalRequest.primaryContactName, rentalRequest.primaryContactPhone, ', ') }
                    </Button>
                  </Unimplemented>
                </ColDisplay>
                <ColDisplay md={12} labelProps={{ md: 4 }} label="Local Area">{ rentalRequest.localAreaName }</ColDisplay>
                <ColDisplay md={12} labelProps={{ md: 4 }} label="Expected Hours">{ rentalRequest.expectedHours }</ColDisplay>
                <ColDisplay md={12} labelProps={{ md: 4 }} label="Expected Start Date">{  formatDateTime(rentalRequest.expectedStartDate, Constant.DATE_YEAR_SHORT_MONTH_DAY) }</ColDisplay>
                <ColDisplay md={12} labelProps={{ md: 4 }} label="Expected End Date">{ formatDateTime(rentalRequest.expectedEndDate, Constant.DATE_YEAR_SHORT_MONTH_DAY) }</ColDisplay>
              </Col>
              <Col md={6}>
                <ColDisplay md={12} labelProps={{ md: 4 }} label="Equipment Type">{ rentalRequest.equipmentTypeName }</ColDisplay>
                <ColDisplay md={12} labelProps={{ md: 4 }} label="Count">{ rentalRequest.equipmentCount }</ColDisplay>
                <ColDisplay md={12} labelProps={{ md: 4 }} label="Attachment(s)">{ numRequestAttachments > 0 ? requestAttachments : 'None' }</ColDisplay>
              </Col>
            </Row>
          </Grid>;
        })()}
      </Well>

      <Well>
        <h3>Request Status <span className="pull-right">
          <Button onClick={ this.print }><Glyphicon glyph="print" title="Print" /></Button>
          <CheckboxControl id="showHiredEquipment" inline checked={ this.state.showHiredEquipment } updateState={ this.updateState }><small>Show Hired</small></CheckboxControl>
        </span></h3>
        {(() => {
          if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

          var rotationList = rentalRequest.rentalRequestRotationList;

          if (!this.state.showHiredEquipment) {
            // Exclude equipment already hired
            rotationList = _.reject(rotationList, { isHired: true });
          }

          if (Object.keys(rotationList || []).length === 0) { return <Alert bsStyle="success" style={{ marginTop: 10 }}>No equipment</Alert>; }

          // Sort in rotation list order
          rotationList = _.sortBy(rotationList, 'rotationListSortOrder');

          var headers = [
            { field: 'seniority',            title: 'Seniority'         },
            { field: 'serviceHoursThisYear', title: 'YTD Hours'         },
            { field: 'equipmentCode',        title: 'Equipment ID'      },
            { field: 'equipmentDetails',     title: 'Equipment Details' },
            { field: 'primaryContactName',   title: 'Contact'           },
            { field: 'status',               title: 'Status'            },
          ];

          var separator = <span style={{ float: 'left'}}>{ '|' }</span>;

          return <TableControl id="rotation-list" headers={ headers }>
            {
              _.map(rotationList, (listItem) => {
                return <tr key={ listItem.id }>
                  <td>{ listItem.seniority }</td>
                  <td>{ listItem.serviceHoursThisYear }</td>
                  <td><Link to={ `${Constant.EQUIPMENT_PATHNAME}/${listItem.equipmentId}` }>{ listItem.equipmentCode }</Link></td>
                  <td>{ listItem.equipmentDetails }</td>
                  <td>
                    <Unimplemented>
                      <Button bsStyle="link" title="Show Contact" onClick={ this.openContactDialog.bind(this, listItem.contact) }>
                        { concat(listItem.contactName, listItem.contactPhone, ': ') }
                      </Button>
                    </Unimplemented>
                  </td>
                  <td>
                    <ButtonGroup>
                      <Button bsStyle="link" title="Show Offer" onClick={ this.openHireOfferDialog.bind(this, listItem) }>Offer</Button>
                      { separator }
                      {(() => {
                        // If RentalRequestRotationList.rentalAgreement is non-null - go to that Rental Agreement.
                        if (listItem.rentalAgreement && listItem.rentalAgreement.id) {
                          return <Link title="Open Rental Agreement" to={ `${Constant.RENTAL_AGREEMENTS_PATHNAME}/${listItem.rentalAgreement.id}` }>Agreement</Link>;
                        }
                        // If RentalRequestRotationList.rentalAgreement is null - go to the Create New Rental Agreement with needed information about the new agreement
                        return <Button bsStyle="link" title="Create Rental Agreement" onClick={ this.saveNewRentalAgreement.bind(this, listItem) }>Agreement</Button>;
                      })()}
                    </ButtonGroup>
                  </td>
                </tr>;
              })
            }
          </TableControl>;
        })()}
      </Well>

      <Well className="history">
        <h3>History <span className="pull-right">
        </span></h3>
        {(() => {
          if (this.state.loadingHistory) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }
          if (Object.keys(this.props.history).length === 0) { return <Alert bsStyle="success" style={{ marginTop: 10 }}>No history</Alert>; }

          var history = _.sortBy(this.props.history, 'createdDate');

          const HistoryEntry = ({ createdDate, historyText }) => (
            <Row>
              <ColDisplay md={12} labelProps={{ md: 2 }} label={ formatDateTime(createdDate, Constant.DATE_YEAR_SHORT_MONTH_DAY) }>
                { historyText }
              </ColDisplay>
            </Row>
          );

          return <div id="rental-requests-history">
            {
              _.map(history, (entry) => <HistoryEntry { ...entry } />)
            }
          </div>;
        })()}
      </Well>
      { this.state.showEditDialog &&
        <RentalRequestsEditDialog show={ this.state.showEditDialog } onSave={ this.saveEdit } onClose={ this.closeEditDialog } />
      }
      { this.state.showHireOfferDialog &&
        <HireOfferEditDialog show={ this.state.showHireOfferDialog } hireOffer={ this.state.rotationListHireOffer } onSave={ this.saveHireOffer } onClose={ this.closeHireOfferDialog } />
      }
      { /* TODO this.state.showContactDialog && <ContactEditDialog /> */}
      { this.state.showDocumentsDialog &&
        <DocumentsListDialog 
          show={ this.state.showDocumentsDialog } 
          parent={ rentalRequest } 
          onClose={ this.closeDocumentsDialog } 
        />
      }
    </div>;
  },
});


function mapStateToProps(state) {
  return {
    rentalRequest: state.models.rentalRequest,
    rentalAgreement: state.models.rentalAgreement,
    notes: state.models.rentalRequestNotes,
    attachments: state.models.rentalRequestAttachments,
    documents: state.models.documents,
    history: state.models.rentalRequestHistory,
  };
}

export default connect(mapStateToProps)(RentalRequestsDetail);
