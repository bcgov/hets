import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';

import {  FormGroup, HelpBlock, ControlLabel } from 'react-bootstrap';

import * as Api from '../../api';

import FormDialog from '../../components/FormDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';


class ConditionAddEditDialog extends React.Component {
  static propTypes = {
    currentUser: PropTypes.object,
    onSave: PropTypes.func.isRequired,
    onClose: PropTypes.func.isRequired,
    show: PropTypes.bool,
    condition: PropTypes.object,
  };

  constructor(props) {
    super(props);

    this.state = {
      isSaving: false,
      isNew: props.condition.id === 0,

      conditionId: props.condition.id,
      typeCode: props.condition.conditionTypeCode || '',
      description: props.condition.description || '',
      concurrencyControlNumber: props.condition.concurrencyControlNumber || 0,
      typeCodeError: '',
      descriptionError: '',
    };
  }

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  didChange = () => {
    if (this.state.isNew && this.state.typeCode !== '') { return true; }
    if (this.state.isNew && this.state.description !== '') { return true; }
    if (!this.state.isNew && this.state.typeCode !== this.props.condition.conditionTypeCode) { return true; }
    if (!this.state.isNew && this.state.description !== this.props.condition.description) { return true; }

    return false;
  };

  isValid = () => {
    this.setState({
      typeCodeError: '',
      descriptionError: '',
    });

    var valid = true;

    if (this.state.typeCode === '') {
      this.setState({ typeCodeError: 'Condition type code is required' });
      valid = false;
    }

    if (this.state.description === '') {
      this.setState({ descriptionError: 'Description is required' });
      valid = false;
    }

    return valid;
  };

  onSave = () => {
    this.props.onSave({
      id: this.state.conditionId,
      conditionTypeCode: this.state.typeCode,
      description: this.state.description,
      concurrencyControlNumber: this.state.concurrencyControlNumber,
      active: true,
    });
  };

  formSubmitted = () => {
    if (this.isValid()) {
      if (this.didChange()) {
        this.setState({ isSaving: true });

        const condition = {
          id: this.state.conditionId,
          conditionTypeCode: this.state.typeCode,
          description: this.state.description,
          concurrencyControlNumber: this.state.concurrencyControlNumber,
          active: true,
          district: { id: this.props.currentUser.district.id },
        };

        const promise = this.state.isNew ? Api.addCondition(condition) : Api.updateCondition(condition);

        promise.then(() => {
          this.setState({ isSaving: false });
          if (this.props.onSave) { this.props.onSave(); }
          this.props.onClose();
        });
      } else {
        this.props.onClose();
      }
    }
  };

  render() {
    return (
      <FormDialog
        id="equipment-add"
        show={ this.props.show }
        title={ this.state.isNew ? 'Add Condition' : 'Edit Condition' }
        isSaving={ this.state.isSaving }
        onClose={ this.props.onClose }
        onSubmit={ this.formSubmitted }>
        <FormGroup controlId="typeCode" validationState={ this.state.typeCodeError ? 'error' : null }>
          <ControlLabel>Type Code <sup>*</sup></ControlLabel>
          <FormInputControl type="text" value={ this.state.typeCode } updateState={ this.updateState }/>
          <HelpBlock>{ this.state.typeCodeError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="description" validationState={ this.state.descriptionError ? 'error' : null }>
          <ControlLabel>Description <sup>*</sup></ControlLabel>
          <FormInputControl type="text" value={ this.state.description } updateState={ this.updateState }/>
          <HelpBlock>{ this.state.descriptionError }</HelpBlock>
        </FormGroup>
      </FormDialog>
    );
  }
}

function mapStateToProps(state) {
  return {
    currentUser: state.user,
  };
}

export default connect(mapStateToProps)(ConditionAddEditDialog);
