import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { Alert, Row, Col, ButtonToolbar, Button, ButtonGroup, Glyphicon, Form } from 'react-bootstrap';
import { Link } from 'react-router';
import _ from 'lodash';
import Moment from 'moment';

import RentalRequestsAddDialog from './dialogs/RentalRequestsAddDialog.jsx';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import * as Log from '../history';
import store from '../store';

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

import { formatDateTime, startOfCurrentFiscal, endOfCurrentFiscal, startOfPreviousFiscal, endOfPreviousFiscal, toZuluTime } from '../utils/date';

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
    router: PropTypes.object,
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
      ui : {
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
      defaultFavourite = _.find(this.props.favourites, f => f.isDefault);
    }

    if (defaultFavourite) {
      this.loadFavourite(defaultFavourite); // also fetches
    } else if (this.props.rentalRequests.loaded) {
      // if a search was performed previously, refresh the search results
      this.fetch();
    }
  }

  fetch = () => {
    Api.searchRentalRequests(this.buildSearchParams());
  };

  search = (e) => {
    e.preventDefault();
    this.fetch();
  };

  clearSearch = () => {
    var defaultSearchParameters = {
      selectedLocalAreasIds: [],
      projectName: '',
      status: Constant.RENTAL_REQUEST_STATUS_CODE_IN_PROGRESS,
      dateRange: '',
    };

    this.setState({ search: defaultSearchParameters }, () => {
      store.dispatch({ type: Action.UPDATE_RENTAL_REQUESTS_SEARCH, rentalRequests: this.state.search });
      store.dispatch({ type: Action.CLEAR_RENTAL_REQUESTS });
    });
  };

  updateSearchState = (state, callback) => {
    this.setState({ search: { ...this.state.search, ...state, ...{ loaded: true } }}, () =>{
      store.dispatch({ type: Action.UPDATE_RENTAL_REQUESTS_SEARCH, rentalRequests: this.state.search });
      if (callback) { callback(); }
    });
  };

  updateUIState = (state, callback) => {
    this.setState({ ui: { ...this.state.ui, ...state }}, () =>{
      store.dispatch({ type: Action.UPDATE_RENTAL_REQUESTS_UI, rentalRequests: this.state.ui });
      if (callback) { callback(); }
    });
  };

  loadFavourite = (favourite) => {
    this.updateSearchState(JSON.parse(favourite.value), this.fetch);
  };

  deleteRequest = (request) => {
    Api.cancelRentalRequest(request.id).then(() => {
      this.fetch();
    });
  };

  openAddDialog = (viewOnly) => {
    this.setState({ showAddDialog: true, addViewOnly: viewOnly });
  };

  closeAddDialog = () => {
    this.setState({ showAddDialog: false });
  };

  newRentalAdded = (rentalRequest) => {
    Log.rentalRequestAdded(rentalRequest);

    this.props.router.push({
      pathname: `${ Constant.RENTAL_REQUESTS_PATHNAME }/${ rentalRequest.id }`,
    });
  };

  renderResults = (addRequestButtons) => {
    if (Object.keys(this.props.rentalRequests.data).length === 0) { return <Alert bsStyle="success">No Rental Requests { addRequestButtons }</Alert>; }

    var rentalRequests = _.sortBy(this.props.rentalRequests.data, rentalRequest => {
      var sortValue = rentalRequest[this.state.ui.sortField];
      if (typeof sortValue === 'string') {
        return sortValue.toLowerCase();
      }
      return sortValue;
    });

    if (this.state.ui.sortDesc) {
      _.reverse(rentalRequests);
    }

    return <SortTable sortField={ this.state.ui.sortField } sortDesc={ this.state.ui.sortDesc } onSort={ this.updateUIState } headers={[
      { field: 'localAreaName',          title: 'Local Area'                                      },
      { field: 'equipmentCount',         title: 'Quantity',        style: { textAlign: 'center' } },
      { field: 'districtEquipmentName',  title: 'Equipment Type'                                  },
      { field: 'expectedStartDate',      title: 'Start Date',      style: { textAlign: 'center' } },
      { field: 'expectedEndDate',        title: 'End Date',        style: { textAlign: 'center' } },
      { field: 'projectName',            title: 'Project'                                         },
      { field: 'primaryContactName',     title: 'Primary Contact'                                 },
      { field: 'status',                 title: 'Status',          style: { textAlign: 'center' } },
      { field: 'addRentalRequest',       title: 'Add Project',     style: { textAlign: 'right'  },
        node: addRequestButtons,
      },
    ]}>
      {
        _.map(rentalRequests, (request) => {
          var projectLink = request.projectId ? <Link to={ request.projectPath }>{ request.projectName }</Link> : request.projectName;

          return <tr key={ request.id } className={ request.isActive ? null : 'info' }>
            <td>{ request.localAreaName }</td>
            <td style={{ textAlign: 'center' }}>{ request.equipmentCount }</td>
            <td>{ request.districtEquipmentName }</td>
            <td style={{ textAlign: 'center' }}>{ formatDateTime(request.expectedStartDate, Constant.DATE_YEAR_SHORT_MONTH_DAY) }</td>
            <td style={{ textAlign: 'center' }}>{ formatDateTime(request.expectedEndDate, Constant.DATE_YEAR_SHORT_MONTH_DAY) }</td>
            <td>{ projectLink }</td>
            <td>
              {
                request.primaryContactName ?
                  <Mailto email={ request.primaryContactEmail }>{ request.primaryContactName }</Mailto> :
                  'None'
              }
            </td>
            <td style={{ textAlign: 'center' }}>{ request.status }</td>
            <td style={{ textAlign: 'right' }}>
              <ButtonGroup>
                {request.canDelete && (
                  <Authorize><DeleteButton name="Rental Request" onConfirm={ this.deleteRequest.bind(this, request) } /></Authorize>
                )}
                {request.canView && (
                  <EditButton name="Rental Request" view pathname={ `${ Constant.RENTAL_REQUESTS_PATHNAME }/${ request.id }` }/>
                )}
              </ButtonGroup>
            </td>
          </tr>;
        })
      }
    </SortTable>;
  };

  render() {
    // Constrain the local area drop downs to those in the District of the current logged in user
    var localAreas = _.chain(this.props.localAreas)
      .sortBy('name')
      .value();

    var resultCount = '';
    if (this.props.rentalRequests.loaded) {
      resultCount = '(' + Object.keys(this.props.rentalRequests.data).length + ')';
    }

    return <div id="rental-requests-list">
      <PageHeader>Rental Requests { resultCount }
        <ButtonGroup>
          <PrintButton disabled={!this.props.rentalRequests.loaded}/>
        </ButtonGroup>
      </PageHeader>
      <SearchBar id="rental-requests-bar">
        <Form onSubmit={ this.search }>
          <Row>
            <Col xs={9} sm={10} id="filters">
              <Row>
                <ButtonToolbar>
                  <MultiDropdown id="selectedLocalAreasIds" placeholder="Local Areas"
                    items={ localAreas } selectedIds={ this.state.search.selectedLocalAreasIds } updateState={ this.updateSearchState } showMaxItems={ 2 } />
                  <DropdownControl id="status" title={ this.state.search.status } updateState={ this.updateSearchState } blankLine="(All)" placeholder="Status"
                    items={[ Constant.RENTAL_REQUEST_STATUS_CODE_IN_PROGRESS, Constant.RENTAL_REQUEST_STATUS_CODE_COMPLETED ]} />
                  <FormInputControl id="projectName" type="text" placeholder="Project name" value={ this.state.search.projectName } updateState={ this.updateSearchState }></FormInputControl>
                  <DropdownControl id="dateRange" title={ this.state.search.dateRange } updateState={ this.updateSearchState } blankLine="(All)" placeholder="Expected Start Date"
                    items={[ WITHIN_30_DAYS, THIS_MONTH, THIS_QUARTER, THIS_FISCAL, LAST_MONTH, LAST_QUARTER, LAST_FISCAL, CUSTOM ]}
                  />
                  <Button id="search-button" bsStyle="primary" type="submit">Search</Button>
                  <Button id="clear-search-button" onClick={ this.clearSearch }>Clear</Button>
                </ButtonToolbar>
              </Row>
              {(() => {
                if (this.state.search.dateRange === CUSTOM) {
                  return <Row>
                    <ButtonToolbar id="rental-requests-custom-date-filters">
                      <DateControl id="startDate" date={ this.state.search.startDate } updateState={ this.updateSearchState } label="From:" title="start date"/>
                      <DateControl id="endDate" date={ this.state.search.endDate } updateState={ this.updateSearchState } label="To:" title="end date"/>
                    </ButtonToolbar>
                  </Row>;
                }
              })()}
            </Col>
            <Col xs={3} sm={2} id="search-buttons">
              <Row>
                <Favourites id="rental-requests-faves-dropdown" type="rentalRequests" favourites={ this.props.favourites } data={ this.state.search } onSelect={ this.loadFavourite } pullRight />
              </Row>
            </Col>
          </Row>
        </Form>
      </SearchBar>

      {(() => {
        if (this.props.rentalRequests.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

        var addViewOnlyRequestButton = (
          <Authorize><Button title="Add Rental Request (View Only)" className="hidden-print" bsSize="xsmall" onClick={ () => this.openAddDialog(true) }>
            <Glyphicon glyph="plus" />&nbsp;<strong>Request (View Only)</strong>
          </Button></Authorize>
        );

        var addRentalRequestButton = (
          <Authorize><Button title="Add Rental Request" className="hidden-print" bsSize="xsmall" onClick={ () => this.openAddDialog(false) }>
            <Glyphicon glyph="plus" />&nbsp;<strong>Add Rental Request</strong>
          </Button></Authorize>
        );

        var addRequestButtons = <div id="add-request-buttons">
          { addRentalRequestButton }
          { addViewOnlyRequestButton }
        </div>;

        if (this.props.rentalRequests.loaded) {
          return this.renderResults(addRequestButtons);
        }

        return <AddButtonContainer>{ addRequestButtons }</AddButtonContainer>;
      })()}
      { this.state.showAddDialog && (
        <RentalRequestsAddDialog
          show={ this.state.showAddDialog }
          viewOnly={ this.state.addViewOnly }
          onRentalAdded={ this.newRentalAdded }
          onClose={ this.closeAddDialog } />
      )}
    </div>;
  }
}


function mapStateToProps(state) {
  return {
    currentUser: state.user,
    rentalRequests: state.models.rentalRequests,
    localAreas: state.lookups.localAreas,
    favourites: state.models.favourites.rentalRequests,
    search: state.search.rentalRequests,
    ui: state.ui.rentalRequests,
  };
}

export default connect(mapStateToProps)(RentalRequests);
