import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { Row, Col, Alert, Button, ButtonGroup, Badge } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Link } from 'react-router-dom';
import _ from 'lodash';
import Promise from 'bluebird';

import ContactsEditDialog from './dialogs/ContactsEditDialog.jsx';
import DocumentsListDialog from './dialogs/DocumentsListDialog.jsx';
import EquipmentAddDialog from './dialogs/EquipmentAddDialog.jsx';
import OwnersEditDialog from './dialogs/OwnersEditDialog.jsx';
import OwnersPolicyEditDialog from './dialogs/OwnersPolicyEditDialog.jsx';
import NotesDialog from './dialogs/NotesDialog.jsx';
import OwnerChangeStatusDialog from './dialogs/OwnerChangeStatusDialog.jsx';
import StatusDropdown from '../components/StatusDropdown.jsx';

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
import ReturnButton from '../components/ReturnButton.jsx';
import PageHeader from '../components/ui/PageHeader.jsx';
import SubHeader from '../components/ui/SubHeader.jsx';
import PrintButton from '../components/PrintButton.jsx';
import Authorize from '../components/Authorize.jsx';

import { activeOwnerSelector, activeOwnerIdSelector } from '../selectors/ui-selectors.js';

import { formatDateTime, formatDateTimeUTCToLocal, today, toZuluTime } from '../utils/date';
import { sortDir, sort } from '../utils/array.js';
import { firstLastName } from '../utils/string.js';

/*

TODO:
* Print / Notes / Policy Proof Documents (attachments)

*/

const CONTACT_NAME_SORT_FIELDS = ['givenName', 'surname'];

const OWNER_WITH_EQUIPMENT_IN_ACTIVE_RENTAL_REQUEST_WARNING_MESSAGE =
  'This owner has equipment that ' +
  'is part of an In Progress Rental Request. Release the list (finish hiring / delete) before making this change';

class OwnersDetail extends React.Component {
  static propTypes = {
    ownerId: PropTypes.number.isRequired,
    owner: PropTypes.object,
    documents: PropTypes.object,
    uiContacts: PropTypes.object,
    uiEquipment: PropTypes.object,
    history: PropTypes.object,
    match: PropTypes.object,
  };

  constructor(props) {
    super(props);

    this.state = {
      loading: true,
      reloading: false,

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
      uiContacts: {
        sortField: props.uiContacts.sortField || CONTACT_NAME_SORT_FIELDS,
        sortDesc: props.uiContacts.sortDesc === true,
      },

      // Equipment
      uiEquipment: {
        sortField: props.uiEquipment.sortField || 'equipmentNumber',
        sortDesc: props.uiEquipment.sortDesc === true,
      },
    };
  }

  componentDidMount() {
    store.dispatch({
      type: Action.SET_ACTIVE_OWNER_ID_UI,
      ownerId: this.props.match.params.ownerId,
    });
    const ownerId = this.props.match.params.ownerId;
    const { owner } = this.props;

    /* Documents need be fetched every time as they are not project specific in the store ATM */
    Api.getOwnerDocuments(ownerId).then(() => this.setState({ loadingDocuments: false }));

    // Only show loading spinner if there is no existing project in the store
    if (owner) {
      this.setState({ loading: false });
    }

    // Re-fetch project and notes every time
    Promise.all([this.fetch(), Api.getOwnerNotes(ownerId)]).then(() => {
      this.setState({ loading: false });
    });
  }

  fetch = () => {
    this.setState({ reloading: true });
    return Api.getOwner(this.props.match.params.ownerId).then(() => this.setState({ reloading: false }));
  };

  updateContactsUIState = (state, callback) => {
    this.setState({ uiContacts: { ...this.state.uiContacts, ...state } }, () => {
      store.dispatch({
        type: Action.UPDATE_OWNER_CONTACTS_UI,
        ownerContacts: this.state.uiContacts,
      });
      if (callback) {
        callback();
      }
    });
  };

