import React from 'react';

import { Grid, Row, Col } from 'react-bootstrap';
import { Form, FormGroup, HelpBlock, ControlLabel, FormControl } from 'react-bootstrap';

import _ from 'lodash';

import DropdownControl from '../../components/DropdownControl.jsx';
import EditDialog from '../../components/EditDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { isBlank, formatCurrency } from '../../utils/string';

const PERCENT_RATE = '%';
const DOLLAR_RATE = '$';

var AttachmentRatesEditDialog = React.createClass({
  propTypes: {
    attachmentRate: React.PropTypes.object.isRequired,
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
  },

  getInitialState() {
    var attachmentRate = this.props.attachmentRate;
    var rentalAgreement = attachmentRate.rentalAgreement;
    var isNew = attachmentRate.id === 0;

    return {
      isNew: isNew,

      componentName: attachmentRate.componentName || '',
      rate: attachmentRate.rate || 0.0,
      percentOfEquipmentRate: attachmentRate.percentOfEquipmentRate || 0,
      comment: attachmentRate.comment || '',
      ratePeriod: rentalAgreement.ratePeriod,  // The period for attachments is the same as for the Pay Rate so is there, but not displayed.

      ui : {
        percentOrRateOption: isNew || this.props.attachmentRate.percentOfEquipmentRate > 0 ? PERCENT_RATE : DOLLAR_RATE,
        percentOrRateValue: isNew ? 10 : this.props.attachmentRate.rate || this.props.attachmentRate.percentOfEquipmentRate || 0,  // Default for new records is 10%
      },

      componentNameError: '',
      rateError: '',
    };
  },

  componentDidMount() {
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
    if (this.state.componentName !== this.props.attachmentRate.componentName) { return true; }
    if (this.state.rate !== this.props.attachmentRate.rate) { return true; }
    if (this.state.percentOfEquipmentRate !== this.props.attachmentRate.percentOfEquipmentRate) { return true; }
    if (this.state.comment !== this.props.attachmentRate.comment) { return true; }

    return false;
  },

  isValid() {
    this.setState({
      componentNameError: '',
      rateError: '',
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

    return valid;
  },

  onSave() {
    this.props.onSave({ ...this.props.attachmentRate, ...{
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
    var equipmentRate = this.props.attachmentRate.rentalAgreement ? this.props.attachmentRate.rentalAgreement.equipmentRate : 0;

    if (option == PERCENT_RATE && value > 0) {
      return equipmentRate * value / 100;
    }
    return null;
  },

  render() {
    var attachmentRate = this.props.attachmentRate;
    var rentalAgreement = attachmentRate.rentalAgreement;
    var attachments = _.sortBy(rentalAgreement.equipment.equipmentAttachments || [], 'typeName');

    // Read-only if the user cannot edit the rental agreement
    var isReadOnly = !attachmentRate.canEdit && attachmentRate.id !== 0;

    return <EditDialog id="rental-rates-edit" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title={
        <strong>Rental Agreement - Attachments</strong>
      }>
      <Form>
        <Grid fluid>
          <Row>
            <Col md={6}>
              <FormGroup controlId="componentName" validationState={ this.state.componentNameError ? 'error' : null }>
                <ControlLabel>Rate Component <sup>*</sup></ControlLabel>
                {/*TODO - use lookup list*/}
                <DropdownControl id="componentName" disabled={ isReadOnly } title={ this.state.componentName } updateState={ this.updateState }
                  items={ attachments } />
                <HelpBlock>{ this.state.componentNameError }</HelpBlock>
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
        </Grid>
      </Form>
    </EditDialog>;
  },
});

export default AttachmentRatesEditDialog;
