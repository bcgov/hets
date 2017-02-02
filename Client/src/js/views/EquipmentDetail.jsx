import React from 'react';

import { connect } from 'react-redux';

import { Well, Row, Col } from 'react-bootstrap';
import { Button, Glyphicon, Label, DropdownButton, MenuItem } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';

import * as Api from '../api';

import ColField from '../components/ColField.jsx';
import ColLabel from '../components/ColLabel.jsx';
import Spinner from '../components/Spinner.jsx';

import { formatDateTime } from '../utils/date';

var EquipmentDetail = React.createClass({
  propTypes: {
    equipment: React.PropTypes.object,
    notes: React.PropTypes.object,
    attachments: React.PropTypes.object,
    params: React.PropTypes.object,
  },

  getInitialState() {
    return {
      loadingEquipment: false,
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

  actionSelected(/*eventKey*/) {
    // TODO Implement
  },

  openEditDialog() {
    //this.setState({ showEditDialog: true });
  },

  closeEditDialog() {
    //this.setState({ showEditDialog: false });
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
              <DropdownButton id='action-drop-down' title='Actions' onSelect={ this.actionSelected }>
                <MenuItem key={1} eventKey={1}>Action 1</MenuItem>
                <MenuItem key={2} eventKey={2}>Action 2</MenuItem>
                <MenuItem key={3} eventKey={3}>Action 3</MenuItem>
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
              <Col md={12}>
                <h1>Company: <small>{ equipment.ownerName }</small></h1>
              </Col>
            </Row>
            <Row>
              <Col md={12}>
                <h1>EquipId: <small>{ equipment.equipCd } ({ equipment.typeName })</small></h1>
              </Col>
            </Row>
          </div>;
        })()}

        <Row>
          <Col md={6}>
            <Well>
              <h3>Equipment Information <span className="pull-right"><Button title="edit" bsSize="small" onClick={ this.openEditDialog }><Glyphicon glyph="edit" /></Button></span></h3>
              {(() => {
                if (this.state.loadingEquipment) { return <div style={{ textAlign: 'center' }}><Spinner /></div>; }

                return <div>
                  <Row>
                    <ColLabel md={4}>Make</ColLabel>
                    <ColField md={8}>{ equipment.make }</ColField>
                  </Row>
                  <Row>
                    <ColLabel md={4}>Model</ColLabel>
                    <ColField md={8}>{ equipment.model }</ColField>
                  </Row>
                  <Row>
                    <ColLabel md={4}>Year</ColLabel>
                    <ColField md={8}>{ equipment.year }</ColField>
                  </Row>
                  <Row>
                    <ColLabel md={4}>Size</ColLabel>
                    <ColField md={8}>{ equipment.size }</ColField>
                  </Row>
                  <Row>
                    <ColLabel md={4}>Type</ColLabel>
                    <ColField md={8}>{ equipment.typeName }</ColField>
                  </Row>
                </div>;
              })()}
            </Well>
          </Col>
          <Col md={6}>
            <Well>
              <h3>Attachments <span className="pull-right"><Button title="edit" bsSize="small" onClick={this.openEditDialog}><Glyphicon glyph="edit" /></Button></span></h3>
            </Well>
          </Col>
        </Row>
        <Row>
          <Col md={6}>
            <Well>
              <h3>Seniority Data <span className="pull-right"><Button title="edit" bsSize="small" onClick={this.openEditDialog}><Glyphicon glyph="edit" /></Button></span></h3>
            </Well>
          </Col>
          <Col md={6}>
            <Well>
              <h3>History</h3>
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
    attachments: state.models.equipmentAttachments,
    notes: state.models.equipmentNotes,
  };
}

export default connect(mapStateToProps)(EquipmentDetail);
