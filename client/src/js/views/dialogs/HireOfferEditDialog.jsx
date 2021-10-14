import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { Container, Row, Col, FormCheck, FormGroup, FormLabel, FormText } from 'react-bootstrap';
import _ from 'lodash';

import * as Api from '../../api';
import * as Constant from '../../constants';

import ConfirmForceHireDialog from '../dialogs/ConfirmForceHireDialog.jsx';
import ConfirmDialog from '../dialogs/ConfirmDialog.jsx';

import CheckboxControl from '../../components/CheckboxControl.jsx';
import DropdownControl from '../../components/DropdownControl.jsx';
import FormDialog from '../../components/FormDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { today, toZuluTime, formatDateTime } from '../../utils/date';
import { isBlank } from '../../utils/string';

const STATUS_YES = 'Yes';
const STATUS_NO = 'No';
const STATUS_ASKED = 'Asked';
const STATUS_FORCE_HIRE = 'Force Hire';

const refusalReasons = [
  Constant.HIRING_REFUSAL_EQUIPMENT_NOT_AVAILABLE,
  Constant.HIRING_REFUSAL_EQUIPMENT_NOT_SUITABLE,
  Constant.HIRING_REFUSAL_NO_RESPONSE,
  Constant.HIRING_REFUSAL_MAXIMUM_HOURS_REACHED,
  Constant.HIRING_REFUSAL_MAINTENANCE_CONTRACTOR,
  Constant.HIRING_REFUSAL_OTHER,
];

class HireOfferEditDialog extends React.Component {
  static displayName = 'HireOfferEditDialog';

  static propTypes = {
    hireOffer: PropTypes.object.isRequired,
    showAllResponseFields: PropTypes.bool.isRequired,
    rentalRequest: PropTypes.object.isRequired,
    onSave: PropTypes.func.isRequired,
    onClose: PropTypes.func.isRequired,
    show: PropTypes.bool,
  };

  constructor(props) {
    super(props);

    this.state = this.buildInitialState();
  }

  buildInitialState() {
    return {
      isSaving: false,

      isForceHire: this.props.hireOffer.isForceHire || false,
      wasAsked: this.props.hireOffer.wasAsked || false,
      askedDateTime: this.props.hireOffer.askedDateTime || '',
      offerResponse: this.props.hireOffer.offerResponse || '',
      offerStatus: this.props.hireOffer.offerResponse || '',
      offerRefusalReason: this.props.hireOffer.offerRefusalReason,
      offerResponseDatetime: this.props.hireOffer.offerResponseDatetime || '',
      offerResponseNote: this.props.hireOffer.offerResponseNote || '',
      note: this.props.hireOffer.note || '',

      offerResponseError: '',
      offerResponseNoteError: '',
      offerRefusalReasonError: '',

      showConfirmForceHireDialog: false,
      showConfirmMaxHoursHireDialog: false,

      equipmentVerifiedActive: false,
    };
  }

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  offerStatusChanged = (value) => {
    this.setState({
      offerStatus: value,
      offerResponse: value,
      offerResponseDatetime: value !== STATUS_ASKED ? today() : null,
      isForceHire: value === STATUS_FORCE_HIRE,
      wasAsked: STATUS_ASKED,
      askedDateTime: value === STATUS_ASKED ? today() : null,
      equipmentVerifiedActive: value === STATUS_YES || value === STATUS_FORCE_HIRE ? true : false,
    });
  };

  didChange = () => {
    if (this.state.isForceHire !== this.props.hireOffer.isForceHire) {
      return true;
    }
    if (this.state.wasAsked !== this.props.hireOffer.wasAsked) {
      return true;
    }
    if (this.state.askedDateTime !== this.props.hireOffer.askedDateTime) {
      return true;
    }
    if (this.state.offerResponse !== this.props.hireOffer.offerResponse) {
      return true;
    }
    if (this.state.offerRefusalReason !== this.props.hireOffer.offerRefusalReason) {
      return true;
    }
    if (this.state.offerResponseDatetime !== this.props.hireOffer.offerResponseDatetime) {
      return true;
    }
    if (this.state.offerResponseNote !== this.props.hireOffer.offerResponseNote) {
      return true;
    }
    if (this.state.note !== this.props.hireOffer.note) {
      return true;
    }

    return false;
  };

