import React from 'react';

import { connect } from 'react-redux';

import { Grid, Row, Col } from 'react-bootstrap';
import { Form, FormGroup, HelpBlock, ControlLabel } from 'react-bootstrap';

import _ from 'lodash';

import * as Constant from '../../constants';

import DropdownControl from '../../components/DropdownControl.jsx';
import EditDialog from '../../components/EditDialog.jsx';
import FilterDropdown from '../../components/FilterDropdown.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { isBlank } from '../../utils/string';

var UsersEditDialog = React.createClass({
  propTypes: {
    user: React.PropTypes.object,
    districts: React.PropTypes.object,

    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
    isNew: React.PropTypes.bool,
  },

  getInitialState() {
    var isNew = this.props.isNew;
    return {
      isNew: isNew,

      active: !isNew ? this.props.user.active === true : false,
      givenName: !isNew ? this.props.user.givenName : '',
      surname: !isNew ? this.props.user.surname : '',
      smUserId: !isNew ? this.props.user.smUserId : '',
      email: !isNew ? this.props.user.email : '',
      districtId: !isNew ? this.props.user.district.id : 0,
      agreementCity: !isNew ? this.props.user.agreementCity : '',

      status: !isNew && this.props.user.active ? Constant.USER_STATUS_ACTIVE : Constant.USER_STATUS_ARCHIVED,

      givenNameError: false,
      surnameError: false,
      smUserIdError: false,
      emailError: false,
      districtIdError: false,
    };
  },

  componentDidMount() {
    this.input.focus();
  },

  updateState(state, callback) {
    this.setState(state, callback);
  },

  updateStatus(state, callback) {
    this.setState({
      status: state.status,
      active: state.status === Constant.USER_STATUS_ACTIVE,
    }, callback);
  },

  didChange() {
    if (this.state.active !== this.props.user.active) { return true; }
    if (this.state.givenName !== this.props.user.givenName) { return true; }
    if (this.state.surname !== this.props.user.surname) { return true; }
    if (this.state.smUserId !== this.props.user.smUserId) { return true; }
    if (this.state.email !== this.props.user.email) { return true; }
    if (this.state.districtId !== this.props.user.districtId) { return true; }
    if (this.state.agreementCity !== this.props.user.agreementCity) { return true; }

    return false;
  },

  isValid() {
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
  },

  onSave() {
    this.props.onSave({ ...this.props.user, ...{
      active: this.state.active,
      givenName: this.state.givenName,
      surname: this.state.surname,
      smUserId: this.state.smUserId,
      email: this.state.email,
      district: { id: this.state.districtId },
      agreementCity: this.state.agreementCity,
    }}).catch(error => {
      this.setState({ smUserIdError: error.message });
    });
  },

  render() {
    var districts = _.sortBy(this.props.districts, 'name');

    return <EditDialog id="users-edit" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title= { <strong>User</strong> }>
      {(() => {
        return <Form>
          <Grid fluid>
            <Row>
              <Col md={12}>
                <FormGroup controlId="givenName" validationState={ this.state.givenNameError ? 'error' : null }>
                  <ControlLabel>Given Name <sup>*</sup></ControlLabel>
                  <FormInputControl type="text" defaultValue={ this.state.givenName } updateState={ this.updateState } inputRef={ ref => { this.input = ref; }}/>
                  <HelpBlock>{ this.state.givenNameError }</HelpBlock>
                </FormGroup>
              </Col>
              <Col md={12}>
                <FormGroup controlId="surname" validationState={ this.state.surnameError ? 'error' : null }>
                  <ControlLabel>Surname <sup>*</sup></ControlLabel>
                  <FormInputControl type="text" defaultValue={ this.state.surname } updateState={ this.updateState }/>
                  <HelpBlock>{ this.state.surnameError }</HelpBlock>
                </FormGroup>
              </Col>
              <Col md={12}>
                <FormGroup controlId="smUserId" validationState={ this.state.smUserIdError ? 'error' : null }>
                  <ControlLabel>User ID <sup>*</sup></ControlLabel>
                  <FormInputControl type="text" defaultValue={ this.state.smUserId } updateState={ this.updateState }/>
                  <HelpBlock>{ this.state.smUserIdError }</HelpBlock>
                </FormGroup>
              </Col>
              <Col md={12}>
                <FormGroup controlId="status">
                  <ControlLabel>Status</ControlLabel>
                  <DropdownControl id="status" title={ this.state.status } updateState={ this.updateStatus }
                    items={[ Constant.USER_STATUS_ACTIVE, Constant.USER_STATUS_ARCHIVED ]} className="full-width"
                  />
                </FormGroup>
              </Col>
              <Col md={12}>
                <FormGroup controlId="email" validationState={ this.state.emailError ? 'error' : null }>
                  <ControlLabel>E-mail <sup>*</sup></ControlLabel>
                  <FormInputControl type="text" defaultValue={ this.state.email } updateState={ this.updateState }/>
                  <HelpBlock>{ this.state.emailError }</HelpBlock>
                </FormGroup>
              </Col>
              <Col md={12}>
                <FormGroup controlId="districtId" validationState={ this.state.districtIdError ? 'error' : null }>
                  <ControlLabel>District <sup>*</sup></ControlLabel>
                  <FilterDropdown id="districtId" placeholder="None" blankLine
                    items={ districts } selectedId={ this.state.districtId } updateState={ this.updateState }  className="full-width" />
                  <HelpBlock>{ this.state.districtIdError }</HelpBlock>
                </FormGroup>
              </Col>
              <Col md={12}>
                <FormGroup controlId="agreementCity">
                  <ControlLabel>Location</ControlLabel>
                  <FormInputControl type="text" defaultValue={ this.state.agreementCity } updateState={ this.updateState }/>
                </FormGroup>
              </Col>
            </Row>
          </Grid>
        </Form>;
      })()}
    </EditDialog>;
  },
});

function mapStateToProps(state) {
  return {
    user: state.models.user,
    districts: state.lookups.districts,
  };
}

export default connect(mapStateToProps)(UsersEditDialog);
