import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import _ from 'lodash';
import {
  Navbar,
  Nav,
  NavItem,
  NavDropdown,
  MenuItem,
  OverlayTrigger,
  Dropdown,
  Popover,
  Button,
  Glyphicon,
  ControlLabel,
  FormGroup,
} from 'react-bootstrap';
import NavLinkWrapper from '../components/ui/NavLinkWrapper';

import * as Constant from '../constants';
import * as Api from '../api';
import { logout } from '../Keycloak';

import Spinner from '../components/Spinner.jsx';
import DropdownControl from '../components/DropdownControl.jsx';

import { formatDateTimeUTCToLocal } from '../utils/date';

class TopNav extends React.Component {
  static propTypes = {
    currentUser: PropTypes.object,
    showWorkingIndicator: PropTypes.bool,
    showNav: PropTypes.bool,
    currentUserDistricts: PropTypes.object,
    rolloverStatus: PropTypes.object,
    history: PropTypes.object, //from react-router-dom
  };

  static defaultProps = {
    showNav: true,
  };

  updateUserDistrict = (state) => {
    var district = _.find(this.props.currentUserDistricts.data, (district) => {
      return district.district.id === state.districtId;
    });
    Api.switchUserDistrict(district.id).then(() => {
      this.props.history.push(Constant.HOME_PATHNAME);
      window.location.reload();
    });
  };

  dismissRolloverNotice = () => {
    Api.dismissRolloverMessage(this.props.currentUser.district.id);
  };

  pathMatch = (path) => {
    return this.props.location.pathname === path;
  };

