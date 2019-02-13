import React from 'react';

import { connect } from 'react-redux';

import { FormGroup, HelpBlock, ControlLabel } from 'react-bootstrap';

import _ from 'lodash';

import * as Constant from '../../constants';

import CheckboxControl from '../../components/CheckboxControl.jsx';
import DropdownControl from '../../components/DropdownControl.jsx';
import EditDialog from '../../components/EditDialog.jsx';
import FilterDropdown from '../../components/FilterDropdown.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';
import Form from '../../components/Form.jsx';

import { isBlank, onlyLetters } from '../../utils/string';

const HELP_TEXT = {
  prefix: 'This field must include only letters, up to 7 characters and be unique across all owners',
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
    // var currentUser = this.props.currentUser;
    // var localAreas = this.props.localAreas;
    // var defaultLocalAreaId = _.find(localAreas, (x) => x.serviceArea.district.id === currentUser.district.id);

    return {
      name: '',
      doingBusinessAs: '',
      givenName: '',
      surname: '',
      address1: '',
      address2: '',
      city: '',
      province: 'BC',
      postalCode: '',
      ownerCode: '',
      // localAreaId: defaultLocalAreaId.id || 0,
      localAreaId: 0,
      isMaintenanceContractor: false,
      meetsResidency: true,
      registeredCompanyNumber: '',
      workSafeBCPolicyNumber: '',
      primaryContactPhone: '',
      primaryContactGivenName: '',
      primaryContactSurname: '',
      primaryContactRole: '',
      status: Constant.OWNER_STATUS_CODE_APPROVED,

      nameError: '',
      ownerGivenNameError: '',
      ownerSurameError: '',
      address1Error: '',
      cityError: '',
      provinceError: '',
      postalCodeError: '',
      ownerCodeError: '',
      localAreaError: '',
      residencyError: '',
      workSafeBCPolicyNumberError: '',
      primaryContactPhoneError: '',
    };
  },

  componentDidMount() {
    this.input.focus();
  },

  updateState(state, callback) {
    this.setState(state, callback);
  },

  didChange() {
    if (this.state.organizationName !== '') { return true; }
    if (this.state.doingBusinessAs !== '') { return true; }
    if (this.state.givenName !== '') { return true; }
    if (this.state.surname !== '') { return true; }
    if (this.state.address1 !== '') { return true; }
    if (this.state.address2 !== '') { return true; }
    if (this.state.city !== '') { return true; }
    if (this.state.province !== 'BC') { return true; }
    if (this.state.postalCode !== '') { return true; }
    if (this.state.ownerCode !== '') { return true; }
    if (this.state.localAreaId !== 0) { return true; }
    if (this.state.registeredCompanyNumber !== '') { return true; }
    if (this.state.workSafeBCPolicyNumber != '') { return true; }
    if (this.state.primaryContactGivenName !== '') { return true; }
    if (this.state.primaryContactSurname !== '') { return true; }
    if (this.state.primaryContactPhone !== '') { return true; }
    if (this.state.primaryContactRole !== '') { return true; }
    if (this.state.status != Constant.OWNER_STATUS_CODE_APPROVED) { return true; }

    return false;
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
      workSafeBCPolicyNumberError: '',
      primaryContactGivenNameError: '',
      primaryContactPhoneError: '',
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

    if (isBlank(this.state.givenName)) {
      this.setState({ ownerGivenNameError: 'Owner first name is required' });
      valid = false;
    }

    if (isBlank(this.state.surname)) {
      this.setState({ ownerSurameError: 'Owner last name is required' });
      valid = false;
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
      this.setState({ ownerCodeError: 'Owner code is required' });
      valid = false;
    } else {
      var code = this.state.ownerCode.toLowerCase().trim();

      // Must only include letters, up to 7 characters
      if (!onlyLetters(code) || code.length > 7) {
        this.setState({ ownerCodeError: 'This owner code must only include letters, up to 7 characters, and has to be unique to each owner' });
        valid = false;
      }

      // Code must be unique across all owners
      owner = _.find(this.props.owners, (owner) => {
        return owner.ownerCode.toLowerCase().trim() === code;
      });
      if (owner) {
        this.setState({ ownerCodeError: 'This owner code already exists in the system' });
        valid = false;
      }
    }

    if (this.state.localAreaId === 0) {
      this.setState({ localAreaError: 'Service area / local area is required' });
      valid = false;
    }

    if (this.state.meetsResidency === false) {
      this.setState({ residencyError: HELP_TEXT.residency });
      valid = false;
    }

    if (isBlank(this.state.workSafeBCPolicyNumber)) {
      this.setState({ workSafeBCPolicyNumberError: 'WCB number is required' });
      valid = false;
    }

    if (isBlank(this.state.primaryContactGivenName)) {
      this.setState({ primaryContactGivenNameError: 'First name is required' });
      valid = false;
    }

    if (isBlank(this.state.primaryContactPhone)) {
      this.setState({ primaryContactPhoneError: 'Phone number is required' });
      valid = false;
    } else if (!Constant.NANP_REGEX.test(this.state.primaryContactPhone)) {
      this.setState({ primaryContactPhoneError: 'Invalid phone number' });
      valid = false;
    }

    return valid;
  },

  onSave() {
    this.props.onSave({
      organizationName: this.state.name,
      doingBusinessAs: this.state.doingBusinessAs,
      givenName: this.state.givenName,
      surname: this.state.surname,
      address1: this.state.address1,
      address2: this.state.address2,
      city: this.state.city,
      province: this.state.province,
      postalCode: this.state.postalCode,
      ownerCode: this.state.ownerCode,
      localArea: { id: this.state.localAreaId },
      isMaintenanceContractor: this.state.isMaintenanceContractor,
      meetsResidency: this.state.meetsResidency,
      registeredCompanyNumber: this.state.registeredCompanyNumber,
      workSafeBCPolicyNumber: this.state.workSafeBCPolicyNumber,
      primaryContactGivenName: this.state.primaryContactGivenName,
      primaryContactSurname: this.state.primaryContactSurname,
      primaryContactPhone: this.state.primaryContactPhone,
      primaryContactRole: this.state.primaryContactRole,
      status: this.state.status,
    });
  },

  render() {
    // Constrain the local area drop downs to those in the District of the current logged in user
    var localAreas = _.chain(this.props.localAreas)
      .sortBy('name')
      .value();

    return <EditDialog id="add-owner" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title={<strong>Add Owner</strong>}>
      <Form>
        <FormGroup controlId="name" validationState={ this.state.nameError ? 'error' : null }>
          <ControlLabel>Company Name <sup>*</sup></ControlLabel>
          <FormInputControl type="text" value={ this.state.name } updateState={ this.updateState } inputRef={ ref => { this.input = ref; }} />
          <HelpBlock>{ this.state.nameError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="doingBusinessAs">
          <ControlLabel>Doing Business As</ControlLabel>
          <FormInputControl type="text" value={ this.state.doingBusinessAs } updateState={ this.updateState } />
        </FormGroup>
        <FormGroup controlId="givenName" validationState={ this.state.ownerGivenNameError ? 'error' : null }>
          <ControlLabel>Owner First Name <sup>*</sup></ControlLabel>
          <FormInputControl type="text" value={ this.state.givenName } updateState={ this.updateState } />
          <HelpBlock>{ this.state.ownerGivenNameError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="surname" validationState={ this.state.ownerSurameError ? 'error' : null }>
          <ControlLabel>Owner Last Name <sup>*</sup></ControlLabel>
          <FormInputControl type="text" value={ this.state.surname } updateState={ this.updateState } />
          <HelpBlock>{ this.state.ownerSurameError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="address1" validationState={ this.state.address1Error ? 'error' : null }>
          <ControlLabel>Address Line 1 <sup>*</sup></ControlLabel>
          <FormInputControl type="text" value={ this.state.address1 } updateState={ this.updateState }/>
          <HelpBlock>{ this.state.address1Error }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="address2">
          <ControlLabel>Address Line 2</ControlLabel>
          <FormInputControl type="text" value={ this.state.address2 } updateState={ this.updateState }/>
        </FormGroup>
        <FormGroup controlId="city" validationState={ this.state.cityError ? 'error' : null }>
          <ControlLabel>City <sup>*</sup></ControlLabel>
          <FormInputControl type="text" value={ this.state.city } updateState={ this.updateState }/>
          <HelpBlock>{ this.state.cityError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="province" validationState={ this.state.provinceError ? 'error' : null }>
          <ControlLabel>Province <sup>*</sup></ControlLabel>
          <FormInputControl type="text" value={ this.state.province } updateState={ this.updateState }/>
          <HelpBlock>{ this.state.provinceError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="postalCode" validationState={ this.state.postalCodeError ? 'error' : null }>
          <ControlLabel>Postal Code <sup>*</sup></ControlLabel>
          <FormInputControl type="text" value={ this.state.postalCode } updateState={ this.updateState }/>
          <HelpBlock>{ this.state.postalCodeError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="ownerCode" validationState={ this.state.ownerCodeError ? 'error' : null }>
          <ControlLabel>Owner Code <sup>*</sup></ControlLabel>
          <FormInputControl type="text" value={ this.state.ownerCode } updateState={ this.updateState } />
          <HelpBlock>{ this.state.ownerCodeError || HELP_TEXT.prefix }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="localAreaId" validationState={ this.state.localAreaError ? 'error' : null }>
          <ControlLabel>Service Area - Local Area <sup>*</sup></ControlLabel>
          <FilterDropdown id="localAreaId" items={ localAreas } selectedId={ this.state.localAreaId } updateState={ this.updateState } className="full-width" />
          <HelpBlock>{ this.state.localAreaError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="workSafeBCPolicyNumber" validationState={ this.state.workSafeBCPolicyNumberError ? 'error' : null }>
          <ControlLabel>WCB Number <sup>*</sup></ControlLabel>
          <FormInputControl type="text" value={ this.state.workSafeBCPolicyNumber } updateState={ this.updateState }/>
          <HelpBlock>{ this.state.workSafeBCPolicyNumberError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="primaryContactGivenName" validationState={ this.state.primaryContactGivenNameError ? 'error' : null }>
          <ControlLabel>Primary Contact First Name <sup>*</sup></ControlLabel>
          <FormInputControl type="text" value={ this.state.primaryContactGivenName } updateState={ this.updateState } />
          <HelpBlock>{ this.state.primaryContactGivenNameError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="primaryContactSurname">
          <ControlLabel>Primary Contact Last Name</ControlLabel>
          <FormInputControl type="text" value={ this.state.primaryContactSurname } updateState={ this.updateState } />
        </FormGroup>
        <FormGroup controlId="primaryContactPhone" validationState={ this.state.primaryContactPhoneError ? 'error' : null }>
          <ControlLabel>Primary Contact Phone <sup>*</sup></ControlLabel>
          <FormInputControl type="text" defaultValue={ this.state.primaryContactPhone } placeholder="250-555-1212x123" updateState={ this.updateState }/>
          <HelpBlock>{ this.state.primaryContactPhoneError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="primaryContactRole">
          <ControlLabel>Primary Contact Role</ControlLabel>
          <FormInputControl type="text" value={ this.state.primaryContactRole } updateState={ this.updateState } />
        </FormGroup>
        <FormGroup controlId="status" validationState={ this.state.projectStatusCodeError ? 'error' : null }>
          <ControlLabel>Status</ControlLabel>
          <DropdownControl id="status" title={ this.state.status } updateState={ this.updateState }
            value={ this.state.status }
            items={[ Constant.OWNER_STATUS_CODE_APPROVED, Constant.OWNER_STATUS_CODE_PENDING, Constant.OWNER_STATUS_CODE_ARCHIVED ]}
          />
        </FormGroup>
        <FormGroup controlId="registeredCompanyNumber">
          <ControlLabel>Registered BC Company Number</ControlLabel>
          <FormInputControl type="text" value={ this.state.registeredCompanyNumber } updateState={ this.updateState } />
        </FormGroup>
        <FormGroup controlId="isMaintenanceContractor">
          <CheckboxControl id="isMaintenanceContractor" checked={ this.state.isMaintenanceContractor } updateState={ this.updateState }>Maintenance Contractor</CheckboxControl>
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
