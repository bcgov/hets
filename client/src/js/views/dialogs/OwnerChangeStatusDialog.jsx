import PropTypes from 'prop-types';
import React from 'react';
import { FormGroup, FormLabel, FormText } from 'react-bootstrap';
import _ from 'lodash';

import * as Api from '../../api';
import * as Constant from '../../constants';
import * as Log from '../../history';

import FormDialog from '../../components/FormDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { isBlank } from '../../utils/string';
import { OWNER_STATUS_CODE_APPROVED } from '../../constants';

const ARCHIVE_WARNING_MESSAGE =
  'This action will archive the owner and all their equipment and remove them from the seniority list.';

class ChangeStatusDialog extends React.Component {
  static propTypes = {
    show: PropTypes.bool,
    status: PropTypes.string.isRequired,
    owner: PropTypes.object.isRequired,
    onClose: PropTypes.func.isRequired,
    onStatusChanged: PropTypes.func.isRequired,
  };

  constructor(props) {
    super(props);

    this.state = {
      comment: '',
      commentError: '',
      statusError: '',
      archiveWarning: props.status === Constant.OWNER_STATUS_CODE_ARCHIVED ? ARCHIVE_WARNING_MESSAGE : '',
    };
  }

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  didChange = () => {
    return true;
  };

  isValid = () => {
    this.setState({
      commentError: '',
      statusError: '',
    });

    var valid = true;

    if (isBlank(this.state.comment)) {
      this.setState({ commentError: 'Comment is required' });
      valid = false;
    }

    if (this.props.owner && this.props.status === OWNER_STATUS_CODE_APPROVED && this.statusRequirements().length > 0) {
      this.setState({ statusError: this.statusRequirements() });
      valid = false;
    }

    return valid;
  };

  statusRequirements = () => {
    const { owner } = this.props;
    var requirements = [];

    if (!owner.primaryContact) {
      requirements.push('Primary contact');
    }
    if (isBlank(owner.workSafeBCPolicyNumber)) {
      requirements.push('WorkSafeBC policy number');
    }
    if (!owner.address1 || !owner.city || !owner.province || !owner.province) {
      requirements.push('Company address');
    }
    if (!owner.meetsResidency) {
      requirements.push('Meets residency');
    }

    return requirements;
  };

  formSubmitted = () => {
    if (this.isValid()) {
      this.setState({ isSaving: true });
      const status = {
        id: this.props.owner.id,
        status: this.props.status,
        statusComment: this.state.comment,
      };
      var currentStatus = this.props.owner.status;
      var equipmentList = { ...this.props.owner.equipmentList };

      return Api.changeOwnerStatus(status)
        .then(() => {
          this.setState({ isSaving: false });
          this.props.onStatusChanged(status);
          Log.ownerModifiedStatus(this.props.owner, status.status, status.statusComment);
          // If owner status goes from approved to unapproved/archived or unapproved to archived
          // this will change all it's equipment statuses. This should be reflected in the equipment history.
          if (
            (currentStatus === Constant.OWNER_STATUS_CODE_APPROVED ||
              currentStatus === Constant.OWNER_STATUS_CODE_PENDING) &&
            (status.status === Constant.OWNER_STATUS_CODE_PENDING ||
              status.status === Constant.OWNER_STATUS_CODE_ARCHIVED)
          ) {
            _.map(equipmentList, (equipment) => {
              if (equipment.status !== status.status) {
                Log.equipmentStatusModified(equipment, status.status, status.statusComment);
              }
            });
          }
        })
        .catch((error) => {
          if (error.status === 400 && error.errorCode === 'HETS-40') {
            this.setState({ commentError: error.errorDescription });
          } else {
            throw error;
          }
        });
    }
  };

  render() {
    var statusErrorText =
      this.state.statusError && this.state.statusError.length <= 1
        ? 'The following is also required:'
        : 'The following are also required:';
    var maxLength = Constant.MAX_LENGTH_STATUS_COMMENT;

    const archiving = this.props.status === Constant.OWNER_STATUS_CODE_ARCHIVED;

    return (
      <FormDialog
        id="notes"
        title="Reason for Status Change"
        saveButtonLabel={archiving ? 'Proceed Anyways' : 'Save'}
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
          <FormText>
            {this.state.statusError && statusErrorText}
            <ul>
              {_.map(this.state.statusError, (error) => {
                return <li>{error}</li>;
              })}
            </ul>
          </FormText>
          {archiving && (
            <div className="has-error">
              <FormText>{this.state.archiveWarning}</FormText>
            </div>
          )}
        </FormGroup>
      </FormDialog>
    );
  }
}

export default ChangeStatusDialog;
