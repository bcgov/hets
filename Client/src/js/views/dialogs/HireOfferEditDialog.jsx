import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { Grid, Row, Col, Radio, FormGroup, ControlLabel, HelpBlock } from 'react-bootstrap';
import _ from 'lodash';

import * as Api from '../../api';
import * as Constant from '../../constants';

import ConfirmForceHireDialog from '../dialogs/ConfirmForceHireDialog.jsx';
import ConfirmDialog from '../dialogs/ConfirmDialog.jsx';

import Spinner from '../../components/Spinner.jsx';
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
    blankRentalAgreements: PropTypes.object,
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
      rentalAgreementError: '',

      showConfirmForceHireDialog: false,
      showConfirmMaxHoursHireDialog: false,

      equipmentVerifiedActive: false,

      rentalAgreementId: null,
    };
  }

  componentDidMount() {
    this.fetch();
  }

  fetch = () => {
    var projectId = this.props.rentalRequest.projectId;
    var equipmentId = this.props.hireOffer.equipmentId;
    Api.getBlankRentalAgreementsForHire(projectId, equipmentId);
  };

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
      equipmentVerifiedActive: (value === STATUS_YES || value === STATUS_FORCE_HIRE) ? true : false,
      rentalAgreementId: null,
    });
  };

  didChange = () => {
    if (this.state.isForceHire !== this.props.hireOffer.isForceHire) { return true; }
    if (this.state.wasAsked !== this.props.hireOffer.wasAsked) { return true; }
    if (this.state.askedDateTime !== this.props.hireOffer.askedDateTime) { return true; }
    if (this.state.offerResponse !== this.props.hireOffer.offerResponse) { return true; }
    if (this.state.offerRefusalReason !== this.props.hireOffer.offerRefusalReason) { return true; }
    if (this.state.offerResponseDatetime !== this.props.hireOffer.offerResponseDatetime) { return true; }
    if (this.state.offerResponseNote !== this.props.hireOffer.offerResponseNote) { return true; }
    if (this.state.note !== this.props.hireOffer.note) { return true; }

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
      this.setState({ offerRefusalReasonError: 'A refusal reason is required' });
      valid = false;
    }

    if (this.state.offerStatus == STATUS_NO && this.state.offerRefusalReason === Constant.HIRING_REFUSAL_OTHER && isBlank(this.state.offerResponseNote)) {
      this.setState({ offerResponseNoteError: 'Note is required' });
      valid = false;
    }

    var blankRentalAgreementCount = _.keys(this.props.blankRentalAgreements.data).length;
    if ((this.state.offerStatus == STATUS_YES || this.state.offerStatus == STATUS_FORCE_HIRE) && blankRentalAgreementCount > 0 && !this.state.rentalAgreementId) {
      this.setState({ rentalAgreementError: 'A rental agreement is required' });
      valid = false;
    }

    return valid;
  };

  formSubmitted = () => {
    if (this.isValid()) {
      if (this.didChange()) {
        this.setState({ isSaving: true });

        var isDumpTruck = this.props.hireOffer.equipment.districtEquipmentType.equipmentType.isDumpTruck;
        var hoursYtd = this.props.hireOffer.equipment.hoursYtd;

        if (this.state.offerStatus !== STATUS_NO && !isDumpTruck && hoursYtd >= 300) {
          this.openConfirmMaxHoursHireDialog();
        } else if (this.state.offerStatus !== STATUS_NO && isDumpTruck && hoursYtd >= 600) {
          this.openConfirmMaxHoursHireDialog();
        } else if (this.state.offerStatus == STATUS_FORCE_HIRE) {
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
      this.setState({ offerRefusalReason: Constant.HIRING_REFUSAL_MAXIMUM_HOURS_REACHED });
    }
    this.closeConfirmMaxHoursHireDialog();
  };

  onConfirmMaxHoursHire = () => {
    if (this.state.offerStatus == STATUS_FORCE_HIRE) {
      return this.openConfirmForceHireDialog();
    }

    this.saveHireOffer();
  };

  saveHireOffer = () => {
    var promise = Promise.resolve();

    if (this.state.equipmentVerifiedActive) {
      // Update Equipment's last verified date
      const equipment = {
        ...this.props.hireOffer.equipment,
        lastVerifiedDate: toZuluTime(today()),
      };

      promise = Api.updateEquipment(equipment);
    }

    promise.then(() => {
      const hireOffer = {
        ..._.omit(this.props.hireOffer, 'displayFields', 'rentalAgreement'),
        isForceHire: this.state.isForceHire,
        wasAsked: this.state.wasAsked ? true : false,
        askedDateTime: this.state.offerResponse === STATUS_ASKED ? formatDateTime(new Date()) : toZuluTime(this.state.askedDateTime),
        offerResponse: this.state.offerResponse,
        offerResponseDatetime: toZuluTime(this.state.offerResponseDatetime),
        offerRefusalReason: this.state.offerRefusalReason,
        offerResponseNote: this.state.offerResponseNote,
        note: this.state.note,
        rentalAgreementId: this.state.rentalAgreementId,
      };

      Api.updateRentalRequestRotationList(hireOffer, this.props.rentalRequest).then(() => {
        this.setState({isSaving: false});
        if (this.props.onSave) { this.props.onSave(hireOffer); }
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

    var blankRentalAgreements = _.sortBy(this.props.blankRentalAgreements.data, 'number');

    var agreementChoiceRow;
    if (_.keys(blankRentalAgreements).length !== 0) {
      agreementChoiceRow = (
        <FormGroup controlId="rentalAgreementId" validationState={ this.state.rentalAgreementError ? 'error' : null }>
          <DropdownControl id="rentalAgreementId" updateState={ this.updateState } items={ blankRentalAgreements } selectedId={ this.state.rentalAgreementId } blankLine="Select rental agreement" placeholder="Select rental agreement" />
          <HelpBlock>{ this.state.rentalAgreementError }</HelpBlock>
        </FormGroup>
      );
    }

    const loading = this.props.blankRentalAgreements.loading;

    return (
      <FormDialog
        id="hire-offer-edit"
        show={this.props.show}
        onClose={this.props.onClose}
        onSubmit={this.formSubmitted}
        isSaving={this.state.isSaving}
        title="Response">
        <Grid fluid>
          <Col md={12}>
            <div className="spinner-container">{loading && <Spinner/>}</div>
            <FormGroup validationState={ this.state.offerResponseError ? 'error' : null }>
              <ControlLabel>Response</ControlLabel>
              <Row>
                <Col md={12}>
                  <FormGroup>
                    <Radio
                      onChange={ () => this.offerStatusChanged(STATUS_YES) }
                      checked={ this.state.offerStatus == STATUS_YES }
                      disabled={ loading || (!this.props.showAllResponseFields && !this.props.hireOffer.offerResponse) }>
                      Yes
                    </Radio>
                  </FormGroup>
                </Col>
              </Row>
              { this.state.offerStatus == STATUS_YES && agreementChoiceRow }
              <Row>
                <Col md={12}>
                  <FormGroup>
                    <Radio
                      onChange={ () => this.offerStatusChanged(STATUS_NO) }
                      checked={ this.state.offerStatus == STATUS_NO }
                      disabled={ loading || (!this.props.showAllResponseFields && !this.props.hireOffer.offerResponse) }>
                      No
                    </Radio>
                  </FormGroup>
                </Col>
              </Row>
              { this.state.offerStatus == STATUS_NO &&
                <Row>
                  <Col md={12}>
                    <FormGroup validationState={ this.state.offerRefusalReasonError ? 'error' : null }>
                      {/*TODO - use lookup list*/}
                      <ControlLabel>Refusal Reason</ControlLabel>
                      <DropdownControl id="offerRefusalReason" className="full-width" disabled={ isReadOnly } title={ this.state.offerRefusalReason } updateState={ this.updateState }
                        items={ refusalReasons } />
                      <HelpBlock>{ this.state.offerRefusalReasonError }</HelpBlock>
                    </FormGroup>
                  </Col>
                </Row>
              }
              <Row>
                <Col md={12}>
                  <FormGroup>
                    <Radio
                      onChange={ () => this.offerStatusChanged(STATUS_FORCE_HIRE) }
                      checked={ this.state.offerStatus == STATUS_FORCE_HIRE }
                      disabled={ loading }>
                      Force Hire
                    </Radio>
                  </FormGroup>
                </Col>
              </Row>
              { this.state.offerStatus == STATUS_FORCE_HIRE && agreementChoiceRow }
              <Row>
                <Col md={12}>
                  <FormGroup>
                    <Radio
                      onChange={ () => this.offerStatusChanged(STATUS_ASKED) }
                      checked={ this.state.offerStatus == STATUS_ASKED }
                      disabled={ loading || (!this.props.showAllResponseFields && !this.props.hireOffer.offerResponse) }>
                      Asked
                    </Radio>
                  </FormGroup>
                </Col>
              </Row>
              <HelpBlock>{ this.state.offerResponseError }</HelpBlock>
            </FormGroup>
          </Col>
          <Row>
            <Col md={12}>
              <FormGroup controlId="offerResponseNote" validationState={ this.state.offerResponseNoteError ? 'error' : null }>
                <ControlLabel>Note</ControlLabel>
                <FormInputControl componentClass="textarea" disabled={loading} defaultValue={ this.state.offerResponseNote } readOnly={ isReadOnly } updateState={ this.updateState } />
                <HelpBlock>{ this.state.offerResponseNoteError }</HelpBlock>
              </FormGroup>
            </Col>
          </Row>
          <Row>
            <Col md={12}>
              <FormGroup controlId="equipmentVerifiedActive">
                <CheckboxControl id="equipmentVerifiedActive" disabled={loading} checked={ this.state.equipmentVerifiedActive } updateState={ this.updateState }>Verified Active</CheckboxControl>
              </FormGroup>
            </Col>
          </Row>
        </Grid>
        { this.state.showConfirmForceHireDialog && (
          <ConfirmForceHireDialog
            show={ this.state.showConfirmForceHireDialog }
            onSave={ this.onConfirmForceHire }
            onClose={ this.closeConfirmForceHireDialog }/>
        )}
        { this.state.showConfirmMaxHoursHireDialog && (
          <ConfirmDialog
            title="Confirm Hire"
            show={ this.state.showConfirmMaxHoursHireDialog }
            onSave={ this.onConfirmMaxHoursHire }
            onClose={ this.onCancelMaxHoursHire }>
            <p>
              Equipment/Dump Truck has already reached the maximum hours for the year. Do you still
              want to hire this Equipment/Dump Truck?
            </p>
          </ConfirmDialog>
        )}
      </FormDialog>
    );
  }
}

function mapStateToProps(state) {
  return {
    blankRentalAgreements: state.lookups.blankRentalAgreements,
  };
}

export default connect(mapStateToProps)(HireOfferEditDialog);
