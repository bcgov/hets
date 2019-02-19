import React from 'react';

import { connect } from 'react-redux';

import { browserHistory } from 'react-router';

import { Well, Row, Col, Alert, Button, ButtonGroup, Glyphicon, Label, DropdownButton, MenuItem } from 'react-bootstrap';
import { Link } from 'react-router';

import _ from 'lodash';
import Promise from 'bluebird';

import ContactsEditDialog from './dialogs/ContactsEditDialog.jsx';
import DocumentsListDialog from './dialogs/DocumentsListDialog.jsx';
import EquipmentAddDialog from './dialogs/EquipmentAddDialog.jsx';
import OwnersEditDialog from './dialogs/OwnersEditDialog.jsx';
import OwnersPolicyEditDialog from './dialogs/OwnersPolicyEditDialog.jsx';
import NotesDialog from './dialogs/NotesDialog.jsx';
import ChangeStatusDialog from './dialogs/ChangeStatusDialog.jsx';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import * as Log from '../history';
import store from '../store';

import CheckboxControl from '../components/CheckboxControl.jsx';
import ColDisplay from '../components/ColDisplay.jsx';
import DeleteButton from '../components/DeleteButton.jsx';
import EditButton from '../components/EditButton.jsx';
import History from '../components/History.jsx';
import SortTable from '../components/SortTable.jsx';
import Spinner from '../components/Spinner.jsx';
import TooltipButton from '../components/TooltipButton.jsx';
import Confirm from '../components/Confirm.jsx';
import OverlayTrigger from '../components/OverlayTrigger.jsx';

import PageHeader from '../components/ui/PageHeader.jsx';
import SubHeader from '../components/ui/SubHeader.jsx';

import { formatDateTime, today, toZuluTime } from '../utils/date';

/*

TODO:
* Print / Notes / Policy Proof Documents (attachments)

*/

