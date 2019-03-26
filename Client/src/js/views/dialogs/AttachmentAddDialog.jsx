import React from 'react';

import { FormGroup, HelpBlock, ControlLabel, Button, Glyphicon } from 'react-bootstrap';

import EditDialog from '../../components/EditDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';
import Form from '../../components/Form.jsx';

import { isBlank } from '../../utils/string';

var AttachmentAddDialog = React.createClass({
  propTypes: {
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
    equipment: React.PropTypes.object.isRequired,
  },

  getInitialState() {
    return {
      forms: [{
        typeName: '',
        attachmentError: '',
      }],
    };
  },

  updateState(state, index) {
    const forms = this.state.forms.slice();

    forms[index].typeName = state.typeName;

    this.setState({forms});
  },

  didChange() {
    if (this.state.typeName !== '') { return true; }

    return false;
  },

  isValid() {
    const forms = this.state.forms.slice();

    var valid = false;

    forms.forEach((form) => {
      const formIsValid = !isBlank(form.typeName);

      form.attachmentError = formIsValid ? '' : 'Attachment is required';

      valid = formIsValid;
    });

    this.setState({forms});

    return valid;
  },

  addInput() {
    if (this.state.forms.length < 10) {
      const forms = this.state.forms.slice(); // shallow clone

      forms.push({
        typeName: '',
        attachmentError: '',
      });

      this.setState({forms});
    }
  },

  removeInput() {
    if (this.state.forms.length > 1) {
      let forms = this.state.forms.slice();
      forms.splice(forms.length - 1, 1);
      this.setState({forms});
    }
  },

  onSave() {
    const attachmentTypeNames = this.state.forms.map((form) => form.typeName);
    this.props.onSave(attachmentTypeNames, this.props.equipment);
  },

  render() {
    const { forms } = this.state;

    return <EditDialog id="attachment-add" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title={<strong>Add Attachment</strong>}>
      <div className="forms-container">
        { forms.map((form, i) => (
          <Form key={i}>
            <FormGroup controlId="typeName" validationState={ form.attachmentError ? 'error' : null }>
              <ControlLabel>Attachment</ControlLabel>
              <FormInputControl type="text" updateState={ (state) => this.updateState(state, i) }/>
              <HelpBlock>{ form.attachmentError }</HelpBlock>
            </FormGroup>
          </Form>
        ))}
      </div>
      <div className="clearfix">
        { forms.length > 1 && (
          <Button
            bsSize="xsmall"
            className="remove-btn"
            onClick={ this.removeInput }>
            <Glyphicon glyph="minus" />&nbsp;<strong>Remove</strong>
          </Button>
        )}
        { forms.length < 10 && (
          <Button
            bsSize="xsmall"
            className="pull-right"
            onClick={ this.addInput }>
            <Glyphicon glyph="plus" />&nbsp;<strong>Add</strong>
          </Button>
        )}
      </div>
    </EditDialog>;
  },
});

export default AttachmentAddDialog;
