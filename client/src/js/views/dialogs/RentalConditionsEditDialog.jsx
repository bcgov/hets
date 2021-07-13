import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { FormGroup, HelpBlock, ControlLabel, Button, Glyphicon } from 'react-bootstrap';
import _ from 'lodash';

import * as Api from '../../api';

import DropdownControl from '../../components/DropdownControl.jsx';
import FormDialog from '../../components/FormDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { isBlank } from '../../utils/string';
import { NON_STANDARD_CONDITION } from '../../constants';


class RentalConditionsEditDialog extends React.Component {
  static propTypes = {
    show: PropTypes.bool.isRequired,
    rentalAgreementId: PropTypes.number.isRequired,
    rentalCondition: PropTypes.object.isRequired,
    rentalConditions: PropTypes.object.isRequired,
    onSave: PropTypes.func,
    onClose: PropTypes.func.isRequired,
  };

  constructor(props) {
    super(props);

    this.state = {
      isNew: props.rentalCondition.id === 0,

      forms: [{
        conditionName: props.rentalCondition.conditionName || '',
        comment: props.rentalCondition.comment || '',

        conditionNameError: '',
        commentError: '',
      }],
      concurrencyControlNumber: props.rentalCondition.concurrencyControlNumber || 0,
    };
  }

  componentDidMount() {
    Api.getRentalConditions();
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

  didChange = () => {
    return true;
  };

  isValid = () => {
    const forms = this.state.forms.slice();

    forms.forEach((form, i) => {
      let state = {
        ...form,
        conditionNameError: '',
        commentError: '',
      };
      forms[i] = state;
    });

    let valid = true;

    forms.forEach((form, i) => {
      if (form.conditionName === NON_STANDARD_CONDITION && isBlank(form.comment)) {
        forms[i] = { ...form, commentError: 'Comment is required for non-standard conditions.' };
        valid = false;
      }

      if (isBlank(form.conditionName)) {
        forms[i] = { ...form, conditionNameError: 'Rental condition is required' };
        valid = false;
      }
    });

    this.setState({ forms });

    return valid;
  };

  formSubmitted = () => {
    const { rentalAgreementId, onSave, onClose } = this.props;

    if (this.isValid()) {
      if (this.didChange()) {
        const forms = this.state.forms;
        const conditions = forms.map((form) => {
          return {
            id: this.props.rentalCondition.id || 0,
            rentalAgreement: { id: rentalAgreementId },
            conditionName: form.conditionName,
            comment: form.conditionName === NON_STANDARD_CONDITION ? form.comment : '',
            concurrencyControlNumber: this.state.concurrencyControlNumber,
          };
        });

        (this.state.isNew ? Api.addRentalConditions(rentalAgreementId, conditions) : Api.updateRentalCondition(_.first(conditions))).then(() => {
          if (onSave) { onSave(); }
        });
      }
      onClose();
    }
  };

  addInput = () => {
    if (this.state.forms.length < 10) {
      const forms = this.state.forms.slice();
      forms.push({
        conditionName: '',
        comment: '',
        conditionNameError: '',
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

  render() {
    // Read-only if the user cannot edit the rental agreement
    var isReadOnly = !this.props.rentalCondition.canEdit && this.props.rentalCondition.id !== 0;
    var conditions = _.map([ ...this.props.rentalConditions.data, { description: NON_STANDARD_CONDITION } ], 'description');

    return (
      <FormDialog
        id="rental-conditions-edit"
        show={this.props.show}
        onClose={this.props.onClose}
        onSubmit={this.formSubmitted}
        title="Rental Agreement – Conditions">
        <div className="forms-container">
          { this.state.forms.map((form, i) => (
            <div className="form-item" key={i}>
              <div>
                <FormGroup controlId={`conditionName${i}`} validationState={ form.conditionNameError ? 'error' : null }>
                  <ControlLabel>Conditions <sup>*</sup></ControlLabel>
                  {/*TODO - use lookup list*/}
                  <DropdownControl
                    id={`conditionName${i}`}
                    disabled={isReadOnly || !this.props.rentalConditions.loaded}
                    updateState={this.updateState}
                    items={conditions}
                    title={form.conditionName}
                    className="full-width" />
                  <HelpBlock>{ form.conditionNameError }</HelpBlock>
                </FormGroup>
              </div>
              { form.conditionName === NON_STANDARD_CONDITION && (
                <div>
                  <FormGroup controlId={`comment${i}`} validationState={ form.commentError ? 'error' : null }>
                    <ControlLabel>Comment <sup>*</sup></ControlLabel>
                    <FormInputControl componentClass="textarea" defaultValue={ form.comment } readOnly={ isReadOnly } updateState={ this.updateState } />
                    <HelpBlock>{ form.commentError }</HelpBlock>
                  </FormGroup>
                </div>
              )}
            </div>
          ))}
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


function mapStateToProps(state) {
  return {
    rentalConditions: state.lookups.rentalConditions,
  };
}

export default connect(mapStateToProps)(RentalConditionsEditDialog);
