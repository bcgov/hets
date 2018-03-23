
import React from 'react';

import { Form, Row, Col, FormGroup, ControlLabel, HelpBlock } from 'react-bootstrap';

import _ from 'lodash';

import EditDialog from '../../components/EditDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { isBlank } from '../../utils/string';

var ChangeEquipmentStatusDialog = React.createClass({
  propTypes: {
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
    status: React.PropTypes.string.isRequired,
    equipment: React.PropTypes.object.isRequired,
  },

  getInitialState() {
    return {
      comment: '',
      commentError: '',
    };
  },

  updateState(state, callback) {
    this.setState(state, callback);
  },

  didChange() {
    return true;
  },

  isValid() {
    this.setState({
      commentError: '',
    });

    var valid = true;

    if (isBlank(this.state.comment)) {
      this.setState({ commentError: 'Comment is required' });
      valid = false;
    }

    return valid; 
  },

  onSave() {
    this.props.onSave({
      id: this.props.owner.id,
      status: this.props.status,
      comment: this.state.comment,
    });
  },

  render() {
    return <EditDialog id="notes" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } isValid={ this.isValid } didChange={ this.didChange }
      title= {
        <strong>Reason for Status Change</strong>
      }>
      <Row>
        <Col md={12}>
          <Form>
            <FormGroup controlId="comment" validationState={ this.state.commentError ? 'error' : null }>
              <ControlLabel>Comment</ControlLabel>
              <FormInputControl value={ this.state.comment } componentClass="textarea" updateState={ this.updateState } /> 
              <HelpBlock>{ this.state.commentError }</HelpBlock>
            </FormGroup>
          </Form>
        </Col> 
      </Row>
    </EditDialog>;
  },
});

export default ChangeStatusDialog;