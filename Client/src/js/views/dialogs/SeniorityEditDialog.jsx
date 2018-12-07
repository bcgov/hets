import React from 'react';

import { connect } from 'react-redux';

import { Grid, Row, Col } from 'react-bootstrap';
import { FormControl, Form, FormGroup, HelpBlock, ControlLabel } from 'react-bootstrap';

import DateControl from '../../components/DateControl.jsx';
import EditDialog from '../../components/EditDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { daysFromToday, isValidDate, toZuluTime, today } from '../../utils/date';
import { isBlank } from '../../utils/string';

var SeniorityEditDialog = React.createClass({
  propTypes: {
    equipment: React.PropTypes.object,

    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
  },

  getInitialState() {
    return {
      isNew: this.props.equipment.id === 0,

      serviceHoursThisYear: this.props.equipment.serviceHoursThisYear,
      serviceHoursLastYear: this.props.equipment.serviceHoursLastYear,
      serviceHoursTwoYearsAgo: this.props.equipment.serviceHoursTwoYearsAgo,
      serviceHoursThreeYearsAgo: this.props.equipment.serviceHoursThreeYearsAgo,
      seniorityEffectiveDate: this.props.equipment.seniorityEffectiveDate || today(),
      yearsRegistered: this.props.equipment.yearsOfService,
      isSeniorityOverridden: this.props.equipment.isSeniorityOverridden,
      seniorityOverrideReason: this.props.equipment.seniorityOverrideReason,

      seniorityDateError: null,
      serviceHoursLastYearError: null,
      serviceHoursTwoYearsAgoError: null,
      serviceHoursThreeYearsAgoError: null,
      yearsRegisteredError: null,
      overrideReasonError: null,
    };
  },

  updateState(state, callback) {
    this.setState(state, callback);
  },

  didChange() {
    if (this.state.serviceHoursLastYear !== this.props.equipment.serviceHoursLastYear) { return true; }
    if (this.state.serviceHoursTwoYearsAgo !== this.props.equipment.serviceHoursTwoYearsAgo) { return true; }
    if (this.state.serviceHoursThreeYearsAgo !== this.props.equipment.serviceHoursThreeYearsAgo) { return true; }
    if (this.state.seniorityEffectiveDate !== this.props.equipment.seniorityEffectiveDate) { return true; }
    if (this.state.yearsRegistered !== this.props.equipment.yearsRegistered) { return true; }
    if (this.state.isSeniorityOverridden !== this.props.equipment.isSeniorityOverridden) { return true; }
    if (this.state.seniorityOverrideReason !== this.props.equipment.seniorityOverrideReason) { return true; }

    return false;
  },

  isValid() {
    this.setState({
      seniorityDateError: null,
      serviceHoursLastYearError: null,
      serviceHoursTwoYearsAgoError: null,
      serviceHoursThreeYearsAgoError: null,
      yearsRegisteredError: null,
      overrideReasonError: null,
    });

    var valid = true;
    // Validate service hours
    if (isBlank(this.state.serviceHoursLastYear)) {
      this.setState({ serviceHoursLastYearError: 'Service hours are required' });
      valid = false;
    }
    
    if (isBlank(this.state.serviceHoursTwoYearsAgo)) {
      this.setState({ serviceHoursTwoYearsAgoError: 'Service hours are required' });
      valid = false;
    }
    
    if (isBlank(this.state.serviceHoursThreeYearsAgo)) {
      this.setState({ serviceHoursThreeYearsAgoError: 'Service hours are required' });
      valid = false;
    }
    
    // Validate registered date
    if (isBlank(this.state.seniorityEffectiveDate)) {
      this.setState({ seniorityDateError: 'Registered date is required' });
      valid = false;
    } else if (!isValidDate(this.state.seniorityEffectiveDate)) {
      this.setState({ seniorityDateError: 'Registered date not valid' });
      valid = false;
    } else if (daysFromToday(this.state.seniorityEffectiveDate) > 0) {
      this.setState({ seniorityDateError: 'Registration date must be today or earlier' });
      valid = false;
    }
    
    if (this.didChange() && (isBlank(this.state.seniorityOverrideReason) || (this.state.seniorityOverrideReason === this.props.equipment.seniorityOverrideReason))) {
      this.setState({ overrideReasonError: 'A new reason must be provided each time seniority is manually overriden' });
      valid = false;
    }

    if (this.state.yearsRegistered < 0) {
      this.setState({ yearsRegisteredError: 'Years registered must be an integer greater than 0.' });
      valid = false;
    }

    return valid;
  },

  serviceHoursChanged() {
    this.setState({ isSeniorityOverridden: true });
  },

  onSave() {
    this.props.onSave({ ...this.props.equipment, ...{
      serviceHoursLastYear: this.state.serviceHoursLastYear,
      serviceHoursTwoYearsAgo: this.state.serviceHoursTwoYearsAgo,
      serviceHoursThreeYearsAgo: this.state.serviceHoursThreeYearsAgo,
      seniorityEffectiveDate: toZuluTime(this.state.seniorityEffectiveDate),
      yearsOfService: this.state.yearsRegistered,
      isSeniorityOverridden: this.state.isSeniorityOverridden,
      seniorityOverrideReason: this.state.seniorityOverrideReason,
    }});
  },

  render() {
    return <EditDialog id="seniority-edit" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title= {
        <strong>Equipment Id: <small>{ this.props.equipment.equipmentCode }</small></strong>
      }>
      {(() => {
        return <Form>
          <Grid fluid cols={6}>
            <Row>
              <Col>
                <FormGroup>
                  <ControlLabel>Hours YTD</ControlLabel>
                  <FormControl.Static>{ this.state.serviceHoursThisYear }</FormControl.Static>
                </FormGroup>
              </Col>
            </Row>
            <Row>
              <Col>
                <FormGroup controlId="serviceHoursLastYear" validationState={ this.state.serviceHoursLastYearError ? 'error' : null }>
                  <ControlLabel>Hours { this.props.equipment.yearMinus1 } <sup>*</sup></ControlLabel>
                  <FormInputControl type="number" value={ this.state.serviceHoursLastYear } onChange={ this.serviceHoursChanged } updateState={ this.updateState } inputRef={ ref => { this.input = ref; }}/>
                  <HelpBlock>{ this.state.serviceHoursLastYearError }</HelpBlock>
                </FormGroup>
              </Col>
            </Row>
            <Row>
              <Col>
                <FormGroup controlId="serviceHoursTwoYearsAgo" validationState={ this.state.serviceHoursTwoYearsAgoError ? 'error' : null }>
                  <ControlLabel>Hours { this.props.equipment.yearMinus2 } <sup>*</sup></ControlLabel>
                  <FormInputControl type="number" value={ this.state.serviceHoursTwoYearsAgo } onChange={ this.serviceHoursChanged } updateState={ this.updateState }/>
                  <HelpBlock>{ this.state.serviceHoursTwoYearsAgoError }</HelpBlock>
                </FormGroup>
              </Col>
            </Row>
            <Row>
              <Col>
                <FormGroup controlId="serviceHoursThreeYearsAgo" validationState={ this.state.serviceHoursThreeYearsAgoError ? 'error' : null }>
                  <ControlLabel>Hours { this.props.equipment.yearMinus3 } <sup>*</sup></ControlLabel>
                  <FormInputControl type="number" value={ this.state.serviceHoursThreeYearsAgo } onChange={ this.serviceHoursChanged } updateState={ this.updateState }/>
                  <HelpBlock>{ this.state.serviceHoursThreeYearsAgoError }</HelpBlock>
                </FormGroup>
              </Col>
            </Row>
            <Row>
              <Col>
                <FormGroup validationState={ this.state.seniorityDateError ? 'error' : null }>
                  <ControlLabel>Registered Date <sup>*</sup></ControlLabel>
                  <DateControl id="seniorityEffectiveDate" date={ this.state.seniorityEffectiveDate } updateState={ this.updateState } title="registered date"/>
                  <HelpBlock>{ this.state.seniorityDateError }</HelpBlock>
                </FormGroup>
              </Col>
            </Row>
            <Row>
              <Col>
                <FormGroup controlId="yearsRegistered" validationState={ this.state.yearsRegisteredError ? 'error' : null }>
                  <ControlLabel>Years Registered</ControlLabel>
                  <FormInputControl type="float" value={ this.state.yearsRegistered } updateState={ this.updateState } />
                  <HelpBlock>{ this.state.yearsRegisteredError }</HelpBlock>
                </FormGroup>
              </Col>
            </Row>
            <Row>
              <Col>
                <FormGroup controlId="seniorityOverrideReason" validationState={ this.state.overrideReasonError ? 'error' : null }>
                  <ControlLabel>Override Reason <sup>*</sup></ControlLabel>
                  <FormInputControl type="text" value={ this.state.seniorityOverrideReason } updateState={ this.updateState }/>
                  <HelpBlock>{ this.state.overrideReasonError }</HelpBlock>
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
    equipment: state.models.equipment,
  };
}

export default connect(mapStateToProps)(SeniorityEditDialog);
