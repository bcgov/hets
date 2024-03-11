import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { Alert, Row, Col, ButtonToolbar, Button, ButtonGroup, Form } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Link } from 'react-router-dom';
import _ from 'lodash';
import Moment from 'moment';

import RentalRequestsAddDialog from './dialogs/RentalRequestsAddDialog.jsx';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import * as Log from '../history';

import AddButtonContainer from '../components/ui/AddButtonContainer.jsx';
import PageHeader from '../components/ui/PageHeader.jsx';
import SearchBar from '../components/ui/SearchBar.jsx';
import DateControl from '../components/DateControl.jsx';
import DeleteButton from '../components/DeleteButton.jsx';
import DropdownControl from '../components/DropdownControl.jsx';
import EditButton from '../components/EditButton.jsx';
import Favourites from '../components/Favourites.jsx';
import FormInputControl from '../components/FormInputControl.jsx';
import Mailto from '../components/Mailto.jsx';
import MultiDropdown from '../components/MultiDropdown.jsx';
import SortTable from '../components/SortTable.jsx';
import Spinner from '../components/Spinner.jsx';
import PrintButton from '../components/PrintButton.jsx';
import Authorize from '../components/Authorize.jsx';

import {
  formatDateTime,
  startOfCurrentFiscal,
  endOfCurrentFiscal,
  startOfPreviousFiscal,
  endOfPreviousFiscal,
  toZuluTime,
} from '../utils/date';

const WITHIN_30_DAYS = 'Within 30 Days';
const THIS_MONTH = 'This Month';
const THIS_QUARTER = 'This Quarter';
const THIS_FISCAL = 'This Fiscal';
const LAST_MONTH = 'Last Month';
const LAST_QUARTER = 'Last Quarter';
const LAST_FISCAL = 'Last Fiscal';
const CUSTOM = 'Custom';

class RentalRequests extends React.Component {
  static propTypes = {
    currentUser: PropTypes.object,
    rentalRequests: PropTypes.object,
    rentalRequest: PropTypes.object,
    localAreas: PropTypes.object,
    favourites: PropTypes.object,
    search: PropTypes.object,
    ui: PropTypes.object,
    history: PropTypes.object,
  };

  constructor(props) {
    super(props);

    this.state = {
      showAddDialog: false,
      addViewOnly: false,
      search: {
        selectedLocalAreasIds: props.search.selectedLocalAreasIds || [],
        projectName: props.search.projectName || '',
        status: props.search.status || Constant.RENTAL_REQUEST_STATUS_CODE_IN_PROGRESS,
        dateRange: props.search.dateRange || '',
      },
      ui: {
        sortField: props.ui.sortField || 'projectName',
        sortDesc: props.ui.sortDesc === true,
      },
    };
  }

  buildSearchParams = () => {
    var searchParams = {
      status: this.state.search.status || '',
      project: this.state.search.projectName || '',
    };

    if (this.state.search.selectedLocalAreasIds.length > 0) {
      searchParams.localAreas = this.state.search.selectedLocalAreasIds;
    }

    // Time period drop-down; e.g. "This Month"
    var startDate;
    var endDate;
    var today = Moment();

    switch (this.state.search.dateRange) {
      case WITHIN_30_DAYS:
        endDate = today.add(30, 'day');
        break;
      case THIS_MONTH:
        startDate = today.startOf('month');
        endDate = Moment(startDate).endOf('month');
        break;
      case THIS_QUARTER:
        startDate = today.startOf('quarter');
        endDate = Moment(startDate).endOf('quarter');
        break;
      case THIS_FISCAL:
        // Fiscal Year: Apr 1 - March 31
        startDate = startOfCurrentFiscal(today);
        endDate = endOfCurrentFiscal(today);
        break;
      case LAST_MONTH:
        startDate = today.subtract(1, 'month').startOf('month');
        endDate = Moment(startDate).endOf('month');
        break;
      case LAST_QUARTER:
        startDate = today.subtract(1, 'quarter').startOf('quarter');
        endDate = Moment(startDate).endOf('quarter');
        break;
      case LAST_FISCAL:
        // Fiscal Year: Apr 1 - March 31
        startDate = startOfPreviousFiscal(today);
        endDate = endOfPreviousFiscal(today);
        break;
      case CUSTOM:
        startDate = Moment(this.state.search.startDate);
        endDate = Moment(this.state.search.endDate);
        break;
      default:
        break;
    }

    if (startDate && startDate.isValid()) {
      searchParams.startDate = toZuluTime(startDate.startOf('day'));
    }
    if (endDate && endDate.isValid()) {
      searchParams.endDate = toZuluTime(endDate.startOf('day'));
    }

    return searchParams;
  };

