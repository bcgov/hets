import React from 'react';

import { connect } from 'react-redux';

import { browserHistory, Link } from 'react-router';

import { Well, Row, Col, Alert, Button, Glyphicon, Label } from 'react-bootstrap';
import _ from 'lodash';
import Promise from 'bluebird';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import store from '../store';

import Main from './Main.jsx';
import Spinner from '../components/Spinner.jsx';
import ColDisplay from '../components/ColDisplay.jsx';
import SortTable from '../components/SortTable.jsx';

import { formatDateTime } from '../utils/date';

var BusinessOwner = React.createClass({
  propTypes: {
    owner: React.PropTypes.object,
    equipment: React.PropTypes.object,
    contact: React.PropTypes.object,
    uiEquipment: React.PropTypes.object,
    uiContacts: React.PropTypes.object,
    params: React.PropTypes.object,
  },

  getInitialState() {
    return {
      loading: true,

      success: false,

      contact: {},

      status: '',

      // Contacts
      uiContacts : {
        sortField: this.props.uiContacts.sortField || 'name',
        sortDesc: this.props.uiContacts.sortDesc  === true,
      },

      // Equipment
      uiEquipment : {
        sortField: this.props.uiEquipment.sortField || 'equipmentCode',
        sortDesc: this.props.uiEquipment.sortDesc  === true,
      },
    };
  },

  componentDidMount() {
    this.fetch();
  },

  componentDidUpdate(prevProps) {
    if (this.props.params.ownerId !== prevProps.params.ownerId) {
      this.fetch();
    }
  },

  fetch() {
    this.setState({ loading: true });
    this.setState({ success: false });
    
    store.dispatch({ type: Action.UPDATE_OWNER, owner: null });

    var ownerId = this.props.params.ownerId;
    var ownerPromise = Api.getOwnerForBusiness(ownerId);

    return Promise.all([ownerPromise]).finally(() => {
      if (!_.isEmpty(this.props.owner)) {
        this.setState({ success: true });
      }
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

  print() {
    window.print();
  },

  render() {
    return (
      <Main showNav={false}>
        <div id="business-owner-screen">
          { this.state.loading && <div className="spinner-container"><Spinner/></div> }
          { !this.state.loading && this.state.success && this.renderPage() }
          { !this.state.loading && !this.state.success && this.renderError() }
        </div>
      </Main>
    );
  },

  renderPage() {
    var owner = this.props.owner;

    return <div id="owners-detail">
      <div>
        {(() => {

          return <Row id="owners-top">
            <Col sm={9}>
              <Label className='ml-5'>{ owner.status }</Label>
              <Label className={ owner.isMaintenanceContractor ? 'ml-5' : 'hide' }>Maintenance Contractor</Label>
            </Col>
            <Col sm={3}>
              <div className="pull-right">
                <Button className="mr-5" onClick={ this.print }><Glyphicon glyph="print" title="Print" /></Button>
                <Link to={ Constant.BUSINESS_PORTAL_PATHNAME } className="btn btn-default"><Glyphicon glyph="arrow-left" /> Return</Link>
              </div>
            </Col>
          </Row>;
        })()}

        {(() => {
          return <div id="owners-header">
            <Row>
              <Col md={12}>
                <h1>Company: <small>{ owner.organizationName }</small></h1>
              </Col>
            </Row>
          </div>;
        })()}

        <Row>
          <Col md={12}>
            <Well>
              <h3>Owner Information</h3>
              {(() => {
                return <div id="owners-data">
                  <Row className="equal-height">
                    <Col lg={4} md={6} sm={12} xs={12}>
                      <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Company">{ owner.organizationName }</ColDisplay>
                    </Col>
                    <Col lg={4} md={6} sm={12} xs={12}>
                      <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Company Address">{ owner.fullAddress }</ColDisplay>
                    </Col>
                    <Col lg={4} md={6} sm={12} xs={12}>
                      <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Owner Name">{ owner.ownerName }</ColDisplay>
                    </Col>
                    <Col lg={4} md={6} sm={12} xs={12}>
                      <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Owner Code">{ owner.ownerCode }</ColDisplay>
                    </Col>
                    <Col lg={4} md={6} sm={12} xs={12}>
                      <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Primary Contact">{ owner.primaryContactName }</ColDisplay>
                    </Col>
                    <Col lg={4} md={6} sm={12} xs={12}>
                      <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Doing Business As">{ owner.doingBusinessAs }</ColDisplay>
                    </Col>
                    <Col lg={4} md={6} sm={12} xs={12}>
                      <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Registered BC Company Number">{ owner.registeredCompanyNumber }</ColDisplay>
                    </Col>
                    <Col lg={4} md={6} sm={12} xs={12}>
                      <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="District Office">{ owner.districtName }</ColDisplay>
                    </Col>
                    <Col lg={4} md={6} sm={12} xs={12}>
                      <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Service/Local Area">{ owner.localAreaName }</ColDisplay>
                    </Col>
                    <Col lg={4} md={6} sm={12} xs={12}>
                      <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Meets Residency?">{ owner.meetsResidency ? 'Yes' : 'No' }</ColDisplay>
                    </Col>
                  </Row>
                </div>;
              })()}
            </Well>
          </Col>
          <Col md={12}>
            <Well>
              <h3>Policy</h3>
              {(() => {
                return <Row id="owners-policy" className="equal-height">
                  <Col lg={4} md={6} sm={12} xs={12}>
                    <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="WCB Number">{ owner.workSafeBCPolicyNumber }</ColDisplay>
                  </Col>
                  <Col lg={4} md={6} sm={12} xs={12}>
                    <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="WCB Expiry Date">
                      { formatDateTime(owner.workSafeBCExpiryDate, Constant.DATE_YEAR_SHORT_MONTH_DAY) }
                    </ColDisplay>
                  </Col>
                  <Col lg={4} md={6} sm={12} xs={12}>
                    <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="CGL Policy Number">
                      { owner.cglPolicyNumber }
                    </ColDisplay>
                  </Col>
                  <Col lg={4} md={6} sm={12} xs={12}>
                    <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="CGL Policy End Date">
                      { formatDateTime(owner.cglEndDate, Constant.DATE_YEAR_SHORT_MONTH_DAY) }
                    </ColDisplay>
                  </Col>
                </Row>;
              })()}
            </Well>
          </Col>
          <Col md={12}>
            <Well>
              <h3>Contacts</h3>
              {(() => {
                if (!owner.contacts || Object.keys(owner.contacts).length === 0) { return <Alert bsStyle="success">No contacts</Alert>; }

                var contacts = _.sortBy(owner.contacts, this.state.uiContacts.sortField);
                if (this.state.uiContacts.sortDesc) {
                  _.reverse(contacts);
                }

                var headers = [
                  { field: 'name',         title: 'Name'  },
                  { field: 'phone',        title: 'Phone' },
                  { field: 'emailAddress', title: 'Email' },
                  { field: 'role',         title: 'Role'  },
                ];

                return <SortTable id="contact-list" sortField={ this.state.uiContacts.sortField } sortDesc={ this.state.uiContacts.sortDesc } onSort={ this.updateContactsUIState } headers={ headers }>
                  {
                    _.map(contacts, (contact) => {
                      return <tr key={ contact.id }>
                        <td>{ contact.isPrimary && <Glyphicon glyph="star" /> } { contact.name }</td>
                        <td>{ contact.phone }</td>
                        <td><a href={ `mailto:${ contact.emailAddress }` } target="_blank">{ contact.emailAddress }</a></td>
                        <td>{ contact.role }</td>
                      </tr>;
                    })
                  }
                </SortTable>;
              })()}
            </Well>
            <Well>
              <h3 className="clearfix">Equipment ({ owner.numberOfEquipment })</h3>
              {(() => {
                if (!owner.equipmentList || owner.equipmentList.length === 0) { return <Alert bsStyle="success">No equipment</Alert>; }

                var equipmentList = _.sortBy(owner.equipmentList, this.state.uiEquipment.sortField);
                if (this.state.uiEquipment.sortDesc) {
                  _.reverse(equipmentList);
                }

                var headers = [
                  { field: 'equipmentCode',    title: 'ID'                   },
                  { field: 'typeName',         title: 'Type'                 },
                  { field: 'details',          title: 'Make/Model/Size/Year' },
                  { field: 'attachments',      title: 'Attachments'          },
                  { field: 'lastVerifiedDate', title: 'Last Verified'        },
                ];

                return <SortTable id="equipment-list" sortField={ this.state.uiEquipment.sortField } sortDesc={ this.state.uiEquipment.sortDesc } onSort={ this.updateEquipmentUIState } headers={ headers }>
                  {
                    _.map(equipmentList, (equipment) => {
                      return <tr key={ equipment.id }>
                        <td>{ equipment.equipmentCode }</td>
                        <td>{ equipment.typeName }</td>
                        <td>{ equipment.details }</td>
                        <td>{ _.map(equipment.equipmentAttachments, a => a.description).join(', ') }</td>
                        <td>{ equipment.isApproved ? formatDateTime(equipment.lastVerifiedDate, Constant.DATE_YEAR_SHORT_MONTH_DAY) : 'Not Approved' }</td>
                      </tr>;
                    })
                  }
                </SortTable>;
              })()}
            </Well>
          </Col>
        </Row>
      </div>
    </div>;
  },

  renderError() {
    return <div id="owners-detail">
      <div>
        {(() => {
          return <Row id="owners-top">
            <Col sm={9}>
            </Col>
            <Col sm={3}>
              <div className="pull-right">
                <Button title="Return" onClick={ browserHistory.goBack }><Glyphicon glyph="arrow-left" /> Return</Button>
              </div>
            </Col>
          </Row>;
        })()}

        {(() => {
          return <div id="owners-header">
            <Row>
              <Col md={12}>
                <h1><small>This record does not exist or you do not have permission to access it.</small></h1>
              </Col>
            </Row>
          </div>;
        })()}
      </div>
    </div>;
  },
});

function mapStateToProps(state) {
  return {
    owner: state.models.owner,
    equipment: state.models.equipment,
    contact: state.models.contact,
    uiEquipment: state.ui.ownerEquipment,
    uiContacts: state.ui.ownerContacts,
  };
}

export default connect(mapStateToProps)(BusinessOwner);
