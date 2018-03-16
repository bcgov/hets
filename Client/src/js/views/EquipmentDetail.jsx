import React from 'react';

import { connect } from 'react-redux';

import { browserHistory } from 'react-router';

import { Well, Row, Col } from 'react-bootstrap';
import { Alert, Button, ButtonGroup, Glyphicon, Label, DropdownButton, MenuItem } from 'react-bootstrap';

import _ from 'lodash';
import Promise from 'bluebird';

import EquipmentEditDialog from './dialogs/EquipmentEditDialog.jsx';
import SeniorityEditDialog from './dialogs/SeniorityEditDialog.jsx';
import AttachmentAddDialog from './dialogs/AttachmentAddDialog.jsx';
import AttachmentEditDialog from './dialogs/AttachmentEditDialog.jsx';
import DocumentsListDialog from './dialogs/DocumentsListDialog.jsx';
import NotesDialog from './dialogs/NotesDialog.jsx';
import ChangeStatusDialog from './dialogs/ChangeStatusDialog.jsx';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import * as Log from '../history';
import store from '../store';

import BadgeLabel from '../components/BadgeLabel.jsx';
import ColDisplay from '../components/ColDisplay.jsx';
import Confirm from '../components/Confirm.jsx';
import OverlayTrigger from '../components/OverlayTrigger.jsx';
import SortTable from '../components/SortTable.jsx';
import Spinner from '../components/Spinner.jsx';
import History from '../components/History.jsx';
import Unimplemented from '../components/Unimplemented.jsx';

import { formatDateTime } from '../utils/date';

/*

TODO:
* Print / Notes / Docs / History / Actions on Equipment / Seniority Data History / Equipment Attachments

*/

