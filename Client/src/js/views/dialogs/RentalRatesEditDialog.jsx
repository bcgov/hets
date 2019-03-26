import React from 'react';

import { Grid, Row, Col } from 'react-bootstrap';
import { FormGroup, HelpBlock, ControlLabel, Button, Glyphicon } from 'react-bootstrap';

import _ from 'lodash';

import CheckboxControl from '../../components/CheckboxControl.jsx';
import EditDialog from '../../components/EditDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';
import Form from '../../components/Form.jsx';

import { isBlank } from '../../utils/string';

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
          isIncludedInTotal: this.props.rentalRate.isIncludedInTotal || false,
          rateType: {},
          rate: this.props.rentalRate.rate || 0.0,
          comment: this.props.rentalRate.comment || '',
          set: this.props.rentalRate.set || false,

          componentNameError: '',
          rateError: '',
          commentError: '',
        },
      },
      concurrencyControlNumber: this.props.rentalRate.concurrencyControlNumber || 0,
    };
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

  didChange() {
    return true;
  },

  isValid() {
    let forms = { ...this.state.forms };

    let formsResetObj = forms;
    Object.keys(forms).forEach((key) => {
      let state = {
        ...forms[key],
        rateError: '',
        commentError: '',
      };
      formsResetObj[key] = state;
    });

    this.setState({ forms: formsResetObj });
    let valid = true;

    let formsErrorsObj = forms;
    Object.keys(forms).forEach((key) => {

      if (isBlank(forms[key].comment)) {
        let state = { ...forms[key], commentError: 'Comment is required.' };
        formsErrorsObj[key] = state;
        valid = false;
      }

      if (isBlank(forms[key].rate) ) {
        let state = { ...forms[key], rateError: 'Pay rate is required' };
        formsErrorsObj[key] = state;
        valid = false;
      } else if (forms[key].rate < 1) {
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
    let rates = Object.keys(forms).map((key) => {
      return {
        id: this.props.rentalRate.id || 0,
        rentalAgreement: { id: this.props.rentalRate.rentalAgreement.id },
        rate: this.state.forms[key].rate,
        comment: this.state.forms[key].comment,
        set: this.state.forms[key].set,
        isIncludedInTotal: this.props.rentalRate.isIncludedInTotal,
        concurrencyControlNumber: this.state.concurrencyControlNumber,
      };
    });
    this.state.isNew ? this.props.onSaveMultiple(rates) : this.props.onSave(rates[0]);
  },

  addInput() {
    if (this.state.numberOfInputs < 10) {
      let numberOfInputs = Object.keys(this.state.forms).length;
      this.setState({
        numberOfInputs: this.state.numberOfInputs + 1,
        forms: {
          ...this.state.forms,
          [numberOfInputs + 1]: {
            isIncludedInTotal: this.props.rentalRate.isIncludedInTotal || false,
            rate: this.props.rentalRate.rate || 0.0,
            comment: this.props.rentalRate.comment || '',
            set: this.props.rentalRate.set || false,

            rateError: '',
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
    var status = this.props.rentalRate.isIncludedInTotal ? 'Included' : 'As-Needed';

    return <EditDialog id="rental-rates-edit" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title={<strong>Rental Agreement - {status} Rates and Attachments</strong>}>
      <div className="forms-container">
        { Object.keys(this.state.forms).map(key => (
          <Form key={key}>
            <Grid fluid>
              <Row>
                <Col md={2}>
                  <FormGroup controlId={`rate${key}`} validationState={ this.state.forms[key].rateError ? 'error' : null }>
                    <ControlLabel>Rate <sup>*</sup></ControlLabel>
                    <FormInputControl type="float" min={ 0 } defaultValue={ this.state.forms[key].rate.toFixed(2) } readOnly={ isReadOnly } updateState={ this.updateState } />
                    <HelpBlock>{ this.state.forms[key].rateError }</HelpBlock>
                  </FormGroup>
                </Col>
                {
                  !this.props.rentalRate.isIncludedInTotal &&
                  <Col md={2}>
                    <FormGroup controlId={`set${key}`}>
                      <ControlLabel>Set</ControlLabel>
                      <CheckboxControl id={`set${key}`} checked={ this.state.forms[key].set } updateState={ this.updateState }>Set</CheckboxControl>
                    </FormGroup>
                  </Col>
                }
                <Col md={2}>
                  <ControlLabel>Period</ControlLabel>
                  <div style={ { marginTop: '10px', marginBottom: '10px' } }>{ this.state.forms[key].set ? 'Set' : this.props.rentalAgreement.ratePeriod }</div>
                </Col>
                <Col md={ this.props.rentalRate.isIncludedInTotal ? 8 : 6 }>
                  <FormGroup controlId={`comment${key}`} validationState={ this.state.forms[key].commentError ? 'error' : null }>
                    <ControlLabel>Comment</ControlLabel>
                    <FormInputControl defaultValue={ this.state.forms[key].comment } readOnly={ isReadOnly } updateState={ this.updateState } />
                    <HelpBlock>{ this.state.forms[key].commentError }</HelpBlock>
                  </FormGroup>
                </Col>
              </Row>
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
