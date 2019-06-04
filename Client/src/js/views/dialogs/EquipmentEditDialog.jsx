import PropTypes from 'prop-types';
import React from 'react';

import { connect } from 'react-redux';

import _ from 'lodash';

import { Grid, Row, Col, FormGroup, HelpBlock, ControlLabel } from 'react-bootstrap';

import * as Api from '../../api';
import * as Log from '../../history';

import FormDialog from '../../components/FormDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';
import FilterDropdown from '../../components/FilterDropdown.jsx';

import { isBlank, notBlank } from '../../utils/string';
import { isValidYear } from '../../utils/date';

class EquipmentEditDialog extends React.Component {
  static propTypes = {
    currentUser: PropTypes.object,
    equipment: PropTypes.object.isRequired,
    localAreas: PropTypes.object,
    districtEquipmentTypes: PropTypes.object,

    onSave: PropTypes.func,
    onClose: PropTypes.func.isRequired,
    show: PropTypes.bool,
  };

  constructor(props) {
    super(props);

    this.state = {
      isSaving: false,
      isNew: props.equipment.id === 0,

      localAreaId: props.equipment.localArea.id || 0,
      equipmentTypeId: props.equipment.districtEquipmentTypeId || null,
      serialNumber: props.equipment.serialNumber || '',
      make: props.equipment.make || '',
      size: props.equipment.size || '',
      model: props.equipment.model || '',
      year: props.equipment.year || '',
      licencePlate: props.equipment.licencePlate || '',
      type: props.equipment.type || '',
      licencedGvw: props.equipment.licencedGvw || '',
      legalCapacity: props.equipment.legalCapacity || '',
      pupLegalCapacity: props.equipment.pupLegalCapacity || '',

      serialNumberError: null,
      duplicateSerialNumberWarning: false,
      makeError: '',
      modelError: '',
      yearError: null,
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
    if (this.state.localAreaId !== this.props.equipment.localArea.id) { return true; }
    if (this.state.equipmentTypeId !== 0) { return true; }
    if (this.state.serialNumber !== this.props.equipment.serialNumber) { return true; }
    if (this.state.make !== this.props.equipment.make) { return true; }
    if (this.state.size !== this.props.equipment.size) { return true; }
    if (this.state.model !== this.props.equipment.model) { return true; }
    if (this.state.year !== this.props.equipment.year) { return true; }
    if (this.state.licencePlate !== this.props.equipment.licencePlate) { return true; }
    if (this.state.type !== this.props.equipment.type) { return true; }
    if (this.state.licencedGvw !== this.props.equipment.licencedGvw) { return true; }
    if (this.state.legalCapacity !== this.props.equipment.legalCapacity) { return true; }
    if (this.state.pupLegalCapacity !== this.props.equipment.pupLegalCapacity) { return true; }

    return false;
  };

  isValid = () => {
    this.setState({
      serialNumberError: null,
      makeError: '',
      modelError: '',
      yearError: '',
    });

    var valid = true;

    if (isBlank(this.state.serialNumber)) {
      this.setState({ serialNumberError: 'Serial number is required' });
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

    return valid;
  };

  formSubmitted = () => {
    if (this.isValid()) {
      if (this.didChange()) {
        if (this.state.duplicateSerialNumberWarning) {
          // proceed regardless of duplicates
          this.setState({ duplicateSerialNumberWarning: false });
          return this.saveEquipment();
        }

        return Api.equipmentDuplicateCheck(this.props.equipment.id, this.state.serialNumber).then((response) => {
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
      } else {
        this.props.onClose();
      }
    }
  };

  saveEquipment = () => {
    this.setState({ isSaving: true });

    const equipment = {
      ...this.props.equipment,
      localArea: { id: this.state.localAreaId },
      districtEquipmentTypeId: this.state.equipmentTypeId,
      serialNumber: this.state.serialNumber,
      make: this.state.make,
      size: this.state.size,
      model: this.state.model,
      year: this.state.year,
      licencePlate: this.state.licencePlate,
      type: this.state.type,
      licencedGvw: this.state.licencedGvw,
      legalCapacity: this.state.legalCapacity,
      pupLegalCapacity: this.state.pupLegalCapacity,
    };

    const promise = Api.updateEquipment(equipment);

    promise.then(() => {
      Log.equipmentModified(this.props.equipment);
      this.setState({ isSaving: false });
      if (this.props.onSave) { this.props.onSave(); }
      this.props.onClose();
    });
  };

  render() {
    var equipment = this.props.equipment;

    var localAreas = _.sortBy(this.props.localAreas, 'name');

    var districtEquipmentTypes = _.chain(this.props.districtEquipmentTypes.data)
      .filter(type => type.district.id == this.props.currentUser.district.id)
      .sortBy('districtEquipmentName')
      .value();

    return (
      <FormDialog
        id="equipment-edit"
        show={ this.props.show }
        title={ `Equipment Id: ${ equipment.equipmentCode }` }
        saveButtonLabel={ this.state.duplicateSerialNumberWarning ? 'Proceed Anyways' : 'Save' }
        isSaving={ this.state.isSaving }
        onClose={ this.props.onClose }
        onSubmit={ this.formSubmitted }>
        <Grid fluid>
          <Row>
            <Col md={12}>
              <FormGroup controlId="localAreaId" validationState={ this.state.localAreaError ? 'error' : null }>
                <ControlLabel>Service Area - Local Area</ControlLabel>
                <FilterDropdown id="localAreaId" selectedId={ this.state.localAreaId } updateState={ this.updateState }
                  items={ localAreas }
                  className="full-width"
                />
                <HelpBlock>{ this.state.localAreaError }</HelpBlock>
              </FormGroup>
            </Col>
          </Row>
          <Row>
            <Col md={12}>
              <FormGroup controlId="equipmentTypeId" validationState={ this.state.equipmentTypeError ? 'error' : null }>
                <ControlLabel>Equipment Type <sup>*</sup></ControlLabel>
                <FilterDropdown
                  id="equipmentTypeId"
                  className="full-width"
                  fieldName="districtEquipmentName"
                  disabled={!this.props.districtEquipmentTypes.loaded}
                  items={districtEquipmentTypes}
                  selectedId={this.state.equipmentTypeId}
                  updateState={this.updateState}/>
                <HelpBlock>{ this.state.equipmentTypeError }</HelpBlock>
              </FormGroup>
            </Col>
          </Row>
          <Row>
            <Col md={12}>
              <FormGroup controlId="make" validationState={ this.state.makeError ? 'error' : null }>
                <ControlLabel>Make <sup>*</sup></ControlLabel>
                <FormInputControl type="text" defaultValue={ this.state.make } updateState={ this.updateState } autoFocus/>
                <HelpBlock>{ this.state.makeError }</HelpBlock>
              </FormGroup>
            </Col>
          </Row>
          <Row>
            <Col md={12}>
              <FormGroup controlId="model" validationState={ this.state.modelError ? 'error' : null }>
                <ControlLabel>Model <sup>*</sup></ControlLabel>
                <FormInputControl type="text" defaultValue={ this.state.model } updateState={ this.updateState }/>
                <HelpBlock>{ this.state.modelError }</HelpBlock>
              </FormGroup>
            </Col>
          </Row>
          <Row>
            <Col md={12}>
              <FormGroup controlId="year" validationState={ this.state.yearError ? 'error' : null }>
                <ControlLabel>Year <sup>*</sup></ControlLabel>
                <FormInputControl type="text" defaultValue={ this.state.year } updateState={ this.updateState }/>
                <HelpBlock>{ this.state.yearError }</HelpBlock>
              </FormGroup>
            </Col>
          </Row>
          <Row>
            <Col md={12}>
              <FormGroup controlId="size">
                <ControlLabel>Size</ControlLabel>
                <FormInputControl type="text" defaultValue={ this.state.size } updateState={ this.updateState }/>
              </FormGroup>
            </Col>
          </Row>
          <Row>
            <Col md={12}>
              <FormGroup controlId="type">
                <ControlLabel>Type</ControlLabel>
                <FormInputControl type="text" defaultValue={ this.state.type } updateState={ this.updateState }/>
              </FormGroup>
            </Col>
          </Row>
          <Row>
            <Col md={12}>
              <FormGroup controlId="licencePlate">
                <ControlLabel>Licence Number</ControlLabel>
                <FormInputControl type="text" defaultValue={ this.state.licencePlate } updateState={ this.updateState }/>
              </FormGroup>
            </Col>
          </Row>
          <Row>
            <Col md={12}>
              <FormGroup controlId="serialNumber" validationState={ this.state.serialNumberError ? 'error' : null }>
                <ControlLabel>Serial Number <sup>*</sup></ControlLabel>
                <FormInputControl type="text" defaultValue={ this.state.serialNumber } updateState={ this.updateState } />
                <HelpBlock>{ this.state.serialNumberError }</HelpBlock>
              </FormGroup>
            </Col>
          </Row>
          { equipment.isDumpTruck &&
            <div>
              <Row>
                <Col md={12}>
                  <FormGroup controlId="licencedGvw">
                    <ControlLabel>Licenced GVW</ControlLabel>
                    <FormInputControl type="text" defaultValue={ this.state.licencedGvw } updateState={ this.updateState }/>
                  </FormGroup>
                </Col>
              </Row>
              <Row>
                <Col md={12}>
                  <FormGroup controlId="legalCapacity">
                    <ControlLabel>Truck Legal Capacity</ControlLabel>
                    <FormInputControl type="text" defaultValue={ this.state.legalCapacity } updateState={ this.updateState }/>
                  </FormGroup>
                </Col>
              </Row>
              <Row>
                <Col md={12}>
                  <FormGroup controlId="pupLegalCapacity">
                    <ControlLabel>Pup Legal Capacity</ControlLabel>
                    <FormInputControl type="text" defaultValue={ this.state.pupLegalCapacity } updateState={ this.updateState }/>
                  </FormGroup>
                </Col>
              </Row>
            </div>
          }
        </Grid>
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

export default connect(mapStateToProps)(EquipmentEditDialog);
