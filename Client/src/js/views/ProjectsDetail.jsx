import React from 'react';

import { connect } from 'react-redux';

import { Well, Row, Col } from 'react-bootstrap';
import { Alert, Button, ButtonGroup, Glyphicon, Label } from 'react-bootstrap';
import { Link } from 'react-router';
import { LinkContainer } from 'react-router-bootstrap';

import _ from 'lodash';

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
    notes: React.PropTypes.object,
    attachments: React.PropTypes.object,
    history: React.PropTypes.object,
    params: React.PropTypes.object,
    ui: React.PropTypes.object,
  },

  getInitialState() {
    return {
      loading: false,
      loadingHistory: false,

      showEditDialog: false,
      showContactDialog: false,

      includeCompletedRequests: false,

      contact: {},

      isNew: this.props.params.projectId == 0,

      // Contacts
      ui : {
        sortField: this.props.ui.sortField || 'name',
        sortDesc: this.props.ui.sortDesc === true,
      },
    };
  },

  componentDidMount() {
    this.fetch();
  },

  fetch() {
    this.setState({ loading: true });
    Api.getProject(this.props.params.projectId).finally(() => {
      this.setState({ loading: false });
    });
  },

  updateState(state, callback) {
    this.setState(state, callback);
  },

  updateContactsUIState(state, callback) {
    this.setState({ ui: { ...this.state.ui, ...state }}, () => {
      store.dispatch({ type: Action.UPDATE_PROJECT_CONTACTS_UI, projectContacts: this.state.ui });
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
    // TODO Edit project data
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
            <Unimplemented>
              <Button title="Documents" onClick={ this.showDocuments }>Docs ({ Object.keys(this.props.attachments).length })</Button>
            </Unimplemented>
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
          </div>;
        })()}

        <Row>
          <Col md={6}>
            <Well>
              <h3>Project Information <span className="pull-right">
                <Unimplemented>
                  <Button title="Edit Project" bsSize="small" onClick={ this.openEditDialog }><Glyphicon glyph="pencil" /></Button>
                </Unimplemented>
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
                      { mailto }{ project.primaryContactPhone ? `, ${project.primaryContactPhone}` : '' }
                    </ColDisplay>
                  </Row>
                  <Row>
                    <ColDisplay md={12} labelProps={{ md: 4 }} label="District">{ project.districtName }</ColDisplay>
                  </Row>
                  <Row>
                    <ColDisplay md={12} labelProps={{ md: 4 }} label="Provincial Project Number">{ project.provincialProjectNumber }</ColDisplay>
                  </Row>
                  <Row>
                    <ColDisplay md={12} labelProps={{ md: 4 }} label="Information">{ project.information }</ColDisplay>
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

                const RentalRequestListItem = ({ item, project }) => (
                  <tr key={ item.id }>
                    <td><Link to={ `projects/${project.id}/requests/${item.id}` }>Request</Link></td>
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
              <h3>Contacts <span className="pull-right">
                <Unimplemented>
                  <Button title="Add Contact" onClick={ this.addContact } bsSize="small"><Glyphicon glyph="plus" /></Button>
                </Unimplemented>
              </span></h3>
              {(() => {
                if (this.state.loading ) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }
                if (!project.contacts || project.contacts.length === 0) { return <Alert bsStyle="success" style={{ marginTop: 10 }}>No contacts</Alert>; }

                var contacts = _.sortBy(project.contacts, this.state.ui.sortField);
                if (this.state.ui.sortDesc) {
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

                return <SortTable id="contact-list" sortField={ this.state.ui.sortField } sortDesc={ this.state.ui.sortDesc } onSort={ this.updateContactsUIState } headers={ headers }>
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
                if (this.state.loadingHistory) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }
                if (Object.keys(this.props.history).length === 0) { return <Alert bsStyle="success" style={{ marginTop: 10 }}>No history</Alert>; }

                var history = _.sortBy(this.props.history, 'createdDate');

                const HistoryEntry = ({ createdDate, historyText }) => (
                  <Row>
                    <ColDisplay md={12} labelProps={{ md: 2 }} label={ formatDateTime(createdDate, Constant.DATE_YEAR_SHORT_MONTH_DAY) }>
                      { historyText }
                    </ColDisplay>
                  </Row>
                );

                return <div id="projects-history">
                  {
                    _.map(history, (entry) => <HistoryEntry { ...entry } />)
                  }
                </div>;
              })()}
            </Well>
          </Col>
        </Row>
      </div>
      { /* TODO this.state.showEditDialog && <ProjectEditDialog /> */}
      { /* TODO this.state.showContactDialog && <ContactEditDialog /> */}
    </div>;
  },
});


function mapStateToProps(state) {
  return {
    project: state.models.project,
    notes: state.models.projectNotes,
    attachments: state.models.projectAttachments,
    history: state.models.projectHistory,
    ui: state.ui.projectContacts,
  };
}

export default connect(mapStateToProps)(ProjectsDetail);
