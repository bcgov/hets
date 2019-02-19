import React from 'react';

import { connect } from 'react-redux';

import { browserHistory } from 'react-router';

import { Well, Row, Col, Alert, Button, ButtonGroup, Glyphicon, Label } from 'react-bootstrap';
import { Link } from 'react-router';

import _ from 'lodash';
import Promise from 'bluebird';

import Moment from 'moment';

import HireOfferEditDialog from './dialogs/HireOfferEditDialog.jsx';
import RentalRequestsEditDialog from './dialogs/RentalRequestsEditDialog.jsx';
import DocumentsListDialog from './dialogs/DocumentsListDialog.jsx';
import NotesDialog from './dialogs/NotesDialog.jsx';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import * as Log from '../history';
import store from '../store';

import CheckboxControl from '../components/CheckboxControl.jsx';
import ColDisplay from '../components/ColDisplay.jsx';
import PageOrientation from '../components/PageOrientation.jsx';
import Spinner from '../components/Spinner.jsx';
import TableControl from '../components/TableControl.jsx';
import Confirm from '../components/Confirm.jsx';
import History from '../components/History.jsx';
import OverlayTrigger from '../components/OverlayTrigger.jsx';
import TooltipButton from '../components/TooltipButton.jsx';

import SubHeader from '../components/ui/SubHeader.jsx';

import { formatDateTime, formatDateTimeUTCToLocal } from '../utils/date';
import { concat } from '../utils/string';

