import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { Container, Row, Col, FormGroup, FormLabel, FormText, Button } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import _ from 'lodash';
import Moment from 'moment';

import * as Api from '../../api';

import FormDialog from '../../components/FormDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';
import DateControl from '../../components/DateControl.jsx';
import DeleteButton from '../../components/DeleteButton.jsx';
import FilterDropdown from '../../components/FilterDropdown.jsx';
import Spinner from '../../components/Spinner.jsx';

import { isBlank, formatHours } from '../../utils/string';
import { formatDateTime } from '../../utils/date';

class TimeEntryDialog extends React.Component {
  static propTypes = {
    onClose: PropTypes.func.isRequired,
    show: PropTypes.bool.isRequired,
    multipleEntryAllowed: PropTypes.bool.isRequired,
    rentalAgreementId: PropTypes.number,
    rentalAgreementTimeRecords: PropTypes.object,
    project: PropTypes.object,
    projectId: PropTypes.number,
    projects: PropTypes.object.isRequired,
    equipment: PropTypes.object.isRequired,
  };

  constructor(props) {
    super(props);

    this.state = {
      isSaving: false,
      loaded: false,
      rentalAgreementId: props.rentalAgreementId,
      equipmentId: null,
      projectId: props.projectId || null,
      projectFiscalYearStartDate: props.project ? props.project.fiscalYearStartDate : null,
      equipmentIdError: '',
      projectIdError: '',
      selectingAgreement: props.rentalAgreementId ? false : true,
      showAllTimeRecords: false,
      numberOfInputs: 1,
      timeEntry: {
        1: {
          hours: '',
          date: '',
          errorHours: '',
          errorDate: '',
        },
      },
    };
  }

  resetStateToSelectAgreement = (clearSelections) => {
    this.setState({
      rentalAgreementId: null,
      equipmentId: clearSelections ? null : this.state.equipmentId,
      projectId: clearSelections ? null : this.state.projectId,
      projectFiscalYearStartDate: clearSelections ? null : this.state.projectFiscalYearStartDate,
      selectingAgreement: true,
      showAllTimeRecords: false,
      numberOfInputs: 1,
      timeEntry: {
        1: {
          hours: '',
          date: '',
          errorHours: '',
          errorDate: '',
        },
      },
    });
  };

  componentDidMount() {
    if (this.state.selectingAgreement) {
      Api.getProjectsCurrentFiscal();
      Api.getEquipmentLite();
    } else {
      this.setState({ loaded: false });
      Promise.all([!this.props.project ? this.fetchProject(this.props.projectId) : null, this.fetchTimeRecords()]).then(
        () => {
          this.setState({ loaded: true });
        }
      );
    }
  }

  fetchTimeRecords = () => {
    return Api.getRentalAgreementTimeRecords(this.state.rentalAgreementId);
  };

  fetchProject = (projectId) => {
    return Api.getProject(projectId).then((project) => {
      this.setState({ projectFiscalYearStartDate: project.fiscalYearStartDate });
    });
  };

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  validateSelectAgreement = () => {
    var valid = true;

    this.setState({ equipmentIdError: '', projectIdError: '' });

    if (isBlank(this.state.equipmentId)) {
      this.setState({ equipmentIdError: 'Equipment ID is required' });
      valid = false;
    }

    if (isBlank(this.state.projectId)) {
      this.setState({ projectIdError: 'Project is required' });
      valid = false;
    }

    return valid;
  };

  selectAgreementFormSubmitted = () => {
    if (this.validateSelectAgreement()) {
      this.setState({ isSaving: true });

      Api.getLatestRentalAgreement(this.state.equipmentId, this.state.projectId)
        .then((agreement) => {
          this.setState({ loaded: false, rentalAgreementId: agreement.id });
          return Promise.all([this.fetchProject(this.state.projectId), this.fetchTimeRecords()]).then(() => {
            this.setState({ isSaving: false, selectingAgreement: false, loaded: true });
          });
        })
        .catch((error) => {
          this.setState({ isSaving: false });
          if (error.status === 400 && error.errorCode === 'HETS-35') {
            this.setState({ projectIdError: error.errorDescription });
          } else {
            throw error;
          }
        });
    }
  };

  updateTimeEntryState = (value) => {
    let property = Object.keys(value)[0];
    let stateValue = _.values(value)[0];
    let number = property.match(/\d+/g)[0];
    let stateName = property.match(/[a-zA-Z]+/g)[0];
    let state = { [stateName]: stateValue };
    let updatedState = { ...this.state.timeEntry, [number]: { ...this.state.timeEntry[number], ...state } };
    this.setState({ timeEntry: updatedState });
  };

