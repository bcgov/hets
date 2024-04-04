import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { withRouter, NavLink } from 'react-router-dom';
import _ from 'lodash';
import {
  Navbar,
  Nav,
  NavItem,
  NavDropdown,
  OverlayTrigger,
  Dropdown,
  Popover,
  Button,
  FormLabel,
  FormGroup,
  Container,
} from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

import * as Constant from '../constants';
import * as Api from '../api';
import { logout } from '../Keycloak';

import Spinner from '../components/Spinner.jsx';
import DropdownControl from '../components/DropdownControl.jsx';
import Authorize, { any } from '../components/Authorize';

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

  updateUserDistrict = async (state) => {
    const district = _.find(this.props.currentUserDistricts.data, (district) => {
      return district.district.id === parseInt(state.districtId);
    });
    await this.props.dispatch(Api.switchUserDistrict(district.id));
    this.props.history.push(Constant.HOME_PATHNAME);
    window.location.reload();
  };

  dismissRolloverNotice = () => {
    this.props.dispatch(Api.dismissRolloverMessage(this.props.currentUser.district.id));
  };

  render() {
    const dispatch = this.props.dispatch;
    const userDistricts = _.map(this.props.currentUserDistricts.data, (district) => {
      return {
        ...district,
        districtName: district.district.name,
        id: district.district.id,
      };
    });

    const navigationDisabled = this.props.rolloverStatus.rolloverActive;

    let environmentClass = '';
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
      <div id="header" className="sticky-top">
        <Navbar id="header-main">
          <Container className={'justify-content-start'}>
            <Navbar.Brand href="http://www2.gov.bc.ca/gov/content/home">
              <div id="logo">
                <img
                  title="Government of B.C."
                  alt="Government of B.C."
                  src={`${process.env.PUBLIC_URL}/images/gov/gov3_bc_logo.png`}
                />
              </div>
            </Navbar.Brand>
            <h1 id="banner">MOTI Hired Equipment Tracking System</h1>
            <div id="working-indicator" hidden={!this.props.showWorkingIndicator}>
              Working <Spinner />
            </div>
          </Container>
        </Navbar>
        <Navbar id="top-nav" expand="md" className={environmentClass}>
          <Container>
            {this.props.showNav && (
              <>
                <Navbar.Toggle aria-controls="responsive-navbar-nav" className="btn-custom" />
                <Navbar.Collapse>
                  <Nav as="ul">
                    <Nav.Item as="li">
                      <Nav.Link as={NavLink} to={Constant.HOME_PATHNAME} disabled={navigationDisabled}>
                        Home
                      </Nav.Link>
                    </Nav.Item>
                    <Nav.Item as="li">
                      <Nav.Link as={NavLink} tag={NavItem} to={Constant.OWNERS_PATHNAME} disabled={navigationDisabled}>
                        Owners
                      </Nav.Link>
                    </Nav.Item>
                    <Nav.Item as="li">
                      <Nav.Link as={NavLink} to={Constant.EQUIPMENT_PATHNAME} disabled={navigationDisabled}>
                        Equipment
                      </Nav.Link>
                    </Nav.Item>
                    <Nav.Item as="li">
                      <Nav.Link as={NavLink} to={Constant.PROJECTS_PATHNAME} disabled={navigationDisabled}>
                        Projects
                      </Nav.Link>
                    </Nav.Item>
                    <Nav.Item as="li">
                      <Nav.Link as={NavLink} to={Constant.RENTAL_REQUESTS_PATHNAME} disabled={navigationDisabled}>
                        Requests
                      </Nav.Link>
                    </Nav.Item>
                    <Nav.Item as="li">
                      <Nav.Link as={NavLink} to={Constant.TIME_ENTRY_PATHNAME} disabled={navigationDisabled}>
                        Time Entry
                      </Nav.Link>
                    </Nav.Item>
                    <NavDropdown id="reports-dropdown" title="Reports" disabled={navigationDisabled} as="li">
                      <NavDropdown.Item as={NavLink} to={Constant.AIT_REPORT_PATHNAME} disabled={navigationDisabled}>
                        Rental Agreement Summary
                      </NavDropdown.Item>
                      <NavDropdown.Item
                        as={NavLink}
                        to={Constant.SENIORITY_LIST_PATHNAME}
                        disabled={navigationDisabled}
                      >
                        Seniority List
                      </NavDropdown.Item>
                      <NavDropdown.Item
                        as={NavLink}
                        to={Constant.STATUS_LETTERS_REPORT_PATHNAME}
                        disabled={navigationDisabled}
                      >
                        Status Letters / Mailing Labels
                      </NavDropdown.Item>
                      <NavDropdown.Item as={NavLink} to={Constant.HIRING_REPORT_PATHNAME} disabled={navigationDisabled}>
                        Hiring Report - Not Hired / Force Hire
                      </NavDropdown.Item>
                      <NavDropdown.Item
                        as={NavLink}
                        to={Constant.OWNERS_COVERAGE_PATHNAME}
                        disabled={navigationDisabled}
                      >
                        WCB / CGL Coverage
                      </NavDropdown.Item>
                    </NavDropdown>
                    <Authorize requires={Constant.PERMISSION_DISTRICT_CODE_TABLE_MANAGEMENT}>
                      <Nav.Item as="li">
                        <Nav.Link as={NavLink} to={Constant.DISTRICT_ADMIN_PATHNAME} disabled={navigationDisabled}>
                          District Admin
                        </Nav.Link>
                      </Nav.Item>
                    </Authorize>
                    <Authorize
                      condition={dispatch(any(
                        Constant.PERMISSION_ADMIN,
                        Constant.PERMISSION_USER_MANAGEMENT,
                        Constant.PERMISSION_ROLES_AND_PERMISSIONS,
                        Constant.PERMISSION_DISTRICT_ROLLOVER,
                        Constant.PERMISSION_VERSION
                      ))}
                    >
                      <NavDropdown id="admin-dropdown" title="Administration" disabled={navigationDisabled} as="li">
                        <Authorize requires={Constant.PERMISSION_ADMIN}>
                          <NavDropdown.Item
                            as={NavLink}
                            to={Constant.OVERTIME_RATES_PATHNAME}
                            disabled={navigationDisabled}
                          >
                            Manage OT Rates
                          </NavDropdown.Item>
                        </Authorize>
                        <Authorize requires={Constant.PERMISSION_USER_MANAGEMENT}>
                          <NavDropdown.Item as={NavLink} to={Constant.USERS_PATHNAME} disabled={navigationDisabled}>
                            User Management
                          </NavDropdown.Item>
                        </Authorize>
                        <Authorize requires={Constant.PERMISSION_ROLES_AND_PERMISSIONS}>
                          <NavDropdown.Item as={NavLink} to={Constant.ROLES_PATHNAME} disabled={navigationDisabled}>
                            Roles and Permissions
                          </NavDropdown.Item>
                        </Authorize>
                        <Authorize requires={Constant.PERMISSION_DISTRICT_ROLLOVER}>
                          <NavDropdown.Item as={NavLink} to={Constant.ROLLOVER_PATHNAME} disabled={navigationDisabled}>
                            Roll Over
                          </NavDropdown.Item>
                        </Authorize>
                        <Authorize requires={Constant.PERMISSION_VERSION}>
                          <NavDropdown.Item as={NavLink} to={Constant.VERSION_PATHNAME} disabled={navigationDisabled}>
                            Version Info
                          </NavDropdown.Item>
                        </Authorize>
                      </NavDropdown>
                    </Authorize>
                  </Nav>
                </Navbar.Collapse>
              </>
            )}
            {this.props.showNav && (
              <div id="navbar-right" className="float-right d-flex">
                {this.props.rolloverStatus.displayRolloverMessage && this.props.rolloverStatus.rolloverComplete && (
                  <OverlayTrigger
                    trigger="click"
                    placement="bottom"
                    rootClose
                    overlay={
                      <Popover id="rollover-notice">
                        <Popover.Title>Roll Over Complete</Popover.Title>
                        <Popover.Content>
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
                          <Button onClick={this.dismissRolloverNotice} variant="primary">
                            Dismiss
                          </Button>
                        </Popover.Content>
                      </Popover>
                    }
                  >
                    <Button id="rollover-notice-button" className="mr-5" variant="info" size="sm">
                      Roll Over Complete
                      <FontAwesomeIcon icon="exclamation-circle" />
                    </Button>
                  </OverlayTrigger>
                )}
                <Dropdown id="profile-menu">
                  <Dropdown.Toggle variant="primary" className="btn-custom">
                    <FontAwesomeIcon icon="user" />
                  </Dropdown.Toggle>
                  <Dropdown.Menu>
                    <Container>
                      <strong>{this.props.currentUser.fullName}</strong>
                      <FormGroup controlId="districtId">
                        <FormLabel>District</FormLabel>
                        <DropdownControl
                          id="districtId"
                          updateState={this.updateUserDistrict}
                          selectedId={this.props.currentUser.district.id}
                          fieldName="districtName"
                          items={userDistricts}
                        />
                      </FormGroup>
                      <Button onClick={() => logout()} variant="primary">
                        Logout
                      </Button>
                    </Container>
                  </Dropdown.Menu>
                </Dropdown>
              </div>
            )}
          </Container>
        </Navbar>
      </div>
    );
  }
}

const mapStateToProps = (state) => ({
  currentUser: state.user,
  showWorkingIndicator: state.ui.requests.waiting,
  currentUserDistricts: state.models.currentUserDistricts,
  rolloverStatus: state.lookups.rolloverStatus,
});

const mapDispatchToProps = (dispatch) => ({ dispatch });

export default connect(mapStateToProps, mapDispatchToProps, null, { pure: false })(withRouter(TopNav));
