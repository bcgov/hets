import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { FormGroup, FormText, FormLabel } from 'react-bootstrap';
import _ from 'lodash';

import * as Constant from '../../constants';
import * as Api from '../../api';
import * as Log from '../../history';

import FormDialog from '../../components/FormDialog.jsx';
import CheckboxControl from '../../components/CheckboxControl.jsx';
import DropdownControl from '../../components/DropdownControl.jsx';
import FilterDropdown from '../../components/FilterDropdown.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { isBlank, onlyLetters } from '../../utils/string';

const HELP_TEXT = {
  prefix: 'This field must include only letters, up to 7 characters and be unique across all owners',
  residency:
    'You have not indicated the owner meets the Residency requirements of the HETS Program. If that is the case, the owner may not be registered in this local area until they have met this requirement. If this check was missed inadvertently, return and activate the checkbox. If the owner does not meet the residency requirement, return and cancel from the Add Owner popup.',
};

class OwnersAddDialog extends React.Component {
  static propTypes = {
    owners: PropTypes.object,
    currentUser: PropTypes.object,
    localAreas: PropTypes.object,
    onSave: PropTypes.func.isRequired,
    onClose: PropTypes.func.isRequired,
    show: PropTypes.bool,
  };

  constructor(props) {
    super(props);

    this.state = {
      isSaving: false,
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
  }

  componentDidMount() {
    Api.getOwnersLite();
  }

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  didChange = () => {
    if (this.state.organizationName !== '') {
      return true;
    }
    if (this.state.doingBusinessAs !== '') {
      return true;
    }
    if (this.state.givenName !== '') {
      return true;
    }
    if (this.state.surname !== '') {
      return true;
    }
    if (this.state.address1 !== '') {
      return true;
    }
    if (this.state.address2 !== '') {
      return true;
    }
    if (this.state.city !== '') {
      return true;
    }
    if (this.state.province !== 'BC') {
      return true;
    }
    if (this.state.postalCode !== '') {
      return true;
    }
    if (this.state.ownerCode !== '') {
      return true;
    }
    if (this.state.localAreaId !== 0) {
      return true;
    }
    if (this.state.registeredCompanyNumber !== '') {
      return true;
    }
    if (this.state.workSafeBCPolicyNumber !== '') {
      return true;
    }
    if (this.state.primaryContactGivenName !== '') {
      return true;
    }
    if (this.state.primaryContactSurname !== '') {
      return true;
    }
    if (this.state.primaryContactPhone !== '') {
      return true;
    }
    if (this.state.primaryContactRole !== '') {
      return true;
    }
    if (this.state.status !== Constant.OWNER_STATUS_CODE_APPROVED) {
      return true;
    }

    return false;
  };

  isValid = () => {
    // Clear out any previous errors
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
      var nameIgnoreCase = this.state.name.toLowerCase().trim();
      var other = _.find(
        this.props.owners.data,
        (other) => other.organizationName.toLowerCase().trim() === nameIgnoreCase
      );
      if (other) {
        this.setState({
          nameError: 'This company name already exists in the system',
        });
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
    } else if (!Constant.POSTAL_CODE_REGEX.test(this.state.postalCode)) {
      this.setState({ postalCodeError: 'Invalid postal code' });
      valid = false;
    }

    if (isBlank(this.state.ownerCode)) {
      this.setState({ ownerCodeError: 'Owner code is required' });
      valid = false;
    } else {
      var code = this.state.ownerCode.toLowerCase().trim();

      // Must only include letters, up to 7 characters
      if (!onlyLetters(code) || code.length > 7) {
        this.setState({
          ownerCodeError:
            'This owner code must only include letters, up to 7 characters, and has to be unique to each owner',
        });
        valid = false;
      }
    }

    if (this.state.localAreaId === 0) {
      this.setState({
        localAreaError: 'Service area / local area is required',
      });
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
  };

  formSubmitted = () => {
    if (this.isValid()) {
      if (this.didChange()) {
        this.setState({ isSaving: true });

        var owner = {
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
        };

        const promise = Api.addOwner(owner);

        promise.then((newOwner) => {
          Log.ownerAdded(newOwner);
          this.setState({ isSaving: false });
          if (this.props.onSave) {
            this.props.onSave(newOwner);
          }
          this.props.onClose();
        });
      } else {
        this.props.onClose();
      }
    }
  };

  render() {
    // Constrain the local area drop downs to those in the District of the current logged in user
    var localAreas = _.chain(this.props.localAreas).sortBy('name').value();

    return (
      <FormDialog
        id="add-owner"
        show={this.props.show}
        title="Add Owner"
        isSaving={this.state.isSaving}
        onClose={this.props.onClose}
        onSubmit={this.formSubmitted}
      >
        <FormGroup controlId="name" validationState={this.state.nameError ? 'error' : null}>
          <FormLabel>
            Company Name <sup>*</sup>
          </FormLabel>
          <FormInputControl type="text" value={this.state.name} updateState={this.updateState} autoFocus />
          <FormText>{this.state.nameError}</FormText>
        </FormGroup>
        <FormGroup controlId="doingBusinessAs">
          <FormLabel>Doing Business As</FormLabel>
          <FormInputControl type="text" value={this.state.doingBusinessAs} updateState={this.updateState} />
        </FormGroup>
        <FormGroup controlId="givenName" validationState={this.state.ownerGivenNameError ? 'error' : null}>
          <FormLabel>
            Owner First Name <sup>*</sup>
          </FormLabel>
          <FormInputControl type="text" value={this.state.givenName} updateState={this.updateState} />
          <FormText>{this.state.ownerGivenNameError}</FormText>
        </FormGroup>
        <FormGroup controlId="surname" validationState={this.state.ownerSurameError ? 'error' : null}>
          <FormLabel>
            Owner Last Name <sup>*</sup>
          </FormLabel>
          <FormInputControl type="text" value={this.state.surname} updateState={this.updateState} />
          <FormText>{this.state.ownerSurameError}</FormText>
        </FormGroup>
        <FormGroup controlId="address1" validationState={this.state.address1Error ? 'error' : null}>
          <FormLabel>
            Address Line 1 <sup>*</sup>
          </FormLabel>
          <FormInputControl type="text" value={this.state.address1} updateState={this.updateState} />
          <FormText>{this.state.address1Error}</FormText>
        </FormGroup>
        <FormGroup controlId="address2">
          <FormLabel>Address Line 2</FormLabel>
          <FormInputControl type="text" value={this.state.address2} updateState={this.updateState} />
        </FormGroup>
        <FormGroup controlId="city" validationState={this.state.cityError ? 'error' : null}>
          <FormLabel>
            City <sup>*</sup>
          </FormLabel>
          <FormInputControl type="text" value={this.state.city} updateState={this.updateState} />
          <FormText>{this.state.cityError}</FormText>
        </FormGroup>
        <FormGroup controlId="province" validationState={this.state.provinceError ? 'error' : null}>
          <FormLabel>
            Province <sup>*</sup>
          </FormLabel>
          <FormInputControl type="text" value={this.state.province} updateState={this.updateState} />
          <FormText>{this.state.provinceError}</FormText>
        </FormGroup>
        <FormGroup controlId="postalCode" validationState={this.state.postalCodeError ? 'error' : null}>
          <FormLabel>
            Postal Code <sup>*</sup>
          </FormLabel>
          <FormInputControl type="text" value={this.state.postalCode} updateState={this.updateState} />
          <FormText>{this.state.postalCodeError}</FormText>
        </FormGroup>
        <FormGroup controlId="ownerCode" validationState={this.state.ownerCodeError ? 'error' : null}>
          <FormLabel>
            Owner Code <sup>*</sup>
          </FormLabel>
          <FormInputControl type="text" value={this.state.ownerCode} updateState={this.updateState} />
          <FormText>{this.state.ownerCodeError || HELP_TEXT.prefix}</FormText>
        </FormGroup>
        <FormGroup controlId="localAreaId" validationState={this.state.localAreaError ? 'error' : null}>
          <FormLabel>
            Service Area - Local Area <sup>*</sup>
          </FormLabel>
          <FilterDropdown
            id="localAreaId"
            items={localAreas}
            selectedId={this.state.localAreaId}
            updateState={this.updateState}
            className="full-width"
          />
          <FormText>{this.state.localAreaError}</FormText>
        </FormGroup>
        <FormGroup
          controlId="workSafeBCPolicyNumber"
          validationState={this.state.workSafeBCPolicyNumberError ? 'error' : null}
        >
          <FormLabel>
            WCB Number <sup>*</sup>
          </FormLabel>
          <FormInputControl type="text" value={this.state.workSafeBCPolicyNumber} updateState={this.updateState} />
          <FormText>{this.state.workSafeBCPolicyNumberError}</FormText>
        </FormGroup>
        <FormGroup
          controlId="primaryContactGivenName"
          validationState={this.state.primaryContactGivenNameError ? 'error' : null}
        >
          <FormLabel>
            Primary Contact First Name <sup>*</sup>
          </FormLabel>
          <FormInputControl type="text" value={this.state.primaryContactGivenName} updateState={this.updateState} />
          <FormText>{this.state.primaryContactGivenNameError}</FormText>
        </FormGroup>
        <FormGroup controlId="primaryContactSurname">
          <FormLabel>Primary Contact Last Name</FormLabel>
          <FormInputControl type="text" value={this.state.primaryContactSurname} updateState={this.updateState} />
        </FormGroup>
        <FormGroup
          controlId="primaryContactPhone"
          validationState={this.state.primaryContactPhoneError ? 'error' : null}
        >
          <FormLabel>
            Primary Contact Phone <sup>*</sup>
          </FormLabel>
          <FormInputControl
            type="text"
            defaultValue={this.state.primaryContactPhone}
            placeholder="250-555-1212x123"
            updateState={this.updateState}
          />
          <FormText>{this.state.primaryContactPhoneError}</FormText>
        </FormGroup>
        <FormGroup controlId="primaryContactRole">
          <FormLabel>Primary Contact Role</FormLabel>
          <FormInputControl type="text" value={this.state.primaryContactRole} updateState={this.updateState} />
        </FormGroup>
        <FormGroup controlId="status" validationState={this.state.projectStatusCodeError ? 'error' : null}>
          <FormLabel>Status</FormLabel>
          <DropdownControl
            id="status"
            title={this.state.status}
            updateState={this.updateState}
            value={this.state.status}
            items={[
              Constant.OWNER_STATUS_CODE_APPROVED,
              Constant.OWNER_STATUS_CODE_PENDING,
              Constant.OWNER_STATUS_CODE_ARCHIVED,
            ]}
          />
        </FormGroup>
        <FormGroup controlId="registeredCompanyNumber">
          <FormLabel>Registered BC Company Number</FormLabel>
          <FormInputControl type="text" value={this.state.registeredCompanyNumber} updateState={this.updateState} />
        </FormGroup>
        <FormGroup controlId="isMaintenanceContractor">
          <CheckboxControl
            id="isMaintenanceContractor"
            checked={this.state.isMaintenanceContractor}
            updateState={this.updateState}
          >
            Maintenance Contractor
          </CheckboxControl>
        </FormGroup>
        <FormGroup controlId="meetsResidency" validationState={this.state.residencyError ? 'error' : null}>
          <CheckboxControl id="meetsResidency" checked={this.state.meetsResidency} updateState={this.updateState}>
            Meets Residency
          </CheckboxControl>
          <FormText>{this.state.residencyError}</FormText>
        </FormGroup>
      </FormDialog>
    );
  }
}

function mapStateToProps(state) {
  return {
    currentUser: state.user,
    owners: state.lookups.owners.lite,
    localAreas: state.lookups.localAreas,
  };
}

export default connect(mapStateToProps)(OwnersAddDialog);
