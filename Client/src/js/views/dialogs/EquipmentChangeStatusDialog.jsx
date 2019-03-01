import React from 'react';
import { FormGroup, ControlLabel, HelpBlock } from 'react-bootstrap';

import * as Api from '../../api';
import * as Log from '../../history';

import FormDialog from '../../components/FormDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { isBlank } from '../../utils/string';


var EquipmentChangeStatusDialog = React.createClass({
  propTypes: {
    show: React.PropTypes.bool,
    status: React.PropTypes.string.isRequired,
    equipment: React.PropTypes.object.isRequired,
    onClose: React.PropTypes.func.isRequired,
    onStatusChanged: React.PropTypes.func.isRequired,
  },

  getInitialState() {
    return {
      saving: false,
      comment: '',
      commentError: '',
    };
  },

  updateState(state, callback) {
    this.setState(state, callback);
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

    return valid;
  },

  formSubmitted() {
    if (this.isValid()) {
      this.setState({isSaving: true});
      const status = {
        id: this.props.equipment.id,
        status: this.props.status,
        statusComment: this.state.comment,
      };

      Api.changeEquipmentStatus(status).then(() => {
        this.setState({isSaving: false});
        this.props.onStatusChanged();
        Log.equipmentStatusModified(this.props.equipment, status.status, status.statusComment);
      }).catch((err) => {
        if (err.status === 400 && err.errorCode === 'HETS-39') {
          this.setState({ commentError: err.errorDescription });
        } else {
          throw err;
        }
      });
    }
  },

  render() {
    return (
      <FormDialog
        id="notes"
        title="Reason for Status Change"
        show={this.props.show}
        isSaving={this.state.isSaving}
        onClose={this.props.onClose}
        onSubmit={this.formSubmitted}>
        <FormGroup controlId="comment" validationState={this.state.commentError ? 'error' : null}>
          <ControlLabel>Comment</ControlLabel>
          <FormInputControl value={this.state.comment} componentClass="textarea" updateState={this.updateState} />
          <HelpBlock>{this.state.commentError}</HelpBlock>
        </FormGroup>
      </FormDialog>
    );
  },
});

export default EquipmentChangeStatusDialog;
