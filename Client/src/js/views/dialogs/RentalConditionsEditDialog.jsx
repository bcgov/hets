import React from 'react';

import { Grid, Row, Col } from 'react-bootstrap';
import { Form, FormGroup, HelpBlock, ControlLabel, Button, Glyphicon } from 'react-bootstrap';

import _ from 'lodash';

import DropdownControl from '../../components/DropdownControl.jsx';
import EditDialog from '../../components/EditDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { isBlank } from '../../utils/string';
import { NON_STANDARD_CONDITION } from '../../constants';

var RentalConditionsEditDialog = React.createClass({
  propTypes: {
    rentalCondition: React.PropTypes.object.isRequired,
    rentalConditions: React.PropTypes.array.isRequired,
    onSave: React.PropTypes.func.isRequired,
    onSaveMultiple: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
  },

  getInitialState() {
    var isNew = this.props.rentalCondition.id === 0;

    return {
      isNew: isNew,
      numberOfInputs: 1,

      forms: {
        1: {
          conditionName: this.props.rentalCondition.conditionName || '',
          comment: this.props.rentalCondition.comment || '',

          conditionNameError: '',
          commentError: '',
        },
      },
    };
  },

  updateState(value) {
    let property = Object.keys(value)[0];
    let stateValue = Object.values(value)[0];
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
    Object.keys(forms).map((key) => {
      let state = { ...forms[key], conditionNameError: '', commentError: '' };
      formsResetObj[key] = state;
    });
    
    this.setState({ forms: formsResetObj });
    let valid = true;

    let formsErrorsObj = forms;
    Object.keys(forms).map((key) => {

      if (forms[key].conditionName === NON_STANDARD_CONDITION && isBlank(forms[key].comment)) {
        let state = { ...forms[key], commentError: 'Comment is required for non-standard conditions.' };
        formsErrorsObj[key] = state;
        valid = false;
      }

      if (isBlank(forms[key].conditionName)) {
        let state = { ...forms[key], conditionNameError: 'Rental condition is required' };
        formsErrorsObj[key] = state;
        valid = false;
      }

    });

    this.setState({ forms: formsErrorsObj });

    return valid;
  },

  onSave() {
    let forms = this.state.forms;
    let conditions = Object.keys(forms).map((key) => {
      return { 
        id: this.props.rentalCondition.id || 0,
        rentalAgreement: { id: this.props.rentalCondition.rentalAgreement.id },
        conditionName: this.state.forms[key].conditionName,
        comment: this.state.forms[key].conditionName === NON_STANDARD_CONDITION ? this.state.forms[key].comment : '',
      };
    });
    this.state.isNew ? this.props.onSaveMultiple(conditions) : this.props.onSave(conditions[0]);
  },

  addInput() {
    if (this.state.numberOfInputs < 10) {
      let numberOfInputs = Object.keys(this.state.forms).length;
      this.setState({ 
        numberOfInputs: this.state.numberOfInputs + 1,
        forms: { 
          ...this.state.forms, 
          [numberOfInputs + 1]: { 
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
    var isReadOnly = !this.props.rentalCondition.canEdit && this.props.rentalCondition.id !== 0;
    var conditions = _.map([ ...this.props.rentalConditions, { description: NON_STANDARD_CONDITION } ], 'description');

    return <EditDialog id="rental-conditions-edit" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title={
        <strong>Rental Agreement - Conditions</strong>
      }>
      <div className="forms-container">
      { Object.keys(this.state.forms).map(key => (
        <Form key={key}>
          <Grid fluid>
            <Row>
              <Col md={12}>
                <FormGroup controlId={`conditionName${key}`} validationState={ this.state.forms[key].conditionNameError ? 'error' : null }>
                  <ControlLabel>Rate Component <sup>*</sup></ControlLabel>
                  {/*TODO - use lookup list*/}
                  <DropdownControl id={`conditionName${key}`}  disabled={ isReadOnly } updateState={ this.updateState }
                    items={ conditions } title={ this.state.forms[key].conditionName } className="full-width" />
                  <HelpBlock>{ this.state.forms[key].conditionNameError }</HelpBlock>
                </FormGroup>
              </Col>
            </Row>
            { this.state.forms[key].conditionName === NON_STANDARD_CONDITION && 
            <Row>
              <Col md={12}>
                <FormGroup controlId={`comment${key}`} validationState={ this.state.forms[key].commentError ? 'error' : null }>
                  <ControlLabel>Comment</ControlLabel>
                  <FormInputControl componentClass="textarea" defaultValue={ this.state.forms[key].comment } readOnly={ isReadOnly } updateState={ this.updateState } />
                  <HelpBlock>{ this.state.forms[key].commentError }</HelpBlock>
                </FormGroup>
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

export default RentalConditionsEditDialog;
