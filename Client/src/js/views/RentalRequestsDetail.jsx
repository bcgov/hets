import PropTypes from 'prop-types';
import React from 'react';
import classNames from 'classnames';
import { connect } from 'react-redux';
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
import ReturnButton from '../components/ReturnButton.jsx';
import SubHeader from '../components/ui/SubHeader.jsx';

import { formatDateTime, formatDateTimeUTCToLocal } from '../utils/date';
import { concat } from '../utils/string';
import PrintButton from '../components/PrintButton.jsx';
import { activeRentalRequestSelector, activeRentalRequestIdSelector } from '../selectors/ui-selectors.js';

/*

TODO:
* Print / Notes / Docs / Contacts (TBD) / History / Request Status List / Clone / Request Attachments

*/
const STATUS_YES = 'Yes';
const STATUS_NO = 'No';
const STATUS_FORCE_HIRE = 'Force Hire';
const STATUS_ASKED = 'Asked';
const STATUS_IN_PROGRESS = 'In Progress';

class RentalRequestsDetail extends React.Component {
  static propTypes = {
    rentalRequestId: PropTypes.number,
    rentalRequest: PropTypes.object,
    documents: PropTypes.object,
    ui: PropTypes.object,
    router: PropTypes.object,
  };

  constructor(props) {
    super(props);

    this.state = {
      loading: true,
      loadingDocuments: false,
      reloading: false,

      showEditDialog: false,
      showHireOfferDialog: false,
      showNotesDialog: false,
      showAttachments: false,

      rotationListHireOffer: {},
      showAllResponseFields: false,

      isNew: props.rentalRequestId == 0,
    };
  }

  componentDidMount() {
    const { rentalRequestId, rentalRequest } = this.props;

    /* Documents need be fetched every time as they are not rentalRequest specific in the store ATM */
    Api.getRentalRequestDocuments(rentalRequestId).then(() => this.setState({ loadingDocuments: false }));

    // Only show loading spinner if there is no existing rental request in the store
    if (rentalRequest) {
      this.setState({ loading: false });
    }

    // Re-fetch rental request, rotationlist, and notes every time
    Promise.all([
      this.fetch(),
      Api.getRentalRequestNotes(rentalRequestId),
      Api.getRentalRequestRotationList(rentalRequestId),
    ]).then(() => {
      this.setState({ loading: false });
    });
  }

  fetch = () => {
    this.setState({ reloading: true });
    return Api.getRentalRequest(this.props.rentalRequestId).then(() => this.setState({ reloading: false }));
  };

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  updateContactsUIState = (state, callback) => {
    this.setState({ ui: { ...this.state.ui, ...state }}, () => {
      store.dispatch({ type: Action.UPDATE_PROJECT_CONTACTS_UI, projectContacts: this.state.ui });
      if (callback) { callback(); }
    });
  };

  showNotes = () => {
    this.setState({ showNotesDialog: true });
  };

  closeNotesDialog = () => {
    this.setState({ showNotesDialog: false });
  };

  showDocuments = () => {
    this.setState({ showDocumentsDialog: true });
  };

  closeDocumentsDialog = () => {
    this.setState({ showDocumentsDialog: false });
  };

  addDocument = () => {

  };

  openEditDialog = () => {
    this.setState({ showEditDialog: true });
  };

  closeEditDialog = () => {
    this.setState({ showEditDialog: false });
  };

  rentalRequestSaved = () => {
    Promise.all([
      this.fetch(),
      Api.getRentalRequestRotationList(this.props.rentalRequestId),
    ]);
  };

  openHireOfferDialog = (hireOffer, showAllResponseFields) => {
    this.setState({
      rotationListHireOffer: hireOffer,
      showAllResponseFields,
      showHireOfferDialog: true,
    });
  };

  closeHireOfferDialog = () => {
    this.setState({ showHireOfferDialog: false });
  };

  hireOfferSaved = (hireOffer) => {
    Log.rentalRequestEquipmentHired(this.props.rentalRequest, hireOffer.equipment, hireOffer.offerResponse);

    this.closeHireOfferDialog();

    var rotationListItem = _.find(this.props.rentalRequest.rotationList, i => i.id === hireOffer.id);
    if (rotationListItem && rotationListItem.rentalAgreementId && !hireOffer.rentalAgreementId) {
      // navigate to rental agreement if it was newly generated
      this.props.router.push({ pathname: `${ Constant.RENTAL_AGREEMENTS_PATHNAME }/${ rotationListItem.rentalAgreementId }` });
    } else {
      // close popup dialog and refresh page data
      this.fetch();
    }
  };

  printSeniorityList = () => {
    var localAreaIds = [ this.props.rentalRequest.localAreaId ];
    var districtEquipmentTypeIds = [ this.props.rentalRequest.districtEquipmentTypeId ];
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
  };

  addRequest = () => {

  };

  renderStatusText = (listItem) => {
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
  };

