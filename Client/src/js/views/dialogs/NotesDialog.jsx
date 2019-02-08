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
    onSave: React.PropTypes.func.isRequired,
    id: React.PropTypes.string.isRequired,
    // Api function to call when updating a note
    onUpdate: React.PropTypes.func.isRequired,
    // Api call to get notes for particular entity
    getNotes: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
    notes: React.PropTypes.object,
  },

  getInitialState() {
    return {
      note: {},
    };
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

  onSave(note) {
    this.props.onSave(this.props.id, note).then(() => {
      this.closeNotesAddDialog();
    });
  },

  onUpdate(note) {
    this.props.onUpdate(note).then(() => {
      this.props.getNotes(this.props.id);
      this.closeNotesAddDialog();
    });
  },

  deleteNote(note) {
    Api.deleteNote(note.id).then(() => {
      this.props.getNotes(this.props.id);
    });
  },

  editNote(note) {
    this.setState({
      note: note,
      showNotesAddDialog: true,
    });
  },

  render() {

    var headers = [
      { field: 'note',            title: 'Note'  },
      { field: 'blank'                           },
    ];

    return (
      <ModalDialog id="notes" show={ this.props.show }
        onClose={ this.props.onClose }
        title= {
          <strong>Notes</strong>
        }>
        <TableControl id="notes-list" headers={ headers }>
          {
            _.map(this.props.notes, (note) => {
              return <tr key={ note.id }>
                <td>{ note.text }</td>
                <td style={{ textAlign: 'right', minWidth: '60px' }}>
                  <ButtonGroup>
                    <EditButton name="editNote" onClick={ this.editNote.bind(this, note) }/>
                    <DeleteButton name="note" onConfirm={ this.deleteNote.bind(this, note) }/>
                  </ButtonGroup>
                </td>
              </tr>;
            })
          }
        </TableControl>
        {(() => {
          if (!this.props.notes || Object.keys(this.props.notes).length === 0) { return <Alert bsStyle="success" style={{ marginTop: 10 }}>No notes</Alert>; }
        })()}
        <Button title="Add Note" bsSize="small" onClick={ this.openNotesAddDialog }><Glyphicon glyph="plus" />&nbsp;<strong>Add Note</strong></Button>
        { this.state.showNotesAddDialog &&
          <NotesAddDialog
            show={ this.state.showNotesAddDialog }
            onSave={ this.onSave }
            onUpdate={ this.onUpdate }
            onClose={ this.closeNotesAddDialog }
            notes={ this.props.notes }
            note={ this.state.note }
          />
        }
      </ModalDialog>
    );
  },
});

export default NotesDialog;