import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { Container, Row, Col, FormGroup, FormText, FormLabel } from 'react-bootstrap';

import FormDialog from '../../components/FormDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { isBlank } from '../../utils/string';

class ConfirmForceHireDialog extends React.Component {
  static propTypes = {
    show: PropTypes.bool,
    onSave: PropTypes.func.isRequired,
    onClose: PropTypes.func.isRequired,
  };

  constructor(props) {
    super(props);

    this.state = {
      reasonForForceHire: '',
    };
  }

  componentDidMount() {
    this.input && this.input.focus();
  }

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  didChange = () => {
    return true;
  };

  isValid = () => {
    this.setState({
      noteError: '',
    });

    var valid = true;

    if (isBlank(this.state.reasonForForceHire)) {
      this.setState({ reasonForForceHireError: 'Reason is required' });
      valid = false;
    }

    return valid;
  };

  formSubmitted = () => {
    this.props.onSave(this.state.reasonForForceHire);
  };

  render() {
    return (
      <FormDialog
        id="confirm-force-hire"
        show={this.props.show}
        onSubmit={this.formSubmitted}
        onClose={this.props.onClose}
        title="Force Hire"
        backdropClassName="confirm-force-hire"
      >
        <Container fluid>
          <Col md={12}>
            <p>
              <strong>Are you sure you want to do a Force Hire?</strong>
            </p>
          </Col>
          <Row>
            <Col md={12}>
              <FormGroup controlId="reasonForForceHire">
                <FormLabel>Reason for Force Hire</FormLabel>
                <FormInputControl
                  as="textarea"
                  updateState={this.updateState}
                  isInvalid={this.state.reasonForForceHireError}
                />
                <FormText>{this.state.reasonForForceHireError}</FormText>
              </FormGroup>
            </Col>
          </Row>
        </Container>
      </FormDialog>
    );
  }
}

export default connect()(ConfirmForceHireDialog);
