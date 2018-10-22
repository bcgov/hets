import React from 'react';

import { connect } from 'react-redux';

import _ from 'lodash';

import { Navbar, Nav, NavItem, NavDropdown, MenuItem, OverlayTrigger, Dropdown, Popover, Button, Glyphicon, ControlLabel, FormGroup } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';

import * as Constant from '../constants';
import * as Api from '../api';

import Spinner from '../components/Spinner.jsx';
import DropdownControl from '../components/DropdownControl.jsx';

import { formatDateTimeUTCToLocal } from '../utils/date';

var TopNav = React.createClass({
  propTypes: {
    currentUser: React.PropTypes.object,
    showWorkingIndicator: React.PropTypes.bool,
    requestError: React.PropTypes.object,
    showNav: React.PropTypes.bool,
    currentUserDistricts: React.PropTypes.object,
    rolloverStatus: React.PropTypes.object,
  },

  getDefaultProps() {
    return {
      showNav: true,
    };
  },

  updateUserDistrict(state) {
    var district = _.find(this.props.currentUserDistricts.data, district => { return district.district.id === state.districtId; });
    Api.switchUserDistrict(district.id).then(() => {
      location.reload();
      this.context.router.push({ pathname: Constant.HOME_PATHNAME });
    });
  },

  logout() {
    Api.logoffUser().then(logoffUrl => {
      if (logoffUrl) {
        window.location.href = logoffUrl;
      }
    });
  },

  dismissRolloverNotice() {
    Api.dismissRolloverMessage(this.props.currentUser.district.id);
  },

  render() {
    var userDistricts = this.props.currentUserDistricts.data.map(district => { 
      return { ...district, districtName: district.district.name, id: district.district.id }; 
    });

    var navigationDisabled = this.props.rolloverStatus.rolloverActive;

    return <div id="header">
      <nav id="header-main" className="navbar navbar-default navbar-fixed-top">
        <div className="container">
          <div id="logo">
            <a href="http://www2.gov.bc.ca/gov/content/home">
              <img title="Government of B.C." alt="Government of B.C." src="images/gov/gov3_bc_logo.png"/>
            </a>
          </div>
          <h1 id="banner">MOTI Hired Equipment Tracking System</h1>
        </div>
        <Navbar id="top-nav">
          { this.props.showNav &&
            <Nav>
              <LinkContainer to={{ pathname: `/${ Constant.HOME_PATHNAME }` }} disabled={ navigationDisabled }>
                <NavItem eventKey={ 1 }>Home</NavItem>
              </LinkContainer>
              <LinkContainer to={{ pathname: `/${ Constant.OWNERS_PATHNAME }` }} disabled={ navigationDisabled }>
                <NavItem eventKey={ 2 }>Owners</NavItem>
              </LinkContainer>
              <LinkContainer to={{ pathname: `/${ Constant.EQUIPMENT_PATHNAME }` }} disabled={ navigationDisabled }>
                <NavItem eventKey={ 3 }>Equipment</NavItem>
              </LinkContainer>
              <LinkContainer to={{ pathname: `/${ Constant.PROJECTS_PATHNAME }` }} disabled={ navigationDisabled }>
                <NavItem eventKey={ 5 }>Projects</NavItem>
              </LinkContainer>
              <LinkContainer to={{ pathname: `/${ Constant.RENTAL_REQUESTS_PATHNAME }` }} disabled={ navigationDisabled }>
                <NavItem eventKey={ 6 }>Requests</NavItem>
              </LinkContainer>
              { (this.props.currentUser.hasPermission(Constant.PERMISSION_ADMIN) ||
                this.props.currentUser.hasPermission(Constant.PERMISSION_USER_MANAGEMENT) ||
                this.props.currentUser.hasPermission(Constant.PERMISSION_ROLES_AND_PERMISSIONS) ||
                this.props.currentUser.hasPermission(Constant.PERMISSION_DISTRICT_ROLLOVER) ||
                this.props.currentUser.hasPermission(Constant.PERMISSION_VERSION)) &&
                <NavDropdown id="admin-dropdown" title="Administration" disabled={ navigationDisabled }>
                  { this.props.currentUser.hasPermission(Constant.PERMISSION_USER_MANAGEMENT) &&
                    <LinkContainer to={{ pathname: `/${ Constant.USERS_PATHNAME }` }}>
                      <MenuItem eventKey={ 7 }>User Management</MenuItem>
                    </LinkContainer>
                  }
                  { this.props.currentUser.hasPermission(Constant.PERMISSION_ROLES_AND_PERMISSIONS) &&
                    <LinkContainer to={{ pathname: `/${ Constant.ROLES_PATHNAME }` }} disabled={ navigationDisabled }>
                      <MenuItem eventKey={ 8 }>Roles and Permissions</MenuItem>
                    </LinkContainer>
                  }
                  { this.props.currentUser.hasPermission(Constant.PERMISSION_DISTRICT_ROLLOVER) &&
                    <LinkContainer to={{ pathname: `/${ Constant.ROLLOVER_PATHNAME }` }} disabled={ navigationDisabled }>
                      <MenuItem eventKey={ 8 }>Roll Over</MenuItem>
                    </LinkContainer>
                  }
                  { this.props.currentUser.hasPermission(Constant.PERMISSION_VERSION) &&
                    <LinkContainer to={{ pathname: `/${ Constant.VERSION_PATHNAME }` }} disabled={ navigationDisabled }>
                      <MenuItem eventKey={ 9 }>Version Info</MenuItem>
                    </LinkContainer>
                  }
                </NavDropdown>
              }
              { this.props.currentUser.hasPermission(Constant.PERMISSION_DISTRICT_CODE_TABLE_MANAGEMENT) &&
                <LinkContainer to={{ pathname: `/${ Constant.DISTRICT_ADMIN_PATHNAME }` }} disabled={ navigationDisabled }>
                  <NavItem eventKey={ 10 }>District Admin</NavItem>
                </LinkContainer>
              }
            </Nav>
          }
          { this.props.showNav &&
            <div id="navbar-right" className="pull-right">
              { this.props.rolloverStatus.displayRolloverMessage && this.props.rolloverStatus.rolloverComplete &&
                <OverlayTrigger trigger="click" placement="bottom" rootClose overlay={
                  <Popover id="rollover-notice" title="Roll Over Complete" >
                    <p>The hired equipment roll over has been completed on { formatDateTimeUTCToLocal(this.props.rolloverStatus.rolloverEndDate, Constant.DATE_TIME_READABLE) }.</p>
                    <p><strong>Note: </strong>Please save/print out the new seniority lists for all equipments corresponding to each local area.</p>
                    <Button onClick={ this.dismissRolloverNotice } bsStyle="primary">Dismiss</Button>
                  </Popover>
                }>
                  <Button id="rollover-notice-button" className="mr-5" bsStyle="info" bsSize="xsmall">
                    Roll Over Complete
                    <Glyphicon glyph="exclamation-sign" />
                  </Button>
                </OverlayTrigger>
              }
              <Dropdown
                id="profile-menu"
              >
                <Dropdown.Toggle><Glyphicon glyph="user" /></Dropdown.Toggle>
                <Dropdown.Menu>
                  <div>{this.props.currentUser.fullName}</div>
                  <FormGroup controlId="districtId">
                    <ControlLabel>District</ControlLabel>
                    <DropdownControl id="districtId" updateState={ this.updateUserDistrict }
                      selectedId={ this.props.currentUser.district.id } fieldName="districtName" items={ userDistricts }
                    />
                  </FormGroup>
                  <Button onClick={ this.logout } bsStyle="primary">Logout</Button>
                </Dropdown.Menu>
              </Dropdown>
            </div>
          }
          <OverlayTrigger trigger="click" placement="bottom" rootClose overlay={
            <Popover id="error-message" title={ this.props.requestError.status + ' – API Error' }>
              <p><small>{ this.props.requestError.message }</small></p>
            </Popover>
          }>
            <Button id="error-indicator" className={ this.props.requestError.message ? '' : 'hide' } bsStyle="danger" bsSize="xsmall">
              Error
              <Glyphicon glyph="exclamation-sign" />
            </Button>
          </OverlayTrigger>
          <div id="working-indicator" hidden={ !this.props.showWorkingIndicator }>Working <Spinner/></div>
        </Navbar>
      </nav>
    </div>;
  },
});

TopNav.contextTypes = {
  router: React.PropTypes.object.isRequired,
};


function mapStateToProps(state) {
  return {
    currentUser: state.user,
    showWorkingIndicator: state.ui.requests.waiting,
    requestError: state.ui.requests.error,
    currentUserDistricts: state.models.currentUserDistricts,
    rolloverStatus: state.lookups.rolloverStatus,
  };
}

export default connect(mapStateToProps, null, null, { pure: false })(TopNav);