  componentDidMount() {
    var defaultFavourite = null;
    // If this is the first load, then look for a default favourite
    if (_.isEmpty(this.props.search)) {
      defaultFavourite = _.find(this.props.favourites, (f) => f.isDefault);
    }

    if (defaultFavourite) {
      this.loadFavourite(defaultFavourite); // also fetches
    } else if (this.props.rentalRequests.loaded) {
      // if a search was performed previously, refresh the search results
      this.fetch();
    }
  }

  fetch = () => {
    this.props.dispatch(Api.searchRentalRequests(this.buildSearchParams()));
  };

  search = (e) => {
    e.preventDefault();
    this.fetch();
  };

  clearSearch = () => {
    const dispatch = this.props.dispatch;
    const defaultSearchParameters = {
      selectedLocalAreasIds: [],
      projectName: '',
      status: Constant.RENTAL_REQUEST_STATUS_CODE_IN_PROGRESS,
      dateRange: '',
    };

    this.setState({ search: defaultSearchParameters }, () => {
      dispatch({ type: Action.UPDATE_RENTAL_REQUESTS_SEARCH, rentalRequests: this.state.search });
      dispatch({ type: Action.CLEAR_RENTAL_REQUESTS });
    });
  };

  updateSearchState = (state, callback) => {
    const dispatch = this.props.dispatch;
    this.setState({ search: { ...this.state.search, ...state, ...{ loaded: true } } }, () => {
      dispatch({ type: Action.UPDATE_RENTAL_REQUESTS_SEARCH, rentalRequests: this.state.search });
      if (callback) {
        callback();
      }
    });
  };

  updateUIState = (state, callback) => {
    const dispatch = this.props.dispatch;
    this.setState({ ui: { ...this.state.ui, ...state } }, () => {
      dispatch({ type: Action.UPDATE_RENTAL_REQUESTS_UI, rentalRequests: this.state.ui });
      if (callback) {
        callback();
      }
    });
  };

  loadFavourite = (favourite) => {
    this.updateSearchState(JSON.parse(favourite.value), this.fetch);
  };

  deleteRequest = async (request) => {
    await this.props.dispatch(Api.cancelRentalRequest(request.id));
    this.fetch();
  };

  openAddDialog = (viewOnly) => {
    this.setState({ showAddDialog: true, addViewOnly: viewOnly });
  };

  closeAddDialog = () => {
    this.setState({ showAddDialog: false });
  };

  newRentalAdded = async (rentalRequest) => {
    await this.props.dispatch(Log.rentalRequestAdded(rentalRequest));
    this.props.history.push(`${Constant.RENTAL_REQUESTS_PATHNAME}/${rentalRequest.id}`);
  };

