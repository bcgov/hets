import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { FormGroup, HelpBlock, ControlLabel } from 'react-bootstrap';
import _ from 'lodash';

import * as Api from '../../api';
import * as Constant from '../../constants';

import FormDialog from '../../components/FormDialog.jsx';
import FilterDropdown from '../../components/FilterDropdown.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { isBlank, notBlank } from '../../utils/string';
import { isValidYear } from '../../utils/date';


class EquipmentAddDialog extends React.Component {
  static propTypes = {
    currentUser: PropTypes.object.isRequired,
    owner: PropTypes.object.isRequired,
    localAreas: PropTypes.object.isRequired,
    districtEquipmentTypes: PropTypes.object.isRequired,
    onSave: PropTypes.func.isRequired,
    onClose: PropTypes.func.isRequired,
    show: PropTypes.bool,
  };

  constructor(props) {
    super(props);

    this.state = {
      isSaving: false,
      localAreaId: props.owner.localArea.id || 0,
      equipmentTypeId: 0,
      licencePlate: '',
      serialNumber: '',
      make: '',
      model: '',
      year: '',
      size: '',
      type: '',
      licencedGvw: '',
      legalCapacity: '',
      pupLegalCapacity: '',
      localAreaError: '',
      equipmentTypeError: '',
      serialNumberError: '',
      duplicateSerialNumberWarning: false,
      makeError: '',
      modelError: '',
      yearError: '',
    };
  }

  componentDidMount() {
    Api.getDistrictEquipmentTypes();
  }

  componentDidUpdate(prevProps, prevState) {
    if (!_.isEqual(this.state.serialNumber, prevState.serialNumber)) {
      this.setState({ duplicateSerialNumberWarning: false, serialNumberError: '' });
    }
  }

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  didChange = () => {
    if (this.state.localAreaId !== 0) { return true; }
    if (this.state.equipmentTypeId !== 0) { return true; }
    if (this.state.serialNumber !== '') { return true; }
    if (this.state.licencePlate !== '') { return true; }
    if (this.state.make !== '') { return true; }
    if (this.state.model !== '') { return true; }
    if (this.state.year !== '') { return true; }
    if (this.state.size !== '') { return true; }
    if (this.state.type !== '') { return true; }
    if (this.state.licencedGvw !== '') { return true; }
    if (this.state.legalCapacity !== '') { return true; }
    if (this.state.pupLegalCapacity !== '') { return true; }

    return false;
  };

  isValid = () => {
    this.setState({
      localAreaError: '',
      equipmentTypeError: '',
      serialNumberError: '',
      makeError: '',
      modelError: '',
      yearError: '',
    });

    var valid = true;

    if (this.state.localAreaId === 0) {
      this.setState({ localAreaError: 'Service area / local area is required.' });
      valid = false;
    }

    if (this.state.equipmentTypeId === 0) {
      this.setState({ equipmentTypeError: 'Equipment type is required.' });
      valid = false;
    }

    if (isBlank(this.state.make)) {
      this.setState({ makeError: 'Make is required.' });
      valid = false;
    }

    if (isBlank(this.state.model)) {
      this.setState({ modelError: 'Model is required.' });
      valid = false;
    }

    if (isBlank(this.state.year)) {
      this.setState({ yearError: 'Year is required.' });
      valid = false;
    } else if (notBlank(this.state.year) && !isValidYear(this.state.year)) {
      this.setState({ yearError: 'This is not a valid year.' });
      valid = false;
    }

    if (isBlank(this.state.serialNumber)) {
      this.setState({ serialNumberError: 'Serial number is required.' });
      valid = false;
    }

    return valid;
  };

  formSubmitted = () => {
    if (this.isValid()) {
      if (this.state.duplicateSerialNumberWarning) {
        // proceed regardless of duplicates
        this.setState({ duplicateSerialNumberWarning: false });
        return this.saveEquipment();
      }

      this.setState({ isSaving: true });

      return Api.equipmentDuplicateCheck(0, this.state.serialNumber).then((response) => {
        this.setState({ isSaving: false });

        if (response.data.length > 0) {
          const equipmentCodes = response.data.map((district) => {
            return district.duplicateEquipment.equipmentCode;
          });
          var districts = _.chain(response.data)
            .map(district => district.districtName)
            .uniq()
            .value();
          const districtsPlural = districts.length === 1 ? 'district' : 'districts';
          this.setState({
            serialNumberError: `Serial number is currently in use for the equipment ${equipmentCodes.join(', ')}, in the following ${districtsPlural}: ${districts.join(', ')}`,
            duplicateSerialNumberWarning: true,
          });
          return null;
        } else {
          this.setState({ duplicateSerialNumberWarning: false });
          return this.saveEquipment();
        }
      });
    }
  };

  saveEquipment = () => {
    if (this.didChange()) {
      this.setState({ isSaving: true });

      const equipment = {
        owner: { id: this.props.owner.id },
        localArea: { id: this.state.localAreaId },
        districtEquipmentType: { id: this.state.equipmentTypeId },
        licencePlate: this.state.licencePlate,
        serialNumber: this.state.serialNumber,
        make: this.state.make,
        model: this.state.model,
        year: this.state.year,
        size: this.state.size,
        type: this.state.type,
        licencedGvw: this.state.licencedGvw,
        legalCapacity: this.state.legalCapacity,
        pupLegalCapacity: this.state.pupLegalCapacity,
        status: Constant.EQUIPMENT_STATUS_CODE_PENDING,
      };

      return Api.addEquipment(equipment).then((savedEquipment) => {
        this.setState({ isSaving: false });
        this.props.onSave(savedEquipment);
      });
    }
  };

