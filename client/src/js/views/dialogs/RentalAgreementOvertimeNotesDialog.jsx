import PropTypes from 'prop-types';
import React from 'react';
import { Grid, Row, Col, FormGroup, ControlLabel } from 'react-bootstrap';
import _ from 'lodash';

import * as Constant from '../../constants';
import * as Api from '../../api';

import CheckboxControl from '../../components/CheckboxControl.jsx';
import FormDialog from '../../components/FormDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';


class RentalAgreementOvertimeNotesDialog extends React.Component {
  static propTypes = {
    show: PropTypes.bool.isRequired,
    rentalAgreement: PropTypes.object.isRequired,
    onSave: PropTypes.func.isRequired,
    onClose: PropTypes.func.isRequired,
  };

  constructor(props) {
    super(props);

    this.state = {
      loading: false,
      overtime: _.some(props.rentalAgreement.overtimeRates, 'active'),
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

  overtimeCheckboxChanged = (e) => {
    var active = e.target.checked;

    var overtimeRates = _.map(this.state.overtimeRates, rate => ({ ...rate, active: active }));

    this.setState({ overtimeRates: overtimeRates });
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
                <CheckboxControl id="overtime" checked={ this.state.overtime } updateState={ this.updateState } onChange={ this.overtimeCheckboxChanged }>
                  {
                    _.map(rates, rate => rate.comment).join(', ')
                  }
                </CheckboxControl>
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
