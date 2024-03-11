import React from "react";
import PropTypes from "prop-types";
import { FormGroup, FormLabel, FormText } from "react-bootstrap";
import { connect } from "react-redux";

import * as Api from "../../api";
import FormInputControl from "../../components/FormInputControl";
import CheckboxControl from "../../components/CheckboxControl";
import FormDialog from "../../components/FormDialog";
import { isBlank } from "../../utils/string";

class EditFavouritesDialog extends React.Component {
  static propTypes = {
    favourite: PropTypes.object.isRequired,
    onSave: PropTypes.func.isRequired,
    onClose: PropTypes.func.isRequired,
    show: PropTypes.bool,
  };

  constructor(props) {
    super(props);

    this.state = {
      isSaving: false,
      name: props.favourite.name || "",
      isDefault: props.favourite.isDefault || false,
      nameError: "",
    };
  }

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  didChange = () => {
    if (this.state.name !== this.props.favourite.name) {
      return true;
    }
    if (this.state.isDefault !== this.props.favourite.isDefault) {
      return true;
    }

    return false;
  };

  isValid = () => {
    if (isBlank(this.state.name)) {
      this.setState({ nameError: "Name is required" });
      return false;
    }
    return true;
  };

  onSubmit = async () => {
    if (this.isValid()) {
      if (this.didChange()) {
        this.setState({ isSaving: true });

        const favourite = {
          ...this.props.favourite,
          name: this.state.name,
          isDefault: this.state.isDefault,
        };

        try {
          if (favourite.id) {
            await this.props.dispatch(Api.updateFavourite(favourite));
          } else {
            await this.props.dispatch(Api.addFavourite(favourite));
          }
        } finally {
          this.setState({ isSaving: false });
          this.props.onSave(favourite);
        }
      }

      this.props.onClose();
    }
  };

  render() {
    const { isSaving, name, nameError, isDefault } = this.state;
    const { show, onClose } = this.props;

    return (
      <FormDialog
        id="edit-favourite"
        title="Favourite"
        size="sm"
        show={show}
        isSaving={isSaving}
        onClose={onClose}
        onSubmit={this.onSubmit}
      >
        <FormGroup controlId="name">
          <FormLabel>
            Name <sup>*</sup>
          </FormLabel>
          <FormInputControl
            type="text"
            readOnly={isSaving}
            defaultValue={name}
            updateState={this.updateState}
            autoFocus
            isInvalid={nameError}
          />
          <FormText>{nameError}</FormText>
        </FormGroup>
        <CheckboxControl id="isDefault" checked={isDefault} updateState={this.updateState} label="Default" />
      </FormDialog>
    );
  }
}

const mapDispatchToProps = (dispatch) => ({ dispatch });

export default connect(null, mapDispatchToProps)(EditFavouritesDialog);