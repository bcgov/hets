import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import { Row, Col } from 'react-bootstrap';
import { Alert, Button, ButtonGroup, Badge } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import _ from 'lodash';

import EquipmentEditDialog from './dialogs/EquipmentEditDialog.jsx';
import SeniorityEditDialog from './dialogs/SeniorityEditDialog.jsx';
import AttachmentAddDialog from './dialogs/AttachmentAddDialog.jsx';
import AttachmentEditDialog from './dialogs/AttachmentEditDialog.jsx';
import DocumentsListDialog from './dialogs/DocumentsListDialog.jsx';
import NotesDialog from './dialogs/NotesDialog.jsx';
import EquipmentChangeStatusDialog from './dialogs/EquipmentChangeStatusDialog.jsx';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import * as Log from '../history';

import BadgeLabel from '../components/BadgeLabel.jsx';
import ColDisplay from '../components/ColDisplay.jsx';
import DeleteButton from '../components/DeleteButton.jsx';
import SortTable from '../components/SortTable.jsx';
import Spinner from '../components/Spinner.jsx';
import History from '../components/History.jsx';
import PageHeader from '../components/ui/PageHeader.jsx';
import SubHeader from '../components/ui/SubHeader.jsx';
import StatusDropdown from '../components/StatusDropdown.jsx';
import ReturnButton from '../components/ReturnButton.jsx';
import PrintButton from '../components/PrintButton.jsx';
import Authorize from '../components/Authorize.jsx';

import { activeEquipmentSelector } from '../selectors/ui-selectors';

import { formatDateTime } from '../utils/date';
import { formatHours } from '../utils/string';

/*

TODO:
* Print / Notes / Docs / History / Actions on Equipment / Seniority Data History / Equipment Attachments

*/

const EQUIPMENT_IN_ACTIVE_RENTAL_REQUEST_WARNING_MESSAGE =
  'This equipment is part of an In Progress ' +
  'Rental Request. Release the list (finish hiring / delete) before making this change';

class EquipmentDetail extends React.Component {
  static propTypes = {
    equipment: PropTypes.object,
    notes: PropTypes.array,
    attachments: PropTypes.object,
    documents: PropTypes.object,
    history: PropTypes.object,
    params: PropTypes.object,
    ui: PropTypes.object,
    location: PropTypes.object,
  };

  constructor(props) {
    super(props);

    this.state = {
      loading: true,
      loadingDocuments: true,
      loadingNotes: true,
      reloading: false,

      showEditDialog: false,
      showDocumentsDialog: false,
      showSeniorityDialog: false,
      showPhysicalAttachmentDialog: false,
      showPhysicalAttachmentEditDialog: false,
      showNotesDialog: false,
      showChangeStatusDialog: false,
      equipmentPhysicalAttachment: {},
      ui: {
        // Physical Attachments
        sortField: props.ui.sortField || 'attachmentTypeName',
        sortDesc: props.ui.sortDesc === true,
      },
    };
  }

  componentDidMount() {
    this.props.dispatch({
      type: Action.SET_ACTIVE_EQUIPMENT_ID_UI,
      equipmentId: this.props.match.params.equipmentId,
    });

    const equipmentId = this.props.match.params.equipmentId;

    // Notes and documents need be fetched every time as they are not equipment-specific in the store ATM
    this.props.dispatch(Api.getEquipmentNotes(equipmentId)).then(() => this.setState({ loadingNotes: false }));
    this.props.dispatch(Api.getEquipmentDocuments(equipmentId)).then(() => this.setState({ loadingDocuments: false }));

    // Re-fetch equipment every time
    Promise.all([this.fetch()]).then(() => {
      this.setState({ loading: false });
    });
  }

