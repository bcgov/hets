import React from 'react';

import { connect } from 'react-redux';

import { Form, FormGroup, ControlLabel, HelpBlock } from 'react-bootstrap';

import DateControl from '../../components/DateControl.jsx';
import EditDialog from '../../components/EditDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { isValidDate, toZuluTime } from '../../utils/date';
import { notBlank } from '../../utils/string';

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
      cglEndDate: owner.cglEndDate || '',
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
    if (this.state.cglEndDate !== owner.cglEndDate) { return true; }

    return false;
  },

  isValid() {
    this.setState({
      workSafeBCExpiryDateError: '',
      cglEndDateError: '',
    });

    var valid = true;

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
      cglEndDate: toZuluTime(this.state.cglEndDate),
    }});
  },

  render() {
    return <EditDialog id="owners-edit" show={ this.props.show } bsSize="small"
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title= {
        <strong>Owner Insurance</strong>
      }>
      <Form>
        <FormGroup controlId="workSafeBCPolicyNumber">
          <ControlLabel>WorkSafeBC Policy Number</ControlLabel>
          <FormInputControl type="text" value={ this.state.workSafeBCPolicyNumber } updateState={ this.updateState } inputRef={ ref => { this.input = ref; }} />
        </FormGroup>
        <FormGroup controlId="workSafeBCExpiryDate" validationState={ this.state.workSafeBCExpiryDateError ? 'error' : null }>
          <ControlLabel>WorkSafeBC Expiry Date</ControlLabel>
          <DateControl id="workSafeBCExpiryDate" date={ this.state.workSafeBCExpiryDate } updateState={ this.updateState } placeholder="mm/dd/yyyy" />
          <HelpBlock>{ this.state.workSafeBCExpiryDateError }</HelpBlock>
        </FormGroup>
        <FormGroup controlId="cglEndDate" validationState={ this.state.cglEndDateError ? 'error' : null }>
          <ControlLabel>CGL Policy End Date</ControlLabel>
          <DateControl id="cglEndDate" date={ this.state.cglEndDate } updateState={ this.updateState } placeholder="mm/dd/yyyy" />
          <HelpBlock>{ this.state.cglEndDateError }</HelpBlock>
        </FormGroup>
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
