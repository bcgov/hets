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
import Confirm from '../components/Confirm.jsx';
import OverlayTrigger from '../components/OverlayTrigger.jsx';

import { formatDateTime } from '../utils/date';
import { concat } from '../utils/string';

/*

TODO:
* Print / Notes / Docs / Contacts (TBD) / History / Request Status List / Clone / Request Attachments

*/
const STATUS_YES = 'Yes';
const STATUS_NO = 'No';
const STATUS_FORCE_HIRE = 'Force Hire';
const STATUS_ASKED = 'Asked';

var RentalRequestsDetail = React.createClass({
  propTypes: {
    rentalRequest: React.PropTypes.object,
    rentalRequestRotationList: React.PropTypes.object,
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

      showAttachmentss: false,

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
    var rentalRequestId = this.props.params.rentalRequestId;
    var rentalRequestsPromise = Api.getRentalRequest(rentalRequestId);
    var rotationListPromise = Api.getRentalRequestRotationList(rentalRequestId);
    var documentsPromise = Api.getRentalRequestDocuments(this.props.params.rentalRequestId);


    return Promise.all([rentalRequestsPromise, rotationListPromise, documentsPromise]).finally(() => {
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
    let hireOfferUpdated = { ...hireOffer };
    delete hireOfferUpdated.isFirstNullRecord;
    delete hireOfferUpdated.displayFields;
    Api.updateRentalRequestRotationList(hireOfferUpdated, this.props.rentalRequest.data).finally(() => {
      this.fetch();
      if ((hireOffer.offerResponse === STATUS_YES || hireOffer.offerResponse === STATUS_FORCE_HIRE) && hireOffer.rentalAgreement && hireOffer.rentalAgreement.id) {
        this.props.router.push({ pathname: `${Constant.RENTAL_AGREEMENTS_PATHNAME}/${this.props.rentalAgreement.id}` });
      } else if (hireOffer.offerResponse === STATUS_YES || hireOffer.offerResponse === STATUS_FORCE_HIRE) {
        this.saveNewRentalAgreement(hireOffer);
      }
      this.closeHireOfferDialog();
    });
  },

  saveNewRentalAgreement(rentalRequestRotationList) {
    var rentalRequest = this.props.rentalRequest.data;

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

  renderStatusText(listItem) {
    let text = 'Hire';
    if (listItem.offerResponse === STATUS_NO) {
      text = listItem.offerRefusalReason;
    } else if (listItem.offerResponse === STATUS_ASKED) {
      text = `${listItem.offerResponse} (${formatDateTime(listItem.askedDateTime, 'YYYY-MM-DD')})`;
    } else if (listItem.offerResponse !== null) {
      text = listItem.offerResponse;
    }
    return text;
  },

  render() {
    var rentalRequest = this.props.rentalRequest.data;
    
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
        
        return (
          <Row id="rental-requests-header">
            <ColDisplay md={12} label={ <h1>Project:</h1> }><h1><small>{ rentalRequest.projectName }</small></h1></ColDisplay>
            <ColDisplay md={12} label="Provincial Project Number:">{ rentalRequest.projectId }</ColDisplay>            
          </Row>
        );
      })()}

      <Well className="request-information">
        <h3>Request Information <span className="pull-right">
          <Button title="Edit Rental Request" bsSize="small" onClick={ this.openEditDialog }><Glyphicon glyph="pencil" /></Button>
        </span></h3>
        {(() => {
          if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }
        
          var requestAttachments = rentalRequest.rentalRequestAttachments && rentalRequest.rentalRequestAttachments[0] ? rentalRequest.rentalRequestAttachments[0].attachment : 'None';
          
          return <Grid fluid id="rental-requests-data" className="nopadding">
            <Row>
              <Col md={6}>
                <ColDisplay md={12} labelProps={{ md: 4 }} label={ rentalRequest.primaryContactRole || 'Primary Contact' }>
                  <Unimplemented>
                    <Button bsStyle="link" title="Show Contact" onClick={ this.openContactDialog.bind(this, rentalRequest.primaryContact) }>
                      { concat(rentalRequest.primaryContactName, rentalRequest.primaryContactPhone, ', ') }
                    </Button>
                  </Unimplemented>
                </ColDisplay>
                <ColDisplay md={12} labelProps={{ md: 4 }} label="Local Area">{ rentalRequest.localAreaName }</ColDisplay>
                <ColDisplay md={12} labelProps={{ md: 4 }} label="Equipment Type">{ rentalRequest.equipmentTypeName }</ColDisplay>
                <ColDisplay md={12} labelProps={{ md: 4 }} label="Count">{ rentalRequest.equipmentCount }</ColDisplay>
              </Col>
              <Col md={6}>
                <ColDisplay md={12} labelProps={{ md: 4 }} label="Attachment(s)">{ requestAttachments }</ColDisplay>
                <ColDisplay md={12} labelProps={{ md: 4 }} label="Expected Hours">{ rentalRequest.expectedHours }</ColDisplay>
                <ColDisplay md={12} labelProps={{ md: 4 }} label="Expected Start Date">{  formatDateTime(rentalRequest.expectedStartDate, Constant.DATE_YEAR_SHORT_MONTH_DAY) }</ColDisplay>
                <ColDisplay md={12} labelProps={{ md: 4 }} label="Expected End Date">{ formatDateTime(rentalRequest.expectedEndDate, Constant.DATE_YEAR_SHORT_MONTH_DAY) }</ColDisplay>
              </Col>
            </Row>
          </Grid>;
        })()}
      </Well>

      <Well>
        <h3>Request Status <span className="pull-right">
          <Button onClick={ this.print }><Glyphicon glyph="print" title="Print" /></Button>
          <CheckboxControl id="showAttachments" inline updateState={ this.updateState }><small>Show Attachments</small></CheckboxControl>
        </span></h3>
        {(() => {
          if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }
          
          var rotationList = this.props.rentalRequestRotationList.data.rentalRequestRotationList;
          
          if (Object.keys(rotationList || []).length === 0) { return <Alert bsStyle="success" style={{ marginTop: 10 }}>No equipment</Alert>; }
          
          // Sort in rotation list order
          rotationList = _.sortBy(rotationList, 'rotationListSortOrder');
          
          var headers = [
            { field: 'seniority',               title: 'Seniority'         },
            { field: 'serviceHoursThisYear',    title: 'YTD Hours'         },
            { field: 'equipmentCode',           title: 'Equipment ID'      },
            { field: 'equipmentDetails',        title: 'Equipment Details' },
            { field: 'equipmentOwner',           title: 'Owner'             },
            { field: 'primaryContactName',      title: 'Contact'           },
            { field: 'primaryContactWorkPhone', title: 'Work Phone'        },
            { field: 'primaryContactCellPhone', title: 'Cell Phone'        },
            { field: 'status',                  title: 'Status'            },
          ];
          
          var previousNullRecord = false;

          return <TableControl id="rotation-list" headers={ headers }>
            {
              _.map(rotationList, (listItem) => {
                const owner = listItem.equipment.owner;
                var isFirstNullRecord = false;
                // Set first null record to show correct response dialog link text
                if (!previousNullRecord && !listItem.offerResponse) { isFirstNullRecord = true; previousNullRecord = true; }
                return (
                  <tr key={ listItem.id }>
                    <td>{ listItem.displayFields.seniority }</td>
                    <td>{ listItem.equipment.serviceHoursLastYear }</td>
                    <td><Link to={ `${Constant.EQUIPMENT_PATHNAME}/${listItem.equipment.id}` }>{ listItem.equipment.equipmentCode }</Link></td>
                    <td>{ listItem.displayFields.equipmentDetails }
                      { this.state.showAttachments && 
                      <div>
                        Attachments:
                        { listItem.equipment.equipmentAttachments && listItem.equipment.equipmentAttachments.map((item, i) => (
                          <span key={item.id}> { item.typeName }
                          { ((i + 1) < listItem.equipment.equipmentAttachments.length) &&
                            <span>,</span>
                          }
                          </span>
                        ))}
                        { (!listItem.equipment.equipmentAttachments || listItem.equipment.equipmentAttachments.length === 0)  &&
                          <span> none</span>
                        }
                      </div>
                      }
                    </td>
                    <td>{ owner && owner.organizationName }</td>
                    <td>{ owner && owner.primaryContact && `${owner.primaryContact.givenName} ${owner.primaryContact.surname}` }</td>
                    <td>{ owner && owner.primaryContact && owner.primaryContact.workPhoneNumber }</td>
                    <td>{ owner && owner.primaryContact && owner.primaryContact.mobilePhoneNumber }</td>
                    <td>
                      <ButtonGroup>
                        {(() => {
                          listItem.isFirstNullRecord = isFirstNullRecord;
                          if (listItem.maximumHours) {
                            return (
                              <OverlayTrigger 
                                trigger="click" 
                                placement="top" 
                                title="This piece of equipment is has met or exceeded its Maximum Allowed Hours for this year. Are you sure you want to edit the Offer on this equipment?"
                                rootClose 
                                overlay={ <Confirm onConfirm={ this.openHireOfferDialog.bind(this, listItem) }/> }
                              >
                                <Button 
                                  bsStyle="link"
                                  bsSize="xsmall"
                                >
                                  Max. hours reached
                                </Button>
                              </OverlayTrigger>
                            );
                          }

                          return (
                            <Button 
                            bsStyle="link" 
                              title="Show Offer" 
                              onClick={ this.openHireOfferDialog.bind(this, listItem) }
                            >
                              { this.renderStatusText(listItem, isFirstNullRecord) }
                            </Button>
                          );
                        })()}
                      </ButtonGroup>
                    </td>
                  </tr>
                );
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
    rentalRequestRotationList: state.models.rentalRequestRotationList,
    rentalAgreement: state.models.rentalAgreement,
    notes: state.models.rentalRequestNotes,
    attachments: state.models.rentalRequestAttachments,
    documents: state.models.documents,
    history: state.models.rentalRequestHistory,
  };
}

export default connect(mapStateToProps)(RentalRequestsDetail);
