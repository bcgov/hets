import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';

import { FormGroup, FormText, FormLabel } from 'react-bootstrap';

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
    if (this.state.isNew && this.state.typeCode !== '') {
      return true;
    }
    if (this.state.isNew && this.state.description !== '') {
      return true;
    }
    if (!this.state.isNew && this.state.typeCode !== this.props.condition.conditionTypeCode) {
      return true;
    }
    if (!this.state.isNew && this.state.description !== this.props.condition.description) {
      return true;
    }

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

  formSubmitted = async () => {
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

        const promise = this.state.isNew ? Api.addCondition : Api.updateCondition;

        await this.props.dispatch(promise(condition));
        this.setState({ isSaving: false });
        if (this.props.onSave) {
          this.props.onSave();
        }
        this.props.onClose();
      } else {
        this.props.onClose();
      }
    }
  };

  render() {
    return (
      <FormDialog
        id="equipment-add"
        show={this.props.show}
        title={this.state.isNew ? 'Add Condition' : 'Edit Condition'}
        isSaving={this.state.isSaving}
        onClose={this.props.onClose}
        onSubmit={this.formSubmitted}
      >
        <FormGroup controlId="typeCode">
          <FormLabel>
            Type Code <sup>*</sup>
          </FormLabel>
          <FormInputControl
            type="text"
            value={this.state.typeCode}
            updateState={this.updateState}
            autoFocus
            isInvalid={this.state.typeCodeError}
          />
          <FormText>{this.state.typeCodeError}</FormText>
        </FormGroup>
        <FormGroup controlId="description">
          <FormLabel>
            Description <sup>*</sup>
          </FormLabel>
          <FormInputControl
            type="text"
            value={this.state.description}
            updateState={this.updateState}
            isInvalid={this.state.descriptionError}
          />
          <FormText>{this.state.descriptionError}</FormText>
        </FormGroup>
      </FormDialog>
    );
  }
}

const mapStateToProps = (state) => ({
  currentUser: state.user,
});

const mapDispatchToProps = (dispatch) => ({ dispatch });

export default connect(mapStateToProps, mapDispatchToProps)(ConditionAddEditDialog);
