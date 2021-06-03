import PropTypes from 'prop-types';
import React from 'react';

import { FormGroup, HelpBlock, ControlLabel, Row, Col } from 'react-bootstrap';

import * as Api from '../../api';
import * as Constant from '../../constants';

import FormDialog from '../../components/FormDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { isBlank } from '../../utils/string';

class OvertimeRateEditDialog extends React.Component {
  static propTypes = {
    onSave: PropTypes.func.isRequired,
    onClose: PropTypes.func.isRequired,
    show: PropTypes.bool,
    overtimeRateType: PropTypes.object.isRequired,
  };

  constructor(props) {
    super(props);

    this.state = {
      isSaving: false,
      rateId: props.overtimeRateType.id,
      rateType: props.overtimeRateType.rateType || '',
      description: props.overtimeRateType.description || '',
      value: props.overtimeRateType.rate || 0,
      concurrencyControlNumber: props.overtimeRateType.concurrencyControlNumber || 0,
      descriptionError: '',
      valueError: '',
    };
  }

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  didChange = () => {
    if (this.state.description !== this.props.overtimeRateType.description) { return true; }
    if (this.state.value !== this.props.overtimeRateType.value) { return true; }

    return false;
  };

  isValid = () => {
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
  };

  formSubmitted = () => {
    if (this.isValid()) {
      if (this.didChange()) {
        this.setState({ isSaving: true });

        var rateType = {
          id: this.state.rateId,
          rateType: this.state.rateType,
          description: this.state.description,
          rate: this.state.value.toFixed(2),
          periodType: Constant.RENTAL_RATE_PERIOD_HOURLY,
          concurrencyControlNumber: this.state.concurrencyControlNumber,
          active: true,
        };

        const promise = Api.updateOvertimeRateType(rateType);

        promise.then(() => {
          this.setState({ isSaving: false });
          if (this.props.onSave) { this.props.onSave(); }
          this.props.onClose();
        });
      } else {
        this.props.onClose();
      }
    }
  };

  render() {
    return (
      <FormDialog
        id="overtime-rate-edit"
        show={ this.props.show }
        title="Edit Overtime Rate"
        isSaving={ this.state.isSaving }
        onClose={ this.props.onClose }
        onSubmit={ this.formSubmitted }>
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
      </FormDialog>
    );
  }
}

export default OvertimeRateEditDialog;
