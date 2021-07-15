import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import { Alert, Row, Col, ButtonToolbar, Button, ButtonGroup, Form } from 'react-bootstrap';
import _ from 'lodash';
import Moment from 'moment';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import store from '../store';

import PageHeader from '../components/ui/PageHeader.jsx';
import SearchBar from '../components/ui/SearchBar.jsx';
import DateControl from '../components/DateControl.jsx';
import DropdownControl from '../components/DropdownControl.jsx';
import Favourites from '../components/Favourites.jsx';
import FormInputControl from '../components/FormInputControl.jsx';
import MultiDropdown from '../components/MultiDropdown.jsx';
import PrintButton from '../components/PrintButton.jsx';
import SortTable from '../components/SortTable.jsx';
import Spinner from '../components/Spinner.jsx';

import {
  formatDateTime,
  startOfCurrentFiscal,
  endOfCurrentFiscal,
  startOfPreviousFiscal,
  endOfPreviousFiscal,
  toZuluTime,
  dateIsBetween,
} from '../utils/date';

const THIS_FISCAL = 'This Fiscal';
const LAST_FISCAL = 'Last Fiscal';
const CUSTOM = 'Custom';

class AitReport extends React.Component {
  static propTypes = {
    currentUser: PropTypes.object,
    agreementSummaryLite: PropTypes.object,
    projects: PropTypes.object,
    districtEquipmentTypes: PropTypes.object,
    equipment: PropTypes.object,
    aitResponses: PropTypes.object,
    favourites: PropTypes.object,
    search: PropTypes.object,
    ui: PropTypes.object,
    router: PropTypes.object,
  };

  constructor(props) {
    super(props);

    var today = Moment();

    this.state = {
      search: {
        projectIds: props.search.projectIds || [],
        districtEquipmentTypes: props.search.districtEquipmentTypes || [],
        equipmentIds: props.search.equipmentIds || [],
        rentalAgreementNumber: props.search.rentalAgreementNumber || '',
        dateRange: props.search.dateRange || THIS_FISCAL,
        startDate: props.search.startDate || startOfCurrentFiscal(today).format('YYYY-MM-DD'),
        endDate: props.search.endDate || endOfCurrentFiscal(today).format('YYYY-MM-DD'),
      },
      ui: {
        sortField: props.ui.sortField || 'rentalAgreementNumber',
        sortDesc: props.ui.sortDesc === true,
      },
    };
  }

  buildSearchParams = () => {
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

    var startDate = Moment(this.state.search.startDate);
    var endDate = Moment(this.state.search.endDate);

    if (startDate && startDate.isValid()) {
      searchParams.startDate = toZuluTime(startDate.startOf('day'));
    }

    if (endDate && endDate.isValid()) {
      searchParams.endDate = toZuluTime(endDate.startOf('day'));
    }
    return searchParams;
  };

  componentDidMount() {
    Api.getRentalAgreementSummaryLite();
    Api.getProjectsAgreementSummary();
    Api.getDistrictEquipmentTypesAgreementSummary();
    Api.getEquipmentAgreementSummary();

    // If this is the first load, then look for a default favourite
    if (_.isEmpty(this.props.search)) {
      var defaultFavourite = _.find(this.props.favourites, (f) => f.isDefault);
      if (defaultFavourite) {
        this.loadFavourite(defaultFavourite);
      }
    }
  }

  fetch = () => {
    Api.searchAitReport(this.buildSearchParams());
  };

  search = (e) => {
    e.preventDefault();
    this.fetch();
  };

  clearSearch = () => {
    var today = Moment();

    var defaultSearchParameters = {
      projectIds: [],
      districtEquipmentTypes: [],
      equipmentIds: [],
      rentalAgreementNumber: '',
      dateRange: THIS_FISCAL,
      startDate: startOfCurrentFiscal(today).format('YYYY-MM-DD'),
      endDate: endOfCurrentFiscal(today).format('YYYY-MM-DD'),
    };

    this.setState({ search: defaultSearchParameters }, () => {
      store.dispatch({
        type: Action.UPDATE_AIT_SEARCH,
        aitResponses: this.state.search,
      });
      store.dispatch({ type: Action.CLEAR_AIT_REPORT });
    });
  };

  updateSearchState = (state, callback) => {
    this.setState({ search: { ...this.state.search, ...state, ...{ loaded: true } } }, () => {
      store.dispatch({
        type: Action.UPDATE_AIT_SEARCH,
        aitResponses: this.state.search,
      });
      if (callback) {
        callback();
      }
    });
  };

  updateUIState = (state, callback) => {
    this.setState({ ui: { ...this.state.ui, ...state } }, () => {
      store.dispatch({
        type: Action.UPDATE_AIT_REPORT_UI,
        aitResponses: this.state.ui,
      });
      if (callback) {
        callback();
      }
    });
  };

