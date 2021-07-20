import PropTypes from 'prop-types';
import React from 'react';
import { Container, Row, Col } from 'react-bootstrap';
import { FormGroup, FormText, FormLabel } from 'react-bootstrap';

import * as Constant from '../../constants';
import * as Api from '../../api';

import DropdownControl from '../../components/DropdownControl.jsx';
import FormDialog from '../../components/FormDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { isBlank } from '../../utils/string';

class EquipmentRentalRatesEditDialog extends React.Component {
  static propTypes = {
    show: PropTypes.bool,
    rentalAgreement: PropTypes.object.isRequired,
    onSave: PropTypes.func,
    onClose: PropTypes.func.isRequired,
  };

  constructor(props) {
    super(props);

    this.state = {
      equipmentRate: props.rentalAgreement.equipmentRate || 0,
      ratePeriod: props.rentalAgreement.ratePeriod || Constant.RENTAL_RATE_PERIOD_HOURLY,
      rateComment: props.rentalAgreement.rateComment || '',

      equipmentRateError: '',
      ratePeriodError: '',
    };
  }

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  didChange = () => {
    if (this.state.equipmentRate !== this.props.rentalAgreement.equipmentRate) {
      return true;
    }
    if (this.state.ratePeriod !== this.props.rentalAgreement.ratePeriod) {
      return true;
    }
    if (this.state.rateComment !== this.props.rentalAgreement.rateComment) {
      return true;
    }

    return false;
  };

  isValid = () => {
    this.setState({
      equipmentRateError: '',
      ratePeriodError: '',
    });

    var valid = true;

    if (isBlank(this.state.equipmentRate)) {
      this.setState({ equipmentRateError: 'Pay rate is required' });
      valid = false;
    } else if (this.state.equipmentRate < 0) {
      this.setState({ equipmentRateError: 'Pay rate not valid' });
      valid = false;
    }

    if (isBlank(this.state.ratePeriod)) {
      this.setState({ ratePeriodError: 'Period is required' });
      valid = false;
    }

    return valid;
  };

  formSubmitted = () => {
    if (this.isValid()) {
      if (this.didChange()) {
        const rentalAgreement = {
          ...this.props.rentalAgreement,
          equipmentRate: this.state.equipmentRate,
          ratePeriod: this.state.ratePeriod,
          rateComment: this.state.rateComment,
        };

        Api.updateRentalAgreement(rentalAgreement).then(() => {
          if (this.props.onSave) {
            this.props.onSave();
          }
        });
      }

      this.props.onClose();
    }
  };

  render() {
    // Read-only if the user cannot edit the rental agreement
    var isReadOnly = !this.props.rentalAgreement.canEdit && this.props.rentalAgreement.id !== 0;
    var ratePeriods = [
      Constant.RENTAL_RATE_PERIOD_HOURLY,
      Constant.RENTAL_RATE_PERIOD_DAILY,
      Constant.RENTAL_RATE_PERIOD_WEEKLY,
      Constant.RENTAL_RATE_PERIOD_MONTHLY,
      Constant.RENTAL_RATE_PERIOD_NEGOTIATED,
    ];

    return (
      <FormDialog
        id="rental-agreements-edit"
        show={this.props.show}
        title="Rental Agreement"
        onSubmit={this.formSubmitted}
        onClose={this.props.onClose}
      >
        <Container fluid>
          <Row>
            <Col md={3}>
              <FormGroup controlId="equipmentRate">
                <FormLabel>
                  Pay Rate <sup>*</sup>
                </FormLabel>
                <FormInputControl
                  type="float"
                  min={0}
                  defaultValue={(this.state.equipmentRate || 0).toFixed(2)}
                  readOnly={isReadOnly}
                  updateState={this.updateState}
                  autoFocus
                  isInvalid={this.state.equipmentRateError}
                />
                <FormText>{this.state.equipmentRateError}</FormText>
              </FormGroup>
            </Col>
            <Col md={3}>
              <FormGroup controlId="ratePeriod">
                <FormLabel>
                  Period <sup>*</sup>
                </FormLabel>
                <DropdownControl
                  id="ratePeriod"
                  disabled={isReadOnly}
                  title={this.state.ratePeriod}
                  updateState={this.updateState}
                  items={ratePeriods}
                  isInvalid={this.state.ratePeriodError}
                />
                <FormText>{this.state.ratePeriodError}</FormText>
              </FormGroup>
            </Col>
            <Col md={6}>
              <FormGroup controlId="rateComment">
                <FormLabel>Comment</FormLabel>
                <FormInputControl
                  defaultValue={this.state.rateComment}
                  readOnly={isReadOnly}
                  updateState={this.updateState}
                />
              </FormGroup>
            </Col>
          </Row>
        </Container>
      </FormDialog>
    );
  }
}

export default EquipmentRentalRatesEditDialog;
