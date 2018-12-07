import React from 'react';

import { connect } from 'react-redux';

import { Grid, Row, Col, Radio, Form, FormGroup, ControlLabel, HelpBlock } from 'react-bootstrap';

import _ from 'lodash';
import Promise from 'bluebird';

import * as Api from '../../api';

import ConfirmForceHireDialog from '../dialogs/ConfirmForceHireDialog.jsx';
import ConfirmDialog from '../dialogs/ConfirmDialog.jsx';

import CheckboxControl from '../../components/CheckboxControl.jsx';
import DropdownControl from '../../components/DropdownControl.jsx';
import EditDialog from '../../components/EditDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { today, toZuluTime, formatDateTime } from '../../utils/date';
import { notBlank, isBlank } from '../../utils/string';

const STATUS_YES = 'Yes';
const STATUS_NO = 'No';
const STATUS_ASKED = 'Asked';
const STATUS_FORCE_HIRE = 'Force Hire';

// TODO Use lookup lists instead of hard-coded values (HETS-236)
const EQUIPMENT_NOT_AVAILABLE = 'Equipment Not Available';
const EQUIPMENT_NOT_SUITABLE = 'Equipment Not Suitable';
const NO_RESPONSE = 'No Response';
const MAXIMUM_HOURS_REACHED = 'Maximum Hours Reached';
const MAINTENANCE_CONTRACTOR = 'Maintenance Contractor';
const OTHER = 'Other (Reason to be mentioned in note)';
const refusalReasons = [
  EQUIPMENT_NOT_AVAILABLE,
  EQUIPMENT_NOT_SUITABLE,
  NO_RESPONSE,
  MAXIMUM_HOURS_REACHED,
  MAINTENANCE_CONTRACTOR,
  OTHER,
];