  didChange = () => {
    // todo
    return true;
  };

  isValid = () => {
    let timeEntry = { ...this.state.timeEntry };

    let timeEntryResetObj = timeEntry;
    Object.keys(timeEntry).forEach((key) => {
      let state = { ...timeEntry[key], errorHours: '', errorDate: '' };
      timeEntryResetObj[key] = state;
    });

    this.setState({ timeEntry: timeEntryResetObj });
    let valid = true;

    let timeEntryErrorsObj = timeEntry;
    Object.keys(timeEntry).forEach((key) => {
      if (isBlank(timeEntry[key].hours)) {
        let state = { ...timeEntry[key], errorHours: 'Hours are required' };
        timeEntryErrorsObj[key] = state;
        valid = false;
      }
      if (isBlank(timeEntry[key].date)) {
        let state = { ...timeEntry[key], errorDate: 'Date is required' };
        timeEntryErrorsObj[key] = state;
        valid = false;
      } else {
        var date = Moment.utc(timeEntry[key].date);
        if (this.isBeforeFiscalStartDate(date)) {
          let state = { ...timeEntry[key], errorDate: 'Date must be in the current fiscal year' };
          timeEntryErrorsObj[key] = state;
          valid = false;
        } else if (this.isInFuture(date)) {
          let state = { ...timeEntry[key], errorDate: 'Date must not be in the future' };
          timeEntryErrorsObj[key] = state;
          valid = false;
        } else if (!this.isSaturdayOrMarch31st(date)) {
          let state = { ...timeEntry[key], errorDate: 'Date must be a Saturday or March 31st' };
          timeEntryErrorsObj[key] = state;
          valid = false;
        }
        Object.keys(timeEntry).forEach((otherKey) => {
          if (key !== otherKey && timeEntry[key].date === timeEntry[otherKey].date) {
            let state = { ...timeEntry[key], errorDate: 'Time record for this date already exists' };
            timeEntryErrorsObj[key] = state;
            valid = false;
          }
        });
        Object.keys(this.props.rentalAgreementTimeRecords.timeRecords).forEach((index) => {
          var existingDate = Moment.utc(this.props.rentalAgreementTimeRecords.timeRecords[index].workedDate);
          if (date.isSame(existingDate)) {
            let state = { ...timeEntry[key], errorDate: 'Time record for this date already exists' };
            timeEntryErrorsObj[key] = state;
            valid = false;
          }
        });
      }
    });
    this.setState({ timeEntry: timeEntryErrorsObj });

    return valid;
  };

  formSubmitted = () => {
    if (this.isValid()) {
      if (this.didChange()) {
        this.setState({ isSaving: true });

        var timeEntry = { ...this.state.timeEntry };
        Object.keys(timeEntry).forEach((key) => {
          timeEntry[key].hours = (timeEntry[key].hours || 0).toFixed(2);
        });

        const promise = Api.addRentalAgreementTimeRecords(this.state.rentalAgreementId, timeEntry);

        promise.then(() => {
          this.setState({ isSaving: false });
          if (this.props.multipleEntryAllowed) {
            this.resetStateToSelectAgreement(true);
          } else {
            this.props.onClose();
          }
        });
      } else {
        this.props.onClose();
      }
    }
  };

  onClose = () => {
    if (this.props.multipleEntryAllowed) {
      this.resetStateToSelectAgreement(false);
    } else {
      this.props.onClose();
    }
  };

  onEquipmentSelected = () => {
    this.setState({ projectId: null });
  };

  getFilteredProjects = () => {
    const projects = this.props.projects.data;

    if (this.state.equipmentId) {
      var equipment = this.props.equipment.data[this.state.equipmentId] || null;
      if (equipment) {
        return _.intersectionWith(projects, equipment.projectIds, (p, pid) => p.id === pid);
      }
    }

    return projects;
  };

  addTimeEntryInput = () => {
    if (this.state.numberOfInputs < 10) {
      let numberOfInputs = Object.keys(this.state.timeEntry).length;
      this.setState({
        numberOfInputs: this.state.numberOfInputs + 1,
        timeEntry: {
          ...this.state.timeEntry,
          [numberOfInputs + 1]: {
            hours: '',
            date: '',
            errorHours: '',
            errorDate: '',
          },
        },
      });
    }
  };

