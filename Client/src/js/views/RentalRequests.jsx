import React from 'react';

import { connect } from 'react-redux';

import { Well, Alert, Row, Col } from 'react-bootstrap';
import { PageHeader, ButtonToolbar, Button, ButtonGroup, Glyphicon } from 'react-bootstrap';
import { Link } from 'react-router';
import { LinkContainer } from 'react-router-bootstrap';

import _ from 'lodash';
import Moment from 'moment';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import store from '../store';

import DateControl from '../components/DateControl.jsx';
import DropdownControl from '../components/DropdownControl.jsx';
import Favourites from '../components/Favourites.jsx';
import FormInputControl from '../components/FormInputControl.jsx';
import Mailto from '../components/Mailto.jsx';
import MultiDropdown from '../components/MultiDropdown.jsx';
import SortTable from '../components/SortTable.jsx';
import Spinner from '../components/Spinner.jsx';
import Unimplemented from '../components/Unimplemented.jsx';

import { formatDateTime, startOfCurrentFiscal, endOfCurrentFiscal, startOfPreviousFiscal, endOfPreviousFiscal } from '../utils/date';

/*

TODO:
* Print / Email / Add Rental Request

*/

const WITHIN_30_DAYS = 'Within 30 Days';
const THIS_MONTH = 'This Month';
const THIS_QUARTER = 'This Quarter';
const THIS_FISCAL = 'This Fiscal';
const LAST_MONTH = 'Last Month';
const LAST_QUARTER = 'Last Quarter';
const LAST_FISCAL = 'Last Fiscal';
const CUSTOM = 'Custom';
const ALL = 'All';

