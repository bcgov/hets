import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { FormGroup, HelpBlock, ControlLabel } from 'react-bootstrap';
import _ from 'lodash';

import * as Constant from '../../constants';
import * as Api from '../../api';

import DropdownControl from '../../components/DropdownControl.jsx';
import FormDialog from '../../components/FormDialog.jsx';
import FilterDropdown from '../../components/FilterDropdown.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { isBlank } from '../../utils/string';


class UsersEditDialog extends React.Component {
  static propTypes = {
    user: PropTypes.object.isRequired,
    districts: PropTypes.object,

    onSave: PropTypes.func.isRequired,
    onClose: PropTypes.func.isRequired,
    show: PropTypes.bool,
    isNew: PropTypes.bool,
  };

  static defaultProps = {
    isNew: false,
  };

  constructor(props) {
    super(props);

    var isNew = props.isNew;

    this.state = {
      isNew: isNew,

      isSaving: false,

      active: !isNew ? props.user.active === true : false,
      givenName: !isNew ? props.user.givenName : '',
      surname: !isNew ? props.user.surname : '',
      smUserId: !isNew ? props.user.smUserId : '',
      email: !isNew ? props.user.email : '',
      districtId: !isNew ? props.user.district.id : 0,
      agreementCity: !isNew ? props.user.agreementCity : '',

      status: !isNew && props.user.active ? Constant.USER_STATUS_ACTIVE : Constant.USER_STATUS_ARCHIVED,

      givenNameError: false,
      surnameError: false,
      smUserIdError: false,
      emailError: false,
      districtIdError: false,
    };
  }

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  updateStatus = (state, callback) => {
    this.setState({
      status: state.status,
      active: state.status === Constant.USER_STATUS_ACTIVE,
    }, callback);
  };

  didChange = () => {
    if (this.props.isNew) { return true; }
    if (this.state.active !== this.props.user.active) { return true; }
    if (this.state.givenName !== this.props.user.givenName) { return true; }
    if (this.state.surname !== this.props.user.surname) { return true; }
    if (this.state.smUserId !== this.props.user.smUserId) { return true; }
    if (this.state.email !== this.props.user.email) { return true; }
    if (this.state.districtId !== this.props.user.districtId) { return true; }
    if (this.state.agreementCity !== this.props.user.agreementCity) { return true; }

    return false;
  };

  isValid = () => {
    this.setState({
      givenNameError: false,
      surnameError: false,
      smUserIdError: false,
      emailError: false,
      districtIdError: false,
    });

    var valid = true;

    if (isBlank(this.state.givenName)) {
      this.setState({ givenNameError: 'Given Name is required' });
      valid = false;
    }

    if (isBlank(this.state.surname)) {
      this.setState({ surnameError: 'Surname is required' });
      valid = false;
    }

    if (isBlank(this.state.smUserId)) {
      this.setState({ smUserIdError: 'User ID is required' });
      valid = false;
    }

    if (isBlank(this.state.email)) {
      this.setState({ emailError: 'E-mail address is required' });
      valid = false;
    }

    if (this.state.districtId === 0) {
      this.setState({ districtIdError: 'District is required' });
      valid = false;
    }

    return valid;
  };

  formSubmitted = () => {
    if (this.isValid()) {
      if (this.didChange()) {
        this.setState({ isSaving: true });

        const user = {
          ...this.props.user,
          active: this.state.active,
          givenName: this.state.givenName,
          surname: this.state.surname,
          smUserId: this.state.smUserId,
          email: this.state.email,
          district: { id: this.state.districtId },
          agreementCity: this.state.agreementCity,
        };

        const isNewUser = this.state.isNew;
        const addOrUpdateUser = isNewUser ? Api.addUser : Api.updateUser;

        addOrUpdateUser(user).then((userResponse) => {
          this.setState({ isSaving: false });

          // Make sure we get the new user's ID
          if (isNewUser) {
            user.id = userResponse.id;
          }

          // Let the parent page component know that the user has been saved
          this.props.onSave(user);
        }, (err) => {
          this.setState({ isSaving: false });

          if (err.errorCode === 'HETS-38') {
            this.setState({ smUserIdError: err.errorDescription });
          } else {
            this.props.onClose();
            throw err;
          }
        });
      } else {
        this.props.onClose();
      }
    }
  };

  render() {
    var districts = _.sortBy(this.props.districts, 'name');

    return (
      <FormDialog
        id="users-edit"
        show={this.props.show}
        title="User"
        isSaving={this.state.isSaving}
        onSubmit={this.formSubmitted}
        onClose={this.props.onClose}>
        <FormGroup controlId="givenName" validationState={ this.state.givenNameError ? 'error' : null }>
          <ControlLabel>Given Name <sup>*</sup></ControlLabel>
          <FormInputControl type="text" defaultValue={ this.state.givenName } updateState={ this.updateState } autoFocus/>
          <HelpBlock>{ this.state.givenNameError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="surname" validationState={ this.state.surnameError ? 'error' : null }>
          <ControlLabel>Surname <sup>*</sup></ControlLabel>
          <FormInputControl type="text" defaultValue={ this.state.surname } updateState={ this.updateState }/>
          <HelpBlock>{ this.state.surnameError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="smUserId" validationState={ this.state.smUserIdError ? 'error' : null }>
          <ControlLabel>User ID <sup>*</sup></ControlLabel>
          <FormInputControl type="text" defaultValue={ this.state.smUserId } updateState={ this.updateState }/>
          <HelpBlock>{ this.state.smUserIdError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="status">
          <ControlLabel>Status</ControlLabel>
          <DropdownControl id="status" title={ this.state.status } updateState={ this.updateStatus }
            items={[ Constant.USER_STATUS_ACTIVE, Constant.USER_STATUS_ARCHIVED ]} className="full-width"
          />
        </FormGroup>
        <FormGroup controlId="email" validationState={ this.state.emailError ? 'error' : null }>
          <ControlLabel>E-mail <sup>*</sup></ControlLabel>
          <FormInputControl type="text" defaultValue={ this.state.email } updateState={ this.updateState }/>
          <HelpBlock>{ this.state.emailError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="districtId" validationState={ this.state.districtIdError ? 'error' : null }>
          <ControlLabel>District <sup>*</sup></ControlLabel>
          <FilterDropdown id="districtId" placeholder="None" blankLine
            items={ districts } selectedId={ this.state.districtId } updateState={ this.updateState }  className="full-width" />
          <HelpBlock>{ this.state.districtIdError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="agreementCity">
          <ControlLabel>Location</ControlLabel>
          <FormInputControl type="text" defaultValue={ this.state.agreementCity } updateState={ this.updateState }/>
        </FormGroup>
      </FormDialog>
    );
  }
}

function mapStateToProps(state) {
  return {
    districts: state.lookups.districts,
  };
}

export default connect(mapStateToProps)(UsersEditDialog);