var OwnersDetail = React.createClass({
  propTypes: {
    owner: React.PropTypes.object,
    equipment: React.PropTypes.object,
    contact: React.PropTypes.object,
    documents: React.PropTypes.object,
    params: React.PropTypes.object,
    uiContacts: React.PropTypes.object,
    uiEquipment: React.PropTypes.object,
    router: React.PropTypes.object,
    notes: React.PropTypes.object,
  },

  getInitialState() {
    return {
      loading: true,

      showEditDialog: false,
      showContactDialog: false,
      showPolicyDialog: false,
      showPolicyDocumentsDialog: false,
      showEquipmentDialog: false,
      showDocumentsDialog: false,
      showNotesDialog: false,
      showChangeStatusDialog: false,

      showAttachments: false,

      contact: {},

      status: '',

      // Contacts
      uiContacts : {
        sortField: this.props.uiContacts.sortField || 'name',
        sortDesc: this.props.uiContacts.sortDesc  === true,
      },

      // Equipment
      uiEquipment : {
        sortField: this.props.uiEquipment.sortField || 'equipmentNumber',
        sortDesc: this.props.uiEquipment.sortDesc  === true,
      },
    };
  },

  componentDidMount() {
    this.fetch();
  },

  fetch() {
    this.setState({ loading: true });

    var ownerId = this.props.params.ownerId;
    var ownerPromise = Api.getOwner(ownerId);
    var documentsPromise = Api.getOwnerDocuments(ownerId);
    var ownerNotesPromise = Api.getOwnerNotes(ownerId);

    return Promise.all([ownerPromise, documentsPromise, ownerNotesPromise]).finally(() => {
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

  updateState(state, callback) {
    this.setState(state, callback);
  },

  showNotes() {

  },

  updateStatusState(state) {
    if (state !== this.props.owner.status) {
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
    var currentStatus = this.props.owner.status;
    var equipmentList = { ...this.props.owner.equipmentList };
    return Api.changeOwnerStatus(status).then(() => {
      this.closeChangeStatusDialog();
      Log.ownerModifiedStatus(this.props.owner, status.status, status.statusComment);
      // If owner status goes from approved to unapproved/archived or unapproved to archived
      // this will change all it's equipment statuses. This should be reflected in the equipment history.
      if (
        (currentStatus === Constant.OWNER_STATUS_CODE_APPROVED || currentStatus === Constant.OWNER_STATUS_CODE_PENDING)
        && (status.status === Constant.OWNER_STATUS_CODE_PENDING || status.status === Constant.OWNER_STATUS_CODE_ARCHIVED)
      ) {
        _.map(equipmentList, equipment => {
          if (equipment.status !== status.status) {
            Log.equipmentStatusModified(equipment, status.status, status.statusComment);
          }
        });
      }
    });
  },

  showDocuments() {
    this.setState({ showDocumentsDialog: true });
  },

  closeDocumentsDialog() {
    this.setState({ showDocumentsDialog: false });
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
    // This just ensures that the normalized data doesn't mess up the PUT call
    Api.updateOwner({ ...owner, contacts: null }).finally(() => {
      Log.ownerModified(this.props.owner);
      this.closeEditDialog();
    });
  },

  openContactDialog(contactId) {
    var contact;
    if (contactId === 0) {
      // New
      contact = {
        id: 0,
        owner: this.props.owner,
      };
    } else if (contactId) {
      // Open the contact for viewing if possible
      contact = this.props.owner.contacts[contactId];
    }
    this.setState({
      contact: contact,
      showContactDialog: true,
    });
  },

  closeContactDialog() {
    this.setState({ showContactDialog: false });
  },

  deleteContact(contact) {
    Api.deleteContact(contact).then(() => {
      Log.ownerContactDeleted(this.props.owner, this.props.contact).then(() => {
        // In addition to refreshing the contacts, we need to update the owner
        // to get primary contact info and history.
        this.fetch();
      });
    });
  },

  saveContact(contact) {
    var isNew = !contact.id;
    var log = isNew ? Log.ownerContactAdded : Log.ownerContactUpdated;

    Api.addOwnerContact(this.props.owner, contact).then(() => {
      // Use this.props.contact to get the contact id
      return log(this.props.owner, this.props.contact);
    }).finally(() => {
      // In addition to refreshing the contacts, we need to update the owner
      // to get primary contact info and history.
      this.fetch();
      this.closeContactDialog();
    });
  },

  openEquipmentDialog() {
    this.setState({ showEquipmentDialog: true });
  },

  closeEquipmentDialog() {
    this.setState({ showEquipmentDialog: false });
  },

  saveNewEquipment(equipment) {
    return Api.addEquipment(equipment).then(() => {
      // Open it up
      Log.ownerEquipmentAdded(this.props.owner, this.props.equipment);
      Log.equipmentAdded(this.props.equipment);
      this.props.router.push({
        pathname: `${Constant.EQUIPMENT_PATHNAME}/${this.props.equipment.id}`,
        state: { returnUrl: `${Constant.OWNERS_PATHNAME}/${this.props.owner.id}` },
      });

      return null;
    });
  },

  equipmentVerifyAll() {
    var now = today();
    var owner = this.props.owner;

    // Update the last verified date on all pieces of equipment
    var equipmentList =_.map(owner.equipmentList, equipment => {
      return {...equipment, ...{
        lastVerifiedDate: toZuluTime(now),
        owner: { id: owner.id },
      }};
    });

    Api.updateOwnerEquipment(owner, equipmentList);
  },

  equipmentVerify(equipment) {
    Api.updateEquipment({...equipment, ...{
      lastVerifiedDate: toZuluTime(today()),
      owner: { id: this.props.owner.id },
    }}).then(() => {
      Log.ownerEquipmentVerified(this.props.owner, equipment);
      this.fetch();
    });
  },

  openPolicyDialog() {
    this.setState({ showPolicyDialog: true });
  },

  closePolicyDialog() {
    this.setState({ showPolicyDialog: false });
  },

  savePolicyEdit(owner) {
    // This just ensures that the normalized data doesn't mess up the PUT call
    Api.updateOwner({ ...owner, contacts: null }).finally(() => {
      Log.ownerModifiedPolicy(this.props.owner);
      this.closePolicyDialog();
    });
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

  openNotesDialog() {
    this.setState({ showNotesDialog: true });
  },

  closeNotesDialog() {
    this.setState({ showNotesDialog: false });
  },

  print() {
    window.print();
  },


  getStatusDropdownStyle() {
    switch(this.props.owner.status) {
      case(Constant.OWNER_STATUS_CODE_APPROVED):
        return 'success';
      case(Constant.OWNER_STATUS_CODE_PENDING):
        return 'danger';
      default:
        return 'default';
    }
  },

  render() {
    var owner = this.props.owner;
    var isApproved = this.props.owner.status === Constant.OWNER_STATUS_CODE_APPROVED;
    var restrictEquipmentAddTooltip = 'Equipment can only be added to an approved owner.';
    var restrictEquipmentVerifyTooltip = 'Equipment can only be verified for an approved owner.';

    return <div id="owners-detail">
      <div>
        {(() => {
          var dropdownItems = _.pull([ Constant.OWNER_STATUS_CODE_APPROVED, Constant.OWNER_STATUS_CODE_PENDING, Constant.OWNER_STATUS_CODE_ARCHIVED ], owner.status);

          if (this.state.loading) { return <div className="spinner-container"><Spinner/></div>; }

          return <Row id="owners-top">
            <Col sm={9}>
              <DropdownButton
                id="status-dropdown"
                bsStyle={ this.getStatusDropdownStyle() }
                title={ owner.status }
                onSelect={ this.updateStatusState }
              >
                { dropdownItems.map((item, i) => (
                  <MenuItem key={ i } eventKey={ item }>{ item }</MenuItem>
                ))}
              </DropdownButton>
              <Button className="ml-5 mr-5" title="Notes" onClick={ this.openNotesDialog }>Notes ({ Object.keys(this.props.notes).length })</Button>
              <Button title="Documents" onClick={ this.showDocuments }>Documents ({ Object.keys(this.props.documents).length })</Button>
              <Label className={ owner.isMaintenanceContractor ? 'ml-5' : 'hide' }>Maintenance Contractor</Label>
            </Col>
            <Col sm={3}>
              <div className="pull-right">
                {/* <DropdownControl id="status" title={ owner.status } updateState={ this.updateStatusState } staticTitle={true}
                  items={_.pull([ Constant.OWNER_STATUS_CODE_APPROVED, Constant.OWNER_STATUS_CODE_PENDING, Constant.OWNER_STATUS_CODE_ARCHIVED ], owner.status)} /> */}
                <Button className="mr-5" onClick={ this.print }><Glyphicon glyph="print" title="Print" /></Button>
                <Button title="Return" onClick={ browserHistory.goBack }><Glyphicon glyph="arrow-left" /> Return</Button>
              </div>
            </Col>
          </Row>;
        })()}

        <PageHeader id="owners-header" title="Company" subTitle={ owner.organizationName }/>

        <Row>
          <Col md={12}>
            <Well>
              <SubHeader title="Owner Information" editButtonTitle="Edit Owner" onEditClicked={ this.openEditDialog }/>
              {(() => {
                if (this.state.loading) { return <div className="spinner-container"><Spinner/></div>; }

                return <div id="owners-data">
                  <Row className="equal-height">
                    <Col lg={4} md={6} sm={12} xs={12}>
                      <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Company">{ owner.organizationName }</ColDisplay>
                    </Col>
                    <Col lg={4} md={6} sm={12} xs={12}>
                      <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Primary Contact">{ owner.primaryContactName }</ColDisplay>
                    </Col>
                    <Col lg={4} md={6} sm={12} xs={12}>
                      <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Owner Name">{ owner.ownerName }</ColDisplay>
                    </Col>
                    <Col lg={4} md={6} sm={12} xs={12}>
                      <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Owner Code">{ owner.ownerCode }</ColDisplay>
                    </Col>
                    <Col lg={4} md={6} sm={12} xs={12}>
                      <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="District Office">{ owner.districtName }</ColDisplay>
                    </Col>
                    <Col lg={4} md={6} sm={12} xs={12}>
                      <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Doing Business As">{ owner.doingBusinessAs }</ColDisplay>
                    </Col>
                    <Col lg={4} md={6} sm={12} xs={12}>
                      <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Registered BC Company Number">{ owner.registeredCompanyNumber }</ColDisplay>
                    </Col>
                    <Col lg={4} md={6} sm={12} xs={12}>
                      <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Meets Residency?">{ owner.meetsResidency ? 'Yes' : 'No' }</ColDisplay>
                    </Col>
                    <Col lg={4} md={6} sm={12} xs={12}>
                      <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Service/Local Area">{ owner.localAreaName }</ColDisplay>
                    </Col>
                    <Col lg={4} md={6} sm={12} xs={12}>
                      <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Maintenance Contractor">{ owner.isMaintenanceContractor ? 'Yes' : 'No' }</ColDisplay>
                    </Col>
                    <Col lg={4} md={6} sm={12} xs={12}>
                      <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Company Address">{ owner.address1 } { owner.address2 } <br/> { owner.city } { owner.province } { owner.postalCode }</ColDisplay>
                    </Col>
                  </Row>
                </div>;
              })()}
            </Well>
          </Col>
          <Col md={12}>
            <Well>
              <SubHeader title="Policy" editButtonTitle="Edit Policy Information" onEditClicked={ this.openPolicyDialog }/>
              {(() => {
                if (this.state.loading) { return <div className="spinner-container"><Spinner/></div>; }

                return <Row id="owners-policy" className="equal-height">
                  <Col lg={4} md={6} sm={12} xs={12}>
                    <ColDisplay labelProps={{ xs: 6 }} fieldProps={{ xs: 6 }} label="WCB Number">{ owner.workSafeBCPolicyNumber }</ColDisplay>
                  </Col>
                  <Col lg={4} md={6} sm={12} xs={12}>
                    <ColDisplay labelProps={{ xs: 6 }} fieldProps={{ xs: 6 }} label="WCB Expiry Date">
                      { formatDateTime(owner.workSafeBCExpiryDate, Constant.DATE_YEAR_SHORT_MONTH_DAY) }
                    </ColDisplay>
                  </Col>
                  <Col lg={4} md={6} sm={12} xs={12}>
                    <ColDisplay labelProps={{ xs: 6 }} fieldProps={{ xs: 6 }} label="CGL Insurance Company">
                      { owner.cglCompanyName }
                    </ColDisplay>
                  </Col>
                  <Col lg={4} md={6} sm={12} xs={12}>
                    <ColDisplay labelProps={{ xs: 6 }} fieldProps={{ xs: 6 }} label="CGL Policy Number">
                      { owner.cglPolicyNumber }
                    </ColDisplay>
                  </Col>
                  <Col lg={4} md={6} sm={12} xs={12}>
                    <ColDisplay labelProps={{ xs: 6 }} fieldProps={{ xs: 6 }} label="CGL Policy End Date">
                      { formatDateTime(owner.cglEndDate, Constant.DATE_YEAR_SHORT_MONTH_DAY) }
                    </ColDisplay>
                  </Col>
                </Row>;
              })()}
            </Well>
          </Col>
          <Col md={12}>
            <Well>
              <SubHeader title="Contacts"/>
              {(() => {
                if (this.state.loading ) { return <div className="spinner-container"><Spinner/></div>; }

                var addContactButton = <Button title="Add Contact" onClick={ this.openContactDialog.bind(this, 0) } bsSize="xsmall"><Glyphicon glyph="plus" />&nbsp;<strong>Add</strong></Button>;

                if (!owner.contacts || Object.keys(owner.contacts).length === 0) { return <Alert bsStyle="success">No contacts { addContactButton }</Alert>; }

                var contacts = _.sortBy(owner.contacts, this.state.uiContacts.sortField);
                if (this.state.uiContacts.sortDesc) {
                  _.reverse(contacts);
                }

                var headers = [
                  { field: 'name',              title: 'Name'  },
                  { field: 'phone',             title: 'Phone' },
                  { field: 'mobilePhoneNumber', title: 'Cell'  },
                  { field: 'faxPhoneNumber',    title: 'Fax'   },
                  { field: 'emailAddress',      title: 'Email' },
                  { field: 'role',              title: 'Role'  },
                  { field: 'notes',             title: 'Notes'  },
                  { field: 'addContact',        title: 'Add Contact', style: { textAlign: 'right'  },
                    node: addContactButton,
                  },
                ];

                return <SortTable id="contact-list" sortField={ this.state.uiContacts.sortField } sortDesc={ this.state.uiContacts.sortDesc } onSort={ this.updateContactsUIState } headers={ headers }>
                  {
                    _.map(contacts, (contact) => {
                      return <tr key={ contact.id }>
                        <td>{ contact.isPrimary && <Glyphicon glyph="star" /> } { contact.name }</td>
                        <td>{ contact.phone }</td>
                        <td>{ contact.mobilePhoneNumber }</td>
                        <td>{ contact.faxPhoneNumber }</td>
                        <td><a href={ `mailto:${ contact.emailAddress }` } target="_blank">{ contact.emailAddress }</a></td>
                        <td>{ contact.role }</td>
                        <td>{ contact.notes ? 'Y' : '' }</td>
                        <td style={{ textAlign: 'right' }}>
                          <ButtonGroup>
                            <DeleteButton name="Contact" hide={ !contact.canDelete || contact.isPrimary } onConfirm={ this.deleteContact.bind(this, contact) }/>
                            <EditButton name="Contact" view={ !contact.canEdit } onClick={ this.openContactDialog.bind(this, contact.id) } />
                          </ButtonGroup>
                        </td>
                      </tr>;
                    })
                  }
                </SortTable>;
              })()}
            </Well>
            <Well>
              <SubHeader title={`Equipment (${ owner.numberOfEquipment })`}>
                <CheckboxControl id="showAttachments" className="mr-5" inline updateState={this.updateState}><small>Show Attachments</small></CheckboxControl>
                <OverlayTrigger trigger="click" placement="top" rootClose overlay={ <Confirm onConfirm={ this.equipmentVerifyAll }></Confirm> }>
                  <TooltipButton disabled={!isApproved} disabledTooltip={restrictEquipmentVerifyTooltip} className="mr-5" title="Verify All Equipment" bsSize="small">Verify All</TooltipButton>
                </OverlayTrigger>
                <TooltipButton disabled={ !isApproved } disabledTooltip={ restrictEquipmentAddTooltip } title="Add Equipment" bsSize="small" onClick={ this.openEquipmentDialog }><Glyphicon glyph="plus" /></TooltipButton>
              </SubHeader>
              {(() => {
                if (this.state.loading) { return <div className="spinner-container"><Spinner/></div>; }

                if (!owner.equipmentList || owner.equipmentList.length === 0) { return <Alert bsStyle="success">No equipment</Alert>; }

                var equipmentList = _.orderBy(owner.equipmentList, [this.state.uiEquipment.sortField], [this.state.uiEquipment.sortDesc ? 'desc' : 'asc']);

                var headers = [
                  { field: 'equipmentNumber',  title: 'ID'                   },
                  { field: 'localArea.name',   title: 'Local Area'           },
                  { field: 'typeName',         title: 'Equipment Type'       },
                  { field: 'details',          title: 'Make/Model/Size/Year' },
                  { field: 'lastVerifiedDate', title: 'Last Verified'        },
                  { field: 'blank' },
                ];

                return <SortTable id="equipment-list" sortField={ this.state.uiEquipment.sortField } sortDesc={ this.state.uiEquipment.sortDesc } onSort={ this.updateEquipmentUIState } headers={ headers }>
                  {
                    _.map(equipmentList, (equipment) => {
                      const location = {
                        pathname: `${Constant.EQUIPMENT_PATHNAME}/${equipment.id}`,
                        state: { returnUrl: `${Constant.OWNERS_PATHNAME}/${owner.id}` },
                      };
                      return <tr key={ equipment.id }>
                        <td><Link to={ location }>{ equipment.equipmentCode }</Link></td>
                        <td>{ equipment.localArea.name }</td>
                        <td>{ equipment.typeName }</td>
                        <td>
                          { equipment.details }
                          { this.state.showAttachments &&
                            <div>
                              Attachments:
                              { equipment.equipmentAttachments && equipment.equipmentAttachments.map((item, i) => (
                                <span key={item.id}>
                                  <span> </span>
                                  <span className="attachment">{ item.typeName }
                                    { ((i + 1) < equipment.equipmentAttachments.length) &&
                                    <span>,</span>
                                    }
                                  </span>
                                </span>
                              ))}
                              { (!equipment.equipmentAttachments || equipment.equipmentAttachments.length === 0)  &&
                                <span> none</span>
                              }
                            </div>
                          }
                        </td>
                        <td>{ equipment.isApproved ? formatDateTime(equipment.lastVerifiedDate, Constant.DATE_YEAR_SHORT_MONTH_DAY) : 'Not Approved' }</td>
                        <td style={{ textAlign: 'right' }}>
                          <TooltipButton disabled={ !isApproved } disabledTooltip={ restrictEquipmentVerifyTooltip } title="Verify Equipment" bsSize="xsmall" onClick={ this.equipmentVerify.bind(this, equipment) }><Glyphicon glyph="ok" /> OK</TooltipButton>
                        </td>
                      </tr>;
                    })
                  }
                </SortTable>;
              })()}
            </Well>
            <Well>
              <SubHeader title="Contacts"/>
              { owner.historyEntity && <History historyEntity={ owner.historyEntity } refresh={ !this.state.loading } /> }
            </Well>
          </Col>
        </Row>
      </div>
      { this.state.showEquipmentDialog &&
        <EquipmentAddDialog show={ this.state.showEquipmentDialog } onSave={ this.saveNewEquipment } onClose={ this.closeEquipmentDialog } />
      }
      { this.state.showEditDialog &&
        <OwnersEditDialog show={ this.state.showEditDialog } onSave={ this.saveEdit } onClose={ this.closeEditDialog } />
      }
      { this.state.showPolicyDialog &&
        <OwnersPolicyEditDialog show={ this.state.showPolicyDialog } onSave={ this.savePolicyEdit } onClose={ this.closePolicyDialog } />
      }
      { this.state.showContactDialog &&
        <ContactsEditDialog
          show={ this.state.showContactDialog }
          contact={ this.state.contact }
          onSave={ this.saveContact }
          onClose={ this.closeContactDialog }
          isFirstContact={!this.props.owner.contacts || Object.keys(this.props.owner.contacts).length === 0}
        />
      }
      { this.state.showDocumentsDialog &&
        <DocumentsListDialog
          show={ owner && this.state.showDocumentsDialog }
          parent={ owner }
          onClose={ this.closeDocumentsDialog }
        />
      }
      { this.state.showNotesDialog &&
        <NotesDialog
          show={ this.state.showNotesDialog }
          onSave={ Api.addOwnerNote }
          id={ this.props.params.ownerId }
          getNotes={ Api.getOwnerNotes }
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
          parent={ owner }
          owner
        />
      }
    </div>;
  },
});


function mapStateToProps(state) {
  return {
    owner: state.models.owner,
    notes: state.models.ownerNotes,
    equipment: state.models.equipment,
    equipmentAttachments: state.models.equipmentAttachments,
    contact: state.models.contact,
    documents: state.models.documents,
    uiContacts: state.ui.ownerContacts,
    uiEquipment: state.ui.ownerEquipment,
  };
}

export default connect(mapStateToProps)(OwnersDetail);
