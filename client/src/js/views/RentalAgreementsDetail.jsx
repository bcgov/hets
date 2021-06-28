import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import { Well, Row, Col, Table, Alert, Button, Badge, ButtonGroup } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import _ from 'lodash';

import EquipmentRentalRatesEditDialog from './dialogs/EquipmentRentalRatesEditDialog.jsx';
import RentalAgreementsEditDialog from './dialogs/RentalAgreementsEditDialog.jsx';
import RentalConditionsEditDialog from './dialogs/RentalConditionsEditDialog.jsx';
import RentalAgreementOvertimeNotesDialog from './dialogs/RentalAgreementOvertimeNotesDialog.jsx';
import RentalRatesEditDialog from './dialogs/RentalRatesEditDialog.jsx';
import CloneDialog from './dialogs/CloneDialog.jsx';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import store from '../store';

import ColDisplay from '../components/ColDisplay.jsx';
import DeleteButton from '../components/DeleteButton.jsx';
import EditButton from '../components/EditButton.jsx';
import Spinner from '../components/Spinner.jsx';
import TooltipButton from '../components/TooltipButton.jsx';
import SubHeader from '../components/ui/SubHeader.jsx';
import ReturnButton from '../components/ReturnButton.jsx';
import Authorize from '../components/Authorize.jsx';

import { activeRentalAgreementSelector, activeRentalAgreementIdSelector } from '../selectors/ui-selectors';

import { buildApiPath } from '../utils/http.js';
import { formatDateTime } from '../utils/date';
import { formatCurrency } from '../utils/string';

class RentalAgreementsDetail extends React.Component {
  static propTypes = {
    rentalAgreement: PropTypes.object,
    rentalAgreementId: PropTypes.number,
    rentalConditions: PropTypes.array,
    ui: PropTypes.object,
    location: PropTypes.object,
    match: PropTypes.object,
  };

  constructor(props) {
    super(props);

    this.state = {
      loading: true,
      rentalAgreementDocumentLoading: false,

      showEditDialog: false,
      showEquipmentRateDialog: false,
      showRentalRateDialog: false,
      showConditionDialog: false,
      showCloneDialog: false,

      rentalRate: {},
      rentalCondition: {},
    };
  }

  componentDidMount() {
    store.dispatch({
      type: Action.SET_ACTIVE_RENTAL_AGREEMENT_ID_UI,
      rentalAgreementId: this.props.match.params.rentalAgreementId,
    });

    const { rentalAgreement } = this.props;

    // Only show loading spinner if there is no existing rental agreement in the store
    if (rentalAgreement) {
      this.setState({ loading: false });
    }

    // Re-fetch rental agreement every time
    Promise.all([this.fetch(rentalAgreement)]).then(() => {
      this.setState({ loading: false });
    });
  }

  fetch = () => {
    return Api.getRentalAgreement(this.props.match.params.rentalAgreementId);
  };

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  openEditDialog = () => {
    this.setState({ showEditDialog: true });
  };

  closeEditDialog = () => {
    this.setState({ showEditDialog: false });
  };

  openEquipmentRateDialog = () => {
    this.setState({ showEquipmentRateDialog: true });
  };

  closeEquipmentRateDialog = () => {
    this.setState({ showEquipmentRateDialog: false });
  };

  openRentalRateDialog = (rentalRate) => {
    this.setState({
      rentalRate,
      showRentalRateDialog: true,
    });
  };

  addRentalRate = (isIncluded) => {
    // New
    this.openRentalRateDialog({
      id: 0,
      isIncludedInTotal: isIncluded,
      rentalAgreement: this.props.rentalAgreement,
    });
  };

  closeRentalRateDialog = () => {
    this.setState({ showRentalRateDialog: false });
  };

  deleteRentalRate = (rentalRate) => {
    Api.deleteRentalRate(rentalRate).then(() => {
      // In addition to refreshing the rental rates, we need to update the rental agreement to get
      // possibly new info.
      this.fetch();
    });
  };

  openConditionDialog = (rentalCondition) => {
    this.setState({
      rentalCondition,
      showConditionDialog: true,
    });
  };

  closeConditionDialog = () => {
    this.setState({ showConditionDialog: false });
  };

  addCondition = () => {
    this.openConditionDialog({
      id: 0,
      rentalAgreement: this.props.rentalAgreement,
    });
  };

