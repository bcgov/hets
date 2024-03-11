import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import _ from 'lodash';
import { FormGroup, FormText, FormLabel, FormControl, Container, Row, Col } from 'react-bootstrap';

import * as Api from '../../api';
import * as Constant from '../../constants';
import * as Log from '../../history';

import FormDialog from '../../components/FormDialog.jsx';
import DropdownControl from '../../components/DropdownControl.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { isBlank, notBlank } from '../../utils/string';

class ProjectsAddDialog extends React.Component {
  static propTypes = {
    currentUser: PropTypes.object,
    projects: PropTypes.object,
    fiscalYears: PropTypes.array,
    onSave: PropTypes.func.isRequired,
    onClose: PropTypes.func.isRequired,
    show: PropTypes.bool,
  };

  constructor(props) {
    super(props);

    this.state = {
      isSaving: false,
      name: '',
      fiscalYear: _.first(_.takeRight(props.fiscalYears, 2)),
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
      fiscalYearError: '',
      provincialProjectNumberError: '',
    };
  }

  componentDidMount() {
    this.props.dispatch(Api.getProjects());
  }

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  didChange = () => {
    return (
      notBlank(this.state.name) ||
      notBlank(this.state.fiscalYear) ||
      notBlank(this.state.provincialProjectNumber) ||
      notBlank(this.state.responsibilityCentre) ||
      notBlank(this.state.serviceLine) ||
      notBlank(this.state.stob) ||
      notBlank(this.state.product) ||
      notBlank(this.state.businessFunction) ||
      notBlank(this.state.workActivity) ||
      notBlank(this.state.costType) ||
      notBlank(this.state.information)
    );
  };

  isValid = () => {
    // Clear out any previous errors
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

    const duplicateProject = _.find(this.props.projects.data, (project) => {
      return (
        project.name.toLowerCase().trim() === name.toLowerCase().trim() &&
        project.fiscalYear.toLowerCase().trim() === fiscalYear.toLowerCase().trim() &&
        project.provincialProjectNumber.toLowerCase().trim() === provincialProjectNumber.toLowerCase().trim()
      );
    });

    if (duplicateProject) {
      this.setState({
        nameError: 'A project with the same name and project number exists for the selected fiscal year.',
      });
      valid = false;
    }

    return valid;
  };

  formSubmitted = async () => {
    const dispatch = this.props.dispatch;
    if (this.isValid()) {
      if (this.didChange()) {
        this.setState({ isSaving: true });

        const project = {
          name: this.state.name,
          fiscalYear: this.state.fiscalYear,
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
        };

        const newProject = await dispatch(Api.addProject(project));
        dispatch(Log.projectAdded(newProject));
        this.setState({ isSaving: false });
        if (this.props.onSave) {
          this.props.onSave(newProject);
        }
        this.props.onClose();
      } else {
        this.props.onClose();
      }
    }
  };

  render() {
    return (
      <FormDialog
        id="add-project"
        show={this.props.show}
        title="Add Project"
        isSaving={this.state.isSaving}
        onClose={this.props.onClose}
        onSubmit={this.formSubmitted}
      >
        <Container fluid>
          <Row>
            <Col xs={12}>
              <FormGroup controlId="name">
                <FormLabel>
                  Project Name <sup>*</sup>
                </FormLabel>
                <FormInputControl
                  type="text"
                  value={this.state.name}
                  updateState={this.updateState}
                  autoFocus
                  maxLength="60"
                  isInvalid={this.state.nameError}
                />
                <FormText>{this.state.nameError}</FormText>
              </FormGroup>
            </Col>
          </Row>
          <Row>
            <Col xs={6}>
              <FormGroup controlId="districtId">
                <FormLabel>District</FormLabel>
                <FormControl placeholder={this.props.currentUser.district.name} readOnly />
              </FormGroup>
            </Col>
            <Col xs={6}>
              <FormGroup controlId="fiscalYear">
                <FormLabel>
                  Fiscal Year <sup>*</sup>
                </FormLabel>
                <DropdownControl
                  id="fiscalYear"
                  title={this.state.fiscalYear}
                  updateState={this.updateState}
                  items={_.takeRight(this.props.fiscalYears, 2)}
                  isInvalid={this.state.fiscalYearError}
                />
                <FormText>{this.state.fiscalYearError}</FormText>
              </FormGroup>
            </Col>
          </Row>
          <Row>
            <Col xs={6}>
              <FormGroup controlId="provincialProjectNumber">
                <FormLabel>
                  Provincial Project Number <sup>*</sup>
                </FormLabel>
                <FormInputControl
                  type="text"
                  value={this.state.provincialProjectNumber}
                  updateState={this.updateState}
                  isInvalid={this.state.provincialProjectNumberError}
                />
                <FormText>{this.state.provincialProjectNumberError}</FormText>
              </FormGroup>
            </Col>
            <Col xs={6}>
              <FormGroup controlId="responsibilityCentre">
                <FormLabel>Responsibility Centre</FormLabel>
                <FormInputControl type="text" value={this.state.responsibilityCentre} updateState={this.updateState} />
              </FormGroup>
            </Col>
          </Row>
          <Row>
            <Col xs={6}>
              <FormGroup controlId="serviceLine">
                <FormLabel>Service Line</FormLabel>
                <FormInputControl type="text" value={this.state.serviceLine} updateState={this.updateState} />
              </FormGroup>
            </Col>
            <Col xs={6}>
              <FormGroup controlId="stob">
                <FormLabel>STOB</FormLabel>
                <FormInputControl type="text" value={this.state.stob} updateState={this.updateState} />
              </FormGroup>
            </Col>
          </Row>
          <Row>
            <Col xs={6}>
              <FormGroup controlId="product">
                <FormLabel>Product</FormLabel>
                <FormInputControl type="text" value={this.state.product} updateState={this.updateState} />
              </FormGroup>
            </Col>
            <Col xs={6}>
              <FormGroup controlId="businessFunction">
                <FormLabel>Business Function</FormLabel>
                <FormInputControl type="text" value={this.state.businessFunction} updateState={this.updateState} />
              </FormGroup>
            </Col>
          </Row>
          <Row>
            <Col xs={6}>
              <FormGroup controlId="workActivity">
                <FormLabel>Work Activity</FormLabel>
                <FormInputControl type="text" value={this.state.workActivity} updateState={this.updateState} />
              </FormGroup>
            </Col>
            <Col xs={6}>
              <FormGroup controlId="costType">
                <FormLabel>Cost Type</FormLabel>
                <FormInputControl type="text" value={this.state.costType} updateState={this.updateState} />
              </FormGroup>
            </Col>
          </Row>
          <Row>
            <Col xs={12}>
              <FormGroup controlId="information">
                <FormLabel>Project Information</FormLabel>
                <FormInputControl
                  type="text"
                  as="textarea"
                  rows="5"
                  value={this.state.information}
                  updateState={this.updateState}
                />
              </FormGroup>
            </Col>
          </Row>
        </Container>
      </FormDialog>
    );
  }
}

const mapStateToProps = (state) => ({
  currentUser: state.user,
  projects: state.lookups.projects,
  fiscalYears: state.lookups.fiscalYears,
});

const mapDispatchToProps = (dispatch) => ({ dispatch });

export default connect(mapStateToProps, mapDispatchToProps)(ProjectsAddDialog);
