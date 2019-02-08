import React from 'react';

import { connect } from 'react-redux';

import { Grid, Row, Col, Form, FormGroup, ControlLabel, Checkbox } from 'react-bootstrap';

import EditDialog from '../../components/EditDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import * as Constant from '../../constants';

var RentalAgreementOvertimeNotesDialog = React.createClass({
  propTypes: {
    rentalAgreement: React.PropTypes.object.isRequired,
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
  },

  getInitialState() {
    return {
      loading: false,
      overtimeRates: this.props.rentalAgreement.overtimeRates || {},
      ot1Rate: this.props.rentalAgreement.overtimeRates[0] || {},
      note: this.props.rentalAgreement.note || '',
    };
  },

  updateState(state, callback) {
    this.setState(state, callback);
  },

  didChange() {
    return true;
  },

  isValid() {
    return true;
  },

  onSave() {
    this.props.onSave({ ...this.props.rentalAgreement, ...{
      overtimeRates: this.state.overtimeRates,
      note: this.state.note,
    }});
  },

  overtimeCheckboxChanged(e) {
    var key = e.target.id.replace('overtime-', '');
    var active = e.target.checked;

    this.setState({ overtimeRates: { ...this.state.overtimeRates, [key]: { ...this.state.overtimeRates[key], active: active }}});
  },

  render() {
    return <EditDialog id="rental-agreements-overtime-notes-edit" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title={
        <strong>Overtime Rates and Notes/Special Instructions</strong>
      }>
      {(() => {
        var rates = this.state.overtimeRates;
        var overtimeCheckboxes = <div>
          {
            Object.keys(this.state.overtimeRates).map(key => (
              <Checkbox key={ key } className="checkbox-control" id={ `overtime-${key}` } checked={ rates[key].active } onChange={ this.overtimeCheckboxChanged }>
                { rates[key].comment }
              </Checkbox>
            ))
          }
        </div>;

        var maxNoteLength = Constant.MAX_LENGTH_RENTAL_AGREEMENT_NOTE;

        return <Form>
          <Grid fluid>
            <Row>
              <Col xs={12} id="overtime-rate-edit">
                <ControlLabel>Overtime Rates</ControlLabel>
                { overtimeCheckboxes }
              </Col>
              <Col xs={12} id="note-edit">
                <FormGroup controlId="note">
                  <ControlLabel>Notes/Special Instructions</ControlLabel>
                  <FormInputControl type="text" componentClass="textarea" rows="3" value={ this.state.note } updateState={ this.updateState } maxLength={ maxNoteLength } />
                  <p>Maximum { maxNoteLength } characters.</p>
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
  };
}

export default connect(mapStateToProps)(RentalAgreementOvertimeNotesDialog);
