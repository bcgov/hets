import React from 'react';

import { Grid, Row, Col } from 'react-bootstrap';
import { Form, FormGroup, HelpBlock, ControlLabel, Button, Glyphicon } from 'react-bootstrap';

import _ from 'lodash';

import DropdownControl from '../../components/DropdownControl.jsx';
import EditDialog from '../../components/EditDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';
import CheckboxControl from '../../components/CheckboxControl.jsx';

import { isBlank } from '../../utils/string';

const PERCENT_RATE = '%';
const DOLLAR_RATE = '$';
const EQUIPMENT_ATTACHMENT_OTHER = 'Other';

var AttachmentRatesEditDialog = React.createClass({
  propTypes: {
    attachmentRate: React.PropTypes.object.isRequired,
    rentalAgreement: React.PropTypes.object.isRequired,
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
  },

  getInitialState() {
    var attachmentRate = this.props.attachmentRate;
    var rentalAgreement = this.props.rentalAgreement;
    var isNew = attachmentRate.id === 0;

    return {
      isNew: isNew,

      componentName: attachmentRate.componentName || '',
      rate: attachmentRate.rate || 0.0,
      percentOfEquipmentRate: attachmentRate.percentOfEquipmentRate || 0,
      comment: attachmentRate.comment || '',
      isIncludedInTotal: attachmentRate.isIncludedInTotal || false,
      ratePeriod: rentalAgreement.ratePeriod,  // The period for attachments is the same as for the Pay Rate so is there, but not displayed.
      numberOfInputs: 1,
      forms: {
        1: {
          componentName: attachmentRate.componentName || '',
          comment: attachmentRate.comment || '',
          rate: attachmentRate.rate || 0.0,
          percentOfEquipmentRate: attachmentRate.percentOfEquipmentRate || 0,
          percentOrRateOption: isNew || this.props.attachmentRate.percentOfEquipmentRate > 0 ? PERCENT_RATE : DOLLAR_RATE,
          percentOrRateValue: isNew ? 10 : this.props.attachmentRate.rate || this.props.attachmentRate.percentOfEquipmentRate || 0,
          commentError: '',
          componentNameError: '',
          isIncludedInTotal: this.props.attachmentRate.isIncludedInTotal || false,
          ratePeriod: rentalAgreement.ratePeriod,
        },
      },

      // ui : {
      //   percentOrRateOption: isNew || this.props.attachmentRate.percentOfEquipmentRate > 0 ? PERCENT_RATE : DOLLAR_RATE,
      //   percentOrRateValue: isNew ? 10 : this.props.attachmentRate.rate || this.props.attachmentRate.percentOfEquipmentRate || 0,  // Default for new records is 10%
      // },

      // componentNameError: '',
      // commentError: '',
      // rateError: '',
    };
  },

  componentDidMount() {
  },

  updateState(value) {
    // this.setState(state, callback);
    let property = Object.keys(value)[0];
    let stateValue = Object.values(value)[0];
    let number = property.match(/\d+/g)[0];
    let stateName = property.match(/[a-zA-Z]+/g)[0];
    let state = { [stateName]:  stateValue };
    let updatedState = { ...this.state.forms, [number]: { ...this.state.forms[number], ...state } };
    this.setState({ forms: updatedState });
  },

  updateUIState(value) {

    // // Update rate and percentOfEquipmentRate fields from what has been entered in the form
    // var option = nextState.ui.percentOrRateOption;
    // var value = nextState.ui.percentOrRateValue;

    // this.setState({ ...nextState, ...{
    //   rate: option == DOLLAR_RATE ? value : 0,
    //   percentOfEquipmentRate: option == PERCENT_RATE ? value : 0,
    // }}, callback);
    let property = Object.keys(value)[0];
    let stateValue = Object.values(value)[0];
    let number = property.match(/\d+/g)[0];
    let stateName = property.match(/[a-zA-Z]+/g)[0];
    // let nextState = { ui: { ...this.state.ui, ...value } };
    let nextState = { ...this.state.forms, [number]: { ...this.state.forms[number], ...{ [stateName]: stateValue } } };
    let option = nextState[number].percentOrRateOption;
    let percentOrRateValue = nextState[number].percentOrRateValue;
    let state = { 
      [stateName]:  stateValue,
      rate: option == DOLLAR_RATE ? percentOrRateValue : 0,
      percentOfEquipmentRate: option == PERCENT_RATE ? percentOrRateValue : 0,
    };
    let updatedState = { ...this.state.forms, [number]: { ...this.state.forms[number], ...state } };
    this.setState({ forms: updatedState });
  },

  didChange() {
    // if (this.state.componentName !== this.props.attachmentRate.componentName) { return true; }
    // if (this.state.rate !== this.props.attachmentRate.rate) { return true; }
    // if (this.state.isIncludedInTotal !== this.props.attachmentRate.isIncludedInTotal) { return true; }
    // if (this.state.percentOfEquipmentRate !== this.props.attachmentRate.percentOfEquipmentRate) { return true; }
    // if (this.state.comment !== this.props.attachmentRate.comment) { return true; }

    // return false;
    return true;
  },

  isValid() {
    // this.setState({
    //   componentNameError: '',
    //   rateError: '',
    // });

    // var valid = true;

    // if (isBlank(this.state.componentName)) {
    //   this.setState({ componentNameError: 'Rate type is required' });
    //   valid = false;
    // }

    // if (this.state.componentName === EQUIPMENT_ATTACHMENT_OTHER && isBlank(this.state.comment)) {
    //   this.setState({ commentError: 'Comment is required '});
    //   valid = false;
    // }

    // if (isBlank(this.state.ui.percentOrRateValue) ) {
    //   this.setState({ rateError: 'Pay rate is required' });
    //   valid = false;
    // } else if (this.state.ui.percentOrRateValue < 1) {
    //   this.setState({ rateError: 'Pay rate not valid' });
    //   valid = false;
    // }

    // return valid;
    let forms = { ...this.state.forms };

    let formsResetObj = forms;
    Object.keys(forms).map((key) => {
      let state = { ...forms[key], componentNameError: '', commentError: '' };
      formsResetObj[key] = state;
    });
    
    this.setState({ forms: formsResetObj });
    let valid = true;

    let formsErrorsObj = forms;
    Object.keys(forms).map((key) => {
      if (forms[key].componentName === EQUIPMENT_ATTACHMENT_OTHER && isBlank(forms[key].comment)) {
        let state = { ...forms[key], commentError: 'Comment is required.' };
        formsErrorsObj[key] = state;
        valid = false;
      }
      if (isBlank(forms[key].componentName)) {
        let state = { ...forms[key], componentNameError: 'Rate type is required.' };
        formsErrorsObj[key] = state;
        valid = false;
      }
      // if (isBlank(this.state.ui.percentOrRateValue) ) {
      //   this.setState({ rateError: 'Pay rate is required' });
      //   valid = false;
      // } else if (this.state.ui.percentOrRateValue < 1) {
      //   this.setState({ rateError: 'Pay rate not valid' });
      //   valid = false;
      // }
    });
    this.setState({ forms: formsErrorsObj });

    return valid;
  },

  onSave() {
    let forms = this.state.forms;
    let attachments = Object.keys(forms).map((key) => {
      delete forms[key].commentError;
      delete forms[key].componentNameError;
      delete forms[key].percentOrRateOption;
      delete forms[key].percentOrRateValue;
      return { ...this.props.rentalAgreement, rentalAgreement: { id: this.props.rentalAgreement.id }, ...forms[key] };
    });
    // this.props.onSave({ ...this.props.attachmentRate, ...{
    //   componentName: this.state.componentName,
    //   rate: this.state.rate,
    //   percentOfEquipmentRate: this.state.percentOfEquipmentRate,
    //   ratePeriod: this.state.ratePeriod,
    //   comment: this.state.comment,
    //   isIncludedInTotal: this.state.isIncludedInTotal,
    // }});
    this.props.onSave(attachments);
  },

  // dollarValue() {
  //   var option = this.state.ui.percentOrRateOption;
  //   var value = this.state.ui.percentOrRateValue;
  //   var equipmentRate = this.props.attachmentRate.rentalAgreement ? this.props.attachmentRate.rentalAgreement.equipmentRate : 0;

  //   if (option == PERCENT_RATE && value > 0) {
  //     return equipmentRate * value / 100;
  //   }
  //   return null;
  // },

  addInput() {
    if (this.state.numberOfInputs < 10) {
      let numberOfInputs = Object.keys(this.state.forms).length;
      this.setState({ 
        numberOfInputs: this.state.numberOfInputs + 1,
        forms: { 
          ...this.state.forms, 
          [numberOfInputs + 1]: { 
            componentName: '',
            comment: '',
            rate: 0.0,
            percentOfEquipmentRate: 0,
            percentOrRateOption: PERCENT_RATE,
            percentOrRateValue: 10,
            commentError: '',
            componentNameError: '',
            isIncludedInTotal: false,
            ratePeriod: this.props.rentalAgreement.ratePeriod,
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
    var attachmentRate = this.props.attachmentRate;
    var rentalAgreement = this.props.rentalAgreement;
    var attachments = _.map(rentalAgreement.equipment &&  [ ...rentalAgreement.equipment.equipmentAttachments, { typeName: EQUIPMENT_ATTACHMENT_OTHER } ] || [], 'typeName');
    // Read-only if the user cannot edit the rental agreement
    var isReadOnly = !attachmentRate.canEdit && attachmentRate.id !== 0;

    return <EditDialog id="rental-rates-edit" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title={
        <strong>Rental Agreement - Attachments</strong>
      }>
      { Object.keys(this.state.forms).map(key => {
        return (
        <Form key={key}>
          <Grid fluid>
            <Row>
              <Col md={5}>
                <FormGroup controlId="componentName" validationState={ this.state.forms[key].componentNameError ? 'error' : null }>
                  <ControlLabel>Rate Component <sup>*</sup></ControlLabel>
                  {/*TODO - use lookup list*/}
                  <DropdownControl id={`componentName${key}`} disabled={ isReadOnly } updateState={ this.updateState }
                    items={ attachments } title={ this.state.forms[key].componentName } className="full-width" />
                  <HelpBlock>{ this.state.forms[key].componentNameError }</HelpBlock>
                </FormGroup>
              </Col>
              <Col md={2}>
                <FormGroup controlId={`percentOrRateValue${key}`} validationState={ this.state.rateError ? 'error' : null }>
                  <ControlLabel>Rate <sup>*</sup></ControlLabel>
                  <FormInputControl type="float" min={ 0 } defaultValue={ this.state.forms[key].percentOrRateValue } readOnly={ isReadOnly } updateState={ this.updateUIState } inputRef={ ref => { this.input = ref; }}/>
                  <HelpBlock>{ this.state.rateError }</HelpBlock>
                </FormGroup>
              </Col>
              <Col md={2}>
                <FormGroup controlId={`percentOrRateOption${key}`}>
                  <ControlLabel>&nbsp;</ControlLabel>
                  <DropdownControl id={`percentOrRateOption${key}`} disabled={ isReadOnly } title={ this.state.forms[key].percentOrRateOption } updateState={ this.updateUIState }
                    items={[ PERCENT_RATE, DOLLAR_RATE ]} />
                </FormGroup>
              </Col>
              {/* <Col md={2}>
                <FormGroup>
                  <ControlLabel>&nbsp;</ControlLabel>
                  <FormControl.Static id="dollar-value" title={ formatCurrency(this.dollarValue()) }>{ formatCurrency(this.dollarValue()) }</FormControl.Static>
                </FormGroup>
              </Col> */}
              <Col md={3}>
                <FormGroup controlId={`isIncludedInTotal${key}`}>
                  <ControlLabel />
                  <CheckboxControl id={`isIncludedInTotal${key}`} checked={ this.state.forms[key].isIncludedInTotal } updateState={ this.updateState }>Include in total</CheckboxControl>
                </FormGroup>
              </Col>
            </Row>
            <Row>
              <Col md={12}>
                <FormGroup controlId={`comment${key}`} validationState={ this.state.forms[key].commentError ? 'error' : null }>
                  <ControlLabel>Comment</ControlLabel>
                  <FormInputControl componentClass="textarea" value={ this.state.forms[key].comment } readOnly={ isReadOnly } updateState={ this.updateState } />
                  <HelpBlock>{ this.state.forms[key].commentError }</HelpBlock>
                </FormGroup>
              </Col>
            </Row>
            <Row>
            </Row>
          </Grid>
          <hr />
        </Form>
        );
      })}
      { this.state.numberOfInputs < 10 && 
        <Button 
          bsSize="xsmall"
          onClick={ this.addInput }
        >
          <Glyphicon glyph="plus" />&nbsp;<strong>Add</strong>
        </Button>
      }
      { this.state.numberOfInputs > 1 &&
        <Button 
          bsSize="xsmall"
          className="remove-btn"
          onClick={ this.removeInput }
        >
          <Glyphicon glyph="minus" />&nbsp;<strong>Remove</strong>
        </Button>
      }
    </EditDialog>;
  },
});

export default AttachmentRatesEditDialog;
