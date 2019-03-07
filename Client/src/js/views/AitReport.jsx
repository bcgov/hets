import React from 'react';

import { connect } from 'react-redux';

import { Link } from 'react-router';

import { PageHeader, Well, Alert, Row, Col, ButtonToolbar, Button, ButtonGroup, Glyphicon, Form  } from 'react-bootstrap';

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
import MultiDropdown from '../components/MultiDropdown.jsx';
import SortTable from '../components/SortTable.jsx';
import Spinner from '../components/Spinner.jsx';
import TooltipButton from '../components/TooltipButton.jsx';

import { formatDateTime, startOfCurrentFiscal, endOfCurrentFiscal, startOfPreviousFiscal, endOfPreviousFiscal, toZuluTime } from '../utils/date';

const THIS_FISCAL = 'This Fiscal';
const LAST_FISCAL = 'Last Fiscal';
const CUSTOM = 'Custom';

var AitReport = React.createClass({
  propTypes: {
    currentUser: React.PropTypes.object,
    projects: React.PropTypes.object,
    districtEquipmentTypes: React.PropTypes.object,
    equipment: React.PropTypes.object,
    aitResponses: React.PropTypes.object,
    favourites: React.PropTypes.object,
    search: React.PropTypes.object,
    ui: React.PropTypes.object,
    router: React.PropTypes.object,
  },

  getInitialState() {
    return {
      loading: true,
      search: {
        projectIds: this.props.search.projectIds || [],
        districtEquipmentTypes: this.props.search.districtEquipmentTypes || [],
        equipmentIds: this.props.search.equipmentIds || [],
        rentalAgreementNumber: this.props.search.rentalAgreementNumber || '',
        dateRange: this.props.search.dateRange || THIS_FISCAL,
      },
      ui : {
        sortField: this.props.ui.sortField || 'rentalAgreementNumber',
        sortDesc: this.props.ui.sortDesc === true,
      },
    };
  },

  buildSearchParams() {
    var searchParams = {
      rentalAgreementNumber: this.state.search.rentalAgreementNumber || '',
    };

    if (this.state.search.projectIds.length > 0) {
      searchParams.projects = this.state.search.projectIds;
    }

    if (this.state.search.districtEquipmentTypes.length > 0) {
      searchParams.districtEquipmentTypes = this.state.search.districtEquipmentTypes;
    }

    if (this.state.search.equipmentIds.length > 0) {
      searchParams.equipment = this.state.search.equipmentIds;
    }

    // Time period drop-down; e.g. "This Month"
    var startDate;
    var endDate;
    var today = Moment();

    switch (this.state.search.dateRange) {
      case THIS_FISCAL:
        // Fiscal Year: Apr 1 - March 31
        startDate = startOfCurrentFiscal(today);
        endDate = endOfCurrentFiscal(today);
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
    var projectsPromise = Api.getProjects();
    var districtEquipmentTypesPromise = this.props.districtEquipmentTypes.loaded ? Promise.resolve() : Api.getDistrictEquipmentTypes();
    var equipmentPromise = this.props.equipment.loaded ? Promise.resolve() : Api.getEquipmentHires();

    Promise.all([ projectsPromise, districtEquipmentTypesPromise, equipmentPromise]).then(() => {
      this.setState({ loading: false });
    });

    // If this is the first load, then look for a default favourite
    if (_.isEmpty(this.props.search)) {
      var defaultFavourite = _.find(this.props.favourites, f => f.isDefault);
      if (defaultFavourite) {
        this.loadFavourite(defaultFavourite);
      }
    }
  },

  fetch() {
    Api.searchAitReport(this.buildSearchParams());
  },

  search(e) {
    e.preventDefault();
    this.fetch();
  },

  clearSearch() {
    var defaultSearchParameters = {
      projectIds: [],
      districtEquipmentTypes: [],
      equipmentIds: [],
      rentalAgreementNumber: '',
      dateRange: THIS_FISCAL,
    };

    this.setState({ search: defaultSearchParameters }, () => {
      store.dispatch({ type: Action.UPDATE_AIT_SEARCH, aitResponses: this.state.search });
      store.dispatch({ type: Action.CLEAR_AIT_REPORT });
    });
  },

  updateSearchState(state, callback) {
    this.setState({ search: { ...this.state.search, ...state, ...{ loaded: true } }}, () =>{
      store.dispatch({ type: Action.UPDATE_AIT_SEARCH, aitResponses: this.state.search });
      if (callback) { callback(); }
    });
  },

  updateUIState(state, callback) {
    this.setState({ ui: { ...this.state.ui, ...state }}, () =>{
      store.dispatch({ type: Action.UPDATE_AIT_REPORT_UI, aitResponses: this.state.ui });
      if (callback) { callback(); }
    });
  },

  loadFavourite(favourite) {
    this.updateSearchState(JSON.parse(favourite.value), this.fetch);
  },

  print() {
    window.print();
  },

  renderResults() {
    if (Object.keys(this.props.aitResponses.data).length === 0) {
      return <Alert bsStyle="success">No results</Alert>;
    }

    var aitResponses = _.sortBy(this.props.aitResponses.data, response => {
      var sortValue = response[this.state.ui.sortField];
      if (typeof sortValue === 'string') {
        return sortValue.toLowerCase();
      }
      return sortValue;
    });

    if (this.state.ui.sortDesc) {
      _.reverse(aitResponses);
    }

    return <SortTable sortField={ this.state.ui.sortField } sortDesc={ this.state.ui.sortDesc } onSort={ this.updateUIState } headers={[
      { field: 'rentalAgreementNumber',   title: 'Rental Agreement'                         },
      { field: 'equipmentCode',           title: 'Equip ID'                                 },
      { field: 'DistrictEquipmentName',   title: 'Equipment Type'                           },
      { field: 'projectNumber',           title: 'Project #'                                },
      { field: 'datedOn',                 title: 'Date On'                                  },
      { field: 'startDate',               title: 'Start Date'                               },
    ]}>
      {
        _.map(aitResponses, (entry) => {
          return <tr key={ entry.id }>
            <td><Link to={`${Constant.RENTAL_AGREEMENTS_PATHNAME}/${entry.id}`}>{ entry.rentalAgreementNumber}</Link></td>
            <td><Link to={`${Constant.EQUIPMENT_PATHNAME}/${entry.equipmentId}`}>{ entry.equipmentCode }</Link></td>
            <td>{ entry.DistrictEquipmentName }</td>
            <td><Link to={`${Constant.PROJECTS_PATHNAME}/${entry.projectId}`}>{ entry.projectNumber ? entry.projectNumber : 'N/A' }</Link></td>
            <td>{ formatDateTime(entry.datedOn, 'YYYY-MMM-DD') }</td>
            <td>{ formatDateTime(entry.startDate, 'YYYY-MMM-DD') }</td>
          </tr>;
        })
      }
    </SortTable>;
  },

  matchesProjectFilter(projectIds) {
    if (this.state.search.projectIds.length == 0) {
      return true;
    }

    return _.intersection(this.state.search.projectIds, projectIds).length > 0;
  },

  updateProjectSearchState(state) {
    this.updateSearchState(state, this.filterSelectedEquipment);
  },

  filterSelectedEquipment() {
    var acceptableEquipmentIds = _.map(this.getFilteredEquipment(), 'id');
    var equipmentIds = _.intersection(this.state.search.equipmentIds, acceptableEquipmentIds);
    this.updateSearchState({ equipmentIds: equipmentIds });
  },

  getFilteredEquipment() {
    return _.chain(this.props.equipment.data)
      .filter(x => this.matchesProjectFilter(x.projectIds))
      .sortBy('equipmentCode')
      .value();
  },

  render() {
    const { loading } = this.state;

    var resultCount = '';
    if (this.props.aitResponses.loaded) {
      resultCount = '(' + Object.keys(this.props.aitResponses.data).length + ')';
    }

    var projects = _.sortBy(this.props.projects, 'name');
    var districtEquipmentTypes = _.chain(this.props.districtEquipmentTypes.data)
      .filter(type => type.district.id == this.props.currentUser.district.id)
      .sortBy('districtEquipmentName')
      .value();
    var equipment = this.getFilteredEquipment();

    return <div id="ait-report">
      <PageHeader>AIT Report { resultCount }
        <ButtonGroup id="ait-report-buttons">
          <TooltipButton onClick={ this.print } disabled={ !this.props.aitResponses.loaded } disabledTooltip={ 'Please complete the search to enable this function.' }>
            <Glyphicon glyph="print" title="Print" />
          </TooltipButton>
        </ButtonGroup>
      </PageHeader>
      <Well id="ait-report-bar" bsSize="small" className="clearfix">
        <Row>
          <Form onSubmit={ this.search }>
            <Col xs={9} sm={10}>
              <Row>
                <ButtonToolbar id="ait-report-filters">
                  <MultiDropdown id="projectIds" disabled={ loading } placeholder="Projects" fieldName="label"
                    items={ projects } selectedIds={ this.state.search.projectIds } updateState={ this.updateProjectSearchState } showMaxItems={ 2 } />
                  <MultiDropdown id="districtEquipmentTypes" fieldName="districtEquipmentName" placeholder="Equipment Types"
                    items={ districtEquipmentTypes } selectedIds={ this.state.search.districtEquipmentTypes } updateState={ this.updateSearchState } showMaxItems={ 2 } />
                  <MultiDropdown id="equipmentIds" disabled={ loading } placeholder="Equipment" fieldName="equipmentCode"
                    items={ equipment } selectedIds={ this.state.search.equipmentIds } updateState={ this.updateSearchState } showMaxItems={ 2 } />
                  <FormInputControl id="rentalAgreementNumber" type="text" placeholder="Rental Agreement" value={ this.state.search.rentalAgreementNumber } updateState={ this.updateSearchState }></FormInputControl>
                  <DropdownControl id="dateRange" title={ this.state.search.dateRange } updateState={ this.updateSearchState } placeholder="Dated On"
                    items={[ THIS_FISCAL, LAST_FISCAL, CUSTOM ]}
                  />
                  <Button id="search-button" bsStyle="primary" type="submit">Search</Button>
                  <Button id="clear-search-button" onClick={ this.clearSearch }>Clear</Button>
                </ButtonToolbar>
              </Row>
              {(() => {
                if (this.state.search.dateRange === CUSTOM) {
                  return <Row>
                    <ButtonToolbar id="ait-report-custom-date-filters">
                      <span>
                        <DateControl id="startDate" date={ this.state.search.startDate } updateState={ this.updateSearchState } label="From:" title="start date"/>
                        <DateControl id="endDate" date={ this.state.search.endDate } updateState={ this.updateSearchState } label="To:" title="end date"/>
                      </span>
                    </ButtonToolbar>
                  </Row>;
                }
              })()}
            </Col>
          </Form>
          <Col xs={3} sm={2}>
            <Favourites id="ait-report-faves-dropdown" type="aitReport" favourites={ this.props.favourites } data={ this.state.search } onSelect={ this.loadFavourite } pullRight />
          </Col>
        </Row>
      </Well>

      {(() => {
        if (this.props.aitResponses.loading) {
          return <div style={{ textAlign: 'center' }}><Spinner/></div>;
        }

        if (this.props.aitResponses.loaded) {
          return this.renderResults();
        }
      })()}
    </div>;
  },
});

function mapStateToProps(state) {
  return {
    currentUser: state.user,
    projects: state.lookups.projects,
    districtEquipmentTypes: state.lookups.districtEquipmentTypes,
    equipment: state.lookups.equipment.hires,
    aitResponses: state.models.aitResponses,
    favourites: state.models.favourites.aitReport,
    search: state.search.aitResponses,
    ui: state.ui.aitResponses,
  };
}

export default connect(mapStateToProps)(AitReport);