var HireOfferEditDialog = React.createClass({
  propTypes: {
    hireOffer: React.PropTypes.object.isRequired,
    rentalRequest: React.PropTypes.object.isRequired,
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
    error: React.PropTypes.object,
    blankRentalAgreements: React.PropTypes.object,
  },

  getInitialState() {
    return {
      isForceHire: this.props.hireOffer.isForceHire || false,
      wasAsked: this.props.hireOffer.wasAsked || false,
      askedDateTime: this.props.hireOffer.askedDateTime || '',
      offerResponse: this.props.hireOffer.offerResponse || '',
      offerStatus: this.props.hireOffer.offerResponse || '',
      offerRefusalReason: this.props.hireOffer.offerRefusalReason,
      offerResponseDatetime: this.props.hireOffer.offerResponseDatetime || '',
      offerResponseNote: this.props.hireOffer.offerResponseNote || '',
      note: this.props.hireOffer.note || '',

      offerResponseNoteError: '',
      offerRefusalReasonError: '',
      rentalAgreementError: '',

      showConfirmForceHireDialog: false,
      showConfirmMaxHoursHireDialog: false,

      equipmentVerifiedActive: false,
      equipmentInformationUpdateNeeded: false,
      equipmentInformationUpdateNeededReason: '',

      rentalAgreementId: null,
    };
  },

  componentDidMount() {
    this.fetch();
  },

  fetch() {
    var projectId = this.props.rentalRequest.projectId;
    var equipmentId = this.props.hireOffer.equipmentId;
    Api.getBlankRentalAgreementsForHire(projectId, equipmentId);
  },

  updateState(state, callback) {
    this.setState(state, callback);
  },
  
  offerStatusChanged(value) {
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
  },

  didChange() {
    if (this.state.isForceHire !== this.props.hireOffer.isForceHire) { return true; }
    if (this.state.wasAsked !== this.props.hireOffer.wasAsked) { return true; }
    if (this.state.askedDateTime !== this.props.hireOffer.askedDateTime) { return true; }
    if (this.state.offerResponse !== this.props.hireOffer.offerResponse) { return true; }
    if (this.state.offerRefusalReason !== this.props.hireOffer.offerRefusalReason) { return true; }
    if (this.state.offerResponseDatetime !== this.props.hireOffer.offerResponseDatetime) { return true; }
    if (this.state.offerResponseNote !== this.props.hireOffer.offerResponseNote) { return true; }
    if (this.state.note !== this.props.hireOffer.note) { return true; }

    return false;
  },

  isValid() {
    this.setState({
      noteError: '',
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

    if (this.state.offerStatus == STATUS_NO && this.state.offerRefusalReason === OTHER && isBlank(this.state.offerResponseNote)) {
      this.setState({ offerResponseNoteError: 'Note is required' });
      valid = false;
    }
    
    var blankRentalAgreementCount = _.keys(this.props.blankRentalAgreements.data).length;
    if ((this.state.offerStatus == STATUS_YES || this.state.offerStatus == STATUS_FORCE_HIRE) && blankRentalAgreementCount > 0 && !this.state.rentalAgreementId) {
      this.setState({ rentalAgreementError: 'A rental agreement is required' });
      valid = false;
    }

    return valid;
  },

  buildEquipmentProps() {
    var props = {};

    if (this.state.equipmentVerifiedActive) {
      props.lastVerifiedDate = toZuluTime(today());
    }

    if (this.state.equipmentInformationUpdateNeeded || notBlank(this.state.equipmentInformationUpdateNeededReason)) {
      props.isInformationUpdateNeeded = true;
    }

    if (notBlank(this.state.equipmentInformationUpdateNeededReason)) {
      props.informationUpdateNeededReason = this.state.equipmentInformationUpdateNeededReason;
    }

    return props;
  },

  onSave() {
    var isDumpTruck = this.props.hireOffer.equipment.districtEquipmentType.equipmentType.isDumpTruck;
    var hoursYtd = this.props.hireOffer.equipment.hoursYtd;
    if (this.state.offerStatus !== STATUS_NO && !isDumpTruck && hoursYtd >= 300) {
      return this.openConfirmMaxHoursHireDialog();
    }
    if (this.state.offerStatus !== STATUS_NO && isDumpTruck && hoursYtd >= 600) {
      return this.openConfirmMaxHoursHireDialog();
    }

    if (this.state.offerStatus == STATUS_FORCE_HIRE) {
      return this.openConfirmForceHireDialog();
    }

    this.saveHireOffer();
  },

  onCancelMaxHoursHire() {
    if (this.state.offerResponse === STATUS_FORCE_HIRE) {
      this.setState(this.getInitialState());
    } else {
      this.offerStatusChanged(STATUS_NO);
      this.setState({ offerRefusalReason: MAXIMUM_HOURS_REACHED });
    }
    this.closeConfirmMaxHoursHireDialog();
  },

  onConfirmMaxHoursHire() {
    if (this.state.offerStatus == STATUS_FORCE_HIRE) {
      return this.openConfirmForceHireDialog();
    }

    this.saveHireOffer();
  },

  saveHireOffer() {
    var props = this.buildEquipmentProps();

    // Update Equipment props only if they changed
    var didChange = !_.isMatch(this.props.hireOffer.equipment, props);
    var promise = didChange ? Api.updateEquipment : Promise.resolve;

    promise({ ...this.props.hireOffer.equipment, ...props }).then(() => {
      this.props.onSave({ ...this.props.hireOffer, ...{
        isForceHire: this.state.isForceHire,
        wasAsked: this.state.wasAsked ? true : false,
        askedDateTime: this.state.offerResponse === STATUS_ASKED ? formatDateTime(new Date()) : toZuluTime(this.state.askedDateTime),
        offerResponse: this.state.offerResponse,
        offerResponseDatetime: toZuluTime(this.state.offerResponseDatetime),
        offerRefusalReason: this.state.offerRefusalReason,
        offerResponseNote: this.state.offerResponseNote,
        note: this.state.reasonForForceHire,
        rentalAgreementId: this.state.rentalAgreementId,
      }});
    });
  },

  onConfirmForceHire(reasonForForceHire) {
    this.setState({ reasonForForceHire: reasonForForceHire }, this.saveHireOffer());
  },

  openConfirmForceHireDialog() {
    this.setState({ showConfirmForceHireDialog: true });
  },

  closeConfirmForceHireDialog() {
    this.setState({ showConfirmForceHireDialog: false });
  },

  openConfirmMaxHoursHireDialog() {
    this.setState({ showConfirmMaxHoursHireDialog: true });
  },

  closeConfirmMaxHoursHireDialog() {
    this.setState({ showConfirmMaxHoursHireDialog: false });
  },

  render() {
    // Read-only if the user cannot edit the rental agreement
    var isReadOnly = !this.props.rentalRequest.canEdit && this.props.rentalRequest.id !== 0;

    return <EditDialog id="hire-offer-edit" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title={
        <strong>Response</strong>
      }>
      {(() => {
        var blankRentalAgreements = _.sortBy(this.props.blankRentalAgreements.data, 'number');
        
        var agreementChoiceRow = _.keys(blankRentalAgreements).length === 0 ? null :
          <FormGroup controlId="rentalAgreementId" validationState={ this.state.rentalAgreementError ? 'error' : null }>
            <DropdownControl id="rentalAgreementId" updateState={ this.updateState } items={ blankRentalAgreements } selectedId={ this.state.rentalAgreementId } blankLine="Select rental agreement" placeholder="Select rental agreement" />
            <HelpBlock>{ this.state.rentalAgreementError }</HelpBlock>
          </FormGroup>;

        return <Form>
          <Grid fluid>
            <Col md={12}>
              <FormGroup validationState={ this.state.offerResponseError ? 'error' : null }>
                <ControlLabel>Response</ControlLabel>
                <Row>
                  <Col md={12}>
                    <FormGroup>
                      <Radio 
                        onChange={ this.offerStatusChanged.bind(this, STATUS_YES) } 
                        checked={ this.state.offerStatus == STATUS_YES }
                        disabled={ !this.props.hireOffer.showAllResponseFields && !this.props.hireOffer.offerResponse }
                      >
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
                        onChange={ this.offerStatusChanged.bind(this, STATUS_NO) } 
                        checked={ this.state.offerStatus == STATUS_NO }
                        disabled={ !this.props.hireOffer.showAllResponseFields && !this.props.hireOffer.offerResponse }
                      >
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
                        onChange={ this.offerStatusChanged.bind(this, STATUS_FORCE_HIRE) } 
                        checked={ this.state.offerStatus == STATUS_FORCE_HIRE }
                      >
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
                        onChange={ this.offerStatusChanged.bind(this, STATUS_ASKED) } 
                        checked={ this.state.offerStatus == STATUS_ASKED }
                        disabled={ !this.props.hireOffer.showAllResponseFields && !this.props.hireOffer.offerResponse }
                      >
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
                  <FormInputControl componentClass="textarea" defaultValue={ this.state.offerResponseNote } readOnly={ isReadOnly } updateState={ this.updateState } />
                  <HelpBlock>{ this.state.offerResponseNoteError }</HelpBlock>
                </FormGroup>
              </Col>
            </Row>
            <Row>
              <Col md={12}>
                <FormGroup controlId="equipmentVerifiedActive">
                  <CheckboxControl id="equipmentVerifiedActive" checked={ this.state.equipmentVerifiedActive } updateState={ this.updateState }>Verified Active</CheckboxControl>
                </FormGroup>
              </Col>
            </Row>
          </Grid>
        </Form>;
      })()}
      { this.state.showConfirmForceHireDialog &&
        <ConfirmForceHireDialog 
          show={ this.state.showConfirmForceHireDialog } 
          onSave={ this.onConfirmForceHire } 
          onClose={ this.closeConfirmForceHireDialog } 
        />
      }
      { this.state.showConfirmMaxHoursHireDialog &&
        <ConfirmDialog 
          show={ this.state.showConfirmMaxHoursHireDialog } 
          onSave={ this.onConfirmMaxHoursHire } 
          onClose={ this.onCancelMaxHoursHire } 
          title="Confirm Hire"
        >
          <p>Equipment/Dump Truck has already reached the maximum hours for the year. Do you still want to hire this Equipment/Dump Truck?</p>
        </ConfirmDialog>
      }
    </EditDialog>;
  },
});

function mapStateToProps(state) {
  return {
    rentalRequest: state.models.rentalRequest.data,
    blankRentalAgreements: state.lookups.blankRentalAgreements,
  };
}

export default connect(mapStateToProps)(HireOfferEditDialog);
