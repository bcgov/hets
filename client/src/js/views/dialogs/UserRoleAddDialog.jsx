import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { Container, Row, Col } from 'react-bootstrap';
import { FormGroup, FormText, FormLabel } from 'react-bootstrap';
import _ from 'lodash';

import * as Api from '../../api';
import * as Constant from '../../constants';

import DateControl from '../../components/DateControl.jsx';
import FormDialog from '../../components/FormDialog.jsx';
import DropdownControl from '../../components/DropdownControl.jsx';
import Spinner from '../../components/Spinner.jsx';

import { isValidDate, toZuluTime } from '../../utils/date';
import { isBlank, notBlank } from '../../utils/string';

class UserRoleAddDialog extends React.Component {
  static propTypes = {
    show: PropTypes.bool,
    user: PropTypes.object,
    roles: PropTypes.object,
    currentUser: PropTypes.object,
    onSave: PropTypes.func,
    onClose: PropTypes.func.isRequired,
  };

  constructor(props) {
    super(props);

    this.state = {
      loading: false,
      isSaving: false,

      roleId: 0,
      effectiveDate: '',
      expiryDate: '',

      roleIdError: '',
      effectiveDateError: '',
      expiryDateError: '',
    };
  }

  componentDidMount() {
    this.setState({ loading: true });
    Api.getRoles().then(() => {
      this.setState({ loading: false });
    });
  }

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  didChange = () => {
    if (this.state.roleId !== 0) {
      return true;
    }
    if (notBlank(this.state.effectiveDate)) {
      return true;
    }
    if (notBlank(this.state.expiryDate)) {
      return true;
    }

    return false;
  };

  isValid = () => {
    this.setState({
      roleIdError: false,
      effectiveDateError: false,
      expiryDateError: false,
    });

    var valid = true;
    if (!this.state.roleId) {
      this.setState({ roleIdError: 'Role is required' });
      valid = false;
    }

    if (isBlank(this.state.effectiveDate)) {
      this.setState({ effectiveDateError: 'Effective date is required' });
      valid = false;
    } else if (!isValidDate(this.state.effectiveDate)) {
      this.setState({ effectiveDateError: 'Effective date not valid' });
      valid = false;
    }

    if (notBlank(this.state.expiryDate) && !isValidDate(this.state.expiryDate)) {
      this.setState({ expiryDateError: 'Expiry date not valid' });
      valid = false;
    }

    return valid;
  };

  formSubmitted = () => {
    if (this.isValid()) {
      if (this.didChange()) {
        this.setState({ isSaving: true });

        const userRole = {
          roleId: this.state.roleId,
          effectiveDate: toZuluTime(this.state.effectiveDate),
          expiryDate: toZuluTime(this.state.expiryDate),
        };

        Api.addUserRole(this.props.user.id, userRole).then(() => {
          this.setState({ isSaving: false });
          if (this.props.onSave) {
            this.props.onSave();
          }
          this.props.onClose();
        });
      } else {
        this.props.onClose();
      }
    }
  };

  render() {
    var isAdministrator = _.some(this.props.currentUser.userRoles, { roleName: Constant.ADMINISTRATOR_ROLE });

    var filteredRoles = isAdministrator
      ? this.props.roles
      : _.reject(this.props.roles, { name: Constant.ADMINISTRATOR_ROLE });

    var roles = _.sortBy(filteredRoles, 'name');

    return (
      <FormDialog
        id="add-role"
        show={this.props.show}
        title="Add Role"
        isSaving={this.state.isSaving}
        onClose={this.props.onClose}
        onSubmit={this.formSubmitted}
      >
        {(() => {
          if (this.state.loading) {
            return (
              <div style={{ textAlign: 'center' }}>
                <Spinner />
              </div>
            );
          }

          return (
            <Container fluid>
              <Row>
                <Col md={3}>
                  <FormGroup controlId="roleId">
                    <FormLabel>
                      Role <sup>*</sup>
                    </FormLabel>
                    <DropdownControl
                      id="roleId"
                      placeholder="None"
                      blankLine
                      items={roles}
                      selectedId={this.state.roleId}
                      updateState={this.updateState}
                      isInvalid={this.state.roleIdError}
                    />
                    <FormText>{this.state.roleIdError}</FormText>
                  </FormGroup>
                </Col>
                <Col md={5}>
                  <FormGroup controlId="effectiveDate">
                    <FormLabel>
                      Effective Date <sup>*</sup>
                    </FormLabel>
                    <DateControl
                      id="effectiveDate"
                      date={this.state.effectiveDate}
                      updateState={this.updateState}
                      title="effective date"
                      isInvalid={this.state.effectiveDateError}
                    />
                    <FormText>{this.state.effectiveDateError}</FormText>
                  </FormGroup>
                </Col>
                <Col md={4}>
                  <FormGroup controlId="expiryDate">
                    <FormLabel>Expiry Date</FormLabel>
                    <DateControl
                      id="expiryDate"
                      date={this.state.expiryDate}
                      updateState={this.updateState}
                      title="expiry date"
                      isInvalid={this.state.expiryDateError}
                    />
                    <FormText>{this.state.expiryDateError}</FormText>
                  </FormGroup>
                </Col>
              </Row>
            </Container>
          );
        })()}
      </FormDialog>
    );
  }
}

function mapStateToProps(state) {
  return {
    currentUser: state.user,
    roles: state.lookups.roles,
  };
}

export default connect(mapStateToProps)(UserRoleAddDialog);
