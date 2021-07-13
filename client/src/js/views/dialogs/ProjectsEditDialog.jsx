import PropTypes from 'prop-types';
import React from 'react';

import { connect } from 'react-redux';

import { Grid, Row, Col, FormGroup, HelpBlock, ControlLabel } from 'react-bootstrap';

import _ from 'lodash';

import * as Action from '../../actionTypes';
import * as Api from '../../api';
import * as Log from '../../history';
import store from '../../store';

import DropdownControl from '../../components/DropdownControl.jsx';
import FormDialog from '../../components/FormDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';
import Form from '../../components/Form.jsx';

import { isBlank } from '../../utils/string';

class ProjectsEditDialog extends React.Component {
  static propTypes = {
    fiscalYears: PropTypes.array,
    project: PropTypes.object,
    projects: PropTypes.object,

    onClose: PropTypes.func.isRequired,
    show: PropTypes.bool,
  };

  constructor(props) {
    super(props);

    this.state = {
      name: props.project.name || '',
      fiscalYear: props.project.fiscalYear || _.first( _.takeRight(props.fiscalYears, 2)),
      provincialProjectNumber: props.project.provincialProjectNumber || '',
      responsibilityCentre: props.project.responsibilityCentre || '',
      serviceLine: props.project.serviceLine || '',
      stob: props.project.stob || '',
      product: props.project.product || '',
      businessFunction: props.project.businessFunction || '',
      workActivity: props.project.workActivity || '',
      costType: props.project.costType || '',
      projectInformation: props.project.information || '',
      concurrencyControlNumber: props.project.concurrencyControlNumber || 0,
      nameError: '',
      fiscalYearError: '',
      provincialProjectNumberError: '',
    };
  }

  componentDidMount() {
    Api.getProjects();
  }

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  didChange = () => {
    var project = this.props.project;

    if (this.state.name !== project.name) { return true; }
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

    return false;
  };

  isValid = () => {
    var valid = true;

    this.setState({
      nameError: '',
      fiscalYearError: '',
      provincialProjectNumberError: '',
    });

    const { name, fiscalYear, provincialProjectNumber } = this.state;

    if (isBlank(name)) {
      this.setState({ nameError: 'Project name is required' });
      valid = false;
    }

    if (isBlank(fiscalYear)) {
      this.setState({ fiscalYearError: 'Fiscal year is required' });
      valid = false;
    }

    if (isBlank(provincialProjectNumber)) {
      this.setState({ provincialProjectNumberError: 'Provincial project number is required' });
      valid = false;
    }

    if (!valid) {
      return false;
    }

    const duplicateProject = _.find(this.props.projects.data, project => {
      return project.id !== this.props.project.id &&
             project.name.toLowerCase().trim() === name.toLowerCase().trim() &&
             project.fiscalYear.toLowerCase().trim() === fiscalYear.toLowerCase().trim() &&
             project.provincialProjectNumber.toLowerCase().trim() === provincialProjectNumber.toLowerCase().trim();
    });

    if (duplicateProject) {
      this.setState({ nameError: 'A project with the same name and project number exists for the selected fiscal year.'});
      valid = false;
    }

    return valid;
  };

  formSubmitted = () => {
    if (this.isValid()) {
      if (this.didChange()) {
        const project = {
          ...this.props.project,
          id: this.props.project.id,
          district: this.props.project.district,
          name: this.state.name,
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
        };

        store.dispatch({ type: Action.UPDATE_PROJECT, project });

        Log.projectModified(this.props.project);

        Api.updateProject(project);
      }

      this.props.onClose();
    }
  };

  render() {
    // TODO: Restrict Information box resize
    return (
      <FormDialog
        id="projects-edit"
        show={ this.props.show }
        title="Projects"
        onClose={ this.props.onClose}
        onSubmit={ this.formSubmitted }>
        <Form>
          <Grid fluid>
            <Row>
              <Col xs={12}>
                <FormGroup controlId="name" validationState={ this.state.nameError ? 'error' : null}>
                  <ControlLabel>Project Name <sup>*</sup></ControlLabel>
                  <FormInputControl type="text" value={ this.state.name } updateState={ this.updateState} autoFocus maxLength="60"/>
                  <HelpBlock>{ this.state.nameError }</HelpBlock>
                </FormGroup>
              </Col>
            </Row>
            <Row>
              <Col xs={6}>
                <FormGroup controlId="fiscalYear" validationState={ this.state.fiscalYearError ? 'error' : null }>
                  <ControlLabel>Fiscal Year <sup>*</sup></ControlLabel>
                  <DropdownControl id="fiscalYear" title={ this.state.fiscalYear } updateState={ this.updateState }
                    items={ _.takeRight(this.props.fiscalYears, 2) }
                  />
                  <HelpBlock>{ this.state.fiscalYearError }</HelpBlock>
                </FormGroup>
              </Col>
              <Col xs={6}>
                <FormGroup controlId="provincialProjectNumber" validationState={ this.state.provincialProjectNumberError ? 'error' : null }>
                  <ControlLabel>Provincial Project Number <sup>*</sup></ControlLabel>
                  <FormInputControl type="text" value={ this.state.provincialProjectNumber } updateState={ this.updateState } />
                  <HelpBlock>{ this.state.provincialProjectNumberError }</HelpBlock>
                </FormGroup>
              </Col>
            </Row>
            <Row>
              <Col xs={12}>
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
      </FormDialog>
    );
  }
}

function mapStateToProps(state) {
  return {
    fiscalYears: state.lookups.fiscalYears,
    projects: state.lookups.projects,
  };
}

export default connect(mapStateToProps)(ProjectsEditDialog);
