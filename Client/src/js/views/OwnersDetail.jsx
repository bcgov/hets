import React from 'react';

import { connect } from 'react-redux';

import { Well, Row, Col } from 'react-bootstrap';
import { Alert, Button, ButtonGroup, Glyphicon, Label } from 'react-bootstrap';
import { Link } from 'react-router';
import { LinkContainer } from 'react-router-bootstrap';

import _ from 'lodash';

import EquipmentAddDialog from './dialogs/EquipmentAddDialog.jsx';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import store from '../store';

import CheckboxControl from '../components/CheckboxControl.jsx';
import ColDisplay from '../components/ColDisplay.jsx';
import Confirm from '../components/Confirm.jsx';
import OverlayTrigger from '../components/OverlayTrigger.jsx';
import SortTable from '../components/SortTable.jsx';
import Spinner from '../components/Spinner.jsx';
import Unimplemented from '../components/Unimplemented.jsx';

import { formatDateTime } from '../utils/date';
import { concat } from '../utils/string';

/*

TODO:
* Print / Notes / Docs / Contacts (TBD) / Owner History
* Proof Documents / Policy Data / Edit Owner dialog

*/

var OwnersDetail = React.createClass({
  propTypes: {
    owner: React.PropTypes.object,
    equipment: React.PropTypes.object,
    notes: React.PropTypes.object,
    attachments: React.PropTypes.object,
    history: React.PropTypes.object,
    params: React.PropTypes.object,
    uiContacts: React.PropTypes.object,
    uiEquipment: React.PropTypes.object,
    router: React.PropTypes.object,
  },

  getInitialState() {
    return {
      loading: false,
      loadingOwnerHistory: false,

      showEditDialog: false,
      showContactDialog: false,
      showPolicyDialog: false,
      showPolicyDocumentsDialog: false,
      showEquipmentDialog: false,

      contact: {},

      isNew: this.props.params.ownerId == 0,

      uiContacts : {
        // Contacts
        sortField: this.props.uiContacts.sortField || 'name',
        sortDesc: this.props.uiContacts.sortDesc != false, // defaults to true
      },

      uiEquipment : {
        // Equipment
        sortField: this.props.uiEquipment.sortField || 'equipmentCode',
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
    this.setState({ uiContacts: { ...this.state.uiContacts, ...state }}, () => {
      store.dispatch({ type: Action.UPDATE_OWNER_CONTACTS_UI, ownerContacts: this.state.uiContacts });
      if (callback) { callback(); }
    });
  },

  updateEquipmentUIState(state, callback) {
    this.setState({ uiEquipment: { ...this.state.uiEquipment, ...state }}, () => {
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
    // TODO Edit owner data
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
    // TODO Delete contacts
    return contact;   
  },

  saveContact() {
    // TODO Save contact
  },

  openEquipmentDialog() {
    this.setState({ showEquipmentDialog: true });
  },

  closeEquipmentDialog() {
    this.setState({ showEquipmentDialog: false });
  },

  saveNewEquipment(equipment) {
    Api.addEquipment(equipment).then(() => {
      // Ensure the owner information is pre-populated for this new equipment
      var owner = this.props.owner;
      store.dispatch({ type: Action.UPDATE_OWNER, owner: owner });
      store.dispatch({ type: Action.UPDATE_RETURN_URL, returnUrl: `${Constant.OWNERS_PATHNAME}/${owner.id}` });

      // Open it up
      this.props.router.push({
        pathname: `${Constant.EQUIPMENT_PATHNAME}/${this.props.equipment.id}`,
      });
    });
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
    // TODO Edit policy data
    this.setState({ showPolicyDialog: true });
  },
  
  closePolicyDialog() {
    this.setState({ showPolicyDialog: false });
  },

  openPolicyDocumentsDialog() {
    // TODO Show popup with links to policy documents
    this.setState({ showPolicyDocumentsDialog: true });
  },

  closePolicyDocumentsDialog() {
    this.setState({ showPolicyDocumentsDialog: false });
  },

  addPolicyDocument() {
    // TODO Upload policy document (proof of policy coverage)
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
            <Unimplemented>
              <Button title="Notes" onClick={ this.showNotes }>Notes ({ Object.keys(this.props.notes).length })</Button>
            </Unimplemented>
            <Unimplemented>
              <Button title="Documents" onClick={ this.showDocuments }>Docs ({ Object.keys(this.props.attachments).length })</Button>
            </Unimplemented>
          </Col>
          <Col md={2}>
            <div className="pull-right">
              <Unimplemented>
                <Button onClick={ this.print }><Glyphicon glyph="print" title="Print" /></Button>
              </Unimplemented>
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
                <Unimplemented>
                  <Button title="Edit Owner" bsSize="small" onClick={ this.openEditDialog }><Glyphicon glyph="pencil" /></Button>
                </Unimplemented>
              </span></h3>              
              {(() => {
                if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

                return <div id="owners-data">
                  <Row>
                    <ColDisplay md={12} labelProps={{ md: 4 }} label="Company">{ owner.organizationName }</ColDisplay>
                  </Row>
                  <Row>
                    <ColDisplay md={12} labelProps={{ md: 4 }} label="Equipment Prefix">{ owner.ownerEquipmentCodePrefix }</ColDisplay>
                  </Row>
                  <Row>
                    <ColDisplay md={12} labelProps={{ md: 4 }} label="Primary Contact">{ owner.primaryContactName }</ColDisplay>
                  </Row>
                  <Row>
                    <ColDisplay md={12} labelProps={{ md: 4 }} label="Doing Business As">{ owner.doingBusinessAs }</ColDisplay>
                  </Row>
                  <Row>
                    <ColDisplay md={12} labelProps={{ md: 4 }} label="Registered BC Company Number">{ owner.registeredCompanyNumber }</ColDisplay>
                  </Row>
                  <Row>
                    <ColDisplay md={12} labelProps={{ md: 4 }} label="District Office">{ owner.districtName }</ColDisplay>
                  </Row>
                  <Row>
                    <ColDisplay md={12} labelProps={{ md: 4 }} label="Service/Local Area">{ owner.localAreaName }</ColDisplay>
                  </Row>
                  <Row>
                    <ColDisplay md={12} labelProps={{ md: 4 }} label="Meets Residency?"><CheckboxControl checked={ owner.meetsResidency } disabled /></ColDisplay>
                  </Row>
                </div>;
              })()}
            </Well>            
            <Well>
              <h3>Equipment ({ owner.numberOfEquipment }) <span className="pull-right">
                <Unimplemented>
                  <Button title="Verify All Equipment" bsSize="small" onClick={ this.equipmentVerifyAll }>Verify All</Button>
                </Unimplemented>
                <Button title="Add Equipment" bsSize="small" onClick={ this.openEquipmentDialog }><Glyphicon glyph="plus" /></Button>
              </span></h3>
              {(() => {
                if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }
                if (!owner.equipmentList || owner.equipmentList.length === 0) { return <Alert bsStyle="success" style={{ marginTop: 10 }}>No equipment</Alert>; }

                var equipmentList = _.sortBy(owner.equipmentList, this.state.uiEquipment.sortField);
                if (this.state.uiEquipment.sortDesc) {
                  _.reverse(equipmentList);
                }

                var headers = [
                  { field: 'equipmentCode',    title: 'ID'                  },
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
                        <td>{ equipment.isApproved ? formatDateTime(equipment.lastVerifiedDate, Constant.DATE_YEAR_SHORT_MONTH_DAY) : 'Not Approved' }</td>
                        <td style={{ textAlign: 'right' }}>
                          <Unimplemented>
                            <Button title="Verify Equipment" bsSize="xsmall" onClick={ this.equipmentVerify.bind(this, equipment) }><Glyphicon glyph="ok" /> OK</Button>
                          </Unimplemented>
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
                <Unimplemented>
                  <Button title="Show Proof Documents" bsSize="small" onClick={ this.openPolicyDocumentsDialog }>Proof Documents ({ owner.numberOfPolicyDocuments })</Button>
                </Unimplemented>
                <Unimplemented>
                  <Button title="Add Policy Document" onClick={ this.addPolicyDocument } bsSize="small"><Glyphicon glyph="plus" /> Attach Proof</Button>
                </Unimplemented>
                <Unimplemented>
                  <Button title="Edit Policy Information" bsSize="small" onClick={ this.openPolicyDialog }><Glyphicon glyph="pencil" /></Button>
                </Unimplemented>
              </span></h3>
              {(() => {
                if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

                return <div id="owners-policy">
                  <Row>
                    <ColDisplay md={12} labelProps={{ md: 4 }} label="WorkSafeBC Policy">{ owner.workSafeBCPolicyNumber }</ColDisplay>
                  </Row>
                  <Row>
                    <ColDisplay md={12} labelProps={{ md: 4 }} label="WorkSafeBC Expiry Date">
                      { formatDateTime(owner.workSafeBCExpiryDate, Constant.DATE_YEAR_SHORT_MONTH_DAY) }
                    </ColDisplay>
                  </Row>
                  <Row>
                    <ColDisplay md={12} labelProps={{ md: 4 }} label="CGL Policy End Date">
                      { formatDateTime(owner.cglEndDate, Constant.DATE_YEAR_SHORT_MONTH_DAY) }
                    </ColDisplay>
                  </Row>
                </div>;
              })()}
            </Well>
            <Well>
              <h3>Contacts <span className="pull-right">
                <Unimplemented>
                  <Button title="Add Contact" onClick={ this.addContact } bsSize="small"><Glyphicon glyph="plus" /></Button>
                </Unimplemented>
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
                            <Unimplemented>
                              <Button className={ contact.canEdit ? '' : 'hidden' } title="Edit Contact" bsSize="xsmall" onClick={ this.openContactDialog.bind(this, contact) }><Glyphicon glyph="pencil" /></Button>
                            </Unimplemented>
                            <Unimplemented>
                              <OverlayTrigger trigger="click" placement="top" rootClose overlay={ <Confirm onConfirm={ this.deleteContact.bind(this, contact) }/> }>
                                <Button className={ contact.canDelete ? '' : 'hidden' } title="Delete Contact" bsSize="xsmall"><Glyphicon glyph="trash" /></Button>
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
            <Well>
              <h3>History <span className="pull-right">
                <Unimplemented>
                  <Button title="Add note" bsSize="small" onClick={ this.addNote }><Glyphicon glyph="plus" /> Add Note</Button>
                </Unimplemented>
                <Unimplemented>
                  <Button title="Add document" bsSize="small" onClick={ this.addDocument }><Glyphicon glyph="paperclip" /></Button>
                </Unimplemented>
              </span></h3>
              {(() => {
                if (this.state.loadingOwnerHistory) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }
                if (Object.keys(this.props.history || []).length === 0) { return <Alert bsStyle="success" style={{ marginTop: 10 }}>No history</Alert>; }

                var history = _.sortBy(this.props.history, 'createdDate');    

                const HistoryEntry = ({ createdDate, historyText }) => (
                  <Row>
                    <ColDisplay md={12} labelProps={{ md: 2 }} label={ formatDateTime(createdDate, Constant.DATE_YEAR_SHORT_MONTH_DAY) }>
                      { historyText }
                    </ColDisplay>
                  </Row>
                );

                return <div id="owners-history">
                  {
                    _.map(history, (entry) => <HistoryEntry { ...entry } />)
                  }
                </div>;
              })()}
            </Well>
          </Col>
        </Row>
      </div>
      { this.state.showEquipmentDialog &&
        <EquipmentAddDialog show={ this.state.showEquipmentDialog } onSave={ this.saveNewEquipment } onClose={ this.closeEquipmentDialog } />
      }
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
    equipment: state.models.equipment,
    notes: state.models.ownerNotes,
    attachments: state.models.ownerAttachments,
    history: state.models.ownerHistory,
    uiContacts: state.ui.ownerContacts,
    uiEquipment: state.ui.ownerEquipment,
  };
}

export default connect(mapStateToProps)(OwnersDetail);
