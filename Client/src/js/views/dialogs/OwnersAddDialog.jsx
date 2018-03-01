import React from 'react';

import { connect } from 'react-redux';

import { Form, FormGroup, HelpBlock, ControlLabel } from 'react-bootstrap';

import _ from 'lodash';

import * as Constant from '../../constants';

import CheckboxControl from '../../components/CheckboxControl.jsx';
import EditDialog from '../../components/EditDialog.jsx';
import FilterDropdown from '../../components/FilterDropdown.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { isBlank, notBlank, onlyLetters } from '../../utils/string';

const HELP_TEXT = {
  prefix: 'This field must include only letters, up to 5 characters and be unique across all owners',
  residency: 'You have not indicated the owner meets the Residency requirements of the HETS Program. If that is the case, the owner may not be registered in this local area until they have met this requirement. If this check was missed inadvertently, return and activate the checkbox. If the owner does not meet the residency requirement, return and cancel from the Add Owner popup.',
};

var OwnersAddDialog = React.createClass({
  propTypes: {
    currentUser: React.PropTypes.object,
    owners: React.PropTypes.object,
    localAreas: React.PropTypes.object,
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
  },

  getInitialState() {
    // Local Area (default to the first local area of the District of the logged in User, but allow any local area to be selected)
    var currentUser = this.props.currentUser;
    var localAreas = this.props.localAreas;
    var defaultLocalAreaId = _.find(localAreas, (x) => x.serviceArea.district.id === currentUser.district.id);

    return {
      name: '',
      address1: '',
      address2: '',
      city: '',
      province: '',
      postalCode: '',
      ownerCode: '',
      localAreaId: defaultLocalAreaId.id || 0,
      meetsResidency: true,
      doingBusinessAs: '',
      registeredCompanyNumber: '',

      nameError: '',
      address1Error: '',
      cityError: '',
      provinceError: '',
      postalCodeError: '',
      ownerCodeError: '',
      localAreaError: '',
      residencyError: '',
      statusError: '',
    };
  },

  componentDidMount() {
    this.input.focus();
  },

  updateState(state, callback) {
    this.setState(state, callback);
  },

  didChange() {
    return notBlank(this.state.name) ||
      notBlank(this.state.ownerCode) ||
      this.state.meetsResidency !== false ||
      this.state.localAreaId !== 0;
  },

  isValid() {
    // Clear out any previous errors
    var owner;
    var valid = true;

    this.setState({
      nameError: '',
      address1Error: '',
      cityError: '',
      provinceError: '',
      postalCodeError: '',
      ownerCodeError: '',
      localAreaError: '',
      residencyError: '',
      statusError: '',
    });

    if (isBlank(this.state.name)) {
      this.setState({ nameError: 'Name is required' });
      valid = false;
    } else {
      // Does the name already exist?
      var name = this.state.name.toLowerCase().trim();
      owner = _.find(this.props.owners, (owner) => {
        return owner.organizationName.toLowerCase().trim() === name;
      });
      if (owner) {
        this.setState({ nameError: 'This owner already exists in the system' });
        valid = false;
      }
    }

    if (isBlank(this.state.address1)) {
      this.setState({ address1Error: 'Address line 1 is required' });
      valid = false;
    }

    if (isBlank(this.state.city)) {
      this.setState({ cityError: 'City is required' });
      valid = false;
    }

    if (isBlank(this.state.province)) {
      this.setState({ provinceError: 'Province is required' });
      valid = false;
    }

    if (isBlank(this.state.postalCode)) {
      this.setState({ postalCodeError: 'Postal code is required' });
      valid = false;
    }

    if (isBlank(this.state.ownerCode)) {
      this.setState({ ownerCodeError: 'Equipment prefix is required' });
      valid = false;
    } else {
      var prefix = this.state.ownerCode.toLowerCase().trim();

      // Prefix must only include letters, up to 5 characters
      if (!onlyLetters(prefix) || prefix.length > 5) {
        this.setState({ ownerCodeError: 'This equipment prefix must only include letters, up to 5 characters' });
        valid = false;
      }

      // Prefix must be unique across all owners
      owner = _.find(this.props.owners, (owner) => {
        return owner.ownerEquipmentCodePrefix.toLowerCase().trim() === prefix;
      });
      if (owner) {
        this.setState({ ownerCodeError: 'This equipment prefix already exists in the system' });
        valid = false;
      }
    }

    if (this.state.localAreaId === 0) {
      this.setState({ localAreaError: 'Local area is required' });
      valid = false;
    }

    if (this.state.meetsResidency === false) {
      this.setState({ residencyError: HELP_TEXT.residency });
      valid = false;
    }

    return valid;
  },

  onSave() {
    this.props.onSave({
      organizationName: this.state.name,
      address1: this.state.address1,
      address2: this.state.address2,
      city: this.state.city,
      province: this.state.province,
      postalCode: this.state.postalCode,
      ownerCode: this.state.ownerCode,
      localArea: { id: this.state.localAreaId },
      meetsResidency: this.state.meetsResidency,
      status: Constant.OWNER_STATUS_CODE_PENDING,
      registeredCompanyNumber: this.state.registeredCompanyNumber,
      doingBusinessAs: this.state.doingBusinessAs,
    });
  },

  render() {
    // Constrain the local area drop downs to those in the District of the current logged in user
    var localAreas = _.chain(this.props.localAreas)
      .sortBy('name')
      .value();

    return <EditDialog id="add-owner" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title= {
        <strong>Add Owner</strong>
      }>
      <Form>
        <FormGroup controlId="name" validationState={ this.state.nameError ? 'error' : null }>
          <ControlLabel>Company Name <sup>*</sup></ControlLabel>
          <FormInputControl type="text" value={ this.state.name } updateState={ this.updateState } inputRef={ ref => { this.input = ref; }} />
          <HelpBlock>{ this.state.nameError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="address1" validationState={ this.state.address1Error ? 'error' : null }>
          <ControlLabel>Address Line 1 <sup>*</sup></ControlLabel>
          <FormInputControl type="text" value={ this.state.address1 } updateState={ this.updateState } inputRef={ ref => { this.input = ref; }} />
          <HelpBlock>{ this.state.address1Error }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="address2">
          <ControlLabel>Address Line 2</ControlLabel>
          <FormInputControl type="text" value={ this.state.address2 } updateState={ this.updateState } inputRef={ ref => { this.input = ref; }} />
        </FormGroup>
        <FormGroup controlId="city" validationState={ this.state.cityError ? 'error' : null }>
          <ControlLabel>City <sup>*</sup></ControlLabel>
          <FormInputControl type="text" value={ this.state.city } updateState={ this.updateState } inputRef={ ref => { this.input = ref; }} />
          <HelpBlock>{ this.state.cityError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="province" validationState={ this.state.provinceError ? 'error' : null }>
          <ControlLabel>Province <sup>*</sup></ControlLabel>
          <FormInputControl type="text" value={ this.state.province } updateState={ this.updateState } inputRef={ ref => { this.input = ref; }} />
          <HelpBlock>{ this.state.provinceError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="postalCode" validationState={ this.state.postalCodeError ? 'error' : null }>
          <ControlLabel>Postal Code <sup>*</sup></ControlLabel>
          <FormInputControl type="text" value={ this.state.postalCode } updateState={ this.updateState } inputRef={ ref => { this.input = ref; }} />
          <HelpBlock>{ this.state.postalCodeError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="ownerCode" validationState={ this.state.ownerCodeError ? 'error' : null }>
          <ControlLabel>Owner Code <sup>*</sup></ControlLabel>
          <FormInputControl type="text" value={ this.state.ownerCode } updateState={ this.updateState } />
          <HelpBlock>{ this.state.ownerCodeError || HELP_TEXT.prefix }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="localAreaId" validationState={ this.state.localAreaError ? 'error' : null }>
          <ControlLabel>Local Area <sup>*</sup></ControlLabel>
          <FilterDropdown id="localAreaId" items={ localAreas } selectedId={ this.state.localAreaId } updateState={ this.updateState } className="full-width" />
          <HelpBlock>{ this.state.localAreaError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="doingBusinessAs">
          <ControlLabel>Doing Business As</ControlLabel>
          <FormInputControl type="text" value={ this.state.doingBusinessAs } updateState={ this.updateState } />
        </FormGroup>
        <FormGroup controlId="registeredCompanyNumber">
          <ControlLabel>Registered BC Company Number</ControlLabel>
          <FormInputControl type="text" value={ this.state.registeredCompanyNumber } updateState={ this.updateState } />
        </FormGroup>
        <FormGroup controlId="meetsResidency" validationState={ this.state.residencyError ? 'error' : null }>
          <CheckboxControl id="meetsResidency" checked={ this.state.meetsResidency } updateState={ this.updateState }>Meets Residency</CheckboxControl>
          <HelpBlock>{ this.state.residencyError }</HelpBlock>
        </FormGroup>
      </Form>
    </EditDialog>;
  },
});

function mapStateToProps(state) {
  return {
    currentUser: state.user,
    owners: state.lookups.owners.data,
    localAreas: state.lookups.localAreas,
  };
}

export default connect(mapStateToProps)(OwnersAddDialog);