  fetch = () => {
    this.setState({ reloading: true });
    return this.props.dispatch(Api.getEquipment(this.props.match.params.equipmentId))
      .then(() => this.setState({ reloading: false }));
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

  updateUIState = (state, callback) => {
    this.setState({ ui: { ...this.state.ui, ...state } }, () => {
      this.props.dispatch({
        type: Action.UPDATE_PHYSICAL_ATTACHMENTS_UI,
        equipmentPhysicalAttachments: this.state.ui,
      });
      if (callback) {
        callback();
      }
    });
  };

  openEditDialog = () => {
    this.setState({ showEditDialog: true });
  };

  closeEditDialog = () => {
    this.setState({ showEditDialog: false });
  };

  updateStatusState = (state) => {
    if (state !== this.props.equipment.status) {
      this.setState({ status: state }, () => this.openChangeStatusDialog());
    }
  };

  openChangeStatusDialog = () => {
    this.setState({ showChangeStatusDialog: true });
  };

  closeChangeStatusDialog = () => {
    this.setState({ showChangeStatusDialog: false });
  };

  onStatusChanged = () => {
    this.closeChangeStatusDialog();
    this.props.dispatch(Api.getEquipmentNotes(this.props.equipment.id));
  };

  openSeniorityDialog = () => {
    this.setState({ showSeniorityDialog: true });
  };

  closeSeniorityDialog = () => {
    this.setState({ showSeniorityDialog: false });
  };

  openPhysicalAttachmentDialog = () => {
    this.setState({
      showPhysicalAttachmentDialog: true,
    });
  };

  closePhysicalAttachmentDialog = () => {
    this.setState({ showPhysicalAttachmentDialog: false });
  };

  physicalAttachmentsAdded = () => {
    var equipId = this.props.match.params.equipmentId;
    this.props.dispatch(Api.getEquipment(equipId));
  };

  openPhysicalAttachmentEditDialog = (attachment) => {
    this.setState({
      equipmentPhysicalAttachment: attachment,
      showPhysicalAttachmentEditDialog: true,
    });
  };

  closePhysicalAttachmentEditDialog = () => {
    this.setState({ showPhysicalAttachmentEditDialog: false });
  };

  physicalAttachmentEdited = () => {
    var equipId = this.props.match.params.equipmentId;
    this.props.dispatch(Api.getEquipment(equipId));
  };

  deletePhysicalAttachment = async (attachmentId) => {
    const dispatch = this.props.dispatch;
    await dispatch(Api.deletePhysicalAttachment(attachmentId));
    let attachment = _.find(
      this.props.equipment.equipmentAttachments,
      (attachment) => attachment.id === attachmentId
    );
    await dispatch(Log.equipmentAttachmentDeleted(this.props.equipment, attachment.typeName));
    const equipId = this.props.match.params.equipmentId;
    dispatch(Api.getEquipment(equipId));
  };

  getLastVerifiedStyle = (equipment) => {
    var daysSinceVerified = equipment.daysSinceVerified;
    if (daysSinceVerified >= Constant.EQUIPMENT_DAYS_SINCE_VERIFIED_CRITICAL) {
      return 'danger';
    }
    if (daysSinceVerified >= Constant.EQUIPMENT_DAYS_SINCE_VERIFIED_WARNING) {
      return 'warning';
    }
    return 'success';
  };

  getStatuses = () => {
    var dropdownItems = _.pull(
      [
        Constant.EQUIPMENT_STATUS_CODE_APPROVED,
        Constant.EQUIPMENT_STATUS_CODE_PENDING,
        Constant.EQUIPMENT_STATUS_CODE_ARCHIVED,
      ],
      this.props.equipment.status
    );
    if (this.props.equipment.ownerStatus === Constant.OWNER_STATUS_CODE_PENDING) {
      return _.pull(dropdownItems, Constant.EQUIPMENT_STATUS_CODE_APPROVED);
    } else if (this.props.equipment.ownerStatus === Constant.OWNER_STATUS_CODE_ARCHIVED) {
      return [];
    }
    return dropdownItems;
  };

  render() {
    var equipment = this.props.equipment || {};
    const { loadingNotes, loadingDocuments } = this.state;
    const { dispatch } = this.props;

    var lastVerifiedStyle = this.getLastVerifiedStyle(equipment);

    return (
      <div id="equipment-detail">
        <div>
          {(() => {
            if (this.state.loading) {
              return (
                <div className="spinner-container">
                  <Spinner />
                </div>
              );
            }

            return (
              <div className="top-container">
                <Row id="equipment-top">
                  <Col sm={9}>
                    <Row>
                      {this.props.equipment && (
                        <StatusDropdown
                          id="equipment-status-dropdown"
                          status={equipment.status}
                          statuses={this.getStatuses()}
                          disabled={equipment.activeRentalRequest}
                          disabledTooltip={EQUIPMENT_IN_ACTIVE_RENTAL_REQUEST_WARNING_MESSAGE}
                          onSelect={this.updateStatusState}
                        />
                      )}
                      <Button
                        className="mr-1 ml-1 btn-custom"
                        title="Notes"
                        onClick={this.showNotes}
                        disabled={loadingNotes}
                      >
                        Notes {!loadingNotes && `(${this.props.notes.length})`}
                      </Button>
                      <Button
                        title="Documents"
                        className="btn-custom"
                        onClick={this.showDocuments}
                        disabled={loadingDocuments}
                      >
                        Documents {!loadingDocuments && `(${Object.keys(this.props.documents).length})`}
                      </Button>
                    </Row>
                  </Col>
                  <Col sm={3}>
                    <div className="float-right">
                      <PrintButton />
                      <ReturnButton />
                    </div>
                  </Col>
                </Row>
                <Row id="equipment-bottom">
                  <Badge variant="secondary" className={equipment.isMaintenanceContractor ? '' : 'hide'}>
                    Maintenance Contractor
                  </Badge>
                  <Badge variant={equipment.isHired ? 'success' : 'secondary'}>
                    {equipment.isHired ? 'Hired' : 'Not Hired'}
                  </Badge>
                  <Badge variant={lastVerifiedStyle}>
                    Last Verified: {formatDateTime(equipment.lastVerifiedDate, Constant.DATE_YEAR_SHORT_MONTH_DAY)}
                  </Badge>
                </Row>
                <div className="equipment-header">
                  <PageHeader title="Equipment Id" subTitle={`${equipment.equipmentCode} (${equipment.typeName})`} />
                  <PageHeader
                    title="Company"
                    subTitle={
                      <Link to={`${Constant.OWNERS_PATHNAME}/${equipment.ownerId}`}>{equipment.organizationName}</Link>
                    }
                  />
                  <div className="district-office">
                    <strong>District Office:</strong> {equipment.districtName}
                  </div>
                  <div className="local-area">
                    <strong>Service/Local Area:</strong>{' '}
                    {equipment.localArea && `${equipment.localArea.serviceAreaId} - ${equipment.localAreaName}`}
                  </div>
                </div>
              </div>
            );
          })()}

          <Row>
            <Col md={12}>
              <div className="well">
                <SubHeader
                  title="Equipment Information"
                  editButtonTitle="Edit Equipment"
                  editButtonDisabled={equipment.activeRentalRequest}
                  editButtonDisabledTooltip={EQUIPMENT_IN_ACTIVE_RENTAL_REQUEST_WARNING_MESSAGE}
                  onEditClicked={this.openEditDialog}
                />
                {(() => {
                  if (this.state.loading) {
                    return (
                      <div className="spinner-container">
                        <Spinner />
                      </div>
                    );
                  }

                  return (
                    <Row className="equal-height">
                      <Col lg={4} md={6} sm={12} xs={12}>
                        <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Equipment Type">
                          {equipment.typeName}
                        </ColDisplay>
                      </Col>
                      <Col lg={4} md={6} sm={12} xs={12}>
                        <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Make">
                          {equipment.make}
                        </ColDisplay>
                      </Col>
                      <Col lg={4} md={6} sm={12} xs={12}>
                        <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Model">
                          {equipment.model}
                        </ColDisplay>
                      </Col>
                      <Col lg={4} md={6} sm={12} xs={12}>
                        <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Year">
                          {equipment.year}
                        </ColDisplay>
                      </Col>
                      <Col lg={4} md={6} sm={12} xs={12}>
                        <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Size">
                          {equipment.size}
                        </ColDisplay>
                      </Col>
                      {equipment.isDumpTruck && (
                        <Col lg={4} md={6} sm={12} xs={12}>
                          <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Licenced GVW">
                            {equipment.licencedGvw}
                          </ColDisplay>
                        </Col>
                      )}
                      {equipment.isDumpTruck && (
                        <Col lg={4} md={6} sm={12} xs={12}>
                          <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Truck Legal Capacity">
                            {equipment.legalCapacity}
                          </ColDisplay>
                        </Col>
                      )}
                      <Col lg={4} md={6} sm={12} xs={12}>
                        <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Type">
                          {equipment.type}
                        </ColDisplay>
                      </Col>
                      <Col lg={4} md={6} sm={12} xs={12}>
                        <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Licence Number">
                          {equipment.licencePlate}
                        </ColDisplay>
                      </Col>
                      <Col lg={4} md={6} sm={12} xs={12}>
                        <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Serial Number">
                          {equipment.serialNumber}
                          {equipment.hasDuplicates ? <BadgeLabel variant="danger">!</BadgeLabel> : null}
                        </ColDisplay>
                      </Col>
                      {equipment.isDumpTruck && (
                        <Col lg={4} md={6} sm={12} xs={12}>
                          <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Pup Legal Capacity">
                            {equipment.pupLegalCapacity}
                          </ColDisplay>
                        </Col>
                      )}
                    </Row>
                  );
                })()}
              </div>
            </Col>
            <Col md={12}>
              <div className="well">
                <Authorize requires={Constant.PERMISSION_WRITE_ACCESS}>
                  <SubHeader
                    title="Attachments"
                    editButtonTitle="Add Attachment"
                    editIcon="plus"
                    onEditClicked={this.openPhysicalAttachmentDialog}
                  />
                </Authorize>
                {(() => {
                  if (this.state.loading) {
                    return (
                      <div className="spinner-container">
                        <Spinner />
                      </div>
                    );
                  }
                  if (!equipment.equipmentAttachments || Object.keys(equipment.equipmentAttachments).length === 0) {
                    return <Alert variant="success">No Attachments</Alert>;
                  }

                  var physicalAttachments = _.sortBy(equipment.equipmentAttachments, this.state.ui.sortField);
                  if (this.state.ui.sortDesc) {
                    _.reverse(physicalAttachments);
                  }

                  var headers = [{ field: 'attachmentTypeName', title: 'Type' }, { field: 'blank' }];

                  return (
                    <SortTable
                      id="physical-attachment-list"
                      sortField={this.state.ui.sortField}
                      sortDesc={this.state.ui.sortDesc}
                      onSort={this.updateUIState}
                      headers={headers}
                    >
                      {_.map(physicalAttachments, (attachment) => {
                        return (
                          <tr key={attachment.id}>
                            <td>{attachment.typeName}</td>
                            <td style={{ textAlign: 'right' }}>
                              <ButtonGroup>
                                <Button
                                  className="btn-custom"
                                  title="Edit Attachment"
                                  size="sm"
                                  onClick={this.openPhysicalAttachmentEditDialog.bind(this, attachment)}
                                >
                                  <FontAwesomeIcon icon="pencil-alt" />
                                </Button>
                                <Authorize requires={Constant.PERMISSION_WRITE_ACCESS}>
                                  <DeleteButton
                                    onConfirm={this.deletePhysicalAttachment.bind(this, attachment.id)}
                                    name="Attachment"
                                  />
                                </Authorize>
                              </ButtonGroup>
                            </td>
                          </tr>
                        );
                      })}
                    </SortTable>
                  );
                })()}
              </div>
            </Col>
          </Row>
          <Row>
            <Col md={12}>
              <div className="well">
                <SubHeader
                  title="Seniority"
                  editButtonTitle="Edit Seniority"
                  editButtonDisabled={equipment.activeRentalRequest}
                  editButtonDisabledTooltip={EQUIPMENT_IN_ACTIVE_RENTAL_REQUEST_WARNING_MESSAGE}
                  onEditClicked={this.openSeniorityDialog}
                />
                {(() => {
                  if (this.state.loading) {
                    return (
                      <div className="spinner-container">
                        <Spinner />
                      </div>
                    );
                  }

                  return (
                    <Row>
                      <Col lg={12}>
                        <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Seniority">
                          {equipment.seniorityString}
                        </ColDisplay>
                      </Col>
                      <Col lg={12}>
                        <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Hours YTD">
                          {formatHours(equipment.hoursYtd)}
                        </ColDisplay>
                      </Col>
                      <Col lg={12}>
                        <ColDisplay
                          labelProps={{ xs: 4 }}
                          fieldProps={{ xs: 8 }}
                          label={<span>Hours {equipment.yearMinus1}</span>}
                        >
                          {formatHours(equipment.serviceHoursLastYear)}
                        </ColDisplay>
                      </Col>
                      <Col lg={12}>
                        <ColDisplay
                          labelProps={{ xs: 4 }}
                          fieldProps={{ xs: 8 }}
                          label={<span>Hours {equipment.yearMinus2}</span>}
                        >
                          {formatHours(equipment.serviceHoursTwoYearsAgo)}
                        </ColDisplay>
                      </Col>
                      <Col lg={12}>
                        <ColDisplay
                          labelProps={{ xs: 4 }}
                          fieldProps={{ xs: 8 }}
                          label={<span>Hours {equipment.yearMinus3}</span>}
                        >
                          {formatHours(equipment.serviceHoursThreeYearsAgo)}
                        </ColDisplay>
                      </Col>
                      <Col lg={12}>
                        <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Years Registered">
                          {equipment.yearsOfService}
                        </ColDisplay>
                      </Col>
                      <Col lg={12}>
                        <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Received Date">
                          {formatDateTime(equipment.receivedDate, Constant.DATE_YEAR_SHORT_MONTH_DAY)}
                        </ColDisplay>
                      </Col>
                      <Col lg={12}>
                        <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Registered Date">
                          {formatDateTime(equipment.approvedDate, Constant.DATE_YEAR_SHORT_MONTH_DAY)}
                        </ColDisplay>
                      </Col>
                      <Col lg={12}>
                        <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Override Status">
                          {equipment.isSeniorityOverridden ? 'Manually Updated' : 'Not Overriden'}
                        </ColDisplay>
                      </Col>
                      <Col lg={12}>
                        <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Override Reason">
                          {equipment.seniorityOverrideReason}
                        </ColDisplay>
                      </Col>
                    </Row>
                  );
                })()}
              </div>
            </Col>
            <Col md={12}>
              <div className="well">
                <SubHeader title="History" />
                {equipment.historyEntity && (
                  <History historyEntity={equipment.historyEntity} refresh={!this.state.reloading} />
                )}
              </div>
            </Col>
          </Row>
        </div>
        {this.state.showChangeStatusDialog && (
          <EquipmentChangeStatusDialog
            show={this.state.showChangeStatusDialog}
            status={this.state.status}
            equipment={equipment}
            onClose={this.closeChangeStatusDialog}
            onStatusChanged={this.onStatusChanged}
          />
        )}
        {this.state.showNotesDialog && (
          <NotesDialog
            show={this.state.showNotesDialog}
            id={this.props.match.params.equipmentId}
            notes={this.props.notes}
            getNotes={(equipmentId) => dispatch(Api.getEquipmentNotes(equipmentId))}
            saveNote={(equipmentId, note) => dispatch(Api.addEquipmentNote(equipmentId, note))}
            onClose={this.closeNotesDialog}
          />
        )}
        {this.state.showDocumentsDialog && (
          <DocumentsListDialog
            show={this.props.equipment && this.state.showDocumentsDialog}
            parent={this.props.equipment}
            onClose={this.closeDocumentsDialog}
          />
        )}
        {this.state.showEditDialog && (
          <EquipmentEditDialog show={this.state.showEditDialog} onClose={this.closeEditDialog} equipment={equipment} />
        )}
        {this.state.showSeniorityDialog && (
          <SeniorityEditDialog
            show={this.state.showSeniorityDialog}
            onClose={this.closeSeniorityDialog}
            equipment={equipment}
          />
        )}
        {this.state.showPhysicalAttachmentDialog && (
          <AttachmentAddDialog
            show={this.state.showPhysicalAttachmentDialog}
            onSave={this.physicalAttachmentsAdded}
            onClose={this.closePhysicalAttachmentDialog}
            equipment={equipment}
          />
        )}
        {this.state.showPhysicalAttachmentEditDialog && (
          <AttachmentEditDialog
            show={this.state.showPhysicalAttachmentEditDialog}
            onSave={this.physicalAttachmentEdited}
            onClose={this.closePhysicalAttachmentEditDialog}
            equipment={equipment}
            attachment={this.state.equipmentPhysicalAttachment}
          />
        )}
      </div>
    );
  }
}

const mapStateToProps = (state) => ({
  equipment: activeEquipmentSelector(state),
  notes: state.models.equipmentNotes,
  attachments: state.models.equipmentAttachments,
  documents: state.models.documents,
  history: state.models.equipmentHistory,
  ui: state.ui.equipmentPhysicalAttachments,
});

const mapDispatchToProps = (dispatch) => ({ dispatch });

export default connect(mapStateToProps, mapDispatchToProps)(EquipmentDetail);