  loadFavourite = (favourite) => {
    this.updateSearchState(JSON.parse(favourite.value), this.fetch);
  };

  print = () => {
    window.print();
  };

  renderResults = () => {
    if (Object.keys(this.props.aitResponses.data).length === 0) {
      return <Alert variant="success">No results</Alert>;
    }

    var aitResponses = _.sortBy(this.props.aitResponses.data, (response) => {
      var sortValue = response[this.state.ui.sortField];
      if (typeof sortValue === 'string') {
        return sortValue.toLowerCase();
      }
      return sortValue;
    });

    if (this.state.ui.sortDesc) {
      _.reverse(aitResponses);
    }

    return (
      <SortTable
        sortField={this.state.ui.sortField}
        sortDesc={this.state.ui.sortDesc}
        onSort={this.updateUIState}
        headers={[
          { field: 'rentalAgreementNumber', title: 'Rental Agreement' },
          { field: 'equipmentCode', title: 'Equip ID' },
          { field: 'districtEquipmentName', title: 'Equipment Type' },
          { field: 'projectNumber', title: 'Project #' },
          { field: 'datedOn', title: 'Date On' },
          { field: 'startDate', title: 'Start Date' },
        ]}
      >
        {_.map(aitResponses, (entry) => {
          return (
            <tr key={entry.id}>
              <td>
                <Link to={`${Constant.RENTAL_AGREEMENTS_PATHNAME}/${entry.id}`}>{entry.rentalAgreementNumber}</Link>
              </td>
              <td>
                <Link to={`${Constant.EQUIPMENT_PATHNAME}/${entry.equipmentId}`}>{entry.equipmentCode}</Link>
              </td>
              <td>{entry.districtEquipmentName}</td>
              <td>
                <Link to={`${Constant.PROJECTS_PATHNAME}/${entry.projectId}`}>
                  {entry.projectNumber ? entry.projectNumber : 'N/A'}
                </Link>
              </td>
              <td>{formatDateTime(entry.datedOn, 'YYYY-MMM-DD')}</td>
              <td>{formatDateTime(entry.startDate, 'YYYY-MMM-DD')}</td>
            </tr>
          );
        })}
      </SortTable>
    );
  };

  matchesDateFilter = (agreementIds) => {
    const startDate = Moment(this.state.search.startDate);
    const endDate = Moment(this.state.search.endDate);

    const matchingAgreementIds = _.chain(this.props.agreementSummaryLite.data)
      .filter((a) => dateIsBetween(Moment(a.datedOn), startDate, endDate))
      .map('id')
      .value();

    return _.intersection(matchingAgreementIds, agreementIds).length > 0;
  };

  matchesProjectFilter = (projectIds) => {
    if (this.state.search.projectIds.length === 0) {
      return true;
    }

    return _.intersection(this.state.search.projectIds, projectIds).length > 0;
  };

  matchesDistrictEquipmentTypeFilter = (districtEquipmentTypeId) => {
    if (this.state.search.districtEquipmentTypes.length === 0) {
      return true;
    }

    return _.includes(this.state.search.districtEquipmentTypes, districtEquipmentTypeId);
  };

  updateDateRangeSearchState = (state) => {
    var today = Moment();
    var startDate;
    var endDate;

    switch (state.dateRange) {
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
        startDate = today;
        endDate = today;
        break;
      default:
        break;
    }

    this.updateSearchState(
      {
        ...state,
        startDate: startDate.format('YYYY-MM-DD'),
        endDate: endDate.format('YYYY-MM-DD'),
      },
      this.filterSelectedProjects
    );
  };

  updateDateSearchState = (state) => {
    this.updateSearchState(state, this.filterSelectedProjects);
  };

  updateProjectSearchState = (state) => {
    this.updateSearchState(state, this.filterSelectedEquipmentType);
  };

  updateEquipmentTypeSearchState = (state) => {
    this.updateSearchState(state, this.filterSelectedEquipment);
  };

  filterSelectedProjects = () => {
    var acceptableProjects = _.map(this.getFilteredProjects(), 'id');
    var projectIds = _.intersection(this.state.search.projectIds, acceptableProjects);
    this.updateSearchState({ projectIds: projectIds }, this.filterSelectedEquipmentType);
  };

  filterSelectedEquipmentType = () => {
    var acceptableDistrictEquipmentTypes = _.map(this.getFilteredDistrictEquipmentType(), 'id');
    var districtEquipmentTypes = _.intersection(
      this.state.search.districtEquipmentTypes,
      acceptableDistrictEquipmentTypes
    );
    this.updateSearchState({ districtEquipmentTypes: districtEquipmentTypes }, this.filterSelectedEquipment);
  };

