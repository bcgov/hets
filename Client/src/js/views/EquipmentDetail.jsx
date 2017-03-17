import React from 'react';

import { connect } from 'react-redux';

import { Well, Row, Col } from 'react-bootstrap';
import { Alert, Button, ButtonGroup, Glyphicon, Label, DropdownButton, MenuItem, Checkbox } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';

import _ from 'lodash';

import EquipmentEditDialog from './dialogs/EquipmentEditDialog.jsx';
import SeniorityEditDialog from './dialogs/SeniorityEditDialog.jsx';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import store from '../store';

import BadgeLabel from '../components/BadgeLabel.jsx';
import ColDisplay from '../components/ColDisplay.jsx';
import Confirm from '../components/Confirm.jsx';
import OverlayTrigger from '../components/OverlayTrigger.jsx';
import SortTable from '../components/SortTable.jsx';
import Spinner from '../components/Spinner.jsx';
import Unimplemented from '../components/Unimplemented.jsx';

import { formatDateTime } from '../utils/date';

// Action drop-down items
const EQUIPMENT_ACTION_ARCHIVE = 'Archive';
const EQUIPMENT_ACTION_VERIFIED = 'Verified';
const EQUIPMENT_ACTION_FOR_HIRE = 'For Hire';

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
    history: React.PropTypes.object,
    params: React.PropTypes.object,
    ui: React.PropTypes.object,
    location: React.PropTypes.object,
  },

  getInitialState() {
    return {
      // If we are coming in through the Owner screen then return to it; otherwise go back to Equipment search
      returnUrl: (this.props.location.state || {}).returnUrl || Constant.EQUIPMENT_PATHNAME,
      loadingEquipment: false,
      loadingPhysicalAttachments: false,
      loadingSeniorityData: false,
      loadingEquipmentHistory: false,
      showEditDialog: false,
      showSeniorityDialog: false,
      showPhysicalAttachmentDialog: false,
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

  fetch() {
    this.setState({ loadingEquipment: true });
    var equipId = this.props.params.equipmentId;
    // Make several calls here
    // TODO Load equipment history, notes and attachments (docs)
    // TODO Load equipment seniority history
    Api.getEquipment(equipId).finally(() => {
      this.setState({ loadingEquipment: false });
    });
  },

  showNotes() {
  },

  showDocuments() {
  },

  showHistory() {
  },

  showSeniorityHistory() {
  },

  print() {
    // TODO Implement
  },

  addNote() {
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

  registeredInAreaChanged() {
  },

  openEditDialog() {
    this.setState({ showEditDialog: true });
  },

  closeEditDialog() {
    this.setState({ showEditDialog: false });
  },

  saveEdit(equipment) {
    Api.updateEquipment(equipment).finally(() => {
      this.closeEditDialog();
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
      this.closeSeniorityDialog();
    });
  },

  openPhysicalAttachmentDialog(attachment) {
    this.setState({
      equipmentPhysicalAttachment: attachment,
      showPhysicalAttachmentDialog: true,
    });
  },

  closePhysicalAttachmentDialog() {
    this.setState({ showPhysicalAttachmentDialog: false });
  },

  addPhysicalAttachment() {
    var newAttachment = {
      id: 0,
      equipment: this.props.equipment,
    };
    this.openPhysicalAttachmentDialog(newAttachment);
  },

  deletePhysicalAttachment(attachment) {
    Api.deletePhysicalAttachment(attachment).then(() => {
      // TODO Refresh attachment list
    });
  },

  getLastVerifiedStyle(equipment) {
    var daysSinceVerified = equipment.daysSinceVerified;
    if (daysSinceVerified >= Constant.EQUIPMENT_DAYS_SINCE_VERIFIED_CRITICAL) { return 'danger'; }
    if (daysSinceVerified >= Constant.EQUIPMENT_DAYS_SINCE_VERIFIED_WARNING) { return 'warning'; }
    return 'success';
  },

  render() {
    var equipment = this.props.equipment;
    var lastVerifiedStyle = this.getLastVerifiedStyle(equipment);

    return <div id="equipment-detail">
      <div>
        <Row id="equipment-top">
          <Col md={8}>
            <Label bsStyle={ equipment.isApproved ? 'success' : 'danger'}>{ equipment.status }</Label>
            <Label className={ equipment.isMaintenanceContractor ? '' : 'hide' }>Maintenance Contractor</Label>
            <Label bsStyle={ equipment.isWorking ? 'danger' : 'success' }>{ equipment.isWorking ? 'Working' : 'Not Working' }</Label>
            <Label bsStyle={ lastVerifiedStyle }>Last Verified: { formatDateTime(equipment.lastVerifiedDate, Constant.DATE_YEAR_SHORT_MONTH_DAY) }</Label>
            <Unimplemented>
              <Button title="Notes" onClick={ this.showNotes }>Notes ({ Object.keys(this.props.notes).length })</Button>
            </Unimplemented>
            <Unimplemented>
              <Button title="Documents" onClick={ this.showDocuments }>Docs ({ Object.keys(this.props.attachments).length })</Button>
            </Unimplemented>
          </Col>
          <Col md={4}>
            <div className="pull-right">
              <Unimplemented>
                <DropdownButton id='equipment-action-drop-down' title='Actions' onSelect={ this.actionSelected }>
                  <MenuItem key={ EQUIPMENT_ACTION_ARCHIVE } eventKey={ EQUIPMENT_ACTION_ARCHIVE }>{ EQUIPMENT_ACTION_ARCHIVE }</MenuItem>
                  <MenuItem key={ EQUIPMENT_ACTION_VERIFIED } eventKey={ EQUIPMENT_ACTION_VERIFIED }>{ EQUIPMENT_ACTION_VERIFIED }</MenuItem>
                  <MenuItem key={ EQUIPMENT_ACTION_FOR_HIRE } eventKey={ EQUIPMENT_ACTION_FOR_HIRE }>{ EQUIPMENT_ACTION_FOR_HIRE }</MenuItem>
                </DropdownButton>
              </Unimplemented>
              <Unimplemented>
                <Button onClick={ this.print }><Glyphicon glyph="print" title="Print" /></Button>
              </Unimplemented>
              <LinkContainer to={{ pathname: this.state.returnUrl }}>
                <Button title="Return to List"><Glyphicon glyph="arrow-left" /> Return to List</Button>
              </LinkContainer>
            </div>
          </Col>
        </Row>

        {(() => {
          if (this.state.loadingEquipment) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

          return <div id="equipment-header">
            <Row>
              <ColDisplay md={12} labelProps={{ md: 4 }} label={ <h1>Company:</h1> }><h1><small>{ equipment.organizationName }</small></h1></ColDisplay>
            </Row>
            <Row>
              <ColDisplay md={12} labelProps={{ md: 4 }} label={ <h1>EquipId:</h1> }><h1><small>{ equipment.equipmentCode } ({ equipment.typeName })</small></h1></ColDisplay>
            </Row>
            <Row>
              <Col md={6}>
                <Row>
                  <ColDisplay md={12} labelProps={{ md: 4 }} label="District Office:">{ equipment.districtName }</ColDisplay>
                </Row>
                <Row>
                  <ColDisplay md={12} labelProps={{ md: 4 }} label="Service/Local Area:">{ equipment.localAreaName }</ColDisplay>
                </Row>
              </Col>
              <Col md={6}>
                <Unimplemented>
                  <Checkbox onChange={this.registeredInAreaChanged}>Have you registered this piece of equipment in this area before?</Checkbox>
                </Unimplemented>
              </Col>
            </Row>
          </div>;
        })()}

        <Row>
          <Col md={6}>
            <Well>
              <h3>Equipment Information <span className="pull-right">
                <Button title="Edit Equipment" bsSize="small" onClick={ this.openEditDialog }><Glyphicon glyph="pencil" /></Button>
              </span></h3>
              {(() => {
                if (this.state.loadingEquipment) { return <div style={{ textAlign: 'center' }}><Spinner /></div>; }

                return <Row>
                  <Col md={4}>
                    <Row>
                      <ColDisplay labelProps={{ md: 6 }} label="Serial Number">{ equipment.serialNumber }
                        { equipment.hasDuplicates ? <BadgeLabel bsStyle="danger">!</BadgeLabel> : null }
                      </ColDisplay>
                    </Row>
                    <Row>
                      <ColDisplay labelProps={{ md: 6 }} label="Make">{ equipment.make }</ColDisplay>
                    </Row>
                    <Row>
                      <ColDisplay labelProps={{ md: 6 }} label="Model">{ equipment.model }</ColDisplay>
                    </Row>
                    <Row>
                      <ColDisplay labelProps={{ md: 6 }} label="Year">{ equipment.year }</ColDisplay>
                    </Row>
                    <Row>&nbsp;</Row>
                  </Col>
                  <Col md={8}>
                    <Row>&nbsp;</Row>
                    <Row>
                      <ColDisplay labelProps={{ md: 4 }} label="Size">{ equipment.size }</ColDisplay>
                    </Row>
                    <Row>
                      <ColDisplay labelProps={{ md: 4 }} label="Type">{ equipment.typeName }</ColDisplay>
                    </Row>
                    <Row>
                      <ColDisplay labelProps={{ md: 4 }} label="Licence Number">{ equipment.licencePlate }</ColDisplay>
                    </Row>
                    <Row>
                      <ColDisplay labelProps={{ md: 4 }} label="Operator">{ equipment.operator }</ColDisplay>
                    </Row>
                  </Col>
                </Row>;
              })()}
            </Well>
          </Col>
          <Col md={6}>
            <Well>
              <h3>Attachments <span className="pull-right">
                <Unimplemented>
                  <Button title="Add Attachment" bsSize="small" onClick={this.addPhysicalAttachment}><Glyphicon glyph="plus" /></Button>
                </Unimplemented>
              </span></h3>
              {(() => {
                if (this.state.loadingPhysicalAttachments ) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }
                if (Object.keys(this.props.equipmentPhysicalAttachments).length === 0) { return <Alert bsStyle="success" style={{ marginTop: 10 }}>No Attachments</Alert>; }

                var physicalAttachments = _.sortBy(this.props.equipmentPhysicalAttachments, this.state.ui.sortField);
                if (this.state.ui.sortDesc) {
                  _.reverse(physicalAttachments);
                }

                var headers = [
                  { field: 'attachmentTypeName', title: 'Type' },
                  { field: 'attachmentDescription', title: 'Description' },
                  { field: 'blank' },
                ];

                return <SortTable id="physical-attachment-list" sortField={ this.state.ui.sortField } sortDesc={ this.state.ui.sortDesc } onSort={ this.updateUIState } headers={ headers }>
                  {
                    _.map(physicalAttachments, (attachment) => {
                      return <tr key={ attachment.id }>
                        <td>{ attachment.typeName }</td>
                        <td>{ attachment.description }</td>
                        <td style={{ textAlign: 'right' }}>
                          <ButtonGroup>
                            <Unimplemented>
                              <Button className={ attachment.canEdit ? '' : 'hidden' } title="Edit Attachment" bsSize="xsmall" onClick={ this.openPhysicalAttachmentDialog.bind(this, attachment) }><Glyphicon glyph="pencil" /></Button>
                            </Unimplemented>
                            <Unimplemented>
                              <OverlayTrigger trigger="click" placement="top" rootClose overlay={ <Confirm onConfirm={ this.deletePhysicalAttachment.bind(this, attachment) }/> }>
                                <Button className={ attachment.canDelete ? '' : 'hidden' } title="Delete Attachment" bsSize="xsmall"><Glyphicon glyph="trash" /></Button>
                              </OverlayTrigger>
                            </Unimplemented>
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
          <Col md={6}>
            <Well>
              <h3>Seniority<span className="pull-right">
                <Button title="Edit Seniority" bsSize="small" onClick={this.openSeniorityDialog}><Glyphicon glyph="pencil" /></Button>
              </span></h3>
              {(() => {
                if (this.state.loadingSeniorityData) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

                var seniorityHistory = this.props.equipmentSeniorityHistory;  // TODO

                return <div>
                  <Row>
                    <ColDisplay md={12} labelProps={{ md: 4 }} label="Seniority">{ equipment.seniorityText }</ColDisplay>
                  </Row>
                  <Row>
                    <ColDisplay md={12} labelProps={{ md: 4 }} label="Hours YTD">{ equipment.serviceHoursThisYear }</ColDisplay>
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
          <Col md={6}>
            <Well>
              <h3>History <span className="pull-right">
                <Unimplemented>
                  <Button title="Add note" bsSize="small" onClick={this.addNote}><Glyphicon glyph="plus" /> Add Note</Button>
                </Unimplemented>
                <Unimplemented>
                  <Button title="Add document" bsSize="small" onClick={this.addDocument}><Glyphicon glyph="paperclip" /></Button>
                </Unimplemented>
              </span></h3>
              {(() => {
                if (this.state.loadingEquipmentHistory) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }
                if (Object.keys(this.props.history || []).length === 0) { return <Alert bsStyle="success" style={{ marginTop: 10 }}>No history</Alert>; }

                var history = _.sortBy(this.props.history, 'createdDate');

                const HistoryEntry = ({ createdDate, historyText }) => (
                  <Row>
                    <ColDisplay md={12} labelProps={{ md: 2 }} label={ formatDateTime(createdDate, Constant.DATE_YEAR_SHORT_MONTH_DAY) }>
                      { historyText }
                    </ColDisplay>
                  </Row>
                );

                return <div id="equipment-history">
                  {
                    _.map(history, (entry) => <HistoryEntry { ...entry } />)
                  }
                </div>;
              })()}
            </Well>
          </Col>
        </Row>
      </div>
      { this.state.showEditDialog &&
        <EquipmentEditDialog show={ this.state.showEditDialog } onSave={ this.saveEdit } onClose= { this.closeEditDialog } />
      }
      { this.state.showSeniorityDialog &&
        <SeniorityEditDialog show={ this.state.showSeniorityDialog } onSave={ this.saveSeniorityEdit } onClose= { this.closeSeniorityDialog } />
      }
    </div>;
  },
});


function mapStateToProps(state) {
  return {
    equipment: state.models.equipment,
    equipmentPhysicalAttachments: state.models.equipmentPhysicalAttachments,
    equipmentSeniorityHistory: state.models.equipmentSeniorityHistory,
    notes: state.models.equipmentNotes,
    attachments: state.models.equipmentAttachments,
    history: state.models.equipmentHistory,
    ui: state.ui.equipmentPhysicalAttachments,
  };
}

export default connect(mapStateToProps)(EquipmentDetail);
