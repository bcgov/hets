import React from 'react';

import { connect } from 'react-redux';

import { Link, browserHistory } from 'react-router';
import { Grid, Well, Row, Col } from 'react-bootstrap';
import { Table, Alert, Button, Glyphicon, Label, ButtonGroup } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';

import _ from 'lodash';

import AttachmentRatesEditDialog from './dialogs/AttachmentRatesEditDialog.jsx';
import EquipmentRentalRatesEditDialog from './dialogs/EquipmentRentalRatesEditDialog.jsx';
import RentalAgreementsEditDialog from './dialogs/RentalAgreementsEditDialog.jsx';
import RentalConditionsEditDialog from './dialogs/RentalConditionsEditDialog.jsx';
import RentalRatesEditDialog from './dialogs/RentalRatesEditDialog.jsx';

import * as Api from '../api';
import * as Constant from '../constants';

import ColDisplay from '../components/ColDisplay.jsx';
import DeleteButton from '../components/DeleteButton.jsx';
import EditButton from '../components/EditButton.jsx';
import Spinner from '../components/Spinner.jsx';
import Unimplemented from '../components/Unimplemented.jsx';

import { formatDateTime } from '../utils/date';
import { formatCurrency } from '../utils/string';

/*

TODO:
* Email / Notes / History / Conditions

*/

