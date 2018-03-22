import React from 'react';

import { connect } from 'react-redux';

import { Well, Alert, Row, Col, PageHeader, ButtonToolbar, Button, ButtonGroup, Glyphicon, Form } from 'react-bootstrap';
import { Link } from 'react-router';

import _ from 'lodash';
import Moment from 'moment';

import RentalRequestsAddDialog from './dialogs/RentalRequestsAddDialog.jsx';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import * as Log from '../history';
import store from '../store';
import { refresh } from '../actions/actions';

import DateControl from '../components/DateControl.jsx';
import DropdownControl from '../components/DropdownControl.jsx';
import EditButton from '../components/EditButton.jsx';
import Favourites from '../components/Favourites.jsx';
import FormInputControl from '../components/FormInputControl.jsx';
import Mailto from '../components/Mailto.jsx';
import MultiDropdown from '../components/MultiDropdown.jsx';
import SortTable from '../components/SortTable.jsx';

import { formatDateTime, startOfCurrentFiscal, endOfCurrentFiscal, startOfPreviousFiscal, endOfPreviousFiscal, toZuluTime } from '../utils/date';

const WITHIN_30_DAYS = 'Within 30 Days';
const THIS_MONTH = 'This Month';
const THIS_QUARTER = 'This Quarter';
const THIS_FISCAL = 'This Fiscal';
const LAST_MONTH = 'Last Month';
const LAST_QUARTER = 'Last Quarter';
const LAST_FISCAL = 'Last Fiscal';
const CUSTOM = 'Custom';