  deleteCondition = (rentalCondition) => {
    Api.deleteRentalCondition(rentalCondition).then(() => {
      // In addition to refreshing the rental condition, we need to update the rental agreement to
      // get possibly new info.
      this.fetch();
    });
  };

  openOvertimeNotesDialog = () => {
    this.setState({ showOvertimeNotesDialog: true });
  };

  closeOvertimeNotesDialog = () => {
    this.setState({ showOvertimeNotesDialog: false });
  };

  generateRentalAgreementDocument = () => {
    Api.generateRentalAgreementDocument(this.props.match.params.rentalAgreementId).then(() => {
      window.open(buildApiPath(`/rentalagreements/${this.props.match.params.rentalAgreementId}/doc`));
    });
  };

  openCloneDialog = () => {
    this.setState({ showCloneDialog: true });
  };

  closeCloneDialog = () => {
    this.setState({ showCloneDialog: false });
  };

  render() {
    const { loading } = this.state;
    const rentalAgreement = this.props.rentalAgreement || {};

    var buttons = (
      <div className="pull-right">
        <Authorize>
          <Button disabled={!rentalAgreement.isActive} onClick={this.openCloneDialog}>
            Copy Other Rental Agreement
          </Button>
        </Authorize>
        <Button title="Print PDF" onClick={this.generateRentalAgreementDocument}>
          <FontAwesomeIcon icon="print" />
        </Button>
        <ReturnButton />
      </div>
    );

    return (
      <div id="rental-agreements-detail">
        <Row id="rental-agreements-top">
          <Col xs={2}>
            <div style={{ marginTop: 6 }}>
              {!loading && (
                <Badge bsStyle={rentalAgreement.isActive ? 'success' : 'danger'}>{rentalAgreement.status}</Badge>
              )}
            </div>
          </Col>
          <Col xs={10}>{buttons}</Col>
        </Row>

        <Well id="rental-agreement-header">
          {(() => {
            if (loading) {
              return (
                <div className="spinner-container">
                  <Spinner />
                </div>
              );
            }

            var equipmentDetails =
              rentalAgreement.equipment && rentalAgreement.equipment.id !== 0
                ? `${rentalAgreement.equipment.year} ${rentalAgreement.equipment.make}/${rentalAgreement.equipment.model}/${rentalAgreement.equipment.size}`
                : '';

            return (
              <div>
                <SubHeader title="Rental Agreement" />
                <Row className="equal-height">
                  <Col lg={6} md={6} sm={12} xs={12}>
                    <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Agreement Number:">
                      {rentalAgreement.number}
                    </ColDisplay>
                  </Col>
                  <Col lg={6} md={6} sm={12} xs={12}>
                    <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Rental Request:">
                      <Link to={`${Constant.RENTAL_REQUESTS_PATHNAME}/${rentalAgreement.rentalRequestId}`}>View</Link>
                    </ColDisplay>
                  </Col>
                  <Col lg={6} md={6} sm={12} xs={12}>
                    <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Project:">
                      {rentalAgreement.project.name}
                    </ColDisplay>
                  </Col>
                  <Col lg={6} md={6} sm={12} xs={12}>
                    <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="District:">
                      {rentalAgreement.district.name}
                    </ColDisplay>
                  </Col>
                  <Col lg={6} md={6} sm={12} xs={12}>
                    <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Owner:">
                      {rentalAgreement.ownerName}
                    </ColDisplay>
                  </Col>
                  <Col lg={6} md={6} sm={12} xs={12}>
                    <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="WorkSafeBC (WCB) Number:">
                      {rentalAgreement.equipment.owner.workSafeBcpolicyNumber}
                    </ColDisplay>
                  </Col>
                  <Col lg={6} md={6} sm={12} xs={12}>
                    <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Equipment ID:">
                      <Link to={`${Constant.EQUIPMENT_PATHNAME}/${rentalAgreement.equipment.id}`}>
                        {rentalAgreement.equipment.equipmentCode}
                      </Link>
                    </ColDisplay>
                  </Col>
                  <Col lg={6} md={6} sm={12} xs={12}>
                    <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Equipment Serial Number:">
                      {rentalAgreement.equipment.serialNumber}
                    </ColDisplay>
                  </Col>
                  <Col lg={6} md={6} sm={12} xs={12}>
                    <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Equipment Yr Mk/Md/Sz:">
                      {equipmentDetails}
                    </ColDisplay>
                  </Col>
                  <Col lg={6} md={6} sm={12} xs={12}>
                    <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Point of Hire:">
                      {rentalAgreement.pointOfHire}
                    </ColDisplay>
                  </Col>
                </Row>
              </div>
            );
          })()}
        </Well>

        <Well style={{ minHeight: 120 }}>
          <SubHeader
            title="Details"
            editButtonDisabled={loading}
            editButtonTitle="Edit Details"
            onEditClicked={this.openEditDialog}
          />
          {(() => {
            if (loading) {
              return (
                <div className="spinner-container">
                  <Spinner />
                </div>
              );
            }

            return (
              <Row className="equal-height">
                <Col lg={6} md={6} sm={12} xs={12}>
                  <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Estimated Commencement:">
                    {formatDateTime(rentalAgreement.estimateStartWork, Constant.DATE_YEAR_SHORT_MONTH_DAY)}
                  </ColDisplay>
                </Col>
                <Col lg={6} md={6} sm={12} xs={12}>
                  <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Estimated Period Hours:">
                    {rentalAgreement.estimateHours}
                  </ColDisplay>
                </Col>
                <Col lg={6} md={6} sm={12} xs={12}>
                  <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Dated On:">
                    {formatDateTime(rentalAgreement.datedOn, Constant.DATE_YEAR_SHORT_MONTH_DAY)}
                  </ColDisplay>
                </Col>
                <Col lg={6} md={6} sm={12} xs={12}>
                  <ColDisplay labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Dated At:">
                    {rentalAgreement.agreementCity}
                  </ColDisplay>
                </Col>
              </Row>
            );
          })()}
        </Well>

        <Well>
          <SubHeader title="Equipment Rate and Included Rates and Attachments" />
          {(() => {
            if (loading) {
              return (
                <div className="spinner-container">
                  <Spinner />
                </div>
              );
            }

            return (
              <Row className="rental-rates">
                <Col sm={3} xs={12}>
                  <strong>Pay Rate: </strong>
                  {formatCurrency(rentalAgreement.equipmentRate)}
                </Col>
                <Col sm={3} xs={12}>
                  <strong>Period: </strong>
                  {rentalAgreement.ratePeriod}
                </Col>
                <Col sm={5} xs={12}>
                  <strong>Comment: </strong>
                  {rentalAgreement.rateComment}
                </Col>
                <EditButton name="Equipment Rate" className="edit-rate-btn" onClick={this.openEquipmentRateDialog} />
              </Row>
            );
          })()}

          {(() => {
            if (loading) {
              return;
            }

            // filter to included rates only
            // as-needed rates are shown in the next section
            var includedRates = _.filter(rentalAgreement.rentalAgreementRates, {
              isIncludedInTotal: true,
            });

            var button = (
              <Authorize>
                <TooltipButton
                  title="Add Included Rates and Attachments"
                  bsSize="small"
                  className="no-margin"
                  onClick={this.addRentalRate.bind(this, true)}
                  enabledTooltip="These rates will be added to the total, along with the equipment pay rate."
                >
                  <FontAwesomeIcon icon="plus" className="mr-5" />
                  <span>Add Included Rates and Attachments</span>
                </TooltipButton>
              </Authorize>
            );

            if (Object.keys(includedRates || []).length === 0) {
              return (
                <div>
                  <Alert bsStyle="success">No included rates or attachments</Alert>
                  {button}
                </div>
              );
            }

            // newly-added rates (with an id of 0) need to appear at the end of the list
            includedRates = _.orderBy(includedRates, [(r) => r.id === 0, (r) => r.id], ['asc', 'asc']);

            return (
              <div id="included-rates">
                <Table striped condensed hover bordered>
                  <thead>
                    <tr>
                      <th>Rate</th>
                      <th>Period</th>
                      <th>Comment</th>
                      <th></th>
                    </tr>
                  </thead>
                  <tbody>
                    {_.map(includedRates, (obj, i) => {
                      return (
                        <tr key={obj.id || i}>
                          <td>{formatCurrency(obj.rate)}</td>
                          <td>{obj.ratePeriod}</td>
                          <td>{obj.comment}</td>
                          <td style={{ textAlign: 'right' }}>
                            <ButtonGroup>
                              <Authorize>
                                <DeleteButton
                                  name="Rate or Attachment"
                                  disabled={!obj.id}
                                  onConfirm={this.deleteRentalRate.bind(this, obj)}
                                />
                              </Authorize>
                              <EditButton
                                name="Rate or Attachment"
                                disabled={!obj.id}
                                onClick={this.openRentalRateDialog.bind(this, obj)}
                              />
                            </ButtonGroup>
                          </td>
                        </tr>
                      );
                    })}
                  </tbody>
                </Table>
                {button}
              </div>
            );
          })()}
        </Well>

        <Well>
          <SubHeader title="As-Needed Rates and Attachments" />
          {(() => {
            if (loading) {
              return (
                <div className="spinner-container">
                  <Spinner />
                </div>
              );
            }

            // filter to as-needed rates only
            // included rates are shown in the previous section
            var asNeededRates = _.filter(rentalAgreement.rentalAgreementRates, {
              isIncludedInTotal: false,
            });

            var button = (
              <Authorize>
                <TooltipButton
                  title="Add Other Rates and Attachments"
                  bsSize="small"
                  className="no-margin"
                  onClick={this.addRentalRate.bind(this, false)}
                  enabledTooltip="These rates will NOT be added to the total."
                >
                  <FontAwesomeIcon icon="plus" className="mr-5" />
                  <span>Add Other Rates and Attachments</span>
                </TooltipButton>
              </Authorize>
            );

            if (Object.keys(asNeededRates || []).length === 0) {
              return (
                <div>
                  <Alert bsStyle="success">No as-needed rates or attachments</Alert>
                  {button}
                </div>
              );
            }

            // newly-added rates (with an id of 0) need to appear at the end of the list
            asNeededRates = _.orderBy(asNeededRates, [(r) => r.id === 0, (r) => r.id], ['asc', 'asc']);

            return (
              <div id="as-needed-rates">
                <Table striped condensed hover bordered>
                  <thead>
                    <tr>
                      <th>Rate</th>
                      <th>Period</th>
                      <th>Comment</th>
                      <th></th>
                    </tr>
                  </thead>
                  <tbody>
                    {_.map(asNeededRates, (obj, i) => {
                      return (
                        <tr key={obj.id || i}>
                          <td>{formatCurrency(obj.rate)}</td>
                          <td>{obj.set ? Constant.RENTAL_RATE_PERIOD_SET : obj.ratePeriod}</td>
                          <td>{obj.comment}</td>
                          <td style={{ textAlign: 'right' }}>
                            <ButtonGroup>
                              <Authorize>
                                <DeleteButton
                                  name="Rate or Attachment"
                                  disabled={!obj.id}
                                  onConfirm={this.deleteRentalRate.bind(this, obj)}
                                />
                              </Authorize>
                              <EditButton
                                name="Rate or Attachment"
                                disabled={!obj.id}
                                onClick={this.openRentalRateDialog.bind(this, obj)}
                              />
                            </ButtonGroup>
                          </td>
                        </tr>
                      );
                    })}
                  </tbody>
                </Table>
                {button}
              </div>
            );
          })()}
        </Well>

        <Well>
          <SubHeader title="Conditions" />
          {(() => {
            if (loading) {
              return (
                <div className="spinner-container">
                  <Spinner />
                </div>
              );
            }

            // newly-added conditions (with an id of 0) need to appear at the end of the list
            var rentalConditions = _.orderBy(
              rentalAgreement.rentalAgreementConditions,
              [(c) => c.id === 0, (c) => c.id],
              ['asc', 'asc']
            );

            var button = (
              <Authorize>
                <Button title="Add Rental Condition" bsSize="small" className="no-margin" onClick={this.addCondition}>
                  <FontAwesomeIcon icon="plus" className="mr-5" />
                  <span>Add</span>
                </Button>
              </Authorize>
            );

            if (Object.keys(rentalConditions || []).length === 0) {
              return (
                <div>
                  <Alert bsStyle="success">No rental conditions</Alert>
                  {button}
                </div>
              );
            }

            return (
              <div id="rental-conditions">
                <Table striped condensed hover bordered>
                  <thead>
                    <tr>
                      <th>Condition</th>
                      <th>Comment</th>
                      <th></th>
                    </tr>
                  </thead>
                  <tbody>
                    {_.map(rentalConditions, (obj, i) => {
                      return (
                        <tr key={obj.id || i}>
                          <td>{obj.conditionName}</td>
                          <td>{obj.comment}</td>
                          <td style={{ textAlign: 'right' }}>
                            <ButtonGroup>
                              <Authorize>
                                <DeleteButton
                                  name="Rental Condition"
                                  disabled={!obj.id}
                                  onConfirm={this.deleteCondition.bind(this, obj)}
                                />
                              </Authorize>
                              <EditButton
                                name="Rental Condition"
                                disabled={!obj.id}
                                onClick={this.openConditionDialog.bind(this, obj)}
                              />
                            </ButtonGroup>
                          </td>
                        </tr>
                      );
                    })}
                  </tbody>
                </Table>
                {button}
              </div>
            );
          })()}
        </Well>

        <Well>
          <SubHeader
            title="Overtime Rates and Notes/Special Instructions"
            editButtonDisabled={loading}
            editButtonTitle="Edit Overtime Rates and Notes/Special Instructions"
            onEditClicked={this.openOvertimeNotesDialog}
          />
          {(() => {
            if (loading) {
              return (
                <div className="spinner-container">
                  <Spinner />
                </div>
              );
            }

            var rates = _.orderBy(rentalAgreement.overtimeRates, ['comment'], ['desc']);

            return (
              <ColDisplay id="overtime-rates" labelProps={{ xs: 4 }} fieldProps={{ xs: 8 }} label="Overtime Rates:">
                {rates.flatMap((rate) =>
                  rate.active ? (
                    <span key={rate.id} className="overtime-rate">
                      {rate.comment}
                    </span>
                  ) : (
                    []
                  )
                )}
              </ColDisplay>
            );
          })()}
          <ColDisplay
            id="rental-agreements-note"
            style={{ whiteSpace: 'pre-line' }}
            labelProps={{ xs: 4 }}
            fieldProps={{ xs: 8 }}
            label="Notes/Special Instructions:"
          >
            {rentalAgreement && rentalAgreement.note}
          </ColDisplay>
        </Well>

        <Row id="rental-agreements-footer">{buttons}</Row>
        {this.state.showEditDialog && (
          <RentalAgreementsEditDialog
            show={this.state.showEditDialog}
            rentalAgreement={rentalAgreement}
            onClose={this.closeEditDialog}
          />
        )}
        {this.state.showEquipmentRateDialog && (
          <EquipmentRentalRatesEditDialog
            show={this.state.showEquipmentRateDialog}
            rentalAgreement={rentalAgreement}
            onSave={this.fetch}
            onClose={this.closeEquipmentRateDialog}
          />
        )}
        {this.state.showRentalRateDialog && (
          <RentalRatesEditDialog
            show={this.state.showRentalRateDialog}
            rentalRate={this.state.rentalRate}
            rentalAgreement={rentalAgreement}
            onSave={this.fetch}
            onClose={this.closeRentalRateDialog}
          />
        )}
        {this.state.showConditionDialog && (
          <RentalConditionsEditDialog
            show={this.state.showConditionDialog}
            rentalAgreementId={rentalAgreement.id}
            rentalCondition={this.state.rentalCondition}
            onSave={this.fetch}
            onClose={this.closeConditionDialog}
          />
        )}
        {this.state.showOvertimeNotesDialog && (
          <RentalAgreementOvertimeNotesDialog
            show={this.state.showOvertimeNotesDialog}
            rentalAgreement={rentalAgreement}
            onSave={this.fetch}
            onClose={this.closeOvertimeNotesDialog}
          />
        )}
        {this.state.showCloneDialog && (
          <CloneDialog
            show={this.state.showCloneDialog}
            rentalAgreement={rentalAgreement}
            onSave={this.fetch}
            onClose={this.closeCloneDialog}
          />
        )}
      </div>
    );
  }
}

function mapStateToProps(state) {
  return {
    rentalAgreement: activeRentalAgreementSelector(state),
    rentalAgreementId: activeRentalAgreementIdSelector(state),
  };
}

export default connect(mapStateToProps)(RentalAgreementsDetail);
