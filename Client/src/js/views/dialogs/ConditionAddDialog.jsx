import React from 'react';

import { Form, FormGroup, HelpBlock, ControlLabel } from 'react-bootstrap';

import EditDialog from '../../components/EditDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

var ConditionAddDialog = React.createClass({
  propTypes: {
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
    condition: React.PropTypes.object,
  },

  getInitialState() {
    return {
      isNew: this.props.condition.id === 0,

      conditionId: this.props.condition.id,
      typeCode: this.props.condition.conditionTypeCode || '',
      description: this.props.condition.description || '',
      typeCodeError: '',
      descriptionError: '',
    };
  },

  updateState(state, callback) {
    this.setState(state, callback);
  },

  didChange() {
    if (this.state.typeCode !== '') { return true; }
    if (this.state.description !== '') { return true; }

    return false;
  },

  isValid() {
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
  },

  onSave() {
    this.props.onSave({
      id: this.state.conditionId,
      conditionTypeCode: this.state.typeCode,
      description: this.state.description,
      active: true,
    });
  },

  render() {
    return <EditDialog id="equipment-add" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title= {
        <strong>Add Condition</strong>
      }>
      <Form>
        <FormGroup controlId="typeCode" validationState={ this.state.typeCodeError ? 'error' : null }>
          <ControlLabel>Type Code</ControlLabel>
          <FormInputControl type="text" value={ this.state.typeCode } updateState={ this.updateState }/>
          <HelpBlock>{ this.state.typeCodeError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="description" validationState={ this.state.descriptionError ? 'error' : null }>
          <ControlLabel>Description</ControlLabel>
          <FormInputControl type="text" value={ this.state.description } updateState={ this.updateState }/>
          <HelpBlock>{ this.state.descriptionError }</HelpBlock>
        </FormGroup>
      </Form>
    </EditDialog>;
  },
});

export default ConditionAddDialog;
