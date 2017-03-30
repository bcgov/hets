import React from 'react';

import { connect } from 'react-redux';

import { Form, FormGroup, HelpBlock, ControlLabel } from 'react-bootstrap';

import _ from 'lodash';

import * as Constant from '../../constants';

import EditDialog from '../../components/EditDialog.jsx';
import FilterDropdown from '../../components/FilterDropdown.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { isBlank, notBlank } from '../../utils/string';

var ProjectsAddDialog = React.createClass({
  propTypes: {
    currentUser: React.PropTypes.object,
    districts: React.PropTypes.object,
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
  },

  getInitialState() {
    // Project district should default to the district of the logged-in user
    return {
      name: '',
      provincialProjectNumber: '',
      districtId: this.props.districts[this.props.currentUser.district.id] || 0,
      information: '',

      nameError: '',
      districtError: '',
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
      notBlank(this.state.provincialProjectNumber) ||
      notBlank(this.state.information) ||
      this.state.districtId !== 0;
  },

  isValid() {
    // Clear out any previous errors
    var valid = true;

    this.setState({
      nameError: '',
      districtError: '',
    });

    if (isBlank(this.state.name)) {
      this.setState({ nameError: 'Name is required' });
      valid = false;
    }

    if (this.state.districtId === 0) {
      this.setState({ districtError: 'District is required' });
      valid = false;
    }

    return valid;
  },

  onSave() {
    this.props.onSave({
      name: this.state.name,
      provincialProjectNumber: this.state.provincialProjectNumber,
      district: { id: this.state.districtId },
      information: this.state.information,
      status: Constant.PROJECT_STATUS_CODE_ACTIVE,
    });
  },

  render() {
    var districts = _.sortBy(this.props.districts, 'name');

    return <EditDialog id="add-project" show={ this.props.show } bsSize="small"
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title= {
        <strong>Add Project</strong>
      }>
      <Form>
        <FormGroup controlId="name" validationState={ this.state.nameError ? 'error' : null }>
          <ControlLabel>Project Name <sup>*</sup></ControlLabel>
          <FormInputControl type="text" value={ this.state.name } updateState={ this.updateState } inputRef={ ref => { this.input = ref; }} />
          <HelpBlock>{ this.state.nameError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="provincialProjectNumber">
          <ControlLabel>Provincial Project Number</ControlLabel>
          <FormInputControl type="text" value={ this.state.provincialProjectNumber } updateState={ this.updateState } />
        </FormGroup>
        <FormGroup controlId="districtId" validationState={ this.state.districtError ? 'error' : null }>
          <ControlLabel>District <sup>*</sup></ControlLabel>
          <FilterDropdown id="districtId" items={ districts } selectedId={ this.state.districtId } updateState={ this.updateState } />
          <HelpBlock>{ this.state.districtError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="information">
          <ControlLabel>Project Information</ControlLabel>
          <FormInputControl type="text" componentClass="textarea" rows="5" value={ this.state.information } updateState={ this.updateState } />
        </FormGroup>
      </Form>
    </EditDialog>;
  },
});

function mapStateToProps(state) {
  return {
    currentUser: state.user,
    districts: state.lookups.districts,
  };
}

export default connect(mapStateToProps)(ProjectsAddDialog);