import React from 'react';

import { FormGroup, HelpBlock, ControlLabel } from 'react-bootstrap';

import EditDialog from '../../components/EditDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';
import Form from '../../components/Form.jsx';

var AttachmentEditDialog = React.createClass({
  propTypes: {
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
    equipment: React.PropTypes.object.isRequired,
    attachment: React.PropTypes.object.isRequired,
  },

  getInitialState() {
    return {
      typeName: this.props.attachment.typeName,
      concurrencyControlNumber: this.props.attachment.concurrencyControlNumber || 0,
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
      id: this.props.attachment.id,
      typeName: this.state.typeName,
      concurrencyControlNumber: this.state.concurrencyControlNumber,
      equipment: { id: this.props.equipment.id },
    });
  },

  render() {
    return <EditDialog id="attachment-edit" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title={<strong>Edit Attachment</strong>}>
      <Form>
        <FormGroup controlId="typeName" validationState={ this.state.attachmentError ? 'error' : null }>
          <ControlLabel>Attachment</ControlLabel>
          <FormInputControl type="text" defaultValue={ this.state.typeName } updateState={ this.updateState }/>
          <HelpBlock>{ this.state.attachmentError }</HelpBlock>
        </FormGroup>
      </Form>
    </EditDialog>;
  },
});

export default AttachmentEditDialog;
