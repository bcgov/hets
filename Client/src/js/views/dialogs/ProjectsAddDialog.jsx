import React from 'react';

import { connect } from 'react-redux';

import { Grid, Row, Col, Form, FormGroup, HelpBlock, ControlLabel, FormControl } from 'react-bootstrap';

import * as Constant from '../../constants';

import EditDialog from '../../components/EditDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { isBlank, notBlank } from '../../utils/string';

var ProjectsAddDialog = React.createClass({
  propTypes: {
    currentUser: React.PropTypes.object,
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
  },

  getInitialState() {
    return {
      name: '',
      provincialProjectNumber: '',
      responsibilityCentre: '',
      serviceLine: '',
      stob: '',
      product: '',
      businessFunction: '',
      workActivity: '',
      costType: '',
      information: '',

      nameError: '',
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
      notBlank(this.state.responsibilityCentre) ||
      notBlank(this.state.serviceLine) ||
      notBlank(this.state.stob) ||
      notBlank(this.state.product) ||
      notBlank(this.state.businessFunction) ||
      notBlank(this.state.workActivity) ||
      notBlank(this.state.costType) ||
      notBlank(this.state.information);
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

    return valid;
  },

  onSave() {
    this.props.onSave({
      name: this.state.name,
      provincialProjectNumber: this.state.provincialProjectNumber,
      district: { id: this.props.currentUser.district.id },
      status: Constant.PROJECT_STATUS_CODE_ACTIVE,
      responsibilityCentre: this.state.responsibilityCentre,
      serviceLine: this.state.serviceLine,
      stob: this.state.stob,
      product: this.state.product,
      businessFunction: this.state.businessFunction,
      workActivity: this.state.workActivity,
      costType: this.state.costType,
      information: this.state.information,
    });
  },

  render() {
    return <EditDialog id="add-project" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title= {
        <strong>Add Project</strong>
      }>
      <Form>
        <Grid fluid>
          <Row>
            <Col xs={6}>
              <FormGroup controlId="name" validationState={ this.state.nameError ? 'error' : null }>
                <ControlLabel>Project Name <sup>*</sup></ControlLabel>
                <FormInputControl type="text" value={ this.state.name } updateState={ this.updateState } inputRef={ ref => { this.input = ref; }} />
                <HelpBlock>{ this.state.nameError }</HelpBlock>
              </FormGroup>
            </Col>
            <Col xs={6}>
              <FormGroup controlId="districtId" validationState={ this.state.districtError ? 'error' : null }>
                <ControlLabel>District</ControlLabel>
                <FormControl.Static>{ this.props.currentUser.district.name }</FormControl.Static>
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
              <FormGroup controlId="information">
                <ControlLabel>Project Information</ControlLabel>
                <FormInputControl type="text" componentClass="textarea" rows="5" value={ this.state.information } updateState={ this.updateState } />
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
    currentUser: state.user,
  };
}

export default connect(mapStateToProps)(ProjectsAddDialog);