import React from 'react';

import { Form, Row, Col, FormGroup, ControlLabel, HelpBlock } from 'react-bootstrap';

import EditDialog from '../../components/EditDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { isBlank } from '../../utils/string';

var NotesAddDialog = React.createClass({
  propTypes: {
    onSave: React.PropTypes.func.isRequired,
    onUpdate: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
    notes: React.PropTypes.object,
    note: React.PropTypes.object,
  },

  getInitialState() {
    return {
      noteId: this.props.note.id || 0,
      note: this.props.note.text || '',
      noteError: '',
      isNoLongerRelevant: false,
    };
  },

  updateState(state, callback) {
    this.setState(state, callback);
  },

  didChange() {
    if (this.props.note.text !== this.state.note) { return true; }

    return false;
  },

  isValid() {
    this.setState({
      noteError: '',
    });

    var valid = true;

    if (isBlank(this.state.note)) {
      this.setState({ noteError: 'Note is required' });
      valid = false;
    }

    return valid; 
  },

  onSave() {
    // If note id === 0 then you are adding a new note, otherwise you are updating an existing note
    if (this.state.noteId === 0) {
      this.props.onSave({
        id: 0,
        text: this.state.note,
        isNoLongerRelevant: false,
      });
    } else {
      this.props.onUpdate({
        id: this.state.noteId,
        text: this.state.note,
        isNoLongerRelevant: false,
      });
    }
  },

  render() {
    return <EditDialog id="notes" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } isValid={ this.isValid } didChange={ this.didChange }
      title= {
        <strong>Add Note</strong>
      }>
      <Row>
         <Col md={12}>
          <Form>
            <FormGroup controlId="note" validationState={ this.state.noteError ? 'error' : null }>
              <ControlLabel>Note</ControlLabel>
              <FormInputControl value={ this.state.note } componentClass="textarea" updateState={ this.updateState } /> 
              <HelpBlock>{ this.state.noteError }</HelpBlock>
            </FormGroup>
          </Form>
        </Col> 
      </Row>
    </EditDialog>;
  },
});

export default NotesAddDialog;