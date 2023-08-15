import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import {
  Row,
  Col,
  Alert,
  Badge,
  Button,
  Popover,
  FormGroup,
  FormText,
  ButtonGroup,
  OverlayTrigger,
} from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import _ from 'lodash';

import UserRoleAddDialog from './dialogs/UserRoleAddDialog.jsx';
import UsersEditDialog from './dialogs/UsersEditDialog.jsx';
import DistrictEditDialog from './dialogs/DistrictEditDialog.jsx';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';

import CheckboxControl from '../components/CheckboxControl.jsx';
import ColDisplay from '../components/ColDisplay.jsx';
import DateControl from '../components/DateControl.jsx';
import SortTable from '../components/SortTable.jsx';
import Spinner from '../components/Spinner.jsx';
import Confirm from '../components/Confirm.jsx';
import TableControl from '../components/TableControl.jsx';
import Form from '../components/Form.jsx';
import PrintButton from '../components/PrintButton.jsx';
import ReturnButton from '../components/ReturnButton.jsx';
import SubHeader from '../components/ui/SubHeader.jsx';
import Authorize from '../components/Authorize.jsx';

import { daysFromToday, formatDateTime, today, isValidDate, toZuluTime } from '../utils/date';
import { isBlank } from '../utils/string';
import { sort, caseInsensitiveSort } from '../utils/array.js';

class UsersDetail extends React.Component {
  static propTypes = {
    currentUser: PropTypes.object,
    user: PropTypes.object,
    ui: PropTypes.object,
    userDistricts: PropTypes.object,
    districts: PropTypes.object,
    match: PropTypes.object,
  };

  constructor(props) {
    super(props);

    this.state = {
      loading: true,

      district: {},

      showEditDialog: false,
      showUserRoleDialog: false,
      showDistrictEditDialog: false,

      ui: {
        // User roles
        sortField: props.ui.sortField || 'roleName',
        sortDesc: props.ui.sortDesc === true,
        showExpiredOnly: false,
      },
    };
  }

  componentDidMount() {
    // if new user
    if (this.props.match.params.userId === '0') {
      // Clear the user store
      this.props.dispatch({
        type: Action.UPDATE_USER,
        user: {
          id: 0,
          active: true,
          district: { id: 0, name: '' },
          groupIds: [],
        },
      });
      // Open editor to add new user
      this.openEditDialog();
    } else {
      this.fetch();
    }
  }

  componentWillReceiveProps(nextProps) {
    if (this.props.match.params.userId !== nextProps.match.params.userId) {
      this.fetch();
    }
  }

  fetch = () => {
    this.setState({ loading: true });
    Promise.all([
      this.props.dispatch(Api.getUser(this.props.match.params.userId)),
      this.props.dispatch(Api.getUserDistricts(this.props.match.params.userId)),
    ]).then(() => {
      this.setState({ loading: false });
    });
  };

  updateUIState = (state, callback) => {
    this.setState({ ui: { ...this.state.ui, ...state } }, () => {
      this.props.dispatch({ type: Action.UPDATE_USER_ROLES_UI, userRoles: this.state.ui });
      if (callback) {
        callback();
      }
    });
  };

  openEditDialog = () => {
    this.setState({ showEditDialog: true });
  };

  closeEditDialog = () => {
    this.setState({ showEditDialog: false });
  };

  onUserSaved = () => {
    this.closeEditDialog();
  };

  onCloseEdit = () => {
    this.closeEditDialog();
    if (this.props.match.params.userId === '0') {
      // Go back to user list if cancelling new user
      this.props.history.push(Constant.USERS_PATHNAME);
    }
  };

  openUserRoleDialog = () => {
    this.setState({ showUserRoleDialog: true });
  };

  closeUserRoleDialog = () => {
    this.setState({ showUserRoleDialog: false });
  };

  updateUserRole = (userRole) => {
    // The API call updates all of the user's user roles so we have to
    // include them all in this call, modifying the one that has just
    // been expired.
    var userRoles = this.props.user.userRoles.map((ur) => {
      return {
        ...ur,
        expiryDate: userRole.id === ur.id ? userRole.expiryDate : ur.expiryDate,
      };
    });

    this.props.dispatch(Api.updateUserRoles(this.props.user.id, userRoles));
    this.closeUserRoleDialog();
  };