  render() {
    const { loading, loadingDocuments } = this.state;
    var rentalRequest = this.props.rentalRequest || {};

    var viewOnly = !rentalRequest.projectId;
    var canEditRequest = !viewOnly && rentalRequest.status !== Constant.RENTAL_REQUEST_STATUS_CODE_COMPLETED;

    return <div id="rental-requests-detail" className={ classNames({ 'view-only': viewOnly }) }>
      <div id="watermark" className="visible-print">
        View Only
        <br />
        Not for Hiring
      </div>
      <PageOrientation type="landscape"/>
      <Row id="rental-requests-top" className="hidden-print">
        <Col sm={9}>
          <div id="rental-request-status">
            <Label bsStyle={ rentalRequest.isActive ? 'success' : rentalRequest.isCancelled ? 'danger' : 'default' }>{ rentalRequest.status }</Label>
          </div>
          <Button title="Notes" disabled={loading} onClick={ this.showNotes }>
            Notes ({ loading ? ' ' : rentalRequest.notes.length })
          </Button>
          <Button id="project-documents-button" title="Documents" disabled={loading} onClick={ this.showDocuments }>
            Documents ({ loadingDocuments ? ' ' :  Object.keys(this.props.documents).length })
          </Button>
        </Col>
        <Col sm={3}>
          <div className="pull-right">
            <ReturnButton/>
          </div>
        </Col>
      </Row>

      <Well className="request-information">
        <SubHeader title="Request Information" className="hidden-print" editButtonTitle="Edit Rental Request" onEditClicked={canEditRequest ? this.openEditDialog : null}/>
        <SubHeader title="Hire Rotation List" className="visible-print text-center"></SubHeader>
        {(() => {
          if (loading) { return <div className="spinner-container"><Spinner/></div>; }

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
        <SubHeader title="Hire Rotation List" className="hidden-print">
          <PrintButton title="Print Hire Rotation List" disabled={ loading } disabledTooltip="Please wait for the request information to finish loading.">
            Hire Rotation List
          </PrintButton>
          <TooltipButton onClick={ this.printSeniorityList } disabled={ loading } disabledTooltip="Please wait for the request information to finish loading.">
            <Glyphicon glyph="print" title="Print Seniority List" className="mr-5" />
            <span>Seniority List</span>
          </TooltipButton>
          <CheckboxControl id="showAttachments" inline updateState={ this.updateState }><small>Show Attachments</small></CheckboxControl>
        </SubHeader>
        {(() => {
          if (loading) { return <div className="spinner-container"><Spinner/></div>; }

          var rotationList = this.props.rentalRequest.rotationList;

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
            { field: 'primaryContactWorkPhone', node: <div><span>Phone</span></div> },
            { field: 'primaryContactCellPhone', node: <div><span>Cell</span> <span>Phone</span></div> },
            { field: 'status',                  title: 'Status'            },
            { field: '',                        title: 'Comments'          },
          ];

          var numberEquipmentAvailableForNormalHire = rentalRequest.equipmentCount - rentalRequest.yesCount;

          return <TableControl id="rotation-list" headers={ headers }>
            {
              _.map(rotationList, (listItem, i) => {
                const owner = listItem.equipment.owner;
                var showAllResponseFields = false;
                if ((numberEquipmentAvailableForNormalHire > 0) && (listItem.offerResponse === STATUS_ASKED || !listItem.offerResponse) && (rentalRequest.yesCount < rentalRequest.equipmentCount)) {
                  showAllResponseFields = true;
                  numberEquipmentAvailableForNormalHire -= 1;
                }

                const showBlankLine = i > 0 && rotationList[i - 1].equipment.blockNumber !== listItem.equipment.blockNumber;

                return [
                  showBlankLine && <tr key={listItem.equipment.blockNumber} className="blank-row"><td colSpan="14">&nbsp;</td></tr>,
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
                          if (!viewOnly && rentalRequest.status === STATUS_IN_PROGRESS && (listItem.offerResponse === STATUS_ASKED || !listItem.offerResponse)) {
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
                  </tr>,
                ];
              })
            }
          </TableControl>;
        })()}
      </Well>

      <Well className="history">
        <SubHeader title="History"/>
        { rentalRequest.historyEntity && <History historyEntity={ rentalRequest.historyEntity } refresh={ !this.state.reloading }/> }
      </Well>
      { this.state.showEditDialog && (
        <RentalRequestsEditDialog
          show={this.state.showEditDialog}
          rentalRequest={this.props.rentalRequest}
          onSave={this.rentalRequestSaved}
          onClose={this.closeEditDialog}/>
      )}
      { this.state.showHireOfferDialog && (
        <HireOfferEditDialog
          show={this.state.showHireOfferDialog}
          hireOffer={this.state.rotationListHireOffer}
          rentalRequest={this.props.rentalRequest}
          showAllResponseFields={this.state.showAllResponseFields}
          onSave={this.hireOfferSaved}
          onClose={this.closeHireOfferDialog}/>
      )}
      { this.state.showDocumentsDialog && (
        <DocumentsListDialog
          show={this.state.showDocumentsDialog}
          parent={rentalRequest}
          onClose={this.closeDocumentsDialog}/>
      )}
      { this.state.showNotesDialog &&
        <NotesDialog
          id={String(this.props.rentalRequestId)}
          show={this.state.showNotesDialog}
          notes={this.props.rentalRequest.notes}
          getNotes={Api.getRentalRequestNotes}
          saveNote={Api.addRentalRequestNote}
          onClose={this.closeNotesDialog}/>
      }
    </div>;
  }
}


function mapStateToProps(state) {
  return {
    rentalRequest: activeRentalRequestSelector(state),
    rentalRequestId: activeRentalRequestIdSelector(state),
    documents: state.models.documents,
  };
}

export default connect(mapStateToProps)(RentalRequestsDetail);
