import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import { Row, Col, FormGroup, Alert, Button, FormText } from 'react-bootstrap';
import _ from 'lodash';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import store from '../store';

import PageHeader from '../components/ui/PageHeader.jsx';
import Spinner from '../components/Spinner.jsx';
import ColDisplay from '../components/ColDisplay.jsx';
import SortTable from '../components/SortTable.jsx';
import FormInputControl from '../components/FormInputControl.jsx';
import Form from '../components/Form.jsx';
import SubHeader from '../components/ui/SubHeader.jsx';

class BusinessPortal extends React.Component {
  static propTypes = {
    user: PropTypes.object,
    business: PropTypes.object,
    uiOwners: PropTypes.object,
    match: PropTypes.object,
  };

  constructor(props) {
    super(props);

    this.state = {
      loading: false,
      validating: false,
      success: false,
      secretKey: '',
      postalCode: '',
      errors: {},

      // owners
      uiOwners: {
        sortField: props.uiOwners.sortField || 'organizationName',
        sortDesc: props.uiOwners.sortDesc === true,
      },
    };
  }

  componentDidMount() {
    const businessLoaded = Boolean(this.props.business);
    this.setState({ loading: !businessLoaded, success: businessLoaded });

    this.fetch().then(() => {
      this.setState({ loading: false });
    });
  }

  fetch = () => {
    return Api.getBusiness().then(() => {
      this.setState({ success: !_.isEmpty(this.props.business) });
    });
  };

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  updateOwnersUIState = (state, callback) => {
    this.setState({ uiOwners: { ...this.state.uiOwners, ...state } }, () => {
      store.dispatch({ type: Action.UPDATE_OWNERS_UI, owners: this.state.uiOwners });
      if (callback) {
        callback();
      }
    });
  };

  validateOwner = (e) => {
    e.preventDefault();
    //for React versions below v.17 this is needed as the e.target becomes null due to event pooling. Causing a crash when trying to clear input fields.
    //source: https://reactjs.org/docs/events.html
    //https://stackoverflow.com/questions/44708518/event-target-is-null-inside-functional-setstate
    e.persist();

    this.setState({ validating: true, errors: {} });
    Api.validateOwner(this.state.secretKey, this.state.postalCode)
      .then(() => {
        debugger;
        // clear input fields
        e.target.postalCode.value = '';
        e.target.secretKey.value = '';
      })
      .catch((error) => {
        if (
          error.status === 400 &&
          (error.errorCode === 'HETS-19' || error.errorCode === 'HETS-20' || error.errorCode === 'HETS-21')
        ) {
          this.setState({ errors: { secretKey: error.errorDescription } });
        } else if (error.status === 400 && error.errorCode === 'HETS-22') {
          this.setState({ errors: { postalCode: error.errorDescription } });
        } else {
          throw error;
        }
      })
      .finally(() => {
        this.setState({ validating: false });
      });
  };