  renderResults = (addRequestButtons) => {
    if (Object.keys(this.props.rentalRequests.data).length === 0) {
      return <Alert variant="success">No Rental Requests {addRequestButtons}</Alert>;
    }

    var rentalRequests = _.sortBy(this.props.rentalRequests.data, (rentalRequest) => {
      var sortValue = rentalRequest[this.state.ui.sortField];
      if (typeof sortValue === 'string') {
        return sortValue.toLowerCase();
      }
      return sortValue;
    });

    if (this.state.ui.sortDesc) {
      _.reverse(rentalRequests);
    }

    return (
      <SortTable
        sortField={this.state.ui.sortField}
        sortDesc={this.state.ui.sortDesc}
        onSort={this.updateUIState}
        headers={[
          { field: 'localAreaName', title: 'Local Area' },
          { field: 'equipmentCount', title: 'Quantity', style: { textAlign: 'center' } },
          { field: 'districtEquipmentName', title: 'Equipment Type' },
          { field: 'expectedStartDate', title: 'Start Date', style: { textAlign: 'center' } },
          { field: 'expectedEndDate', title: 'End Date', style: { textAlign: 'center' } },
          { field: 'projectName', title: 'Project' },
          { field: 'primaryContactName', title: 'Primary Contact' },
          { field: 'status', title: 'Status', style: { textAlign: 'center' } },
          { field: 'addRentalRequest', title: 'Add Project', style: { textAlign: 'right' }, node: addRequestButtons },
        ]}
      >
        {_.map(rentalRequests, (request) => {
          var projectLink = request.projectId ? (
            <Link to={request.projectPath}>{request.projectName}</Link>
          ) : (
            request.projectName
          );

          return (
            <tr key={request.id} className={request.isActive ? null : 'bg-info'}>
              <td>{request.localAreaName}</td>
              <td style={{ textAlign: 'center' }}>{request.equipmentCount}</td>
              <td>{request.districtEquipmentName}</td>
              <td style={{ textAlign: 'center' }}>
                {formatDateTime(request.expectedStartDate, Constant.DATE_YEAR_SHORT_MONTH_DAY)}
              </td>
              <td style={{ textAlign: 'center' }}>
                {formatDateTime(request.expectedEndDate, Constant.DATE_YEAR_SHORT_MONTH_DAY)}
              </td>
              <td>{projectLink}</td>
              <td>
                {request.primaryContactName ? (
                  <Mailto email={request.primaryContactEmail}>{request.primaryContactName}</Mailto>
                ) : (
                  'None'
                )}
              </td>
              <td style={{ textAlign: 'center' }}>{request.status}</td>
              <td style={{ textAlign: 'right' }}>
                <ButtonGroup>
                  {request.canDelete && (
                    <Authorize requires={Constant.PERMISSION_WRITE_ACCESS}>
                      <DeleteButton name="Rental Request" onConfirm={this.deleteRequest.bind(this, request)} />
                    </Authorize>
                  )}
                  {request.canView && (
                    <EditButton
                      name="Rental Request"
                      view
                      pathname={`${Constant.RENTAL_REQUESTS_PATHNAME}/${request.id}`}
                    />
                  )}
                </ButtonGroup>
              </td>
            </tr>
          );
        })}
      </SortTable>
    );
  };