  isValid = () => {
    this.setState({
      offerResponseError: '',
      offerRefusalReasonError: '',
      offerResponseNoteError: '',
      rentalAgreementError: '',
    });

    var valid = true;

    if (isBlank(this.state.offerResponse)) {
      this.setState({ offerResponseError: 'A response is required' });
      valid = false;
    }

    if (this.state.offerResponse === STATUS_NO && isBlank(this.state.offerRefusalReason)) {
      this.setState({
        offerRefusalReasonError: 'A refusal reason is required',
      });
      valid = false;
    }

    if (
      this.state.offerStatus === STATUS_NO &&
      this.state.offerRefusalReason === Constant.HIRING_REFUSAL_OTHER &&
      isBlank(this.state.offerResponseNote)
    ) {
      this.setState({ offerResponseNoteError: 'Note is required' });
      valid = false;
    }

    return valid;
  };

  formSubmitted = () => {
    if (this.isValid()) {
      if (this.didChange()) {
        this.setState({ isSaving: true });

        var isDumpTruck = this.props.hireOffer.districtEquipmentType.equipmentType.isDumpTruck;
        var hoursYtd = this.props.hireOffer.equipment.hoursYtd;

        if (this.state.offerStatus !== STATUS_NO && !isDumpTruck && hoursYtd >= 300) {
          this.openConfirmMaxHoursHireDialog();
        } else if (this.state.offerStatus !== STATUS_NO && isDumpTruck && hoursYtd >= 600) {
          this.openConfirmMaxHoursHireDialog();
        } else if (this.state.offerStatus === STATUS_FORCE_HIRE) {
          this.openConfirmForceHireDialog();
        } else {
          this.saveHireOffer();
        }
      } else {
        this.props.onClose();
      }
    }
  };

  onCancelMaxHoursHire = () => {
    if (this.state.offerResponse === STATUS_FORCE_HIRE) {
      this.setState(this.buildInitialState());
    } else {
      this.offerStatusChanged(STATUS_NO);
      this.setState({
        offerRefusalReason: Constant.HIRING_REFUSAL_MAXIMUM_HOURS_REACHED,
      });
    }
    this.closeConfirmMaxHoursHireDialog();
  };

  onConfirmMaxHoursHire = () => {
    if (this.state.offerStatus === STATUS_FORCE_HIRE) {
      return this.openConfirmForceHireDialog();
    }

    this.saveHireOffer();
  };

  saveHireOffer = () => {
    var promise = Promise.resolve();

    if (this.state.equipmentVerifiedActive) {
      // Update Equipment's last verified date
      promise = Api.verifyEquipmentActive(this.props.hireOffer.equipment.id);
    }

    promise.then(() => {
      const hireOffer = {
        ..._.omit(this.props.hireOffer, 'displayFields', 'rentalAgreement'),
        isForceHire: this.state.isForceHire,
        wasAsked: this.state.wasAsked ? true : false,
        askedDateTime:
          this.state.offerResponse === STATUS_ASKED ? formatDateTime(new Date()) : toZuluTime(this.state.askedDateTime),
        offerResponse: this.state.offerResponse,
        offerResponseDatetime: toZuluTime(this.state.offerResponseDatetime),
        offerRefusalReason: this.state.offerRefusalReason,
        offerResponseNote: this.state.offerResponseNote,
        note: this.state.note,
      };

      Api.updateRentalRequestRotationList(hireOffer, this.props.rentalRequest).then(() => {
        this.setState({ isSaving: false });
        if (this.props.onSave) {
          this.props.onSave(hireOffer);
        }
      });
    });
  };

  onConfirmForceHire = (reasonForForceHire) => {
    this.setState({ note: reasonForForceHire, showConfirmForceHireDialog: false }, this.saveHireOffer);
  };

  openConfirmForceHireDialog = () => {
    this.setState({ showConfirmForceHireDialog: true });
  };

  closeConfirmForceHireDialog = () => {
    this.setState({ showConfirmForceHireDialog: false, isSaving: false });
  };

  openConfirmMaxHoursHireDialog = () => {
    this.setState({ showConfirmMaxHoursHireDialog: true });
  };

  closeConfirmMaxHoursHireDialog = () => {
    this.setState({ showConfirmMaxHoursHireDialog: false, isSaving: false });
  };

