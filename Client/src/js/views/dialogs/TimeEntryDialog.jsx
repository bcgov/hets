import React from 'react';

import { connect } from 'react-redux';

import { Grid, Row, Col } from 'react-bootstrap';
import { Form, FormGroup, ControlLabel, HelpBlock, Button, Glyphicon } from 'react-bootstrap';
import _ from 'lodash';

import * as Api from '../../api';

import FormInputControl from '../../components/FormInputControl.jsx';
import DateControl from '../../components/DateControl.jsx';
import EditDialog from '../../components/EditDialog.jsx';
import Spinner from '../../components/Spinner.jsx';

import FilterDropdown from '../../components/FilterDropdown.jsx';

import { isBlank } from '../../utils/string';

var TimeEntryDialog = React.createClass({
  propTypes: {
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool.isRequired,
    projects: React.PropTypes.object,
    project: React.PropTypes.object,
    equipmentList: React.PropTypes.object,
    equipment: React.PropTypes.object,
    rentalRequest: React.PropTypes.object,
  },

  getInitialState() {  
    return {
      projectId: this.props.project.id,
      equipment: {},
      numberOfInputs: 1,
      timeEntry: {
        1: {
          hours: '',
          date: '',
        },
      },

      errors: {},
    };
  },

  componentDidMount() {
    let getProjectsPromise = Api.getProjects();
    let getEquipmentListPromise = Api.getEquipmentList();

    return Promise.all([getProjectsPromise, getEquipmentListPromise]);
  },

  updateState(state, callback) {
    this.setState(state, callback);
  },

  updateTimeEntryState(value) {
    let property = Object.keys(value)[0];
    let stateValue = Object.values(value)[0];
    let number = property.match(/\d+/g)[0];
    let stateName = property.match(/[a-zA-Z]+/g)[0];
    let state = { [stateName]:  stateValue };
    let updatedState = { ...this.state.timeEntry, [number]: { ...this.state.timeEntry[number], ...state } };
    this.setState({ timeEntry: updatedState });
  },

  didChange() {
    // todo
    return true;
  },

  isValid() {
    // todo
    let timeEntry = this.state.timeEntry;
    let errors = this.state.errors;

    Object.keys(errors).map((key) => {
      // console.log(timeEntry, errors);
      // let errorKey = { ...errors, [key]: { ...errors[key] } };
      // errorKey[key].hours = '';
      // this.setState({ errors: errorKey });
      let state = { hours: '' };
      let updatedState = { ...errors, [key]: { ...errors[key], ...state } };
      this.setState({ errros: updatedState });
    });
    // this.setState({ errors: { ...{} } }, console.log(this.state));

    let valid = true;

    // _.map(timeEntry, (item, index) => {;
    Object.keys(timeEntry).map((key) => {
      if ((timeEntry[key] == undefined) || isBlank(timeEntry[key].hours)) {
        let state = { ...this.state.errors, [key]: { hours: 'Hours are required' } };
        this.setState({ errors: state });
        valid = false;
      }
    });

    return valid;
  },

  onSave() {
    this.props.onSave({
    });
  },

  onEquipmentSelected(equipment) {
    this.setState({ equipment: equipment });
  },

  addTimeEntryInput() {
    if (this.state.numberOfInputs < 10) {
      this.setState({ numberOfInputs: this.state.numberOfInputs + 1 });
    }
  },

  removeTimeEntryInput() {
    if (this.state.numberOfInputs > 1) {
      this.setState({ numberOfInputs: this.state.numberOfInputs - 1 });
    }
  },

  render() {
    const projects = _.sortBy(this.props.projects, 'name');
    const equipmentList = _.sortBy(this.props.equipmentList.data);
    const rentalRequest = this.props.rentalRequest.data;
    const isValidDate = function( current ){
      return current.day() === 6;
    };
    return (
      <EditDialog 
        id="time-entry" 
        show={ this.props.show }
        onClose={ this.props.onClose } 
        onSave={ this.onSave } 
        didChange={ this.didChange }
        isValid={ this.isValid }
        title={
          <strong>Hets Time Entry</strong>
        }
      >
       { this.props.rentalRequest.loading || this.props.equipmentList.loading ? 
        <div style={{ textAlign: 'center' }}><Spinner/></div>
        :
        <Form>
          <Grid fluid>
            <Row>
              <FormGroup controlId="projectId" validationState={ this.state.projectError ? 'error' : null }>
                <ControlLabel>Project</ControlLabel>
                <FilterDropdown 
                  id="projectId" 
                  selectedId={ this.state.projectId } 
                  onSelect={ this.onProjectSelected } 
                  updateState={ this.updateState }
                  items={ projects } 
                  className="full-width"
                />
                <HelpBlock>{ this.state.projectError }</HelpBlock>
              </FormGroup>
            </Row>
            <Row>
              <FormGroup controlId="equipmentId" validationState={ this.state.equipmentIDError ? 'error' : null }>
                <ControlLabel>Equipment ID</ControlLabel>
                <FilterDropdown 
                  id="selectedEquipmentTypesIds" 
                  fieldName="id"
                  selectedId={ rentalRequest.districtEquipmentType.id }
                  onSelect={ this.onEquipmentSelected } 
                  updateState={ this.updateState } 
                  items={ equipmentList } 
                  className="full-width"
                /> 
                <div>({ this.state.equipment.typeName || rentalRequest.districtEquipmentType.districtEquipmentName })</div>
                <div>Owner: { this.state.equipment.owner && this.state.equipment.owner.organizationName }</div>
              </FormGroup>
            </Row>
            <hr />
            { Array.from(Array(this.state.numberOfInputs), (_, i) => {
              const index = i + 1;
              return (
                <Row key={index}>
                  <Col md={4} className="nopadding">
                    <FormGroup validationState={ this.state.dateError ? 'error' : null }>
                      <ControlLabel>Week Ending</ControlLabel>
                      <DateControl
                        id={`date${index}`}
                        name='date'
                        isValidDate={ isValidDate } 
                        date={ (this.state.timeEntry[index] && this.state.timeEntry[index].date) ? this.state.timeEntry[index].date : null }
                        updateState={ this.updateTimeEntryState } 
                        placeholder="mm/dd/yyyy"   
                      />
                    </FormGroup>
                  </Col>
                  <Col md={4} className="nopadding">
                    <FormGroup validationState={ this.state.hoursError ? 'error' : null }>
                      <ControlLabel>Hours</ControlLabel>
                      <FormInputControl 
                        id={`hours${index}`} 
                        name='hours'
                        type="number" 
                        value={ this.state.timeEntry[index] ? this.state.timeEntry[index].hours : '' }
                        updateState={ this.updateTimeEntryState } 
                      />
                      <HelpBlock>{ this.state.errors[index] ? this.state.errors[index].hours : '' }</HelpBlock>
                    </FormGroup>
                  </Col>
                </Row>
              );
            })}
            { this.state.numberOfInputs < 10 && 
              <Button 
                bsSize="xsmall"
                onClick={ this.addTimeEntryInput }
              >
                <Glyphicon glyph="plus" />&nbsp;<strong>Add</strong>
              </Button>
            }
            { this.state.numberOfInputs > 1 &&
              <Button 
                bsSize="xsmall"
                className="remove-btn"
                onClick={ this.removeTimeEntryInput }
              >
                <Glyphicon glyph="minus" />&nbsp;<strong>Remove</strong>
              </Button>
            }
          </Grid>
        </Form>
       } 
      </EditDialog>
    );
  },
});

function mapStateToProps(state) {
  return {
    projects: state.lookups.projects,
    equipment: state.models.equipment,
    equipmentList: state.models.equipmentList,
    rentalRequest: state.models.rentalRequest,
  };
}

export default connect(mapStateToProps)(TimeEntryDialog);