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
      companyAdress: '',
      companyCode: '',
      localAreaId: defaultLocalAreaId.id || 0,
      meetsResidency: true,
      doingBusinessAs: '',
      registeredCompanyNumber: '',
      status: Constant.OWNER_STATUS_CODE_PENDING,

      nameError: '',
      companyAddressError: '',
      companyCodeError: '',
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
      notBlank(this.state.companyCode) ||
      this.state.meetsResidency !== false ||
      this.state.localAreaId !== 0;
  },

  isValid() {
    // Clear out any previous errors
    var owner;
    var valid = true;

    this.setState({
      nameError: '',
      companyAddressError: '',
      companyCodeError: '',
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

    if (isBlank(this.state.companyAddress)) {
      this.setState({ companyAddressError: 'Company address is required' });
      valid = false;
    }

    if (isBlank(this.state.companyCode)) {
      this.setState({ companyCodeError: 'Equipment prefix is required' });
      valid = false;
    } else {
      var prefix = this.state.companyCode.toLowerCase().trim();

      // Prefix must only include letters, up to 5 characters
      if (!onlyLetters(prefix) || prefix.length > 5) {
        this.setState({ companyCodeError: 'This equipment prefix must only include letters, up to 5 characters' });
        valid = false;
      }

      // Prefix must be unique across all owners
      owner = _.find(this.props.owners, (owner) => {
        return owner.ownerEquipmentCodePrefix.toLowerCase().trim() === prefix;
      });
      if (owner) {
        this.setState({ companyCodeError: 'This equipment prefix already exists in the system' });
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
      companyAdress: this.state.companyAdress,
      ownerEquipmentCodePrefix: this.state.companyCode,
      localArea: { id: this.state.localAreaId },
      meetsResidency: this.state.meetsResidency,
      status: Constant.OWNER_STATUS_CODE_APPROVED,
      registeredCompanyNumber: this.state.registeredCompanyNumber,
      doingBusinessAs: this.state.doingBusinessAs,
    });
  },

  render() {
    // Constrain the local area drop downs to those in the District of the current logged in user
    var localAreas = _.chain(this.props.localAreas)
      .filter(localArea => localArea.serviceArea.district.id == this.props.currentUser.district.id)
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
        <FormGroup controlId="companyAddress" validationState={ this.state.companyAddressError ? 'error' : null }>
          <ControlLabel>Company Address <sup>*</sup></ControlLabel>
          <FormInputControl type="text" value={ this.state.companyAddress } updateState={ this.updateState } inputRef={ ref => { this.input = ref; }} />
          <HelpBlock>{ this.state.companyAddressError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="companyCode" validationState={ this.state.companyCodeError ? 'error' : null }>
          <ControlLabel>Company Code <sup>*</sup></ControlLabel>
          <FormInputControl type="text" value={ this.state.companyCode } updateState={ this.updateState } />
          <HelpBlock>{ this.state.companyCodeError || HELP_TEXT.prefix }</HelpBlock>
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