  render() {
    // Read-only if the user cannot edit the rental agreement
    var isReadOnly = !this.props.rentalRequest.canEdit && this.props.rentalRequest.id !== 0;

    return (
      <FormDialog
        id="hire-offer-edit"
        show={this.props.show}
        onClose={this.props.onClose}
        onSubmit={this.formSubmitted}
        isSaving={this.state.isSaving}
        title="Response"
      >
        <Container fluid>
          <Col md={12}>
            <FormGroup>
              <FormLabel>Response</FormLabel>
              <Row>
                <Col md={12}>
                  <FormGroup>
                    <FormCheck
                      type="radio"
                      onChange={() => this.offerStatusChanged(STATUS_YES)}
                      checked={this.state.offerStatus === STATUS_YES}
                      disabled={!this.props.showAllResponseFields && !this.props.hireOffer.offerResponse}
                      label="Yes"
                      isInvalid={this.state.offerResponseError}
                    />
                  </FormGroup>
                </Col>
              </Row>
              <Row>
                <Col md={12}>
                  <FormGroup>
                    <FormCheck
                      type="radio"
                      onChange={() => this.offerStatusChanged(STATUS_NO)}
                      checked={this.state.offerStatus === STATUS_NO}
                      disabled={!this.props.showAllResponseFields && !this.props.hireOffer.offerResponse}
                      label="No"
                      isInvalid={this.state.offerResponseError}
                    />
                  </FormGroup>
                </Col>
              </Row>
              {this.state.offerStatus === STATUS_NO && (
                <Row>
                  <Col md={12}>
                    <FormGroup>
                      {/*TODO - use lookup list*/}
                      <FormLabel>Refusal Reason</FormLabel>
                      <DropdownControl
                        id="offerRefusalReason"
                        className="full-width"
                        disabled={isReadOnly}
                        title={this.state.offerRefusalReason}
                        updateState={this.updateState}
                        items={refusalReasons}
                        isInvalid={this.state.offerRefusalReasonError}
                      />
                      <FormText>{this.state.offerRefusalReasonError}</FormText>
                    </FormGroup>
                  </Col>
                </Row>
              )}
              <Row>
                <Col md={12}>
                  <FormGroup>
                    <FormCheck
                      type="radio"
                      onChange={() => this.offerStatusChanged(STATUS_FORCE_HIRE)}
                      checked={this.state.offerStatus === STATUS_FORCE_HIRE}
                      label="Force Hire"
                      isInvalid={this.state.offerResponseError}
                    />
                  </FormGroup>
                </Col>
              </Row>
              {/* HETS-1327 Remove Asked */}
              {/* <Row>
                <Col md={12}>
                  <FormGroup>
                    <FormCheck
                      type="radio"
                      onChange={() => this.offerStatusChanged(STATUS_ASKED)}
                      checked={this.state.offerStatus === STATUS_ASKED}
                      disabled={!this.props.showAllResponseFields && !this.props.hireOffer.offerResponse}
                      label="Asked"
                      isInvalid={this.state.offerResponseError}
                    />
                  </FormGroup>
                </Col>
              </Row> */}
              <FormText>{this.state.offerResponseError}</FormText>
            </FormGroup>
          </Col>
          <Row>
            <Col md={12}>
              <FormGroup controlId="offerResponseNote">
                <FormLabel>Note</FormLabel>
                <FormInputControl
                  as="textarea"
                  defaultValue={this.state.offerResponseNote}
                  readOnly={isReadOnly}
                  updateState={this.updateState}
                  isInvalid={this.state.offerResponseNoteError}
                />
                <FormText>{this.state.offerResponseNoteError}</FormText>
              </FormGroup>
            </Col>
          </Row>
          <Row>
            <Col md={12}>
              <FormGroup controlId="equipmentVerifiedActive">
                <CheckboxControl
                  id="equipmentVerifiedActive"
                  checked={this.state.equipmentVerifiedActive}
                  updateState={this.updateState}
                  label="Verified Active"
                />
              </FormGroup>
            </Col>
          </Row>
        </Container>
        {this.state.showConfirmForceHireDialog && (
          <ConfirmForceHireDialog
            show={this.state.showConfirmForceHireDialog}
            onSave={this.onConfirmForceHire}
            onClose={this.closeConfirmForceHireDialog}
          />
        )}
        {this.state.showConfirmMaxHoursHireDialog && (
          <ConfirmDialog
            title="Confirm Hire"
            show={this.state.showConfirmMaxHoursHireDialog}
            onSave={this.onConfirmMaxHoursHire}
            onClose={this.onCancelMaxHoursHire}
          >
            <p>
              Equipment/Dump Truck has already reached the maximum hours for the year. Do you still want to hire this
              Equipment/Dump Truck?
            </p>
          </ConfirmDialog>
        )}
      </FormDialog>
    );
  }
}

function mapStateToProps() {
  return {};
}

export default connect(mapStateToProps)(HireOfferEditDialog);
