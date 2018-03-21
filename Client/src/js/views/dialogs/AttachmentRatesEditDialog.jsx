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
    onSaveMultiple: React.PropTypes.func.isRequired,
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
          percentOfEquipmentRate: attachmentRate.percentOfEquipmentRate || isNew ? 10 : 0,
          percentOrRateOption: isNew || this.props.attachmentRate.percentOfEquipmentRate > 0 ? PERCENT_RATE : DOLLAR_RATE,
          percentOrRateValue: isNew ? 10 : this.props.attachmentRate.rate || this.props.attachmentRate.percentOfEquipmentRate || 0,
          commentError: '',
          componentNameError: '',
          isIncludedInTotal: this.props.attachmentRate.isIncludedInTotal || false,
          ratePeriod: rentalAgreement.ratePeriod,
        },
      },
    };
  },

  componentDidMount() {
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

  updateUIState(value) {
    let property = Object.keys(value)[0];
    let stateValue = _.values(value)[0];
    let number = property.match(/\d+/g)[0];
    let stateName = property.match(/[a-zA-Z]+/g)[0];
    let nextState = { ...this.state.forms, [number]: { ...this.state.forms[number], ...{ [stateName]: stateValue } } };

    // Update rate and percentOfEquipmentRate fields from what has been entered in the form
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
    return true;
  },

  isValid() {
    let forms = { ...this.state.forms };

    let formsResetObj = forms;
    Object.keys(forms).forEach((key) => {
      let state = { ...forms[key], componentNameError: '', commentError: '' };
      formsResetObj[key] = state;
    });
    
    this.setState({ forms: formsResetObj });
    let valid = true;

    let formsErrorsObj = forms;
    Object.keys(forms).forEach((key) => {
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
      return { ...this.props.attachmentRate, rentalAgreement: { id: this.props.rentalAgreement.id }, ...forms[key] };
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
            componentName: '',
            comment: '',
            rate: 0.0,
            percentOfEquipmentRate: 10,
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
      <div className="forms-container">
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
        </Form>
        );
      })}
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

export default AttachmentRatesEditDialog;
