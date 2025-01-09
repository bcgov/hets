import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
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

const TopNav = ({
  currentUser,
  showWorkingIndicator,
  showNav,
  currentUserDistricts,
  rolloverStatus,
  dispatch,
  history,
}) => {
  const [userDistricts, setUserDistricts] = useState([]);

  useEffect(() => {
    const districts = _.map(currentUserDistricts.data, (district) => ({
      ...district,
      districtName: district.district.name,
      id: district.district.id,
    }));
    setUserDistricts(districts);
  }, [currentUserDistricts]);

  const updateUserDistrict = async (state) => {
    const district = _.find(currentUserDistricts.data, (district) => {
      return district.district.id === parseInt(state.districtId);
    });
    await dispatch(Api.switchUserDistrict(district.id));
    history.push(Constant.HOME_PATHNAME);
    window.location.reload();
  };

  const dismissRolloverNotice = () => {
    dispatch(Api.dismissRolloverMessage(currentUser.district.id));
  };

  const navigationDisabled = rolloverStatus.rolloverActive;

  let environmentClass = '';
  switch (currentUser.environment) {
    case 'Development':
      environmentClass = 'env-dev';
      break;
    case 'Test':
      environmentClass = 'env-test';
      break;
    case 'Training':
      environmentClass = 'env-trn';
      break;
    case 'UAT':
      environmentClass = 'env-uat';
      break;
    default:
      break;
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
          <div id="working-indicator" hidden={!showWorkingIndicator}>
            Working <Spinner />
          </div>
        </Container>
      </Navbar>
      <Navbar id="top-nav" expand="md" className={environmentClass}>
        <Container>
          {showNav && (
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
                    <Nav.Link as={NavLink} to={Constant.OWNERS_PATHNAME} disabled={navigationDisabled}>
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
                    <NavDropdown.Item as={NavLink} to={Constant.SENIORITY_LIST_PATHNAME} disabled={navigationDisabled}>
                      Seniority List
                    </NavDropdown.Item>
                    <NavDropdown.Item as={NavLink} to={Constant.STATUS_LETTERS_REPORT_PATHNAME} disabled={navigationDisabled}>
                      Status Letters / Mailing Labels
                    </NavDropdown.Item>
                    <NavDropdown.Item as={NavLink} to={Constant.HIRING_REPORT_PATHNAME} disabled={navigationDisabled}>
                      Hiring Report - Not Hired / Force Hire
                    </NavDropdown.Item>
                    <NavDropdown.Item as={NavLink} to={Constant.OWNERS_COVERAGE_PATHNAME} disabled={navigationDisabled}>
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
                        <NavDropdown.Item as={NavLink} to={Constant.OVERTIME_RATES_PATHNAME} disabled={navigationDisabled}>
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
          {showNav && (
            <div id="navbar-right" className="float-right d-flex">
              {rolloverStatus.displayRolloverMessage && rolloverStatus.rolloverComplete && (
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
                            rolloverStatus.rolloverEndDate,
                            Constant.DATE_TIME_READABLE
                          )}
                          .
                        </p>
                        <p>
                          <strong>Note: </strong>Please save/print out the new seniority lists for all equipments
                          corresponding to each local area.
                        </p>
                        <Button onClick={dismissRolloverNotice} variant="primary">
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
                    <strong>{currentUser.fullName}</strong>
                    <FormGroup controlId="districtId">
                      <FormLabel>District</FormLabel>
                      <DropdownControl
                        id="districtId"
                        updateState={updateUserDistrict}
                        selectedId={currentUser.district.id}
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
};

TopNav.propTypes = {
  currentUser: PropTypes.object,
  showWorkingIndicator: PropTypes.bool,
  showNav: PropTypes.bool,
  currentUserDistricts: PropTypes.object,
  rolloverStatus: PropTypes.object,
  dispatch: PropTypes.func.isRequired,
  history: PropTypes.object.isRequired, //from react-router-dom
};

TopNav.defaultProps = {
  showNav: true,
};

const mapStateToProps = (state) => ({
  currentUser: state.user,
  showWorkingIndicator: state.ui.requests.waiting,
  currentUserDistricts: state.models.currentUserDistricts,
  rolloverStatus: state.lookups.rolloverStatus,
});

const mapDispatchToProps = (dispatch) => ({ dispatch });

export default connect(mapStateToProps, mapDispatchToProps, null, { pure: false })(withRouter(TopNav));