  render() {
    // Constrain the local area drop downs to those in the District of the current logged in user
    var localAreas = _.chain(this.props.localAreas).sortBy('name').value();

    var resultCount = '';
    if (this.props.rentalRequests.loaded) {
      resultCount = '(' + Object.keys(this.props.rentalRequests.data).length + ')';
    }

    return (
      <div id="rental-requests-list">
        <PageHeader>
          Rental Requests {resultCount}
          <ButtonGroup>
            <PrintButton disabled={!this.props.rentalRequests.loaded} />
          </ButtonGroup>
        </PageHeader>
        <SearchBar id="rental-requests-bar">
          <Form onSubmit={this.search}>
            <Row>
              <Col xs={9} sm={10} id="filters">
                <Row>
                  <ButtonToolbar>
                    <MultiDropdown
                      id="selectedLocalAreasIds"
                      placeholder="Local Areas"
                      items={localAreas}
                      selectedIds={this.state.search.selectedLocalAreasIds}
                      updateState={this.updateSearchState}
                      showMaxItems={2}
                    />
                    <DropdownControl
                      id="status"
                      title={this.state.search.status}
                      updateState={this.updateSearchState}
                      blankLine="(All)"
                      placeholder="Status"
                      items={[
                        Constant.RENTAL_REQUEST_STATUS_CODE_IN_PROGRESS,
                        Constant.RENTAL_REQUEST_STATUS_CODE_COMPLETED,
                      ]}
                    />
                    <FormInputControl
                      id="projectName"
                      type="text"
                      placeholder="Project name"
                      value={this.state.search.projectName}
                      updateState={this.updateSearchState}
                    ></FormInputControl>
                    <DropdownControl
                      id="dateRange"
                      title={this.state.search.dateRange}
                      updateState={this.updateSearchState}
                      blankLine="(All)"
                      placeholder="Expected Start Date"
                      items={[
                        WITHIN_30_DAYS,
                        THIS_MONTH,
                        THIS_QUARTER,
                        THIS_FISCAL,
                        LAST_MONTH,
                        LAST_QUARTER,
                        LAST_FISCAL,
                        CUSTOM,
                      ]}
                    />
                    <Button id="search-button" variant="primary" type="submit">
                      Search
                    </Button>
                    <Button className="btn-custom" id="clear-search-button" onClick={this.clearSearch}>
                      Clear
                    </Button>
                  </ButtonToolbar>
                </Row>
                {(() => {
                  if (this.state.search.dateRange === CUSTOM) {
                    return (
                      <Row>
                        <ButtonToolbar id="rental-requests-custom-date-filters">
                          <DateControl
                            id="startDate"
                            date={this.state.search.startDate}
                            updateState={this.updateSearchState}
                            label="From:"
                            title="start date"
                          />
                          <DateControl
                            id="endDate"
                            date={this.state.search.endDate}
                            updateState={this.updateSearchState}
                            label="To:"
                            title="end date"
                          />
                        </ButtonToolbar>
                      </Row>
                    );
                  }
                })()}
              </Col>
              <Col xs={3} sm={2} id="search-buttons">
                <Row className="float-right">
                  <Favourites
                    id="rental-requests-faves-dropdown"
                    type="rentalRequests"
                    favourites={this.props.favourites}
                    data={this.state.search}
                    onSelect={this.loadFavourite}
                  />
                </Row>
              </Col>
            </Row>
          </Form>
        </SearchBar>

        {(() => {
          if (this.props.rentalRequests.loading) {
            return (
              <div style={{ textAlign: 'center' }}>
                <Spinner />
              </div>
            );
          }

          var addViewOnlyRequestButton = (
            <Authorize requires={Constant.PERMISSION_WRITE_ACCESS}>
              <Button
                title="Add Rental Request (View Only)"
                className="d-print-none btn-custom"
                size="sm"
                onClick={() => this.openAddDialog(true)}
              >
                <FontAwesomeIcon icon="plus" />
                &nbsp;<strong>Request (View Only)</strong>
              </Button>
            </Authorize>
          );

          var addRentalRequestButton = (
            <Authorize requires={Constant.PERMISSION_WRITE_ACCESS}>
              <Button
                title="Add Rental Request"
                className="d-print-none btn-custom"
                size="sm"
                onClick={() => this.openAddDialog(false)}
              >
                <FontAwesomeIcon icon="plus" />
                &nbsp;<strong>Add Rental Request</strong>
              </Button>
            </Authorize>
          );

          var addRequestButtons = (
            <div id="add-request-buttons">
              {addRentalRequestButton}
              {addViewOnlyRequestButton}
            </div>
          );

          if (this.props.rentalRequests.loaded) {
            return this.renderResults(addRequestButtons);
          }

          return <AddButtonContainer>{addRequestButtons}</AddButtonContainer>;
        })()}
        {this.state.showAddDialog && (
          <RentalRequestsAddDialog
            show={this.state.showAddDialog}
            viewOnly={this.state.addViewOnly}
            onRentalAdded={this.newRentalAdded}
            onClose={this.closeAddDialog}
          />
        )}
      </div>
    );
  }
}

const mapStateToProps = (state) => ({
  currentUser: state.user,
  rentalRequests: state.models.rentalRequests,
  localAreas: state.lookups.localAreas,
  favourites: state.models.favourites.rentalRequests,
  search: state.search.rentalRequests,
  ui: state.ui.rentalRequests,
});

const mapDispatchToProps = (dispatch) => ({ dispatch });

export default connect(mapStateToProps, mapDispatchToProps)(RentalRequests);
