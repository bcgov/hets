import React from 'react';

import { connect } from 'react-redux';

import { Form, Row, Col, FormGroup, ControlLabel, HelpBlock } from 'react-bootstrap';

import _ from 'lodash';
// import Promise from 'bluebird';

import * as Api from '../../api';
// import * as Constant from '../../constants';

import EditDialog from '../../components/EditDialog.jsx';
import Spinner from '../../components/Spinner.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

var NotesDialog = React.createClass({
  propTypes: {
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
    owner: React.PropTypes.obj,
  },

  getInitialState() {
    return {
      loading: false,
      note: '',
      noteError: '',
    };
  },

  componentDidMount() {
    this.setState({ loading: true });
    Api.getNotes().then(() => {
      this.setState({ loading: false });
    });
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
      note: this.state.note,
    });
  },

  render() {
    var owner = this.props.owner;

    if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

    return <EditDialog id="notes" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } isValid={ this.isValid } didChange={ this.didChange }
      title= {
        <strong>Notes</strong>
      }>
      { 
        _.map(owner.notes, (note) => {
          return (
            <Row>
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
    </EditDialog>;
  },
});

function mapStateToProps(state) {
  return {
    owner: state.models.owner,
  };
}

export default connect(mapStateToProps)(NotesDialog);