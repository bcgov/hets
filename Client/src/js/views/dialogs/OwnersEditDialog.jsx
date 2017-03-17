import React from 'react';

import { connect } from 'react-redux';

import { Form, FormGroup, HelpBlock, ControlLabel } from 'react-bootstrap';

import _ from 'lodash';

import EditDialog from '../../components/EditDialog.jsx';
import FilterDropdown from '../../components/FilterDropdown.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { isBlank } from '../../utils/string';

var OwnersEditDialog = React.createClass({
  propTypes: {
    owner: React.PropTypes.object,
    owners: React.PropTypes.object,
    localAreas: React.PropTypes.object,
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
  },

  getInitialState() {
    var owner = this.props.owner;
    return {
      organizationName: owner.organizationName || '',
      localAreaId: owner.localArea.id || 0,
      doingBusinessAs: owner.doingBusinessAs || '',
      registeredCompanyNumber: owner.registeredCompanyNumber || '',
      organizationNameError: '',
      localAreaError: '',
    };
  },

  componentDidMount() {
    this.input.focus();
  },

  updateState(state, callback) {
    this.setState(state, callback);
  },

  didChange() {
    var owner = this.props.owner;

    if (this.state.organizationName !== owner.organizationName) { return true; }
    if (this.state.localAreaId !== owner.localArea.id) { return true; }
    if (this.state.doingBusinessAs !== owner.doingBusinessAs) { return true; }
    if (this.state.registeredCompanyNumber !== owner.registeredCompanyNumber) { return true; }

    return false;
  },

  isValid() {
    this.setState({
      organizationNameError: '',
      localAreaError: '',
    });

    var valid = true;
    var owner = this.props.owner;
    var orgName = this.state.organizationName;

    if (isBlank(orgName)) {
      this.setState({ organizationNameError: 'Company name is required' });
      valid = false;
    } else if (orgName !== owner.organizationName) {
      // Does the name already exist?
      var nameIgnoreCase = orgName.toLowerCase().trim();
      var otherOwners = _.reject(this.props.owners, { id: owner.id });
      var other = _.find(otherOwners, other => other.organizationName.toLowerCase().trim() === nameIgnoreCase);
      if (other) {
        this.setState({ organizationNameError: 'This company name already exists in the system' });
        valid = false;
      }
    }

    if (this.state.localAreaId === 0) {
      this.setState({ localAreaError: 'Local area is required' });
      valid = false;
    }

    return valid;
  },

  onSave() {
    this.props.onSave({ ...this.props.owner, ...{
      organizationName: this.state.organizationName,
      localArea: { id: this.state.localAreaId },
      doingBusinessAs: this.state.doingBusinessAs,
      registeredCompanyNumber: this.state.registeredCompanyNumber,
    }});
  },

  render() {
    var owner = this.props.owner;
    var localAreas = _.sortBy(this.props.localAreas, 'name');

    return <EditDialog id="owners-edit" show={ this.props.show } bsSize="small"
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title= {
        <strong>Owner</strong>
      }>
      <Form>
        <FormGroup controlId="ownerEquipmentCodePrefix">
          <ControlLabel>Equipment Prefix</ControlLabel>
          <h4>{ owner.ownerEquipmentCodePrefix }</h4>
        </FormGroup>
        <FormGroup controlId="organizationName" validationState={ this.state.organizationNameError ? 'error' : null }>
          <ControlLabel>Company Name <sup>*</sup></ControlLabel>
          <FormInputControl type="text" value={ this.state.organizationName } updateState={ this.updateState } inputRef={ ref => { this.input = ref; }} />
          <HelpBlock>{ this.state.organizationNameError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="localAreaId" validationState={ this.state.localAreaError ? 'error' : null }>
          <ControlLabel>Local Area <sup>*</sup></ControlLabel>
          <FilterDropdown id="localAreaId" items={ localAreas } selectedId={ this.state.localAreaId } updateState={ this.updateState } />
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
      </Form>
    </EditDialog>;
  },
});

function mapStateToProps(state) {
  return {
    owner: state.models.owner,
    owners: state.lookups.owners,
    localAreas: state.lookups.localAreas,
  };
}

export default connect(mapStateToProps)(OwnersEditDialog);
