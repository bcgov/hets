import React from 'react';

import { Grid, Row, Col } from 'react-bootstrap';
import { Form, FormGroup, HelpBlock, ControlLabel, FormControl } from 'react-bootstrap';

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
  },

  getInitialState() {
    var isNew = this.props.rentalRate.id === 0;

    return {
      isNew: isNew,

      isAttachment: this.props.rentalRate.isAttachment || false,
      componentName: this.props.rentalRate.componentName || '',
      rate: this.props.rentalRate.rate || 0.0,
      percentOfEquipmentRate: this.props.rentalRate.percentOfEquipmentRate || 0,
      ratePeriod: this.props.rentalRate.ratePeriod || '',
      comment: this.props.rentalRate.comment || '',
      includeInTotal: this.props.rentalRate.includeInTotal || false,

      ui : {
        percentOrRateOption: isNew || this.props.rentalRate.percentOfEquipmentRate > 0 ? PERCENT_RATE : DOLLAR_RATE,
        percentOrRateValue: this.props.rentalRate.rate || this.props.rentalRate.percentOfEquipmentRate || 0,
      },

      componentNameError: '',
      rateError: '',
      ratePeriodError: '',
    };
  },

  componentDidMount() {
    // TODO - use lookup list
    if (this.state.isNew) {
      this.setState({ ratePeriod: Constant.RENTAL_RATE_PERIOD_HOURLY });
    }
  },

  updateState(state, callback) {
    this.setState(state, callback);
  },

  updateUIState(state, callback) {
    var nextState = { ui: { ...this.state.ui, ...state } };

    // Update rate and percentOfEquipmentRate fields from what has been entered in the form
    var option = nextState.ui.percentOrRateOption;
    var value = nextState.ui.percentOrRateValue;

    this.setState({ ...nextState, ...{
      rate: option == DOLLAR_RATE ? value : 0,
      percentOfEquipmentRate: option == PERCENT_RATE ? value : 0,
    }}, callback);
  },

  didChange() {
    if (this.state.componentName !== this.props.rentalRate.componentName) { return true; }
    if (this.state.rate !== this.props.rentalRate.rate) { return true; }
    if (this.state.percentOfEquipmentRate !== this.props.rentalRate.percentOfEquipmentRate) { return true; }
    if (this.state.ratePeriod !== this.props.rentalRate.ratePeriod) { return true; }
    if (this.state.comment !== this.props.rentalRate.comment) { return true; }

    return false;
  },

  isValid() {
    this.setState({
      componentNameError: '',
      rateError: '',
      ratePeriodError: '',
    });

    var valid = true;

    if (isBlank(this.state.componentName)) {
      this.setState({ componentNameError: 'Rate type is required' });
      valid = false;
    }

    if (isBlank(this.state.ui.percentOrRateValue) ) {
      this.setState({ rateError: 'Pay rate is required' });
      valid = false;
    } else if (this.state.ui.percentOrRateValue < 1) {
      this.setState({ rateError: 'Pay rate not valid' });
      valid = false;
    }

    if (isBlank(this.state.ratePeriod)) {
      this.setState({ ratePeriodError: 'Period is required' });
      valid = false;
    }

    return valid;
  },

  onSave() {
    this.props.onSave({ ...this.props.rentalRate, ...{
      componentName: this.state.componentName,
      rate: this.state.rate,
      percentOfEquipmentRate: this.state.percentOfEquipmentRate,
      ratePeriod: this.state.ratePeriod,
      comment: this.state.comment,
    }});
  },

  dollarValue() {
    var option = this.state.ui.percentOrRateOption;
    var value = this.state.ui.percentOrRateValue;
    var equipmentRate = this.props.rentalRate.rentalAgreement ? this.props.rentalRate.rentalAgreement.equipmentRate : 0;

    if (option == PERCENT_RATE && value > 0) {
      return equipmentRate * value / 100;
    }
    return null;
  },

  render() {
    // Read-only if the user cannot edit the rental agreement
    var isReadOnly = !this.props.rentalRate.canEdit && this.props.rentalRate.id !== 0;

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
                {/*TODO - use lookup list*/}
                <DropdownControl id="componentName" disabled={ isReadOnly } title={ this.state.componentName } updateState={ this.updateState }
                  items={[ 'Base Rate', 'Rest Break Rate', 'Stand-By Rate', 'OT after 8 hours', 'OT after 12 hours' ]} />
                <HelpBlock>{ this.state.componentNameError }</HelpBlock>
              </FormGroup>
            </Col>
            <Col md={2}>
              <FormGroup controlId="ratePeriod" validationState={ this.state.ratePeriodError ? 'error' : null }>
                <ControlLabel>Period <sup>*</sup></ControlLabel>
                {/*TODO - use lookup list*/}
                <DropdownControl id="ratePeriod" disabled={ isReadOnly } title={ this.state.ratePeriod } updateState={ this.updateState }
                  items={[ Constant.RENTAL_RATE_PERIOD_HOURLY, Constant.RENTAL_RATE_PERIOD_DAILY ]} />
                <HelpBlock>{ this.state.ratePeriodError }</HelpBlock>
              </FormGroup>
            </Col>
            <Col md={2}>
              <FormGroup controlId="percentOrRateValue" validationState={ this.state.rateError ? 'error' : null }>
                <ControlLabel>Rate <sup>*</sup></ControlLabel>
                <FormInputControl type="number" min={ 0 } defaultValue={ this.state.ui.percentOrRateValue } readOnly={ isReadOnly } updateState={ this.updateUIState } inputRef={ ref => { this.input = ref; }}/>
                <HelpBlock>{ this.state.rateError }</HelpBlock>
              </FormGroup>
            </Col>
            <Col md={2}>
              <FormGroup controlId="percentOrRateOption">
                <ControlLabel>&nbsp;</ControlLabel>
                <DropdownControl id="percentOrRateOption" disabled={ isReadOnly } title={ this.state.ui.percentOrRateOption } updateState={ this.updateUIState }
                  items={[ PERCENT_RATE, DOLLAR_RATE ]} />
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
              <FormGroup controlId="comment">
                <ControlLabel>Comment</ControlLabel>
                <FormInputControl componentClass="textarea" defaultValue={ this.state.comment } readOnly={ isReadOnly } updateState={ this.updateState } />
              </FormGroup>
            </Col>
          </Row>
          <Row>
            <Col md={12}>
              <FormGroup controlId="includeInTotal">
                <CheckboxControl id="includeInTotal" checked={ this.state.includeInTotal } updateState={ this.updateState }>Include in total</CheckboxControl>
              </FormGroup>
            </Col>
          </Row>
        </Grid>
      </Form>
    </EditDialog>;
  },
});

export default RentalRatesEditDialog;
