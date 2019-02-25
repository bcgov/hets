import React from 'react';

import { connect } from 'react-redux';

import { browserHistory } from 'react-router';

import { Well, Row, Col } from 'react-bootstrap';
import { Alert, Button, ButtonGroup, Glyphicon, Label } from 'react-bootstrap';
import { Link } from 'react-router';

import _ from 'lodash';
import Promise from 'bluebird';

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
import DeleteButton from '../components/DeleteButton.jsx';
import EditButton from '../components/EditButton.jsx';
import History from '../components/History.jsx';
import SortTable from '../components/SortTable.jsx';
import Spinner from '../components/Spinner.jsx';
import TableControl from '../components/TableControl.jsx';
import Confirm from '../components/Confirm.jsx';
import OverlayTrigger from '../components/OverlayTrigger.jsx';

import PageHeader from '../components/ui/PageHeader.jsx';
import SubHeader from '../components/ui/SubHeader.jsx';

import { formatDateTime } from '../utils/date';

/*

TODO:
* Print / Notes / Docs / Contacts (TBD) / History / Hired Equipment List

*/

var ProjectsDetail = React.createClass({
  propTypes: {
    project: React.PropTypes.object,
    contact: React.PropTypes.object,
    notes: React.PropTypes.object,
    attachments: React.PropTypes.object,
    documents: React.PropTypes.object,
    params: React.PropTypes.object,
    uiContacts: React.PropTypes.object,
    router: React.PropTypes.object,
  },

  getInitialState() {
    return {
      loading: true,
      loadingHistory: false,

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
      uiContacts : {
        sortField: this.props.uiContacts.sortField || 'name',
        sortDesc: this.props.uiContacts.sortDesc === true,
      },
    };
  },

  componentDidMount() {
    this.setState({ loading: true });
    this.fetch().finally(() => {
      this.setState({ loading: false });
    });
  },

  fetch() {
    const { projectId } = this.props.params;

    return Promise.all([
      Api.getProject(projectId),
      Api.getProjectDocuments(projectId),
      Api.getProjectNotes(projectId),
    ]);
  },

  updateState(state, callback) {
    this.setState(state, callback);
  },

  updateContactsUIState(state, callback) {
    this.setState({ uiContacts: { ...this.state.uiContacts, ...state }}, () => {
      store.dispatch({ type: Action.UPDATE_PROJECT_CONTACTS_UI, projectContacts: this.state.uiContacts });
      if (callback) { callback(); }
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

  addDocument() {

  },

  openEditDialog() {
    this.setState({ showEditDialog: true });
  },

  closeEditDialog() {
    this.setState({ showEditDialog: false });
  },

  openContactDialog(contactId) {
    var contact;
    if (contactId === 0) {
      // New
      contact = {
        id: 0,
        owner: this.props.project,
      };
    } else if (contactId) {
      // Open the contact for viewing if possible
      contact = this.props.project.contacts[contactId];
    }
    this.setState({
      contact: contact,
      showContactDialog: true,
    });
  },

  closeContactDialog() {
    this.setState({ contact:null, showContactDialog: false });
  },

  deleteContact(contact) {
    Api.deleteContact(contact).then(() => {
      Log.projectContactDeleted(this.props.project, this.props.contact).then(() => {
        this.fetch();
      });
    });
  },

  saveContact(contact) {
    var isNew = !contact.id;
    var log = isNew ? Log.projectContactAdded : Log.projectContactUpdated;

    Api.addProjectContact(this.props.project, contact).then(() => {
      log(this.props.project, this.props.contact);
      this.fetch();
    });

    this.closeContactDialog();
  },

  print() {
    window.print();
  },

  openAddRequestDialog() {
    this.setState({ showAddRequestDialog: true });
  },

  closeAddRequestDialog() {
    this.setState({ showAddRequestDialog: false });
    store.dispatch({ type: Action.ADD_RENTAL_REQUEST_REFRESH });
  },

  gotoProject(projectId) {
    this.props.router.push({
      pathname: `${ Constant.RENTAL_REQUESTS_PATHNAME }/${ projectId }`,
    });
  },

  confirmEndHire(item) {
    Api.releaseRentalAgreement(item.id).then(() => {
      Api.getProject(this.props.params.projectId);
      Log.projectEquipmentReleased(this.props.project, item.equipment);
    });
  },

  openTimeEntryDialog(rentalAgreement) {
    this.setState({ rentalAgreement: rentalAgreement }, () => {
      this.setState({ showTimeEntryDialog: true });
    });
  },

  closeTimeEntryDialog() {
    this.setState({ showTimeEntryDialog: false });
  },


  cancelRequest(request) {
    store.dispatch({ type: Action.DELETE_PROJECT_RENTAL_REQUEST, requestId: request.id });
    Api.cancelRentalRequest(request.id).then(() => {
      this.fetch();
    });
  },

  render() {
    const { loading } = this.state;
    var project = this.props.project;

    // As per business requirements:
    // "Lists the records - requests then rental agreements, within the groups, list in largest-to-smallest ID order (aka reverse chronological create)."
    var rentalRequests = _.orderBy(project.rentalRequests, ['id'], ['desc']);
    var rentalAgreements = _.orderBy(project.rentalAgreements, ['id'], ['desc']);

    // Exclude unassociated rental agremeents
    _.remove(rentalAgreements, x => !x.rentalRequestId);

    var combinedList =_.concat(rentalRequests, rentalAgreements);
    // Exclude completed items
    if (!this.state.includeCompletedRequests) {
      _.remove(combinedList, (x) => !x.isActive);
    }

    return (
      <div id="projects-detail">
        <div>
          <div className="top-container">
            <Row id="projects-top">
              <Col sm={1} style={{ padding: 7 }}>
                <Label bsStyle={ project.isActive ? 'success' : 'danger'}>{ project.status }</Label>
              </Col>
              <Col sm={8}>
                <Button title="Notes" disabled={loading} onClick={ this.showNotes }>
                  Notes ({ loading ? ' ' : Object.keys(this.props.notes).length })
                </Button>
                <Button title="Documents" disabled={loading} onClick={ this.showDocuments }>
                  Documents ({ loading ? ' ' :  Object.keys(this.props.documents).length })
                </Button>
              </Col>
              <Col sm={3}>
                <div className="pull-right">
                  <Button onClick={ this.print } disabled={loading}><Glyphicon glyph="print" title="Print" /></Button>
                  <Button title="Return" onClick={ browserHistory.goBack }><Glyphicon glyph="arrow-left" /> Return</Button>
                </div>
              </Col>
            </Row>
            <PageHeader title="Project" subTitle={loading ? '...' : project.name}/>
            <PageHeader title="District" subTitle={loading ? '...' : project.districtName}/>
          </div>

          <Row>
            <Col md={12}>
              <Well>
                <SubHeader title="Project Information" editButtonTitle="Edit Project" onEditClicked={ this.openEditDialog }/>
                {(() => {
                  if (loading) { return <div className="spinner-container"><Spinner/></div>; }

                  var mailto = <a href={ `mailto:${project.primaryContactEmail}` }>
                    { project.primaryContactName }
                  </a>;

                  return <Row id="projects-data" className="equal-height">
                    <Col lg={4} md={6} sm={12} xs={12}>
                      <ColDisplay labelProps={{ xs: 6 }} fieldProps={{ xs: 6 }} label="Fiscal Year">{ project.fiscalYear }</ColDisplay>
                    </Col>
                    <Col lg={4} md={6} sm={12} xs={12}>
                      <ColDisplay labelProps={{ xs: 6 }} fieldProps={{ xs: 6 }} label="Provincial Project Number">{ project.provincialProjectNumber }</ColDisplay>
                    </Col>
                    <Col lg={4} md={6} sm={12} xs={12}>
                      <ColDisplay labelProps={{ xs: 6 }} fieldProps={{ xs: 6 }} label="Responsibility Centre">{ project.responsibilityCentre }</ColDisplay>
                    </Col>
                    <Col lg={4} md={6} sm={12} xs={12}>
                      <ColDisplay labelProps={{ xs: 6 }} fieldProps={{ xs: 6 }} label="Service Line">{ project.serviceLine }</ColDisplay>
                    </Col>
                    <Col lg={4} md={6} sm={12} xs={12}>
                      <ColDisplay labelProps={{ xs: 6 }} fieldProps={{ xs: 6 }} label="STOB">{ project.stob }</ColDisplay>
                    </Col>
                    <Col lg={4} md={6} sm={12} xs={12}>
                      <ColDisplay labelProps={{ xs: 6 }} fieldProps={{ xs: 6 }} label="Product">{ project.product }</ColDisplay>
                    </Col>
                    <Col lg={4} md={6} sm={12} xs={12}>
                      <ColDisplay labelProps={{ xs: 6 }} fieldProps={{ xs: 6 }} label="Business Function">{ project.businessFunction }</ColDisplay>
                    </Col>
                    <Col lg={4} md={6} sm={12} xs={12}>
                      <ColDisplay labelProps={{ xs: 6 }} fieldProps={{ xs: 6 }} label="Work Activity">{ project.workActivity }</ColDisplay>
                    </Col>
                    <Col lg={4} md={6} sm={12} xs={12}>
                      <ColDisplay labelProps={{ xs: 6 }} fieldProps={{ xs: 6 }} label="Cost Type">{ project.costType }</ColDisplay>
                    </Col>
                    <Col lg={4} md={6} sm={12} xs={12}>
                      <ColDisplay labelProps={{ xs: 6 }} fieldProps={{ xs: 6 }} label="Project Information">{ project.information }</ColDisplay>
                    </Col>
                    <Col lg={4} md={6} sm={12} xs={12}>
                      <ColDisplay labelProps={{ xs: 6 }} fieldProps={{ xs: 6 }} label={ project.primaryContactRole || 'Primary Contact' }>
                        { project.primaryContactEmail ? mailto : `${project.primaryContactName}` }{ project.primaryContactPhone ? `, ${project.primaryContactPhone}` : '' }
                      </ColDisplay>
                    </Col>
                  </Row>;
                })()}
              </Well>
              <Well>
                <SubHeader title="Hired Equipment / Requests">
                  <CheckboxControl id="includeCompletedRequests" inline checked={ this.state.includeCompletedRequests } updateState={ this.updateState }><small>Show Completed</small></CheckboxControl>
                  <Button title="Add Request" bsSize="small" onClick={ this.openAddRequestDialog }><Glyphicon glyph="plus" /> Add</Button>
                </SubHeader>
                {(() => {
                  if (loading) { return <div className="spinner-container"><Spinner/></div>; }

                  if (Object.keys(combinedList).length === 0) { return <Alert bsStyle="success">No equipment</Alert>; }

                  const RentalRequestListItem = ({ item }) => (
                    <tr key={ item.id }>
                      <td>
                        <Link
                          to={ `rental-requests/${item.id}` }
                          className={item.status === Constant.RENTAL_REQUEST_STATUS_CODE_COMPLETED ? 'light' : ''}>
                          Request
                        </Link>
                      </td>
                      <td>{ item.localAreaName }</td>
                      <td>{ item.equipmentTypeName }</td>
                      <td>{ item.equipmentCount }</td>
                      <td>TBD</td>
                      <td>N/A</td>
                      <td>N/A</td>
                      <td>N/A</td>
                      <td>N/A</td>
                      <td>
                        <DeleteButton name="Cancel Rental Request" hide={ item.yesCount > 0 } onConfirm={ this.cancelRequest.bind(this, item) }/>
                      </td>
                    </tr>
                  );

                  const RentalAgreementListItem = ({ item }) => (
                    <tr key={ item.id }>
                      <td>
                        <Link
                          to={ `equipment/${item.equipmentId}` }
                          className={item.status === Constant.RENTAL_REQUEST_STATUS_CODE_COMPLETED ? 'light' : ''}>
                          { item.equipmentCode }
                        </Link>
                      </td>
                      <td>{ item.localAreaName }</td>
                      <td>{ item.equipmentTypeName }</td>
                      <td>&nbsp;</td>
                      <td>{ item.equipment.equipmentDetails }</td>
                      <td>{ item.isCompleted ?
                        'Completed'
                        :
                        <EditButton name="Time Entry" onClick={this.openTimeEntryDialog.bind(this, item)} />
                      }
                      </td>
                      <td>
                        { item.status === Constant.RENTAL_REQUEST_STATUS_CODE_COMPLETED ?
                          <div>Released</div>
                          :
                          <OverlayTrigger
                            trigger="click"
                            placement="top"
                            rootClose
                            overlay={ <Confirm onConfirm={ this.confirmEndHire.bind(this, item) }/> }>
                            <Button
                              bsSize="xsmall">
                              <Glyphicon glyph="check" />
                            </Button>
                          </OverlayTrigger>
                        }
                      </td>
                      <td><Link to={`${Constant.RENTAL_AGREEMENTS_PATHNAME}/${item.id}`}>Agreement</Link></td>
                      <td>{ formatDateTime(item.datedOn, Constant.DATE_YEAR_SHORT_MONTH_DAY) }</td>
                      <td></td>
                    </tr>
                  );

                  var headers = [
                    { field: 'equipmentCode',     title: 'ID'               },
                    { field: 'localAreaName',     title: 'Local Area'       },
                    { field: 'equipmentTypeName', title: 'Type'             },
                    { field: 'equipmentCount',    title: 'Quantity'         },
                    { field: 'equipmentMake',     title: 'Year Make/Model/Size'  },
                    { field: 'lastTimeRecord',    title: 'Time Entry'       },
                    { field: 'release',           title: 'Release'          },
                    { field: 'agreement',         title: 'Agreement'        },
                    { field: 'hiredDate',         title: 'Hired Date'       },
                    { field: 'blank'                                        },
                  ];

                  return <TableControl id="equipment-list" headers={ headers }>
                    {
                      _.map(combinedList, (listItem, index) => {
                        if (listItem.isRentalRequest) {
                          return <RentalRequestListItem key={index} item={ listItem } project={ project } />;
                        } else {
                          return <RentalAgreementListItem key={index} item={ listItem } />;
                        }
                      })
                    }
                  </TableControl>;
                })()}
              </Well>
            </Col>
            <Col md={12}>
              <Well>
                <SubHeader title="Contacts"/>
                {(() => {
                  if (loading ) { return <div className="spinner-container"><Spinner/></div>; }

                  var addContactButton = <Button title="Add Contact" onClick={ this.openContactDialog.bind(this, 0) } bsSize="small"><Glyphicon glyph="plus" />&nbsp;<strong>Add</strong></Button>;

                  if (!project.contacts || Object.keys(project.contacts).length === 0) { return <Alert bsStyle="success">No contacts { addContactButton }</Alert>; }

                  var contacts = _.sortBy(project.contacts, this.state.uiContacts.sortField);
                  if (this.state.uiContacts.sortDesc) {
                    _.reverse(contacts);
                  }

                  var headers = [
                    { field: 'name',              title: 'Name'         },
                    { field: 'phone',             title: 'Phone Number' },
                    { field: 'mobilePhoneNumber', title: 'Cell'         },
                    { field: 'faxPhoneNumber',    title: 'Fax'          },
                    { field: 'emailAddress',      title: 'Email'        },
                    { field: 'role',              title: 'Role'         },
                    { field: 'addContact',        title: 'Add Contact', style: { textAlign: 'right'  },
                      node: addContactButton,
                    },
                  ];

                  return <SortTable id="contact-list" sortField={ this.state.uiContacts.sortField } sortDesc={ this.state.uiContacts.sortDesc } onSort={ this.updateContactsUIState } headers={ headers }>
                    {
                      _.map(contacts, (contact) => {
                        return <tr key={ contact.id }>
                          <td>{ contact.isPrimary && <Glyphicon glyph="star" /> } { contact.name } </td>
                          <td>{ contact.phone }</td>
                          <td>{ contact.mobilePhoneNumber }</td>
                          <td>{ contact.faxPhoneNumber }</td>
                          <td><a href={ `mailto:${ contact.emailAddress }` } target="_blank">{ contact.emailAddress }</a></td>
                          <td>{ contact.role }</td>
                          <td style={{ textAlign: 'right' }}>
                            <ButtonGroup>
                              <DeleteButton name="Contact" hide={ !contact.canDelete  } onConfirm={ this.deleteContact.bind(this, contact) } />
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
                <SubHeader title="History"/>
                { project.historyEntity && (
                  <History historyEntity={ project.historyEntity } refresh={ !loading } />
                )}
              </Well>
            </Col>
          </Row>
        </div>
        {this.state.showEditDialog && (
          <ProjectsEditDialog
            show={this.state.showEditDialog}
            project={project}
            onClose={this.closeEditDialog}
            />
        )}
        {this.state.showNotesDialog && (
          <NotesDialog
            show={this.state.showNotesDialog}
            id={this.props.params.projectId}
            getNotes={Api.getProjectNotes}
            onSave={Api.addProjectNote}
            onUpdate={Api.updateNote}
            onClose={this.closeNotesDialog}
            notes={this.props.notes}/>
        )}
        {this.state.showDocumentsDialog && (
          <DocumentsListDialog
            show={this.state.showDocumentsDialog}
            parent={project}
            onClose={this.closeDocumentsDialog}/>
        )}
        {this.state.showAddRequestDialog && (
          <RentalRequestsAddDialog
            show={this.state.showAddRequestDialog}
            project={project}
            onClose={this.closeAddRequestDialog}
            onProjectSaved={this.gotoProject}/>
        )}
        {this.state.showTimeEntryDialog && (
          <TimeEntryDialog
            show={this.state.showTimeEntryDialog}
            onClose={this.closeTimeEntryDialog}
            multipleEntryAllowed={false}
            rentalAgreementId={this.state.rentalAgreement.id}/>
        )}
        {this.state.showContactDialog && (
          <ContactsEditDialog
            show={this.state.showContactDialog}
            contact={this.state.contact}
            onSave={this.saveContact}
            onClose={this.closeContactDialog}/>
        )}
      </div>
    );
  },
});


function mapStateToProps(state) {
  return {
    project: state.models.project,
    contact: state.models.contact,
    notes: state.models.projectNotes,
    attachments: state.models.projectAttachments,
    documents: state.models.documents,
    uiContacts: state.ui.projectContacts,
  };
}

export default connect(mapStateToProps)(ProjectsDetail);
