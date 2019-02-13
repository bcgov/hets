import React from 'react';

import { connect } from 'react-redux';

import { Grid, Row, Col, Form, FormGroup, HelpBlock, ControlLabel } from 'react-bootstrap';

import _ from 'lodash';

import * as Constant from '../../constants';

import DropdownControl from '../../components/DropdownControl.jsx';
import EditDialog from '../../components/EditDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { isBlank } from '../../utils/string';

var ProjectsEditDialog = React.createClass({
  propTypes: {
    fiscalYears: React.PropTypes.array,
    project: React.PropTypes.object,
    projects: React.PropTypes.object,

    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
  },

  getInitialState() {
    return {
      projectName: this.props.project.name || '',
      projectStatus: this.props.project.status || Constant.PROJECT_STATUS_CODE_ACTIVE,
      fiscalYear: this.props.project.fiscalYear || _.first( _.takeRight(this.props.fiscalYears, 2)),
      provincialProjectNumber: this.props.project.provincialProjectNumber || '',
      responsibilityCentre: this.props.project.responsibilityCentre || '',
      serviceLine: this.props.project.serviceLine || '',
      stob: this.props.project.stob || '',
      product: this.props.project.product || '',
      businessFunction: this.props.project.businessFunction || '',
      workActivity: this.props.project.workActivity || '',
      costType: this.props.project.costType || '',
      projectInformation: this.props.project.information || '',
      concurrencyControlNumber: this.props.project.concurrencyControlNumber || 0,
      statusError: '',
      projectNameError: '',
      projectStatusCodeError: '',
    };
  },

  componentDidMount() {
    this.input.focus();
  },

  updateState(state, callback) {
    this.setState(state, callback);
  },

  didChange() {
    var project = this.props.project;

    if (this.state.projectName !== project.name) { return true; }
    if (this.state.fiscalYear !== project.fiscalYear) { return true; }
    if (this.state.provincialProjectNumber !== project.provincialProjectNumber) { return true; }
    if (this.state.responsibilityCentre !== project.responsibilityCentre) { return true; }
    if (this.state.serviceLine !== project.serviceLine) { return true; }
    if (this.state.stob !== project.stob) { return true; }
    if (this.state.product !== project.product) { return true; }
    if (this.state.businessFunction !== project.businessFunction) { return true; }
    if (this.state.workActivity !== project.workActivity) { return true; }
    if (this.state.costType !== project.costType) { return true; }
    if (this.state.projectInformation !== project.information) { return true; }
    if (this.state.projectStatus !== project.status) { return true; }

    return false;
  },

  isValid() {
    this.setState({
      statusError: '',
      fiscalYearError: '',
      projectNameError: '',
    });

    var valid = true;
    var project = this.props.project;
    var projectName = this.state.projectName;

    if(isBlank(projectName)) {
      this.setState({ projectNameError: 'Project name is required' });
      valid = false;
    } else if (projectName !== project.projectName) {
      var nameIgnoreCase = projectName.toLowerCase().trim();
      var existingProjects = _.reject(this.props.projects.data, { id: project.id});
      var existingProjectName = _.find(existingProjects, existingProjectName => existingProjectName.name.toLowerCase().trim() === nameIgnoreCase);
      if (existingProjectName) {
        this.setState({ projectNameError: 'This project name already exists'});
        valid = false;
      }
    }

    if (isBlank(this.state.fiscalYear)) {
      this.setState({ fiscalYearError: 'Fiscal year is required' });
      valid = false;
    }

    if (isBlank(this.state.projectStatus)) {
      this.setState({ projectStatusCodeError: 'Project status is required' });
      valid = false;
    }

    return valid;
  },

  onSave() {
    this.props.onSave({
      id: this.props.project.id,
      canEditStatus: this.props.project.canEditStatus,
      district: this.props.project.district,
      primaryContact: this.props.project.primaryContact,
      name: this.state.projectName,
      status: this.state.projectStatus,
      fiscalYear: this.state.fiscalYear,
      provincialProjectNumber: this.state.provincialProjectNumber,
      responsibilityCentre: this.state.responsibilityCentre,
      serviceLine: this.state.serviceLine,
      stob: this.state.stob,
      product: this.state.product,
      businessFunction: this.state.businessFunction,
      workActivity: this.state.workActivity,
      costType: this.state.costType,
      information: this.state.projectInformation,
      concurrencyControlNumber: this.state.concurrencyControlNumber,
    });
  },

  render() {
    // TODO: Restrict Information box resize
    return <EditDialog id="projects-edit" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title= {
        <strong>Projects</strong>
      }>
      <Form>
        <Grid fluid>
          <Row>
            <Col xs={12}>
              <FormGroup controlId="projectName" validationState={ this.state.projectNameError ? 'error' : null}>
                <ControlLabel>Project Name <sup>*</sup></ControlLabel>
                <FormInputControl type="text" value={ this.state.projectName } updateState={ this.updateState} inputRef={ ref => { this.input = ref; }}/>
                <HelpBlock>{ this.state.projectNameError }</HelpBlock>
              </FormGroup>
            </Col>
          </Row>
          <Row>
            <Col xs={6}>
              <FormGroup controlId="projectStatus" validationState={ this.state.projectStatusCodeError ? 'error' : null }>
                <ControlLabel>Project Status</ControlLabel>
                <DropdownControl id="projectStatus" title={ this.state.projectStatus } updateState={ this.updateState } disabled={ !this.props.project.canEditStatus }
                  value={ this.state.projectStatus }
                  items={[ Constant.PROJECT_STATUS_CODE_ACTIVE, Constant.PROJECT_STATUS_CODE_COMPLETED ]}
                />
                <HelpBlock>{ this.state.projectStatusCodeError }</HelpBlock>
              </FormGroup>
            </Col>
            <Col xs={6}>
              <FormGroup controlId="fiscalYear" validationState={ this.state.fiscalYearError ? 'error' : null }>
                <ControlLabel>Fiscal Year <sup>*</sup></ControlLabel>
                <DropdownControl id="fiscalYear" title={ this.state.fiscalYear } updateState={ this.updateState }
                  items={ _.takeRight(this.props.fiscalYears, 2) }
                />
                <HelpBlock>{ this.state.fiscalYearError }</HelpBlock>
              </FormGroup>
            </Col>
          </Row>
          <Row>
            <Col xs={6}>
              <FormGroup controlId="provincialProjectNumber">
                <ControlLabel>Provincial Project Number</ControlLabel>
                <FormInputControl type="text" value={ this.state.provincialProjectNumber } updateState={ this.updateState } />
              </FormGroup>
            </Col>
            <Col xs={6}>
              <FormGroup controlId="responsibilityCentre">
                <ControlLabel>Responsibility Centre</ControlLabel>
                <FormInputControl type="text" value={ this.state.responsibilityCentre } updateState={ this.updateState } />
              </FormGroup>
            </Col>
          </Row>
          <Row>
            <Col xs={6}>
              <FormGroup controlId="serviceLine">
                <ControlLabel>Service Line</ControlLabel>
                <FormInputControl type="text" value={ this.state.serviceLine } updateState={ this.updateState } />
              </FormGroup>
            </Col>
            <Col xs={6}>
              <FormGroup controlId="stob">
                <ControlLabel>STOB</ControlLabel>
                <FormInputControl type="text" value={ this.state.stob } updateState={ this.updateState } />
              </FormGroup>
            </Col>
          </Row>
          <Row>
            <Col xs={6}>
              <FormGroup controlId="product">
                <ControlLabel>Product</ControlLabel>
                <FormInputControl type="text" value={ this.state.product } updateState={ this.updateState } />
              </FormGroup>
            </Col>
            <Col xs={6}>
              <FormGroup controlId="businessFunction">
                <ControlLabel>Business Function</ControlLabel>
                <FormInputControl type="text" value={ this.state.businessFunction } updateState={ this.updateState } />
              </FormGroup>
            </Col>
          </Row>
          <Row>
            <Col xs={6}>
              <FormGroup controlId="workActivity">
                <ControlLabel>Work Activity</ControlLabel>
                <FormInputControl type="text" value={ this.state.workActivity } updateState={ this.updateState } />
              </FormGroup>
            </Col>
            <Col xs={6}>
              <FormGroup controlId="costType">
                <ControlLabel>Cost Type</ControlLabel>
                <FormInputControl type="text" value={ this.state.costType } updateState={ this.updateState } />
              </FormGroup>
            </Col>
          </Row>
          <Row>
            <Col xs={12}>
              <FormGroup controlId="projectInformation">
                <ControlLabel>Project Information</ControlLabel>
                <FormInputControl type="text" componentClass="textarea" rows="5" value={ this.state.projectInformation } updateState={ this.updateState } />
              </FormGroup>
            </Col>
          </Row>
        </Grid>
      </Form>
    </EditDialog>;
  },
});

function mapStateToProps(state) {
  return {
    fiscalYears: state.lookups.fiscalYears,
    project: state.models.project,
    projects: state.models.projects,
  };
}

export default connect(mapStateToProps)(ProjectsEditDialog);
