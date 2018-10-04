import React from 'react';

import { Grid, Row, Col } from 'react-bootstrap';
import { Form, FormGroup, HelpBlock, ControlLabel, Button, Glyphicon, Alert } from 'react-bootstrap';

import _ from 'lodash';

import * as Constant from '../../constants';

import DropdownControl from '../../components/DropdownControl.jsx';
import EditDialog from '../../components/EditDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';
import CheckboxControl from '../../components/CheckboxControl.jsx';

import { isBlank } from '../../utils/string';

const PERCENT_RATE = '%';
const DOLLAR_RATE = '$';

var RentalRatesEditDialog = React.createClass({
  propTypes: {
    rentalRate: React.PropTypes.object.isRequired,
    onSave: React.PropTypes.func.isRequired,
    onSaveMultiple: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
    provincialRateTypes: React.PropTypes.array,
    rentalAgreement: React.PropTypes.object,
  },

  getInitialState() {
    var isNew = this.props.rentalRate.id === 0;

    return {
      isNew: isNew,
      numberOfInputs: 1,

      forms: { 
        1: { 
          isAttachment: this.props.rentalRate.isAttachment || false,
          componentName: this.props.rentalRate.componentName || '',
          rateType: {},
          rate: this.props.rentalRate.rate || 0.0,
          percentOfEquipmentRate: this.props.rentalRate.percentOfEquipmentRate || 0,
          ratePeriod: this.props.rentalRate.ratePeriod || Constant.RENTAL_RATE_PERIOD_HOURLY,
          comment: this.props.rentalRate.comment || '',
          isIncludedInTotal: this.props.rentalRate.includeInTotal || false,
          differentRatePeriods: false,

          // UI fields
          percentOrRateOption: isNew || this.props.rentalRate.percentOfEquipmentRate > 0 ? PERCENT_RATE : DOLLAR_RATE,
          percentOrRateValue: this.props.rentalRate.rate || this.props.rentalRate.percentOfEquipmentRate || 0,

          componentNameError: '',
          rateError: '',
          ratePeriodError: '',
          commentError: '',
        },
      },
      concurrencyControlNumber: this.props.rentalRate.concurrencyControlNumber || 0,
    };
  },

  componentDidMount() {
    if (this.props.rentalRate.componentName) {
      var rateType = _.find(this.props.provincialRateTypes, {description: this.props.rentalRate.componentName } );
      let updatedState = { 
        ...this.state.forms, 
        1: { 
          ...this.state.forms[1], 
          rateType: rateType, 
        }, 
      };
      this.setState({ forms: updatedState });
    }
  },

  updateState(value) {
    let property = Object.keys(value)[0];
    let stateValue = _.values(value)[0];
    let number = property.match(/\d+/g)[0];
    let stateName = property.match(/[a-zA-Z]+/g)[0];
    let state = { [stateName]:  stateValue };
    let updatedState = { ...this.state.forms, [number]: { ...this.state.forms[number], ...state } };
    this.setState({ forms: updatedState });
  },

  updateRatePeriodState(value) {
    let property = Object.keys(value)[0];
    let stateValue = _.values(value)[0];
    let number = property.match(/\d+/g)[0];
    let stateName = property.match(/[a-zA-Z]+/g)[0];
    let state = { [stateName]:  stateValue };
    let updatedState = { 
      ...this.state.forms, 
      [number]: { 
        ...this.state.forms[number], 
        ...state, 
      },
    };
    if (this.props.rentalAgreement.ratePeriod === stateValue) {
      updatedState[number].differentRatePeriods = false;
    } else {
      updatedState[number].differentRatePeriods = true;
    }
    this.setState({ forms: updatedState });
  },

  updateRateTypeState(value) {
    let stateValue = _.values(value)[0];
    let provincialRateTypes = this.props.provincialRateTypes;
    let rateType = _.find(provincialRateTypes, {id: stateValue } );
    let property = Object.keys(value)[0];
    let stateName = property.match(/[a-zA-Z]+/g)[0];
    let number = property.match(/\d+/g)[0];
    let state = { 
      rateType: rateType, 
      rate: rateType.rate,
      percentOrRateValue: rateType.rate ? rateType.rate : 0, 
      percentOfEquipmentRate: rateType.isPercentRate ? rateType.rate : 0,
      percentOrRateOption: rateType.isPercentRate ? PERCENT_RATE : DOLLAR_RATE,
      [stateName]: stateValue,
    };
    let updatedState = { ...this.state.forms, [number]: { ...this.state.forms[number], ...state } };
    this.setState({ forms: updatedState });
  },

  updateUIState(value) {
    let property = Object.keys(value)[0];
    let stateValue = _.values(value)[0];
    let number = property.match(/\d+/g)[0];
    let stateName = property.match(/[a-zA-Z]+/g)[0];
    let state = { [stateName]:  stateValue };
    if (state.percentOrRateValue) {
      let percentOfEquipmentRate = this.state.forms[number].percentOrRateOption == PERCENT_RATE ? state.percentOrRateValue : 0;
      let rate = this.state.forms[number].percentOrRateOption == DOLLAR_RATE ? state.percentOrRateValue : 0;
      let updatedState = { 
        ...this.state.forms, 
        [number]: { 
          ...this.state.forms[number], 
          ...state,
          percentOfEquipmentRate: percentOfEquipmentRate, 
          rate: rate,  
        }, 
      };
      return this.setState({ forms: updatedState });
    }
    let percentOfEquipmentRate = state.percentOrRateOption == PERCENT_RATE ? this.state.forms[number].percentOrRateValue : 0;
    let rate = state.percentOrRateOption == DOLLAR_RATE ? this.state.forms[number].percentOrRateValue : 0;
    let updatedState = { 
      ...this.state.forms, 
      [number]: { 
        ...this.state.forms[number], 
        ...state,
        percentOfEquipmentRate: percentOfEquipmentRate, 
        rate: rate,  
      }, 
    };
    return this.setState({ forms: updatedState });
  },

  didChange() {
    return true;
  },

  isValid() {
    let forms = { ...this.state.forms };

    let formsResetObj = forms;
    Object.keys(forms).forEach((key) => {
      let state = { 
        ...forms[key], 
        componentNameError: '',
        rateError: '',
        ratePeriodError: '',
        commentError: '',
      };
      formsResetObj[key] = state;
    });
    
    this.setState({ forms: formsResetObj });
    let valid = true;

    let formsErrorsObj = forms;
    Object.keys(forms).forEach((key) => {

      if (forms[key].rateType.description === Constant.NON_STANDARD_CONDITION && isBlank(forms[key].comment)) {
        let state = { ...forms[key], commentError: 'Comment is required.' };
        formsErrorsObj[key] = state;
        valid = false;
      }
      
      if (isBlank(forms[key].componentName)) {
        let state = { ...forms[key], componentNameError: 'Rate type is required.' };
        formsErrorsObj[key] = state;
        valid = false;
      }

      if (isBlank(this.state.forms[key].ratePeriod)) {
        let state = { ...forms[key], ratePeriodError: 'Period is required' };
        formsErrorsObj[key] = state;
        valid = false;
      }

      if (isBlank(this.state.forms[key].percentOrRateValue) ) {
        let state = { ...forms[key], rateError: 'Pay rate is required' };
        formsErrorsObj[key] = state;
        valid = false;
      } else if (this.state.forms[key].percentOrRateValue < 1) {
        let state = { ...forms[key], rateError: 'Pay rate not valid' };
        formsErrorsObj[key] = state;
        valid = false;
      }
    });

    this.setState({ forms: formsErrorsObj });

    return valid;
  },

  onSave() {
    let forms = this.state.forms;
    let attachments = Object.keys(forms).map((key) => {
      return { 
        id: this.props.rentalRate.id || 0,
        rentalAgreement: { id: this.props.rentalRate.rentalAgreement.id }, 
        componentName: this.state.forms[key].rateType.description,
        rate: this.state.forms[key].rate,
        percentOfEquipmentRate: this.state.forms[key].percentOfEquipmentRate,
        ratePeriod: this.state.forms[key].ratePeriod,
        comment: this.state.forms[key].comment,
        isIncludedInTotal: this.state.forms[key].isIncludedInTotal,
        concurrencyControlNumber: this.state.concurrencyControlNumber,
      };
    });
    this.state.isNew ? this.props.onSaveMultiple(attachments) : this.props.onSave(attachments[0]);
  },

  addInput() {
    if (this.state.numberOfInputs < 10) {
      let numberOfInputs = Object.keys(this.state.forms).length;
      this.setState({ 
        numberOfInputs: this.state.numberOfInputs + 1,
        forms: { 
          ...this.state.forms, 
          [numberOfInputs + 1]: { 
            isAttachment: this.props.rentalRate.isAttachment || false,
            componentName: this.props.rentalRate.componentName || '',
            rateType: {},
            rate: this.props.rentalRate.rate || 0.0,
            percentOfEquipmentRate: this.props.rentalRate.percentOfEquipmentRate || 0,
            ratePeriod: this.props.rentalRate.ratePeriod || Constant.RENTAL_RATE_PERIOD_HOURLY,
            comment: this.props.rentalRate.comment || '',
            includeInTotal: this.props.rentalRate.includeInTotal || false,

            // UI fields
            percentOrRateOption: this.state.isNew || this.props.rentalRate.percentOfEquipmentRate > 0 ? PERCENT_RATE : DOLLAR_RATE,
            percentOrRateValue: this.props.rentalRate.rate || this.props.rentalRate.percentOfEquipmentRate || 0,

            componentNameError: '',
            rateError: '',
            ratePeriodError: '',
            commentError: '',
          }, 
        },
      });
    }
  },

  removeInput() {
    if (this.state.numberOfInputs > 1) {
      let numberOfInputs = Object.keys(this.state.forms).length;
      let forms = { ...this.state.forms };
      delete forms[numberOfInputs];
      this.setState({ 
        numberOfInputs: this.state.numberOfInputs - 1,
        forms: forms, 
      });
    }
  },

  render() {
    // Read-only if the user cannot edit the rental agreement
    var isReadOnly = !this.props.rentalRate.canEdit && this.props.rentalRate.id !== 0;
    var provincialRateTypes = this.props.provincialRateTypes;

    return <EditDialog id="rental-rates-edit" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title={
        <strong>Rental Agreement - Additional Rates</strong>
      }>
      <div className="forms-container">
        { Object.keys(this.state.forms).map(key => (
          <Form key={key}>
            <Grid fluid>
              <Row>
                <Col md={3}>
                  <FormGroup controlId={`componentName${key}`} validationState={ this.state.forms[key].componentNameError ? 'error' : null }>
                    <ControlLabel>Rate Component <sup>*</sup></ControlLabel>
                    <DropdownControl id={`componentName${key}`} disabled={ isReadOnly } updateState={ this.updateRateTypeState }
                      items={ provincialRateTypes } fieldName="description" selectedId={ this.state.forms[key].rateType.id } />
                    <HelpBlock>{ this.state.forms[key].componentNameError }</HelpBlock>
                  </FormGroup>
                </Col>
                <Col md={2}>
                  <FormGroup controlId={`ratePeriod${key}`} validationState={ this.state.forms[key].ratePeriodError ? 'error' : null }>
                    <ControlLabel>Period <sup>*</sup></ControlLabel>
                    <DropdownControl id={`ratePeriod${key}`} title={ this.state.forms[key].ratePeriod } updateState={ this.updateRatePeriodState }
                      items={[ Constant.RENTAL_RATE_PERIOD_HOURLY, Constant.RENTAL_RATE_PERIOD_DAILY ]} disabled={ !this.state.forms[key].rateType.isRateEditable }  />
                    <HelpBlock>{ this.state.forms[key].ratePeriodError }</HelpBlock>
                  </FormGroup>
                </Col>
                <Col md={2}>
                  <FormGroup controlId={`percentOrRateValue${key}`} validationState={ this.state.forms[key].rateError ? 'error' : null }>
                    <ControlLabel>Rate <sup>*</sup></ControlLabel>
                    <FormInputControl type="float" min={ 0 } value={ this.state.forms[key].percentOrRateValue } disabled={ !this.state.forms[key].rateType.isRateEditable } updateState={ this.updateUIState } />
                    <HelpBlock>{ this.state.forms[key].rateError }</HelpBlock>
                  </FormGroup>
                </Col>
                <Col md={2}>
                  <FormGroup controlId={`percentOrRateOption${key}`}>
                    <ControlLabel>&nbsp;</ControlLabel>
                    <DropdownControl id={`percentOrRateOption${key}`} disabled={ !this.state.forms[key].rateType.isRateEditable }  title={ this.state.forms[key].percentOrRateOption } updateState={ this.updateUIState }
                      items={[ DOLLAR_RATE, PERCENT_RATE ]} />
                  </FormGroup>
                </Col>
                <Col md={3}>
                  <FormGroup controlId={`isIncludedInTotal${key}`}>
                    <ControlLabel />
                    <CheckboxControl id={`isIncludedInTotal${key}`} disabled={ !this.state.forms[key].rateType.isInTotalEditable || this.state.forms[key].differentRatePeriods } checked={ this.state.forms[key].isIncludedInTotal } updateState={ this.updateState }>Include in total</CheckboxControl>
                  </FormGroup>
                </Col>
              </Row>
              <Row>
                <Col md={12}>
                  <FormGroup controlId={`comment${key}`} validationState={ this.state.forms[key].commentError ? 'error' : null }>
                    <ControlLabel>Comment</ControlLabel>
                    <FormInputControl componentClass="textarea" defaultValue={ this.state.forms[key].comment } readOnly={ isReadOnly } updateState={ this.updateState } />
                    <HelpBlock>{ this.state.forms[key].commentError }</HelpBlock>
                  </FormGroup>
                </Col>
              </Row>
              { this.state.forms[key].differentRatePeriods && 
              <Row>
                <Col md={12}>
                  <Alert bsStyle="warning">Only rates with same period as the pay rate can be added to the total.</Alert>
                </Col>
              </Row>
              }
            </Grid>
          </Form>
        ))}
      </div>
      <Grid fluid>
        <Row className="align-right">
          <Col md={12}>
            { this.state.isNew && this.state.numberOfInputs > 1 &&
            <Button 
              bsSize="xsmall"
              className="remove-btn"
              onClick={ this.removeInput }
            >
              <Glyphicon glyph="minus" />&nbsp;<strong>Remove</strong>
            </Button>
            }
            { this.state.isNew && this.state.numberOfInputs < 10 && 
            <Button 
              bsSize="xsmall"
              onClick={ this.addInput }
            >
              <Glyphicon glyph="plus" />&nbsp;<strong>Add</strong>
            </Button>
            }
          </Col>
        </Row>
      </Grid>
    </EditDialog>;
  },
});

export default RentalRatesEditDialog;
