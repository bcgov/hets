import React from 'react';

import { connect } from 'react-redux';

import { Grid, Row, Col } from 'react-bootstrap';
import { Radio } from 'react-bootstrap';
import { Form, FormGroup, ControlLabel } from 'react-bootstrap';

import _ from 'lodash';
import Promise from 'bluebird';

import * as Api from '../../api';

import CheckboxControl from '../../components/CheckboxControl.jsx';
import DropdownControl from '../../components/DropdownControl.jsx';
import EditDialog from '../../components/EditDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';

import { today, toZuluTime } from '../../utils/date';
import { notBlank } from '../../utils/string';

const STATUS_YES = 'Yes';
const STATUS_NO = 'No';
const STATUS_ASKED = 'Asked';
const STATUS_FORCE_HIRE = 'Force Hire';

// TODO Use lookup lists instead of hard-coded values (HETS-236)
const refusalReasons = [
  'Equipment Not Available',
  'Hour Limit Reached',
  'No Reply',
  'Other - see comment',
];

var HireOfferEditDialog = React.createClass({
  propTypes: {
    hireOffer: React.PropTypes.object.isRequired,
    rentalRequest: React.PropTypes.object.isRequired,
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
  },

  getInitialState() {
    return {
      isForceHire: this.props.hireOffer.isForceHire || false,
      wasAsked: this.props.hireOffer.wasAsked || false,
      askedDateTime: this.props.hireOffer.askedDateTime || '',
      offerResponse: this.props.hireOffer.offerResponse || '',
      offerRefusalReason: this.props.hireOffer.offerRefusalReason || '',
      offerResponseDatetime: this.props.hireOffer.offerResponseDatetime || '',
      offerResponseNote: this.props.hireOffer.offerResponseNote || '',
      note: this.props.hireOffer.note || '',

      equipmentVerifiedActive: false,
      equipmentInformationUpdateNeeded: this.props.hireOffer.equipment.isInformationUpdateNeeded,
      equipmentInformationUpdateNeededReason: this.props.hireOffer.equipment.informationUpdateNeededReason || '',
    };
  },

  componentDidMount() {
    this.fetch();
  },

  fetch() {

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
    });
  },

  didChange() {
    if (this.state.isForceHire !== this.props.hireOffer.equipmentCount) { return true; }
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
    return true;
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
    var props = this.buildEquipmentProps();

    // Update Equipment props only if they changed
    var didChange = !_.isMatch(this.props.hireOffer.equipment, props);
    var promise = didChange ? Api.updateEquipment : Promise.resolve;

    promise({ ...this.props.hireOffer.equipment, ...props }).then(() => {
      this.props.onSave({ ...this.props.hireOffer, ...{
        isForceHire: this.state.isForceHire,
        wasAsked: this.state.wasAsked,
        askedDateTime: toZuluTime(this.state.askedDateTime),
        offerResponse: this.state.offerResponse,
        offerResponseDatetime: toZuluTime(this.state.offerResponseDatetime),
        offerRefusalReason: this.state.offerRefusalReason,
        offerResponseNote: this.state.offerResponseNote,
        note: this.state.note,
      }});
    });
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
        return <Form>
          <Grid fluid>
            <Row>
              <Col md={12}>
                <FormGroup>
                  <Radio onChange={ this.offerStatusChanged.bind(this, STATUS_FORCE_HIRE) } checked={ this.state.offerStatus == STATUS_FORCE_HIRE }>Force Hire</Radio>
                </FormGroup>
              </Col>
            </Row>
            <Row>
              <Col md={12}>
                <FormGroup>
                  <Radio onChange={ this.offerStatusChanged.bind(this, STATUS_ASKED) } checked={ this.state.offerStatus == STATUS_ASKED }>Asked</Radio>
                </FormGroup>
              </Col>
            </Row>
            <Row>
              <Col md={12}>
                <FormGroup>
                  <Radio onChange={ this.offerStatusChanged.bind(this, STATUS_YES) } checked={ this.state.offerStatus == STATUS_YES }>Yes</Radio>
                </FormGroup>
              </Col>
            </Row>
            <Row>
              <Col md={1}>
                <FormGroup>
                  <Radio onChange={ this.offerStatusChanged.bind(this, STATUS_NO) } checked={ this.state.offerStatus == STATUS_NO }>No</Radio>
                </FormGroup>
              </Col>
              <Col md={11}>
                <FormGroup>
                  {/*TODO - use lookup list*/}
                  <DropdownControl id="offerRefusalReason" disabled={ isReadOnly } title={ this.state.offerRefusalReason } updateState={ this.updateState }
                    items={ refusalReasons } />
                </FormGroup>
              </Col>
            </Row>
            <Row>
              <Col md={12}>
                <FormGroup controlId="offerResponseNote">
                  <ControlLabel>Note</ControlLabel>
                  <FormInputControl componentClass="textarea" defaultValue={ this.state.offerResponseNote } readOnly={ isReadOnly } updateState={ this.updateState } />
                </FormGroup>
              </Col>
            </Row>
            <Row>
              <Col md={12}>
                <FormGroup controlId="equipmentVerifiedActive">
                  <CheckboxControl id="equipmentVerifiedActive" checked={ this.state.equipmentVerifiedActive } updateState={ this.updateState }>Equipment Verified Active</CheckboxControl>
                </FormGroup>
              </Col>
            </Row>
            <Row>
              <Col md={12}>
                <FormGroup controlId="equipmentInformationUpdateNeeded">
                  <CheckboxControl id="equipmentInformationUpdateNeeded" checked={ this.state.equipmentInformationUpdateNeeded } updateState={ this.updateState }>Flag Equipment Updates</CheckboxControl>
                </FormGroup>
              </Col>
            </Row>
            <Row>
              <Col md={12}>
                <FormGroup controlId="equipmentInformationUpdateNeededReason">
                  <ControlLabel>Update Reason</ControlLabel>
                  <FormInputControl componentClass="textarea" defaultValue={ this.state.equipmentInformationUpdateNeededReason } readOnly={ isReadOnly } updateState={ this.updateState } />
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
    rentalRequest: state.models.rentalRequest,
  };
}

export default connect(mapStateToProps)(HireOfferEditDialog);