  openDistrictEditDialog = () => {
    this.setState({ showDistrictEditDialog: true });
  };

  closeDistrictEditDialog = () => {
    this.setState({ showDistrictEditDialog: false });
  };

  addUserDistrict = () => {
    this.setState({ district: { id: 0 }, showDistrictEditDialog: true });
  };

  editUserDistrict = (district) => {
    this.setState({ district, showDistrictEditDialog: true });
  };

  districtSaved = (district, districts) => {
    this.updateCurrentUserDistricts(districts);
    this.closeDistrictEditDialog();
  };

  deleteDistrict = (district) => {
    this.props.dispatch(Api.deleteUserDistrict(district)).then((response) => {
      this.updateCurrentUserDistricts(response.data);
    });
  };

  updateCurrentUserDistricts = (districts) => {
    if (this.props.user.id === this.props.currentUser.id) {
      this.props.dispatch({ type: Action.CURRENT_USER_DISTRICTS, currentUserDistricts: districts });
    }
  };

  render() {
    const { loading } = this.state;
    const { user } = this.props;

    if (
      !this.props.currentUser.hasPermission(Constant.PERMISSION_USER_MANAGEMENT) &&
      !this.props.currentUser.hasPermission(Constant.PERMISSION_ADMIN)
    ) {
      return <div>You do not have permission to view this page.</div>;
    }

    return (
      <div id="users-detail">
        <div>
          <Row id="users-top">
            <Col sm={8}>
              {!loading && (
                <Badge variant={user.active ? 'success' : 'danger'}>
                  {user.active ? 'Verified Active' : 'Inactive'}
                </Badge>
              )}
            </Col>
            <Col sm={4}>
              <div className="float-right">
                <PrintButton />
                <ReturnButton />
              </div>
            </Col>
          </Row>

          <div id="users-header">
            <h1>
              User: <small>{loading ? '...' : user.fullName}</small>
            </h1>
          </div>

          <Row>
            <Col md={12}>
              <div className="well">
                <SubHeader
                  title="General"
                  editButtonTitle="Edit User"
                  editButtonDisabled={loading}
                  onEditClicked={this.openEditDialog}
                />
                {(() => {
                  if (loading) {
                    return (
                      <div style={{ textAlign: 'center' }}>
                        <Spinner />
                      </div>
                    );
                  }

                  return (
                    <Row id="user-data" className="equal-height">
                      <Col lg={4} md={6} sm={12} xs={12}>
                        <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Given Name">
                          {user.givenName}
                        </ColDisplay>
                      </Col>
                      <Col lg={4} md={6} sm={12} xs={12}>
                        <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Surname">
                          {user.surname}
                        </ColDisplay>
                      </Col>
                      <Col lg={4} md={6} sm={12} xs={12}>
                        <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="User ID">
                          {user.smUserId}
                        </ColDisplay>
                      </Col>
                      <Col lg={4} md={6} sm={12} xs={12}>
                        <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="E-mail">
                          {user.email}
                        </ColDisplay>
                      </Col>
                      <Col lg={4} md={6} sm={12} xs={12}>
                        <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="District">
                          {user.districtName}
                        </ColDisplay>
                      </Col>
                      <Col lg={4} md={6} sm={12} xs={12}>
                        <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Location">
                          {user.agreementCity}
                        </ColDisplay>
                      </Col>
                    </Row>
                  );
                })()}
              </div>
            </Col>
          </Row>
          <Row>
            <Col md={12}>
              <div className="well">
                <SubHeader title="Districts" />
                {(() => {
                  var addDistrictButton = (
                    <Authorize requires={Constant.PERMISSION_WRITE_ACCESS}>
                      <Button className="btn-custom" title="Add District" size="sm" onClick={this.addUserDistrict}>
                        <FontAwesomeIcon icon="plus" />
                        &nbsp;<strong>Add District</strong>
                      </Button>
                    </Authorize>
                  );

                  if (loading) {
                    return (
                      <div style={{ textAlign: 'center' }}>
                        <Spinner />
                      </div>
                    );
                  }

                  if (this.props.userDistricts.data.length === 0) {
                    return <Alert variant="success">No Districts {addDistrictButton}</Alert>;
                  }

                  const userDistricts = sort(
                    this.props.userDistricts.data,
                    ['isPrimary', 'district.name'],
                    ['asc', 'asc'],
                    caseInsensitiveSort
                  );

                  return (
                    <TableControl
                      headers={[
                        { field: 'district.name', title: 'District Name' },
                        {
                          field: 'addCondition',
                          title: 'Add Condition',
                          style: { textAlign: 'right' },
                          node: addDistrictButton,
                        },
                      ]}
                    >
                      {_.map(userDistricts, (district) => {
                        return (
                          <tr key={district.id}>
                            <td>
                              {district.isPrimary && <FontAwesomeIcon icon="star" />}
                              {district.district.name}
                            </td>
                            <td style={{ textAlign: 'right' }}>
                              {!district.isPrimary && (
                                <ButtonGroup>
                                  <Authorize requires={Constant.PERMISSION_WRITE_ACCESS}>
                                    <OverlayTrigger
                                      trigger="click"
                                      placement="top"
                                      rootClose
                                      overlay={<Confirm onConfirm={this.deleteDistrict.bind(this, district)} />}
                                    >
                                      <Button className="btn-custom" title="Delete District" size="sm">
                                        <FontAwesomeIcon icon="trash-alt" />
                                      </Button>
                                    </OverlayTrigger>
                                  </Authorize>
                                  <Button
                                    title="Edit District"
                                    className="btn-custom"
                                    size="sm"
                                    onClick={this.editUserDistrict.bind(this, district)}
                                  >
                                    <FontAwesomeIcon icon="edit" />
                                  </Button>
                                </ButtonGroup>
                              )}
                            </td>
                          </tr>
                        );
                      })}
                    </TableControl>
                  );
                })()}
              </div>
            </Col>
          </Row>
          <Row>
            <Col md={12}>
              <div className="well" id="users-access">
                <SubHeader title="Access">
                  <CheckboxControl
                    inline
                    id="showExpiredOnly"
                    checked={this.state.ui.showExpiredOnly}
                    updateState={this.updateUIState}
                    label="Include Expired Roles"
                  />{' '}
                </SubHeader>
                {(() => {
                  if (loading) {
                    return (
                      <div style={{ textAlign: 'center' }}>
                        <Spinner />
                      </div>
                    );
                  }

                  var addUserRoleButton = (
                    <Authorize requires={Constant.PERMISSION_WRITE_ACCESS}>
                      <Button className="btn-custom" title="Add User Role" onClick={this.openUserRoleDialog} size="sm">
                        <FontAwesomeIcon icon="plus" />
                        &nbsp;<strong>Add Role</strong>
                      </Button>
                    </Authorize>
                  );

                  var userRoles = _.filter(user.userRoles, (userRole) => {
                    let include = true;

                    if (!this.state.ui.showExpiredOnly) {
                      include = daysFromToday(userRole.expiryDate) >= 0 || userRole.expiryDate === null;
                    }

                    return include;
                  });

                  if (userRoles.length === 0) {
                    return <Alert variant="success">No roles {addUserRoleButton}</Alert>;
                  }

                  userRoles = _.sortBy(userRoles, this.state.ui.sortField);
                  if (this.state.ui.sortDesc) {
                    _.reverse(userRoles);
                  }

                  var headers = [
                    { field: 'roleName', title: 'Role' },
                    { field: 'effectiveDateSort', title: 'Effective Date' },
                    { field: 'expiryDateSort', title: 'Expiry Date' },
                    {
                      field: 'addUserRole',
                      title: 'Add User Role',
                      style: { textAlign: 'right' },
                      node: addUserRoleButton,
                    },
                  ];

                  return (
                    <SortTable
                      id="user-roles-list"
                      sortField={this.state.ui.sortField}
                      sortDesc={this.state.ui.sortDesc}
                      onSort={this.updateUIState}
                      headers={headers}
                    >
                      {_.map(userRoles, (userRole) => {
                        return (
                          <tr key={userRole.id}>
                            <td>{userRole.roleName}</td>
                            <td>{formatDateTime(userRole.effectiveDate, Constant.DATE_FULL_MONTH_DAY_YEAR)}</td>
                            <td>
                              {formatDateTime(userRole.expiryDate, Constant.DATE_FULL_MONTH_DAY_YEAR)}
                              &nbsp;{daysFromToday(userRole.expiryDate) < 0 ? <FontAwesomeIcon icon="asterisk" /> : ''}
                            </td>
                            <td style={{ textAlign: 'right' }}>
                              {userRole.expiryDate ? null : (
                                <Authorize requires={Constant.PERMISSION_WRITE_ACCESS}>
                                  <OverlayTrigger
                                    trigger="click"
                                    placement="left"
                                    overlay={<ExpireOverlay userRole={userRole} onSave={this.updateUserRole} />}
                                    rootClose
                                  >
                                    <Button className="btn-custom" title="Expire User Role" size="sm">
                                      <FontAwesomeIcon icon="pencil-alt" />
                                      &nbsp;Expire
                                    </Button>
                                  </OverlayTrigger>
                                </Authorize>
                              )}
                            </td>
                          </tr>
                        );
                      })}
                    </SortTable>
                  );
                })()}
              </div>
            </Col>
          </Row>
        </div>
        {this.state.showEditDialog && (
          <UsersEditDialog
            show={this.state.showEditDialog}
            user={user}
            onSave={this.onUserSaved}
            onClose={this.onCloseEdit}
          />
        )}
        {this.state.showUserRoleDialog && (
          <UserRoleAddDialog show={this.state.showUserRoleDialog} user={user} onClose={this.closeUserRoleDialog} />
        )}
        {this.state.showDistrictEditDialog && (
          <DistrictEditDialog
            show={this.state.showDistrictEditDialog}
            user={user}
            districts={this.props.districts}
            district={this.state.district}
            userDistricts={this.props.userDistricts.data}
            onSave={this.districtSaved}
            onClose={this.closeDistrictEditDialog}
          />
        )}
      </div>
    );
  }
}

