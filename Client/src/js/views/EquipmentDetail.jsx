import React from 'react';

import { connect } from 'react-redux';

import { Well, Row, Col } from 'react-bootstrap';
import { Alert, Button, ButtonGroup, Glyphicon, Label, DropdownButton, MenuItem, Checkbox } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';

import _ from 'lodash';

import * as Action from '../actionTypes';
import * as Api from '../api';
import store from '../store';

import BadgeLabel from '../components/BadgeLabel.jsx';
import ColField from '../components/ColField.jsx';
import ColLabel from '../components/ColLabel.jsx';
import Confirm from '../components/Confirm.jsx';
import OverlayTrigger from '../components/OverlayTrigger.jsx';
import SortTable from '../components/SortTable.jsx';
import Spinner from '../components/Spinner.jsx';

import { formatDateTime } from '../utils/date';

// Action drop-down items
const EQUIPMENT_ACTION_ARCHIVE = 'Archive';
const EQUIPMENT_ACTION_VERIFIED = 'Verified';
const EQUIPMENT_ACTION_FOR_HIRE = 'For Hire';

var EquipmentDetail = React.createClass({
  propTypes: {
    equipment: React.PropTypes.object,
    physicalAttachments: React.PropTypes.object,
    notes: React.PropTypes.object,
    attachments: React.PropTypes.object,
    params: React.PropTypes.object,
    ui: React.PropTypes.object,
  },

  getInitialState() {
    return {
      loadingEquipment: false,
      loadingPhysicalAttachments: false,
      loadingSeniorityData: false,
      loadingEquipmentHistory: false,

      showEditDialog: false,
      showPhysicalAttachmentDialog: false,

      physicalAttachment: {},

      ui : {
        // Physical Attachments
        sortField: this.props.ui.sortField || 'attachmentTypeName',
        sortDesc: this.props.ui.sortDesc != false, // defaults to true
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
    Api.getEquipment(equipId).finally(() => {
      this.setState({ loadingEquipment: false });
    });
  },

  showNotes() {
  },

  showAttachments() {
  },

  showHistory() {
  },

  print() {
    // TODO Implement
  },

  updateUIState(state, callback) {
    this.setState({ ui: { ...this.state.ui, ...state }}, () =>{
      store.dispatch({ type: Action.UPDATE_PHYSICAL_ATTACHMENTS_UI, physicalAttachments: this.state.ui });
      if (callback) { callback(); }
    });
  },

  actionSelected(/*eventKey*/) {
    // TODO Implement
  },

  registeredInAreaChanged(/*e*/) {
  },

  openEditDialog() {
    this.setState({ showEditDialog: true });
  },

  closeEditDialog() {
    this.setState({ showEditDialog: false });
  },

  openPhysicalAttachmentDialog(attachment) {
    this.setState({
      physicalAttachment: attachment,
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
    this.openInspectionDialog(newAttachment);
  },

  deletePhysicalAttachment(attachment) {
    Api.deletePhysicalAttachment(attachment).then(() => {
      // TODO Refresh attachment list
    });
  },

  render() {
    var equipment = this.props.equipment;
    var equipmentStatus = equipment.statusCd || 'New';
    var lastVerifiedStyle = 'danger'; // TODO Needs clarification

    return <div id="equipment-detail">
      <div>
        <Row id="equipment-top">
          <Col md={8}>
            <Label bsStyle={ equipment.isApproved ? 'success' : 'danger'}>{ equipmentStatus }</Label>
            <Label className={ equipment.isMaintenanceContractor ? '' : 'hide' }>Maintenance Contractor</Label>
            <Label bsStyle={ equipment.isWorking ? 'danger' : 'success' }>{ equipment.isWorking ? 'Working' : 'Not Working' }</Label>
            <Label bsStyle={ lastVerifiedStyle }>Last Verified: { formatDateTime(equipment.lastVerifiedDate, 'YYYY-MMM-DD') }</Label>
            <Button title="Notes" onClick={ this.showNotes }>Notes ({ Object.keys(this.props.notes).length })</Button>
            <Button title="Attachments" onClick={ this.showAttachments }>Docs ({ Object.keys(this.props.attachments).length })</Button>
          </Col>
          <Col md={4}>
            <div className="pull-right">
              <DropdownButton id='equipment-action-drop-down' title='Actions' onSelect={ this.actionSelected }>
                <MenuItem key={ EQUIPMENT_ACTION_ARCHIVE } eventKey={ EQUIPMENT_ACTION_ARCHIVE }>{ EQUIPMENT_ACTION_ARCHIVE }</MenuItem>
                <MenuItem key={ EQUIPMENT_ACTION_VERIFIED } eventKey={ EQUIPMENT_ACTION_VERIFIED }>{ EQUIPMENT_ACTION_VERIFIED }</MenuItem>
                <MenuItem key={ EQUIPMENT_ACTION_FOR_HIRE } eventKey={ EQUIPMENT_ACTION_FOR_HIRE }>{ EQUIPMENT_ACTION_FOR_HIRE }</MenuItem>
              </DropdownButton>
              <Button><Glyphicon glyph="print" title="Print" /></Button>
              <LinkContainer to={{ pathname: 'equipment' }}>
                <Button title="Return to List"><Glyphicon glyph="arrow-left" /> Return to List</Button>
              </LinkContainer>
            </div>
          </Col>
        </Row>

        {(() => {
          if (this.state.loadingEquipment) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

          return <div id="equipment-header">
            <Row>
              <ColLabel md={2}><h1>Company:</h1></ColLabel>
              <ColField md={10}><h1><small>{ equipment.ownerName }</small></h1></ColField>
            </Row>
            <Row>
              <ColLabel md={2}><h1>EquipId:</h1></ColLabel>
              <ColField md={10}><h1><small>{ equipment.equipCd } ({ equipment.typeName })</small></h1></ColField>
            </Row>
            <Row>
              <Col md={6}>
                <Row>
                  <ColLabel md={4}>District Office:</ColLabel>
                  <ColField md={8}>{ equipment.districtName }</ColField>
                </Row>
                <Row>
                  <ColLabel md={4}>Service/Local Area:</ColLabel>
                  <ColField md={8}>{ equipment.localAreaName }</ColField>
                </Row>
              </Col>
              <Col md={6}>
                <Checkbox onChange={this.registeredInAreaChanged}>Have you registered this piece of equipment in this area before?</Checkbox>
              </Col>
            </Row>
          </div>;
        })()}

        <Row>
          <Col md={6}>
            <Well>
              <h3>Equipment Information <span className="pull-right"><Button title="Edit" bsSize="small" onClick={ this.openEditDialog }><Glyphicon glyph="edit" /></Button></span></h3>
              {(() => {
                if (this.state.loadingEquipment) { return <div style={{ textAlign: 'center' }}><Spinner /></div>; }

                return <div>
                  <Row>
                    <ColLabel md={2}>Serial Number</ColLabel>
                    <ColField md={10}>{ equipment.serialNum || 'N/A' }
                      { equipment.hasDuplicates ? <BadgeLabel bsStyle="danger">!</BadgeLabel> : null } 
                    </ColField>
                  </Row>
                  <Row>
                    <ColLabel md={2}>Make</ColLabel>
                    <ColField md={4}>{ equipment.make }</ColField>
                    <ColLabel md={2}>Size</ColLabel>
                    <ColField md={4}>{ equipment.size }</ColField>
                  </Row>
                  <Row>
                    <ColLabel md={2}>Model</ColLabel>
                    <ColField md={4}>{ equipment.model }</ColField>
                    <ColLabel md={2}>Type</ColLabel>
                    <ColField md={4}>{ equipment.typeName }</ColField>
                  </Row>
                  <Row>
                    <ColLabel md={2}>Year</ColLabel>
                    <ColField md={2}>{ equipment.year }</ColField>
                    <ColLabel md={4}>Licence Number</ColLabel>
                    <ColField md={4}>{ equipment.licence || 'N/A' }</ColField>
                  </Row>
                  <Row>
                    <ColLabel md={6}></ColLabel>
                    <ColLabel md={2}>Operator</ColLabel>
                    <ColField md={4}>{ equipment.operator || 'N/A' }</ColField>
                  </Row>
                </div>;
              })()}
            </Well>
          </Col>
          <Col md={6}>
            <Well>
              <h3>Attachments <span className="pull-right"><Button title="Add Attachment" bsSize="small" onClick={this.addPhysicalAttachment}><Glyphicon glyph="plus" /></Button></span></h3>
              {(() => {
                if (this.state.loadingPhysicalAttachments ) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }
                if (Object.keys(this.props.physicalAttachments).length === 0) { return <Alert bsStyle="success" style={{ marginTop: 10 }}>No Attachments</Alert>; }

                var physicalAttachments = _.sortBy(this.props.physicalAttachments, this.state.ui.sortField);
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
                            <Button className={ attachment.canEdit ? '' : 'hidden' } title="Edit Attachment" bsSize="xsmall" onClick={ this.openPhysicalAttachmentDialog.bind(this, attachment) }><Glyphicon glyph="pencil" /></Button>
                            <OverlayTrigger trigger="click" placement="top" rootClose overlay={ <Confirm onConfirm={ this.deletePhysicalAttachment.bind(this, attachment) }/> }>
                              <Button className={ attachment.canDelete ? '' : 'hidden' } title="Delete Attachment" bsSize="xsmall"><Glyphicon glyph="trash" /></Button>
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
          <Col md={6}>
            <Well>
              <h3>Seniority Data <span className="pull-right"><Button title="Edit" bsSize="small" onClick={this.openEditDialog}><Glyphicon glyph="edit" /></Button></span></h3>
              {(() => {
                if (this.state.loadingSeniorityData) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

              })()}
            </Well>
          </Col>
          <Col md={6}>
            <Well>
              <h3>History</h3>
              {(() => {
                if (this.state.loadingEquipmentHistory) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

              })()}
            </Well>
          </Col>
        </Row>
      </div>
    </div>;
  },
});


function mapStateToProps(state) {
  return {
    equipment: state.models.equipment,
    physicalAttachments: state.models.physicalAttachments,
    equipmentHistory: state.models.equipmentHistory,
    attachments: state.models.equipmentAttachments,
    notes: state.models.equipmentNotes,
    ui: state.ui.physicalAttachments,
  };
}

export default connect(mapStateToProps)(EquipmentDetail);
