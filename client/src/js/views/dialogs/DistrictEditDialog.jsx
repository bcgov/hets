import PropTypes from 'prop-types';
import React from 'react';
import _ from 'lodash';
import { FormGroup, FormText } from 'react-bootstrap';

import * as Api from '../../api';

import FormDialog from '../../components/FormDialog.jsx';
import CheckboxControl from '../../components/CheckboxControl.jsx';
import FilterDropdown from '../../components/FilterDropdown.jsx';

class DistrictEditDialog extends React.Component {
  static propTypes = {
    show: PropTypes.bool,
    districts: PropTypes.object,
    user: PropTypes.object,
    district: PropTypes.object,
    userDistricts: PropTypes.array,
    onSave: PropTypes.func,
    onClose: PropTypes.func.isRequired,
  };

  constructor(props) {
    super(props);

    var isNew = props.district.id === 0;

    this.state = {
      isNew: isNew,
      isSaving: false,

      districtId: isNew ? 0 : props.district.district.id,
      isPrimary: props.district.isPrimary || false,

      districtIdError: '',
    };
  }

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  didChange = () => {
    if (this.state.isNew && this.state.districtId !== '') {
      return true;
    }
    if (this.state.isNew && this.state.isPrimary !== '') {
      return true;
    }
    if (!this.state.isNew && this.state.districtId !== this.props.district.district.id) {
      return true;
    }
    if (!this.state.isNew && this.state.isPrimary !== this.props.district.isPrimary) {
      return true;
    }

    return false;
  };

  isValid = () => {
    this.setState({
      districtIdError: '',
    });

    var valid = true;

    if (this.state.districtId === 0) {
      this.setState({ districtIdError: 'District is required' });
      valid = false;
    }

    return valid;
  };

  formSubmitted = () => {
    if (this.isValid()) {
      if (this.didChange()) {
        this.setState({ isSaving: true });

        const district = {
          id: this.props.district.id,
          userId: this.props.user.id,
          districtId: this.state.districtId,
          isPrimary: this.state.isPrimary,
        };

        const promise = this.state.isNew ? Api.addUserDistrict(district) : Api.editUserDistrict(district);

        promise.then((response) => {
          this.setState({ isSaving: false });
          if (this.props.onSave) {
            this.props.onSave(response.data);
          }
          this.props.onClose();
        });
      } else {
        this.props.onClose();
      }
    }
  };

  render() {
    var userDistrictsFiltered = _.map(this.props.userDistricts, (district) => {
      if (this.state.isNew || district.district.id !== this.props.district.district.id) {
        return district.district.id;
      }
      return;
    });

    var districts = _.chain(this.props.districts)
      .sortBy('name')
      .reject((district) => {
        return _.includes(userDistrictsFiltered, district.id);
      })
      .value();

    return (
      <FormDialog
        id="equipment-add"
        show={this.props.show}
        title={this.state.isNew ? 'Add District' : 'Edit District'}
        size="sm"
        isSaving={this.state.isSaving}
        onClose={this.props.onClose}
        onSubmit={this.formSubmitted}
      >
        <FormGroup controlId="districtId">
          <FilterDropdown
            id="districtId"
            placeholder="Choose District"
            className="full-width"
            items={districts}
            selectedId={this.state.districtId}
            updateState={this.updateState}
            isInvalid={this.state.districtIdError}
          />
          <FormText>{this.state.districtIdError}</FormText>
        </FormGroup>
        <CheckboxControl
          id="isPrimary"
          checked={this.state.isPrimary}
          updateState={this.updateState}
          label="Primary District"
        />
      </FormDialog>
    );
  }
}

export default DistrictEditDialog;