var RentalAgreementsDetail = React.createClass({
  propTypes: {
    rentalAgreement: React.PropTypes.object,
    notes: React.PropTypes.object,
    history: React.PropTypes.object,
    rentalConditions: React.PropTypes.array,
    params: React.PropTypes.object,
    ui: React.PropTypes.object,
    location: React.PropTypes.object,
    router: React.PropTypes.object,
    provincialRateTypes: React.PropTypes.array,
  },

  getInitialState() {
    return {
      loading: true,
      rentalAgreementDocumentLoading: false,

      showEditDialog: false,
      showEquipmentRateDialog: false,
      showRentalRateDialog: false,
      showAttachmentRateDialog: false,
      showConditionDialog: false,

      returnUrl: (this.props.location.state || {}).returnUrl || Constant.RENTAL_REQUESTS_PATHNAME,
      rentalRate: {},
      attachmentRate: {},
      rentalCondition: {},
    };
  },

  componentDidMount() {
    this.fetch();
  },

  fetch() {
    this.setState({ loading: true });
    var getRentalAgreementPromise = Api.getRentalAgreement(this.props.params.rentalAgreementId);
    var getRentalConditionsPromise = Api.getRentalConditions();
    var getProvincialRateTypesPromise = Api.getProvincialRateTypes();
    return Promise.all([getRentalAgreementPromise, getRentalConditionsPromise, getProvincialRateTypesPromise]).finally(() => {
      this.setState({ loading: false });
    });
  },

  updateState(state, callback) {
    this.setState(state, callback);
  },

  openEditDialog() {
    this.setState({ showEditDialog: true });
  },

  closeEditDialog() {
    this.setState({ showEditDialog: false });
  },

  saveEdit(rentalAgreement) {
    return Api.updateRentalAgreement(rentalAgreement).finally(() => {
      this.fetch();
      this.closeEditDialog();
    });
  },

  openEquipmentRateDialog() {
    this.setState({ showEquipmentRateDialog: true });
  },

  closeEquipmentRateDialog() {
    this.setState({ showEquipmentRateDialog: false });
  },

  saveEquipmentRate(rentalAgreement) {
    return Api.updateRentalAgreement(rentalAgreement).finally(() => {
      this.fetch();
      this.closeEquipmentRateDialog();
    });
  },

  openRentalRateDialog(rentalRate) {
    this.setState({
      rentalRate: rentalRate,
      showRentalRateDialog: true,
    });
  },

  closeRentalRateDialog() {
    this.setState({ showRentalRateDialog: false });
  },

  addRentalRate() {
    // New
    this.openRentalRateDialog({
      id: 0,
      isAttachment: false,
      rentalAgreement: this.props.rentalAgreement,
    });
  },

  deleteRentalRate(rentalRate) {
    Api.deleteRentalRate(rentalRate).then(() => {
      // In addition to refreshing the rental rates, we need to update the rental agreement
      // to get possibly new info.
      this.fetch();
    });
  },

  saveRentalRate(rentalRate) {
    // Update or add accordingly
    var isNew = !rentalRate.id;
    var savePromise = isNew ? Api.addRentalRate : Api.updateRentalRate;

    savePromise(rentalRate).finally(() => {
      this.fetch();
      this.closeRentalRateDialog();
    });
  },

  openAttachmentRateDialog(attachmentRate) {
    this.setState({
      attachmentRate: attachmentRate,
      showAttachmentRateDialog: true,
    });
  },

  closeAttachmentRateDialog() {
    this.setState({ 
      attachmentRate: _.omit({ ...this.state.attachmentRate }, 'rentalAgreement', 'id', 'isAttachment'),
      showAttachmentRateDialog: false,
    });
  },

  addAttachmentRate() {
    this.openAttachmentRateDialog({
      id: 0,
      isAttachment: true,
      rentalAgreement: this.props.rentalAgreement,
    });
  },

  deleteAttachmentRate(attachmentRate) {
    Api.deleteRentalRate(attachmentRate).then(() => {
      // In addition to refreshing the rental rates, we need to update the rental agreement
      // to get possibly new info.
      this.fetch();
    });
  },

  saveAttachmentRate(attachmentRate) {
    // Update or add accordingly
    var isNew = !attachmentRate.id;
    var savePromise = isNew ? Api.addRentalRate : Api.updateRentalRate;

    savePromise(attachmentRate).finally(() => {
      this.fetch();
      this.closeAttachmentRateDialog();
    });
  },

  openConditionDialog(rentalCondition) {
    this.setState({
      rentalCondition: rentalCondition,
      showConditionDialog: true,
    });
  },

  closeConditionDialog() {
    this.setState({ showConditionDialog: false });
  },

  addCondition() {
    this.openConditionDialog({
      id: 0,
      rentalAgreement: this.props.rentalAgreement,
    });
  },

  deleteCondition(rentalCondition) {
    Api.deleteRentalCondition(rentalCondition).then(() => {
      // In addition to refreshing the rental rates, we need to update the rental agreement
      // to get possibly new info.
      this.fetch();
    });
  },

  saveCondition(rentalCondition) {
    // Update or add accordingly
    var isNew = !rentalCondition.id;
    var savePromise = isNew ? Api.addRentalCondition : Api.updateRentalCondition;

    savePromise(rentalCondition).finally(() => {
      this.fetch();
      this.closeConditionDialog();
    });
  },

  email() {

  },

  addNote() {

  },

  showNotes() {

  },

  showHistory() {

  },

  generateRentalAgreementDocument() {
    // Temporary approach to download PDFs
    // TODO: Research proper download technique
    // this.setState({ rentalAgreementDocumentLoading: true });
    // window.open(`/api/rentalagreements/${ this.props.params.rentalAgreementId }/pdf`);
    // this.setState({ rentalAgreementDocumentLoading: false });
    Api.generateRentalAgreementDocument(this.props.params.rentalAgreementId);
  },

  render() {
    var rentalAgreement = this.props.rentalAgreement;
    var rentalConditions = this.props.rentalConditions;
    var provincialRateTypes = this.props.provincialRateTypes;

    return <div id="rental-agreements-detail">
      <Row id="rental-agreements-top">
        <Col md={8}>
          <Label bsStyle={ rentalAgreement.isActive ? 'success' : 'danger' }>{ rentalAgreement.status }</Label>
          <Unimplemented>
            <Button title="History" onClick={ this.showHistory }>History</Button>
          </Unimplemented>
          <Unimplemented>
            <Button title="Notes" onClick={ this.showNotes }>Notes ({ Object.keys(this.props.notes).length })</Button>
          </Unimplemented>
        </Col>
        <Col md={4}>
          <div className="pull-right">
            <Unimplemented>
              <Button><Glyphicon glyph="time" title="Time Entry" /></Button>
            </Unimplemented>
            <Unimplemented>
              <Button onClick={ this.email }><Glyphicon glyph="envelope" title="E-mail" /></Button>
            </Unimplemented>
            <Button title="Return to List" onClick={ browserHistory.goBack }><Glyphicon glyph="arrow-left" /> Return to List</Button>
          </div>
        </Col>
      </Row>

      {(() => {
        if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

        return (
          <Well>
            <Grid id="rental-agreements-header">
              <Row>
                <ColDisplay md={12} labelProps={{ md: 4 }} label={ <h3>Agreement Number:</h3>}>
                  <small>{ rentalAgreement.number }</small>
                </ColDisplay>
              </Row>
              <Row>
                <ColDisplay md={12} labelProps={{ md: 4 }} label={ <h3>Owner:</h3> }>
                  <small>{ rentalAgreement.ownerName }</small>
                </ColDisplay>
              </Row>
              <Row>
                <ColDisplay md={12} labelProps={{ md: 4 }} label={ <h3>Equipment ID:</h3> }>
                  <Link to={{ pathname: 'equipment/' + rentalAgreement.equipment.id }}>
                    <small>{ rentalAgreement.equipment.equipmentCode }</small>
                  </Link>
                </ColDisplay>
              </Row>
              <Row>
                <ColDisplay md={12} labelProps={{ md: 4 }} label={ <h3>Equipment Serial Number:</h3> }>
                  <small>{ rentalAgreement.equipment.serialNumber }</small>
                </ColDisplay>
              </Row>
              <Row>
                <ColDisplay md={12} labelProps={{ md: 4 }} label={ <h3>Equipment Yr Mk/Md/Sz:</h3> }>
                  <small>{`${rentalAgreement.equipment.year} ${rentalAgreement.equipment.make}/${rentalAgreement.equipment.model}/${rentalAgreement.equipment.size}`}</small>
                </ColDisplay>
              </Row>
              <Row>
                <ColDisplay md={12} labelProps={{ md: 4 }} label={ <h3>Project:</h3> }>
                  <small>{ rentalAgreement.project.name }</small>
                </ColDisplay>
              </Row>
            </Grid>
          </Well>
        );
      })()}

      <Well>
        <h3>Rates</h3>
        {(() => {
          if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

          return <div>
            <Grid id="rental-rates" fluid className="nopadding">
              <Row>
                <Col md={3}>
                  <ColDisplay md={12} labelProps={{ md: 6 }} label="Pay Rate:">{ formatCurrency(rentalAgreement.equipmentRate) }</ColDisplay>
                </Col>
                <Col md={3}>
                  <ColDisplay md={12} labelProps={{ md: 6 }} label="Period:">{ rentalAgreement.ratePeriod }</ColDisplay>
                </Col>
                <Col md={5}>
                  <ColDisplay md={12} labelProps={{ md: 3 }} label="Comment:">{ rentalAgreement.rateComment }</ColDisplay>
                </Col>
                <Col md={1}>
                  <EditButton title="Edit Pay Rate" className="pull-right" onClick={ this.openEquipmentRateDialog } />
                </Col>
              </Row>
            </Grid>
          </div>;
        })()}

        {(() => {
          if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

          // Exclude attachment rates - those are shown on the next section
          var rentalRates = _.reject(rentalAgreement.rentalAgreementRates, { isAttachment: true });

          var button = <Button title="Add Rate" bsSize="small" className="no-margin" onClick={ this.addRentalRate }>
              <Glyphicon glyph="plus" />
          </Button>;

          if (Object.keys(rentalRates || []).length === 0) { return <div><Alert bsStyle="success" style={{ marginTop: 10 }}>No additional rates</Alert>{ button }</div>; }

          return <div>
            <Table striped condensed hover bordered>
              <thead>
                <tr>
                  <th>Rate Type</th>
                  <th>Rate</th>
                  <th>Period</th>
                  <th>Comment</th>
                  <th>Include in Total</th>
                  <th></th>
                </tr>
              </thead>
              <tbody>
                {
                  _.map(rentalRates, obj => {
                    return <tr key={ obj.id }>
                      <td>{ obj.componentName }</td>
                      <td>
                        { obj.dollarValue > 0 &&
                          <span>{ formatCurrency(obj.dollarValue) }</span>
                        }
                        { obj.percentOfEquipmentRate > 0 &&
                          <span>&nbsp;({ `${obj.percentOfEquipmentRate}%` })</span>
                        }
                      </td>
                      <td>{ obj.ratePeriod }</td>
                      <td>{ obj.comment }</td>
                      <td>{ obj.includeInTotal ? 'Yes' : 'No' }</td>
                      <td style={{ textAlign: 'right' }}>
                        <ButtonGroup>
                          <DeleteButton name="Rental rate" hide={ !obj.canDelete } onConfirm={ this.deleteRentalRate.bind(this, obj) }/>
                          <EditButton name="Rental rate" view={ !obj.canEdit } onClick={ this.openRentalRateDialog.bind(this, obj) }/>
                        </ButtonGroup>
                      </td>
                    </tr>;
                  })
                }
              </tbody>
            </Table>
            { button }
          </div>;
        })()}
      </Well>

      <Well>
        <h3>Attachments</h3>
        {(() => {
          var equipmentAttachments = rentalAgreement.equipment && rentalAgreement.equipment.equipmentAttachments;

          if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }
          if (Object.keys(equipmentAttachments).length === 0) { return <Alert bsStyle="success" style={{ marginTop: 10 }}>This piece of equipment has no attachments</Alert>; }

          // Only want attachments rates here - the rest are shown above
          var attachmentRates = _.filter(rentalAgreement.rentalAgreementRates, { isAttachment: true });

          var button = <Button title="Add Attachment Rate" bsSize="small" className="no-margin" onClick={ this.addAttachmentRate }>
            <Glyphicon glyph="plus" />
          </Button>;

          if (Object.keys(attachmentRates || []).length === 0) { return <div><Alert bsStyle="success" style={{ marginTop: 10 }}>No attachment rates</Alert>{ button }</div>; }

          return <div id="attachment-rates">
            <Table striped condensed hover bordered>
              <thead>
                <tr>
                  <th>Attachment</th>
                  <th>Rate</th>
                  <th>Period</th>
                  <th>Comment</th>
                  <th>Include in Total</th>
                  <th></th>
                </tr>
              </thead>
              <tbody>
                {
                  _.map(attachmentRates, obj => {
                    return <tr key={ obj.id }>
                      <td>{ obj.componentName }</td>
                      <td>
                        { obj.dollarValue > 0 &&
                          <span>{ formatCurrency(obj.dollarValue) }</span>
                        }
                        { obj.percentOfEquipmentRate > 0 &&
                          <span>&nbsp;({ `${obj.percentOfEquipmentRate}%` })</span>
                        }
                      </td>
                      <td>{ obj.ratePeriod }</td>
                      <td>{ obj.comment }</td>
                      <td>{ obj.isIncludedInTotal ? 'Yes' : 'No' }</td>
                      <td style={{ textAlign: 'right' }}>
                        <ButtonGroup>
                          <DeleteButton name="Attachment Rate" hide={ !obj.canDelete } onConfirm={ this.deleteAttachmentRate.bind(this, obj) }/>
                          <EditButton name="Attachment Rate" view={ !obj.canEdit } onClick={ this.openAttachmentRateDialog.bind(this, obj ) }/>
                        </ButtonGroup>
                      </td>
                    </tr>;
                  })
                }
              </tbody>
            </Table>
            { button }
          </div>;
        })()}
      </Well>

      <Well>
        <h3>Conditions</h3>
        {(() => {
          if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

          var rentalConditions = rentalAgreement.rentalAgreementConditions;

          var button = <Button title="Add Rental Condition" bsSize="small" className="no-margin" onClick={ this.addCondition }>
            <Glyphicon glyph="plus" />
          </Button>;

          if (Object.keys(rentalConditions || []).length === 0) { return <div><Alert bsStyle="success" style={{ marginTop: 10 }}>No rental conditions</Alert>{ button }</div>; }

          return <div id="rental-conditions">
            <Table striped condensed hover bordered>
              <thead>
                <tr>
                  <th>Condition</th>
                  <th>Comment</th>
                  <th></th>
                </tr>
              </thead>
              <tbody>
                {
                  _.map(rentalConditions, obj => {
                    return <tr key={ obj.id }>
                      <td>{ obj.conditionName }</td>
                      <td>{ obj.comment }</td>
                      <td style={{ textAlign: 'right' }}>
                        <ButtonGroup>
                          <DeleteButton name="Rental Condition" hide={ !obj.canDelete } onConfirm={ this.deleteCondition.bind(this, obj) }/>
                          <EditButton name="Rental Condition" view={ !obj.canEdit } onClick={ this.openConditionDialog.bind(this, obj) }/>
                        </ButtonGroup>
                      </td>
                    </tr>;
                  })
                }
              </tbody>
            </Table>
            { button }
          </div>;
        })()}
      </Well>

      <Well>
        <div className="clearfix">
          <span className="pull-right">
            <Button title="Edit Rental Agreement" bsSize="small" onClick={ this.openEditDialog }><Glyphicon glyph="pencil" /></Button>
          </span>
        </div>
        {(() => {
          if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

          return <Grid fluid>
            <Row>
              <Col md={6}>
                <ColDisplay md={12} labelProps={{ md: 4 }} label="Estimated Commencement:">{ formatDateTime(rentalAgreement.estimateStartWork, Constant.DATE_YEAR_SHORT_MONTH_DAY) }</ColDisplay>
                <ColDisplay md={12} labelProps={{ md: 4 }} label="Point of Hire:">{ rentalAgreement.pointOfHire }</ColDisplay>
                <ColDisplay md={12} labelProps={{ md: 4 }} label="District:">{ rentalAgreement.districtName }</ColDisplay>
              </Col>
              <Col md={6}>
                <ColDisplay md={12} labelProps={{ md: 4 }} label="Dated At:">{ formatDateTime(rentalAgreement.datedOn, Constant.DATE_YEAR_SHORT_MONTH_DAY) }</ColDisplay>
                <ColDisplay md={12} labelProps={{ md: 4 }} label="Estimated Period Hours:">{ rentalAgreement.estimateHours }</ColDisplay>
                <ColDisplay md={12} labelProps={{ md: 4 }} label="WorkSafeBC (WCB) Number:">{ rentalAgreement.workSafeBCPolicyNumber }</ColDisplay>
              </Col>
            </Row>
          </Grid>;
        })()}
      </Well>

      <Well>
        <h3>History <span className="pull-right">
          <Unimplemented>
            <Button title="Add note" bsSize="small" onClick={ this.addNote }><Glyphicon glyph="plus" /></Button>
          </Unimplemented>
        </span></h3>
        {(() => {
          if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }
          if (Object.keys(this.props.history || []).length === 0) { return <Alert bsStyle="success" style={{ marginTop: 10 }}>No history</Alert>; }

          var history = _.sortBy(this.props.history, 'createdDate');

          const HistoryEntry = ({ createdDate, historyText }) => (
            <Row>
              <ColDisplay md={12} labelProps={{ md: 2 }} label={ formatDateTime(createdDate, Constant.DATE_YEAR_SHORT_MONTH_DAY) }>
                { historyText }
              </ColDisplay>
            </Row>
          );

          return <div id="rental-agreements-history">
            {
              _.map(history, (entry) => <HistoryEntry { ...entry } />)
            }
          </div>;
        })()}
      </Well>
      <Row id="rental-agreements-footer">
        <div className="pull-right">
          <Button title="Generate Rental Agreement PDF" onClick={ this.generateRentalAgreementDocument } bsStyle="primary">Generate</Button>
        </div>
      </Row>
      { this.state.showEditDialog &&
        <RentalAgreementsEditDialog show={ this.state.showEditDialog } onSave={ this.saveEdit } onClose={ this.closeEditDialog } />
      }
      { this.state.showEquipmentRateDialog &&
        <EquipmentRentalRatesEditDialog show={ this.state.showEquipmentRateDialog } onSave={ this.saveEquipmentRate } onClose={ this.closeEquipmentRateDialog } />
      }
      { this.state.showRentalRateDialog &&
        <RentalRatesEditDialog 
          show={ this.state.showRentalRateDialog } 
          rentalRate={ this.state.rentalRate } 
          onSave={ this.saveRentalRate } 
          onClose={ this.closeRentalRateDialog } 
          provincialRateTypes={ provincialRateTypes }
        />
      }
      { this.state.showAttachmentRateDialog &&
        <AttachmentRatesEditDialog 
          show={ this.state.showAttachmentRateDialog } 
          attachmentRate={ this.state.attachmentRate } 
          onSave={ this.saveAttachmentRate } 
          onClose={ this.closeAttachmentRateDialog } 
          rentalAgreement={ rentalAgreement }
        />
      }
      { this.state.showConditionDialog &&
        <RentalConditionsEditDialog 
          show={ this.state.showConditionDialog } 
          rentalCondition={ this.state.rentalCondition } 
          rentalConditions={ rentalConditions } 
          onSave={ this.saveCondition } 
          onClose={ this.closeConditionDialog } 
        />
      }
    </div>;
  },
});


function mapStateToProps(state) {
  return {
    rentalAgreement: state.models.rentalAgreement,
    notes: state.models.rentalAgreementNotes,
    history: state.models.rentalAgreementHistory,
    rentalConditions: state.lookups.rentalConditions,
    provincialRateTypes: state.lookups.provincialRateTypes,
  };
}

export default connect(mapStateToProps)(RentalAgreementsDetail);
