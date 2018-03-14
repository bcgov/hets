import React from 'react';

import { connect } from 'react-redux';

import { Grid, Row, Col, Button, Label } from 'react-bootstrap';
import { Form, FormGroup, HelpBlock, ControlLabel } from 'react-bootstrap';

import * as Constant from '../../constants';

import EditDialog from '../../components/EditDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { isBlank } from '../../utils/string';

var ContactsEditDialog = React.createClass({
  propTypes: {
    contact: React.PropTypes.object.isRequired,

    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
    isFirstContact: React.PropTypes.bool,
  },

  getInitialState() {
    return {
      isNew: this.props.contact.id === 0,

      givenName: this.props.contact.givenName || '',
      surname: this.props.contact.surname || '',
      role: this.props.contact.role || '',
      notes: this.props.contact.notes || '',
      emailAddress: this.props.contact.emailAddress || '',
      workPhoneNumber: this.props.contact.workPhoneNumber || '',
      mobilePhoneNumber: this.props.contact.mobilePhoneNumber || '',
      faxPhoneNumber: this.props.contact.faxPhoneNumber || '',
      isPrimary: this.props.contact.isPrimary || this.props.isFirstContact || false,

      givenNameError: false,
      surnameError: false,
      emailAddressError: false,
      workPhoneNumberError: false,
      mobilePhoneNumberError: false,
      faxPhoneNumberError: false,
    };
  },

  componentDidMount() {
    this.input.focus();
  },

  updateState(state, callback) {
    this.setState(state, callback);
  },

  makePrimary() {
    this.setState({ isPrimary: true });
  },

  didChange() {
    if (this.state.givenName !== this.props.contact.givenName) { return true; }
    if (this.state.surname !== this.props.contact.surname) { return true; }
    if (this.state.organizationName !== this.props.contact.organizationName) { return true; }
    if (this.state.role !== this.props.contact.role) { return true; }
    if (this.state.notes !== this.props.contact.notes) { return true; }
    if (this.state.emailAddress !== this.props.contact.emailAddress) { return true; }
    if (this.state.workPhoneNumber !== this.props.contact.workPhoneNumber) { return true; }
    if (this.state.mobilePhoneNumber !== this.props.contact.mobilePhoneNumber) { return true; }
    if (this.state.faxPhoneNumber !== this.props.contact.faxPhoneNumber) { return true; }
    if (this.state.isPrimary !== this.props.contact.isPrimary) { return true; }

    return false;
  },

  isValid() {
    this.setState({
      givenNameError: false,
      surnameError: false,
      emailAddressError: false,
      workPhoneNumberError: false,
      mobilePhoneNumberError: false,
      faxPhoneNumberError: false,
    });

    var valid = true;

    if (isBlank(this.state.givenName)) {
      this.setState({ givenNameError: 'Given name is required' });
      valid = false;
    }

    if (isBlank(this.state.surname)) {
      this.setState({ surnameError: 'Surname is required' });
      valid = false;
    }

    // Check the phone numbers against the North American Numbering Plan format. We basically want to
    // make sure that there's the right number of digits to make an actual phone number. Note for testers:
    // an area code and an exchange code cannot start with 0 or 1.
    if (!isBlank(this.state.workPhoneNumber) && !Constant.NANP_REGEX.test(this.state.workPhoneNumber)) {
      this.setState({ workPhoneNumberError: 'Invalid phone number' });
      valid = false;
    }

    if (!isBlank(this.state.mobilePhoneNumber) && !Constant.NANP_REGEX.test(this.state.mobilePhoneNumber)) {
      this.setState({ mobilePhoneNumberError: 'Invalid phone number' });
      valid = false;
    }

    if (!isBlank(this.state.faxPhoneNumber) && !Constant.NANP_REGEX.test(this.state.faxPhoneNumber)) {
      this.setState({ faxPhoneNumberError: 'Invalid phone number' });
      valid = false;
    }

    if (!isBlank(this.state.emailAddress) && !Constant.EMAIL_REGEX.test(this.state.emailAddress)) {
      // Just a simple RegEx test for X@Y.Z
      this.setState({ emailAddressError: 'Invalid email' });
      valid = false;
    }

    // A Primary Contact requires at least one of the phone/email fields to be filled out.
    if (this.state.isPrimary && isBlank(this.state.workPhoneNumber) && isBlank(this.state.mobilePhoneNumber) && isBlank(this.state.emailAddress)) {
      this.setState({
        workPhoneNumberError: 'A primary contact requires a phone number or email address',
        mobilePhoneNumberError: ' ',
        emailAddressError: ' ',
      });
      valid = false;
    }

    return valid;
  },

  onSave() {
    this.props.onSave({ ...this.props.contact, ...{
      givenName: this.state.givenName,
      surname: this.state.surname,
      role: this.state.role,
      notes: this.state.notes,
      emailAddress: this.state.emailAddress,
      workPhoneNumber: this.state.workPhoneNumber,
      mobilePhoneNumber: this.state.mobilePhoneNumber,
      faxPhoneNumber: this.state.faxPhoneNumber,
      isPrimary: this.state.isPrimary,
    }});
  },

  render() {
    // Read-only if the user cannot edit the contact
    var isReadOnly = !this.props.contact.canEdit && this.props.contact.id !== 0;

    return <EditDialog id="contacts-edit" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title={ <span>
        <strong>Contact</strong>
        { this.state.isPrimary ?
          <Label bsStyle="success">Primary</Label> :
          <Button title="Make Primary Contact" onClick={ this.makePrimary }>Make Primary</Button>
        }
      </span> }>
      {(() => {
        return <Form>
          <Grid fluid>
            <Row>
              <Col md={12}>
                <FormGroup controlId="givenName" validationState={ this.state.givenNameError ? 'error' : null }>
                  <ControlLabel>Given Name <sup>*</sup></ControlLabel>
                  <FormInputControl type="text" defaultValue={ this.state.givenName } readOnly={ isReadOnly } updateState={ this.updateState } inputRef={ ref => { this.input = ref; }}/>
                  <HelpBlock>{ this.state.givenNameError }</HelpBlock>
                </FormGroup>
              </Col>
              <Col md={12}>
                <FormGroup controlId="surname" validationState={ this.state.surnameError ? 'error' : null }>
                  <ControlLabel>Surname <sup>*</sup></ControlLabel>
                  <FormInputControl type="text" defaultValue={ this.state.surname } readOnly={ isReadOnly } updateState={ this.updateState }/>
                  <HelpBlock>{ this.state.surnameError }</HelpBlock>
                </FormGroup>
              </Col>
              <Col md={12}>
                <FormGroup controlId="role">
                  <ControlLabel>Role</ControlLabel>
                  <FormInputControl type="text" defaultValue={ this.state.role } readOnly={ isReadOnly } updateState={ this.updateState }/>
                </FormGroup>
              </Col>
              <Col md={12}>
                <FormGroup controlId="workPhoneNumber" validationState={ this.state.workPhoneNumberError ? 'error' : null }>
                  <ControlLabel>Work Phone</ControlLabel>
                  <FormInputControl type="text" defaultValue={ this.state.workPhoneNumber } placeholder="250-555-1212x123" readOnly={ isReadOnly } updateState={ this.updateState }/>
                  <HelpBlock>{ this.state.workPhoneNumberError }</HelpBlock>
                </FormGroup>
              </Col>
              <Col md={12}>
                <FormGroup controlId="mobilePhoneNumber" validationState={ this.state.mobilePhoneNumberError ? 'error' : null }>
                  <ControlLabel>Cell Phone</ControlLabel>
                  <FormInputControl type="text" defaultValue={ this.state.mobilePhoneNumber } placeholder="250-555-1212" readOnly={ isReadOnly } updateState={ this.updateState }/>
                  <HelpBlock>{ this.state.mobilePhoneNumberError }</HelpBlock>
                </FormGroup>
              </Col>
              <Col md={12}>
                <FormGroup controlId="faxPhoneNumber" validationState={ this.state.faxPhoneNumberError ? 'error' : null }>
                  <ControlLabel>Fax</ControlLabel>
                  <FormInputControl type="text" defaultValue={ this.state.faxPhoneNumber } placeholder="250-555-1212" readOnly={ isReadOnly } updateState={ this.updateState }/>
                  <HelpBlock>{ this.state.faxPhoneNumberError }</HelpBlock>
                </FormGroup>
              </Col>
              <Col md={12}>
                <FormGroup controlId="emailAddress" validationState={ this.state.emailAddressError ? 'error' : null }>
                  <ControlLabel>Email</ControlLabel>
                  <FormInputControl type="text" defaultValue={ this.state.emailAddress } readOnly={ isReadOnly } updateState={ this.updateState }/>
                  <HelpBlock>{ this.state.emailAddressError }</HelpBlock>
                </FormGroup>
              </Col>
            </Row>
            <Row>
              <Col md={12}>
                <FormGroup controlId="notes">
                  <ControlLabel>Notes</ControlLabel>
                  <FormInputControl componentClass="textarea" defaultValue={ this.state.notes } updateState={ this.updateState } />
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
    contactz: state.models.contact,
  };
}

export default connect(mapStateToProps)(ContactsEditDialog);
