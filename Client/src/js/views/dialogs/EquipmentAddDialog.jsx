import React from 'react';

import { connect } from 'react-redux';

import { Form, FormGroup, HelpBlock, ControlLabel } from 'react-bootstrap';

import _ from 'lodash';

import * as Api from '../../api';
import * as Constant from '../../constants';

import EditDialog from '../../components/EditDialog.jsx';
import FilterDropdown from '../../components/FilterDropdown.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';
import Spinner from '../../components/Spinner.jsx';

import { isBlank } from '../../utils/string';

var EquipmentAddDialog = React.createClass({
  propTypes: {
    currentUser: React.PropTypes.object,
    owner: React.PropTypes.object.isRequired,
    localAreas: React.PropTypes.object,
    districtEquipmentTypes: React.PropTypes.object,
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
  },

  getInitialState() {
    return {
      loading: false,
      localAreaId: 0,
      equipmentTypeId: 0,
      serialNumber: '',
      make: '',
      model: '',
      year: '',
      localAreaError: '',
      equipmentTypeError: '',
      serialNumberError: '',
    };
  },

  componentDidMount() {
    this.setState({ loading: true });
    Api.getDistrictEquipmentTypes(this.props.currentUser.district.id).then(() => {
      this.setState({ loading: false });
      this.input.focus();
    });
  },

  updateState(state, callback) {
    this.setState(state, callback);
  },

  didChange() {
    if (this.state.localAreaId !== 0) { return true; }
    if (this.state.equipmentTypeId !== 0) { return true; }
    if (this.state.serialNumber !== '') { return true; }
    if (this.state.make !== '') { return true; }
    if (this.state.model !== '') { return true; }
    if (this.state.year !== '') { return true; }

    return false;
  },

  isValid() {
    this.setState({
      localAreaError: '',
      equipmentTypeError: '',
      serialNumberError: '',
    });

    var valid = true;

    if (this.state.localAreaId === 0) {
      this.setState({ localAreaError: 'Local area is required' });
      valid = false;
    }

    if (this.state.equipmentTypeId === 0) {
      this.setState({ equipmentTypeError: 'Equipment type is required' });
      valid = false;
    }

    if (isBlank(this.state.serialNumber)) {
      this.setState({ serialNumberError: 'Serial number is required' });
      valid = false;
    }

    return valid;
  },

  onSave() {
    this.props.onSave({
      owner: { id: this.props.owner.id },
      localArea: { id: this.state.localAreaId },
      districtEquipmentType: { id: this.state.equipmentTypeId },
      serialNumber: this.state.serialNumber,
      make: this.state.make,
      model: this.state.model,
      year: this.state.year,
      status: Constant.EQUIPMENT_STATUS_CODE_APPROVED,
    });
  },

  render() {
    if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

    var owner = this.props.owner;
    var localAreas = _.sortBy(this.props.localAreas, 'name');
    var districtEquipmentTypes = _.sortBy(this.props.districtEquipmentTypes, 'districtEquipmentName');

    return <EditDialog id="equipment-add" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title= {
        <strong>Add Equipment</strong>
      }>
      <Form>
        <FormGroup controlId="organizationName">
          <ControlLabel>Owner</ControlLabel>
          <h4>{ owner.organizationName }</h4>
        </FormGroup>
        <FormGroup controlId="equipmentTypeId" validationState={ this.state.equipmentTypeError ? 'error' : null }>
          <ControlLabel>Equipment Type <sup>*</sup></ControlLabel>
          <FilterDropdown id="equipmentTypeId" fieldName="name" selectedId={ this.state.equipmentTypeId } updateState={ this.updateState }
            items={ districtEquipmentTypes }
          />
          <HelpBlock>{ this.state.equipmentTypeError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="localAreaId" validationState={ this.state.localAreaError ? 'error' : null }>
          <ControlLabel>Local Area <sup>*</sup></ControlLabel>
          <FilterDropdown id="localAreaId" selectedId={ this.state.localAreaId } updateState={ this.updateState }
            items={ localAreas }
          />
          <HelpBlock>{ this.state.localAreaError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="serialNumber" validationState={ this.state.serialNumberError ? 'error' : null }>
          <ControlLabel>Serial Number <sup>*</sup></ControlLabel>
          <FormInputControl type="text" defaultValue={ this.state.serialNumber } updateState={ this.updateState } inputRef={ ref => { this.input = ref; }}/>
          <HelpBlock>{ this.state.serialNumberError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="make">
          <ControlLabel>Make</ControlLabel>
          <FormInputControl type="text" defaultValue={ this.state.make } updateState={ this.updateState }/>
        </FormGroup>
        <FormGroup controlId="model">
          <ControlLabel>Model</ControlLabel>
          <FormInputControl type="text" defaultValue={ this.state.model } updateState={ this.updateState }/>
        </FormGroup>
        <FormGroup controlId="year">
          <ControlLabel>Year</ControlLabel>
          <FormInputControl type="text" defaultValue={ this.state.year } updateState={ this.updateState }/>
        </FormGroup>
      </Form>
    </EditDialog>;
  },
});

function mapStateToProps(state) {
  return {
    currentUser: state.user,
    owner: state.models.owner,
    localAreas: state.lookups.localAreas,
    districtEquipmentTypes: state.lookups.districtEquipmentTypes,
  };
}

export default connect(mapStateToProps)(EquipmentAddDialog);
