import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { Row, Col } from 'react-bootstrap';
import { FormGroup, FormText, FormLabel } from 'react-bootstrap';

import * as Api from '../../api';

import DateControl from '../../components/DateControl.jsx';
import FormDialog from '../../components/FormDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { isValidDate } from '../../utils/date';
import { isBlank, notBlank } from '../../utils/string';

class RentalAgreementsEditDialog extends React.Component {
  static propTypes = {
    rentalAgreement: PropTypes.object.isRequired,
    show: PropTypes.bool,
    owner: PropTypes.object,
    onSave: PropTypes.func,
    onClose: PropTypes.func.isRequired,
  };

  constructor(props) {
    super(props);

    this.state = {
      estimateStartWork: props.rentalAgreement.estimateStartWork || '',
      estimateHours: props.rentalAgreement.estimateHours || 0,
      datedOn: props.rentalAgreement.datedOn || '',
      agreementCity: props.rentalAgreement.agreementCity || '',

      estimateStartWorkError: '',
      estimateHoursError: '',
      datedOnError: '',
    };
  }

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  didChange = () => {
    if (this.state.estimateStartWork !== this.props.rentalAgreement.estimateStartWork) {
      return true;
    }
    if (this.state.estimateHours !== this.props.rentalAgreement.estimateHours) {
      return true;
    }
    if (this.state.datedOn !== this.props.rentalAgreement.datedOn) {
      return true;
    }
    if (this.state.note !== this.props.rentalAgreement.note) {
      return true;
    }

    return false;
  };

  isValid = () => {
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
  };

  formSubmitted = () => {
    if (this.isValid()) {
      if (this.didChange()) {
        const rentalAgreement = {
          ...this.props.rentalAgreement,
          estimateStartWork: this.state.estimateStartWork,
          estimateHours: this.state.estimateHours,
          datedOn: this.state.datedOn,
          agreementCity: this.state.agreementCity,
        };

        this.props.dispatch(Api.updateRentalAgreement(rentalAgreement)).then(() => {
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

    return (
      <FormDialog
        id="rental-agreements-edit"
        show={this.props.show}
        title="Rental Agreement Details"
        onSubmit={this.formSubmitted}
        onClose={this.props.onClose}
      >
        <Row>
          <Col md={6}>
            <FormGroup controlId="estimateStartWork">
              <FormLabel>Estimated Commencement</FormLabel>
              <DateControl
                id="estimateStartWork"
                disabled={isReadOnly}
                date={this.state.estimateStartWork}
                updateState={this.updateState}
                title="Estimated Commencement"
                isInvalid={this.state.estimateStartWorkError}
              />
              <FormText>{this.state.estimateStartWorkError}</FormText>
            </FormGroup>
          </Col>
          <Col md={6}>
            <FormGroup controlId="estimateHours">
              <FormLabel>
                Estimated Period Hours <sup>*</sup>
              </FormLabel>
              <FormInputControl
                type="number"
                min={0}
                defaultValue={this.state.estimateHours}
                readOnly={isReadOnly}
                updateState={this.updateState}
                isInvalid={this.state.estimateHoursError}
              />
              <FormText>{this.state.estimateHoursError}</FormText>
            </FormGroup>
          </Col>
        </Row>
        <Row>
          <Col md={6}>
            <FormGroup controlId="datedOn">
              <FormLabel>Dated On</FormLabel>
              <DateControl
                id="datedOn"
                disabled={isReadOnly}
                date={this.state.datedOn}
                updateState={this.updateState}
                title="Dated On"
                isInvalid={this.state.datedOnError}
              />
              <FormText>{this.state.datedOnError}</FormText>
            </FormGroup>
          </Col>
          <Col md={6}>
            <FormGroup controlId="agreementCity">
              <FormLabel>Dated At</FormLabel>
              <FormInputControl type="text" value={this.state.agreementCity} updateState={this.updateState} />
            </FormGroup>
          </Col>
        </Row>
      </FormDialog>
    );
  }
}

const mapDispatchToProps = (dispatch) => ({ dispatch });

export default connect(null, mapDispatchToProps)(RentalAgreementsEditDialog);
