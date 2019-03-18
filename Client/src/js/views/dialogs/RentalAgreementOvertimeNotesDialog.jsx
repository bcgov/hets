import React from 'react';
import { Grid, Row, Col, FormGroup, ControlLabel, Checkbox } from 'react-bootstrap';
import _ from 'lodash';

import * as Constant from '../../constants';
import * as Api from '../../api';

import FormDialog from '../../components/FormDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';
import { findAndUpdate } from '../../utils/array';


class RentalAgreementOvertimeNotesDialog extends React.Component {
  static propTypes = {
    show: React.PropTypes.bool.isRequired,
    rentalAgreement: React.PropTypes.object.isRequired,
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
  };

  constructor(props) {
    super(props);

    this.state = {
      loading: false,
      overtimeRates: props.rentalAgreement.overtimeRates || [],
      note: props.rentalAgreement.note || '',
    };
  }

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  didChange = () => {
    return true;
  };

  isValid = () => {
    return true;
  };

  formSubmitted = () => {
    const { onSave, onClose } = this.props;
    if (this.isValid()) {
      if (this.didChange()) {
        const rentalAgreement = {
          ...this.props.rentalAgreement,
          overtimeRates: this.state.overtimeRates,
          note: this.state.note,
        };

        Api.updateRentalAgreement(rentalAgreement).then(() => {
          if (onSave) { onSave(); }
        });
      }

      onClose();
    }
  };

  overtimeCheckboxChanged = (rate, e) => {
    var active = e.target.checked;

    const overtimeRates = this.state.overtimeRates.slice();
    findAndUpdate(overtimeRates, { ...rate, active });

    this.setState({ overtimeRates });
  };

  render() {
    const maxNoteLength = Constant.MAX_LENGTH_RENTAL_AGREEMENT_NOTE;
    const rates = _.orderBy(this.state.overtimeRates, ['rate']);

    return (
      <FormDialog
        id="rental-agreements-overtime-notes-edit"
        show={this.props.show}
        onSubmit={this.formSubmitted}
        onClose={this.props.onClose}
        title="Overtime Rates and Notes/Special Instructions">
        <Grid fluid>
          <Row>
            <Col xs={12} id="overtime-rate-edit">
              <ControlLabel>Overtime Rates</ControlLabel>
              <div>
                {
                  rates.map((rate) => (
                    <Checkbox key={rate.id} className="checkbox-control" id={ `overtime-${rate.id}` } checked={ rate.active } onChange={ (e) => this.overtimeCheckboxChanged(rate, e) }>
                      { rate.comment }
                    </Checkbox>
                  ))
                }
              </div>
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
      </FormDialog>
    );
  }
}

export default RentalAgreementOvertimeNotesDialog;
