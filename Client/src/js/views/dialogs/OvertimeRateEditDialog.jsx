import React from 'react';

import { Form, FormGroup, HelpBlock, ControlLabel, Row, Col } from 'react-bootstrap';

import * as Constant from '../../constants';

import EditDialog from '../../components/EditDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { isBlank } from '../../utils/string';

var OvertimeRateEditDialog = React.createClass({
  propTypes: {
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
    overtimeRateType: React.PropTypes.object.isRequired,
  },

  getInitialState() {
    return {
      rateId: this.props.overtimeRateType.id,
      rateType: this.props.overtimeRateType.rateType || '',
      description: this.props.overtimeRateType.description || '',
      value: this.props.overtimeRateType.rate || 0,
      concurrencyControlNumber: this.props.overtimeRateType.concurrencyControlNumber || 0,
      descriptionError: '',
      valueError: '',
    };
  },

  updateState(state, callback) {
    this.setState(state, callback);
  },

  didChange() {
    if (this.state.description !== this.props.overtimeRateType.description) { return true; }
    if (this.state.value !== this.props.overtimeRateType.value) { return true; }

    return false;
  },

  isValid() {
    this.setState({
      descriptionError: '',
      valueError: '',
    });

    var valid = true;

    if (isBlank(this.state.description)) {
      this.setState({ descriptionError: 'Description is required' });
      valid = false;
    }

    var moneyRegex = new RegExp(Constant.MONEY_REGEX);
    if (isBlank(this.state.value)) {
      this.setState({ valueError: 'Value is required' });
      valid = false;
    } else if (this.state.value < 0) {
      this.setState({ valueError: 'Value must be positive' });
      valid = false;
    } else if (!moneyRegex.test(this.state.value)) {
      this.setState({ valueError: 'Value must have at most 2 digits after the decimal' });
      valid = false;
    }

    return valid;
  },

  onSave() {
    this.props.onSave({
      id: this.state.rateId,
      rateType: this.state.rateType,
      description: this.state.description,
      rate: this.state.value.toFixed(2),
      periodType: Constant.RENTAL_RATE_PERIOD_HOURLY,
      concurrencyControlNumber: this.state.concurrencyControlNumber,
      active: true,
    });
  },

  render() {
    return <EditDialog id="overtime-rate-edit" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title= {
        <strong>Edit Overtime Rate</strong>
      }>
      <Form>

        <Row>
          <Col xs={6}>
            <FormGroup controlId="description" validationState={ this.state.descriptionError ? 'error' : null }>
              <ControlLabel>Description <sup>*</sup></ControlLabel>
              <FormInputControl type="text" value={ this.state.description } updateState={ this.updateState }/>
              <HelpBlock>{ this.state.descriptionError }</HelpBlock>
            </FormGroup>
          </Col>
          <Col xs={6}>
            <FormGroup controlId="value" validationState={ this.state.valueError ? 'error' : null }>
              <ControlLabel>Value <sup>*</sup></ControlLabel>
              <div>
                <ControlLabel>$</ControlLabel>
                <FormInputControl type="float" value={ this.state.value } updateState={ this.updateState } style={ { display: 'inline-block', width: '100px', margin: '0 5px' } } />
                <ControlLabel>per hour</ControlLabel>
              </div>
              <HelpBlock>{ this.state.valueError }</HelpBlock>
            </FormGroup>
          </Col>
        </Row>
      </Form>
    </EditDialog>;
  },
});

export default OvertimeRateEditDialog;
