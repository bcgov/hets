import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { Row, Col } from 'react-bootstrap';
import { Alert, Button, ButtonGroup, OverlayTrigger } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Link } from 'react-router-dom';
import _ from 'lodash';

import ProjectsEditDialog from './dialogs/ProjectsEditDialog.jsx';
import ContactsEditDialog from './dialogs/ContactsEditDialog.jsx';
import DocumentsListDialog from './dialogs/DocumentsListDialog.jsx';
import RentalRequestsAddDialog from './dialogs/RentalRequestsAddDialog.jsx';
import TimeEntryDialog from './dialogs/TimeEntryDialog.jsx';
import NotesDialog from './dialogs/NotesDialog.jsx';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import * as Log from '../history';
import store from '../store';

import CheckboxControl from '../components/CheckboxControl.jsx';
import ColDisplay from '../components/ColDisplay.jsx';
import Confirm from '../components/Confirm.jsx';
import DeleteButton from '../components/DeleteButton.jsx';
import EditButton from '../components/EditButton.jsx';
import History from '../components/History.jsx';
import SortTable from '../components/SortTable.jsx';
import Spinner from '../components/Spinner.jsx';
import StatusDropdown from '../components/StatusDropdown.jsx';
import TableControl from '../components/TableControl.jsx';
import PageHeader from '../components/ui/PageHeader.jsx';
import SubHeader from '../components/ui/SubHeader.jsx';
import Authorize from '../components/Authorize.jsx';

import { activeProjectSelector } from '../selectors/ui-selectors.js';

import { formatDateTime } from '../utils/date';
import { sort, caseInsensitiveSort } from '../utils/array.js';
import { firstLastName } from '../utils/string.js';
import ReturnButton from '../components/ReturnButton.jsx';
import PrintButton from '../components/PrintButton.jsx';

const CONTACT_NAME_SORT_FIELDS = ['givenName', 'surname'];

const STATUS_NOT_EDITABLE_MESSAGE =
  'The project can only be marked as completed when it has no active requests or actively hired equipment.';

class ProjectsDetail extends React.Component {
  static propTypes = {
    project: PropTypes.object,
    documents: PropTypes.object,
    uiContacts: PropTypes.object,
    history: PropTypes.object,
    match: PropTypes.object,
  };

  constructor(props) {
    super(props);

    this.state = {
      loading: true,
      loadingDocuments: true,
      reloading: false,

      showNotesDialog: false,
      showDocumentsDialog: false,
      showEditDialog: false,
      showAddRequestDialog: false,
      showTimeEntryDialog: false,
      showContactDialog: false,

      includeCompletedRequests: false,

      contact: {},

      rentalAgreement: {},

      // Contacts
      uiContacts: {
        sortField: props.uiContacts.sortField || CONTACT_NAME_SORT_FIELDS,
        sortDesc: props.uiContacts.sortDesc === true,
      },
    };
  }

  componentDidMount() {
    //dispatch Set_ACTIVE_PROJECT_ID_UI needed for activeProjectSelector(state) to work. Solution uses redux state to pass argument values to another selector.
    //https://github.com/reduxjs/reselect#q-how-do-i-create-a-selector-that-takes-an-argument
    store.dispatch({
      type: Action.SET_ACTIVE_PROJECT_ID_UI,
      projectId: this.props.match.params.projectId,
    });

    const { project } = this.props;
    const projectId = this.props.match.params.projectId;

    /* Documents need be fetched every time as they are not project specific in the store ATM */
    Api.getProjectDocuments(projectId).then(() => this.setState({ loadingDocuments: false }));

    // Only show loading spinner if there is no existing project in the store
    if (project) {
      this.setState({ loading: false });
    }

    // Re-fetch project and notes every time
    Promise.all([this.fetch(), Api.getProjectNotes(projectId)]).then(() => {
      this.setState({ loading: false });
    });
  }

