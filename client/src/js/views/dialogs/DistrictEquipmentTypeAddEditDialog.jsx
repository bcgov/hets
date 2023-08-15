import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { FormGroup, FormText, FormLabel } from 'react-bootstrap';
import _ from 'lodash';

import * as Api from '../../api';

import FormDialog from '../../components/FormDialog.jsx';
import FilterDropdown from '../../components/FilterDropdown.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

const PROHIBITED_SECTIONS = [1.2, 1.8, 2.3, 2.6, 3.3, 6.3, 7.4, 8.2, 9.3, 11.2, 12.2, 13.5, 13.6, 16.3];

class DistrictEquipmentTypeAddEditDialog extends React.Component {
  static propTypes = {
    currentUser: PropTypes.object,
    show: PropTypes.bool,
    districtEquipmentType: PropTypes.object,
    equipmentTypes: PropTypes.object,
    onSave: PropTypes.func.isRequired,
    onClose: PropTypes.func.isRequired,
  };

  constructor(props) {
    super(props);

    this.state = {
      isSaving: false,
      isNew: props.districtEquipmentType.id === 0,

      id: props.districtEquipmentType.id || 0,
      equipmentTypeId: props.districtEquipmentType.equipmentType
        ? props.districtEquipmentType.equipmentType.id
        : undefined,
      districtEquipmentName: props.districtEquipmentType.districtEquipmentName || '',
      concurrencyControlNumber: props.districtEquipmentType.concurrencyControlNumber || 0,
      equipmentTypeIdError: '',
      districtEquipmentNameError: '',
    };
  }

  componentDidMount() {
    this.props.dispatch(Api.getEquipmentTypes());
  }

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  didChange = () => {
    if (this.state.isNew && this.state.equipmentTypeId !== undefined) {
      return true;
    }
    if (this.state.isNew && this.state.districtEquipmentName !== '') {
      return true;
    }
    if (!this.state.isNew && this.state.equipmentTypeId !== this.props.districtEquipmentType.equipmentType.id) {
      return true;
    }
    if (
      !this.state.isNew &&
      this.state.districtEquipmentName !== this.props.districtEquipmentType.districtEquipmentName
    ) {
      return true;
    }

    return false;
  };

  isValid = () => {
    this.setState({
      equipmentTypeIdError: '',
      districtEquipmentNameError: '',
    });

    var valid = true;

    if (this.state.equipmentTypeId === undefined) {
      this.setState({ equipmentTypeIdError: 'Blue book section is required' });
      valid = false;
    } else {
      var equipmentType = _.find(this.props.equipmentTypes, { id: this.state.equipmentTypeId });
      if (equipmentType && _.includes(PROHIBITED_SECTIONS, equipmentType.blueBookSection)) {
        this.setState({ equipmentTypeIdError: 'Equipment types cannot be created for this blue book section' });
        valid = false;
      }
    }

    if (this.state.districtEquipmentName === '') {
      this.setState({ districtEquipmentNameError: 'Equipment type/description is required' });
      valid = false;
    }

    return valid;
  };

  formSubmitted = async () => {
    if (this.isValid()) {
      if (this.didChange()) {
        this.setState({ isSaving: true });

        const equipmentType = {
          id: this.state.id,
          equipmentType: { id: this.state.equipmentTypeId },
          districtEquipmentName: this.state.districtEquipmentName,
          concurrencyControlNumber: this.state.concurrencyControlNumber,
          district: { id: this.props.currentUser.district.id },
        };

        const promise = equipmentType.id !== 0 ? 
          Api.updateDistrictEquipmentType : 
          Api.addDistrictEquipmentType;

        await this.props.dispatch(promise(equipmentType));
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
    let equipmentTypes = _.sortBy(this.props.equipmentTypes.data, 'blueBookSection');

    return (
      <FormDialog
        id="district-equipment-add"
        show={this.props.show}
        title={this.state.isNew ? 'Add District Equipment Type' : 'Edit District Equipment Type'}
        isSaving={this.state.isSaving}
        onClose={this.props.onClose}
        onSubmit={this.formSubmitted}
      >
        <FormGroup controlId="equipmentTypeId">
          <FormLabel>
            Blue Book Section <sup>*</sup>
          </FormLabel>
          <FilterDropdown
            id="equipmentTypeId"
            className="full-width"
            fieldName="blueBookSectionAndName"
            disabled={!this.props.equipmentTypes.loaded}
            selectedId={this.state.equipmentTypeId}
            items={equipmentTypes}
            updateState={this.updateState}
            isInvalid={this.state.equipmentTypeIdError}
          />
          <FormText>{this.state.equipmentTypeIdError}</FormText>
        </FormGroup>
        <FormGroup controlId="districtEquipmentName">
          <FormLabel>
            Equipment Type/Description <sup>*</sup>
          </FormLabel>
          <FormInputControl
            type="text"
            value={this.state.districtEquipmentName}
            updateState={this.updateState}
            isInvalid={this.state.districtEquipmentNameError}
          />
          <FormText>{this.state.districtEquipmentNameError}</FormText>
        </FormGroup>
      </FormDialog>
    );
  }
}

const mapStateToProps = (state) => ({
  currentUser: state.user,
  equipmentTypes: state.lookups.equipmentTypes,
});

const mapDispatchToProps = (dispatch) => ({ dispatch });

export default connect(mapStateToProps, mapDispatchToProps)(DistrictEquipmentTypeAddEditDialog);