  render() {
    return (
      <div id="business-portal">
        <PageHeader>Business Portal</PageHeader>
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
    var business = this.props.business;
    const hasErrors = Object.keys(this.state.errors).length > 0;

    return (
      <div>
        <div className="well" id="business-info">
          <SubHeader title="Business Information" />
          <Row>
            <Col lg={6} md={6} sm={12} xs={12}>
              <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Legal Name">
                {business.bceidLegalName}
              </ColDisplay>
            </Col>
            <Col lg={6} md={6} sm={12} xs={12}>
              <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Doing Business As">
                {business.bceidDoingBusinessAs}
              </ColDisplay>
            </Col>
          </Row>
        </div>
        <div className="well" id="owners">
          <SubHeader title="HETS District Owners Associated With Your BCeID" />
          {(() => {
            if (_.isEmpty(this.props.business.owners)) {
              return <Alert variant="success">No district owners associated</Alert>;
            }

            var owners = _.sortBy(this.props.business.owners, this.state.uiOwners.sortField);
            if (this.state.uiOwners.sortDesc) {
              _.reverse(owners);
            }

            var headers = [
              { field: 'organizationName', title: 'Name' },
              { field: 'primaryContactName', title: 'Primary Contact' },
              { field: 'districtName', title: 'District' },
              { field: 'localAreaName', title: 'Local Area' },
            ];

            return (
              <SortTable
                id="owner-list"
                sortField={this.state.uiOwners.sortField}
                sortDesc={this.state.uiOwners.sortDesc}
                onSort={this.updateOwnersUIState}
                headers={headers}
              >
                {_.map(owners, (owner) => {
                  return (
                    <tr key={owner.id}>
                      <td>
                        <Link to={`${Constant.BUSINESS_DETAILS_PATHNAME}/${owner.id}`}> {owner.organizationName}</Link>
                      </td>
                      <td>{owner.primaryContactNameBusiness}</td>
                      <td>{owner.districtNameBusiness}</td>
                      <td>{owner.localAreaNameBusiness}</td>
                    </tr>
                  );
                })}
              </SortTable>
            );
          })()}
        </div>
        <div className="well" id="associate-owner">
          <SubHeader title="Associate HETS District Owner" />
          <div id="overview">
            <Row>
              <img
                id="hets-logo"
                title="Hired Equipment Tracking System"
                alt="Hired Equipment Tracking System"
                src="images/gov/hets.jpg"
              />
              <p>
                The Hired Equipment Program is for owners/operators who have a dump truck, bulldozer, backhoe or other
                piece of equipment they want to hire out to the Ministry Transportation and Infrastructure for day
                labour and emergency projects.
              </p>
              <p>
                The Hired Equipment Program distributes available work to local equipment owners. The program is based
                on seniority and is designed to deliver work to registered users fairly and efficiently through the
                development of local area call-out lists. Details about the Hired Equipment Program can be found{' '}
                <a href="https://www2.gov.bc.ca/gov/content/industry/construction-industry/transportation-infrastructure/hired-equipment-program">
                  here
                </a>
                .
              </p>
            </Row>
            <p>
              If you are NEW to the Hired Equipment Program, contact your{' '}
              <a href="https://www2.gov.bc.ca/gov/content/industry/construction-industry/transportation-infrastructure/hired-equipment-program/need-help">
                local district office
              </a>{' '}
              to register your company and equipment.
            </p>
            <p>
              If you are REGISTERED with the Hired Equipment Program and this is your first time to the site, enter your
              Secret Key and Postal Code to validate your account, then select your account.
            </p>
            <p>For RETURNING equipment owners, select your company above to view your account.</p>
          </div>
          <Form inline onSubmit={this.validateOwner}>
            <FormGroup controlId="secretKey">
              <FormInputControl
                type="text"
                placeholder="Please enter your secret key here"
                className="mr-5"
                disabled={this.state.validating}
                defaultValue={this.state.secretKey}
                updateState={this.updateState}
                isInvalid={this.state.errors.secretKey}
              />
              <FormText>{this.state.errors.secretKey}</FormText>
            </FormGroup>
            <FormGroup controlId="postalCode">
              <FormInputControl
                type="text"
                placeholder="Postal code"
                className="mr-5"
                disabled={this.state.validating}
                defaultValue={this.state.postalCode}
                updateState={this.updateState}
                isInvalid={this.state.errors.postalCode}
              />
              <FormText>{this.state.errors.postalCode}</FormText>
            </FormGroup>
            <Button type="submit" disabled={this.state.validating}>
              Validate {this.state.validating && <Spinner />}
            </Button>
          </Form>
          {hasErrors && <div className="validation-error">Secret key validation failed.</div>}
        </div>
      </div>
    );
  };

  renderError = () => {
    return (
      <h1>
        <small>An error was encountered. You may not have permission to access this page.</small>
      </h1>
    );
  };
}

function mapStateToProps(state) {
  return {
    user: state.user,
    business: state.models.business,
    uiOwners: state.ui.owners,
  };
}

export default connect(mapStateToProps)(BusinessPortal);