var RentalRequests = React.createClass({
  propTypes: {
    rentalRequests: React.PropTypes.object,
    localAreas: React.PropTypes.object,
    favourites: React.PropTypes.object,
    search: React.PropTypes.object,
    ui: React.PropTypes.object,
    router: React.PropTypes.object,
  },

  getInitialState() {
    return {
      loading: true,

      showAddDialog: false,

      search: {
        selectedLocalAreasIds: this.props.search.selectedLocalAreasIds || [],
        projectName: this.props.search.projecName || '',
        status: this.props.search.status || Constant.RENTAL_REQUEST_STATUS_CODE_IN_PROGRESS,
        dateRange: this.props.search.dateRange || WITHIN_30_DAYS,
      },

      ui : {
        sortField: this.props.ui.sortField || 'localAreaName',
        sortDesc: this.props.ui.sortDesc === true,
      },
    };
  },

  buildSearchParams() {
    var searchParams = {
      status: this.state.search.status || Constant.RENTAL_REQUEST_STATUS_CODE_IN_PROGRESS,
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
      case ALL:
      default:
        break;
    }

    if (startDate && startDate.isValid()) {
      searchParams.startDate = startDate.format(Constant.DATE_ZULU);
    }
    if (endDate && endDate.isValid()) {
      searchParams.endDate = endDate.format(Constant.DATE_ZULU);
    }

    return searchParams;
  },

  componentDidMount() {
    this.setState({ loading: true });

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
    this.setState({ loading: true });
    Api.searchRentalRequests(this.buildSearchParams()).finally(() => {
      this.setState({ loading: false });
    });
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
    // TODO Add Rental Request
    this.setState({ showAddDialog: true });
  },

  closeAddDialog() {
    this.setState({ showAddDialog: false });
  },

  email() {

  },

  print() {

  },

  
  render() {
    var localAreas = _.sortBy(this.props.localAreas, 'name');
    var numRentalRequests = this.state.loading ? '...' : Object.keys(this.props.rentalRequests).length;

    return <div id="rental-requests-list">
      <PageHeader>Rental Requests ({ numRentalRequests })
        <ButtonGroup id="rental-requests-buttons">
          <Unimplemented>
            <Button onClick={ this.email }><Glyphicon glyph="envelope" title="E-mail" /></Button>
          </Unimplemented>
          <Unimplemented>
            <Button onClick={ this.print }><Glyphicon glyph="print" title="Print" /></Button>
          </Unimplemented>
        </ButtonGroup>
      </PageHeader>
      <Well id="rental-requests-bar" bsSize="small" className="clearfix">
        <Row>
          <Col md={11}>
            <ButtonToolbar id="rental-requests-filters">
              <MultiDropdown id="selectedLocalAreasIds" placeholder="Local Areas"
                items={ localAreas } selectedIds={ this.state.search.selectedLocalAreasIds } updateState={ this.updateSearchState } showMaxItems={ 2 } />
              <FormInputControl id="projectName" type="text" placeholder="Project name" value={ this.state.search.projectName } updateState={ this.updateSearchState }></FormInputControl>
              <DropdownControl id="status" title={ this.state.search.status } updateState={ this.updateSearchState }
                  items={[ Constant.RENTAL_REQUEST_STATUS_CODE_IN_PROGRESS, Constant.RENTAL_REQUEST_STATUS_CODE_COMPLETED, Constant.RENTAL_REQUEST_STATUS_CODE_CANCELLED ]} />
              <DropdownControl id="dateRange" title={ this.state.search.dateRange } updateState={ this.updateSearchState }
                  items={[ ALL, WITHIN_30_DAYS, THIS_MONTH, THIS_QUARTER, THIS_FISCAL, LAST_MONTH, LAST_QUARTER, LAST_FISCAL, CUSTOM ]}
              />
              {(() => {
                if (this.state.search.dateRange === CUSTOM) {
                  return <span>
                    <DateControl id="startDate" date={ this.state.search.startDate } updateState={ this.updateSearchState } placeholder="mm/dd/yyyy" label="From:" title="start date"/>
                    <DateControl id="endDate" date={ this.state.search.endDate } updateState={ this.updateSearchState } placeholder="mm/dd/yyyy" label="To:" title="end date"/>
                  </span>;
                }
              })()}
              <Button id="search-button" bsStyle="primary" onClick={ this.fetch }>Search</Button>
            </ButtonToolbar>
          </Col>
          <Col md={1}>
            <Row id="rental-requests-faves">
              <Favourites id="rental-requests-faves-dropdown" type="rentalRequests" favourites={ this.props.favourites } data={ this.state.search } onSelect={ this.loadFavourite } />
            </Row>
          </Col>
        </Row>
      </Well>

      {(() => {
        var addRentalRequestButton = (
          <Unimplemented>
            <Button title="Add Rental Request" bsSize="xsmall" onClick={ this.openAddDialog }><Glyphicon glyph="plus" />&nbsp;<strong>Add Rental Request</strong></Button>
          </Unimplemented>
        );

        if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }
        if (Object.keys(this.props.rentalRequests).length === 0) { return <Alert bsStyle="success">No Rental Requests { addRentalRequestButton }</Alert>; }

        var rentalRequests = _.sortBy(this.props.rentalRequests, this.state.ui.sortField);
        if (this.state.ui.sortDesc) {
          _.reverse(rentalRequests);
        }

        return <SortTable sortField={ this.state.ui.sortField } sortDesc={ this.state.ui.sortDesc } onSort={ this.updateUIState } headers={[
          { field: 'localAreaName',          title: 'Local Area'                                      },
          { field: 'equipmentCount',         title: 'Pieces',          style: { textAlign: 'center' } },
          { field: 'equipmentTypeName',      title: 'Equipment Type'                                  },
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
            _.map(rentalRequests, (req) => {
              return <tr key={ req.id } className={ req.isActive ? null : 'info' }>
                <td>{ req.localAreaName }</td>
                <td style={{ textAlign: 'center' }}>{ req.equipmentCount }</td>
                <td>{ req.equipmentTypeName }</td>
                <td style={{ textAlign: 'center' }}>{ formatDateTime(req.expectedStartDate, Constant.DATE_YEAR_SHORT_MONTH_DAY) }</td>
                <td style={{ textAlign: 'center' }}>{ formatDateTime(req.expectedEndDate, Constant.DATE_YEAR_SHORT_MONTH_DAY) }</td>
                <td>
                  <Link to={ req.projectPath }>{ req.projectName }</Link>
                </td>
                <td>
                  {
                    req.primaryContactName ?
                      <Mailto email={ req.primaryContactEmail }>{ req.primaryContactName }</Mailto> :
                      'None'
                  }
                </td>
                <td style={{ textAlign: 'center' }}>{ req.status }</td>
                <td style={{ textAlign: 'right' }}>
                  <LinkContainer to={{ pathname: `rentalRequests/${ req.id }` }}>
                    <Button title="View Rental Request" bsSize="xsmall"><Glyphicon glyph="edit" /></Button>
                  </LinkContainer>
                </td>
              </tr>;
            })
          }
        </SortTable>;
      })()}

    </div>;
  },
});


function mapStateToProps(state) {
  return {
    rentalRequests: state.models.rentalRequests,
    localAreas: state.lookups.localAreas,
    favourites: state.models.favourites,
    search: state.search.rentalRequests,
    ui: state.ui.rentalRequests,
  };
}

export default connect(mapStateToProps)(RentalRequests);