  render() {
    var userDistricts = _.map(this.props.currentUserDistricts.data, (district) => {
      return {
        ...district,
        districtName: district.district.name,
        id: district.district.id,
      };
    });

    var navigationDisabled = this.props.rolloverStatus.rolloverActive;

    var environmentClass = '';
    if (this.props.currentUser.environment === 'Development') {
      environmentClass = 'env-dev';
    } else if (this.props.currentUser.environment === 'Test') {
      environmentClass = 'env-test';
    } else if (this.props.currentUser.environment === 'Training') {
      environmentClass = 'env-trn';
    } else if (this.props.currentUser.environment === 'UAT') {
      environmentClass = 'env-uat';
    }

    return (
      <div id="header">
        <nav id="header-main" className="navbar navbar-fixed-top">
          <div className="container">
            <div id="logo">
              <a href="http://www2.gov.bc.ca/gov/content/home">
                <img title="Government of B.C." alt="Government of B.C." src="images/gov/gov3_bc_logo.png" />
              </a>
            </div>
            <h1 id="banner">MOTI Hired Equipment Tracking System</h1>
            <div id="working-indicator" hidden={!this.props.showWorkingIndicator}>
              Working <Spinner />
            </div>
          </div>
          <Navbar id="top-nav" className={environmentClass}>
            {this.props.showNav && (
              <Nav>
                <NavLinkWrapper
                  tag={NavItem}
                  to={Constant.HOME_PATHNAME}
                  disabled={navigationDisabled}
                  active={this.pathMatch(Constant.HOME_PATHNAME)}
                >
                  Home
                </NavLinkWrapper>
                <NavLinkWrapper
                  tag={NavItem}
                  to={Constant.OWNERS_PATHNAME}
                  disabled={navigationDisabled}
                  active={this.pathMatch(Constant.OWNERS_PATHNAME)}
                >
                  Owners
                </NavLinkWrapper>
                <NavLinkWrapper
                  tag={NavItem}
                  to={Constant.EQUIPMENT_PATHNAME}
                  disabled={navigationDisabled}
                  active={this.pathMatch(Constant.EQUIPMENT_PATHNAME)}
                >
                  Equipment
                </NavLinkWrapper>
                <NavLinkWrapper
                  tag={NavItem}
                  to={Constant.PROJECTS_PATHNAME}
                  disabled={navigationDisabled}
                  active={this.pathMatch(Constant.PROJECTS_PATHNAME)}
                >
                  Projects
                </NavLinkWrapper>
                <NavLinkWrapper
                  tag={NavItem}
                  to={Constant.RENTAL_REQUESTS_PATHNAME}
                  disabled={navigationDisabled}
                  active={this.pathMatch(Constant.RENTAL_REQUESTS_PATHNAME)}
                >
                  Requests
                </NavLinkWrapper>
                <NavLinkWrapper
                  tag={NavItem}
                  to={Constant.TIME_ENTRY_PATHNAME}
                  disabled={navigationDisabled}
                  active={this.pathMatch(Constant.TIME_ENTRY_PATHNAME)}
                >
                  Time Entry
                </NavLinkWrapper>

                <NavDropdown id="reports-dropdown" title="Reports" disabled={navigationDisabled}>
                  <NavLinkWrapper
                    tag={MenuItem}
                    to={Constant.AIT_REPORT_PATHNAME}
                    disabled={navigationDisabled}
                    active={this.pathMatch(Constant.AIT_REPORT_PATHNAME)} //active prop needed so NavDropdown will highlight if one of chidlren is active
                  >
                    Rental Agreement Summary
                  </NavLinkWrapper>
                  <NavLinkWrapper
                    tag={MenuItem}
                    to={Constant.SENIORITY_LIST_PATHNAME}
                    disabled={navigationDisabled}
                    active={this.pathMatch(Constant.SENIORITY_LIST_PATHNAME)}
                  >
                    Seniority List
                  </NavLinkWrapper>
                  <NavLinkWrapper
                    tag={MenuItem}
                    to={Constant.STATUS_LETTERS_REPORT_PATHNAME}
                    disabled={navigationDisabled}
                    active={this.pathMatch(Constant.STATUS_LETTERS_REPORT_PATHNAME)}
                  >
                    Status Letters / Mailing Labels
                  </NavLinkWrapper>
                  <NavLinkWrapper
                    tag={MenuItem}
                    to={Constant.HIRING_REPORT_PATHNAME}
                    disabled={navigationDisabled}
                    active={this.pathMatch(Constant.HIRING_REPORT_PATHNAME)}
                  >
                    Hiring Report - Not Hired / Force Hire
                  </NavLinkWrapper>
                  <NavLinkWrapper
                    tag={MenuItem}
                    to={Constant.OWNERS_COVERAGE_PATHNAME}
                    disabled={navigationDisabled}
                    active={this.pathMatch(Constant.OWNERS_COVERAGE_PATHNAME)}
                  >
                    WCB / CGL Coverage
                  </NavLinkWrapper>
                </NavDropdown>
                {this.props.currentUser.hasPermission(Constant.PERMISSION_DISTRICT_CODE_TABLE_MANAGEMENT) && (
                  <NavLinkWrapper
                    tag={NavItem}
                    to={Constant.DISTRICT_ADMIN_PATHNAME}
                    disabled={navigationDisabled}
                    active={this.pathMatch(Constant.DISTRICT_ADMIN_PATHNAME)}
                  >
                    District Admin
                  </NavLinkWrapper>
                )}
                {(this.props.currentUser.hasPermission(Constant.PERMISSION_ADMIN) ||
                  this.props.currentUser.hasPermission(Constant.PERMISSION_USER_MANAGEMENT) ||
                  this.props.currentUser.hasPermission(Constant.PERMISSION_ROLES_AND_PERMISSIONS) ||
                  this.props.currentUser.hasPermission(Constant.PERMISSION_DISTRICT_ROLLOVER) ||
                  this.props.currentUser.hasPermission(Constant.PERMISSION_VERSION)) && (
                  <NavDropdown id="admin-dropdown" title="Administration" disabled={navigationDisabled}>
                    {this.props.currentUser.hasPermission(Constant.PERMISSION_ADMIN) && (
                      <NavLinkWrapper
                        tag={MenuItem}
                        to={Constant.OVERTIME_RATES_PATHNAME}
                        disabled={navigationDisabled}
                        active={this.pathMatch(Constant.OVERTIME_RATES_PATHNAME)}
                      >
                        Manage OT Rates
                      </NavLinkWrapper>
                    )}
                    {this.props.currentUser.hasPermission(Constant.PERMISSION_USER_MANAGEMENT) && (
                      <NavLinkWrapper
                        tag={MenuItem}
                        to={Constant.USERS_PATHNAME}
                        disabled={navigationDisabled}
                        active={this.pathMatch(Constant.USERS_PATHNAME)}
                      >
                        User Management
                      </NavLinkWrapper>
                    )}
                    {this.props.currentUser.hasPermission(Constant.PERMISSION_ROLES_AND_PERMISSIONS) && (
                      <NavLinkWrapper
                        tag={MenuItem}
                        to={Constant.ROLES_PATHNAME}
                        disabled={navigationDisabled}
                        active={this.pathMatch(Constant.ROLES_PATHNAME)}
                      >
                        Roles and Permissions
                      </NavLinkWrapper>
                    )}
                    {this.props.currentUser.hasPermission(Constant.PERMISSION_DISTRICT_ROLLOVER) && (
                      <NavLinkWrapper
                        tag={MenuItem}
                        to={Constant.ROLLOVER_PATHNAME}
                        disabled={navigationDisabled}
                        active={this.pathMatch(Constant.ROLLOVER_PATHNAME)}
                      >
                        Roll Over
                      </NavLinkWrapper>
                    )}
                    {this.props.currentUser.hasPermission(Constant.PERMISSION_VERSION) && (
                      <NavLinkWrapper
                        tag={MenuItem}
                        to={Constant.VERSION_PATHNAME}
                        disabled={navigationDisabled}
                        active={this.pathMatch(Constant.VERSION_PATHNAME)}
                      >
                        Version Info
                      </NavLinkWrapper>
                    )}
                  </NavDropdown>
                )}
              </Nav>
            )}
            {this.props.showNav && (
              <div id="navbar-right" className="pull-right">
                {this.props.rolloverStatus.displayRolloverMessage && this.props.rolloverStatus.rolloverComplete && (
                  <OverlayTrigger
                    trigger="click"
                    placement="bottom"
                    rootClose
                    overlay={
                      <Popover id="rollover-notice" title="Roll Over Complete">
                        <p>
                          The hired equipment roll over has been completed on{' '}
                          {formatDateTimeUTCToLocal(
                            this.props.rolloverStatus.rolloverEndDate,
                            Constant.DATE_TIME_READABLE
                          )}
                          .
                        </p>
                        <p>
                          <strong>Note: </strong>Please save/print out the new seniority lists for all equipments
                          corresponding to each local area.
                        </p>
                        <Button onClick={this.dismissRolloverNotice} bsStyle="primary">
                          Dismiss
                        </Button>
                      </Popover>
                    }
                  >
                    <Button id="rollover-notice-button" className="mr-5" bsStyle="info" bsSize="xsmall">
                      Roll Over Complete
                      <Glyphicon glyph="exclamation-sign" />
                    </Button>
                  </OverlayTrigger>
                )}
                <Dropdown id="profile-menu">
                  <Dropdown.Toggle>
                    <Glyphicon glyph="user" />
                  </Dropdown.Toggle>
                  <Dropdown.Menu>
                    <div>{this.props.currentUser.fullName}</div>
                    <FormGroup controlId="districtId">
                      <ControlLabel>District</ControlLabel>
                      <DropdownControl
                        id="districtId"
                        updateState={this.updateUserDistrict}
                        selectedId={this.props.currentUser.district.id}
                        fieldName="districtName"
                        items={userDistricts}
                      />
                    </FormGroup>
                    <Button onClick={() => logout()} bsStyle="primary">
                      Logout
                    </Button>
                  </Dropdown.Menu>
                </Dropdown>
              </div>
            )}
          </Navbar>
        </nav>
      </div>
    );
  }
}

function mapStateToProps(state) {
  return {
    currentUser: state.user,
    showWorkingIndicator: state.ui.requests.waiting,
    currentUserDistricts: state.models.currentUserDistricts,
    rolloverStatus: state.lookups.rolloverStatus,
  };
}

export default connect(mapStateToProps, null, null, { pure: false })(withRouter(TopNav));
