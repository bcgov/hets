import React from 'react';

import { connect } from 'react-redux';

import { Grid, Row, Col } from 'react-bootstrap';
import { Form, FormGroup, HelpBlock, ControlLabel } from 'react-bootstrap';

import Promise from 'bluebird';

import * as Api from '../../api';

import DateControl from '../../components/DateControl.jsx';
import EditDialog from '../../components/EditDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';
import Spinner from '../../components/Spinner.jsx';

import { isValidDate } from '../../utils/date';
import { isBlank, notBlank } from '../../utils/string';

var RentalAgreementsEditDialog = React.createClass({
  propTypes: {
    rentalAgreement: React.PropTypes.object.isRequired,
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
    owner: React.PropTypes.object,
  },

  getInitialState() {
    return {
      loading: true,
      estimateStartWork: this.props.rentalAgreement.estimateStartWork || '',
      estimateHours: this.props.rentalAgreement.estimateHours || 0,
      datedOn: this.props.rentalAgreement.datedOn || '',

      estimateStartWorkError: '',
      estimateHoursError: '',
      datedOnError: '',
    };
  },

  componentDidMount() {
    this.fetch();
  },

  fetch() {
    this.setState({ loading: true });
    Api.getOwner(this.props.rentalAgreement.ownerId).finally(() => {
      this.setState({ loading: false });
    });
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

  onSave() {
    // HACK Alert! this just ensures that the normalized data doesn't mess up the PUT call
    Promise.resolve({ ...this.props.owner, ...{
      contacts: null, // this just ensures that the normalized data doesn't mess up the PUT call
    }}).then(() => {
      this.props.onSave({ ...this.props.rentalAgreement, ...{
        estimateStartWork: this.state.estimateStartWork,
        estimateHours: this.state.estimateHours,
        datedOn: this.state.datedOn,
      }});
    });
  },

  render() {
    // Read-only if the user cannot edit the rental agreement
    var isReadOnly = !this.props.rentalAgreement.canEdit && this.props.rentalAgreement.id !== 0;

    return <EditDialog id="rental-agreements-edit" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title={
        <strong>Rental Agreement Details</strong>
      }>
      {(() => {
        if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

        return <Form>
          <Grid fluid>
            <Row>
              <Col md={6}>
                <FormGroup controlId="estimateStartWork" validationState={ this.state.estimateStartWorkError ? 'error' : null }>
                  <ControlLabel>Estimated Commencement</ControlLabel>
                  <DateControl id="estimateStartWork" disabled={ isReadOnly } date={ this.state.estimateStartWork } updateState={ this.updateState } placeholder="mm/dd/yyyy" title="Estimated Commencement" />
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
                  <ControlLabel>Dated At</ControlLabel>
                  <DateControl id="datedOn" disabled={ isReadOnly } date={ this.state.datedOn } updateState={ this.updateState } placeholder="mm/dd/yyyy" title="Dated At" />
                  <HelpBlock>{ this.state.datedOnError }</HelpBlock>
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
    rentalAgreement: state.models.rentalAgreement,
    owner: state.models.owner,
  };
}

export default connect(mapStateToProps)(RentalAgreementsEditDialog);
