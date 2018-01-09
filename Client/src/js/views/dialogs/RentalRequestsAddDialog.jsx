import React from 'react';

import { connect } from 'react-redux';

import { Form, FormGroup, HelpBlock, ControlLabel } from 'react-bootstrap';

import _ from 'lodash';
import Promise from 'bluebird';

import * as Api from '../../api';
import * as Constant from '../../constants';

import EditDialog from '../../components/EditDialog.jsx';
import FilterDropdown from '../../components/FilterDropdown.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';
import Spinner from '../../components/Spinner.jsx';

var RentalRequestsAddDialog = React.createClass({
  propTypes: {
    currentUser: React.PropTypes.object,
    localAreas: React.PropTypes.object,
    districtEquipmentTypes: React.PropTypes.object,
    projects: React.PropTypes.object,
    project: React.PropTypes.object,
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
  },

  getInitialState() {
    const { project } = this.props;
    return {
      loading: false,
      projectId: project ? project.id : 0,
      localAreaId: 0,
      equipmentTypeId: 0,
      count: 1,
      projectError: '',
      localAreaError: '',
      equipmentTypeError: '',
      countError: '',
    };
  },

  componentDidMount() {
    this.setState({ loading: true });
    var equipmentTypesPromise = Api.getDistrictEquipmentTypes(this.props.currentUser.district.id);
    var projectsPromise = Api.getProjects();  // TODO This API call returns unnecessary data for this popup. It should be replaced with a lightweight version of projects (name, Id)
    Promise.all([equipmentTypesPromise, projectsPromise]).then(() => {
      this.setState({ loading: false });
    });
  },

  updateState(state, callback) {
    this.setState(state, callback);
  },

  didChange() {
    return this.state.projectId !== 0 ||
      this.state.localAreaId !== 0 ||
      this.state.equipmentTypeId !== 0 ||
      this.state.count !== 0;
  },

  isValid() {
    // Clear out any previous errors
    var valid = true;
    this.setState({
      projectError: '',
      localAreaError: '',
      equipmentTypeError: '',
      countError: '',
    });

    if (this.state.projectId === 0) {
      this.setState({ projectError: 'Project is required' });
      valid = false;
    }

    if (this.state.localAreaId === 0) {
      this.setState({ localAreaError: 'Local area is required' });
      valid = false;
    }

    if (this.state.equipmentTypeId === 0) {
      this.setState({ equipmentTypeError: 'Equipment type is required' });
      valid = false;
    }

    if (this.state.count < 1) {
      this.setState({ countError: 'Valid equipment count is required' });
      valid = false;
    }

    return valid;
  },

  onProjectSelected(/* project */) {
    // TODO Restrict the available local areas to a project service area
  },

  onSave() {
    this.props.onSave({
      project: { id: this.state.projectId },
      localArea: { id: this.state.localAreaId },
      districtEquipmentType: { id: this.state.equipmentTypeId },
      equipmentCount: this.state.count,
      status: Constant.RENTAL_REQUEST_STATUS_CODE_IN_PROGRESS,  // TODO Use lookup table
    });
  },

  render() {
    if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

    // Constrain the local area drop downs to those in the District of the current logged in user
    var localAreas = _.chain(this.props.localAreas)
      .filter(localArea => localArea.serviceArea.district.id == this.props.currentUser.district.id)
      .sortBy('name')
      .value();

    var districtEquipmentTypes = _.chain(this.props.districtEquipmentTypes)
      .filter(type => type.district.id == this.props.currentUser.district.id)
      .sortBy('districtEquipmentName')
      .value();

    var projects = _.sortBy(this.props.projects, 'name');

    const { project } = this.props;

    return <EditDialog id="add-rental-request" show={ this.props.show } bsSize="small"
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title= {
        <strong>Add Rental Request</strong>
      }>
      <Form>
        <FormGroup controlId="projectId" validationState={ this.state.projectError ? 'error' : null }>
          <ControlLabel>Project <sup>*</sup></ControlLabel>
          { project ?
            <div>{ project.name }</div>
            :
            <FilterDropdown id="projectId" selectedId={ this.state.projectId } onSelect={ this.onProjectSelected } updateState={ this.updateState }
              items={ projects } className="full-width"
            />
          }
          <HelpBlock>{ this.state.projectError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="localAreaId" validationState={ this.state.localAreaError ? 'error' : null }>
          <ControlLabel>Local Area <sup>*</sup></ControlLabel>
          <FilterDropdown id="localAreaId" selectedId={ this.state.localAreaId } updateState={ this.updateState }
            items={ localAreas } className="full-width"
          />
          <HelpBlock>{ this.state.localAreaError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="equipmentTypeId" validationState={ this.state.equipmentTypeError ? 'error' : null }>
          <ControlLabel>Equipment Type <sup>*</sup></ControlLabel>
          <FilterDropdown id="equipmentTypeId" fieldName="districtEquipmentName" selectedId={ this.state.equipmentTypeId } updateState={ this.updateState }
            items={ districtEquipmentTypes } className="full-width"
          />
          <HelpBlock>{ this.state.equipmentTypeError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="count" validationState={ this.state.countError ? 'error' : null }>
          <ControlLabel>Count <sup>*</sup></ControlLabel>
          <FormInputControl type="number" min="0" value={ this.state.count } updateState={ this.updateState } />
          <HelpBlock>{ this.state.countError }</HelpBlock>
        </FormGroup>
      </Form>
    </EditDialog>;
  },
});

function mapStateToProps(state) {
  return {
    currentUser: state.user,
    localAreas: state.lookups.localAreas,
    districtEquipmentTypes: state.lookups.districtEquipmentTypes.data,
    projects: state.lookups.projects,
  };
}

export default connect(mapStateToProps)(RentalRequestsAddDialog);