  removeTimeEntryInput = () => {
    if (this.state.numberOfInputs > 1) {
      let numberOfInputs = Object.keys(this.state.timeEntry).length;
      let timeEntry = { ...this.state.timeEntry };
      delete timeEntry[numberOfInputs];
      this.setState({
        numberOfInputs: this.state.numberOfInputs - 1,
        timeEntry: timeEntry,
      });
    }
  };

  deleteTimeRecord = (timeRecord) => {
    Api.deleteTimeRecord(timeRecord.id).then(() => {
      Api.getRentalAgreementTimeRecords(this.state.rentalAgreementId);
    });
  };

  showAllTimeRecords = () => {
    this.setState({ showAllTimeRecords: !this.state.showAllTimeRecords });
  };

  getHoursYtdClassName = () => {
    var equipment = this.props.rentalAgreementTimeRecords;

    if (equipment.hoursYtd > 0.85 * equipment.maximumHours) {
      return true;
    }

    return false;
  };

  isBeforeFiscalStartDate = (date) => {
    return date.isBefore(Moment(this.state.projectFiscalYearStartDate));
  };

  isInFuture = (date) => {
    // The next Saturday is allowed but not after
    return date.isAfter(Moment().day(6));
  };

  isSaturdayOrMarch31st = (date) => {
    return date.day() === 6 || (date.month() === 2 && date.date() === 31);
  };

  isValidDate = (current) => {
    if (this.isBeforeFiscalStartDate(current)) {
      return false;
    }

    if (this.isInFuture(current)) {
      return false;
    }

    return this.isSaturdayOrMarch31st(current);
  };

  render() {
    if (this.state.selectingAgreement) {
      return this.renderSelectAgreement();
    } else {
      return this.renderEditDialog();
    }
  }

  renderSelectAgreement = () => {
    var equipment = _.sortBy(this.props.equipment.data, 'equipmentCode');
    var projects = this.getFilteredProjects();

    return (
      <FormDialog
        id="time-entry"
        show={this.props.show}
        title="HETS Time Entry"
        saveButtonLabel="Continue"
        isSaving={this.state.isSaving}
        onClose={this.props.onClose}
        onSubmit={this.selectAgreementFormSubmitted}
      >
        <Container fluid>
          <Row>
            <Col xs={6}>
              <FormGroup controlId="equipmentId">
                <FormLabel>
                  Equipment ID <sup>*</sup>
                </FormLabel>
                <FilterDropdown
                  id="equipmentId"
                  fieldName="equipmentCode"
                  disabled={!this.props.equipment.loaded}
                  selectedId={this.state.equipmentId}
                  onSelect={this.onEquipmentSelected}
                  updateState={this.updateState}
                  items={equipment}
                  isInvalid={this.state.equipmentIdError}
                />
                <FormText>{this.state.equipmentIdError}</FormText>
              </FormGroup>
            </Col>
            <Col xs={6}>
              <FormGroup controlId="projectId">
                <FormLabel>
                  Project <sup>*</sup>
                </FormLabel>
                <FilterDropdown
                  id="projectId"
                  fieldName="label"
                  disabled={!this.props.projects.loaded}
                  selectedId={this.state.projectId}
                  updateState={this.updateState}
                  items={projects}
                  isInvalid={this.state.projectIdError}
                />
                <FormText>{this.state.projectIdError}</FormText>
              </FormGroup>
            </Col>
          </Row>
        </Container>
      </FormDialog>
    );
  };

  renderTimeRecordItem = (timeRecord) => {
    return (
      <Row key={timeRecord.id}>
        <Col xs={3}>
          <div>{formatDateTime(timeRecord.workedDate, 'YYYY-MMM-DD')}</div>
        </Col>
        <Col xs={3}>
          <div>{formatHours(timeRecord.hours)}</div>
        </Col>
        <Col xs={6}>
          <DeleteButton name="Document" onConfirm={() => this.deleteTimeRecord(timeRecord)} />
        </Col>
      </Row>
    );
  };