  updateEquipmentUIState = (state, callback) => {
    this.setState({ uiEquipment: { ...this.state.uiEquipment, ...state } }, () => {
      store.dispatch({
        type: Action.UPDATE_OWNER_EQUIPMENT_UI,
        ownerEquipment: this.state.uiEquipment,
      });
      if (callback) {
        callback();
      }
    });
  };

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  updateStatusState = (state) => {
    if (state !== this.props.owner.status) {
      this.setState({ status: state }, this.openChangeStatusDialog);
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
    Api.getOwnerNotes(this.props.owner.id);
  };

  showDocuments = () => {
    this.setState({ showDocumentsDialog: true });
  };

  closeDocumentsDialog = () => {
    this.setState({ showDocumentsDialog: false });
  };

  openEditDialog = () => {
    this.setState({ showEditDialog: true });
  };

  closeEditDialog = () => {
    this.setState({ showEditDialog: false });
  };

  ownerSaved = () => {
    Log.ownerModified(this.props.owner);
  };

  openContactDialog = (contactId) => {
    var contact;
    if (contactId === 0) {
      // New
      contact = {
        id: 0,
        owner: this.props.owner,
      };
    } else if (contactId) {
      // Open the contact for viewing if possible
      contact = this.props.owner.contacts.find((contact) => contact.id === contactId);
    }
    this.setState({
      contact: contact,
      showContactDialog: true,
    });
  };

  closeContactDialog = () => {
    this.setState({ showContactDialog: false });
  };

  deleteContact = (contact) => {
    Api.deleteContact(contact).then(() => {
      Log.ownerContactDeleted(this.props.owner, contact).then(() => {
        // In addition to refreshing the contacts, we need to update the owner
        // to get primary contact info and history.
        this.fetch();
      });
    });
  };

  contactSaved = (contact) => {
    var isNew = !contact.id;
    var log = isNew ? Log.ownerContactAdded : Log.ownerContactUpdated;

    log(this.props.owner, contact).then(() => {
      // In addition to refreshing the contacts, we need to update the owner
      // to get primary contact info and history.
      this.fetch();
    });

    this.closeContactDialog();
  };

  openEquipmentDialog = () => {
    this.setState({ showEquipmentDialog: true });
  };

  closeEquipmentDialog = () => {
    this.setState({ showEquipmentDialog: false });
  };

  equipmentSaved = (equipment) => {
    this.closeEquipmentDialog();
    Log.ownerEquipmentAdded(this.props.owner, equipment);
    Log.equipmentAdded(equipment);
    // Open it up
    this.props.history.push(`${Constant.EQUIPMENT_PATHNAME}/${equipment.id}`, {
      returnUrl: `${Constant.OWNERS_PATHNAME}/${this.props.owner.id}`,
    });
  };

  equipmentVerifyAll = () => {
    var now = today();
    var owner = this.props.owner;

    // Update the last verified date on all pieces of equipment
    var equipmentList = _.map(owner.equipmentList, (equipment) => {
      return {
        ...equipment,
        lastVerifiedDate: toZuluTime(now),
        owner: { id: owner.id },
      };
    });

    Api.updateOwnerEquipment(owner, equipmentList).then(() => {
      this.fetch();
    });
  };

  equipmentVerify = (equipment) => {
    const updatedEquipment = {
      ...equipment,
      lastVerifiedDate: toZuluTime(today()),
      owner: { id: this.props.owner.id },
    };

    Log.ownerEquipmentVerified(this.props.owner, updatedEquipment);

    Api.updateEquipment(updatedEquipment).then(() => {
      this.fetch();
    });
  };

  openPolicyDialog = () => {
    this.setState({ showPolicyDialog: true });
  };

  closePolicyDialog = () => {
    this.setState({ showPolicyDialog: false });
  };

  policySaved = () => {
    Log.ownerModifiedPolicy(this.props.owner);
  };

  openPolicyDocumentsDialog = () => {
    // TODO Show popup with links to policy documents
    this.setState({ showPolicyDocumentsDialog: true });
  };

  closePolicyDocumentsDialog = () => {
    this.setState({ showPolicyDocumentsDialog: false });
  };

  addPolicyDocument = () => {
    // TODO Upload policy document (proof of policy coverage)
  };

  openNotesDialog = () => {
    this.setState({ showNotesDialog: true });
  };

  closeNotesDialog = () => {
    this.setState({ showNotesDialog: false });
  };

  render() {
    const { loading, loadingDocuments } = this.state;
    var owner = this.props.owner || {};

    var isApproved = owner.status === Constant.OWNER_STATUS_CODE_APPROVED;
    var restrictEquipmentAddTooltip = 'Equipment can only be added to an approved owner.';
    var restrictEquipmentVerifyTooltip = 'Equipment can only be verified for an approved owner.';

    const statuses = _.pull(
      [Constant.OWNER_STATUS_CODE_APPROVED, Constant.OWNER_STATUS_CODE_PENDING, Constant.OWNER_STATUS_CODE_ARCHIVED],
      owner.status
    );

    return (
      <div id="owners-detail">
        <div>
          <Row id="owners-top" className="top-container">
            <Col sm={9}>
              <StatusDropdown
                id="owner-status-dropdown"
                status={loading ? 'Loading ...' : owner.status}
                statuses={statuses}
                disabled={owner.activeRentalRequest || loading}
                disabledTooltip={OWNER_WITH_EQUIPMENT_IN_ACTIVE_RENTAL_REQUEST_WARNING_MESSAGE}
                onSelect={this.updateStatusState}
              />
              <Button id="owner-notes-button" title="Notes" disabled={loading} onClick={this.openNotesDialog}>
                Notes ({loading ? ' ' : owner.notes?.length})
              </Button>
              <Button id="owner-documents-button" title="Documents" disabled={loading} onClick={this.showDocuments}>
                Documents ({loadingDocuments ? ' ' : Object.keys(this.props.documents).length})
              </Button>
              <Badge className={owner.isMaintenanceContractor ? 'ml-5' : 'hide'}>Maintenance Contractor</Badge>
            </Col>
            <Col sm={3}>
              <div className="pull-right">
                <PrintButton disabled={loading} />
                <ReturnButton />
              </div>
            </Col>
          </Row>

          <PageHeader id="owners-header" title="Company" subTitle={loading ? '...' : owner.organizationName} />

          <Row>
            <Col md={12}>
              <div className="well">
                <SubHeader
                  title="Owner Information"
                  editButtonTitle="Edit Owner"
                  editButtonDisabled={loading || owner.activeRentalRequest}
                  editButtonDisabledTooltip={!loading && OWNER_WITH_EQUIPMENT_IN_ACTIVE_RENTAL_REQUEST_WARNING_MESSAGE}
                  onEditClicked={this.openEditDialog}
                />
                {(() => {
                  if (loading) {
                    return (
                      <div className="spinner-container">
                        <Spinner />
                      </div>
                    );
                  }

                  return (
                    <div id="owners-data">
                      <Row className="equal-height">
                        <Col lg={4} md={6} sm={12} xs={12}>
                          <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Doing Business As">
                            {owner.doingBusinessAs}
                          </ColDisplay>
                        </Col>
                        <Col lg={4} md={6} sm={12} xs={12}>
                          <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Primary Contact">
                            {owner.primaryContactName}
                          </ColDisplay>
                        </Col>
                        <Col lg={4} md={6} sm={12} xs={12}>
                          <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Owner Name">
                            {owner.ownerName}
                          </ColDisplay>
                        </Col>
                        <Col lg={4} md={6} sm={12} xs={12}>
                          <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Owner Code">
                            {owner.ownerCode}
                          </ColDisplay>
                        </Col>
                        <Col lg={4} md={6} sm={12} xs={12}>
                          <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="District Office">
                            {owner.districtName}
                          </ColDisplay>
                        </Col>
                        <Col lg={4} md={6} sm={12} xs={12}>
                          <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Meets Residency?">
                            {owner.meetsResidency ? 'Yes' : 'No'}
                          </ColDisplay>
                        </Col>
                        <Col lg={4} md={6} sm={12} xs={12}>
                          <ColDisplay
                            labelProps={{ xs: 4 }}
                            fieldProps={{ xs: 8 }}
                            label="Registered BC Company Number"
                          >
                            {owner.registeredCompanyNumber}
                          </ColDisplay>
                        </Col>
                        <Col lg={4} md={6} sm={12} xs={12}>
                          <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Service/Local Area">
                            {owner.localAreaName}
                          </ColDisplay>
                        </Col>
                        <Col lg={4} md={6} sm={12} xs={12}>
                          <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Maintenance Contractor">
                            {owner.isMaintenanceContractor ? 'Yes' : 'No'}
                          </ColDisplay>
                        </Col>
                        <Col lg={4} md={6} sm={12} xs={12}>
                          <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Company Address">
                            {owner.address1} {owner.address2} <br /> {owner.city} {owner.province} {owner.postalCode}
                          </ColDisplay>
                        </Col>
                      </Row>
                    </div>
                  );
                })()}
              </div>
            </Col>
            <Col md={12}>
              <div className="well">
                <SubHeader
                  title="Policy"
                  editButtonTitle="Edit Policy Information"
                  editButtonDisabled={loading}
                  onEditClicked={this.openPolicyDialog}
                />
                {(() => {
                  if (loading) {
                    return (
                      <div className="spinner-container">
                        <Spinner />
                      </div>
                    );
                  }

                  return (
                    <Row id="owners-policy" className="equal-height">
                      <Col lg={4} md={6} sm={12} xs={12}>
                        <ColDisplay labelProps={{ xs: 6 }} fieldProps={{ xs: 6 }} label="WCB Number">
                          {owner.workSafeBCPolicyNumber}
                        </ColDisplay>
                      </Col>
                      <Col lg={4} md={6} sm={12} xs={12}>
                        <ColDisplay labelProps={{ xs: 6 }} fieldProps={{ xs: 6 }} label="WCB Expiry Date">
                          {formatDateTime(owner.workSafeBCExpiryDate, Constant.DATE_YEAR_SHORT_MONTH_DAY)}
                        </ColDisplay>
                      </Col>
                      <Col lg={4} md={6} sm={12} xs={12}>
                        <ColDisplay labelProps={{ xs: 6 }} fieldProps={{ xs: 6 }} label="CGL Insurance Company">
                          {owner.cglCompanyName}
                        </ColDisplay>
                      </Col>
                      <Col lg={4} md={6} sm={12} xs={12}>
                        <ColDisplay labelProps={{ xs: 6 }} fieldProps={{ xs: 6 }} label="CGL Policy Number">
                          {owner.cglPolicyNumber}
                        </ColDisplay>
                      </Col>
                      <Col lg={4} md={6} sm={12} xs={12}>
                        <ColDisplay labelProps={{ xs: 6 }} fieldProps={{ xs: 6 }} label="CGL Policy End Date">
                          {formatDateTime(owner.cglEndDate, Constant.DATE_YEAR_SHORT_MONTH_DAY)}
                        </ColDisplay>
                      </Col>
                    </Row>
                  );
                })()}
              </div>
            </Col>
            <Col md={12}>
              <div className="well">
                <SubHeader title="Contacts" />
                {(() => {
                  if (loading) {
                    return (
                      <div className="spinner-container">
                        <Spinner />
                      </div>
                    );
                  }

                  var addContactButton = (
                    <Authorize>
                      <Button title="Add Contact" onClick={this.openContactDialog.bind(this, 0)} bsSize="xsmall">
                        <FontAwesomeIcon icon="plus" />
                        &nbsp;<strong>Add</strong>
                      </Button>
                    </Authorize>
                  );

                  if (!owner.contacts || owner.contacts.length === 0) {
                    return <Alert bsStyle="success">No contacts {addContactButton}</Alert>;
                  }

                  var contacts = sort(owner.contacts, this.state.uiContacts.sortField, this.state.uiContacts.sortDesc);

                  var headers = [
                    { field: CONTACT_NAME_SORT_FIELDS, title: 'Name' },
                    { field: 'phone', title: 'Phone' },
                    { field: 'mobilePhoneNumber', title: 'Cell Phone' },
                    { field: 'faxPhoneNumber', title: 'Fax' },
                    { field: 'emailAddress', title: 'Email' },
                    { field: 'role', title: 'Role' },
                    { field: 'notes', title: 'Notes' },
                    {
                      field: 'addContact',
                      title: 'Add Contact',
                      style: { textAlign: 'right' },
                      node: addContactButton,
                    },
                  ];

                  return (
                    <SortTable
                      id="contact-list"
                      sortField={this.state.uiContacts.sortField}
                      sortDesc={this.state.uiContacts.sortDesc}
                      onSort={this.updateContactsUIState}
                      headers={headers}
                    >
                      {contacts.map((contact) => {
                        return (
                          <tr key={contact.id}>
                            <td>
                              {contact.isPrimary && <FontAwesomeIcon icon="star" />}
                              {firstLastName(contact.givenName, contact.surname)}
                            </td>
                            <td>{contact.phone}</td>
                            <td>{contact.mobilePhoneNumber}</td>
                            <td>{contact.faxPhoneNumber}</td>
                            <td>
                              <a href={`mailto:${contact.emailAddress}`} rel="noopener noreferrer" target="_blank">
                                {contact.emailAddress}
                              </a>
                            </td>
                            <td>{contact.role}</td>
                            <td>{contact.notes ? 'Y' : ''}</td>
                            <td style={{ textAlign: 'right' }}>
                              <ButtonGroup>
                                {contact.canDelete && !contact.isPrimary && (
                                  <Authorize>
                                    <DeleteButton name="Contact" onConfirm={this.deleteContact.bind(this, contact)} />
                                  </Authorize>
                                )}
                                {contact.canEdit && (
                                  <EditButton name="Contact" onClick={this.openContactDialog.bind(this, contact.id)} />
                                )}
                              </ButtonGroup>
                            </td>
                          </tr>
                        );
                      })}
                    </SortTable>
                  );
                })()}
              </div>
              <div className="well">
                <SubHeader title={`Equipment (${loading ? ' ' : owner.numberOfEquipment})`}>
                  <CheckboxControl id="showAttachments" className="mr-5" inline updateState={this.updateState}>
                    <small>Show Attachments</small>
                  </CheckboxControl>
                  <Authorize>
                    <OverlayTrigger
                      trigger="click"
                      placement="top"
                      rootClose
                      overlay={<Confirm onConfirm={this.equipmentVerifyAll}></Confirm>}
                    >
                      <TooltipButton
                        disabled={!isApproved}
                        disabledTooltip={restrictEquipmentVerifyTooltip}
                        className="mr-5"
                        title="Verify All Equipment"
                        bsSize="small"
                      >
                        Verify All
                      </TooltipButton>
                    </OverlayTrigger>
                    <TooltipButton
                      disabled={!isApproved}
                      disabledTooltip={restrictEquipmentAddTooltip}
                      title="Add Equipment"
                      bsSize="small"
                      onClick={this.openEquipmentDialog}
                    >
                      <FontAwesomeIcon icon="plus" />
                    </TooltipButton>
                  </Authorize>
                </SubHeader>
                {(() => {
                  if (loading) {
                    return (
                      <div className="spinner-container">
                        <Spinner />
                      </div>
                    );
                  }

                  if (!owner.equipmentList || owner.equipmentList.length === 0) {
                    return <Alert bsStyle="success">No equipment</Alert>;
                  }

                  var equipmentList = _.orderBy(
                    owner.equipmentList,
                    [this.state.uiEquipment.sortField],
                    sortDir(this.state.uiEquipment.sortDesc)
                  );

                  var headers = [
                    { field: 'equipmentNumber', title: 'ID' },
                    { field: 'localArea.name', title: 'Local Area' },
                    { field: 'typeName', title: 'Equipment Type' },
                    { field: 'details', title: 'Make/Model/Size/Year' },
                    { field: 'lastVerifiedDate', title: 'Last Verified' },
                    { field: 'blank' },
                  ];

                  return (
                    <SortTable
                      id="equipment-list"
                      sortField={this.state.uiEquipment.sortField}
                      sortDesc={this.state.uiEquipment.sortDesc}
                      onSort={this.updateEquipmentUIState}
                      headers={headers}
                    >
                      {_.map(equipmentList, (equipment) => {
                        return (
                          <tr key={equipment.id}>
                            <td>
                              <Link to={`${Constant.EQUIPMENT_PATHNAME}/${equipment.id}`}>
                                {equipment.equipmentCode}
                              </Link>
                            </td>
                            <td>{equipment.localArea.name}</td>
                            <td>{equipment.typeName}</td>
                            <td>
                              {equipment.details}
                              {this.state.showAttachments && (
                                <div>
                                  Attachments:
                                  {equipment.equipmentAttachments &&
                                    equipment.equipmentAttachments.map((item, i) => (
                                      <span key={item.id}>
                                        <span> </span>
                                        <span className="attachment">
                                          {item.typeName}
                                          {i + 1 < equipment.equipmentAttachments.length && <span>,</span>}
                                        </span>
                                      </span>
                                    ))}
                                  {(!equipment.equipmentAttachments || equipment.equipmentAttachments.length === 0) && (
                                    <span> none</span>
                                  )}
                                </div>
                              )}
                            </td>
                            <td>
                              {equipment.isApproved
                                ? formatDateTimeUTCToLocal(
                                    equipment.lastVerifiedDate,
                                    Constant.DATE_YEAR_SHORT_MONTH_DAY
                                  )
                                : 'Not Approved'}
                            </td>
                            <Authorize>
                              <td style={{ textAlign: 'right' }}>
                                <TooltipButton
                                  disabled={!isApproved}
                                  disabledTooltip={restrictEquipmentVerifyTooltip}
                                  title="Verify Equipment"
                                  bsSize="xsmall"
                                  onClick={this.equipmentVerify.bind(this, equipment)}
                                >
                                  <FontAwesomeIcon icon="check" /> OK
                                </TooltipButton>
                              </td>
                            </Authorize>
                          </tr>
                        );
                      })}
                    </SortTable>
                  );
                })()}
              </div>
              <div className="well">
                <SubHeader title="History" />
                {owner.historyEntity && <History historyEntity={owner.historyEntity} refresh={!this.state.reloading} />}
              </div>
            </Col>
          </Row>
        </div>
        {this.state.showChangeStatusDialog && (
          <OwnerChangeStatusDialog
            show={this.state.showChangeStatusDialog}
            owner={owner}
            status={this.state.status}
            onClose={this.closeChangeStatusDialog}
            onStatusChanged={this.onStatusChanged}
          />
        )}
        {this.state.showNotesDialog && (
          <NotesDialog
            show={this.state.showNotesDialog}
            id={this.props.match.params.ownerId}
            notes={owner.notes}
            getNotes={Api.getOwnerNotes}
            saveNote={Api.addOwnerNote}
            onClose={this.closeNotesDialog}
          />
        )}
        {this.state.showDocumentsDialog && (
          <DocumentsListDialog
            show={this.state.showDocumentsDialog}
            parent={owner}
            onClose={this.closeDocumentsDialog}
          />
        )}
        {this.state.showEditDialog && (
          <OwnersEditDialog
            show={this.state.showEditDialog}
            owner={owner}
            onSave={this.ownerSaved}
            onClose={this.closeEditDialog}
          />
        )}
        {this.state.showEquipmentDialog && (
          <EquipmentAddDialog
            show={this.state.showEquipmentDialog}
            owner={owner}
            onSave={this.equipmentSaved}
            onClose={this.closeEquipmentDialog}
          />
        )}
        {this.state.showPolicyDialog && (
          <OwnersPolicyEditDialog
            show={this.state.showPolicyDialog}
            owner={owner}
            onSave={this.policySaved}
            onClose={this.closePolicyDialog}
          />
        )}
        {this.state.showContactDialog && (
          <ContactsEditDialog
            show={this.state.showContactDialog}
            contact={this.state.contact}
            parent={owner}
            saveContact={Api.saveOwnerContact}
            defaultPrimary={owner.contacts.length === 0}
            onSave={this.contactSaved}
            onClose={this.closeContactDialog}
          />
        )}
      </div>
    );
  }
}

function mapStateToProps(state) {
  return {
    owner: activeOwnerSelector(state),
    ownerId: activeOwnerIdSelector(state), //TODO: check if ownerID is still needed as a redux prop. Could be removed?
    documents: state.models.documents,
    uiContacts: state.ui.ownerContacts,
    uiEquipment: state.ui.ownerEquipment,
  };
}

export default connect(mapStateToProps)(OwnersDetail);
