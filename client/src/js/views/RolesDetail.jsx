import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { Container, Row, Col } from 'react-bootstrap';
import { FormGroup, FormText, FormLabel } from 'react-bootstrap';
import { Table, Button } from 'react-bootstrap';
import _ from 'lodash';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';

import FormInputControl from '../components/FormInputControl.jsx';
import Spinner from '../components/Spinner.jsx';
import Form from '../components/Form.jsx';
import PageHeader from '../components/ui/PageHeader.jsx';
import SubHeader from '../components/ui/SubHeader.jsx';
import ReturnButton from '../components/ReturnButton.jsx';
import PrintButton from '../components/PrintButton.jsx';

import { isBlank } from '../utils/string';

class RolesDetail extends React.Component {
  static propTypes = {
    currentUser: PropTypes.object,
    role: PropTypes.object,
    rolePermissions: PropTypes.object,
    permissions: PropTypes.object,
    history: PropTypes.object,
    match: PropTypes.object,
  };

  constructor(props) {
    super(props);

    this.state = {
      loading: false,

      name: '',
      description: '',

      nameError: '',
      descriptionError: '',

      selectedPermissionIds: [],

      isNew: props.match.params.roleId === '0',
    };
  }

  componentDidMount() {
    if (this.state.isNew) {
      // Clear the role and permissions store
      this.props.dispatch({ type: Action.UPDATE_ROLE, role: {} });
      this.props.dispatch({
        type: Action.UPDATE_ROLE_PERMISSIONS,
        rolePermissions: {},
      });
    } else {
      this.fetch();
    }
  }

  fetch = () => {
    const rolePromise = this.props.dispatch(Api.getRole(this.props.match.params.roleId));
    const permissionsPromise = this.props.dispatch(Api.getRolePermissions(this.props.match.params.roleId));

    this.setState({ loading: true });
    Promise.all([rolePromise, permissionsPromise]).then(() => {
      var selectedPermissionIds = _.map(this.props.rolePermissions, (rolePermission) => {
        return rolePermission.permission.id;
      });
      this.setState({
        loading: false,
        name: this.props.role.name,
        description: this.props.role.description,
        selectedPermissionIds: selectedPermissionIds,
      });
    });
  };

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  permissionClicked = (permission) => {
    var selectedPermissionIds = _.clone(this.state.selectedPermissionIds);
    if (selectedPermissionIds.indexOf(permission.id) === -1) {
      selectedPermissionIds.push(permission.id);
    } else {
      _.pull(selectedPermissionIds, permission.id);
    }
    this.setState({ selectedPermissionIds: selectedPermissionIds });
  };

  isValid = () => {
    this.setState({
      nameError: false,
      descriptionError: false,
    });

    var valid = true;

    if (isBlank(this.state.name)) {
      this.setState({ nameError: 'Name is required' });
      valid = false;
    }

    if (isBlank(this.state.description)) {
      this.setState({ descriptionError: 'Description is required' });
      valid = false;
    }

    return valid;
  };

  didChangeRole = () => {
    if (this.state.name !== this.props.role.name) {
      return true;
    }
    if (this.state.description !== this.props.role.description) {
      return true;
    }

    return false;
  };

  didChangePermissions = () => {
    var originalPermissionIds = _.map(this.props.rolePermissions, (rolePermission) => {
      return rolePermission.permission.id;
    });
    if (_.xor(originalPermissionIds, this.state.selectedPermissionIds).length > 0) {
      return true;
    }

    return false;
  };

  savePermissions = () => {
    if (this.didChangePermissions()) {
      this.props.dispatch(Api.updateRolePermissions(
        this.props.role.id,
        _.map(this.state.selectedPermissionIds, (id) => {
          return { id: id };
        })
      )).then(() => {
        this.returnToList();
      });
    } else {
      this.returnToList();
    }
  };

  returnToList = () => {
    this.props.history.push({
      pathname: Constant.ROLES_PATHNAME,
    });
  };

