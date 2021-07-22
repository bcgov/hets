import PropTypes from 'prop-types';
import React from 'react';

import { FormGroup, FormLabel, FormText } from 'react-bootstrap';

import FormDialog from '../../components/FormDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import * as Constant from '../../constants';

import { isBlank } from '../../utils/string';

class NotesAddDialog extends React.Component {
  static propTypes = {
    onSave: PropTypes.func.isRequired,
    onUpdate: PropTypes.func.isRequired,
    onClose: PropTypes.func.isRequired,
    show: PropTypes.bool,
    note: PropTypes.object,
  };

  constructor(props) {
    super(props);

    this.state = {
      noteId: props.note.id || 0,
      note: props.note.text || '',
      concurrencyControlNumber: props.note.concurrencyControlNumber || 0,
      noteError: '',
      isNoLongerRelevant: false,
    };
  }

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  didChange = () => {
    if (this.props.note.text !== this.state.note) {
      return true;
    }

    return false;
  };

  isValid = () => {
    this.setState({
      noteError: '',
    });

    var valid = true;

    if (isBlank(this.state.note)) {
      this.setState({ noteError: 'Note is required' });
      valid = false;
    }

    return valid;
  };

  onFormSubmitted = () => {
    if (this.isValid()) {
      if (this.didChange()) {
        // If note id === 0 then you are adding a new note, otherwise you are updating an existing note
        if (this.state.noteId === 0) {
          this.props.onSave({
            id: 0,
            text: this.state.note,
            isNoLongerRelevant: false,
            createDate: new Date().toISOString(),
          });
        } else {
          this.props.onUpdate({
            id: this.state.noteId,
            text: this.state.note,
            concurrencyControlNumber: this.state.concurrencyControlNumber,
            isNoLongerRelevant: false,
          });
        }
      } else {
        this.props.onClose();
      }
    }
  };

  render() {
    const { noteId } = this.state;
    var maxLength = Constant.MAX_LENGTH_NOTE_TEXT;

    return (
      <FormDialog
        id="notes"
        title={noteId ? 'Edit Note' : 'Add Note'}
        show={this.props.show}
        onClose={this.props.onClose}
        onSubmit={this.onFormSubmitted}
      >
        <FormGroup controlId="note">
          <FormLabel>Note</FormLabel>
          <FormInputControl
            value={this.state.note}
            as="textarea"
            updateState={this.updateState}
            maxLength={maxLength}
            isInvalid={this.state.noteError}
          />
          <FormText>{this.state.noteError}</FormText>
          <p>Maximum {maxLength} characters.</p>
        </FormGroup>
      </FormDialog>
    );
  }
}

export default NotesAddDialog;
