import React from 'react';

import {  FormGroup, HelpBlock, ControlLabel } from 'react-bootstrap';

import EditDialog from '../../components/EditDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';
import Form from '../../components/Form.jsx';


class ConditionAddEditDialog extends React.Component {
  static propTypes = {
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
    condition: React.PropTypes.object,
  };

  constructor(props) {
    super(props);

    this.state = {
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

  render() {
    return <EditDialog id="equipment-add" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title={<strong>Add Condition</strong>}>
      <Form>
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
      </Form>
    </EditDialog>;
  }
}

export default ConditionAddEditDialog;
