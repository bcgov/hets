import React from 'react';

import { connect } from 'react-redux';

import { Link, browserHistory } from 'react-router';
import { Grid, Well, Row, Col } from 'react-bootstrap';
import { Table, Alert, Button, Glyphicon, Label, ButtonGroup } from 'react-bootstrap';

import _ from 'lodash';
import Promise from 'bluebird';

import { buildApiPath } from '../utils/http.js';

import AttachmentRatesEditDialog from './dialogs/AttachmentRatesEditDialog.jsx';
import EquipmentRentalRatesEditDialog from './dialogs/EquipmentRentalRatesEditDialog.jsx';
import RentalAgreementsEditDialog from './dialogs/RentalAgreementsEditDialog.jsx';
import RentalConditionsEditDialog from './dialogs/RentalConditionsEditDialog.jsx';
import RentalRatesEditDialog from './dialogs/RentalRatesEditDialog.jsx';
import CloneDialog from './dialogs/CloneDialog.jsx';

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
      showCloneDialog: false,

      cloneRentalAgreementError: '',

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

  saveRentalRates(rentalRates) {
    Api.addRentalRates(this.props.params.rentalAgreementId, rentalRates).finally(() => {
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

  saveAttachmentRates(attachmentRates) {
    Api.addRentalRates(this.props.params.rentalAgreementId, attachmentRates).finally(() => {
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

  saveConditions(rentalConditions) {
    Api.addRentalConditions(this.props.params.rentalAgreementId, rentalConditions).finally(() => {
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
    Api.generateRentalAgreementDocument(this.props.params.rentalAgreementId).finally(() => {
      window.open(buildApiPath(`/rentalagreements/${ this.props.params.rentalAgreementId }/pdf`));
    });
  },

  openCloneDialog() {
    this.setState({ showCloneDialog: true });
  },

  closeCloneDialog() {
    this.setState({ showCloneDialog: false, cloneRentalAgreementError: '' });
  },

  cloneRentalAgreement(rentalAgreementCloneId, type) {
    var data = {
      projectId: this.props.rentalAgreement.project.id,
      agreementToCloneId: rentalAgreementCloneId,
      rentalAgreementId: this.props.rentalAgreement.id,
    };
    var clonePromise = Api.cloneProjectRentalAgreement(data);

    if (type === Constant.BY_EQUIPMENT) {
      data.equipmentId = this.props.rentalAgreement.equipment.id;      
      clonePromise = Api.cloneEquipmentRentalAgreement(data);
    }

    this.setState({ cloneRentalAgreementError: '' });
    clonePromise.then(() => {
      this.closeCloneDialog();
    })
    .catch((error) => {
      this.setState({ cloneRentalAgreementError: error });
    });
  },

  render() {
    var rentalAgreement = this.props.rentalAgreement;
    var rentalConditions = this.props.rentalConditions;
    var provincialRateTypes = this.props.provincialRateTypes;

    return (
      <div id="rental-agreements-detail">

      {(() => {
        if (this.state.loading) { return <div className="spinner-container"><Spinner/></div>; }

        return (
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
                <Button disabled={ !rentalAgreement.isActive } onClick={ this.openCloneDialog }>Clone</Button>
                <Button title="Return" onClick={ browserHistory.goBack }><Glyphicon glyph="arrow-left" /> Return</Button>
              </div>
            </Col>
          </Row>
        );
      })()}

      <Well>
      {(() => {
        if (this.state.loading) { return <div className="spinner-container"><Spinner/></div>; }

        return (
          <div id="rental-agreements-header">
            <h3>Rental Agreement</h3>
            <Row>
              <ColDisplay md={12} labelProps={{ md: 4 }} label="Agreement Number:">{ rentalAgreement.number }</ColDisplay>
            </Row>
            <Row>
              <ColDisplay md={12} labelProps={{ md: 4 }} label="Owner:">{ rentalAgreement.ownerName }</ColDisplay>
            </Row>
            <Row>
              <ColDisplay md={12} labelProps={{ md: 4 }} label="Equipment ID:">
                <Link to={{ pathname: 'equipment/' + rentalAgreement.equipment.id }}>{ rentalAgreement.equipment.equipmentCode }</Link></ColDisplay>
            </Row>
            <Row>
              <ColDisplay md={12} labelProps={{ md: 4 }} label="Equipment Serial Number:">{ rentalAgreement.equipment.serialNumber }</ColDisplay>
            </Row>
            <Row>
              <ColDisplay md={12} labelProps={{ md: 4 }} label="Equipment Yr Mk/Md/Sz:">
                {`${rentalAgreement.equipment.year} ${rentalAgreement.equipment.make}/${rentalAgreement.equipment.model}/${rentalAgreement.equipment.size}`}
              </ColDisplay>
            </Row>
            <Row>
              <ColDisplay md={12} labelProps={{ md: 4 }} label="Project:">{ rentalAgreement.project.name }</ColDisplay>
            </Row>
          </div>
        );
      })()}
      </Well>

      <Well>
        <h3>Rates</h3>
        {(() => {
          if (this.state.loading) { return <div className="spinner-container"><Spinner/></div>; }

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
          if (this.state.loading) { return; }

          // Exclude attachment rates - those are shown on the next section
          var rentalRates = _.reject(rentalAgreement.rentalAgreementRates, { isAttachment: true });

          var button = <Button title="Add Rate" bsSize="small" className="no-margin" onClick={ this.addRentalRate }>
              <Glyphicon glyph="plus" />
          </Button>;

          if (Object.keys(rentalRates || []).length === 0) { return <div><Alert bsStyle="success">No additional rates</Alert>{ button }</div>; }

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
                      <td>{ obj.isIncludedInTotal ? 'Yes' : 'No' }</td>
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
          if (this.state.loading) { return <div className="spinner-container"><Spinner/></div>; }

          // Only want attachments rates here - the rest are shown above
          var attachmentRates = _.filter(rentalAgreement.rentalAgreementRates, { isAttachment: true });

          var button = <Button title="Add Attachment Rate" bsSize="small" className="no-margin" onClick={ this.addAttachmentRate }>
            <Glyphicon glyph="plus" />
          </Button>;

          if (Object.keys(attachmentRates || []).length === 0) { return <div><Alert bsStyle="success">No attachment rates</Alert>{ button }</div>; }

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
          if (this.state.loading) { return <div className="spinner-container"><Spinner/></div>; }

          var rentalConditions = rentalAgreement.rentalAgreementConditions;

          var button = <Button title="Add Rental Condition" bsSize="small" className="no-margin" onClick={ this.addCondition }>
            <Glyphicon glyph="plus" />
          </Button>;

          if (Object.keys(rentalConditions || []).length === 0) { return <div><Alert bsStyle="success">No rental conditions</Alert>{ button }</div>; }

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
          if (this.state.loading) { return <div className="spinner-container"><Spinner/></div>; }

          return (
            <Row>
              <Col md={6}>
                <ColDisplay md={12} labelProps={{ md: 6 }} label="Estimated Commencement:">{ formatDateTime(rentalAgreement.estimateStartWork, Constant.DATE_YEAR_SHORT_MONTH_DAY) }</ColDisplay>
                <ColDisplay md={12} labelProps={{ md: 6 }} label="Point of Hire:">{ rentalAgreement.pointOfHire }</ColDisplay>
                <ColDisplay md={12} labelProps={{ md: 6 }} label="District:">{ rentalAgreement.districtName }</ColDisplay>
              </Col>
              <Col md={6}>
                <ColDisplay md={12} labelProps={{ md: 6 }} label="Dated On:">{ formatDateTime(rentalAgreement.datedOn, Constant.DATE_YEAR_SHORT_MONTH_DAY) }</ColDisplay>
                <ColDisplay md={12} labelProps={{ md: 6 }} label="Estimated Period Hours:">{ rentalAgreement.estimateHours }</ColDisplay>
                <ColDisplay md={12} labelProps={{ md: 6 }} label="WorkSafeBC (WCB) Number:">{ rentalAgreement.workSafeBCPolicyNumber }</ColDisplay>
              </Col>
            </Row>
          );
        })()}
      </Well>

      <Well>
        <h3>History <span className="pull-right">
          <Unimplemented>
            <Button title="Add note" bsSize="small" onClick={ this.addNote }><Glyphicon glyph="plus" /></Button>
          </Unimplemented>
        </span></h3>
        {(() => {
          if (this.state.loading) { return <div className="spinner-container"><Spinner/></div>; }
          if (Object.keys(this.props.history || []).length === 0) { return <Alert bsStyle="success">No history</Alert>; }

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
          onSaveMultiple={ this.saveRentalRates }
          onClose={ this.closeRentalRateDialog } 
          provincialRateTypes={ provincialRateTypes }
          rentalAgreement={ rentalAgreement }
        />
      }
      { this.state.showAttachmentRateDialog &&
        <AttachmentRatesEditDialog 
          show={ this.state.showAttachmentRateDialog } 
          attachmentRate={ this.state.attachmentRate } 
          onSave={ this.saveAttachmentRate }
          onSaveMultiple={ this.saveAttachmentRates } 
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
          onSaveMultiple={ this.saveConditions }
          onClose={ this.closeConditionDialog } 
        />
      }
      { this.state.showCloneDialog &&
        <CloneDialog 
          show={ this.state.showCloneDialog }  
          onSave={ this.cloneRentalAgreement } 
          onClose={ this.closeCloneDialog }
          rentalAgreement={ rentalAgreement }
          cloneRentalAgreementError={ this.state.cloneRentalAgreementError  } 
        />
      }
      </div>
    );
  },
});


function mapStateToProps(state) {
  return {
    rentalAgreement: state.models.rentalAgreement,
    notes: state.models.rentalAgreementNotes,
    history: state.models.rentalAgreementHistory,
    rentalConditions: state.lookups.rentalConditions.data,
    provincialRateTypes: state.lookups.provincialRateTypes,
  };
}

export default connect(mapStateToProps)(RentalAgreementsDetail);
