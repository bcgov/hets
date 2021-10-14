import PropTypes from 'prop-types';
import React from 'react';

import { FormGroup, FormText, FormLabel, Button } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

import * as Api from '../../api';
import * as Log from '../../history';

import FormDialog from '../../components/FormDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { isBlank } from '../../utils/string';

class AttachmentAddDialog extends React.Component {
  static propTypes = {
    onSave: PropTypes.func.isRequired,
    onClose: PropTypes.func.isRequired,
    show: PropTypes.bool,
    equipment: PropTypes.object.isRequired,
  };

  constructor(props) {
    super(props);

    this.state = {
      isSaving: false,
      forms: [
        {
          typeName: '',
          attachmentError: '',
        },
      ],
    };
  }

  updateState = (state, index) => {
    const forms = this.state.forms.slice();

    forms[index].typeName = state.typeName;

    this.setState({ forms });
  };

  didChange = () => {
    if (this.state.typeName !== '') {
      return true;
    }

    return false;
  };

  isValid = () => {
    const forms = this.state.forms.slice();

    var valid = false;

    forms.forEach((form) => {
      const formIsValid = !isBlank(form.typeName);

      form.attachmentError = formIsValid ? '' : 'Attachment is required';

      valid = formIsValid;
    });

    this.setState({ forms });

    return valid;
  };

  addInput = () => {
    if (this.state.forms.length < 10) {
      const forms = this.state.forms.slice(); // shallow clone

      forms.push({
        typeName: '',
        attachmentError: '',
      });

      this.setState({ forms });
    }
  };

  removeInput = () => {
    if (this.state.forms.length > 1) {
      let forms = this.state.forms.slice();
      forms.splice(forms.length - 1, 1);
      this.setState({ forms });
    }
  };

  formSubmitted = () => {
    if (this.isValid()) {
      if (this.didChange()) {
        this.setState({ isSaving: true });

        const attachmentTypeNames = this.state.forms.map((form) => form.typeName);

        const promise = Api.addPhysicalAttachments(this.props.equipment.id, attachmentTypeNames);

        promise.then(() => {
          attachmentTypeNames.forEach((typeName) => {
            Log.equipmentAttachmentAdded(this.props.equipment, typeName);
          });
          this.setState({ isSaving: false });
          if (this.props.onSave) {
            this.props.onSave();
          }
          this.props.onClose();
        });
      } else {
        this.props.onClose();
      }
    }
  };

  render() {
    const { forms } = this.state;

    return (
      <FormDialog
        id="attachment-add"
        show={this.props.show}
        title="Add Attachment"
        isSaving={this.state.isSaving}
        onClose={this.props.onClose}
        onSubmit={this.formSubmitted}
      >
        <div className="forms-container">
          {forms.map((form, i) => (
            <FormGroup key={i} controlId="typeName">
              <FormLabel>Attachment</FormLabel>
              <FormInputControl
                type="text"
                updateState={(state) => this.updateState(state, i)}
                isInvalid={form.attachmentError}
              />
              <FormText>{form.attachmentError}</FormText>
            </FormGroup>
          ))}
        </div>
        <div className="text-right">
          {forms.length > 1 && (
            <Button size="sm" className="mr-2 btn-custom" onClick={this.removeInput}>
              <FontAwesomeIcon icon="minus" />
              &nbsp;<strong>Remove</strong>
            </Button>
          )}
          {forms.length < 10 && (
            <Button size="sm" className="btn-custom" onClick={this.addInput}>
              <FontAwesomeIcon icon="plus" />
              &nbsp;<strong>Add</strong>
            </Button>
          )}
        </div>
      </FormDialog>
    );
  }
}

export default AttachmentAddDialog;
