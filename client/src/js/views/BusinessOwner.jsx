import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { Row, Col, Alert, Badge } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import _ from 'lodash';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';

import Spinner from '../components/Spinner.jsx';
import ColDisplay from '../components/ColDisplay.jsx';
import SortTable from '../components/SortTable.jsx';
import ReturnButton from '../components/ReturnButton.jsx';
import PageHeader from '../components/ui/PageHeader.jsx';
import SubHeader from '../components/ui/SubHeader.jsx';
import PrintButton from '../components/PrintButton.jsx';

import { formatDateTime } from '../utils/date';
import { sortDir } from '../utils/array';
import { activeOwnerSelector } from '../selectors/ui-selectors';

class BusinessOwner extends React.Component {
  static propTypes = {
    owner: PropTypes.object,
    uiEquipment: PropTypes.object,
    uiContacts: PropTypes.object,
    params: PropTypes.object,
  };

  constructor(props) {
    super(props);

    this.state = {
      loading: true,

      success: false,

      contact: {},

      status: '',

      // Contacts
      uiContacts: {
        sortField: props.uiContacts.sortField || 'name',
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
    this.props.dispatch({
      type: Action.SET_ACTIVE_OWNER_ID_UI,
      ownerId: this.props.match.params.ownerId,
    });

    this.fetch().then(() => {
      this.setState({ loading: false });
    });
  }

  componentDidUpdate(prevProps) {
    if (this.props.match.params.ownerId !== prevProps.match.params.ownerId) {
      this.fetch();
    }
  }

  fetch = () => {
    const ownerId = this.props.match.params.ownerId;

    return this.props.dispatch(Api.getOwnerForBusiness(ownerId))
      .then(() => {
        this.setState({ success: true });
      })
      .catch(() => {
        this.setState({ success: false });
      });
  };

  updateContactsUIState = (state, callback) => {
    const dispatch = this.props.dispatch;
    this.setState({ uiContacts: { ...this.state.uiContacts, ...state } }, () => {
      dispatch({ type: Action.UPDATE_OWNER_CONTACTS_UI, ownerContacts: this.state.uiContacts });
      if (callback) {
        callback();
      }
    });
  };

  updateEquipmentUIState = (state, callback) => {
    const dispatch = this.props.dispatch;
    this.setState({ uiEquipment: { ...this.state.uiEquipment, ...state } }, () => {
      dispatch({ type: Action.UPDATE_OWNER_EQUIPMENT_UI, ownerEquipment: this.state.uiEquipment });
      if (callback) {
        callback();
      }
    });
  };

  render() {
    return (
      <div id="business-owner-screen">
        {this.state.loading && (
          <div className="spinner-container">
            <Spinner />
          </div>
        )}
        {!this.state.loading && this.state.success && this.renderPage()}
        {!this.state.loading && !this.state.success && this.renderError()}
      </div>
    );
  }

  renderPage = () => {
    var owner = this.props.owner;

    return (
      <div id="owners-detail">
        <div>
          <Row id="owners-top">
            <Col sm={9}>
              <Badge variant={owner?.status === Constant.OWNER_STATUS_CODE_APPROVED ? 'success' : 'danger'}>
                {owner?.status}
              </Badge>
              <Badge className={owner?.isMaintenanceContractor ? '' : 'd-none'} variant="secondary">
                Maintenance Contractor
              </Badge>
            </Col>
            <Col sm={3}>
              <div className="float-right">
                <PrintButton />
                <ReturnButton />
              </div>
            </Col>
          </Row>

          <PageHeader id="owners-header" title="Company" subTitle={owner.organizationName} />

          <div className="well">
            <SubHeader title="Owner Information" />
            <div id="owners-data">
              <Row className="equal-height">
                <Col lg={4} md={6} sm={12} xs={12}>
                  <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Company">
                    {owner.organizationName}
                  </ColDisplay>
                </Col>
                <Col lg={4} md={6} sm={12} xs={12}>
                  <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Company Address">
                    {owner.fullAddress}
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
                  <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Primary Contact">
                    {owner.primaryContactName}
                  </ColDisplay>
                </Col>
                <Col lg={4} md={6} sm={12} xs={12}>
                  <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Doing Business As">
                    {owner.doingBusinessAs}
                  </ColDisplay>
                </Col>
                <Col lg={4} md={6} sm={12} xs={12}>
                  <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Registered BC Company Number">
                    {owner.registeredCompanyNumber}
                  </ColDisplay>
                </Col>
                <Col lg={4} md={6} sm={12} xs={12}>
                  <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="District Office">
                    {owner.districtName}
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
                  <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Meets Residency?">
                    {owner.meetsResidency ? 'Yes' : 'No'}
                  </ColDisplay>
                </Col>
              </Row>
            </div>
          </div>
          <div className="well">
            <SubHeader title="Policy" />
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
          </div>
          <div className="well">
            <SubHeader title="Contacts" />
            {(() => {
              if (!owner.contacts || Object.keys(owner.contacts).length === 0) {
                return <Alert variant="success">No contacts</Alert>;
              }

              var contacts = _.orderBy(
                owner.contacts,
                [this.state.uiContacts.sortField],
                sortDir(this.state.uiContacts.sortDesc)
              );

              var headers = [
                { field: 'name', title: 'Name' },
                { field: 'phone', title: 'Phone' },
                { field: 'mobilePhoneNumber', title: 'Cell' },
                { field: 'faxPhoneNumber', title: 'Fax' },
                { field: 'emailAddress', title: 'Email' },
                { field: 'role', title: 'Role' },
              ];

              return (
                <SortTable
                  id="contact-list"
                  sortField={this.state.uiContacts.sortField}
                  sortDesc={this.state.uiContacts.sortDesc}
                  onSort={this.updateContactsUIState}
                  headers={headers}
                >
                  {_.map(contacts, (contact) => {
                    return (
                      <tr key={contact.id}>
                        <td>
                          {contact.isPrimary && <FontAwesomeIcon icon="star" />} {contact.name}
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
                      </tr>
                    );
                  })}
                </SortTable>
              );
            })()}
          </div>
          <div className="well">
            <SubHeader title={`Equipment (${owner.numberOfEquipment})`} />
            {(() => {
              if (!owner.equipmentList || owner.equipmentList.length === 0) {
                return <Alert variant="success">No equipment</Alert>;
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
                { field: 'attachments', title: 'Attachments' },
                { field: 'lastVerifiedDate', title: 'Last Verified' },
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
                        <td>{equipment.equipmentCode}</td>
                        <td>{equipment.localArea.name}</td>
                        <td>{equipment.typeName}</td>
                        <td>{equipment.details}</td>
                        <td>{_.map(equipment.equipmentAttachments, (a) => a.description).join(', ')}</td>
                        <td>
                          {equipment.isApproved
                            ? formatDateTime(equipment.lastVerifiedDate, Constant.DATE_YEAR_SHORT_MONTH_DAY)
                            : 'Not Approved'}
                        </td>
                      </tr>
                    );
                  })}
                </SortTable>
              );
            })()}
          </div>
        </div>
      </div>
    );
  };

  renderError = () => {
    return (
      <div id="owners-detail">
        <div>
          {(() => {
            return (
              <Row id="owners-top">
                <Col sm={9}></Col>
                <Col sm={3}>
                  <div className="float-right">
                    <ReturnButton />
                  </div>
                </Col>
              </Row>
            );
          })()}

          {(() => {
            return (
              <div id="owners-header">
                <Row>
                  <Col md={12}>
                    <h1>
                      <small>This record does not exist or you do not have permission to access it.</small>
                    </h1>
                  </Col>
                </Row>
              </div>
            );
          })()}
        </div>
      </div>
    );
  };
}

const mapStateToProps = (state) => ({
  owner: activeOwnerSelector(state),
  uiEquipment: state.ui.ownerEquipment,
  uiContacts: state.ui.ownerContacts,
});

const mapDispatchToProps = (dispatch) => ({ dispatch });

export default connect(mapStateToProps, mapDispatchToProps)(BusinessOwner);
