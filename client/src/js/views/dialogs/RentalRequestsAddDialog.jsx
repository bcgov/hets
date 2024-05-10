import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { FormGroup, FormText, FormLabel, Alert } from 'react-bootstrap';
import _ from 'lodash';
import Moment from 'moment';

import * as Api from '../../api';
import * as Constant from '../../constants';

import DateControl from '../../components/DateControl.jsx';
import FormDialog from '../../components/FormDialog.jsx';
import FilterDropdown from '../../components/FilterDropdown.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { isValidDate, today } from '../../utils/date';
import { isBlank } from '../../utils/string';

class RentalRequestsAddDialog extends React.Component {
  static propTypes = {
    currentUser: PropTypes.object,
    localAreas: PropTypes.object,
    districtEquipmentTypes: PropTypes.object,
    projects: PropTypes.object,
    project: PropTypes.object,
    onRentalAdded: PropTypes.func,
    onClose: PropTypes.func.isRequired,
    show: PropTypes.bool,
    viewOnly: PropTypes.bool,
  };

  constructor(props) {
    super(props);

    this.state = {
      loading: false,
      savingError: '',
      projectId: props.project ? props.project.id : 0,
      localAreaId: 0,
      equipmentTypeId: 0,
      count: 1,
      expectedHours: '',
      expectedStartDate: today(),
      expectedEndDate: '',
      rentalRequestAttachments: [],

      projectError: '',
      localAreaError: '',
      equipmentTypeError: '',
      countError: '',
      expectedHoursError: '',
      expectedStartDateError: '',
      expectedEndDateError: '',
    };
  }

  componentDidMount() {
    this.props.dispatch(Api.getDistrictEquipmentTypes());
    if (this.canChangeProject()) {
      this.props.dispatch(Api.getProjectsCurrentFiscal());
    }
  }

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  updateEquipmentTypeState = (state) => {
    var selectedEquipment = _.find(this.props.districtEquipmentTypes.data, { id: state.equipmentTypeId });
    var isDumpTruck = selectedEquipment.equipmentType.isDumpTruck;
    var expectedHours = isDumpTruck ? 600 : 300;
    this.setState({ ...state, expectedHours });
  };

  didChange = () => {
    if (this.state.projectId !== 0) {
      return true;
    }
    if (this.state.localAreaId !== 0) {
      return true;
    }
    if (this.state.equipmentTypeId !== 0) {
      return true;
    }
    if (this.state.count !== 1) {
      return true;
    }
    if (this.state.expectedHours !== '') {
      return true;
    }
    if (this.state.expectedStartDate !== today()) {
      return true;
    }
    if (this.state.expectedEndDate !== '') {
      return true;
    }
    if (this.state.rentalRequestAttachments !== '') {
      return true;
    }

    return false;
  };