class ExpireOverlay extends React.Component {
  static propTypes = {
    userRole: PropTypes.object.isRequired,
    onSave: PropTypes.func.isRequired,
  };

  constructor(props) {
    super(props);

    this.state = {
      expiryDate: today(),
      expiryDateError: '',
    };
  }

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  saveUserRole = () => {
    this.setState({ expiryDateError: false });

    if (isBlank(this.state.expiryDate)) {
      this.setState({ expiryDateError: 'Expiry date is required' });
    } else if (!isValidDate(this.state.expiryDate)) {
      this.setState({ expiryDateError: 'Expiry date not valid' });
    } else {
      this.props.onSave({
        ...this.props.userRole,
        ...{
          expiryDate: toZuluTime(this.state.expiryDate),
          roleId: this.props.userRole.role.id,
        },
      });
      document.body.click(); //used to close popover after saving. Overlaytrigger must be set with trigger="click" Otherwise popover stays open after expiry
    }
  };

  render() {
    var props = _.omit(this.props, 'onSave', 'hide', 'userRole');
    return (
      <Popover id="users-role-popover" {...props}>
        <Popover.Title>Set Expiry Date</Popover.Title>
        <Popover.Content>
          <Form inline onSubmit={this.saveUserRole}>
            <FormGroup controlId="expiryDate">
              <DateControl
                id="expiryDate"
                date={this.state.expiryDate}
                updateState={this.updateState}
                title="Expiry Date"
                isInvalid={this.state.expiryDateError}
              />
              <FormText>{this.state.expiryDateError}</FormText>
            </FormGroup>
            <Button variant="primary" onClick={this.saveUserRole} className="float-right">
              Save
            </Button>
          </Form>
        </Popover.Content>
      </Popover>
    );
  }
}

const mapStateToProps = (state) => ({
  currentUser: state.user,
  user: state.models.user,
  ui: state.ui.userRoles,
  userDistricts: state.models.userDistricts,
  districts: state.lookups.districts,
});

const mapDispatchToProps = (dispatch) => ({ dispatch });

export default connect(mapStateToProps, mapDispatchToProps)(UsersDetail);
