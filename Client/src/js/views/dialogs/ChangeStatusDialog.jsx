import React from 'react';

import { Form, Row, Col, FormGroup, ControlLabel, HelpBlock } from 'react-bootstrap';

import _ from 'lodash';

import EditDialog from '../../components/EditDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { isBlank } from '../../utils/string';
import { OWNER_STATUS_CODE_APPROVED } from '../../constants';

var ChangeStatusDialog = React.createClass({
  propTypes: {
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
    status: React.PropTypes.string.isRequired,
    parent: React.PropTypes.object.isRequired,
    owner: React.PropTypes.bool,
  },

  getInitialState() {
    return {
      comment: '',
      commentError: '',
      statusError: '',
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
      statusError: '',
    });

    var valid = true;

    if (isBlank(this.state.comment)) {
      this.setState({ commentError: 'Comment is required' });
      valid = false;
    }

    if (this.props.owner && this.props.status === OWNER_STATUS_CODE_APPROVED && (this.statusRequirements()).length > 0) {
      this.setState({ statusError: this.statusRequirements() });
      valid = false;
    }


    return valid;
  },

  statusRequirements() {
    var parent = this.props.parent;
    var requirements = [];

    if (!parent.primaryContact) {
      requirements.push('Primary contact');
    }
    if (isBlank(parent.workSafeBCPolicyNumber)) {
      requirements.push('WorkSafeBC policy number');
    }
    if (!parent.address1 || !parent.city || !parent.province || !parent.province) {
      requirements.push('Company address');
    }
    if (!parent.meetsResidency) {
      requirements.push('Meets residency');
    }

    return requirements;
  },

  onSave() {
    this.props.onSave({
      id: this.props.parent.id,
      status: this.props.status,
      statusComment: this.state.comment,
    }).catch((err) => {
      this.setState({ commentError: err.json.error.description });
    });
  },

  render() {
    var statusErrorText = this.state.statusError && this.state.statusError.length <= 1 ? 'The following is also required:' : 'The following are also required:';

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
              { this.props.owner &&
              <HelpBlock>{ this.state.statusError && statusErrorText }
                <ul>
                  {
                    _.map(this.state.statusError, (error) => {
                      return <li>{ error }</li>;
                    })
                  }
                </ul>
              </HelpBlock>
              }
            </FormGroup>
          </Form>
        </Col>
      </Row>
    </EditDialog>;
  },
});

export default ChangeStatusDialog;
