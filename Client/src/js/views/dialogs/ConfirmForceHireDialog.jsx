import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { Grid, Row, Col, FormGroup, HelpBlock, ControlLabel } from 'react-bootstrap';

import FormDialog from '../../components/FormDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { isBlank } from '../../utils/string';


class ConfirmForceHireDialog extends React.Component {
  static propTypes = {
    show: PropTypes.bool,
    onSave: PropTypes.func.isRequired,
    onClose: PropTypes.func.isRequired,
  };

  state = {
    reasonForForceHire: '',
  };

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

    if (isBlank(this.state.reasonForForceHire) ) {
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
        backdropClassName="confirm-force-hire">
        <Grid fluid>
          <Col md={12}>
            <p><strong>Are you sure you want to do a Force Hire?</strong></p>
          </Col>
          <Row>
            <Col md={12}>
              <FormGroup controlId="reasonForForceHire" validationState={ this.state.reasonForForceHireError ? 'error' : null }>
                <ControlLabel>Reason for Force Hire</ControlLabel>
                <FormInputControl componentClass="textarea" updateState={ this.updateState } />
                <HelpBlock>{ this.state.reasonForForceHireError }</HelpBlock>
              </FormGroup>
            </Col>
          </Row>
        </Grid>
      </FormDialog>
    );
  }
}

export default connect()(ConfirmForceHireDialog);
