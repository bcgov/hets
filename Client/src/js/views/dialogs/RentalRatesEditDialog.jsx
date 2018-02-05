import React from 'react';

import { Grid, Row, Col } from 'react-bootstrap';
import { Form, FormGroup, HelpBlock, ControlLabel, FormControl } from 'react-bootstrap';

import _ from 'lodash';

import * as Constant from '../../constants';

import DropdownControl from '../../components/DropdownControl.jsx';
import EditDialog from '../../components/EditDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';
import CheckboxControl from '../../components/CheckboxControl.jsx';

import { isBlank, formatCurrency } from '../../utils/string';

const PERCENT_RATE = '%';
const DOLLAR_RATE = '$';

var RentalRatesEditDialog = React.createClass({
  propTypes: {
    rentalRate: React.PropTypes.object.isRequired,
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
    provincialRateTypes: React.PropTypes.array,
  },

  getInitialState() {
    var isNew = this.props.rentalRate.id === 0;

    return {
      isNew: isNew,

      isAttachment: this.props.rentalRate.isAttachment || false,
      componentName: this.props.rentalRate.componentName || '',
      rateType: {},
      rate: this.props.rentalRate.rate || 0.0,
      percentOfEquipmentRate: this.props.rentalRate.percentOfEquipmentRate || 0,
      ratePeriod: this.props.rentalRate.ratePeriod || '',
      comment: this.props.rentalRate.comment || '',
      includeInTotal: this.props.rentalRate.includeInTotal || false,

      // UI fields
      percentOrRateOption: isNew || this.props.rentalRate.percentOfEquipmentRate > 0 ? PERCENT_RATE : DOLLAR_RATE,
      percentOrRateValue: this.props.rentalRate.rate || this.props.rentalRate.percentOfEquipmentRate || 0,

      componentNameError: '',
      rateError: '',
      ratePeriodError: '',
      commentError: '',
    };
  },

  componentDidMount() {
    if (this.state.isNew) {
      this.setState({ ratePeriod: Constant.RENTAL_RATE_PERIOD_HOURLY });
    }
    if (this.props.rentalRate.componentName) {
      var rateType = _.find(this.props.provincialRateTypes, {description: this.props.rentalRate.componentName } );
      this.setState({ 
        rateType: rateType, 
      });
    }
  },

  updateState(state, callback) {
    this.setState(state, callback);
  },

  updateRateTypeState(state) {
    var provincialRateTypes = this.props.provincialRateTypes;
    var rateType = _.find(provincialRateTypes, {id: state.componentName } );
    this.setState({ 
      rateType: rateType, 
      rate: rateType.rate,
      percentOrRateValue: rateType.rate ? rateType.rate : 0, 
      percentOfEquipmentRate: rateType.isPercentRate ? rateType.rate : 0,
      percentOrRateOption: rateType.isPercentRate ? PERCENT_RATE : DOLLAR_RATE,
      ...state, 
    });
  },

  updateUIState(state) {
    if (state.percentOrRateValue) {
      let percentOfEquipmentRate = this.state.percentOrRateOption == PERCENT_RATE ? state.percentOrRateValue : 0;
      let rate = this.state.percentOrRateOption == DOLLAR_RATE ? state.percentOrRateValue : 0;
      return this.setState({ ...state, ...{ percentOfEquipmentRate: percentOfEquipmentRate, rate: rate } });
    }
    let percentOfEquipmentRate = state.percentOrRateOption == PERCENT_RATE ? this.state.percentOrRateValue : 0;
    let rate = state.percentOrRateOption == DOLLAR_RATE ? this.state.percentOrRateValue : 0;
    this.setState({ ...state, ...{ percentOfEquipmentRate: percentOfEquipmentRate, rate: rate } });
  },

  didChange() {
    if (this.state.componentName !== this.props.rentalRate.componentName) { return true; }
    if (this.state.rate !== this.props.rentalRate.rate) { return true; }
    if (this.state.percentOfEquipmentRate !== this.props.rentalRate.percentOfEquipmentRate) { return true; }
    if (this.state.ratePeriod !== this.props.rentalRate.ratePeriod) { return true; }
    if (this.state.comment !== this.props.rentalRate.comment) { return true; }
    if (this.state.includeInTotal !== this.props.rentalRate.isIncludedInTotal) { return true; }

    return false;
  },

  isValid() {
    this.setState({
      componentNameError: '',
      rateError: '',
      ratePeriodError: '',
      commentError: '',
    });

    var valid = true;

    if (isBlank(this.state.componentName)) {
      this.setState({ componentNameError: 'Rate type is required' });
      valid = false;
    }

    if (isBlank(this.state.percentOrRateValue) ) {
      this.setState({ rateError: 'Pay rate is required' });
      valid = false;
    } else if (this.state.percentOrRateValue < 1) {
      this.setState({ rateError: 'Pay rate not valid' });
      valid = false;
    }

    if (isBlank(this.state.ratePeriod)) {
      this.setState({ ratePeriodError: 'Period is required' });
      valid = false;
    }
    
    if (this.state.rateType.description === Constant.NON_STANDARD_CONDITION && isBlank(this.state.comment)) {
      this.setState({ commentError: 'Comment is required for non-standard conditions' });
      valid = false;
    }

    return valid;
  },

  onSave() {
    this.props.onSave({ ...this.props.rentalRate, ...{
      componentName: this.state.rateType.description,
      rate: this.state.rate,
      percentOfEquipmentRate: this.state.percentOfEquipmentRate,
      ratePeriod: this.state.ratePeriod,
      comment: this.state.comment,
      isIncludedInTotal: this.state.includeInTotal,
    }});
  },

  dollarValue() {
    var option = this.state.percentOrRateOption;
    var value = this.state.percentOrRateValue;
    var equipmentRate = this.props.rentalRate.rentalAgreement ? this.props.rentalRate.rentalAgreement.equipmentRate : 0;
    if (option == PERCENT_RATE && value > 0) {
      return equipmentRate * value / 100;
    }
    return null;
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
      <Form>
        <Grid fluid>
          <Row>
            <Col md={4}>
              <FormGroup controlId="componentName" validationState={ this.state.componentNameError ? 'error' : null }>
                <ControlLabel>Rate Component <sup>*</sup></ControlLabel>
                <DropdownControl id="componentName" disabled={ isReadOnly } updateState={ this.updateRateTypeState }
                  items={ provincialRateTypes } fieldName="description" selectedId={ this.state.rateType.id } />
                <HelpBlock>{ this.state.componentNameError }</HelpBlock>
              </FormGroup>
            </Col>
            <Col md={2}>
              <FormGroup controlId="ratePeriod" validationState={ this.state.ratePeriodError ? 'error' : null }>
                <ControlLabel>Period <sup>*</sup></ControlLabel>
                {/*TODO - use lookup list*/}
                <DropdownControl id="ratePeriod" title={ this.state.ratePeriod } updateState={ this.updateState }
                  items={[ Constant.RENTAL_RATE_PERIOD_HOURLY, Constant.RENTAL_RATE_PERIOD_DAILY ]} disabled={ !this.state.rateType.isRateEditable }  />
                <HelpBlock>{ this.state.ratePeriodError }</HelpBlock>
              </FormGroup>
            </Col>
            <Col md={2}>
              <FormGroup controlId="percentOrRateValue" validationState={ this.state.rateError ? 'error' : null }>
                <ControlLabel>Rate <sup>*</sup></ControlLabel>
                <FormInputControl type="float" min={ 0 } value={ this.state.percentOrRateValue } disabled={ !this.state.rateType.isRateEditable } updateState={ this.updateUIState } />
                <HelpBlock>{ this.state.rateError }</HelpBlock>
              </FormGroup>
            </Col>
            <Col md={2}>
              <FormGroup controlId="percentOrRateOption">
                <ControlLabel>&nbsp;</ControlLabel>
                <DropdownControl id="percentOrRateOption" disabled={ !this.state.rateType.isRateEditable }  title={ this.state.percentOrRateOption } updateState={ this.updateUIState }
                  items={[ DOLLAR_RATE, PERCENT_RATE ]} />
              </FormGroup>
            </Col>
            <Col md={2}>
              <FormGroup>
                <ControlLabel>&nbsp;</ControlLabel>
                <FormControl.Static id="dollar-value" title={ formatCurrency(this.dollarValue()) }>{ formatCurrency(this.dollarValue()) }</FormControl.Static>
              </FormGroup>
            </Col>
          </Row>
          <Row>
            <Col md={12}>
              <FormGroup controlId="comment" validationState={ this.state.commentError ? 'error' : null }>
                <ControlLabel>Comment</ControlLabel>
                <FormInputControl componentClass="textarea" defaultValue={ this.state.comment } readOnly={ isReadOnly } updateState={ this.updateState } />
                <HelpBlock>{ this.state.commentError }</HelpBlock>
              </FormGroup>
            </Col>
          </Row>
          <Row>
            <Col md={12}>
              <FormGroup controlId="includeInTotal">
                <CheckboxControl id="includeInTotal" disabled={ !this.state.rateType.isInTotalEditable } checked={ this.state.includeInTotal } updateState={ this.updateState }>Include in total</CheckboxControl>
              </FormGroup>
            </Col>
          </Row>
        </Grid>
      </Form>
    </EditDialog>;
  },
});

export default RentalRatesEditDialog;
