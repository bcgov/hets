import React from 'react';

import { connect } from 'react-redux';

import { Grid, Row, Col } from 'react-bootstrap';
import { Form, FormGroup, HelpBlock, ControlLabel } from 'react-bootstrap';

import EditDialog from '../../components/EditDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { isBlank } from '../../utils/string';

var SeniorityEditDialog = React.createClass({
  propTypes: {
    equipment: React.PropTypes.object,

    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
  },

  getInitialState() {
    // TODO Make all fields required. Add error messages
    return {
      isNew: this.props.equipment.id === 0,

      ytd: this.props.equipment.ytd || 0,
      serviceHoursLastYear: this.props.equipment.serviceHoursLastYear || 0,
      serviceHoursTwoYearsAgo: this.props.equipment.serviceHoursTwoYearsAgo || 0,
      serviceHoursThreeYearsAgo: this.props.equipment.serviceHoursThreeYearsAgo || 0,
      numYears: this.props.equipment.numYears || 0,

      ytdError: null,
    };
  },

  componentDidMount() {
    this.input.focus();
  },

  updateState(state, callback) {
    this.setState(state, callback);
  },

  didChange() {
    if (this.state.ytd !== this.props.equipment.ytd) { return true; }
    if (this.state.serviceHoursLastYear !== this.props.equipment.serviceHoursLastYear) { return true; }
    if (this.state.serviceHoursTwoYearsAgo !== this.props.equipment.serviceHoursTwoYearsAgo) { return true; }
    if (this.state.serviceHoursThreeYearsAgo !== this.props.equipment.serviceHoursThreeYearsAgo) { return true; }
    if (this.state.numYears !== this.props.equipment.numYears) { return true; }

    return false;
  },

  isValid() {
    this.setState({
      ytdError: null,
    });

    var valid = true;

    if (isBlank(this.state.ytd)) {
      this.setState({ ytdError: 'Hour for current fiscal year is required' });
      valid = false;
    }

    return valid;
  },

  onSave() {
    this.props.onSave({ ...this.props.equipment, ...{
      ytd: this.state.ytd,
      serviceHoursLastYear: this.state.serviceHoursLastYear,
      serviceHoursTwoYearsAgo: this.state.serviceHoursTwoYearsAgo,
      serviceHoursThreeYearsAgo: this.state.serviceHoursThreeYearsAgo,
      numYears: this.state.numYears,
    }});
  },

  render() {
    return <EditDialog id="seniority-edit" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title= { 
        <strong>Equipment
          <span>Serial Number: <small>{ this.props.equipment.serialNum }</small></span>
          <span>Plate: <small>{ this.props.equipment.licencePlate }</small></span>
        </strong>
      }>
      {(() => {
        return <Form>
          <Grid fluid cols={6}>
            <Row>
              <Col>
                <FormGroup controlId="ytd" validationState={ this.state.ytdError ? 'error' : null }>
                  <ControlLabel>Hours YTD <sup>*</sup></ControlLabel>
                  <FormInputControl type="number" defaultValue={ this.state.ytd } updateState={ this.updateState } inputRef={ ref => { this.input = ref; }}/>
                  <HelpBlock>{ this.state.ytdError }</HelpBlock>
                </FormGroup>
              </Col>
            </Row>
            <Row>
              <Col>
                <FormGroup controlId="serviceHoursLastYear">
                  <ControlLabel>Hours { this.props.equipment.lastYear }</ControlLabel>
                  <FormInputControl type="number" defaultValue={ this.state.serviceHoursLastYear } updateState={ this.updateState }/>                  
                </FormGroup>
              </Col>
            </Row>
            <Row>
              <Col>
                <FormGroup controlId="serviceHoursTwoYearsAgo">
                  <ControlLabel>Hours { this.props.equipment.twoYearsAgo }</ControlLabel>
                  <FormInputControl type="number" defaultValue={ this.state.serviceHoursTwoYearsAgo } updateState={ this.updateState }/>                  
                </FormGroup>
              </Col>
            </Row>
            <Row>
              <Col>
                <FormGroup controlId="serviceHoursThreeYearsAgo">
                  <ControlLabel>Hours { this.props.equipment.threeYearsAgo }</ControlLabel>
                  <FormInputControl type="number" defaultValue={ this.state.serviceHoursThreeYearsAgo } updateState={ this.updateState }/>                  
                </FormGroup>
              </Col>
            </Row>
            <Row>
              <Col>
                <FormGroup controlId="numYears">
                  <ControlLabel>Years Registered</ControlLabel>
                  <FormInputControl type="number" defaultValue={ this.state.numYears } updateState={ this.updateState }/>                  
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
