import React from 'react';

import { connect } from 'react-redux';

import { Well, Row, Col } from 'react-bootstrap';
import { Alert, Button, ButtonGroup, Glyphicon, Label } from 'react-bootstrap';
import { Link } from 'react-router';
import { LinkContainer } from 'react-router-bootstrap';

import _ from 'lodash';
import Promise from 'bluebird';

import ProjectsEditDialog from './dialogs/ProjectsEditDialog.jsx';
import ContactsEditDialog from './dialogs/ContactsEditDialog.jsx';
import DocumentsListDialog from './dialogs/DocumentsListDialog.jsx';

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
import Unimplemented from '../components/Unimplemented.jsx';

import { formatDateTime } from '../utils/date';
import { concat } from '../utils/string';

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

      includeCompletedRequests: false,

      contact: {},

      // Contacts
      uiContacts : {
        sortField: this.props.uiContacts.sortField || 'name',
        sortDesc: this.props.uiContacts.sortDesc === true,
      },
    };
  },

  componentDidMount() {
    this.fetch().then(() => {
      if (this.props.params.contactId) {
        this.openContact(this.props);
      }
    });
  },

  componentWillReceiveProps(nextProps) {
    if (!_.isEqual(nextProps.params, this.props.params)) {
      if (nextProps.params.contactId &&  nextProps.params.contactId !== this.props.params.contactId) {
        this.openContact(nextProps);
      }
    }
  },

  fetch() {
    this.setState({ loading: true });

    var getProjectPromise = Api.getProject(this.props.params.projectId);
    var documentsPromise = Api.getProjectDocuments(this.props.params.projectId);

    return Promise.all([getProjectPromise, documentsPromise]).finally(() => {
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

  saveEdit(project) {
    Api.updateProject(project).finally(() => {
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
    this.setState({ showContactDialog: false }, () => {
      //Reset project location
      this.props.router.push({
        pathname: this.props.project.path,
      });
    });
  },

  openContact(props) {
    var contact = null;

    if (props.params.contactId === '0') {
      // New Contact
      contact = {
        id: 0,
        project: props.project,
      };
    } else if (props.params.contactId) {
      // Select contact for viewing if possible
      contact = props.project.contacts[props.params.contactId];
    }

    if (contact) {
      this.openContactDialog(contact);
    } else {
      this.props.router.push({
        pathname: this.props.project.path,
      });
    }
  },

  addContact() {
    this.openContactDialog({ id: 0 });
    this.props.router.push({
      pathname: `${ this.props.project.path }/${ Constant.CONTACTS_PATHNAME }/0`,
    });
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

    var contactPromise = isNew ? Api.addProjectContact : Api.updateContact;
    var log = isNew ? Log.projectContactAdded : Log.projectContactUpdated;

    contactPromise(this.props.project, contact).then(() => {
      return log(this.props.project, this.props.contact).then(() => {
        if (contact.isPrimary) {
          return Api.updateProject({ ...this.props.project, ...{
            contacts: null,
            primaryContact: { id: this.state.contact.id },
          }});
        }
      });
    }).finally(() => {
      this.fetch();
      this.closeContactDialog();
    });
  },

  print() {

  },

  addRequest() {

  },

  render() {
    var project = this.props.project;

    return <div id="projects-detail">
      <div>
        <Row id="projects-top">
          <Col md={10}>
            <Label bsStyle={ project.isActive ? 'success' : 'danger'}>{ project.status }</Label>
            <Unimplemented>
              <Button title="Notes" onClick={ this.showNotes }>Notes ({ Object.keys(this.props.notes).length })</Button>
            </Unimplemented>
            <Button title="Documents" onClick={ this.showDocuments }>Documents ({ Object.keys(this.props.documents).length })</Button>
          </Col>
          <Col md={2}>
            <div className="pull-right">
              <Unimplemented>
                <Button onClick={ this.print }><Glyphicon glyph="print" title="Print" /></Button>
              </Unimplemented>
              <LinkContainer to={{ pathname: Constant.PROJECTS_PATHNAME }}>
                <Button title="Return to List"><Glyphicon glyph="arrow-left" /> Return to List</Button>
              </LinkContainer>
            </div>
          </Col>
        </Row>

        {(() => {
          if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

          return <div id="projects-header">
            <Row>
              <Col md={12}>
                <h1>Project: <small>{ project.name }</small></h1>
              </Col>
            </Row>
            <Row>
              <Col md={12}>
                <h1>District: <small>{ project.districtName }</small></h1>
              </Col>
            </Row>
          </div>;
        })()}

        <Row>
          <Col md={6}>
            <Well>
              <h3>Project Information <span className="pull-right">
                  <Button title="Edit Project" bsSize="small" onClick={ this.openEditDialog }><Glyphicon glyph="pencil" /></Button>
              </span></h3>
              {(() => {
                if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

                var mailto = <a href={ `mailto:${project.primaryContactEmail}` }>
                  { project.primaryContactName }
                </a>;

                return <div id="projects-data">
                  <Row>
                    <ColDisplay md={12} labelProps={{ md: 4 }} label="Project">{ project.name }</ColDisplay>
                  </Row>
                  <Row>
                    <ColDisplay md={12} labelProps={{ md: 4 }} label={ project.primaryContactRole || 'Primary Contact' }>
                      { project.primaryContactEmail ? mailto : `${project.primaryContactName}` }{ project.primaryContactPhone ? `, ${project.primaryContactPhone}` : '' }
                    </ColDisplay>
                  </Row>
                  <Row>
                    <ColDisplay md={12} labelProps={{ md: 4 }} label="Provincial Project Number">{ project.provincialProjectNumber }</ColDisplay>
                  </Row>
                  <Row>
                    <ColDisplay md={12} labelProps={{ md: 4 }} fieldProps={{ md: 7 }} label="Information">{ project.information }</ColDisplay>
                  </Row>
                </div>;
              })()}
            </Well>
            <Well>
              <h3>Hired Equipment ({ (project.numberOfRequests) }) <span className="pull-right">
                <Unimplemented>
                  <CheckboxControl id="includeCompletedRequests" inline checked={ this.state.includeCompletedRequests } updateState={ this.updateState }><small>Show Completed</small></CheckboxControl>
                </Unimplemented>
                <Unimplemented>
                  <Button title="Add Request" bsSize="small" onClick={ this.addRequest }><Glyphicon glyph="plus" /> Add</Button>
                </Unimplemented>
              </span></h3>
              {(() => {
                if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

                // As per business requirements:
                // "Lists the records - requests then rental agreements, within the groups, list in largest-to-smallest ID order (aka reverse chronological create)."
                var rentalRequests = _.orderBy(project.rentalRequests, ['id'], ['desc']);
                var rentalAgreements = _.orderBy(project.rentalAgreements, ['id'], ['desc']);
                var combinedList =_.concat(rentalRequests, rentalAgreements);

                // Exclude completed items
                if (!this.state.includeCompletedRequests) {
                  _.remove(combinedList, (x) => !x.isActive);
                }

                if (Object.keys(combinedList).length === 0) { return <Alert bsStyle="success" style={{ marginTop: 10 }}>No equipment</Alert>; }

                const RentalRequestListItem = ({ item }) => (
                  <tr key={ item.id }>
                    <td><Link to={ `rental-requests/${item.id}` }>Request</Link></td>
                    <td>{ item.equipmentTypeName }</td>
                    <td>TBD</td>
                    <td>N/A</td>
                  </tr>
                );

                // TODO Wire-up link to Time Records screen
                const TimeEntryLink = ({ item }) => (
                  <Link to={ '#' }>{ item.lastTimeRecord ? formatDateTime(item.lastTimeRecord, Constant.DATE_YEAR_SHORT_MONTH_DAY) : 'None' }</Link>
                );

                const RentalAgreementListItem = ({ item }) => (
                  <tr key={ item.id }>
                    <td><Link to={ `equipment/${item.equipmentId}` }>{ item.equipmentCode }</Link></td>
                    <td>{ item.equipmentTypeName }</td>
                    <td>{ concat(item.equipmentMake, concat(item.equipmentModel, item.equipmentSize, '/'), '/') }</td>
                    <td>{ item.isCompleted ? 'Completed' : <TimeEntryLink item={ item } /> }</td>
                  </tr>
                );

                var headers = [
                  { field: 'equipmentCode',     title: 'ID'               },
                  { field: 'equipmentTypeName', title: 'Type'             },
                  { field: 'equipmentMake',     title: 'Make/Model/Size'  },
                  { field: 'lastTimeRecord',    title: 'Time Entry'       },
                ];

                return <TableControl id="equipment-list" headers={ headers }>
                  {
                    _.map(combinedList, (listItem) => {
                      if (listItem.isRentalRequest) {
                        return <RentalRequestListItem item={ listItem } project={ project } />;
                      } else {
                        return <RentalAgreementListItem item={ listItem } />;
                      }
                    })
                  }
                </TableControl>;
              })()}
            </Well>
          </Col>
          <Col md={6}>
            <Well>
              <h3>Contacts</h3>
              {(() => {
                if (this.state.loading ) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

                var addContactButton = <Button title="Add Contact" onClick={ this.addContact } bsSize="xsmall"><Glyphicon glyph="plus" />&nbsp;<strong>Add</strong></Button>;

                if (!project.contacts || Object.keys(project.contacts).length === 0) { return <Alert bsStyle="success">No contacts <span className="pull-right">{ addContactButton }</span></Alert>; }
                
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
                              <EditButton name="Contact" view={ !contact.canEdit } pathname={ contact.path } />
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
                <History historyEntity={ project.historyEntity } refresh={ this.state.loading } />
              }
            </Well>
          </Col>
        </Row>
      </div>
      { this.state.showEditDialog &&
        <ProjectsEditDialog show={ this.state.showEditDialog } onSave={ this.saveEdit } onClose={ this.closeEditDialog } />  
      }
      { this.state.showContactDialog &&
        <ContactsEditDialog show={ this.state.showContactDialog } contact={ this.state.contact } onSave={ this.saveContact } onClose={ this.closeContactDialog } />
      }
      { this.state.showDocumentsDialog &&
        <DocumentsListDialog 
          show={ this.state.showDocumentsDialog } 
          parent={ project } 
          onClose={ this.closeDocumentsDialog } 
        />
      }
    </div>;
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
