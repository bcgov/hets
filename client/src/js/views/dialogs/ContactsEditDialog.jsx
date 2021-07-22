import PropTypes from 'prop-types';
import React from 'react';

import { connect } from 'react-redux';

import { Container, Row, Col, Button, Badge } from 'react-bootstrap';
import { FormGroup, FormText, FormLabel } from 'react-bootstrap';

import * as Constant from '../../constants';

import FormDialog from '../../components/FormDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { isBlank, formatPhoneNumber } from '../../utils/string';

class ContactsEditDialog extends React.Component {
  static propTypes = {
    contact: PropTypes.object.isRequired,
    parent: PropTypes.object.isRequired,
    saveContact: PropTypes.func.isRequired,
    onSave: PropTypes.func.isRequired,
    onClose: PropTypes.func.isRequired,
    show: PropTypes.bool,
    defaultPrimary: PropTypes.bool,
  };

  constructor(props) {
    super(props);

    this.state = {
      isNew: props.contact.id === 0,

      isSaving: false,

      givenName: props.contact.givenName || '',
      surname: props.contact.surname || '',
      role: props.contact.role || '',
      notes: props.contact.notes || '',
      emailAddress: props.contact.emailAddress || '',
      workPhoneNumber: props.contact.workPhoneNumber || '',
      mobilePhoneNumber: props.contact.mobilePhoneNumber || '',
      faxPhoneNumber: props.contact.faxPhoneNumber || '',
      isPrimary: props.contact.isPrimary || props.defaultPrimary || false,

      givenNameError: false,
      emailAddressError: false,
      workPhoneNumberError: false,
      mobilePhoneNumberError: false,
      faxPhoneNumberError: false,
    };
  }

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  makePrimary = () => {
    this.setState({ isPrimary: true });
  };

  didChange = () => {
    if (this.state.givenName !== this.props.contact.givenName) {
      return true;
    }
    if (this.state.surname !== this.props.contact.surname) {
      return true;
    }
    if (this.state.organizationName !== this.props.contact.organizationName) {
      return true;
    }
    if (this.state.role !== this.props.contact.role) {
      return true;
    }
    if (this.state.notes !== this.props.contact.notes) {
      return true;
    }
    if (this.state.emailAddress !== this.props.contact.emailAddress) {
      return true;
    }
    if (this.state.workPhoneNumber !== this.props.contact.workPhoneNumber) {
      return true;
    }
    if (this.state.mobilePhoneNumber !== this.props.contact.mobilePhoneNumber) {
      return true;
    }
    if (this.state.faxPhoneNumber !== this.props.contact.faxPhoneNumber) {
      return true;
    }
    if (this.state.isPrimary !== this.props.contact.isPrimary) {
      return true;
    }

    return false;
  };

  isValidPhoneNumber = (number) => {
    if (isBlank(number)) {
      return true;
    }
    return Constant.NANP_REGEX.test(number) && formatPhoneNumber(number).length <= Constant.MAX_LENGTH_PHONE_NUMBER;
  };