/*

TODO:
* Print / Notes / Docs / Contacts (TBD) / History / Request Status List / Clone / Request Attachments

*/
const STATUS_YES = 'Yes';
const STATUS_NO = 'No';
const STATUS_FORCE_HIRE = 'Force Hire';
const STATUS_ASKED = 'Asked';
const STATUS_IN_PROGRESS = 'In Progress';

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
      showNotesDialog: false,

      showAttachmentss: false,

      rotationListHireOffer: {},
      showAllResponseFields: false,

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
    var documentsPromise = Api.getRentalRequestDocuments(rentalRequestId);
    var rentalRequestNotesPromise = Api.getRentalRequestNotes(rentalRequestId);


    return Promise.all([rentalRequestsPromise, rotationListPromise, documentsPromise, rentalRequestNotesPromise]).finally(() => {
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
    this.setState({ showNotesDialog: true });
  },

  closeNotesDialog() {
    this.setState({ showNotesDialog: false });
  },

  showDocuments() {
    this.setState({ showDocumentsDialog: true });
  },

  closeDocumentsDialog() {
    this.setState({ showDocumentsDialog: false });
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
      Log.rentalRequestModified(this.props.rentalRequest.data);
      this.closeEditDialog();
      Api.getRentalRequestRotationList(this.props.params.rentalRequestId);
    });
  },

  openHireOfferDialog(hireOffer, showAllResponseFields) {
    this.setState({
      rotationListHireOffer: hireOffer,
      showAllResponseFields,
      showHireOfferDialog: true,
    });
  },

  closeHireOfferDialog() {
    this.setState({ showHireOfferDialog: false });
  },

  saveHireOffer(hireOffer) {
    let hireOfferUpdated = { ...hireOffer };
    delete hireOfferUpdated.showAllResponseFields;
    delete hireOfferUpdated.displayFields;
    delete hireOfferUpdated.rentalAgreement;

    Api.updateRentalRequestRotationList(hireOfferUpdated, this.props.rentalRequest.data).then(() => {
      Log.rentalRequestEquipmentHired(this.props.rentalRequest.data, hireOffer.equipment, hireOffer.offerResponse);

      var rotationListItem = _.find(this.props.rentalRequestRotationList.data, i => i.id === hireOffer.id);
      if (rotationListItem && rotationListItem.rentalAgreementId && !hireOfferUpdated.rentalAgreementId) {
        // navigate to rental agreement if it was newly generated
        this.props.router.push({ pathname: `${ Constant.RENTAL_AGREEMENTS_PATHNAME }/${ rotationListItem.rentalAgreementId }` });
      } else {
        // close popup dialog and refresh page data
        this.closeHireOfferDialog();
        this.fetch();
      }
    });
  },

  print() {
    window.print();
  },

  printSeniorityList() {
    var localAreaIds = [ this.props.rentalRequest.data.localAreaId ];
    var districtEquipmentTypeIds = [ this.props.rentalRequest.data.districtEquipmentTypeId ];
    Api.equipmentSeniorityListPdf(localAreaIds, districtEquipmentTypeIds).then(response => {
      var filename = 'SeniorityList-' + formatDateTimeUTCToLocal(new Date(), Constant.DATE_TIME_FILENAME) + '.pdf';

      var blob = new Blob([response], {type: 'image/pdf'});
      if (window.navigator.msSaveBlob) {
        blob = window.navigator.msSaveBlob([response], filename);
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
  },

  addRequest() {

  },

  renderStatusText(listItem) {
    let text = 'Hire';
    if (listItem.offerResponse === STATUS_NO && listItem.offerRefusalReason == Constant.HIRING_REFUSAL_OTHER) {
      text = listItem.offerResponseNote;
    } else if (listItem.offerResponse == STATUS_NO) {
      text = listItem.offerRefusalReason;
    } else if (listItem.offerResponse === STATUS_ASKED) {
      text = `${listItem.offerResponse} (${Moment(listItem.askedDateTime).format('YYYY-MM-DD hh:mm A')})`;
    } else if (listItem.offerResponse === STATUS_FORCE_HIRE || listItem.offerResponse === STATUS_YES) {
      text = listItem.offerResponse;
    }
    return text;
  },

  render() {
    var rentalRequest = this.props.rentalRequest.data;

    var canEditRequest = rentalRequest.projectId > 0 && rentalRequest.status !== Constant.RENTAL_REQUEST_STATUS_CODE_COMPLETED;

    return <div id="rental-requests-detail">
      <PageOrientation type="landscape"/>
      <Row id="rental-requests-top">
        <Col sm={10}>
          <Label bsStyle={ rentalRequest.isActive ? 'success' : rentalRequest.isCancelled ? 'danger' : 'default' }>{ rentalRequest.status }</Label>
          <Button title="Notes" onClick={ this.showNotes }>Notes ({ Object.keys(this.props.notes).length })</Button>
          <Button title="Documents" onClick={ this.showDocuments }>Documents ({ Object.keys(this.props.documents).length })</Button>
        </Col>
        <Col sm={2}>
          <div className="pull-right">
            <Button title="Return" onClick={ browserHistory.goBack }><Glyphicon glyph="arrow-left" /> Return</Button>
          </div>
        </Col>
      </Row>

      <Well className="request-information">
        <SubHeader title="Request Information" editButtonTitle="Edit Rental Request" onEditClicked={canEditRequest ? this.openEditDialog : null}/>
        {(() => {
          if (this.state.loading) { return <div className="spinner-container"><Spinner/></div>; }

          var requestAttachments = rentalRequest.rentalRequestAttachments && rentalRequest.rentalRequestAttachments[0] ? rentalRequest.rentalRequestAttachments[0].attachment : 'None';

          return <Row id="rental-requests-data" className="equal-height">
            <Col lg={6} md={6} sm={6} xs={12}>
              <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Local Area">{ rentalRequest.localAreaName }</ColDisplay>
            </Col>
            <Col lg={6} md={6} sm={6} xs={12}>
              <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label={ rentalRequest.projectPrimaryContactRole || 'Primary Contact' }>
                { concat(rentalRequest.projectPrimaryContactName, rentalRequest.projectPrimaryContactPhone, ', ') }
              </ColDisplay>
            </Col>
            <Col lg={6} md={6} sm={6} xs={12}>
              <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Equipment Type">{ rentalRequest.equipmentTypeName }</ColDisplay>
            </Col>
            <Col lg={6} md={6} sm={6} xs={12}>
              <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Quantity">{ rentalRequest.equipmentCount }</ColDisplay>
            </Col>
            <Col lg={6} md={6} sm={6} xs={12}>
              <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Attachment(s)">{ requestAttachments }</ColDisplay>
            </Col>
            <Col lg={6} md={6} sm={6} xs={12}>
              <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Expected Hours">{ rentalRequest.expectedHours }</ColDisplay>
            </Col>
            <Col lg={6} md={6} sm={6} xs={12}>
              <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Project"><strong>{ rentalRequest.project && rentalRequest.project.name }</strong></ColDisplay>
            </Col>
            <Col lg={6} md={6} sm={6} xs={12}>
              <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Expected Start Date">{  formatDateTime(rentalRequest.expectedStartDate, Constant.DATE_YEAR_SHORT_MONTH_DAY) }</ColDisplay>
            </Col>
            <Col lg={6} md={6} sm={6} xs={12}>
              <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Provincial Project Number"><strong>{ rentalRequest.project && rentalRequest.project.provincialProjectNumber }</strong></ColDisplay>
            </Col>
            <Col lg={6} md={6} sm={6} xs={12}>
              <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Expected End Date">{ formatDateTime(rentalRequest.expectedEndDate, Constant.DATE_YEAR_SHORT_MONTH_DAY) }</ColDisplay>
            </Col>
          </Row>;
        })()}
      </Well>

      <Well>
        <SubHeader title="Hire Rotation List">
          <TooltipButton onClick={ this.print } disabled={ this.state.loading } disabledTooltip="Please wait for the request information to finish loading.">
            <Glyphicon glyph="print" title="Print Hire Rotation List" className="mr-5" />
            <span>Hire Rotation List</span>
          </TooltipButton>
          <TooltipButton onClick={ this.printSeniorityList } disabled={ this.state.loading } disabledTooltip="Please wait for the request information to finish loading.">
            <Glyphicon glyph="print" title="Print Seniority List" className="mr-5" />
            <span>Seniority List</span>
          </TooltipButton>
          <CheckboxControl id="showAttachments" inline updateState={ this.updateState }><small>Show Attachments</small></CheckboxControl>
        </SubHeader>
        {(() => {
          if (this.state.loading) { return <div className="spinner-container"><Spinner/></div>; }

          var rotationList = this.props.rentalRequestRotationList.data.rentalRequestRotationList;

          if (Object.keys(rotationList || []).length === 0) { return <Alert bsStyle="success">No equipment</Alert>; }

          // Sort in rotation list order
          rotationList = _.sortBy(rotationList, 'rotationListSortOrder');

          // use spans for table headers so we can force them to wrap when printing
          var headers = [
            { field: 'seniorityString',         title: 'Seniority'         },
            { field: 'block',                   title: 'Blk'               },
            { field: 'serviceHoursThisYear',    node: <div><span>YTD</span> <span>Hours</span></div> },
            { field: 'equipmentCode',           node: <div><span>Equip.</span> <span>ID</span></div> },
            { field: 'equipmentDetails',        node: <div><span>Equip.</span> <span>Details</span></div> },
            { field: 'equipmentOwner',          title: 'Owner'             },
            { field: 'primaryContactName',      title: 'Contact'           },
            { field: 'primaryContactWorkPhone', node: <div><span>Work</span> <span>Phone</span></div> },
            { field: 'primaryContactCellPhone', node: <div><span>Cell</span> <span>Phone</span></div> },
            { field: 'status',                  title: 'Status'            },
            { field: '',                        title: 'Comments'          },
          ];

          var numberEquipmentAvailableForNormalHire = rentalRequest.equipmentCount - rentalRequest.yesCount;

          return <TableControl id="rotation-list" headers={ headers }>
            {
              _.map(rotationList, (listItem) => {
                const owner = listItem.equipment.owner;
                var showAllResponseFields = false;
                if ((numberEquipmentAvailableForNormalHire > 0) && (listItem.offerResponse === STATUS_ASKED || !listItem.offerResponse) && (rentalRequest.yesCount < rentalRequest.equipmentCount)) {
                  showAllResponseFields = true;
                  numberEquipmentAvailableForNormalHire -= 1;
                }
                return (
                  <tr key={ listItem.id }>
                    <td>{ listItem.equipment.seniorityString }</td>
                    <td>{ listItem.equipment.blockNumber }</td>
                    <td>{ listItem.equipment.hoursYtd }</td>
                    <td><Link to={ `${Constant.EQUIPMENT_PATHNAME}/${listItem.equipment.id}` }>{ listItem.equipment.equipmentCode }</Link></td>
                    <td>{ listItem.equipment.equipmentDetails }
                      { this.state.showAttachments &&
                      <div>
                        Attachments:
                        { listItem.equipment.equipmentAttachments && listItem.equipment.equipmentAttachments.map((item, i) => (
                          <span key={item.id}>
                            <span> </span>
                            <span className="attachment">{ item.typeName }
                              { ((i + 1) < listItem.equipment.equipmentAttachments.length) &&
                              <span>,</span>
                              }
                            </span>
                          </span>
                        ))}
                        { (!listItem.equipment.equipmentAttachments || listItem.equipment.equipmentAttachments.length === 0)  &&
                          <span> none</span>
                        }
                      </div>
                      }
                    </td>
                    <td>{ owner && owner.organizationName }</td>
                    <td>{ listItem.displayFields.primaryContactName }</td>
                    <td>{ owner && owner.primaryContact && owner.primaryContact.workPhoneNumber }</td>
                    <td>{ owner && owner.primaryContact && owner.primaryContact.mobilePhoneNumber }</td>
                    <td>
                      <ButtonGroup>
                        {(() => {
                          const changeOfferWarningMessage = 'This piece of equipment is has met or ' +
                            'exceeded its Maximum Allowed Hours for this year. Are you sure you want ' +
                            'to edit the Offer on this equipment?';

                          const confirm = (
                            <Confirm
                              title={changeOfferWarningMessage}
                              onConfirm={ () => this.openHireOfferDialog(listItem, showAllResponseFields) }
                            />
                          );

                          if (listItem.maximumHours) {
                            return (
                              <OverlayTrigger
                                trigger="click"
                                placement="top"
                                rootClose
                                overlay={ confirm }>
                                <Button bsStyle="link" bsSize="xsmall">
                                  Max. hours reached
                                </Button>
                              </OverlayTrigger>
                            );
                          }
                          if (rentalRequest.projectId > 0 && rentalRequest.status === STATUS_IN_PROGRESS && (listItem.offerResponse === STATUS_ASKED || !listItem.offerResponse)) {
                            return (
                              <Button
                                bsStyle="link"
                                title="Show Offer"
                                onClick={ () => this.openHireOfferDialog(listItem, showAllResponseFields) }>
                                { this.renderStatusText(listItem) }
                              </Button>
                            );
                          }
                          return this.renderStatusText(listItem);
                        })()}
                      </ButtonGroup>
                    </td>
                    <td></td>
                  </tr>
                );
              })
            }
          </TableControl>;
        })()}
      </Well>

      <Well className="history">
        <SubHeader title="History"/>
        { rentalRequest.historyEntity &&
          <History historyEntity={ rentalRequest.historyEntity } refresh={ !this.state.loading } />
        }
      </Well>
      { this.state.showEditDialog &&
        <RentalRequestsEditDialog
          show={ this.state.showEditDialog }
          onSave={ this.saveEdit }
          onClose={ this.closeEditDialog }
        />
      }
      { this.state.showHireOfferDialog &&
        <HireOfferEditDialog
          show={ this.state.showHireOfferDialog }
          hireOffer={ this.state.rotationListHireOffer }
          showAllResponseFields={this.state.showAllResponseFields}
          onSave={ this.saveHireOffer }
          onClose={ this.closeHireOfferDialog }
          error={ this.props.rentalRequestRotationList.error }
        />
      }
      { this.state.showDocumentsDialog &&
        <DocumentsListDialog
          show={ this.state.showDocumentsDialog }
          parent={ rentalRequest }
          onClose={ this.closeDocumentsDialog }
        />
      }
      { this.state.showNotesDialog &&
        <NotesDialog
          show={ this.state.showNotesDialog }
          onSave={ Api.addRentalRequestNote }
          id={ this.props.params.rentalRequestId }
          getNotes={ Api.getRentalRequestNotes }
          onUpdate={ Api.updateNote }
          onClose={ this.closeNotesDialog }
          notes={ this.props.notes }
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
