import React from 'react';

import { ButtonGroup, Button, Glyphicon, Alert } from 'react-bootstrap';

import _ from 'lodash';

import * as Api from '../../api';

import NotesAddDialog from './NotesAddDialog.jsx';
import ModalDialog from '../../components/ModalDialog.jsx';
import TableControl from '../../components/TableControl.jsx';
import DeleteButton from '../../components/DeleteButton.jsx';
import EditButton from '../../components/EditButton.jsx';

var NotesDialog = React.createClass({
  propTypes: {
    // Api function to call on save
    id: React.PropTypes.string.isRequired,
    // Api function to call when updating a note
    // Api call to get notes for particular entity
    onUpdate: React.PropTypes.func.isRequired,
    getNotes: React.PropTypes.func.isRequired,
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
    notes: React.PropTypes.object,
  },

  getInitialState() {
    return {
      note: {},
      notes: this.props.notes ? _.values(this.props.notes) : [],
    };
  },

  componentWillReceiveProps(nextProps) {
    if (!_.isEqual(Object.keys(this.props.notes), Object.keys(nextProps.notes))) {
      this.setState({ notes: _.values(nextProps.notes) });
    }
  },

  openNotesAddDialog() {
    this.setState({ showNotesAddDialog : true });
  },

  closeNotesAddDialog() {
    this.setState({
      note: {},
      showNotesAddDialog: false,
    });
  },

  onNoteAdded(note) {
    this.setState({ notes: this.state.notes.concat([note])});
    this.props.onSave(this.props.id, note);
    this.closeNotesAddDialog();
  },

  onNoteUpdated(note) {
    const noteId = note.id;
    const updatedNotes = this.state.notes.map((_note) => {
      return _note.id === noteId ? note : _note;
    });

    this.setState({ notes: updatedNotes });
    this.props.onUpdate(note);
    this.closeNotesAddDialog();
  },

  deleteNote(note) {
    const noteId = note.id;
    const updatedNotes = this.state.notes.filter((note) => {
      return note.id !== noteId;
    });

    this.setState({ notes: updatedNotes });
    Api.deleteNote(note.id).then(() => {

    });
  },

  editNote(note) {
    this.setState({
      note: note,
      showNotesAddDialog: true,
    });
  },

  onClose() {
    this.props.getNotes(this.props.id);
    this.props.onClose();
  },

  render() {
    const { notes } = this.state;

    var headers = [
      { field: 'note',            title: 'Note'  },
      { field: 'blank'                           },
    ];

    const showNoNotesMessage = !notes || notes.length === 0;

    return (
      <ModalDialog
        id="notes"
        show={ this.props.show }
        onClose={ this.onClose }
        title={<strong>Notes</strong>}>
        <TableControl id="notes-list" headers={ headers }>
          {
            notes.map((note) => {
              return (
                <tr key={ note.id }>
                  <td>{ note.text }</td>
                  <td style={{ textAlign: 'right', minWidth: '60px' }}>
                    <ButtonGroup>
                      <EditButton name="editNote" disabled={!note.id} onClick={ this.editNote.bind(this, note) }/>
                      <DeleteButton name="note" disabled={!note.id} onConfirm={ this.deleteNote.bind(this, note) }/>
                    </ButtonGroup>
                  </td>
                </tr>
              );
            })
          }
        </TableControl>
        {showNoNotesMessage && (
          <Alert bsStyle="success" style={{ marginTop: 10 }}>No notes</Alert>
        )}
        <Button title="Add Note" bsSize="small" onClick={ this.openNotesAddDialog }>
          <Glyphicon glyph="plus" />&nbsp;<strong>Add Note</strong>
        </Button>
        { this.state.showNotesAddDialog && (
            <NotesAddDialog
              show={ this.state.showNotesAddDialog }
              note={ this.state.note }
              onSave={ this.onNoteAdded }
              onUpdate={ this.onNoteUpdated }
              onClose={ this.closeNotesAddDialog }/>
        )}
      </ModalDialog>
    );
  },
});

export default NotesDialog;