  onSave = () => {
    if (this.isValid()) {
      if (this.didChangeRole()) {
        if (this.state.isNew) {
          this.props.dispatch(Api.addRole({
            name: this.state.name,
            description: this.state.description,
          })).then(() => {
            this.savePermissions();
          });
        } else {
          this.props.dispatch(Api.updateRole({
            ...this.props.role,
            ...{
              name: this.state.name,
              description: this.state.description,
            },
          })).then(() => {
            this.savePermissions();
          });
        }
      } else {
        this.savePermissions();
      }
    }
  };

  render() {
    var role = this.props.role;

    if (
      !this.props.currentUser.hasPermission(Constant.PERMISSION_ROLES_AND_PERMISSIONS) &&
      !this.props.currentUser.hasPermission(Constant.PERMISSION_ADMIN)
    ) {
      return <div>You do not have permission to view this page.</div>;
    }

    return (
      <div id="roles-detail">
        <div id="roles-top">
          <PrintButton />
          <ReturnButton />
        </div>

        {(() => {
          if (this.state.loading) {
            return (
              <div style={{ textAlign: 'center' }}>
                <Spinner />
              </div>
            );
          }

          return (
            <div id="roles-header">
              <PageHeader title="Role" subTitle={role.name || 'New'} />
            </div>
          );
        })()}
        <div className="well">
          {(() => {
            if (this.state.loading) {
              return (
                <div style={{ textAlign: 'center' }}>
                  <Spinner />
                </div>
              );
            }

            return (
              <Form id="roles-edit" onSubmit={this.onSave}>
                <Container fluid>
                  <Row>
                    <Col md={3}>
                      <FormGroup controlId="name">
                        <FormLabel>
                          Name <sup>*</sup>
                        </FormLabel>
                        <FormInputControl
                          type="text"
                          defaultValue={this.state.name}
                          updateState={this.updateState}
                          isInvalid={this.state.nameError}
                        />
                        <FormText>{this.state.nameError}</FormText>
                      </FormGroup>
                    </Col>
                    <Col md={9}>
                      <FormGroup controlId="description">
                        <FormLabel>
                          Description <sup>*</sup>
                        </FormLabel>
                        <FormInputControl
                          type="text"
                          defaultValue={this.state.description}
                          updateState={this.updateState}
                          isInvalid={this.state.descriptionError}
                        />
                        <FormText>{this.state.descriptionError}</FormText>
                      </FormGroup>
                    </Col>
                  </Row>
                </Container>
              </Form>
            );
          })()}
        </div>
        <div className="well" id="roles-permissions">
          <SubHeader title="Permissions" />
          {(() => {
            if (this.state.loading) {
              return (
                <div style={{ textAlign: 'center' }}>
                  <Spinner />
                </div>
              );
            }

            var permissions = _.sortBy(this.props.permissions, 'name');

            return (
              <Table striped hover bordered>
                <thead>
                  <tr>
                    <th>Name</th>
                    <th>Description</th>
                  </tr>
                </thead>
                <tbody>
                  {_.map(permissions, (permission) => {
                    var selected = this.state.selectedPermissionIds.indexOf(permission.id) !== -1;
                    return (
                      <tr
                        key={permission.id}
                        className={selected ? 'selected' : ''}
                        onClick={this.permissionClicked.bind(this, permission)}
                      >
                        <td style={{ whiteSpace: 'nowrap' }}>
                          <strong>{permission.name}</strong>
                        </td>
                        <td>{permission.description}</td>
                      </tr>
                    );
                  })}
                </tbody>
              </Table>
            );
          })()}
        </div>
        <Button variant="primary" onClick={this.onSave}>
          Save
        </Button>
      </div>
    );
  }
}

const mapStateToProps = (state) => ({
  currentUser: state.user,
  role: state.models.role,
  rolePermissions: state.models.rolePermissions,
  permissions: state.lookups.permissions,
});

const mapDispatchToProps = (dispatch) => ({ dispatch });

export default connect(mapStateToProps, mapDispatchToProps)(RolesDetail);