  isValid = () => {
    // Clear out any previous errors
    var valid = true;
    this.setState({
      projectError: '',
      localAreaError: '',
      equipmentTypeError: '',
      countError: '',
      expectedHoursError: '',
      expectedStartDateError: '',
      expectedEndDateError: '',
    });

    if (this.state.projectId === 0 && !this.props.viewOnly) {
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

    // all fields for view-only requests have now been validated
    if (this.props.viewOnly) {
      return valid;
    }

    if (isBlank(this.state.count)) {
      this.setState({ countError: 'Equipment quantity is required' });
      valid = false;
    } else if (this.state.count < 1) {
      this.setState({ countError: 'Equipment quantity not valid' });
      valid = false;
    }

    if (isBlank(this.state.expectedHours)) {
      this.setState({ expectedHoursError: 'Estimated hours are required' });
      valid = false;
    } else if (this.state.expectedHours < 1) {
      this.setState({ expectedHoursError: 'Estimated hours not valid' });
      valid = false;
    }

    if (isBlank(this.state.expectedStartDate)) {
      this.setState({ expectedStartDateError: 'Start date is required' });
      valid = false;
    } else if (!isValidDate(this.state.expectedStartDate)) {
      this.setState({ expectedStartDateError: 'Date not valid' });
      valid = false;
    }

    if (Moment(this.state.expectedEndDate).isBefore(this.state.expectedStartDate)) {
      this.setState({ expectedEndDateError: 'End date must be later than the start date' });
      valid = false;
    }

    return valid;
  };

  onLocalAreaSelected = (localArea) => {
    // clear the selected equipment type if it's not included in the types for the new local area
    var districtEquipmentTypes = this.getFilteredEquipmentTypes(localArea.id);
    if (_.filter(districtEquipmentTypes, (type) => type.id === this.state.equipmentTypeId).length === 0) {
      this.setState({ equipmentTypeId: 0 });
    }
  };

  onProjectSelected = () => {
    // TODO Restrict the available local areas to a project service area
  };

  formSubmitted = async () => {
    if (this.isValid()) {
      if (this.didChange()) {
        this.setState({ isSaving: true });

        const dispatch = this.props.dispatch;
        const { rentalRequestAttachments } = this.state;

        const request = {
          project: { id: this.state.projectId },
          localArea: { id: this.state.localAreaId },
          districtEquipmentType: { id: this.state.equipmentTypeId },
          equipmentCount: this.state.count,
          status: Constant.RENTAL_REQUEST_STATUS_CODE_IN_PROGRESS,
          expectedHours: this.state.expectedHours,
          expectedStartDate: this.state.expectedStartDate,
          expectedEndDate: this.state.expectedEndDate,
          rentalRequestAttachments: [
            {
              id: 0,
              attachment: rentalRequestAttachments.length === 0 ? undefined : rentalRequestAttachments,
            },
          ],
        };

        try {
          const response = await dispatch(Api.addRentalRequest(request, this.props.viewOnly));
          this.setState({ isSaving: false, newAddedRequestId: response.id});
          this.props.onRentalAdded(response);
          this.props.onNewRentalQuestIdGenerated(response.id);
          this.props.onClose();
        } catch (error) {
          this.setState({ isSaving: false });
          if (error.status === 400 && error.errorCode === 'HETS-28') {
            this.setState({ savingError: error.errorDescription });
          } else {
            throw error;
          }
        }
      }
    }
  };

  getFilteredEquipmentTypes = (localAreaId) => {
    return _.chain(this.props.districtEquipmentTypes.data)
      .filter(
        (type) =>
          (type.equipmentCount > 0 && !localAreaId) ||
          _.filter(type.localAreas, (localArea) => localArea.id === localAreaId && localArea.equipmentCount > 0)
            .length > 0
      )
      .sortBy('districtEquipmentName')
      .value();
  };

  canChangeProject = () => {
    return !this.props.project && !this.props.viewOnly;
  };

  renderForm = () => {
    const { project } = this.props;

    // Constrain the local area drop downs to those in the District of the current logged in user
    var localAreas = _.chain(this.props.localAreas).sortBy('name').value();

    var districtEquipmentTypes = this.getFilteredEquipmentTypes(this.state.localAreaId);

    var projects = _.sortBy(this.props.projects.data, 'name');

    const hasPickedLocalArea = Boolean(this.state.localAreaId);

    return (
      <div>
        <FormGroup controlId="projectId">
          <FormLabel>Project {!project && !this.props.viewOnly && <sup>*</sup>}</FormLabel>
          {this.canChangeProject() ? (
            <FilterDropdown
              id="projectId"
              className="full-width"
              fieldName="label"
              disabled={!this.props.projects.loaded}
              selectedId={this.state.projectId}
              onSelect={this.onProjectSelected}
              updateState={this.updateState}
              items={projects}
              isInvalid={this.state.projectError}
            />
          ) : (
            <div>{project ? project.name : 'Request - View Only'}</div>
          )}
          <FormText>{this.state.projectError}</FormText>
        </FormGroup>
        <FormGroup controlId="localAreaId">
          <FormLabel>
            Local Area <sup>*</sup>
          </FormLabel>
          <FilterDropdown
            id="localAreaId"
            selectedId={this.state.localAreaId}
            onSelect={this.onLocalAreaSelected}
            updateState={this.updateState}
            items={localAreas}
            className="full-width"
            isInvalid={this.state.localAreaError}
          />
          <FormText>{this.state.localAreaError}</FormText>
        </FormGroup>
        <FormGroup controlId="equipmentTypeId">
          <FormLabel>
            Equipment Type <sup>*</sup>
          </FormLabel>
          <FilterDropdown
            id="equipmentTypeId"
            className="full-width"
            fieldName="districtEquipmentName"
            disabled={!this.props.districtEquipmentTypes.loaded || !hasPickedLocalArea}
            disabledTooltip="Select Local Area to see associated Equipment Type"
            selectedId={this.state.equipmentTypeId}
            updateState={this.updateEquipmentTypeState}
            items={districtEquipmentTypes}
            isInvalid={this.state.equipmentTypeError}
          />
          <FormText>{this.state.equipmentTypeError}</FormText>
        </FormGroup>
        {!this.props.viewOnly && (
          <FormGroup controlId="count">
            <FormLabel>
              Quantity <sup>*</sup>
            </FormLabel>
            <FormInputControl
              type="number"
              min="0"
              value={this.state.count}
              updateState={this.updateState}
              isInvalid={this.state.countError}
            />
            <FormText>{this.state.countError}</FormText>
          </FormGroup>
        )}
        {!this.props.viewOnly && (
          <FormGroup>
            <FormLabel>Attachment(s)</FormLabel>
            <FormInputControl
              id="rentalRequestAttachments"
              type="text"
              defaultValue={this.state.rentalRequestAttachments}
              updateState={this.updateState}
            />
          </FormGroup>
        )}
        {!this.props.viewOnly && (
          <FormGroup controlId="expectedHours">
            <FormLabel>
              Expected Hours <sup>*</sup>
            </FormLabel>
            <FormInputControl
              type="number"
              className="full-width"
              min={0}
              value={this.state.expectedHours}
              updateState={this.updateState}
              isInvalid={this.state.expectedHoursError}
            />
            <FormText>{this.state.expectedHoursError}</FormText>
          </FormGroup>
        )}
        {!this.props.viewOnly && (
          <FormGroup controlId="expectedStartDate">
            <FormLabel>
              Start Date <sup>*</sup>
            </FormLabel>
            <DateControl
              id="expectedStartDate"
              date={this.state.expectedStartDate}
              updateState={this.updateState}
              title="Dated At"
              isInvalid={this.state.expectedStartDateError}
            />
            <FormText>{this.state.expectedStartDateError}</FormText>
          </FormGroup>
        )}
        {!this.props.viewOnly && (
          <FormGroup controlId="expectedEndDate">
            <FormLabel>End Date</FormLabel>
            <DateControl
              id="expectedEndDate"
              date={this.state.expectedEndDate}
              updateState={this.updateState}
              title="Dated At"
              isInvalid={this.state.expectedEndDateError}
            />
            <FormText>{this.state.expectedEndDateError}</FormText>
          </FormGroup>
        )}
        {this.state.savingError && <Alert variant="danger">{this.state.savingError}</Alert>}
      </div>
    );
  };

  render() {
    const { isSaving } = this.state;
    const { show, onClose } = this.props;

    return (
      <FormDialog
        id="add-rental-request"
        title="Add Rental Request"
        show={show}
        isSaving={isSaving}
        onClose={onClose}
        onSubmit={this.formSubmitted}
      >
        {this.renderForm()}
      </FormDialog>
    );
  }
}

const mapStateToProps = (state) => ({
  currentUser: state.user,
  localAreas: state.lookups.localAreas,
  districtEquipmentTypes: state.lookups.districtEquipmentTypes,
  projects: state.lookups.projectsCurrentFiscal,
});

const mapDispatchToProps = (dispatch) => ({ dispatch });

export default connect(mapStateToProps, mapDispatchToProps)(RentalRequestsAddDialog);
