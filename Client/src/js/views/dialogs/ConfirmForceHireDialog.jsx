import React from 'react';

import { connect } from 'react-redux';

import { Grid, Row, Col, Form, FormGroup, HelpBlock, ControlLabel } from 'react-bootstrap';

import EditDialog from '../../components/EditDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { isBlank } from '../../utils/string';

var ConfirmForceHireDialog = React.createClass({
  propTypes: {
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
  },

  getInitialState() {
    return {
      reasonForForceHire: '',
    };
  },

  componentDidMount() {
    this.input && this.input.focus();
  },

  updateState(state, callback) {
    this.setState(state, callback);
  },

  didChange() {
    return true;
  },

  isValid() {
    this.setState({
      noteError: '',
    });

    var valid = true;

    if (isBlank(this.state.reasonForForceHire) ) {
      this.setState({ reasonForForceHireError: 'Reason is required' });
      valid = false;
    }

    return valid;
  },

  onSave() {
    this.props.onSave({});
  },

  render() {
    return <EditDialog id="confirm-force-hire" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title={
        <strong>Force Hire</strong>
      }
      backdropClassName="confirm-force-hire"
      >
      <Form>
        <Grid fluid>
          <Col md={12}>
            <h3><strong>Are you sure you want to do a Force Hire?</strong></h3>
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
      </Form>
    </EditDialog>;
  },
});

export default connect()(ConfirmForceHireDialog);