  render() {
    var owner = this.props.owner;

    var localAreas = _.sortBy(this.props.localAreas, 'name');

    var districtEquipmentTypes = _.chain(this.props.districtEquipmentTypes.data)
      .filter(type => type.district.id == this.props.currentUser.district.id)
      .sortBy('districtEquipmentName')
      .value();

    var equipment = _.find(districtEquipmentTypes, equipment => {
      return equipment.id == this.state.equipmentTypeId;
    });

    var isDumpTruck = equipment && equipment.equipmentType.isDumpTruck;

    return (
      <FormDialog
        id="equipment-add"
        show={this.props.show}
        title="Add Equipment"
        saveButtonLabel={this.state.duplicateSerialNumberWarning ? 'Proceed Anyways' : 'Save'}
        isSaving={this.state.isSaving}
        onSubmit={this.formSubmitted}
        onClose={this.props.onClose}>
        <FormGroup controlId="organizationName">
          <ControlLabel>Owner</ControlLabel>
          <h4>{ owner.organizationName }</h4>
        </FormGroup>
        <FormGroup controlId="localAreaId" validationState={ this.state.localAreaError ? 'error' : null }>
          <ControlLabel>Service Area - Local Area <sup>*</sup></ControlLabel>
          <FilterDropdown id="localAreaId" selectedId={ this.state.localAreaId } updateState={ this.updateState }
            items={ localAreas }
            className="full-width"
          />
          <HelpBlock>{ this.state.localAreaError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="equipmentTypeId" validationState={ this.state.equipmentTypeError ? 'error' : null }>
          <ControlLabel>Equipment Type <sup>*</sup></ControlLabel>
          <FilterDropdown
            id="equipmentTypeId"
            className="full-width"
            fieldName="districtEquipmentName"
            disabled={!this.props.districtEquipmentTypes.loaded}
            selectedId={this.state.equipmentTypeId}
            updateState={this.updateState}
            items={districtEquipmentTypes}/>
          <HelpBlock>{ this.state.equipmentTypeError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="make" validationState={ this.state.makeError ? 'error' : null }>
          <ControlLabel>Make <sup>*</sup></ControlLabel>
          <FormInputControl type="text" defaultValue={ this.state.make } updateState={ this.updateState } autoFocus/>
          <HelpBlock>{ this.state.makeError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="model" validationState={ this.state.modelError ? 'error' : null }>
          <ControlLabel>Model <sup>*</sup></ControlLabel>
          <FormInputControl type="text" defaultValue={ this.state.model } updateState={ this.updateState }/>
          <HelpBlock>{ this.state.modelError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="year" validationState={ this.state.yearError ? 'error' : null }>
          <ControlLabel>Year <sup>*</sup></ControlLabel>
          <FormInputControl type="text" defaultValue={ this.state.year } updateState={ this.updateState }/>
          <HelpBlock>{ this.state.yearError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="size">
          <ControlLabel>Size</ControlLabel>
          <FormInputControl type="text" defaultValue={ this.state.size } updateState={ this.updateState }/>
        </FormGroup>
        <FormGroup controlId="type">
          <ControlLabel>Type</ControlLabel>
          <FormInputControl type="text" defaultValue={ this.state.type } updateState={ this.updateState }/>
        </FormGroup>
        <FormGroup controlId="licencePlate">
          <ControlLabel>Licence Number</ControlLabel>
          <FormInputControl type="text" defaultValue={ this.state.licencePlate } updateState={ this.updateState }/>
        </FormGroup>
        <FormGroup controlId="serialNumber" validationState={ this.state.serialNumberError ? 'error' : null }>
          <ControlLabel>Serial Number <sup>*</sup></ControlLabel>
          <FormInputControl type="text" defaultValue={ this.state.serialNumber } updateState={ this.updateState }/>
          <HelpBlock>{ this.state.serialNumberError }</HelpBlock>
        </FormGroup>
        { isDumpTruck &&
          <div>
            <FormGroup controlId="licencedGvw">
              <ControlLabel>Licenced GVW</ControlLabel>
              <FormInputControl type="text" defaultValue={ this.state.licencedGvw } updateState={ this.updateState }/>
            </FormGroup>
            <FormGroup controlId="legalCapacity">
              <ControlLabel>Truck Legal Capacity</ControlLabel>
              <FormInputControl type="text" defaultValue={ this.state.legalCapacity } updateState={ this.updateState }/>
            </FormGroup>
            <FormGroup controlId="pupLegalCapacity">
              <ControlLabel>Pup Legal Capacity</ControlLabel>
              <FormInputControl type="text" defaultValue={ this.state.pupLegalCapacity } updateState={ this.updateState }/>
            </FormGroup>
          </div>
        }
      </FormDialog>
    );
  }
}

function mapStateToProps(state) {
  return {
    currentUser: state.user,
    localAreas: state.lookups.localAreas,
    districtEquipmentTypes: state.lookups.districtEquipmentTypes,
  };
}

export default connect(mapStateToProps)(EquipmentAddDialog);
