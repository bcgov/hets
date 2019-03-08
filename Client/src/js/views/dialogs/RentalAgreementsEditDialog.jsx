import React from 'react';
import { Grid, Row, Col } from 'react-bootstrap';
import { FormGroup, HelpBlock, ControlLabel } from 'react-bootstrap';

import * as Api from '../../api';

import DateControl from '../../components/DateControl.jsx';
import FormDialog from '../../components/FormDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { isValidDate } from '../../utils/date';
import { isBlank, notBlank } from '../../utils/string';


var RentalAgreementsEditDialog = React.createClass({
  propTypes: {
    rentalAgreement: React.PropTypes.object.isRequired,
    show: React.PropTypes.bool,
    owner: React.PropTypes.object,
    onSave: React.PropTypes.func,
    onClose: React.PropTypes.func.isRequired,
  },

  getInitialState() {
    return {
      estimateStartWork: this.props.rentalAgreement.estimateStartWork || '',
      estimateHours: this.props.rentalAgreement.estimateHours || 0,
      datedOn: this.props.rentalAgreement.datedOn || '',
      agreementCity: this.props.rentalAgreement.agreementCity || '',

      estimateStartWorkError: '',
      estimateHoursError: '',
      datedOnError: '',
    };
  },

  updateState(state, callback) {
    this.setState(state, callback);
  },

  didChange() {
    if (this.state.estimateStartWork !== this.props.rentalAgreement.estimateStartWork) { return true; }
    if (this.state.estimateHours !== this.props.rentalAgreement.estimateHours) { return true; }
    if (this.state.datedOn !== this.props.rentalAgreement.datedOn) { return true; }
    if (this.state.note !== this.props.rentalAgreement.note) { return true; }

    return false;
  },

  isValid() {
    this.setState({
      estimateStartWorkError: '',
      estimateHoursError: '',
      datedOnError: '',
    });

    var valid = true;

    if (notBlank(this.state.estimateStartWork) && !isValidDate(this.state.estimateStartWork)) {
      this.setState({ estimateStartWorkError: 'Date not valid' });
      valid = false;
    }

    if (notBlank(this.state.datedOn) && !isValidDate(this.state.datedOn)) {
      this.setState({ datedOnError: 'Date not valid' });
      valid = false;
    }

    if (isBlank(this.state.estimateHours)) {
      this.setState({ estimateHoursError: 'Estimated hours are required' });
      valid = false;
    } else if (this.state.estimateHours < 1) {
      this.setState({ estimateHoursError: 'Estimated hours not valid' });
      valid = false;
    }

    return valid;
  },

  formSubmitted() {
    if (this.isValid()) {
      if (this.didChange()) {
        const rentalAgreement = {
          ...this.props.rentalAgreement,
          estimateStartWork: this.state.estimateStartWork,
          estimateHours: this.state.estimateHours,
          datedOn: this.state.datedOn,
          agreementCity: this.state.agreementCity,
        };

        Api.updateRentalAgreement(rentalAgreement).then(() => {
          if (this.props.onSave) { this.props.onSave(); }
        });
      }

      this.props.onClose();
    }
  },

  render() {
    // Read-only if the user cannot edit the rental agreement
    var isReadOnly = !this.props.rentalAgreement.canEdit && this.props.rentalAgreement.id !== 0;

    return (
      <FormDialog
        id="rental-agreements-edit"
        show={this.props.show}
        title="Rental Agreement Details"
        onSubmit={this.formSubmitted}
        onClose={this.props.onClose}>
        <Grid fluid>
          <Row>
            <Col md={6}>
              <FormGroup controlId="estimateStartWork" validationState={ this.state.estimateStartWorkError ? 'error' : null }>
                <ControlLabel>Estimated Commencement</ControlLabel>
                <DateControl id="estimateStartWork" disabled={ isReadOnly } date={ this.state.estimateStartWork } updateState={ this.updateState } title="Estimated Commencement" />
                <HelpBlock>{ this.state.estimateStartWorkError }</HelpBlock>
              </FormGroup>
            </Col>
            <Col md={6}>
              <FormGroup controlId="estimateHours" validationState={ this.state.estimateHoursError ? 'error' : null }>
                <ControlLabel>Estimated Period Hours <sup>*</sup></ControlLabel>
                <FormInputControl type="number" min={0} defaultValue={ this.state.estimateHours } readOnly={ isReadOnly } updateState={ this.updateState }/>
                <HelpBlock>{ this.state.estimateHoursError }</HelpBlock>
              </FormGroup>
            </Col>
          </Row>
          <Row>
            <Col md={6}>
              <FormGroup controlId="datedOn" validationState={ this.state.datedOnError ? 'error' : null }>
                <ControlLabel>Dated On</ControlLabel>
                <DateControl id="datedOn" disabled={ isReadOnly } date={ this.state.datedOn } updateState={ this.updateState } title="Dated On" />
                <HelpBlock>{ this.state.datedOnError }</HelpBlock>
              </FormGroup>
            </Col>
            <Col md={6}>
              <FormGroup controlId="agreementCity">
                <ControlLabel>Dated At</ControlLabel>
                <FormInputControl type="text" value={ this.state.agreementCity } updateState={ this.updateState } />
              </FormGroup>
            </Col>
          </Row>
        </Grid>
      </FormDialog>
    );
  },
});

export default RentalAgreementsEditDialog;