var RentalRequests = React.createClass({
  propTypes: {
    currentUser: React.PropTypes.object,
    rentalRequests: React.PropTypes.object,
    rentalRequest: React.PropTypes.object,
    localAreas: React.PropTypes.object,
    favourites: React.PropTypes.object,
    search: React.PropTypes.object,
    ui: React.PropTypes.object,
    router: React.PropTypes.object,
  },

  getInitialState() {
    return {
      showAddDialog: false,
      search: {
        selectedLocalAreasIds: this.props.search.selectedLocalAreasIds || [],
        projectName: this.props.search.projectName || '',
        status: this.props.search.status || Constant.RENTAL_REQUEST_STATUS_CODE_IN_PROGRESS,
        dateRange: this.props.search.dateRange || '',
      },
      ui : {
        sortField: this.props.ui.sortField || 'localAreaName',
        sortDesc: this.props.ui.sortDesc === true,
      },
    };
  },

  buildSearchParams() {
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
  },

  componentDidMount() {
    Api.getFavourites('rentalRequests').then(() => {
      // If this is the first load, then look for a default favourite
      if (!this.props.search.loaded) {
        var favourite = _.find(this.props.favourites, (favourite) => { return favourite.isDefault; });
        if (favourite) {
          this.loadFavourite(favourite);
          return;
        }
      }
      this.fetch();
    });
  },

  fetch() {
    Api.searchRentalRequests(this.buildSearchParams());
  },

  search(e) {
    e.preventDefault();
    this.fetch();
  },

  updateSearchState(state, callback) {
    this.setState({ search: { ...this.state.search, ...state, ...{ loaded: true } }}, () =>{
      store.dispatch({ type: Action.UPDATE_RENTAL_REQUESTS_SEARCH, rentalRequests: this.state.search });
      if (callback) { callback(); }
    });
  },

  updateUIState(state, callback) {
    this.setState({ ui: { ...this.state.ui, ...state }}, () =>{
      store.dispatch({ type: Action.UPDATE_RENTAL_REQUESTS_UI, rentalRequests: this.state.ui });
      if (callback) { callback(); }
    });
  },

  loadFavourite(favourite) {
    this.updateSearchState(JSON.parse(favourite.value), this.fetch);
  },

  openAddDialog() {
    this.setState({ showAddDialog: true });
  },

  closeAddDialog() {
    this.setState({ showAddDialog: false });
    store.dispatch(refresh(Action.ADD_RENTAL_REQUEST_REFRESH));
  },

  saveNewRequest(request) {
    Api.addRentalRequest(request).then(() => {
      // Open it up
      Log.rentalRequestAdded(this.props.rentalRequest.data);
      this.props.router.push({
        pathname: `${ Constant.RENTAL_REQUESTS_PATHNAME }/${ this.props.rentalRequest.data.id }`,
      });
    });
  },

  print() {
    window.print();
  },

  render() {
    // Constrain the local area drop downs to those in the District of the current logged in user
    var localAreas = _.chain(this.props.localAreas)
      .sortBy('name')
      .value();

    var numRentalRequests = this.props.rentalRequests.loading ? '...' : Object.keys(this.props.rentalRequests.data).length;

    return <div id="rental-requests-list">
      <PageHeader>Rental Requests ({ numRentalRequests })
        <ButtonGroup id="rental-requests-buttons">
          <Button onClick={ this.print }><Glyphicon glyph="print" title="Print" /></Button>
        </ButtonGroup>
      </PageHeader>
      <Well id="rental-requests-bar" bsSize="small" className="clearfix">
        <Form onSubmit={ this.search }>
          <Row>
            <Col sm={10}>
              <Row>
                <ButtonToolbar id="rental-requests-filters">
                  <MultiDropdown id="selectedLocalAreasIds" placeholder="Local Areas"
                    items={ localAreas } selectedIds={ this.state.search.selectedLocalAreasIds } updateState={ this.updateSearchState } showMaxItems={ 2 } />
                  <DropdownControl id="status" title={ this.state.search.status } updateState={ this.updateSearchState } blankLine="(All)" placeholder="Status"
                      items={[ Constant.RENTAL_REQUEST_STATUS_CODE_IN_PROGRESS, Constant.RENTAL_REQUEST_STATUS_CODE_COMPLETED ]} />
                  <FormInputControl id="projectName" type="text" placeholder="Project name" value={ this.state.search.projectName } updateState={ this.updateSearchState }></FormInputControl>
                </ButtonToolbar>
              </Row>
              <Row>
                <ButtonToolbar id="rental-requests-date-filters">
                  <DropdownControl id="dateRange" title={ this.state.search.dateRange } updateState={ this.updateSearchState } blankLine="(All)" placeholder="Expected Start Date"
                      items={[ WITHIN_30_DAYS, THIS_MONTH, THIS_QUARTER, THIS_FISCAL, LAST_MONTH, LAST_QUARTER, LAST_FISCAL, CUSTOM ]}
                  />
                  {(() => {
                    if (this.state.search.dateRange === CUSTOM) {
                      return <span>
                        <DateControl id="startDate" date={ this.state.search.startDate } updateState={ this.updateSearchState } placeholder="mm/dd/yyyy" label="From:" title="start date"/>
                        <DateControl id="endDate" date={ this.state.search.endDate } updateState={ this.updateSearchState } placeholder="mm/dd/yyyy" label="To:" title="end date"/>
                      </span>;
                    }
                  })()}
                </ButtonToolbar>
              </Row>
            </Col>
            <Col sm={2}>
              <Row id="rental-requests-faves">
                <Favourites id="rental-requests-faves-dropdown" type="rentalRequests" favourites={ this.props.favourites.data } data={ this.state.search } onSelect={ this.loadFavourite } pullRight />
              </Row>
              <Row id="rental-requests-search">
                <Button id="search-button" bsStyle="primary" type="submit">Search</Button>
              </Row>
            </Col>
          </Row>
        </Form>
      </Well>

      {(() => {
        var addRentalRequestButton = <Button title="Add Rental Request" bsSize="small" onClick={ this.openAddDialog }>
          <Glyphicon glyph="plus" />&nbsp;<strong>Add Rental Request</strong>
        </Button>;

        if (Object.keys(this.props.rentalRequests).length === 0) { return <Alert bsStyle="success">No Rental Requests { addRentalRequestButton }</Alert>; }

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
          { field: 'equipmentCount',         title: 'Pieces',          style: { textAlign: 'center' } },
          { field: 'districtEquipmentName',  title: 'Equipment Type'                                  },
          { field: 'expectedStartDate',      title: 'Start Date',      style: { textAlign: 'center' } },
          { field: 'expectedEndDate',        title: 'End Date',        style: { textAlign: 'center' } },
          { field: 'projectName',            title: 'Project'                                         },
          { field: 'primaryContactName',     title: 'Primary Contact'                                 },
          { field: 'status',                 title: 'Status',          style: { textAlign: 'center' } },
          { field: 'addRentalRequest',       title: 'Add Project',     style: { textAlign: 'right'  },
            node: addRentalRequestButton,
          },
        ]}>
          {
            _.map(rentalRequests, (request) => {
              return <tr key={ request.id } className={ request.isActive ? null : 'info' }>
                <td>{ request.localAreaName }</td>
                <td style={{ textAlign: 'center' }}>{ request.equipmentCount }</td>
                <td>{ request.districtEquipmentName }</td>
                <td style={{ textAlign: 'center' }}>{ formatDateTime(request.expectedStartDate, Constant.DATE_YEAR_SHORT_MONTH_DAY) }</td>
                <td style={{ textAlign: 'center' }}>{ formatDateTime(request.expectedEndDate, Constant.DATE_YEAR_SHORT_MONTH_DAY) }</td>
                <td>
                  <Link to={ request.projectPath }>{ request.projectName }</Link>
                </td>
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
                    <EditButton name="Rental Request" hide={ !request.canView } view pathname={ `${ Constant.RENTAL_REQUESTS_PATHNAME }/${ request.id }` }/>
                  </ButtonGroup>
                </td>
              </tr>;
            })
          }
        </SortTable>;
      })()}
      <RentalRequestsAddDialog show={ this.state.showAddDialog } onSave={ this.saveNewRequest } onClose={ this.closeAddDialog } />
    </div>;
  },
});


function mapStateToProps(state) {
  return {
    currentUser: state.user,
    rentalRequests: state.models.rentalRequests,
    rentalRequest: state.models.rentalRequest,
    localAreas: state.lookups.localAreas,
    favourites: state.models.favourites,
    search: state.search.rentalRequests,
    ui: state.ui.rentalRequests,
  };
}

export default connect(mapStateToProps)(RentalRequests);