var EquipmentDetail = React.createClass({
  propTypes: {
    equipment: React.PropTypes.object,
    equipmentPhysicalAttachments: React.PropTypes.object,
    equipmentSeniorityHistory: React.PropTypes.object,
    notes: React.PropTypes.object,
    attachments: React.PropTypes.object,
    documents: React.PropTypes.object,
    history: React.PropTypes.object,
    params: React.PropTypes.object,
    ui: React.PropTypes.object,
    location: React.PropTypes.object,
  },

  getInitialState() {
    return {
      // If we are coming in through the Owner screen then return to it; otherwise go back to Equipment search
      returnUrl: (this.props.location.state || {}).returnUrl || Constant.EQUIPMENT_PATHNAME,
      loading: false,
      showEditDialog: false,
      showDocumentsDialog: false,
      showSeniorityDialog: false,
      showPhysicalAttachmentDialog: false,
      showPhysicalAttachmentEditDialog: false,
      showNotesDialog: false,
      showChangeStatusDialog: false,
      equipmentPhysicalAttachment: {},
      ui : {
        // Physical Attachments
        sortField: this.props.ui.sortField || 'attachmentTypeName',
        sortDesc: this.props.ui.sortDesc === true,
      },
    };
  },

  componentDidMount() {
    this.fetch();
  },

  componentDidUpdate(prevProps) {
    if (prevProps.params.equipmentId !== this.props.params.equipmentId) {
      this.fetch();
    }
  },

  fetch() {
    this.setState({ loading: true });
    
    var equipmentId = this.props.params.equipmentId;
    var getEquipmentPromise = Api.getEquipment(equipmentId);
    var documentsPromise = Api.getEquipmentDocuments(equipmentId);
    var getEquipmentNotesPromise = Api.getEquipmentNotes(equipmentId);

    return Promise.all([getEquipmentPromise, documentsPromise, getEquipmentNotesPromise]).finally(() => {
      this.setState({ loading: false });
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

  showHistory() {
  },

  showSeniorityHistory() {
  },

  print() {
    window.print();
  },

  addDocument() {
  },

  updateUIState(state, callback) {
    this.setState({ ui: { ...this.state.ui, ...state } }, () => {
      store.dispatch({ type: Action.UPDATE_PHYSICAL_ATTACHMENTS_UI, equipmentPhysicalAttachments: this.state.ui });
      if (callback) { callback(); }
    });
  },

  actionSelected(/*eventKey*/) {
    // TODO Implement
  },

  openEditDialog() {
    this.setState({ showEditDialog: true });
  },

  closeEditDialog() {
    this.setState({ showEditDialog: false });
  },

  saveEdit(equipment) {
    Api.updateEquipment(equipment).finally(() => {
      Log.equipmentModified(this.props.equipment);
      this.closeEditDialog();
    });
  },

  updateStatusState(state) {
    if (state !== this.props.equipment.status) {
      this.setState({ status: state }, this.openChangeStatusDialog());
    }
  },

  openChangeStatusDialog() {
    this.setState({ showChangeStatusDialog: true });
  },

  closeChangeStatusDialog() {
    this.setState({ showChangeStatusDialog: false });
  },

  onChangeStatus(status) {
    Api.changeEquipmentStatus(status).then(() => {
      Log.equipmentStatusModified(this.props.equipment, status.status);
      this.closeChangeStatusDialog();
    });
  },

  openSeniorityDialog() {
    this.setState({ showSeniorityDialog: true });
  },

  closeSeniorityDialog() {
    this.setState({ showSeniorityDialog: false });
  },

  saveSeniorityEdit(equipment) {
    Api.updateEquipment(equipment).finally(() => {
      Log.equipmentSeniorityModified(this.props.equipment);
      this.closeSeniorityDialog();
    });
  },

  openPhysicalAttachmentDialog() {
    this.setState({
      showPhysicalAttachmentDialog: true,
    });
  },

  closePhysicalAttachmentDialog() {
    this.setState({ showPhysicalAttachmentDialog: false });
  },

  addPhysicalAttachment(attachment) {
    Api.addPhysicalAttachment(attachment).then(() => {
      Log.equipmentAttachmentAdded(this.props.equipment, attachment.typeName);
      var equipId = this.props.params.equipmentId;
      Api.getEquipment(equipId);
      this.closePhysicalAttachmentDialog();
    });
  },

  openPhysicalAttachmentEditDialog(attachment) {
    this.setState({
      equipmentPhysicalAttachment: attachment,
      showPhysicalAttachmentEditDialog: true,
    });
  },

  closePhysicalAttachmentEditDialog() {
    this.setState({ showPhysicalAttachmentEditDialog: false });
  },

  updatePhysicalAttachment(attachment) {
    Api.updatePhysicalAttachment(attachment).then(() => {
      Log.equipmentAttachmentUpdated(this.props.equipment, attachment.typeName);
      var equipId = this.props.params.equipmentId;
      Api.getEquipment(equipId);
      this.closePhysicalAttachmentEditDialog();
    });
  },

  deletePhysicalAttachment(attachmentId) {
    Api.deletePhysicalAttachment(attachmentId).then(() => {
      let attachment = _.find(this.props.equipment.equipmentAttachments, ((attachment) => attachment.id === attachmentId ));
      Log.equipmentAttachmentDeleted(this.props.equipment, attachment.typeName);
      var equipId = this.props.params.equipmentId;
      Api.getEquipment(equipId);
    });
  },

  getLastVerifiedStyle(equipment) {
    var daysSinceVerified = equipment.daysSinceVerified;
    if (daysSinceVerified >= Constant.EQUIPMENT_DAYS_SINCE_VERIFIED_CRITICAL) { return 'danger'; }
    if (daysSinceVerified >= Constant.EQUIPMENT_DAYS_SINCE_VERIFIED_WARNING) { return 'warning'; }
    return 'success';
  },

  getStatusDropdownStyle() {
    switch(this.props.equipment.status) {
      case(Constant.EQUIPMENT_STATUS_CODE_APPROVED):
        return 'success';
      case(Constant.EQUIPMENT_STATUS_CODE_PENDING):
        return 'danger';
      default: 
        return 'default';
    }
  },

  getStatuses() {
    var dropdownItems = _.pull([ Constant.EQUIPMENT_STATUS_CODE_APPROVED, Constant.EQUIPMENT_STATUS_CODE_PENDING, Constant.EQUIPMENT_STATUS_CODE_ARCHIVED ], this.props.equipment.status);
    if (this.props.equipment.ownerStatus === Constant.OWNER_STATUS_CODE_PENDING) {
      return _.pull(dropdownItems, Constant.EQUIPMENT_STATUS_CODE_APPROVED);
    } else if (this.props.equipment.ownerStatus === Constant.OWNER_STATUS_CODE_ARCHIVED) {
      return [];
    }
    return dropdownItems;
  },

  render() {
    var equipment = this.props.equipment;
    var lastVerifiedStyle = this.getLastVerifiedStyle(equipment);
    var dropdownItems = this.getStatuses();

    return (
      <div id="equipment-detail">
        <div>
          {(() => {

            if (this.state.loading) { return <div className="spinner-container"><Spinner/></div>; }

            return (
              <div className="top-container">
                <Row id="equipment-top">
                  <Col md={8}>
                    <Row>
                      <DropdownButton
                        id="owner-status"
                        bsStyle={ this.getStatusDropdownStyle() }
                        title={ equipment.status || '' }
                        onSelect={ this.updateStatusState }
                        disabled={ equipment.ownerStatus === Constant.OWNER_STATUS_CODE_ARCHIVED }
                      >
                      { _.map(dropdownItems.map((item, i) =>
                        <MenuItem key={ i } eventKey={ item }>{ item }</MenuItem>
                      ))}
                      </DropdownButton>
                      <Button className="mr-5 ml-5" title="Notes" onClick={ this.showNotes }>Notes ({ Object.keys(this.props.notes).length })</Button>
                      <Button title="Documents" onClick={ this.showDocuments }>Documents ({ Object.keys(this.props.documents).length })</Button>
                    </Row>
                  </Col>
                  <Col md={4}>
                    <div className="pull-right">
                      <Button className="mr-5" onClick={ this.print }><Glyphicon glyph="print" title="Print" /></Button>
                      <Button title="Return" onClick={ browserHistory.goBack }><Glyphicon glyph="arrow-left" /> Return</Button>
                    </div>
                  </Col>
                </Row>
                <Row id="equipment-bottom">
                  <Label className={ equipment.isMaintenanceContractor ? '' : 'hide' }>Maintenance Contractor</Label>
                  <Label bsStyle={ equipment.isHired ? 'success' : 'default' }>{ equipment.isHired ? 'Hired' : 'Not Hired' }</Label>
                  <Label bsStyle={ lastVerifiedStyle }>Last Verified: { formatDateTime(equipment.lastVerifiedDate, Constant.DATE_YEAR_SHORT_MONTH_DAY) }</Label>
                </Row>
                <Row id="equipment-header">
                  <ColDisplay md={12} label={ <h1>Equipment Id:</h1> }><h1><small>{ equipment.equipmentCode } ({ equipment.typeName })</small></h1></ColDisplay>
                </Row>
                <Row>
                  <ColDisplay md={12} label={ <h1>Company:</h1> }><h1><small>{ equipment.organizationName }</small></h1></ColDisplay>
                </Row>
                <Row>
                  <Col md={6}>
                    <Row>
                      <ColDisplay md={12} labelProps={{ md: 4 }} label="District Office:">{ equipment.districtName }</ColDisplay>
                    </Row>
                    <Row>
                      <ColDisplay md={12} labelProps={{ md: 4 }} style={{ marginBottom: '20px' }} label="Service/Local Area:">{ equipment.localAreaName }</ColDisplay>
                    </Row>
                  </Col>
                </Row>
              </div>
            );
          })()}

          <Row>
            <Col md={12}>
              <Well>
                <h3>Equipment Information <span className="pull-right">
                  <Button title="Edit Equipment" bsSize="small" onClick={ this.openEditDialog }><Glyphicon glyph="pencil" /></Button>
                </span></h3>
                {(() => {
                  if (this.state.loading) { return <div className="spinner-container"><Spinner /></div>; }

                  return <Row>
                    <Col md={6}>
                      <Row>
                        <ColDisplay labelProps={{ md: 4 }} label="Type">{ equipment.typeName }</ColDisplay>
                      </Row>
                      <Row>
                        <ColDisplay labelProps={{ md: 4 }} label="Make">{ equipment.make }</ColDisplay>
                      </Row>
                      <Row>
                        <ColDisplay labelProps={{ md: 4 }} label="Model">{ equipment.model }</ColDisplay>
                      </Row>
                      <Row>
                        <ColDisplay labelProps={{ md: 4 }} label="Year">{ equipment.year }</ColDisplay>
                      </Row>
                      { equipment.isDumpTruck &&
                        <div>
                          <Row>
                            <ColDisplay labelProps={{ md: 4 }} label="Licenced GVW">{ equipment.licencedGvw }</ColDisplay>
                          </Row>
                          <Row>
                            <ColDisplay labelProps={{ md: 4 }} label="Truck Legal Capacity">{ equipment.legalCapacity }</ColDisplay>
                          </Row>
                        </div>
                      }
                    </Col>
                    <Col md={6}>
                      <Row>
                        <ColDisplay labelProps={{ md: 4 }} label="Size">{ equipment.size }</ColDisplay>
                      </Row>
                      <Row>
                        <ColDisplay labelProps={{ md: 4 }} label="Type">{ equipment.type }</ColDisplay>
                      </Row>
                      <Row>
                        <ColDisplay labelProps={{ md: 4 }} label="Licence Number">{ equipment.licencePlate }</ColDisplay>
                      </Row>
                      <Row>
                        <ColDisplay labelProps={{ md: 4 }} label="Serial Number">{ equipment.serialNumber }
                          { equipment.hasDuplicates ? <BadgeLabel bsStyle="danger">!</BadgeLabel> : null }
                        </ColDisplay>
                      </Row>
                      { equipment.isDumpTruck && 
                        <Row>
                          <ColDisplay labelProps={{ md: 4 }} label="Pup Legal Capacity">{ equipment.pupLegalCapacity }</ColDisplay>
                        </Row>
                      }
                    </Col>
                  </Row>;
                })()}
              </Well>
            </Col>
            <Col md={12}>
              <Well>
                <h3>Attachments <span className="pull-right">
                  <Button title="Add Attachment" bsSize="small" onClick={this.openPhysicalAttachmentDialog}><Glyphicon glyph="plus" /></Button>
                </span></h3>
                {(() => {
                  if (this.state.loading ) { return <div className="spinner-container"><Spinner/></div>; } 
                  if (equipment.equipmentAttachments && Object.keys(equipment.equipmentAttachments).length === 0) { return <Alert bsStyle="success">No Attachments</Alert>; }

                  var physicalAttachments = _.sortBy(equipment.equipmentAttachments, this.state.ui.sortField);
                  if (this.state.ui.sortDesc) {
                    _.reverse(physicalAttachments);
                  } 


                  var headers = [
                    { field: 'attachmentTypeName', title: 'Type' },
                    { field: 'blank' },
                  ];

                  return <SortTable 
                            id="physical-attachment-list" 
                            sortField={ this.state.ui.sortField } 
                            sortDesc={ this.state.ui.sortDesc } 
                            onSort={ this.updateUIState } 
                            headers={ headers }
                          >
                    {
                      _.map(physicalAttachments, (attachment) => {
                        return <tr key={ attachment.id }>
                          <td>{ attachment.typeName }</td>
                          <td style={{ textAlign: 'right' }}>
                            <ButtonGroup>
                              <Button 
                                title="Edit Attachment" 
                                bsSize="xsmall" 
                                onClick={ this.openPhysicalAttachmentEditDialog.bind(this, attachment) }
                              >
                                <Glyphicon glyph="pencil" />
                              </Button>
                              <OverlayTrigger 
                                trigger="click" 
                                placement="top" 
                                rootClose 
                                overlay={ <Confirm onConfirm={ this.deletePhysicalAttachment.bind(this, attachment.id) }/> }
                              >
                                <Button title="Delete Attachment" bsSize="xsmall"><Glyphicon glyph="trash" /></Button>
                              </OverlayTrigger>
                            </ButtonGroup>
                          </td>
                        </tr>;
                      })
                    }
                  </SortTable>;
                })()}
              </Well>
            </Col>
          </Row>
          <Row>
            <Col md={12}>
              <Well>
                <h3>Seniority<span className="pull-right">
                  <Button title="Edit Seniority" bsSize="small" onClick={this.openSeniorityDialog}><Glyphicon glyph="pencil" /></Button>
                </span></h3>
                {(() => {
                  if (this.state.loading) { return <div className="spinner-container"><Spinner/></div>; }

                  var seniorityHistory = this.props.equipmentSeniorityHistory;  // TODO

                  return <div>
                    <Row>
                      <ColDisplay md={12} labelProps={{ md: 4 }} label="Seniority">{ equipment.seniorityText }</ColDisplay>
                    </Row>
                    <Row>
                      <ColDisplay md={12} labelProps={{ md: 4 }} label="Hours YTD">{ equipment.hoursYtd }</ColDisplay>
                    </Row>
                    <Row>
                      <ColDisplay md={12} labelProps={{ md: 4 }} label={ <span>Hours { equipment.lastYear }</span> }>{ equipment.serviceHoursLastYear }</ColDisplay>
                    </Row>
                    <Row>
                      <ColDisplay md={12} labelProps={{ md: 4 }} label={ <span>Hours { equipment.twoYearsAgo }</span> }>{ equipment.serviceHoursTwoYearsAgo }</ColDisplay>
                    </Row>
                    <Row>
                      <ColDisplay md={12} labelProps={{ md: 4 }} label={ <span>Hours { equipment.threeYearsAgo }</span> }>{ equipment.serviceHoursThreeYearsAgo }</ColDisplay>
                    </Row>
                    <Row>
                      <ColDisplay md={12} labelProps={{ md: 4 }} label="Received Date">
                        { formatDateTime(equipment.receivedDate, Constant.DATE_YEAR_SHORT_MONTH_DAY) }
                      </ColDisplay>
                    </Row>
                    <Row>
                      <ColDisplay md={12} labelProps={{ md: 4 }} label="Registered Date">
                        { formatDateTime(equipment.approvedDate, Constant.DATE_YEAR_SHORT_MONTH_DAY) }
                      </ColDisplay>
                    </Row>
                    <Row>
                      <ColDisplay md={12} labelProps={{ md: 4 }} label="Years Registered">{ equipment.yearsOfService }</ColDisplay>
                    </Row>
                    <Row>
                      <ColDisplay md={12} labelProps={{ md: 4 }} label="Override Status">
                        { equipment.isSeniorityOverridden ? 'Manually Updated' : 'Not Overriden'}
                      </ColDisplay>
                    </Row>
                    <Row>
                      <ColDisplay labelProps={{ md: 4 }} label="Override Reason">{ equipment.seniorityOverrideReason }</ColDisplay>
                      <span className="pull-right">
                        <Unimplemented>
                          <Button className="pull-right" title="Show Seniority History" bsSize="small" onClick={ this.showSeniorityHistory} >All ({ Object.keys(seniorityHistory).length })</Button>
                        </Unimplemented>
                      </span>
                    </Row>
                  </div>;
                })()}
              </Well>
            </Col>
            <Col md={12}>
              <Well>
                <h3>History <span className="pull-right"></span></h3>
                { equipment.historyEntity &&
                  <History historyEntity={ equipment.historyEntity } refresh={ !this.state.loading } />
                }
              </Well>
            </Col>
          </Row>
        </div>
        { this.state.showEditDialog &&
          <EquipmentEditDialog 
            show={ this.state.showEditDialog } 
            onSave={ this.saveEdit } 
            onClose= { this.closeEditDialog } 
          />
        }
        { this.state.showSeniorityDialog &&
          <SeniorityEditDialog 
            show={ this.state.showSeniorityDialog } 
            onSave={ this.saveSeniorityEdit } 
            onClose={ this.closeSeniorityDialog } 
          />
        }
        { this.state.showPhysicalAttachmentDialog &&
          <AttachmentAddDialog 
            show={ this.state.showPhysicalAttachmentDialog } 
            onSave={ this.addPhysicalAttachment } 
            onClose={ this.closePhysicalAttachmentDialog }
            equipment={ equipment } 
          />
        }
        { this.state.showPhysicalAttachmentEditDialog &&
          <AttachmentEditDialog 
            show={ this.state.showPhysicalAttachmentEditDialog } 
            onSave={ this.updatePhysicalAttachment } 
            onClose={ this.closePhysicalAttachmentEditDialog }
            equipment={ equipment } 
            attachment={ this.state.equipmentPhysicalAttachment }
          />
        }
        { this.state.showDocumentsDialog &&
          <DocumentsListDialog 
            show={ this.props.equipment && this.state.showDocumentsDialog }  
            parent={ this.props.equipment }
            onClose={ this.closeDocumentsDialog } 
          />
        }
        { this.state.showNotesDialog &&
          <NotesDialog 
            show={ this.state.showNotesDialog } 
            onSave={ Api.addEquipmentNote } 
            id={ this.props.params.equipmentId }
            getNotes={ Api.getEquipmentNotes }
            onUpdate={ Api.updateNote }
            onClose={ this.closeNotesDialog } 
            notes={ this.props.notes }
          />
        } 
        { this.state.showChangeStatusDialog &&
          <ChangeStatusDialog
            show={ this.state.showChangeStatusDialog}
            onClose={ this.closeChangeStatusDialog }
            onSave={ this.onChangeStatus }
            status={ this.state.status }
            parent={ equipment }
          />
        }
      </div>
    );
  },
});


function mapStateToProps(state) {
  return {
    equipment: state.models.equipment,
    equipmentPhysicalAttachments: state.models.equipmentPhysicalAttachments,
    equipmentSeniorityHistory: state.models.equipmentSeniorityHistory,
    notes: state.models.equipmentNotes,
    attachments: state.models.equipmentAttachments,
    documents: state.models.documents,
    history: state.models.equipmentHistory,
    ui: state.ui.equipmentPhysicalAttachments,
  };
}

export default connect(mapStateToProps)(EquipmentDetail);
