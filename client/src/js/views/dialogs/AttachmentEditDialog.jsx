import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';

import { FormGroup, FormText, FormLabel } from 'react-bootstrap';

import * as Api from '../../api';
import * as Log from '../../history';

import FormDialog from '../../components/FormDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

class AttachmentEditDialog extends React.Component {
  static propTypes = {
    onSave: PropTypes.func.isRequired,
    onClose: PropTypes.func.isRequired,
    show: PropTypes.bool,
    equipment: PropTypes.object.isRequired,
    attachment: PropTypes.object.isRequired,
  };

  constructor(props) {
    super(props);

    this.state = {
      isSaving: false,
      typeName: props.attachment.typeName,
      concurrencyControlNumber: props.attachment.concurrencyControlNumber || 0,
      attachmentError: '',
    };
  }

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  didChange = () => {
    if (this.state.typeName !== '') {
      return true;
    }

    return false;
  };

  isValid = () => {
    this.setState({
      attachmentError: '',
    });

    var valid = true;

    if (this.state.typeName === '') {
      this.setState({ attachmentError: 'Attachment is required' });
      valid = false;
    }

    return valid;
  };

  formSubmitted = async () => {
    if (this.isValid()) {
      if (this.didChange()) {
        this.setState({ isSaving: true });

        const dispatch = this.props.dispatch;
        const attachment = {
          id: this.props.attachment.id,
          typeName: this.state.typeName,
          concurrencyControlNumber: this.state.concurrencyControlNumber,
          equipment: { id: this.props.equipment.id },
        };

        await dispatch(Api.updatePhysicalAttachment(attachment));
        await dispatch(Log.equipmentAttachmentUpdated(this.props.equipment, attachment.typeName));
        this.setState({ isSaving: false });
        if (this.props.onSave) {
          this.props.onSave();
        }
        this.props.onClose();
      } else {
        this.props.onClose();
      }
    }
  };

  render() {
    return (
      <FormDialog
        id="attachment-edit"
        show={this.props.show}
        title="Edit Attachment"
        isSaving={this.state.isSaving}
        onClose={this.props.onClose}
        onSubmit={this.formSubmitted}
      >
        <FormGroup controlId="typeName">
          <FormLabel>Attachment</FormLabel>
          <FormInputControl
            type="text"
            defaultValue={this.state.typeName}
            updateState={this.updateState}
            isInvalid={this.state.attachmentError}
          />
          <FormText>{this.state.attachmentError}</FormText>
        </FormGroup>
      </FormDialog>
    );
  }
}

const mapDispatchToProps = (dispatch) => ({ dispatch });

export default connect(null, mapDispatchToProps)(AttachmentEditDialog);
