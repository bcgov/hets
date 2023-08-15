import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { FormGroup, FormText, FormLabel, FormControl } from 'react-bootstrap';
import Moment from 'moment';

import * as Api from '../../api';
import * as Log from '../../history';

import DateControl from '../../components/DateControl.jsx';
import FormDialog from '../../components/FormDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { isValidDate } from '../../utils/date';
import { isBlank } from '../../utils/string';

class RentalRequestsEditDialog extends React.Component {
  static propTypes = {
    rentalRequest: PropTypes.object.isRequired,
    onSave: PropTypes.func.isRequired,
    onClose: PropTypes.func.isRequired,
    show: PropTypes.bool,
  };

  constructor(props) {
    super(props);

    const rentalRequest = props.rentalRequest;

    this.state = {
      equipmentCount: rentalRequest.equipmentCount || 0,
      expectedHours: rentalRequest.expectedHours || 0,
      expectedStartDate: rentalRequest.expectedStartDate || '',
      expectedEndDate: rentalRequest.expectedEndDate || '',
      rentalRequestAttachments:
        rentalRequest.rentalRequestAttachments && rentalRequest.rentalRequestAttachments[0]
          ? rentalRequest.rentalRequestAttachments[0].attachment
          : '',
      rentalRequestAttachmentId:
        rentalRequest.rentalRequestAttachments && rentalRequest.rentalRequestAttachments[0]
          ? rentalRequest.rentalRequestAttachments[0].id
          : undefined,

      equipmentCountError: '',
      expectedHoursError: '',
      expectedStartDateError: '',
      expectedEndDateError: '',
    };
  }

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  didChange = () => {
    if (this.state.equipmentCount !== this.props.rentalRequest.equipmentCount) {
      return true;
    }
    if (this.state.expectedHours !== this.props.rentalRequest.expectedHours) {
      return true;
    }
    if (this.state.expectedStartDate !== this.props.rentalRequest.expectedStartDate) {
      return true;
    }
    if (this.state.expectedEndDate !== this.props.rentalRequest.expectedEndDate) {
      return true;
    }
    if (this.state.rentalRequestAttachments !== this.props.rentalRequest.rentalRequestAttachments) {
      return true;
    }

    return false;
  };

  isValid = () => {
    this.setState({
      equipmentCountError: '',
      expectedHoursError: '',
      expectedStartDateError: '',
      expectedEndDateError: '',
    });

    var valid = true;

    if (isBlank(this.state.equipmentCount)) {
      this.setState({ equipmentCountError: 'Quantity is required' });
      valid = false;
    } else if (this.state.equipmentCount < 1) {
      this.setState({ equipmentCountError: 'Quantity not valid' });
      valid = false;
    } else if (this.state.equipmentCount < this.props.rentalRequest.yesCount) {
      this.setState({ equipmentCountError: 'Quantity can not be less than number of equipment already hired' });
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

  formSubmitted = async () => {
    const dispatch = this.props.dispatch;
    if (this.isValid()) {
      if (this.didChange()) {
        const rentalRequest = {
          ...this.props.rentalRequest,
          equipmentCount: this.state.equipmentCount,
          expectedHours: this.state.expectedHours,
          expectedStartDate: this.state.expectedStartDate,
          expectedEndDate: this.state.expectedEndDate,
          rentalRequestAttachments: [
            {
              id: this.state.rentalRequestAttachmentId,
              attachment: this.state.rentalRequestAttachments,
            },
          ],
        };

        await dispatch(Api.updateRentalRequest(rentalRequest));
        await dispatch(Log.rentalRequestModified(this.props.rentalRequest));
        if (this.props.onSave) {
          this.props.onSave();
        }
      }

      this.props.onClose();
    }
  };

  render() {
    // Read-only if the user cannot edit the rental agreement
    var isReadOnly = !this.props.rentalRequest.canEdit && this.props.rentalRequest.id !== 0;
    // var numRequestAttachments = Object.keys(this.props.rentalRequest.rentalRequestAttachments || []).length;
    // var requestAttachments = (this.props.rentalRequest.rentalRequestAttachments || []).join(', ');

    return (
      <FormDialog
        id="rental-requests-edit"
        show={this.props.show}
        onClose={this.props.onClose}
        onSubmit={this.formSubmitted}
        title="Rental Request"
      >
        <FormGroup>
          <FormLabel>Equipment Type</FormLabel>
          <FormControl placeholder={this.props.rentalRequest.equipmentTypeName} readOnly />
        </FormGroup>
        <FormGroup>
          <FormLabel>Attachment(s)</FormLabel>
          <FormInputControl
            id="rentalRequestAttachments"
            type="text"
            defaultValue={this.state.rentalRequestAttachments}
            readOnly={isReadOnly}
            updateState={this.updateState}
          />
        </FormGroup>
        <FormGroup controlId="equipmentCount">
          <FormLabel>
            Quantity <sup>*</sup>
          </FormLabel>
          <FormInputControl
            type="number"
            min={0}
            defaultValue={this.state.equipmentCount}
            readOnly={isReadOnly}
            updateState={this.updateState}
            isInvalid={this.state.equipmentCountError}
            autoFocus
          />
          <FormText>{this.state.equipmentCountError}</FormText>
        </FormGroup>
        <FormGroup controlId="expectedHours">
          <FormLabel>
            Expected Hours <sup>*</sup>
          </FormLabel>
          <FormInputControl
            type="number"
            min={0}
            defaultValue={this.state.expectedHours}
            readOnly={isReadOnly}
            updateState={this.updateState}
            isInvalid={this.state.expectedHoursError}
          />
          <FormText>{this.state.expectedHoursError}</FormText>
        </FormGroup>
        <FormGroup controlId="expectedStartDate">
          <FormLabel>
            Start Date <sup>*</sup>
          </FormLabel>
          <DateControl
            id="expectedStartDate"
            disabled={isReadOnly}
            date={this.state.expectedStartDate}
            updateState={this.updateState}
            title="Dated At"
            isInvalid={this.state.expectedStartDateError}
          />
          <FormText>{this.state.expectedStartDateError}</FormText>
        </FormGroup>
        <FormGroup controlId="expectedEndDate">
          <FormLabel>
            End Date <sup>*</sup>
          </FormLabel>
          <DateControl
            id="expectedEndDate"
            disabled={isReadOnly}
            date={this.state.expectedEndDate}
            updateState={this.updateState}
            title="Dated At"
            isInvalid={this.state.expectedEndDateError}
          />
          <FormText>{this.state.expectedEndDateError}</FormText>
        </FormGroup>
      </FormDialog>
    );
  }
}

const mapDispatchToProps = (dispatch) => ({ dispatch });

export default connect(null, mapDispatchToProps)(RentalRequestsEditDialog);
