import PropTypes from 'prop-types';
import React from 'react';
import { Row, Col } from 'react-bootstrap';
import { FormGroup, HelpBlock, ControlLabel, Button, Glyphicon } from 'react-bootstrap';
import _ from 'lodash';

import * as Constant from '../../constants';
import * as Api from '../../api';

import DropdownControl from '../../components/DropdownControl.jsx';
import FormDialog from '../../components/FormDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { isBlank } from '../../utils/string';

const ratePeriods = [
  Constant.RENTAL_RATE_PERIOD_HOURLY,
  Constant.RENTAL_RATE_PERIOD_DAILY,
  Constant.RENTAL_RATE_PERIOD_WEEKLY,
  Constant.RENTAL_RATE_PERIOD_MONTHLY,
  Constant.RENTAL_RATE_PERIOD_NEGOTIATED,
];

class RentalRatesEditDialog extends React.Component {
  static propTypes = {
    show: PropTypes.bool,
    rentalRate: PropTypes.object.isRequired,
    rentalAgreement: PropTypes.object,
    provincialRateTypes: PropTypes.array,
    onSave: PropTypes.func,
    onClose: PropTypes.func.isRequired,
  };

  constructor(props) {
    super(props);

    const ratePeriod = props.rentalRate.id === 0 ?
      props.rentalAgreement.ratePeriod || Constant.RENTAL_RATE_PERIOD_HOURLY :
      props.rentalRate.ratePeriod;

    this.state = {
      isNew: props.rentalRate.id === 0,

      forms: [{
        isIncludedInTotal: props.rentalRate.isIncludedInTotal || false,
        rateType: {},
        rate: props.rentalRate.rate || 0.0,
        ratePeriod: ratePeriod,
        uiRatePeriod: props.rentalRate.set ? Constant.RENTAL_RATE_PERIOD_SET : ratePeriod,
        set: props.rentalRate.set || false,
        comment: props.rentalRate.comment || '',

        componentNameError: '',
        rateError: '',
        commentError: '',
      }],
      concurrencyControlNumber: props.rentalRate.concurrencyControlNumber || 0,
    };
  }

  updateState = (value) => {
    let property = Object.keys(value)[0];
    let stateValue = _.values(value)[0];
    let number = property.match(/\d+/g)[0];
    let stateName = property.match(/[a-zA-Z]+/g)[0];
    let state = { [stateName]:  stateValue };
    const updatedForms = this.state.forms.slice();
    updatedForms.splice(number, 1, { ...updatedForms[number], ...state});
    this.setState({ forms: updatedForms });
  };

  updateUiRatePeriod = (value, index) => {
    const uiRatePeriod = _.values(value)[0];
    const updatedForms = this.state.forms.slice();
    const state = {
      uiRatePeriod: uiRatePeriod,
      set: uiRatePeriod === Constant.RENTAL_RATE_PERIOD_SET,
      ratePeriod: uiRatePeriod === Constant.RENTAL_RATE_PERIOD_SET ? this.props.rentalAgreement.ratePeriod : uiRatePeriod,
    };
    updatedForms.splice(index, 1, { ...updatedForms[index], ...state});
    this.setState({ forms: updatedForms });
  };

  didChange = () => {
    return true;
  };

  isValid = () => {
    const forms = this.state.forms.slice();

    forms.forEach((form, i) => {
      let state = {
        ...form,
        rateError: '',
        commentError: '',
      };
      forms[i] = state;
    });

    let valid = true;

    forms.forEach((form, i) => {
      if (isBlank(form.comment)) {
        forms[i] = { ...forms[i], commentError: 'Comment is required.' };
        valid = false;
      }

      if (isBlank(form.rate) ) {
        forms[i] = { ...forms[i], rateError: 'Pay rate is required' };
        valid = false;
      } else if (form.rate < 0) {
        forms[i] = { ...forms[i], rateError: 'Pay rate not valid' };
        valid = false;
      }
    });

    this.setState({ forms });

    return valid;
  };

  formSubmitted = () => {
    const { rentalAgreement, onSave, onClose } = this.props;

    if (this.isValid()) {
      if (this.didChange()) {
        const forms = this.state.forms;
        const rates = forms.map((form) => {
          return {
            id: this.props.rentalRate.id || 0,
            rentalAgreement: { id: this.props.rentalRate.rentalAgreement.id },
            rate: form.rate,
            ratePeriod: form.ratePeriod,
            comment: form.comment,
            set: form.set,
            isIncludedInTotal: this.props.rentalRate.isIncludedInTotal,
            concurrencyControlNumber: this.state.concurrencyControlNumber,
          };
        });

        (this.state.isNew ? Api.addRentalRates(rentalAgreement.id, rates) : Api.updateRentalRate(_.first(rates))).then(() => {
          if (onSave) { onSave(); }
        });
      }

      onClose();
    }
  };