  isValid = () => {
    this.setState({
      givenNameError: false,
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

    // Check the phone numbers against the North American Numbering Plan format. We basically want to
    // make sure that there's the right number of digits to make an actual phone number. Note for testers:
    // an area code and an exchange code cannot start with 0 or 1.
    if (!this.isValidPhoneNumber(this.state.workPhoneNumber)) {
      this.setState({ workPhoneNumberError: 'Invalid phone number' });
      valid = false;
    }

    if (!this.isValidPhoneNumber(this.state.mobilePhoneNumber)) {
      this.setState({ mobilePhoneNumberError: 'Invalid phone number' });
      valid = false;
    }

    if (!this.isValidPhoneNumber(this.state.faxPhoneNumber)) {
      this.setState({ faxPhoneNumberError: 'Invalid phone number' });
      valid = false;
    }

    if (!isBlank(this.state.emailAddress) && !Constant.EMAIL_REGEX.test(this.state.emailAddress)) {
      // Just a simple RegEx test for X@Y.Z
      this.setState({ emailAddressError: 'Invalid email' });
      valid = false;
    }

    // A Primary Contact requires at least one of the phone/email fields to be filled out.
    if (
      this.state.isPrimary &&
      isBlank(this.state.workPhoneNumber) &&
      isBlank(this.state.mobilePhoneNumber) &&
      isBlank(this.state.emailAddress)
    ) {
      this.setState({
        workPhoneNumberError: 'A primary contact requires a phone number or email address',
        mobilePhoneNumberError: ' ',
        emailAddressError: ' ',
      });
      valid = false;
    }

    return valid;
  };

  formSubmitted = () => {
    const { isNew } = this.state;

    if (this.isValid()) {
      if (this.didChange()) {
        this.setState({ isSaving: true });

        const contact = {
          ...this.props.contact,
          givenName: this.state.givenName,
          surname: this.state.surname,
          role: this.state.role,
          notes: this.state.notes,
          emailAddress: this.state.emailAddress,
          workPhoneNumber: formatPhoneNumber(this.state.workPhoneNumber),
          mobilePhoneNumber: formatPhoneNumber(this.state.mobilePhoneNumber),
          faxPhoneNumber: formatPhoneNumber(this.state.faxPhoneNumber),
          isPrimary: this.state.isPrimary,
        };

        this.props.saveContact(this.props.parent, contact).then((savedContact) => {
          this.setState({ isSaving: false });
          this.props.onSave(savedContact);
        });

        if (!isNew) {
          // can be closed right away if it isn't new
          this.props.onClose();
        }
      }
    }
  };

  render() {
    // Read-only if the user cannot edit the contact
    var isReadOnly = !this.props.contact.canEdit && this.props.contact.id !== 0;

    const dialogTitle = (
      <span>
        Contact
        {this.state.isPrimary ? (
          <Badge variant="success">Primary</Badge>
        ) : (
          <Button title="Make Primary Contact" onClick={this.makePrimary}>
            Make Primary
          </Button>
        )}
      </span>
    );

    return (
      <FormDialog
        id="contacts-edit"
        show={this.props.show}
        isSaving={this.state.isSaving}
        onClose={this.props.onClose}
        onSubmit={this.formSubmitted}
        title={dialogTitle}
      >
        <Container fluid>
          <Row>
            <Col md={12}>
              <FormGroup controlId="givenName">
                <FormLabel>
                  Given Name <sup>*</sup>
                </FormLabel>
                <FormInputControl
                  type="text"
                  defaultValue={this.state.givenName}
                  readOnly={isReadOnly}
                  updateState={this.updateState}
                  autoFocus
                  isInvalid={this.state.givenNameError}
                />
                <FormText>{this.state.givenNameError}</FormText>
              </FormGroup>
            </Col>
          </Row>
          <Row>
            <Col md={12}>
              <FormGroup controlId="surname">
                <FormLabel>Surname</FormLabel>
                <FormInputControl
                  type="text"
                  defaultValue={this.state.surname}
                  readOnly={isReadOnly}
                  updateState={this.updateState}
                />
              </FormGroup>
            </Col>
          </Row>
          <Row>
            <Col md={12}>
              <FormGroup controlId="role">
                <FormLabel>Role</FormLabel>
                <FormInputControl
                  type="text"
                  defaultValue={this.state.role}
                  readOnly={isReadOnly}
                  updateState={this.updateState}
                />
              </FormGroup>
            </Col>
          </Row>
          <Row>
            <Col md={12}>
              <FormGroup controlId="workPhoneNumber">
                <FormLabel>Phone {this.state.isPrimary && <sup>*</sup>}</FormLabel>
                <FormInputControl
                  type="text"
                  defaultValue={this.state.workPhoneNumber}
                  placeholder="250-555-1212x123"
                  readOnly={isReadOnly}
                  updateState={this.updateState}
                  isInvalid={this.state.workPhoneNumberError}
                />
                <FormText>{this.state.workPhoneNumberError}</FormText>
              </FormGroup>
            </Col>
          </Row>
          <Row>
            <Col md={12}>
              <FormGroup controlId="mobilePhoneNumber">
                <FormLabel>Cell Phone {this.state.isPrimary && <sup>*</sup>}</FormLabel>
                <FormInputControl
                  type="text"
                  defaultValue={this.state.mobilePhoneNumber}
                  placeholder="250-555-1212"
                  readOnly={isReadOnly}
                  updateState={this.updateState}
                  isInvalid={this.state.mobilePhoneNumberError}
                />
                <FormText>{this.state.mobilePhoneNumberError}</FormText>
              </FormGroup>
            </Col>
          </Row>
          <Row>
            <Col md={12}>
              <FormGroup controlId="faxPhoneNumber">
                <FormLabel>Fax</FormLabel>
                <FormInputControl
                  type="text"
                  defaultValue={this.state.faxPhoneNumber}
                  placeholder="250-555-1212"
                  readOnly={isReadOnly}
                  updateState={this.updateState}
                  isInvalid={this.state.faxPhoneNumberError}
                />
                <FormText>{this.state.faxPhoneNumberError}</FormText>
              </FormGroup>
            </Col>
          </Row>
          <Row>
            <Col md={12}>
              <FormGroup controlId="emailAddress">
                <FormLabel>Email {this.state.isPrimary && <sup>*</sup>}</FormLabel>
                <FormInputControl
                  type="text"
                  defaultValue={this.state.emailAddress}
                  readOnly={isReadOnly}
                  updateState={this.updateState}
                  isInvalid={this.state.emailAddressError}
                />
                <FormText>{this.state.emailAddressError}</FormText>
              </FormGroup>
            </Col>
          </Row>
          <Row>
            <Col md={12}>
              <FormGroup controlId="notes">
                <FormLabel>Notes</FormLabel>
                <FormInputControl as="textarea" defaultValue={this.state.notes} updateState={this.updateState} />
              </FormGroup>
            </Col>
          </Row>
        </Container>
      </FormDialog>
    );
  }
}

function mapStateToProps(state) {
  return {
    contactz: state.models.contact,
  };
}

export default connect(mapStateToProps)(ContactsEditDialog);
