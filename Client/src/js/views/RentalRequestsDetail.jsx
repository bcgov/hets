import React from 'react';

import { connect } from 'react-redux';

import { Well, Row, Col } from 'react-bootstrap';
import { Alert, Button, Glyphicon, Label } from 'react-bootstrap';
import { Link } from 'react-router';
import { LinkContainer } from 'react-router-bootstrap';

import _ from 'lodash';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import store from '../store';

import CheckboxControl from '../components/CheckboxControl.jsx';
import ColDisplay from '../components/ColDisplay.jsx';
import Spinner from '../components/Spinner.jsx';
import TableControl from '../components/TableControl.jsx';
import Unimplemented from '../components/Unimplemented.jsx';

import { formatDateTime } from '../utils/date';
import { concat } from '../utils/string';

/*

TODO:
* Print / Notes / Docs / Contacts (TBD) / History / Request Status List / Clone

*/

var RentalRequestsDetail = React.createClass({
  propTypes: {
    rentalRequest: React.PropTypes.object,
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

      includeHiredEquipment: false,

      contact: {},

      isNew: this.props.params.rentalRequestId == 0,
    };
  },

  componentDidMount() {
    this.fetch();
  },

  fetch() {
    this.setState({ loading: true });
    Api.getRentalRequest(this.props.params.rentalRequestId).finally(() => {
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
    // TODO Edit rental request data
    this.setState({ showEditDialog: true });
  },

  closeEditDialog() {
    this.setState({ showEditDialog: false });
  },

  saveEdit(rentalRequest) {
    Api.updateRentalRequest(rentalRequest).finally(() => {
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

  saveContact() {
    // TODO Save contact
  },

  print() {

  },

  addRequest() {

  },
  
  cloneRequest() {
    // TODO
  },

  render() {
    var rentalRequest = this.props.rentalRequest;

    return <div id="rental-requests-detail">
      <div>
        <Row id="rental-requests-top">
          <Col md={10}>
            <Label bsStyle={ rentalRequest.isActive ? 'success' : rentalRequest.isCancelled ? 'danger' : 'default' }>{ rentalRequest.status }</Label>
            <Unimplemented>
              <Button title="Clone" onClick={ this.cloneRequest }>Clone</Button>
            </Unimplemented>
            <Unimplemented>
              <Button title="Notes" onClick={ this.showNotes }>Notes ({ Object.keys(this.props.notes).length })</Button>
            </Unimplemented>
            <Unimplemented>
              <Button title="Documents" onClick={ this.showDocuments }>Docs ({ Object.keys(this.props.attachments).length })</Button>
            </Unimplemented>
          </Col>
          <Col md={2}>
            <div className="pull-right">
              <LinkContainer to={{ pathname: 'rentalrequests' }}>
                <Button title="Return to List"><Glyphicon glyph="arrow-left" /> Return to List</Button>
              </LinkContainer>
            </div>
          </Col>
        </Row>

        {(() => {
          if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

          return <div id="rental-requests-header">
            <Row>
              <Col md={12}>
                <h1>Project: <small>{ rentalRequest.projectName }</small></h1>
              </Col>
            </Row>
          </div>;
        })()}

        <Row>
          <Col md={12}>
            <Well>
              <h3>Request Information <span className="pull-right">
                <Unimplemented>
                  <Button title="Edit Rental Request" bsSize="small" onClick={ this.openEditDialog }><Glyphicon glyph="edit" /></Button>
                </Unimplemented>
              </span></h3>              
              {(() => {
                if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

                return <div id="rental-requests-data">
                  <Row>
                    <ColDisplay md={12} labelProps={{ md: 4 }} label="Project">{ rentalRequest.projectName }</ColDisplay>
                  </Row>
                  <Row>
                    <ColDisplay md={12} labelProps={{ md: 4 }} label={ rentalRequest.primaryContactRole || 'Primary Contact' }>
                      <Unimplemented>
                        <Button bsStyle="link" title="show contact" onClick={ this.openContactDialog.bind(this, rentalRequest.primaryContact) }>
                          { concat(rentalRequest.primaryContactName, rentalRequest.primaryContactPhone, ', ') }
                        </Button>
                      </Unimplemented>
                    </ColDisplay>
                  </Row>
                  <Row>
                    <ColDisplay md={12} labelProps={{ md: 4 }} label="Local Area">{ rentalRequest.localAreaName }</ColDisplay>
                  </Row>
                  <Row>
                    <ColDisplay md={6} labelProps={{ md: 4 }} label="Equipment Type">{ rentalRequest.equipmentTypeName }</ColDisplay>
                    <ColDisplay md={6} labelProps={{ md: 2 }} label="Count">{ rentalRequest.equipmentCount }</ColDisplay>
                  </Row>
                  <Row>
                    <ColDisplay md={12} labelProps={{ md: 4 }} label="Attachment(s)">
                      { 'None' /* TODO */ }
                    </ColDisplay>
                  </Row>
                  <Row>
                    <ColDisplay md={12} labelProps={{ md: 4 }} label="Expected Hours">{ rentalRequest.expectedHours }</ColDisplay>
                  </Row>
                  <Row>
                    <ColDisplay md={12} labelProps={{ md: 4 }} label="Expected Start Date">
                      {  formatDateTime(rentalRequest.expectedStartDate, Constant.DATE_YEAR_SHORT_MONTH_DAY) }
                    </ColDisplay>
                  </Row>
                  <Row>
                    <ColDisplay md={12} labelProps={{ md: 4 }} label="Expected End Data">
                      { formatDateTime(rentalRequest.expectedEndDate, Constant.DATE_YEAR_SHORT_MONTH_DAY) }
                    </ColDisplay>
                  </Row>
                </div>;
              })()}
            </Well>            
            <Well>
              <h3>Request Status <span className="pull-right">
                <Unimplemented>
                  <Button onClick={ this.print }><Glyphicon glyph="print" title="Print" /></Button>
                </Unimplemented>
                <CheckboxControl id="includeHiredEquipment" inline checked={ this.state.includeHiredEquipment } updateState={ this.updateState }><small>Show Hired</small></CheckboxControl>
              </span></h3>
              {(() => {
                if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

                // Exclude equipment already hired 
                var rotationList = rentalRequest.rentalRequestRotationList;
                if (!this.state.includeHiredEquipment) {
                  rotationList = _.filter(rotationList, (x) => !x.isHired);
                }

                if (Object.keys(rotationList || []).length === 0) { return <Alert bsStyle="success" style={{ marginTop: 10 }}>No equipment</Alert>; }

                var headers = [
                  { field: 'seniority',            title: 'Seniority'         },
                  { field: 'serviceHoursThisYear', title: 'YTD Hours'         },
                  { field: 'equipmentCode',        title: 'Equipment ID'      },                  
                  { field: 'equipmentDetails',     title: 'Equipment Details' },
                  { field: 'primaryContactName',   title: 'Contact'           },
                  { field: 'status',               title: 'Status'            },
                ];

                return <TableControl id="equipment-list" headers={ headers }>
                  {
                    _.map(rotationList, (listItem) => {
                      return <tr key={ listItem.id }>
                        <td>{ listItem.seniority }</td>
                        <td>{ listItem.serviceHoursThisYear }</td>
                        <td><Link to={ `equipment/${listItem.equipmentId}` }>{ listItem.equipmentCode }</Link></td>
                        <td>{ listItem.equipmentDetails }</td>
                        <td>
                          <Unimplemented>
                            <Button bsStyle="link" title="show contact" onClick={ this.openContactDialog.bind(this, listItem.contact) }>
                              { concat(listItem.contactName, listItem.contactPhone, ': ') }
                            </Button>
                          </Unimplemented>
                        </td>
                        <td>
                          <Unimplemented>
                            <Link to={'#'}>{ listItem.status }</Link>
                          </Unimplemented>
                        </td>
                      </tr>;
                    })
                  }
                </TableControl>;
              })()}
            </Well>
          </Col>
        </Row>
        <Row>
          <Col md={12}>
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

                return <div id="rental-requests-history">
                  {
                    _.map(history, (entry) => <HistoryEntry { ...entry } />)
                  }
                </div>;
              })()}
            </Well>
          </Col>
        </Row>
      </div>
      { /* TODO this.state.showEditDialog && <RentalRequestEditDialog /> */}
      { /* TODO this.state.showContactDialog && <ContactEditDialog /> */}
    </div>;
  },
});


function mapStateToProps(state) {
  return {
    rentalRequest: state.models.rentalRequest,
    notes: state.models.rentalRequestNotes,
    attachments: state.models.rentalRequestAttachments,
    history: state.models.rentalRequestHistory,
  };
}

export default connect(mapStateToProps)(RentalRequestsDetail);
