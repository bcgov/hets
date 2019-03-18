import React from 'react';
import { connect } from 'react-redux';
import { FormGroup, HelpBlock, ControlLabel } from 'react-bootstrap';
import _ from 'lodash';

import * as Api from '../../api';

import EditDialog from '../../components/EditDialog.jsx';
import FilterDropdown from '../../components/FilterDropdown.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';
import Form from '../../components/Form.jsx';


const PROHIBITED_SECTIONS = [ 1.2, 1.8, 2.3, 2.6, 3.3, 6.3, 7.4, 8.2, 9.3, 11.2, 12.2, 13.5, 13.6, 16.3 ];


class DistrictEquipmentTypeAddEditDialog extends React.Component {
  static propTypes = {
    show: React.PropTypes.bool,
    districtEquipmentType: React.PropTypes.object,
    equipmentTypes: React.PropTypes.object,
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
  };

  constructor(props) {
    super(props);

    this.state = {
      isNew: props.districtEquipmentType.id === 0,

      id: props.districtEquipmentType.id || 0,
      equipmentTypeId: props.districtEquipmentType.equipmentType ? props.districtEquipmentType.equipmentType.id : undefined,
      districtEquipmentName: props.districtEquipmentType.districtEquipmentName || '',
      concurrencyControlNumber: props.districtEquipmentType.concurrencyControlNumber || 0,
      equipmentTypeIdError: '',
      districtEquipmentNameError: '',
    };
  }

  componentDidMount() {
    Api.getEquipmentTypes();
  }

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  didChange = () => {
    if (this.state.isNew && this.state.equipmentTypeId !== undefined) { return true; }
    if (this.state.isNew && this.state.districtEquipmentName !== '') { return true; }
    if (!this.state.isNew && this.state.equipmentTypeId !== this.props.districtEquipmentType.equipmentType.id) { return true; }
    if (!this.state.isNew && this.state.districtEquipmentName !== this.props.districtEquipmentType.districtEquipmentName) { return true; }

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

  onSave = () => {
    this.props.onSave({
      id: this.state.id,
      equipmentType: { id: this.state.equipmentTypeId },
      districtEquipmentName: this.state.districtEquipmentName,
      concurrencyControlNumber: this.state.concurrencyControlNumber,
    });
  };

  render() {
    var equipmentTypes = _.sortBy(this.props.equipmentTypes.data, 'blueBookSection');

    return <EditDialog id="district-equipment-add" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title={<strong>Add District Equipment Type</strong>}>
      <Form>
        <FormGroup controlId="equipmentTypeId" validationState={ this.state.equipmentTypeIdError ? 'error' : null }>
          <ControlLabel>Blue Book Section <sup>*</sup></ControlLabel>
          <FilterDropdown
            id="equipmentTypeId"
            className="full-width"
            fieldName="blueBookSectionAndName"
            disabled={!this.props.equipmentTypes.loaded}
            selectedId={this.state.equipmentTypeId}
            items={equipmentTypes}
            updateState={this.updateState}
          />
          <HelpBlock>{ this.state.equipmentTypeIdError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="districtEquipmentName" validationState={ this.state.districtEquipmentNameError ? 'error' : null }>
          <ControlLabel>Equipment Type/Description <sup>*</sup></ControlLabel>
          <FormInputControl type="text" value={ this.state.districtEquipmentName } updateState={ this.updateState }/>
          <HelpBlock>{ this.state.districtEquipmentNameError }</HelpBlock>
        </FormGroup>
      </Form>
    </EditDialog>;
  }
}

function mapStateToProps(state) {
  return {
    equipmentTypes: state.lookups.equipmentTypes,
  };
}

export default connect(mapStateToProps)(DistrictEquipmentTypeAddEditDialog);
