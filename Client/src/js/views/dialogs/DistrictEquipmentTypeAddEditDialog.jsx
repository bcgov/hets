import React from 'react';

import { Form, FormGroup, HelpBlock, ControlLabel } from 'react-bootstrap';

import _ from 'lodash';

import EditDialog from '../../components/EditDialog.jsx';
import FilterDropdown from '../../components/FilterDropdown.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

const PROHIBITED_SECTIONS = [ 1.2, 1.8, 2.3, 2.6, 3.3, 6.3, 7.4, 8.2, 9.3, 11.2, 12.2, 13.5, 13.6, 16.3 ];

var DistrictEquipmentTypeAddEditDialog = React.createClass({
  propTypes: {
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
    districtEquipmentType: React.PropTypes.object,
    equipmentTypes: React.PropTypes.array,
    serviceAreas: React.PropTypes.array,
  },

  getInitialState() {
    return {
      isNew: this.props.districtEquipmentType.id === 0,

      id: this.props.districtEquipmentType.id || 0,
      equipmentTypeId: this.props.districtEquipmentType.equipmentType ? this.props.districtEquipmentType.equipmentType.id : undefined,
      districtEquipmentName: this.props.districtEquipmentType.districtEquipmentName || '',
      serviceAreaId: this.props.districtEquipmentType.serviceAreaId || undefined,
      concurrencyControlNumber: this.props.districtEquipmentType.concurrencyControlNumber || 0,
      equipmentTypeIdError: '',
      districtEquipmentNameError: '',
      serviceAreaIdError: '',
    };
  },

  updateState(state, callback) {
    this.setState(state, callback);
  },

  didChange() {
    if (this.state.isNew && this.state.equipmentTypeId !== undefined) { return true; }
    if (this.state.isNew && this.state.districtEquipmentName !== '') { return true; }
    if (this.state.isNew && this.state.serviceAreaId !== undefined) { return true; }
    if (!this.state.isNew && this.state.equipmentTypeId !== this.props.districtEquipmentType.equipmentType.id) { return true; }
    if (!this.state.isNew && this.state.districtEquipmentName !== this.props.districtEquipmentType.districtEquipmentName) { return true; }
    if (!this.state.isNew && this.state.serviceAreaId !== this.props.districtEquipmentType.serviceAreaId) { return true; }

    return false;
  },

  isValid() {
    this.setState({
      equipmentTypeIdError: '',
      districtEquipmentNameError: '',
      serviceAreaIdError: '',
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

    if (this.state.serviceAreaId === undefined) {
      this.setState({ serviceAreaIdError: 'Service area is required' });
      valid = false;
    }

    return valid;
  },

  onSave() {
    this.props.onSave({
      id: this.state.id,
      equipmentType: { id: this.state.equipmentTypeId },
      districtEquipmentName: this.state.districtEquipmentName,
      serviceAreaId: this.state.serviceAreaId,
      concurrencyControlNumber: this.state.concurrencyControlNumber,
    });
  },

  render() {
    return <EditDialog id="district-equipment-add" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title= {
        <strong>Add District Equipment Type</strong>
      }>
      <Form>
        <FormGroup controlId="equipmentTypeId" validationState={ this.state.equipmentTypeIdError ? 'error' : null }>
          <ControlLabel>Blue Book Section <sup>*</sup></ControlLabel>
          <FilterDropdown id="equipmentTypeId" fieldName="blueBookSectionAndName" selectedId={ this.state.equipmentTypeId } updateState={ this.updateState }
            items={ this.props.equipmentTypes }
            className="full-width"
          />
          <HelpBlock>{ this.state.equipmentTypeIdError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="districtEquipmentName" validationState={ this.state.districtEquipmentNameError ? 'error' : null }>
          <ControlLabel>Equipment Type/Description <sup>*</sup></ControlLabel>
          <FormInputControl type="text" value={ this.state.districtEquipmentName } updateState={ this.updateState }/>
          <HelpBlock>{ this.state.districtEquipmentNameError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="serviceAreaId" validationState={ this.state.serviceAreaIdError ? 'error' : null }>
          <ControlLabel>Service Area <sup>*</sup></ControlLabel>
          <FilterDropdown id="serviceAreaId" fieldName="id" selectedId={ this.state.serviceAreaId } updateState={ this.updateState }
            items={ this.props.serviceAreas }
            className="full-width"
          />
          <HelpBlock>{ this.state.serviceAreaIdError }</HelpBlock>
        </FormGroup>
      </Form>
    </EditDialog>;
  },
});

export default DistrictEquipmentTypeAddEditDialog;