  renderEditDialog = () => {
    const rentalAgreementTimeRecords = this.props.rentalAgreementTimeRecords;
    var sortedTimeRecords = _.sortBy(rentalAgreementTimeRecords.timeRecords, 'workedDate').reverse();

    return (
      <FormDialog
        id="time-entry"
        show={this.props.show}
        title="HETS Time Entry"
        closeButtonLabel={this.props.multipleEntryAllowed ? 'Back' : 'Close'}
        isSaving={this.state.isSaving}
        onClose={this.onClose}
        onSubmit={this.formSubmitted}
      >
        <Container fluid>
          {(() => {
            if (!this.state.loaded) {
              return (
                <div style={{ textAlign: 'center', minHeight: 160 }}>
                  <Spinner />
                </div>
              );
            }

            return (
              <div>
                <Row>
                  <Col xs={3}>
                    <div className="text-label">Equipment ID</div>
                    <div>{rentalAgreementTimeRecords.equipmentCode}</div>
                  </Col>
                  <Col xs={3}>
                    <div className="text-label">YTD Hours</div>
                    <div className={this.getHoursYtdClassName() ? 'highlight' : ''}>
                      {formatHours(rentalAgreementTimeRecords.hoursYtd)}
                      {this.getHoursYtdClassName()}
                    </div>
                  </Col>
                  <Col xs={3}>
                    <div className="text-label">Project</div>
                    <div>{rentalAgreementTimeRecords.projectName}</div>
                  </Col>
                  <Col xs={3}>
                    <div className="text-label">Project Number</div>
                    <div>{rentalAgreementTimeRecords.provincialProjectNumber}</div>
                  </Col>
                </Row>
                <div className="time-entries-container">
                  <Row>
                    <Col xs={3}>
                      <div className="column-title">Week Ending</div>
                    </Col>
                    <Col xs={3}>
                      <div className="column-title">Hours</div>
                    </Col>
                  </Row>
                  {sortedTimeRecords.length === 0 && (
                    <Row>
                      <Col xs={12}>
                        <div>No time records have been added yet.</div>
                      </Col>
                    </Row>
                  )}

                  {
                    sortedTimeRecords.length > 0 && !this.state.showAllTimeRecords
                      ? this.renderTimeRecordItem(sortedTimeRecords[0])
                      : _.map(sortedTimeRecords, (timeRecord, index) => this.renderTimeRecordItem(timeRecord))

                    // <Row>
                    //   <Col xs={12}>
                    //     <ul className="time-records-list">
                    //       {_.map(sortedTimeRecords, (timeRecord) => (
                    //         <li key={timeRecord.id} className="list-item">
                    //           {this.renderTimeRecordItem(timeRecord)}
                    //         </li>
                    //       ))}
                    //     </ul>
                    //   </Col>
                    // </Row>
                  }
                </div>
                {sortedTimeRecords.length > 1 && (
                  <Button className="btn-custom" onClick={this.showAllTimeRecords}>
                    {this.state.showAllTimeRecords ? 'Hide' : 'Show All'}
                  </Button>
                )}
              </div>
            );
          })()}
          <hr />
          {Object.keys(this.state.timeEntry).map((key) => {
            return (
              <Row key={key}>
                <Col sm={4}>
                  <FormGroup>
                    <FormLabel>Week Ending</FormLabel>
                    <DateControl
                      disabled={!this.state.projectFiscalYearStartDate}
                      id={`date${key}`}
                      name="date"
                      isValidDate={this.isValidDate}
                      date={this.state.timeEntry[key].date}
                      updateState={this.updateTimeEntryState}
                      isInvalid={this.state.timeEntry[key].errorDate}
                    />
                    <FormText>{this.state.timeEntry[key].errorDate}</FormText>
                  </FormGroup>
                </Col>
                <Col sm={4}>
                  <FormGroup>
                    <FormLabel>Hours</FormLabel>
                    <FormInputControl
                      id={`hours${key}`}
                      name="hours"
                      type="float"
                      value={this.state.timeEntry[key].hours}
                      updateState={this.updateTimeEntryState}
                      isInvalid={this.state.timeEntry[key].errorHours}
                    />
                    <FormText>{this.state.timeEntry[key].errorHours}</FormText>
                  </FormGroup>
                </Col>
              </Row>
            );
          })}
          <Row>
            <Col xs={12}>
              {this.state.numberOfInputs < 10 && (
                <Button className="btn-custom" size="sm" onClick={this.addTimeEntryInput}>
                  <FontAwesomeIcon icon="plus" />
                  &nbsp;<strong>Add</strong>
                </Button>
              )}
              {this.state.numberOfInputs > 1 && (
                <Button size="sm" className="btn-custom ml-3" onClick={this.removeTimeEntryInput}>
                  <FontAwesomeIcon icon="minus" />
                  &nbsp;<strong>Remove</strong>
                </Button>
              )}
            </Col>
          </Row>
        </Container>
      </FormDialog>
    );
  };
}

function mapStateToProps(state) {
  return {
    rentalAgreementTimeRecords: state.models.rentalAgreementTimeRecords,
    projects: state.lookups.projectsCurrentFiscal,
    equipment: state.lookups.equipment.ts,
  };
}

export default connect(mapStateToProps)(TimeEntryDialog);