  filterSelectedEquipment = () => {
    var acceptableEquipmentIds = _.map(this.getFilteredEquipment(), 'id');
    var equipmentIds = _.intersection(this.state.search.equipmentIds, acceptableEquipmentIds);
    this.updateSearchState({ equipmentIds: equipmentIds });
  };

  getFilteredProjects = () => {
    return _.chain(this.props.projects.data)
      .filter((x) => this.matchesDateFilter(x.agreementIds))
      .sortBy('name')
      .value();
  };

  getFilteredDistrictEquipmentType = () => {
    return _.chain(this.props.districtEquipmentTypes.data)
      .filter((x) => this.matchesDateFilter(x.agreementIds) && this.matchesProjectFilter(x.projectIds))
      .sortBy('name')
      .value();
  };

  getFilteredEquipment = () => {
    return _.chain(this.props.equipment.data)
      .filter(
        (x) =>
          this.matchesDateFilter(x.agreementIds) &&
          this.matchesProjectFilter(x.projectIds) &&
          this.matchesDistrictEquipmentTypeFilter(x.districtEquipmentTypeId)
      )
      .sortBy('equipmentCode')
      .value();
  };

  render() {
    var resultCount = '';
    if (this.props.aitResponses.loaded) {
      resultCount = '(' + Object.keys(this.props.aitResponses.data).length + ')';
    }

    var projects = this.getFilteredProjects();
    var districtEquipmentTypes = this.getFilteredDistrictEquipmentType();
    var equipment = this.getFilteredEquipment();

    return (
      <div id="rental-agreement-summary">
        <PageHeader>
          Rental Agreement Summary {resultCount}
          <ButtonGroup>
            <PrintButton disabled={!this.props.aitResponses.loaded} />
          </ButtonGroup>
        </PageHeader>
        <SearchBar>
          <Form onSubmit={this.search}>
            <Row>
              <Col xs={9} sm={10} id="filters">
                <Row>
                  <ButtonToolbar>
                    <MultiDropdown
                      id="projectIds"
                      placeholder="Projects"
                      disabled={!this.props.projects.loaded}
                      items={projects}
                      selectedIds={this.state.search.projectIds}
                      updateState={this.updateProjectSearchState}
                      showMaxItems={2}
                    />
                    <MultiDropdown
                      id="districtEquipmentTypes"
                      placeholder="Equipment Types"
                      items={districtEquipmentTypes}
                      disabled={!this.props.districtEquipmentTypes.loaded}
                      selectedIds={this.state.search.districtEquipmentTypes}
                      updateState={this.updateSearchState}
                      showMaxItems={2}
                    />
                    <MultiDropdown
                      id="equipmentIds"
                      placeholder="Equipment"
                      fieldName="equipmentCode"
                      items={equipment}
                      disabled={!this.props.equipment.loaded}
                      selectedIds={this.state.search.equipmentIds}
                      updateState={this.updateSearchState}
                      showMaxItems={2}
                    />
                    <FormInputControl
                      id="rentalAgreementNumber"
                      type="text"
                      placeholder="Rental Agreement"
                      value={this.state.search.rentalAgreementNumber}
                      updateState={this.updateSearchState}
                    />
                    <DropdownControl
                      id="dateRange"
                      title={this.state.search.dateRange}
                      updateState={this.updateDateRangeSearchState}
                      placeholder="Dated On"
                      items={[THIS_FISCAL, LAST_FISCAL, CUSTOM]}
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
                        <ButtonToolbar id="rental-agreement-summary-custom-date-filters">
                          <DateControl
                            id="startDate"
                            date={this.state.search.startDate}
                            updateState={this.updateDateSearchState}
                            label="From:"
                            title="start date"
                          />
                          <DateControl
                            id="endDate"
                            date={this.state.search.endDate}
                            updateState={this.updateDateSearchState}
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
                    id="faves-dropdown"
                    type="aitReport"
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
          if (this.props.aitResponses.loading) {
            return (
              <div style={{ textAlign: 'center' }}>
                <Spinner />
              </div>
            );
          }

          if (this.props.aitResponses.loaded) {
            return this.renderResults();
          }
        })()}
      </div>
    );
  }
}

function mapStateToProps(state) {
  return {
    currentUser: state.user,
    agreementSummaryLite: state.lookups.agreementSummaryLite,
    projects: state.lookups.projectsAgreementSummary,
    districtEquipmentTypes: state.lookups.districtEquipmentTypesAgreementSummary,
    equipment: state.lookups.equipment.agreementSummary,
    aitResponses: state.models.aitResponses,
    favourites: state.models.favourites.aitReport,
    search: state.search.aitResponses,
    ui: state.ui.aitResponses,
  };
}

export default connect(mapStateToProps)(AitReport);
