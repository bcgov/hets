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

      showEditDialog: false,
      showContactDialog: false,
      showAddRequestDialog: false,
      showTimeEntryDialog: false,
      showNotesDialog: false,

      includeCompletedRequests: false,

      contact: {},
      
      rentalRequest: {},

      // Contacts
      uiContacts : {
        sortField: this.props.uiContacts.sortField || 'name',
        sortDesc: this.props.uiContacts.sortDesc === true,
      },
    };
  },

  componentDidMount() {
    this.fetch();
  },

  fetch() {
    this.setState({ loading: true });

    var projectId = this.props.params.projectId;
    var getProjectPromise = Api.getProject(projectId);
    var documentsPromise = Api.getProjectDocuments(projectId);
    var getProjectNotesPromise = Api.getProjectNotes(projectId);

    return Promise.all([getProjectPromise, documentsPromise, getProjectNotesPromise]).finally(() => {
      this.setState({ loading: false });
    });
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

  saveEdit(project) {
    Api.updateProject(project).finally(() => {
      Log.projectModified(this.props.project);
      this.closeEditDialog();
    });
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
    this.setState({ showContactDialog: false });
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
      return log(this.props.project, this.props.contact);
    }).finally(() => {
      this.fetch();
      this.closeContactDialog();
    });
  },

  print() {
    window.print();
  },

  openAddRequestDialog() {
    this.setState({ showAddRequestDialog: true });
  },

  closeAddRequestDialog() {
    this.setState({ showAddRequestDialog: false });
  },

  saveNewRequest(request) {
    Api.addRentalRequest(request).then((response) => {
      // Open it up
      this.props.router.push({
        pathname: `${ Constant.RENTAL_REQUESTS_PATHNAME }/${ response.id }`,
      });
      Log.projectRentalRequestAdded(this.props.project, response);
    });
  },

  confirmEndHire(item) {
    Api.releaseRentalAgreement(item.id).then(() => {
      Api.getProject(this.props.params.projectId);
      Log.projectEquipmentReleased(this.props.project, item.equipment);
    });
  },

  openTimeEntryDialog(rentalRequest) {
    this.setState({ rentalRequest: rentalRequest }, () => {
      this.setState({ showTimeEntryDialog: true });
    });
  },

  closeTimeEntryDialog() {
    this.setState({ showTimeEntryDialog: false });
  },


  cancelRequest(request) {
    Api.cancelRentalRequest(request.id).then(() => {
      this.fetch();
    });
  },

  render() {
    var project = this.props.project;

    // As per business requirements:
    // "Lists the records - requests then rental agreements, within the groups, list in largest-to-smallest ID order (aka reverse chronological create)."
    var rentalRequests = _.orderBy(project.rentalRequests, ['id'], ['desc']);
    var rentalAgreements = _.orderBy(project.rentalAgreements, ['id'], ['desc']);
    var combinedList =_.concat(rentalRequests, rentalAgreements);
    // Exclude completed items
    if (!this.state.includeCompletedRequests) {
      _.remove(combinedList, (x) => !x.isActive);
    }

    return (
      <div id="projects-detail">
        <div>
          {(() => {
            if (this.state.loading) { return <div className="spinner-container"><Spinner/></div>; }

            return (
              <div className="top-container">
                <Row id="projects-top">
                  <Col sm={9}>
                    <Label bsStyle={ project.isActive ? 'success' : 'danger'}>{ project.status }</Label>
                    <Button title="Notes" onClick={ this.showNotes }>Notes ({ Object.keys(this.props.notes).length })</Button>
                    <Button title="Documents" onClick={ this.showDocuments }>Documents ({ Object.keys(this.props.documents).length })</Button>
                  </Col>
                  <Col sm={3}>
                    <div className="pull-right">
                      <Button onClick={ this.print }><Glyphicon glyph="print" title="Print" /></Button>
                      <Button title="Return" onClick={ browserHistory.goBack }><Glyphicon glyph="arrow-left" /> Return</Button>
                    </div>
                  </Col>
                </Row>
                <Row id="projects-header">
                  <Col md={12}>
                    <h1>Project: <small>{ project.name }</small></h1>
                  </Col>
                </Row>
                <Row>
                  <Col md={12}>
                    <h1>District: <small>{ project.districtName }</small></h1>
                  </Col>
                </Row>
              </div>
            );
          })()}

          <Row>
            <Col md={12}>
              <Well>
                <h3 className="clearfix">Project Information <span className="pull-right">
                  <Button title="Edit Project" bsSize="small" onClick={ this.openEditDialog }><Glyphicon glyph="pencil" /></Button>
                </span></h3>
                {(() => {
                  if (this.state.loading) { return <div className="spinner-container"><Spinner/></div>; }

                  var mailto = <a href={ `mailto:${project.primaryContactEmail}` }>
                    { project.primaryContactName }
                  </a>;

                  return <Row id="projects-data" className="equal-height">
                    <Col lg={4} md={6} sm={12} xs={12}>
                      <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Project">{ project.name }</ColDisplay>
                    </Col>
                    <Col lg={4} md={6} sm={12} xs={12}>
                      <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label={ project.primaryContactRole || 'Primary Contact' }>
                        { project.primaryContactEmail ? mailto : `${project.primaryContactName}` }{ project.primaryContactPhone ? `, ${project.primaryContactPhone}` : '' }
                      </ColDisplay>
                    </Col>
                    <Col lg={4} md={6} sm={12} xs={12}>
                      <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Provincial Project Number">{ project.provincialProjectNumber }</ColDisplay>
                    </Col>
                    <Col lg={4} md={6} sm={12} xs={12}>
                      <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Information">{ project.information }</ColDisplay>
                    </Col>
                  </Row>;
                })()}
              </Well>
              <Well>
                <h3>Hired Equipment / Requests<span className="pull-right">
                  <CheckboxControl id="includeCompletedRequests" inline checked={ this.state.includeCompletedRequests } updateState={ this.updateState }><small>Show Completed</small></CheckboxControl>
                  <Button title="Add Request" bsSize="small" onClick={ this.openAddRequestDialog }><Glyphicon glyph="plus" /> Add</Button>
                </span></h3>
                {(() => {
                  if (this.state.loading) { return <div className="spinner-container"><Spinner/></div>; }

                  if (Object.keys(combinedList).length === 0) { return <Alert bsStyle="success">No equipment</Alert>; }

                  const RentalRequestListItem = ({ item }) => (
                    <tr key={ item.id }>
                      <td>
                        <Link 
                          to={ `rental-requests/${item.id}` } 
                          className={item.status === Constant.RENTAL_REQUEST_STATUS_CODE_COMPLETED ? 'light' : ''}
                        >
                      Request
                        </Link>
                      </td>
                      <td>{ item.equipmentTypeName }</td>
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
                          className={item.status === Constant.RENTAL_REQUEST_STATUS_CODE_COMPLETED ? 'light' : ''}
                        >
                          { item.equipmentCode }
                        </Link>
                      </td>
                      <td>{ item.equipmentTypeName }</td>
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
                            overlay={ <Confirm onConfirm={ this.confirmEndHire.bind(this, item) }/> }
                          >
                            <Button 
                              bsSize="xsmall"
                            >
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
                    { field: 'equipmentTypeName', title: 'Type'             },
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
                <h3>Contacts</h3>
                {(() => {
                  if (this.state.loading ) { return <div className="spinner-container"><Spinner/></div>; }

                  var addContactButton = <Button title="Add Contact" onClick={ this.openContactDialog.bind(this, 0) } bsSize="small"><Glyphicon glyph="plus" />&nbsp;<strong>Add</strong></Button>;

                  if (!project.contacts || Object.keys(project.contacts).length === 0) { return <Alert bsStyle="success">No contacts { addContactButton }</Alert>; }
                
                  var contacts = _.sortBy(project.contacts, this.state.uiContacts.sortField);
                  if (this.state.uiContacts.sortDesc) {
                    _.reverse(contacts);
                  }

                  var headers = [
                    { field: 'name',  title: 'Name'         },
                    { field: 'phone', title: 'Phone Number' },
                    { field: 'emailAddress', title: 'Email'        },
                    { field: 'role',  title: 'Role'         },
                    { field: 'addContact',   title: 'Add Contact', style: { textAlign: 'right'  },
                      node: addContactButton,
                    },
                  ];

                  return <SortTable id="contact-list" sortField={ this.state.uiContacts.sortField } sortDesc={ this.state.uiContacts.sortDesc } onSort={ this.updateContactsUIState } headers={ headers }>
                    {
                      _.map(contacts, (contact) => {
                        return <tr key={ contact.id }>
                          <td>{ contact.isPrimary && <Glyphicon glyph="star" /> } { contact.name } </td>
                          <td>{ contact.phone }</td>
                          <td><a href={ `mailto:${ contact.emailAddress }` } target="_blank">{ contact.emailAddress }</a></td>
                          <td>{ contact.role }</td>
                          <td style={{ textAlign: 'right' }}>
                            <ButtonGroup>
                              <DeleteButton name="Contact" hide={ !contact.canDelete || contact.isPrimary } onConfirm={ this.deleteContact.bind(this, contact) } />
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
                <h3>History</h3>
                { project.historyEntity && 
                <History historyEntity={ project.historyEntity } refresh={ !this.state.loading } />
                }
              </Well>
            </Col>
          </Row>
        </div>
        { this.state.showEditDialog &&
        <ProjectsEditDialog show={ this.state.showEditDialog } onSave={ this.saveEdit } onClose={ this.closeEditDialog } />  
        }
        { this.state.showContactDialog &&
        <ContactsEditDialog 
          show={ this.state.showContactDialog } 
          contact={ this.state.contact } 
          onSave={ this.saveContact } 
          onClose={ this.closeContactDialog } 
        />
        }
        { this.state.showDocumentsDialog &&
        <DocumentsListDialog 
          show={ this.state.showDocumentsDialog } 
          parent={ project } 
          onClose={ this.closeDocumentsDialog } 
        />
        }
        { this.state.showAddRequestDialog &&
        <RentalRequestsAddDialog 
          show={ this.state.showAddRequestDialog } 
          onSave={ this.saveNewRequest } 
          onClose={ this.closeAddRequestDialog } 
          project={ project }
        />
        }
        { this.state.showTimeEntryDialog &&
        <TimeEntryDialog
          show={ this.state.showTimeEntryDialog }
          onClose={ this.closeTimeEntryDialog }
          activeRentalRequest={ this.state.rentalRequest }
        />
        }
        { this.state.showNotesDialog &&
        <NotesDialog 
          show={ this.state.showNotesDialog } 
          onSave={ Api.addProjectNote } 
          id={ this.props.params.projectId }
          getNotes={ Api.getProjectNotes }
          onUpdate={ Api.updateNote }
          onClose={ this.closeNotesDialog } 
          notes={ this.props.notes }
        />
        } 
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
