import React from 'react';

import { connect } from 'react-redux';

import { Grid, Row, Col } from 'react-bootstrap';
import { Form, FormGroup, HelpBlock, ControlLabel, FormControl } from 'react-bootstrap';

import Moment from 'moment';

import DateControl from '../../components/DateControl.jsx';
import EditDialog from '../../components/EditDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { isValidDate } from '../../utils/date';
import { isBlank } from '../../utils/string';

var RentalRequestsEditDialog = React.createClass({
  propTypes: {
    rentalRequest: React.PropTypes.object.isRequired,
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
  },

  getInitialState() {
    const rentalRequest = this.props.rentalRequest;
    return {
      equipmentCount: rentalRequest.equipmentCount || 0,
      expectedHours: rentalRequest.expectedHours || 0,
      expectedStartDate: rentalRequest.expectedStartDate || '',
      expectedEndDate: rentalRequest.expectedEndDate || '',
      rentalRequestAttachments: rentalRequest.attachments && rentalRequest.attachments[0] ? rentalRequest.attachments[0].description : '',

      equipmentCountError: '',
      expectedHoursError: '',
      expectedStartDateError: '',
      expectedEndDateError: '',
    };
  },

  componentDidMount() {
    this.input && this.input.focus();
  },

  updateState(state, callback) {
    this.setState(state, callback);
  },

  didChange() {
    if (this.state.equipmentCount !== this.props.rentalRequest.equipmentCount) { return true; }
    if (this.state.expectedHours !== this.props.rentalRequest.expectedHours) { return true; }
    if (this.state.expectedStartDate !== this.props.rentalRequest.expectedStartDate) { return true; }
    if (this.state.expectedEndDate !== this.props.rentalRequest.expectedEndDate) { return true; }
    if (this.state.rentalRequestAttachments !== this.props.rentalRequest.rentalRequestAttachments) { return true; }

    return false;
  },

  isValid() {
    this.setState({
      equipmentCountError: '',
      expectedHoursError: '',
      expectedStartDateError: '',
      expectedEndDateError: '',
    });

    var valid = true;

    if (isBlank(this.state.equipmentCount)) {
      this.setState({ equipmentCountError: 'Equipment count is required' });
      valid = false;
    } else if (this.state.equipmentCount < 1) {
      this.setState({ equipmentCountError: 'Equipment count not valid' });
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

    if (isBlank(this.state.expectedEndDate)) {
      this.setState({ expectedEndDateError: 'End date is required' });
      valid = false;
    } else if (!isValidDate(this.state.expectedEndDate)) {
      this.setState({ expectedEndDateError: 'Date not valid' });
      valid = false;
    }

    if (valid && Moment(this.state.expectedEndDate).isBefore(this.state.expectedStartDate)) {
      this.setState({ expectedEndDateError: 'End date must be later than the start date' });
      valid = false;
    }

    return valid;
  },

  onSave() {
    this.props.onSave({ ...this.props.rentalRequest, ...{
      equipmentCount: this.state.equipmentCount,
      expectedHours: this.state.expectedHours,
      expectedStartDate: this.state.expectedStartDate,
      expectedEndDate: this.state.expectedEndDate,
      attachments: [{ description: this.state.rentalRequestAttachments }],
    }});
  },

  render() {
    // Read-only if the user cannot edit the rental agreement
    var isReadOnly = !this.props.rentalRequest.canEdit && this.props.rentalRequest.id !== 0;
    // var numRequestAttachments = Object.keys(this.props.rentalRequest.rentalRequestAttachments || []).length;
    // var requestAttachments = (this.props.rentalRequest.rentalRequestAttachments || []).join(', ');

    return <EditDialog id="rental-requests-edit" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title={
        <strong>Rental Request</strong>
      }>
      {(() => {
        return <Form>
          <Grid fluid>
            <Row>
              <Col md={6}>
                <FormGroup>
                  <ControlLabel>Equipment Type</ControlLabel>
                  <FormControl.Static>{ this.props.rentalRequest.equipmentTypeName }</FormControl.Static>
                </FormGroup>
              </Col>
              <Col md={6}>
                <FormGroup>
                  <ControlLabel>Attachment(s)</ControlLabel>
                  {/* <FormControl.Static>{ numRequestAttachments > 0 ? requestAttachments : 'None' }</FormControl.Static> */}
                  <FormInputControl id="rentalRequestAttachments" type="text" defaultValue={ this.state.rentalRequestAttachments } readOnly={ isReadOnly } updateState={ this.updateState } />
                </FormGroup>
              </Col>
            </Row>
            <Row>
              <Col md={6}>
                <FormGroup controlId="equipmentCount" validationState={ this.state.equipmentCountError ? 'error' : null }>
                  <ControlLabel>Count <sup>*</sup></ControlLabel>
                  <FormInputControl type="number" min={0} defaultValue={ this.state.equipmentCount } readOnly={ isReadOnly } updateState={ this.updateState } inputRef={ ref => { this.input = ref; }}/>
                  <HelpBlock>{ this.state.equipmentCountError }</HelpBlock>
                </FormGroup>
              </Col>
              <Col md={6}>
                <FormGroup controlId="expectedHours" validationState={ this.state.expectedHoursError ? 'error' : null }>
                  <ControlLabel>Expected Hours <sup>*</sup></ControlLabel>
                  <FormInputControl type="number" className="full-width" min={0} defaultValue={ this.state.expectedHours } readOnly={ isReadOnly } updateState={ this.updateState }/>
                  <HelpBlock>{ this.state.expectedHoursError }</HelpBlock>
                </FormGroup>
              </Col>
            </Row>
            <Row>
              <Col md={6}>
                <FormGroup controlId="expectedStartDate" validationState={ this.state.expectedStartDateError ? 'error' : null }>
                  <ControlLabel>Start Date <sup>*</sup></ControlLabel>
                  <DateControl id="expectedStartDate" disabled={ isReadOnly } date={ this.state.expectedStartDate } updateState={ this.updateState } placeholder="mm/dd/yyyy" title="Dated At" />
                  <HelpBlock>{ this.state.expectedStartDateError }</HelpBlock>
                </FormGroup>
              </Col>
              <Col md={6}>
                <FormGroup controlId="expectedEndDate" validationState={ this.state.expectedEndDateError ? 'error' : null }>
                  <ControlLabel>End Date <sup>*</sup></ControlLabel>
                  <DateControl id="expectedEndDate" disabled={ isReadOnly } date={ this.state.expectedEndDate } updateState={ this.updateState } placeholder="mm/dd/yyyy" title="Dated At" />
                  <HelpBlock>{ this.state.expectedEndDateError }</HelpBlock>
                </FormGroup>
              </Col>
            </Row>
          </Grid>
        </Form>;
      })()}
    </EditDialog>;
  },
});

function mapStateToProps(state) {
  return {
    rentalRequest: state.models.rentalRequest.data,
  };
}

export default connect(mapStateToProps)(RentalRequestsEditDialog);
