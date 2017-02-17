import React from 'react';

import { connect } from 'react-redux';

import { Well, Row, Col } from 'react-bootstrap';
import { Alert, Button, ButtonGroup, Glyphicon, Label } from 'react-bootstrap';
import { Link } from 'react-router';
import { LinkContainer } from 'react-router-bootstrap';

import _ from 'lodash';

import * as Action from '../actionTypes';
import * as Api from '../api';
import store from '../store';

import CheckboxControl from '../components/CheckboxControl.jsx';
import ColField from '../components/ColField.jsx';
import ColLabel from '../components/ColLabel.jsx';
import Confirm from '../components/Confirm.jsx';
import OverlayTrigger from '../components/OverlayTrigger.jsx';
import SortTable from '../components/SortTable.jsx';
import Spinner from '../components/Spinner.jsx';

import { formatDateTime } from '../utils/date';
import { concat } from '../utils/string';

/*

TODO:
* Print / Verify / Contacts (TBD)

*/

var OwnersDetail = React.createClass({
  propTypes: {
    owner: React.PropTypes.object,
    notes: React.PropTypes.object,
    attachments: React.PropTypes.object,
    history: React.PropTypes.object,
    params: React.PropTypes.object,
    uiContacts: React.PropTypes.object,
    uiEquipment: React.PropTypes.object,
  },

  getInitialState() {
    return {
      loading: false,
      loadingOwnerHistory: false,

      showEditDialog: false,
      showContactDialog: false,
      showPolicyDialog: false,
      showPolicyDocumentsDialog: false,

      contact: {},

      isNew: this.props.params.ownerId == 0,

      uiContacts : {
        // Contacts
        sortField: this.props.uiContacts.sortField || 'name',
        sortDesc: this.props.uiContacts.sortDesc != false, // defaults to true
      },

      uiEquipment : {
        // Equipment
        sortField: this.props.uiEquipment.sortField || 'equipCode',
        sortDesc: this.props.uiEquipment.sortDesc != false, // defaults to true
      },
    };
  },

  componentDidMount() {
    this.fetch();
  },

  fetch() {
    this.setState({ loading: true });
    Api.getOwner(this.props.params.ownerId).finally(() => {
      this.setState({ loading: false });
    });
  },

  updateContactsUIState(state, callback) {
    this.setState({ uiContacts: { ...this.state.uiContacts, ...state }}, () =>{
      store.dispatch({ type: Action.UPDATE_OWNER_CONTACTS_UI, ownerContacts: this.state.uiContacts });
      if (callback) { callback(); }
    });
  },

  updateEquipmentUIState(state, callback) {
    this.setState({ uiEquipment: { ...this.state.uiEquipment, ...state }}, () =>{
      store.dispatch({ type: Action.UPDATE_OWNER_EQUIPMENT_UI, ownerEquipment: this.state.uiEquipment });
      if (callback) { callback(); }
    });
  },

  showNotes() {

  },

  showDocuments() {

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

  saveEdit(owner) {
    Api.updateOwner(owner).finally(() => {
      this.closeEditDialog();
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

  addContact() {
    this.openContactDialog({ id: 0 });
  },

  deleteContact(contact) {
    // TODO
    return contact;   
  },

  saveContact() {
  },

  addEquipment() {

  },

  equipmentVerifyAll() {

  },

  equipmentVerify(equipment) {
    // TODO
    // equipment.lastVerifiedDate = today();
    // store.dispatch(...);
    return equipment;
  },

  openPolicyDialog() {
    this.setState({ showPolicyDialog: true });
  },
  
  closePolicyDialog() {
    this.setState({ showPolicyDialog: false });
  },

  openPolicyDocumentsDialog() {
    this.setState({ showPolicyDocumentsDialog: true });
  },

  closePolicyDocumentsDialog() {
    this.setState({ showPolicyDocumentsDialog: false });
  },

  addPolicyDocument() {

  },

  print() {

  },
  
  render() {
    var owner = this.props.owner;

    return <div id="owners-detail">
      <div>
        <Row id="owners-top">
          <Col md={10}>
            <Label bsStyle={ owner.isApproved ? 'success' : 'danger'}>{ owner.status }</Label>
            <Label className={ owner.isMaintenanceContractor ? '' : 'hide' }>Maintenance Contractor</Label>
            <Button title="Notes" onClick={ this.showNotes }>Notes ({ Object.keys(this.props.notes).length })</Button>
            <Button title="Documents" onClick={ this.showDocuments }>Docs ({ Object.keys(this.props.attachments).length })</Button>
          </Col>
          <Col md={2}>
            <div className="pull-right">
              <Button onClick={ this.print }><Glyphicon glyph="print" title="Print" /></Button>
              <LinkContainer to={{ pathname: 'owners' }}>
                <Button title="Return to List"><Glyphicon glyph="arrow-left" /> Return to List</Button>
              </LinkContainer>
            </div>
          </Col>
        </Row>

        {(() => {
          if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

          return <div id="owners-header">
            <Row>
              <Col md={12}>
                <h1>Company: <small>{ owner.organizationName }</small></h1>
              </Col>
            </Row>
          </div>;
        })()}

        <Row>
          <Col md={6}>
            <Well>
              <h3>Owner Information <span className="pull-right">
                <Button title="Edit Owner" bsSize="small" onClick={ this.openEditDialog }><Glyphicon glyph="edit" /></Button>
              </span></h3>              
              {(() => {
                if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

                return <div id="owners-data">
                  <Row>
                    <ColLabel md={4}>Company</ColLabel>
                    <ColField md={8}>{ owner.organizationName }</ColField>
                  </Row>
                  <Row>
                    <ColLabel md={4}>Equipment Prefix</ColLabel>
                    <ColField md={8}>{ owner.ownerEquipmentCodePrefix }</ColField>
                  </Row>
                  <Row>
                    <ColLabel md={4}>Primary Contact</ColLabel>
                    <ColField md={8}>{ owner.primaryContactName }</ColField>
                  </Row>
                  <Row>
                    <ColLabel md={4}>Doing Business As</ColLabel>
                    <ColField md={8}>{ owner.doingBusinessAs }</ColField>
                  </Row>
                  <Row>
                    <ColLabel md={4}>Registered BC Company Number</ColLabel>
                    <ColField md={8}>{ owner.registeredCompanyNumber }</ColField>
                  </Row>
                  <Row>
                    <ColLabel md={4}>District Office</ColLabel>
                    <ColField md={8}>{ owner.districtName }</ColField>
                  </Row>
                  <Row>
                    <ColLabel md={4}>Service/Local Area</ColLabel>
                    <ColField md={8}>{ owner.localAreaName }</ColField>
                  </Row>
                  <Row>
                    <ColLabel md={4}>Meets Residency?</ColLabel>
                    <ColField md={8}><CheckboxControl checked={ owner.meetsResidency } disabled></CheckboxControl></ColField>
                  </Row>
                </div>;
              })()}
            </Well>            
            <Well>
              <h3>Equipment ({ owner.numberOfEquipment }) <span className="pull-right">
                <Button title="Verify All" bsSize="small" onClick={ this.equipmentVerifyAll }>Verify All</Button>
                <Button title="Add Equipment" bsSize="small" onClick={ this.addEquipment }><Glyphicon glyph="plus" /></Button>
              </span></h3>
              {(() => {
                if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }
                if (!owner.equipmentList || owner.equipmentList.length === 0) { return <Alert bsStyle="success" style={{ marginTop: 10 }}>No equipment</Alert>; }

                var equipmentList = _.sortBy(owner.equipmentList, this.state.uiEquipment.sortField);
                if (this.state.uiEquipment.sortDesc) {
                  _.reverse(equipmentList);
                }

                var headers = [
                  { field: 'equipCode',        title: 'ID'                  },
                  { field: 'typeName',         title: 'Type'                },
                  { field: 'make',             title: 'Make/Model/Size' },
                  { field: 'lastVerifiedDate', title: 'Last Verified'       },
                  { field: 'blank' },
                ];

                return <SortTable id="equipment-list" sortField={ this.state.uiEquipment.sortField } sortDesc={ this.state.uiEquipment.sortDesc } onSort={ this.updateEquipmentUIState } headers={ headers }>
                  {
                    _.map(equipmentList, (equipment) => {
                      return <tr key={ equipment.id }>
                        <td><Link to={`equipment/${equipment.id}`}>{ equipment.equipCode }</Link></td>
                        <td>{ equipment.typeName }</td>
                        <td>{ concat(equipment.make, concat(equipment.model, equipment.size, '/'), '/') }</td>
                        <td>{ equipment.isApproved ? formatDateTime(equipment.lastVerifiedDate, 'YYYY-MMM-DD') : 'Not Approved' }</td>
                        <td style={{ textAlign: 'right' }}>
                          <Button title="Verify Equipment" bsSize="xsmall" onClick={ this.equipmentVerify.bind(this, equipment) }><Glyphicon glyph="ok" /> OK</Button>
                        </td>
                      </tr>;
                    })
                  }
                </SortTable>;
              })()}
            </Well>
          </Col>
          <Col md={6}>
            <Well>        
              <h3>Policy <span className="pull-right">
                <Button title="Proof Documents" bsSize="small" onClick={ this.openPolicyDocumentsDialog }>Proof Documents ({ owner.numberOfPolicyDocuments })</Button>
                <Button title="Add Policy Document" onClick={ this.addPolicyDocument } bsSize="small"><Glyphicon glyph="plus" /> Add Policy Doc</Button>
                <Button title="Edit Policy Information" bsSize="small" onClick={ this.openPolicyDialog }><Glyphicon glyph="edit" /></Button>
              </span></h3>
              {(() => {
                if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

                return <div id="owners-policy">
                  <Row>
                    <ColLabel md={4}>WCB Expiry Date</ColLabel>
                    <ColField md={4}>{ formatDateTime(owner.wcbExpiryDate, 'YYYY-MMM-DD') }</ColField>
                    <ColField md={4}>
                      
                    </ColField>
                  </Row>
                  <Row>
                    <ColLabel md={4}>CGL Policy End Date</ColLabel>
                    <ColField md={4}>{ formatDateTime(owner.cglEndDate, 'YYYY-MMM-DD') }</ColField>
                  </Row>
                </div>;
              })()}
            </Well>
            <Well>
              <h3>Contacts <span className="pull-right">
                <Button title="Add Contact" onClick={ this.addContact } bsSize="small"><Glyphicon glyph="plus" /></Button>
              </span></h3>
              {(() => {
                if (this.state.loading ) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }
                if (!owner.contacts || owner.contacts.length === 0) { return <Alert bsStyle="success" style={{ marginTop: 10 }}>No contacts</Alert>; }

                var contacts = _.sortBy(owner.contacts, this.state.uiContacts.sortField);
                if (this.state.uiContacts.sortDesc) {
                  _.reverse(contacts);
                }
                
                // TODO The Contact model will be simplified (TBD)
                
                var headers = [
                  { field: 'name',  title: 'Name'         },
                  { field: 'phone', title: 'Phone Number' },
                  { field: 'email', title: 'Email'        },
                  { field: 'role',  title: 'Role'         },
                  { field: 'blank' },
                ];

                return <SortTable id="contact-list" sortField={ this.state.uiContacts.sortField } sortDesc={ this.state.uiContacts.sortDesc } onSort={ this.updateContactsUIState } headers={ headers }>
                  {
                    _.map(contacts, (contact) => {
                      return <tr key={ contact.id }>
                        <td>{ contact.name }</td>
                        <td>{ contact.phone }</td>
                        <td>{ contact.email }</td>
                        <td>{ contact.role }</td>
                        <td style={{ textAlign: 'right' }}>
                          <ButtonGroup>
                            <Button className={ contact.canEdit ? '' : 'hidden' } title="editContact" bsSize="xsmall" onClick={ this.openContactDialog.bind(this, contact) }><Glyphicon glyph="pencil" /></Button>
                            <OverlayTrigger trigger="click" placement="top" rootClose overlay={ <Confirm onConfirm={ this.deleteContact.bind(this, contact) }/> }>
                              <Button className={ contact.canDelete ? '' : 'hidden' } title="deleteContact" bsSize="xsmall"><Glyphicon glyph="trash" /></Button>
                            </OverlayTrigger>
                          </ButtonGroup>
                        </td>
                      </tr>;
                    })
                  }
                </SortTable>;
              })()}
            </Well>            
            <Well>
              <h3>History <span className="pull-right">
                <Button title="Add note" bsSize="small" onClick={ this.addNote }><Glyphicon glyph="plus" /> Add Note</Button>
                <Button title="Add document" bsSize="small" onClick={ this.addDocument }><Glyphicon glyph="paperclip" /></Button>
              </span></h3>
              {(() => {
                if (this.state.loadingOwnerHistory) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }
                if (Object.keys(this.props.history || []).length === 0) { return <Alert bsStyle="success" style={{ marginTop: 10 }}>No history</Alert>; }

                var history = _.sortBy(this.props.history, 'createdDate');    

                return <div id="owner-history">
                  {
                    _.map(history, (entry) => {
                      return <Row>
                        <ColLabel md={2}>{ formatDateTime(entry.createdDate, 'YYYY-MMM-DD') }</ColLabel>
                        <ColField md={10}>{ entry.historyText }</ColField>
                      </Row>;
                    })
                  }
                </div>;
              })()}
            </Well>
          </Col>
        </Row>
      </div>
      { /* TODO this.state.showEditDialog && <OwnerEditDialog /> */}
      { /* TODO this.state.showContactDialog && <ContactEditDialog /> */}
      { /* TODO this.state.showPolicyDialog && <OwnerPolicyDialog /> */}
      { /* TODO this.state.showPolicyDocumentsDialog && <OwnerPolicyDocumentsDialog /> */}
    </div>;
  },
});


function mapStateToProps(state) {
  return {
    owner: state.models.owner,
    notes: state.models.ownerNotes,
    attachments: state.models.ownerAttachments,
    history: state.models.ownerHistory,
    uiContacts: state.ui.ownerContacts,
    uiEquipment: state.ui.ownerEquipment,
  };
}

export default connect(mapStateToProps)(OwnersDetail);
