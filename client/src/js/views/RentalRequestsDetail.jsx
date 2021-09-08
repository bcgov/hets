import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { Row, Col, Alert, Button, ButtonGroup, Badge, OverlayTrigger } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Link } from 'react-router-dom';
import _ from 'lodash';
import Moment from 'moment';
import { saveAs } from 'file-saver';

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
import TooltipButton from '../components/TooltipButton.jsx';
import ReturnButton from '../components/ReturnButton.jsx';
import SubHeader from '../components/ui/SubHeader.jsx';
import Watermark from '../components/Watermark.jsx';

import { formatDateTime, formatDateTimeUTCToLocal } from '../utils/date';
import { concat } from '../utils/string';
import PrintButton from '../components/PrintButton.jsx';
import { activeRentalRequestSelector } from '../selectors/ui-selectors.js';

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
    rentalRequest: PropTypes.object,
    documents: PropTypes.object,
    ui: PropTypes.object,
    history: PropTypes.object,
    match: PropTypes.object,
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

      isNew: props.match.params.rentalRequestId === 0,
    };
  }

  componentDidMount() {
    store.dispatch({
      type: Action.SET_ACTIVE_RENTAL_REQUEST_ID_UI,
      rentalRequestId: this.props.match.params.rentalRequestId,
    });

    const rentalRequestId = this.props.match.params.rentalRequestId;

    /* Documents need be fetched every time as they are not rentalRequest specific in the store ATM */
    Api.getRentalRequestDocuments(rentalRequestId).then(() => this.setState({ loadingDocuments: false }));

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
    return Api.getRentalRequest(this.props.match.params.rentalRequestId).then(() =>
      this.setState({ reloading: false })
    );
  };

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  updateContactsUIState = (state, callback) => {
    this.setState({ ui: { ...this.state.ui, ...state } }, () => {
      store.dispatch({
        type: Action.UPDATE_PROJECT_CONTACTS_UI,
        projectContacts: this.state.ui,
      });
      if (callback) {
        callback();
      }
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

  addDocument = () => {};

  openEditDialog = () => {
    this.setState({ showEditDialog: true });
  };

  closeEditDialog = () => {
    this.setState({ showEditDialog: false });
  };

  rentalRequestSaved = () => {
    Promise.all([this.fetch(), Api.getRentalRequestRotationList(this.props.match.params.rentalRequestId)]);
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

    var rotationListItem = _.find(this.props.rentalRequest.rotationList, (i) => i.id === hireOffer.id);
    if (rotationListItem && rotationListItem.rentalAgreementId && !hireOffer.rentalAgreementId) {
      // navigate to rental agreement if it was newly generated
      this.props.history.push(`${Constant.RENTAL_AGREEMENTS_PATHNAME}/${rotationListItem.rentalAgreementId}`);
    } else {
      // close popup dialog and refresh page data
      this.fetch();
    }
  };

  printSeniorityList = () => {
    let filename = 'SeniorityList-' + formatDateTimeUTCToLocal(new Date(), Constant.DATE_TIME_FILENAME) + '.docx';
    Api.rentalRequestSeniorityList(this.props.rentalRequest.id)
      .then((res) => {
        saveAs(res, filename);
      })
      .catch((error) => {
        console.log(error);
      });
  };

  addRequest = () => {};

  renderStatusText = (listItem) => {
    let text = 'Hire';
    if (listItem.offerResponse === STATUS_NO && listItem.offerRefusalReason === Constant.HIRING_REFUSAL_OTHER) {
      text = listItem.offerResponseNote;
    } else if (listItem.offerResponse === STATUS_NO) {
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

    return (
      <div id="rental-requests-detail">
        <Watermark enable={viewOnly} />
        <PageOrientation type="landscape" />
        <Row id="rental-requests-top" className="d-print-none">
          <Col sm={9}>
            <div id="rental-request-status">
              <Badge variant={rentalRequest.isActive ? 'success' : rentalRequest.isCancelled ? 'danger' : 'primary'}>
                {rentalRequest.status}
              </Badge>
            </div>
            <Button className="btn-custom" title="Notes" disabled={loading} onClick={this.showNotes}>
              Notes ({loading ? ' ' : rentalRequest.notes?.length})
            </Button>
            <Button
              className="btn-custom"
              id="project-documents-button"
              title="Documents"
              disabled={loading}
              onClick={this.showDocuments}
            >
              Documents ({loadingDocuments ? ' ' : Object.keys(this.props.documents).length})
            </Button>
          </Col>
          <Col sm={3}>
            <div className="float-right">
              <ReturnButton />
            </div>
          </Col>
        </Row>

        <div className="well request-information">
          <SubHeader
            title="Request Information"
            className="d-print-none"
            editButtonTitle="Edit Rental Request"
            onEditClicked={canEditRequest ? this.openEditDialog : null}
          />
          <SubHeader title="Hire Rotation List" className="visible-print text-center"></SubHeader>
          {(() => {
            if (loading) {
              return (
                <div className="spinner-container">
                  <Spinner />
                </div>
              );
            }

            var requestAttachments =
              rentalRequest.rentalRequestAttachments && rentalRequest.rentalRequestAttachments[0]
                ? rentalRequest.rentalRequestAttachments[0].attachment
                : 'None';

            return (
              <Row id="rental-requests-data" className="equal-height">
                <Col lg={6} md={6} sm={6} xs={12}>
                  <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Local Area">
                    {rentalRequest.localAreaName}
                  </ColDisplay>
                </Col>
                <Col lg={6} md={6} sm={6} xs={12}>
                  <ColDisplay
                    labelProps={{ xs: 4 }}
                    fieldProps={{ xs: 8 }}
                    label={rentalRequest.projectPrimaryContactRole || 'Primary Contact'}
                  >
                    {concat(rentalRequest.projectPrimaryContactName, rentalRequest.projectPrimaryContactPhone, ', ')}
                  </ColDisplay>
                </Col>
                <Col lg={6} md={6} sm={6} xs={12}>
                  <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Equipment Type">
                    {rentalRequest.equipmentTypeName}
                  </ColDisplay>
                </Col>
                <Col lg={6} md={6} sm={6} xs={12}>
                  <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Quantity">
                    {rentalRequest.equipmentCount}
                  </ColDisplay>
                </Col>
                <Col lg={6} md={6} sm={6} xs={12}>
                  <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Attachment(s)">
                    {requestAttachments}
                  </ColDisplay>
                </Col>
                <Col lg={6} md={6} sm={6} xs={12}>
                  <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Expected Hours">
                    {rentalRequest.expectedHours}
                  </ColDisplay>
                </Col>
                <Col lg={6} md={6} sm={6} xs={12}>
                  <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Project">
                    <strong>{rentalRequest.project && rentalRequest.project.name}</strong>
                  </ColDisplay>
                </Col>
                <Col lg={6} md={6} sm={6} xs={12}>
                  <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Expected Start Date">
                    {formatDateTime(rentalRequest.expectedStartDate, Constant.DATE_YEAR_SHORT_MONTH_DAY)}
                  </ColDisplay>
                </Col>
                <Col lg={6} md={6} sm={6} xs={12}>
                  <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Provincial Project Number">
                    <strong>{rentalRequest.project && rentalRequest.project.provincialProjectNumber}</strong>
                  </ColDisplay>
                </Col>
                <Col lg={6} md={6} sm={6} xs={12}>
                  <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Expected End Date">
                    {formatDateTime(rentalRequest.expectedEndDate, Constant.DATE_YEAR_SHORT_MONTH_DAY)}
                  </ColDisplay>
                </Col>
              </Row>
            );
          })()}
        </div>

        <div className="well">
          <SubHeader title="Hire Rotation List" className="d-print-none">
            <div className="d-flex align-items-baseline">
              <PrintButton
                title="Print Hire Rotation List"
                disabled={loading}
                disabledTooltip="Please wait for the request information to finish loading."
              >
                Hire Rotation List
              </PrintButton>
              <TooltipButton
                onClick={this.printSeniorityList}
                disabled={loading}
                disabledTooltip="Please wait for the request information to finish loading."
              >
                <FontAwesomeIcon icon="print" title="Print Seniority List" className="mr-1" />
                <span>Seniority List</span>
              </TooltipButton>
              <CheckboxControl
                id="showAttachments"
                inline
                updateState={this.updateState}
                label={<small>Show Attachments</small>}
              />
            </div>
          </SubHeader>
          {(() => {
            if (loading) {
              return (
                <div className="spinner-container">
                  <Spinner />
                </div>
              );
            }

            var rotationList = this.props.rentalRequest?.rotationList;

            if (Object.keys(rotationList || []).length === 0) {
              return <Alert variant="success">No equipment</Alert>;
            }

            // Sort in rotation list order
            rotationList = _.sortBy(rotationList, 'rotationListSortOrder');

            // use spans for table headers so we can force them to wrap when printing
            var headers = [
              { field: 'seniorityString', title: 'Seniority' },
              { field: 'block', title: 'Blk' },
              {
                field: 'serviceHoursThisYear',
                node: (
                  <div>
                    <span>YTD</span> <span>Hours</span>
                  </div>
                ),
              },
              {
                field: 'equipmentCode',
                node: (
                  <div>
                    <span>Equip.</span> <span>ID</span>
                  </div>
                ),
              },
              {
                field: 'equipmentDetails',
                node: (
                  <div>
                    <span>Equip.</span> <span>Details</span>
                  </div>
                ),
              },
              { field: 'equipmentOwner', title: 'Owner' },
              { field: 'primaryContactName', title: 'Contact' },
              {
                field: 'primaryContactWorkPhone',
                node: (
                  <div>
                    <span>Phone</span>
                  </div>
                ),
              },
              {
                field: 'primaryContactCellPhone',
                node: (
                  <div>
                    <span>Cell</span> <span>Phone</span>
                  </div>
                ),
              },
              { field: 'status', title: 'Status' },
              { field: '', title: 'Comments' },
            ];

            var numberEquipmentAvailableForNormalHire = rentalRequest.equipmentCount - rentalRequest.yesCount;

            return (
              <TableControl id="rotation-list" headers={headers}>
                {_.map(rotationList, (listItem, i) => {
                  const owner = listItem.equipment.owner;
                  var showAllResponseFields = false;
                  if (
                    numberEquipmentAvailableForNormalHire > 0 &&
                    (listItem.offerResponse === STATUS_ASKED || !listItem.offerResponse) &&
                    rentalRequest.yesCount < rentalRequest.equipmentCount
                  ) {
                    showAllResponseFields = true;
                    numberEquipmentAvailableForNormalHire -= 1;
                  }

                  const showBlankLine =
                    i > 0 && rotationList[i - 1].equipment.blockNumber !== listItem.equipment.blockNumber;

                  return [
                    showBlankLine && (
                      <tr key={listItem.equipment.blockNumber} className="blank-row">
                        <td colSpan="14">&nbsp;</td>
                      </tr>
                    ),
                    <tr key={listItem.id}>
                      <td>{listItem.equipment.seniorityString}</td>
                      <td>{listItem.equipment.blockNumber}</td>
                      <td>{listItem.equipment.hoursYtd}</td>
                      <td>
                        <Link to={`${Constant.EQUIPMENT_PATHNAME}/${listItem.equipment.id}`}>
                          {listItem.equipment.equipmentCode}
                        </Link>
                      </td>
                      <td>
                        {listItem.equipment.equipmentDetails}
                        {this.state.showAttachments && (
                          <div>
                            Attachments:
                            {listItem.equipment.equipmentAttachments &&
                              listItem.equipment.equipmentAttachments.map((item, i) => (
                                <span key={item.id}>
                                  <span> </span>
                                  <span className="attachment">
                                    {item.typeName}
                                    {i + 1 < listItem.equipment.equipmentAttachments.length && <span>,</span>}
                                  </span>
                                </span>
                              ))}
                            {(!listItem.equipment.equipmentAttachments ||
                              listItem.equipment.equipmentAttachments.length === 0) && <span> none</span>}
                          </div>
                        )}
                      </td>
                      <td>{owner && owner.organizationName}</td>
                      <td>{listItem.displayFields.primaryContactName}</td>
                      <td>{owner && owner.primaryContact && owner.primaryContact.workPhoneNumber}</td>
                      <td>{owner && owner.primaryContact && owner.primaryContact.mobilePhoneNumber}</td>
                      <td>
                        <ButtonGroup>
                          {(() => {
                            const changeOfferWarningMessage =
                              'This piece of equipment is has met or ' +
                              'exceeded its Maximum Allowed Hours for this year. Are you sure you want ' +
                              'to edit the Offer on this equipment?';

                            const confirm = (
                              <Confirm
                                title={changeOfferWarningMessage}
                                onConfirm={() => this.openHireOfferDialog(listItem, showAllResponseFields)}
                              />
                            );

                            if (listItem.maximumHours) {
                              return (
                                <OverlayTrigger trigger="focus" placement="top" rootClose overlay={confirm}>
                                  <Button variant="link" size="sm">
                                    Max. hours reached
                                  </Button>
                                </OverlayTrigger>
                              );
                            }
                            if (
                              !viewOnly &&
                              rentalRequest.status === STATUS_IN_PROGRESS &&
                              (listItem.offerResponse === STATUS_ASKED || !listItem.offerResponse)
                            ) {
                              return (
                                <Button
                                  variant="link"
                                  title="Show Offer"
                                  onClick={() => this.openHireOfferDialog(listItem, showAllResponseFields)}
                                >
                                  {this.renderStatusText(listItem)}
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
                })}
              </TableControl>
            );
          })()}
        </div>

        <div className="well history">
          <SubHeader title="History" />
          {rentalRequest.historyEntity && (
            <History historyEntity={rentalRequest.historyEntity} refresh={!this.state.reloading} />
          )}
        </div>
        {this.state.showEditDialog && (
          <RentalRequestsEditDialog
            show={this.state.showEditDialog}
            rentalRequest={this.props.rentalRequest}
            onSave={this.rentalRequestSaved}
            onClose={this.closeEditDialog}
          />
        )}
        {this.state.showHireOfferDialog && (
          <HireOfferEditDialog
            show={this.state.showHireOfferDialog}
            hireOffer={this.state.rotationListHireOffer}
            rentalRequest={this.props.rentalRequest}
            showAllResponseFields={this.state.showAllResponseFields}
            onSave={this.hireOfferSaved}
            onClose={this.closeHireOfferDialog}
          />
        )}
        {this.state.showDocumentsDialog && (
          <DocumentsListDialog
            show={this.state.showDocumentsDialog}
            parent={rentalRequest}
            onClose={this.closeDocumentsDialog}
          />
        )}
        {this.state.showNotesDialog && (
          <NotesDialog
            id={String(this.props.match.params.rentalRequestId)}
            show={this.state.showNotesDialog}
            notes={this.props.rentalRequest.notes}
            getNotes={Api.getRentalRequestNotes}
            saveNote={Api.addRentalRequestNote}
            onClose={this.closeNotesDialog}
          />
        )}
      </div>
    );
  }
}

function mapStateToProps(state) {
  return {
    rentalRequest: activeRentalRequestSelector(state),
    documents: state.models.documents,
  };
}

export default connect(mapStateToProps)(RentalRequestsDetail);
