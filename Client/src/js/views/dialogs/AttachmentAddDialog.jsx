import React from 'react';

import { Form, FormGroup, HelpBlock, ControlLabel } from 'react-bootstrap';

import EditDialog from '../../components/EditDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

var AttachmentAddDialog = React.createClass({
  propTypes: {
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
    equipment: React.PropTypes.object.isRequired,
  },

  getInitialState() {
    return {
      typeName: '',
      attachmentError: '',
    };
  },

  updateState(state, callback) {
    this.setState(state, callback);
  },

  didChange() {
    if (this.state.typeName !== '') { return true; }

    return false;
  },

  isValid() {
    this.setState({
      attachmentError: '',
    });

    var valid = true;

    if (this.state.typeName === '') {
      this.setState({ attachmentError: 'Attachment is required' });
      valid = false;
    }

    return valid;
  },

  onSave() {
    this.props.onSave({
      typeName: this.state.typeName,
      equipment: { id: this.props.equipment.id },
    });
  },

  render() {
    return <EditDialog id="equipment-add" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title= {
        <strong>Add Attachment</strong>
      }>
      <Form>
        <FormGroup controlId="typeName" validationState={ this.state.attachmentError ? 'error' : null }>
          <ControlLabel>Attachment</ControlLabel>
          <FormInputControl type="text" updateState={ this.updateState }/>
          <HelpBlock>{ this.state.attachmentError }</HelpBlock>
        </FormGroup>
      </Form>
    </EditDialog>;
  },
});

export default AttachmentAddDialog;
