import PropTypes from 'prop-types';
import React from 'react';
import { FormGroup, FormLabel, FormText } from 'react-bootstrap';

import * as Api from '../../api';
import * as Constant from '../../constants';
import * as Log from '../../history';

import FormDialog from '../../components/FormDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { isBlank } from '../../utils/string';

class EquipmentChangeStatusDialog extends React.Component {
  static propTypes = {
    show: PropTypes.bool,
    status: PropTypes.string.isRequired,
    equipment: PropTypes.object.isRequired,
    onClose: PropTypes.func.isRequired,
    onStatusChanged: PropTypes.func.isRequired,
  };

  constructor(props) {
    super(props);

    this.state = {
      saving: false,
      comment: '',
      commentError: '',
    };
  }

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  isValid = () => {
    this.setState({
      commentError: '',
    });

    var valid = true;

    if (isBlank(this.state.comment)) {
      this.setState({ commentError: 'Comment is required' });
      valid = false;
    }

    return valid;
  };

  formSubmitted = () => {
    if (this.isValid()) {
      this.setState({ isSaving: true });
      const status = {
        id: this.props.equipment.id,
        status: this.props.status,
        statusComment: this.state.comment,
      };

      Api.changeEquipmentStatus(status)
        .then(() => {
          this.setState({ isSaving: false });
          this.props.onStatusChanged();
          Log.equipmentStatusModified(this.props.equipment, status.status, status.statusComment);
        })
        .catch((error) => {
          if (error.status === 400 && (error.errorCode === 'HETS-39' || error.errorCode === 'HETS-41')) {
            this.setState({ commentError: error.errorDescription });
          } else {
            throw error;
          }
        });
    }
  };

  render() {
    var maxLength = Constant.MAX_LENGTH_STATUS_COMMENT;

    return (
      <FormDialog
        id="notes"
        title="Reason for Status Change"
        show={this.props.show}
        isSaving={this.state.isSaving}
        onClose={this.props.onClose}
        onSubmit={this.formSubmitted}
      >
        <FormGroup controlId="comment">
          <FormLabel>Comment</FormLabel>
          <FormInputControl
            value={this.state.comment}
            as="textarea"
            updateState={this.updateState}
            maxLength={maxLength}
            isInvalid={this.state.commentError}
          />
          <FormText>{this.state.commentError}</FormText>
          <p>Maximum {maxLength} characters.</p>
        </FormGroup>
      </FormDialog>
    );
  }
}

export default EquipmentChangeStatusDialog;
