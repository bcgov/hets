import React from 'react';

import { connect } from 'react-redux';

import { FormGroup, ControlLabel, HelpBlock, Row, Col } from 'react-bootstrap';

import * as Constant from '../../constants';

import DateControl from '../../components/DateControl.jsx';
import EditDialog from '../../components/EditDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';
import Form from '../../components/Form.jsx';

import { isValidDate, toZuluTime } from '../../utils/date';
import { notBlank, isBlank } from '../../utils/string';

var OwnersPolicyEditDialog = React.createClass({
  propTypes: {
    owner: React.PropTypes.object,
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
  },

  getInitialState() {
    var owner = this.props.owner;
    return {
      workSafeBCPolicyNumber: owner.workSafeBCPolicyNumber || '',
      workSafeBCExpiryDate: owner.workSafeBCExpiryDate || '',
      cglCompanyName: owner.cglCompanyName || '',
      cglPolicyNumber: owner.cglPolicyNumber || '',
      cglEndDate: owner.cglEndDate || '',

      workSafeBCPolicyNumberError: '',
      workSafeBCExpiryDateError: '',
      cglEndDateError: '',
    };
  },

  componentDidMount() {
    this.input.focus();
  },

  updateState(state, callback) {
    this.setState(state, callback);
  },

  didChange() {
    var owner = this.props.owner;

    if (this.state.workSafeBCPolicyNumber !== owner.workSafeBCPolicyNumber) { return true; }
    if (this.state.workSafeBCExpiryDate !== owner.workSafeBCExpiryDate) { return true; }
    if (this.state.cglCompanyName !== owner.cglCompanyName) { return true; }
    if (this.state.cglPolicyNumber !== owner.cglPolicyNumber) { return true; }
    if (this.state.cglEndDate !== owner.cglEndDate) { return true; }

    return false;
  },

  isValid() {
    this.setState({
      workSafeBCPolicyNumberError: '',
      workSafeBCExpiryDateError: '',
      cglEndDateError: '',
    });

    var valid = true;

    if (isBlank(this.state.workSafeBCPolicyNumber)) {
      this.setState({ workSafeBCPolicyNumberError: 'WBC is required' });
      valid = false;
    }

    if (notBlank(this.state.workSafeBCExpiryDate) && !isValidDate(this.state.workSafeBCExpiryDate)) {
      this.setState({ workSafeBCExpiryDateError: 'Date not valid' });
      valid = false;
    }

    if (notBlank(this.state.cglEndDate) && !isValidDate(this.state.cglEndDate)) {
      this.setState({ cglEndDateError: 'Date not valid' });
      valid = false;
    }

    return valid;
  },

  onSave() {
    this.props.onSave({ ...this.props.owner, ...{
      workSafeBCPolicyNumber: this.state.workSafeBCPolicyNumber,
      workSafeBCExpiryDate: toZuluTime(this.state.workSafeBCExpiryDate),
      cglCompanyName: this.state.cglCompanyName,
      cglPolicyNumber: this.state.cglPolicyNumber,
      cglEndDate: toZuluTime(this.state.cglEndDate),
    }});
  },

  render() {
    console.log(this.props.owner);
    return <EditDialog id="owners-edit" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title={<strong>Owner Insurance</strong>}>
      <Form>
        <Row>
          <Col xs={6}>
            <FormGroup controlId="workSafeBCPolicyNumber" validationState={ this.state.workSafeBCPolicyNumberError ? 'error' : null }>
              <ControlLabel>WCB Number <sup>*</sup></ControlLabel>
              <FormInputControl type="text" value={ this.state.workSafeBCPolicyNumber } updateState={ this.updateState } inputRef={ ref => { this.input = ref; }} />
              <HelpBlock>{ this.state.workSafeBCPolicyNumberError }</HelpBlock>
            </FormGroup>
          </Col>
          <Col xs={6}>
            <FormGroup controlId="workSafeBCExpiryDate" validationState={ this.state.workSafeBCExpiryDateError ? 'error' : null }>
              <ControlLabel>WCB Expiry Date</ControlLabel>
              <DateControl id="workSafeBCExpiryDate" date={ this.state.workSafeBCExpiryDate } updateState={ this.updateState } />
              <HelpBlock>{ this.state.workSafeBCExpiryDateError }</HelpBlock>
            </FormGroup>
          </Col>
        </Row>
        <Row>
          <Col xs={12}>
            <FormGroup controlId="cglCompanyName">
              <ControlLabel>CGL Insurance Company</ControlLabel>
              <FormInputControl type="text" value={ this.state.cglCompanyName } updateState={ this.updateState } maxLength={ Constant.MAX_LENGTH_CGL_COMPANY_NAME } />
            </FormGroup>
          </Col>
        </Row>
        <Row>
          <Col xs={6}>
            <FormGroup controlId="cglPolicyNumber">
              <ControlLabel>CGL Policy Number</ControlLabel>
              <FormInputControl type="text" value={ this.state.cglPolicyNumber } updateState={ this.updateState } />
            </FormGroup>
          </Col>
          <Col xs={6}>
            <FormGroup controlId="cglEndDate" validationState={ this.state.cglEndDateError ? 'error' : null }>
              <ControlLabel>CGL Policy End Date</ControlLabel>
              <DateControl id="cglEndDate" date={ this.state.cglEndDate } updateState={ this.updateState } />
              <HelpBlock>{ this.state.cglEndDateError }</HelpBlock>
            </FormGroup>
          </Col>
        </Row>
      </Form>
    </EditDialog>;
  },
});

function mapStateToProps(state) {
  return {
    owner: state.models.owner,
  };
}

export default connect(mapStateToProps)(OwnersPolicyEditDialog);