  fetch = () => {
    this.setState({ reloading: true });
    return Api.getProject(this.props.match.params.projectId).then(() => this.setState({ reloading: false }));
  };

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  updateContactsUIState = (state, callback) => {
    this.setState({ uiContacts: { ...this.state.uiContacts, ...state } }, () => {
      store.dispatch({ type: Action.UPDATE_PROJECT_CONTACTS_UI, projectContacts: this.state.uiContacts });
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

  openContactDialog = (contactId) => {
    var contact;
    if (contactId === 0) {
      // New
      contact = {
        id: 0,
        owner: this.props.project,
      };
    } else if (contactId) {
      // Open the contact for viewing if possible
      contact = _.find(this.props.project.contacts, (contact) => contact.id === contactId);
    }
    this.setState({
      contact: contact,
      showContactDialog: true,
    });
  };

  closeContactDialog = () => {
    this.setState({ contact: null, showContactDialog: false });
  };

  deleteContact = (contact) => {
    Api.deleteContact(contact).then(() => {
      Log.projectContactDeleted(this.props.project, contact).then(() => {
        this.fetch();
      });
    });
  };

  contactSaved = (contact) => {
    var isNew = !contact.id;
    var log = isNew ? Log.projectContactAdded : Log.projectContactUpdated;

    log(this.props.project, contact).then(() => {
      // In addition to refreshing the contacts, we need to update the owner
      // to get primary contact info and history.
      this.fetch();
    });

    this.closeContactDialog();
  };

  openAddRequestDialog = () => {
    this.setState({ showAddRequestDialog: true });
  };

  closeAddRequestDialog = () => {
    this.setState({ showAddRequestDialog: false });
  };

  newRentalAdded = (rentalRequest) => {
    this.fetch();

    Log.projectRentalRequestAdded(this.props.project, rentalRequest);

    this.props.history.push(`${Constant.RENTAL_REQUESTS_PATHNAME}/${rentalRequest.id}`);
  };

  confirmEndHire = (item) => {
    Api.releaseRentalAgreement(item.id).then(() => {
      Api.getProject(this.props.match.params.projectId);
      Log.projectEquipmentReleased(this.props.project, item.equipment);
    });
  };

  openTimeEntryDialog = (rentalAgreement) => {
    this.setState({ rentalAgreement }, () => {
      this.setState({
        showTimeEntryDialog: true,
        fiscalYearStartDate: this.props.project.fiscalYearStartDate,
      });
    });
  };

  closeTimeEntryDialog = () => {
    this.setState({ showTimeEntryDialog: false });
  };

  cancelRequest = (request) => {
    Api.cancelRentalRequest(request.id).then(() => {
      this.fetch();
    });
  };

  renderRentalRequestListItem = (item) => {
    return (
      <tr key={item.id}>
        <td>
          <Link
            to={`${Constant.RENTAL_REQUESTS_PATHNAME}/${item.id}`}
            className={item.status === Constant.RENTAL_REQUEST_STATUS_CODE_COMPLETED ? 'light' : ''}
          >
            Request
          </Link>
        </td>
        <td>{item.localAreaName}</td>
        <td>{item.equipmentTypeName}</td>
        <td>{item.equipmentCount}</td>
        <td>TBD</td>
        <td>N/A</td>
        <td>N/A</td>
        <td>N/A</td>
        <td>N/A</td>
        <td>
          <DeleteButton
            name="Cancel Rental Request"
            hide={item.yesCount > 0}
            onConfirm={this.cancelRequest.bind(this, item)}
          />
        </td>
      </tr>
    );
  };

  renderRentalAgreementListItem = (item) => {
    return (
      <tr key={item.id}>
        <td>
          <Link
            to={`${Constant.EQUIPMENT_PATHNAME}/${item.equipmentId}`}
            className={item.status === Constant.RENTAL_REQUEST_STATUS_CODE_COMPLETED ? 'light' : ''}
          >
            {item.equipmentCode}
          </Link>
        </td>
        <td>{item.localAreaName}</td>
        <td>{item.equipmentTypeName}</td>
        <td>&nbsp;</td>
        <td>{item.equipment.equipmentDetails}</td>
        <td>
          {item.isCompleted ? (
            'Completed'
          ) : (
            <EditButton name="Time Entry" onClick={this.openTimeEntryDialog.bind(this, item)} />
          )}
        </td>
        <td>
          {item.status === Constant.RENTAL_REQUEST_STATUS_CODE_COMPLETED ? (
            <div>Released</div>
          ) : (
            <Authorize requires={Constant.PERMISSION_WRITE_ACCESS}>
              <OverlayTrigger
                trigger="focus"
                placement="top"
                overlay={<Confirm onConfirm={this.confirmEndHire.bind(this, item)} />}
              >
                <Button className="btn-custom" size="sm">
                  <FontAwesomeIcon icon={['far', 'check-square']} />
                </Button>
              </OverlayTrigger>
            </Authorize>
          )}
        </td>
        <td>
          <Link to={`${Constant.RENTAL_AGREEMENTS_PATHNAME}/${item.id}`}>Agreement</Link>
        </td>
        <td>{formatDateTime(item.datedOn, Constant.DATE_YEAR_SHORT_MONTH_DAY)}</td>
        <td></td>
      </tr>
    );
  };

  getStatuses = () => {
    var project = this.props.project || {};

    return _.pull([Constant.PROJECT_STATUS_CODE_ACTIVE, Constant.PROJECT_STATUS_CODE_COMPLETED], project.status);
  };

  updateStatusState = (state) => {
    if (state !== this.props.project.status) {
      const project = {
        ...this.props.project,
        status: state,
      };

      store.dispatch({ type: Action.UPDATE_PROJECT, project });
      Log.projectModifiedStatus(project);
      Api.updateProject(project);
    }
  };

  render() {
    const { loading, loadingDocuments } = this.state;
    var project = this.props.project || {};

    // As per business requirements:
    // "Lists the records - requests then rental agreements, within the groups, list in largest-to-smallest ID order (aka reverse chronological create)."
    var rentalRequests = _.orderBy(project.rentalRequests, ['id'], ['desc']);
    var rentalAgreements = _.orderBy(project.rentalAgreements, ['id'], ['desc']);

    var combinedList = _.concat(rentalRequests, rentalAgreements);
    // Exclude completed items
    if (!this.state.includeCompletedRequests) {
      _.remove(combinedList, (x) => !x.isActive);
    }

    return (
      <div id="projects-detail">
        <div>
          <div className="top-container">
            <Row id="projects-top">
              <Col sm={9}>
                <StatusDropdown
                  id="project-status-dropdown"
                  status={project.status || Constant.PROJECT_STATUS_CODE_ACTIVE}
                  statuses={this.getStatuses()}
                  disabled={!project.canEditStatus}
                  disabledTooltip={STATUS_NOT_EDITABLE_MESSAGE}
                  onSelect={this.updateStatusState}
                />
                <Button title="Notes" className="ml-1 mr-1 btn-custom" disabled={loading} onClick={this.showNotes}>
                  Notes ({loading ? ' ' : project.notes?.length})
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
                  <PrintButton disabled={loading} />
                  <ReturnButton />
                </div>
              </Col>
            </Row>
            <PageHeader title="Project" subTitle={loading ? '...' : project.name} />
            <PageHeader title="District" subTitle={loading ? '...' : project.districtName} />
          </div>

          <Row>
            <Col md={12}>
              <div className="well">
                <SubHeader
                  title="Project Information"
                  editButtonTitle="Edit Project"
                  editButtonDisabled={loading}
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

                  var mailto = <a href={`mailto:${project.primaryContactEmail}`}>{project.primaryContactName}</a>;

                  return (
                    <Row id="projects-data" className="equal-height">
                      <Col lg={4} md={6} sm={12} xs={12}>
                        <ColDisplay labelProps={{ xs: 6 }} fieldProps={{ xs: 6 }} label="Fiscal Year">
                          {project.fiscalYear}
                        </ColDisplay>
                      </Col>
                      <Col lg={4} md={6} sm={12} xs={12}>
                        <ColDisplay labelProps={{ xs: 6 }} fieldProps={{ xs: 6 }} label="Provincial Project Number">
                          {project.provincialProjectNumber}
                        </ColDisplay>
                      </Col>
                      <Col lg={4} md={6} sm={12} xs={12}>
                        <ColDisplay labelProps={{ xs: 6 }} fieldProps={{ xs: 6 }} label="Responsibility Centre">
                          {project.responsibilityCentre}
                        </ColDisplay>
                      </Col>
                      <Col lg={4} md={6} sm={12} xs={12}>
                        <ColDisplay labelProps={{ xs: 6 }} fieldProps={{ xs: 6 }} label="Service Line">
                          {project.serviceLine}
                        </ColDisplay>
                      </Col>
                      <Col lg={4} md={6} sm={12} xs={12}>
                        <ColDisplay labelProps={{ xs: 6 }} fieldProps={{ xs: 6 }} label="STOB">
                          {project.stob}
                        </ColDisplay>
                      </Col>
                      <Col lg={4} md={6} sm={12} xs={12}>
                        <ColDisplay labelProps={{ xs: 6 }} fieldProps={{ xs: 6 }} label="Product">
                          {project.product}
                        </ColDisplay>
                      </Col>
                      <Col lg={4} md={6} sm={12} xs={12}>
                        <ColDisplay labelProps={{ xs: 6 }} fieldProps={{ xs: 6 }} label="Business Function">
                          {project.businessFunction}
                        </ColDisplay>
                      </Col>
                      <Col lg={4} md={6} sm={12} xs={12}>
                        <ColDisplay labelProps={{ xs: 6 }} fieldProps={{ xs: 6 }} label="Work Activity">
                          {project.workActivity}
                        </ColDisplay>
                      </Col>
                      <Col lg={4} md={6} sm={12} xs={12}>
                        <ColDisplay labelProps={{ xs: 6 }} fieldProps={{ xs: 6 }} label="Cost Type">
                          {project.costType}
                        </ColDisplay>
                      </Col>
                      <Col lg={4} md={6} sm={12} xs={12}>
                        <ColDisplay labelProps={{ xs: 6 }} fieldProps={{ xs: 6 }} label="Project Information">
                          {project.information}
                        </ColDisplay>
                      </Col>
                      <Col lg={4} md={6} sm={12} xs={12}>
                        <ColDisplay
                          labelProps={{ xs: 6 }}
                          fieldProps={{ xs: 6 }}
                          label={project.primaryContactRole || 'Primary Contact'}
                        >
                          {project.primaryContactEmail ? mailto : `${project.primaryContactName}`}
                          {project.primaryContactPhone ? `, ${project.primaryContactPhone}` : ''}
                        </ColDisplay>
                      </Col>
                    </Row>
                  );
                })()}
              </div>
              <div className="well">
                <SubHeader title="Hired Equipment / Requests">
                  <div className="d-flex align-items-baseline">
                    <CheckboxControl
                      id="includeCompletedRequests"
                      inline
                      checked={this.state.includeCompletedRequests}
                      updateState={this.updateState}
                      label={<small>Show Completed</small>}
                    />

                    <Authorize requires={Constant.PERMISSION_WRITE_ACCESS}>
                      <Button
                        className="btn-custom"
                        id="add-request-button"
                        title="Add Request"
                        size="sm"
                        onClick={this.openAddRequestDialog}
                      >
                        <FontAwesomeIcon icon="plus" /> Add
                      </Button>
                    </Authorize>
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

                  if (Object.keys(combinedList).length === 0) {
                    return <Alert variant="success">No equipment</Alert>;
                  }

                  var headers = [
                    { field: 'equipmentCode', title: 'ID' },
                    { field: 'localAreaName', title: 'Local Area' },
                    { field: 'equipmentTypeName', title: 'Type' },
                    { field: 'equipmentCount', title: 'Quantity' },
                    { field: 'equipmentMake', title: 'Year Make/Model/Size' },
                    { field: 'lastTimeRecord', title: 'Time Entry' },
                    { field: 'release', title: 'Release' },
                    { field: 'agreement', title: 'Agreement' },
                    { field: 'hiredDate', title: 'Hired Date' },
                    { field: 'blank' },
                  ];

                  return (
                    <TableControl id="equipment-list" headers={headers}>
                      {_.map(combinedList, (listItem) => {
                        if (listItem.isRentalRequest) {
                          return this.renderRentalRequestListItem(listItem);
                        } else {
                          return this.renderRentalAgreementListItem(listItem);
                        }
                      })}
                    </TableControl>
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
                    <Authorize requires={Constant.PERMISSION_WRITE_ACCESS}>
                      <Button
                        className="btn-custom"
                        title="Add Contact"
                        onClick={this.openContactDialog.bind(this, 0)}
                        size="sm"
                      >
                        <FontAwesomeIcon icon="plus" />
                        &nbsp;<strong>Add</strong>
                      </Button>
                    </Authorize>
                  );

                  if (!project.contacts || project.contacts.length === 0) {
                    return <Alert variant="success">No contacts {addContactButton}</Alert>;
                  }

                  var contacts = sort(
                    project.contacts,
                    this.state.uiContacts.sortField,
                    this.state.uiContacts.sortDesc,
                    caseInsensitiveSort
                  );

                  var headers = [
                    { field: 'name', title: 'Name' },
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
                                {contact.canDelete && (
                                  <DeleteButton name="Contact" onConfirm={this.deleteContact.bind(this, contact)} />
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
                <SubHeader title="History" />
                {project.historyEntity && (
                  <History historyEntity={project.historyEntity} refresh={!this.state.reloading} />
                )}
              </div>
            </Col>
          </Row>
        </div>
        {this.state.showEditDialog && (
          <ProjectsEditDialog show={this.state.showEditDialog} project={project} onClose={this.closeEditDialog} />
        )}
        {this.state.showNotesDialog && (
          <NotesDialog
            show={this.state.showNotesDialog}
            id={String(this.props.match.params.projectId)}
            getNotes={Api.getProjectNotes}
            saveNote={Api.addProjectNote}
            onClose={this.closeNotesDialog}
            notes={project.notes}
          />
        )}
        {this.state.showDocumentsDialog && (
          <DocumentsListDialog
            show={this.state.showDocumentsDialog}
            parent={project}
            onClose={this.closeDocumentsDialog}
          />
        )}
        {this.state.showAddRequestDialog && (
          <RentalRequestsAddDialog
            show={this.state.showAddRequestDialog}
            project={project}
            onClose={this.closeAddRequestDialog}
            onRentalAdded={this.newRentalAdded}
          />
        )}
        {this.state.showTimeEntryDialog && (
          <TimeEntryDialog
            show={this.state.showTimeEntryDialog}
            project={project}
            rentalAgreementId={this.state.rentalAgreement.id}
            multipleEntryAllowed={false}
            onClose={this.closeTimeEntryDialog}
          />
        )}
        {this.state.showContactDialog && (
          <ContactsEditDialog
            show={this.state.showContactDialog}
            saveContact={Api.saveProjectContact}
            contact={this.state.contact}
            parent={project}
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
    project: activeProjectSelector(state),
    documents: state.models.documents,
    uiContacts: state.ui.projectContacts,
  };
}

export default connect(mapStateToProps)(ProjectsDetail);