  addInput = () => {
    if (this.state.forms.length < 10) {
      const forms = this.state.forms.slice();

      const ratePeriod = this.props.rentalAgreement.ratePeriod || Constant.RENTAL_RATE_PERIOD_HOURLY;

      forms.push({
        isIncludedInTotal: this.props.rentalRate.isIncludedInTotal || false,
        rate: this.props.rentalRate.rate || 0.0,
        ratePeriod: ratePeriod,
        uiRatePeriod: this.props.rentalRate.set ? Constant.RENTAL_RATE_PERIOD_SET : ratePeriod,
        set: this.props.rentalRate.set || false,
        comment: this.props.rentalRate.comment || '',

        rateError: '',
        commentError: '',
      });

      this.setState({ forms });
    }
  };

  removeInput = () => {
    if (this.state.forms.length > 1) {
      const forms = this.state.forms.slice();
      forms.pop();
      this.setState({ forms });
    }
  };

  renderForm(form, i) {
    // Read-only if the user cannot edit the rental agreement
    const isReadOnly = !this.props.rentalRate.canEdit && this.props.rentalRate.id !== 0;

    const uiRatePeriods = _.concat(ratePeriods, Constant.RENTAL_RATE_PERIOD_SET);

    const ratePeriodElement = this.props.rentalRate.isIncludedInTotal ?
      <>
        <ControlLabel>Period</ControlLabel>
        <div style={ { marginTop: '10px', marginBottom: '10px' } }>{ form.uiRatePeriod }</div>
      </>
      :
      <FormGroup controlId={`uiRatePeriod${i}`}>
        <ControlLabel>Period <sup>*</sup></ControlLabel>
        <DropdownControl id={`uiRatePeriod${i}`} disabled={ isReadOnly } title={ form.uiRatePeriod } updateState={ (state) => this.updateUiRatePeriod(state, i) }
          items={ uiRatePeriods } />
      </FormGroup>;

    return (
      <div className="form-item" key={i}>
        <Row>
          <Col md={2}>
            <FormGroup controlId={`rate${i}`} validationState={ form.rateError ? 'error' : null }>
              <ControlLabel>Rate <sup>*</sup></ControlLabel>
              <FormInputControl type="float" min={ 0 } defaultValue={ (form.rate || 0).toFixed(2) } readOnly={ isReadOnly } updateState={ this.updateState } autoFocus />
              <HelpBlock>{ form.rateError }</HelpBlock>
            </FormGroup>
          </Col>
          <Col md={3}>
            { ratePeriodElement }
          </Col>
          <Col md={7}>
            <FormGroup controlId={`comment${i}`} validationState={ form.commentError ? 'error' : null }>
              <ControlLabel>Comment <sup>*</sup></ControlLabel>
              <FormInputControl defaultValue={ form.comment } readOnly={ isReadOnly } updateState={ this.updateState } />
              <HelpBlock>{ form.commentError }</HelpBlock>
            </FormGroup>
          </Col>
        </Row>
      </div>
    );
  }

  render() {
    var status = this.props.rentalRate.isIncludedInTotal ? 'Included' : 'As-Needed';

    return (
      <FormDialog
        id="rental-rates-edit"
        show={this.props.show}
        title={`Rental Agreement – ${status} Rates and Attachments`}
        onSubmit={this.formSubmitted}
        onClose={this.props.onClose}>
        <div className="forms-container">
          { this.state.forms.map((form, i) => this.renderForm(form, i))}
        </div>
        <div className="align-right">
          { this.state.isNew && this.state.forms.length > 1 && (
            <Button
              bsSize="xsmall"
              className="remove-btn"
              onClick={ this.removeInput }>
              <Glyphicon glyph="minus" />&nbsp;<strong>Remove</strong>
            </Button>
          )}
          { this.state.isNew && this.state.forms.length < 10 && (
            <Button
              bsSize="xsmall"
              onClick={ this.addInput }>
              <Glyphicon glyph="plus" />&nbsp;<strong>Add</strong>
            </Button>
          )}
        </div>
      </FormDialog>
    );
  }
}

export default RentalRatesEditDialog;
