import React from 'react';

import { connect } from 'react-redux';

import { Form, Row, Col, FormGroup, ControlLabel, HelpBlock, ButtonGroup } from 'react-bootstrap';

import EditDialog from '../../components/EditDialog.jsx';
import Spinner from '../../components/Spinner.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';
import TableControl from '../../components/TableControl.jsx';
import DeleteButton from '../../components/DeleteButton.jsx';
import Unimplemented from '../../components/Unimplemented.jsx';

var NotesDialog = React.createClass({
  propTypes: {
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
    notes: React.PropTypes.object,
  },

  getInitialState() {
    return {
      loading: false,
      note: '',
      noteError: '',
      isNoLongerRelevant: false,
    };
  },

  updateState(state, callback) {
    this.setState(state, callback);
  },

  didChange() {
    if (this.state.note !== '') { return true; }

    return false;
  },

  isValid() {
    this.setState({
      noteError: '',
    });

    var valid = true;

    if (this.state.note === '') {
      this.setState({ noteError: 'Note is required' });
      valid = false;
    }

    return valid;
  },

  onSave() {
    this.props.onSave({
      id: 0,
      text: this.state.note,
      isNoLongerRelevant: false,
    });
  },

  deleteNote(note) {

  },

  render() {
    if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

    return <EditDialog id="notes" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } isValid={ this.isValid } didChange={ this.didChange }
      title= {
        <strong>Notes</strong>
      }>
      { 
        _.map(this.props.notes, (note) => {
          return (
            <Row key={note.id}>
              <Col md={12}>{note.text}</Col>
            </Row>
          );
        })
      } 
      <Row>
         <Col md={12}>
          <Form>
            <FormGroup controlId="note" validationState={ this.state.noteError ? 'error' : null }>
              <ControlLabel>Note</ControlLabel>
              <FormInputControl componentClass="textarea" updateState={ this.updateState } /> 
              <HelpBlock>{ this.state.noteError }</HelpBlock>
            </FormGroup>
          </Form>
        </Col> 
      </Row>
      <Row>
      {(()=> {
        var headers = [
          { field: 'date',             title: 'Date'    },
          { field: 'user',             title: 'User'    },
          { field: 'notes',            title: 'Notes'  },
          { field: 'blank'                              },
        ];

        return <TableControl id="notes-list" headers={ headers }>
          {
            _.map(this.props.notes, (note) => {
              return <tr key={ note.id }>
                <td></td>
                <td></td>
                <td>{ note.text }</td>
                <td style={{ textAlign: 'right' }}>
                  <Unimplemented>
                    <ButtonGroup>
                      <DeleteButton name="note" onConfirm={ this.deleteNote.bind(this, note) }/>
                    </ButtonGroup>
                  </Unimplemented>
                </td>
              </tr>;
            })
          }
        </TableControl>;
      })()}
      </Row>
    </EditDialog>;
  },
});

function mapStateToProps(state) {
  return {
    notes: state.models.notes,
  };
}

export default connect(mapStateToProps)(NotesDialog);