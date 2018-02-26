import React from 'react';

import { Form, FormGroup, HelpBlock, ControlLabel } from 'react-bootstrap';

import EditDialog from '../../components/EditDialog.jsx';
import FilterDropdown from '../../components/FilterDropdown.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

var DistrictEquipmentTypeAddEditDialog = React.createClass({
  propTypes: {
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
    districtEquipmentType: React.PropTypes.object,
    equipmentTypes: React.PropTypes.object,
  },

  getInitialState() {
    return {
      isNew: this.props.districtEquipmentType.id === 0,

      id: this.props.districtEquipmentType.id || 0,
      equipmentTypeId: this.props.districtEquipmentType.equipmentType ? this.props.districtEquipmentType.equipmentType.id : '',
      districtEquipmentName: this.props.districtEquipmentType.districtEquipmentName || '',
      equipmentTypeIdError: '',
      districtEquipmentNameError: '',
    };
  },

  updateState(state, callback) {
    this.setState(state, callback);
  },

  didChange() {
    if (this.state.isNew && this.state.equipmentTypeId !== '') { return true; }
    if (this.state.isNew && this.state.districtEquipmentName !== '') { return true; }
    if (!this.state.isNew && this.state.equipmentTypeId !== this.props.districtEquipmentType.equipmentType.id) { return true; }
    if (!this.state.isNew && this.state.districtEquipmentName !== this.props.districtEquipmentType.districtEquipmentName) { return true; }

    return false;
  },

  isValid() {
    this.setState({
      equipmentTypeIdError: '',
      districtEquipmentNameError: '',
    });

    var valid = true;

    if (this.state.equipmentTypeId === '') {
      this.setState({ equipmentTypeIdError: 'Equipment type is required' });
      valid = false;
    }

    if (this.state.districtEquipmentName === '') {
      this.setState({ districtEquipmentNameError: 'District equipment name is required' });
      valid = false;
    }

    return valid;
  },

  onSave() {
    this.props.onSave({
      id: this.state.id,
      equipmentType: { id: this.state.equipmentTypeId },
      districtEquipmentName: this.state.districtEquipmentName,
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
          <ControlLabel>Equipment Type <sup>*</sup></ControlLabel>
          <FilterDropdown id="equipmentTypeId" fieldName="name" selectedId={ this.state.equipmentTypeId } updateState={ this.updateState }
            items={ this.props.equipmentTypes }
            className="full-width"
          />
          <HelpBlock>{ this.state.equipmentTypeIdError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="districtEquipmentName" validationState={ this.state.districtEquipmentNameError ? 'error' : null }>
          <ControlLabel>District Equipment Name <sup>*</sup></ControlLabel>
          <FormInputControl type="text" value={ this.state.districtEquipmentName } updateState={ this.updateState }/>
          <HelpBlock>{ this.state.districtEquipmentNameError }</HelpBlock>
        </FormGroup>
      </Form>
    </EditDialog>;
  },
});

export default DistrictEquipmentTypeAddEditDialog;
