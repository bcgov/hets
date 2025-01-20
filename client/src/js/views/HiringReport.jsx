import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import { Alert, Row, Col, ButtonToolbar, Button, ButtonGroup, Form } from 'react-bootstrap';
import _ from 'lodash';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';

import PageHeader from '../components/ui/PageHeader.jsx';
import SearchBar from '../components/ui/SearchBar.jsx';
import Favourites from '../components/Favourites.jsx';
import MultiDropdown from '../components/MultiDropdown.jsx';
import SortTable from '../components/SortTable.jsx';
import Spinner from '../components/Spinner.jsx';
import PrintButton from '../components/PrintButton.jsx';

import { formatDateTime } from '../utils/date';

class HiringReport extends React.Component {
  static propTypes = {
    projects: PropTypes.object,
    localAreas: PropTypes.object,
    owners: PropTypes.object,
    equipment: PropTypes.object,
    hiringResponses: PropTypes.object,
    favourites: PropTypes.object,
    search: PropTypes.object,
    ui: PropTypes.object,
    router: PropTypes.object,
  };

  constructor(props) {
    super(props);

    this.state = {
      search: {
        projectIds: props.search.projectIds || [],
        localAreaIds: props.search.localAreaIds || [],
        ownerIds: props.search.ownerIds || [],
        equipmentIds: props.search.equipmentIds || [],
      },
      ui: {
        sortField: props.ui.sortField || 'name',
        sortDesc: props.ui.sortDesc === true,
      },
    };
  }

  componentDidMount() {
    this.props.dispatch(Api.getProjectsCurrentFiscal());
    this.props.dispatch(Api.getEquipmentHires());
    this.props.dispatch(Api.getOwnersLiteHires());

    // If this is the first load, then look for a default favourite
    if (_.isEmpty(this.props.search)) {
      var defaultFavourite = _.find(this.props.favourites, (f) => f.isDefault);
      if (defaultFavourite) {
        this.loadFavourite(defaultFavourite);
      }
    }
  }

  buildSearchParams = () => {
    var searchParams = {};

    if (this.state.search.projectIds.length > 0) {
      searchParams.projects = this.state.search.projectIds;
    }

    if (this.state.search.localAreaIds.length > 0) {
      searchParams.localAreas = this.state.search.localAreaIds;
    }

    if (this.state.search.ownerIds.length > 0) {
      searchParams.owners = this.state.search.ownerIds;
    }

    if (this.state.search.equipmentIds.length > 0) {
      searchParams.equipment = this.state.search.equipmentIds;
    }

    return searchParams;
  };

  fetch = () => {
    this.props.dispatch(Api.searchHiringReport(this.buildSearchParams()));
  };

  search = (e) => {
    e.preventDefault();
    this.fetch();
  };

  clearSearch = () => {
    var defaultSearchParameters = {
      projectIds: [],
      localAreaIds: [],
      ownerIds: [],
      equipmentIds: [],
    };

    this.setState({ search: defaultSearchParameters }, () => {
      this.props.dispatch({ type: Action.UPDATE_HIRING_RESPONSES_SEARCH, hiringResponses: this.state.search });
      this.props.dispatch({ type: Action.CLEAR_HIRING_RESPONSES });
    });
  };

  updateSearchState = (state, callback) => {
    this.setState({ search: { ...this.state.search, ...state, ...{ loaded: true } } }, () => {
      this.props.dispatch({
        type: Action.UPDATE_HIRING_RESPONSES_SEARCH,
        hiringResponses: this.state.search,
      });
      if (callback) {
        callback();
      }
    });
  };

  updateUIState = (state, callback) => {
    this.setState({ ui: { ...this.state.ui, ...state } }, () => {
      this.props.dispatch({
        type: Action.UPDATE_HIRING_RESPONSES_UI,
        hiringResponses: this.state.ui,
      });
      if (callback) {
        callback();
      }
    });
  };

  loadFavourite = (favourite) => {
    this.updateSearchState(JSON.parse(favourite.value), this.fetch);
  };

  renderResults = () => {
    if (Object.keys(this.props.hiringResponses.data).length === 0) {
      return <Alert variant="success">No results</Alert>;
    }

    var hiringResponses = _.sortBy(this.props.hiringResponses.data, (response) => {
      var sortValue = response[this.state.ui.sortField];
      if (typeof sortValue === 'string') {
        return sortValue.toLowerCase();
      }
      return sortValue;
    });

    if (this.state.ui.sortDesc) {
      _.reverse(hiringResponses);
    }

    return (
      <SortTable
        sortField={this.state.ui.sortField}
        sortDesc={this.state.ui.sortDesc}
        onSort={this.updateUIState}
        headers={[
          { field: 'localAreaLabel', title: 'Local Area' },
          { field: 'ownerCode', title: 'Owner Code' },
          { field: 'companyName', title: 'Company Name' },
          { field: 'sortableEquipmentCode', title: 'Equip. ID' },
          { field: 'equipmentDetails', title: 'Make/Model/Size/Year' },
          { field: 'projectNumber', title: 'Project #' },
          { field: 'noteDate', title: 'Note Date' },
          { field: 'noteType', title: 'Note Type' },
          { field: 'reason', title: 'Reason' },
          { field: 'userId', title: 'User ID' },
        ]}
      >
        {_.map(hiringResponses, (entry) => {
          var reason = entry.reason === Constant.HIRING_REFUSAL_OTHER ? entry.offerResponseNote : entry.reason;

          return (
            <tr key={entry.id}>
              <td>{entry.localAreaLabel}</td>
              <td>{entry.ownerCode}</td>
              <td>
                <Link to={`${Constant.OWNERS_PATHNAME}/${entry.ownerId}`}>{entry.companyName}</Link>
              </td>
              <td>
                <Link to={`${Constant.EQUIPMENT_PATHNAME}/${entry.equipmentId}`}>{entry.equipmentCode}</Link>
              </td>
              <td>{entry.equipmentDetails}</td>
              <td>
                <Link to={`${Constant.PROJECTS_PATHNAME}/${entry.projectId}`}>
                  {entry.projectNumber ? entry.projectNumber : 'N/A'}
                </Link>
              </td>
              <td>{formatDateTime(entry.noteDate, 'YYYY-MMM-DD')}</td>
              <td>{entry.noteType}</td>
              <td>{reason}</td>
              <td>
                {entry.userName} ({entry.userId})
              </td>
            </tr>
          );
        })}
      </SortTable>
    );
  };

  matchesProjectFilter = (projectIds) => {
    if (this.state.search.projectIds.length === 0) {
      return true;
    }

    return _.intersection(this.state.search.projectIds, projectIds).length > 0;
  };

  matchesLocalAreaFilter = (localAreaId) => {
    if (this.state.search.localAreaIds.length === 0) {
      return true;
    }

    return _.includes(this.state.search.localAreaIds, localAreaId);
  };

  matchesOwnerFilter = (ownerId) => {
    if (this.state.search.ownerIds.length === 0) {
      return true;
    }

    return _.includes(this.state.search.ownerIds, ownerId);
  };

  updateProjectSearchState = (state) => {
    this.updateSearchState(state, this.filterSelectedOwners);
  };

  updateLocalAreaSearchState = (state) => {
    this.updateSearchState(state, this.filterSelectedOwners);
  };

  updateOwnerSearchState = (state) => {
    this.updateSearchState(state, this.filterSelectedEquipment);
  };

  filterSelectedOwners = () => {
    var acceptableOwnerIds = _.map(this.getFilteredOwners(), 'id');
    var ownerIds = _.intersection(this.state.search.ownerIds, acceptableOwnerIds);
    this.updateSearchState({ ownerIds: ownerIds }, this.filterSelectedEquipment);
  };

  filterSelectedEquipment = () => {
    var acceptableEquipmentIds = _.map(this.getFilteredEquipment(), 'id');
    var equipmentIds = _.intersection(this.state.search.equipmentIds, acceptableEquipmentIds);
    this.updateSearchState({ equipmentIds: equipmentIds });
  };

  getFilteredOwners = () => {
    return _.chain(this.props.owners.data)
      .filter((x) => this.matchesProjectFilter(x.projectIds) && this.matchesLocalAreaFilter(x.localAreaId))
      .sortBy('organizationName')
      .value();
  };

  getFilteredEquipment = () => {
    return _.chain(this.props.equipment.data)
      .filter((x) => this.matchesProjectFilter(x.projectIds) && this.matchesOwnerFilter(x.ownerId))
      .sortBy('equipmentCode')
      .value();
  };

  render() {
    var resultCount = '';
    if (this.props.hiringResponses.loaded) {
      resultCount = '(' + Object.keys(this.props.hiringResponses.data).length + ')';
    }

    var projects = _.sortBy(this.props.projects.data, 'name');
    var localAreas = _.sortBy(this.props.localAreas, 'name');
    var owners = this.getFilteredOwners();
    var equipment = this.getFilteredEquipment();

    return (
      <div id="hiring-report">
        <PageHeader>
          Hiring Report - Not Hired / Force Hire {resultCount}
          <ButtonGroup>
            <PrintButton disabled={!this.props.hiringResponses.loaded} />
          </ButtonGroup>
        </PageHeader>
        <SearchBar>
          <Form onSubmit={this.search}>
            <Row>
              <Col xs={9} sm={10} id="filters">
                <ButtonToolbar>
                  <MultiDropdown
                    id="projectIds"
                    disabled={!this.props.projects.loaded}
                    placeholder="Projects"
                    fieldName="label"
                    items={projects}
                    selectedIds={this.state.search.projectIds}
                    updateState={this.updateProjectSearchState}
                    showMaxItems={2}
                  />
                  <MultiDropdown
                    id="localAreaIds"
                    placeholder="Local Areas"
                    items={localAreas}
                    selectedIds={this.state.search.localAreaIds}
                    updateState={this.updateLocalAreaSearchState}
                    showMaxItems={2}
                  />
                  <MultiDropdown
                    id="ownerIds"
                    disabled={!this.props.owners.loaded}
                    placeholder="Companies"
                    fieldName="organizationName"
                    items={owners}
                    selectedIds={this.state.search.ownerIds}
                    updateState={this.updateOwnerSearchState}
                    showMaxItems={2}
                  />
                  <MultiDropdown
                    id="equipmentIds"
                    disabled={!this.props.equipment.loaded}
                    placeholder="Equipment"
                    fieldName="equipmentCode"
                    items={equipment}
                    selectedIds={this.state.search.equipmentIds}
                    updateState={this.updateSearchState}
                    showMaxItems={2}
                  />
                  <Button id="search-button" variant="primary" type="submit">
                    Search
                  </Button>
                  <Button className="btn-custom" id="clear-search-button" onClick={this.clearSearch}>
                    Clear
                  </Button>
                </ButtonToolbar>
              </Col>
              <Col xs={3} sm={2} id="search-buttons">
                <Row className="float-right">
                  <Favourites
                    id="hiring-report-faves-dropdown"
                    type="hiringReport"
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
          if (this.props.hiringResponses.loading) {
            return (
              <div style={{ textAlign: 'center' }}>
                <Spinner />
              </div>
            );
          }

          if (this.props.hiringResponses.loaded) {
            return this.renderResults();
          }
        })()}
      </div>
    );
  }
}

const mapStateToProps = (state) => ({
  projects: state.lookups.projectsCurrentFiscal,
  localAreas: state.lookups.localAreas,
  owners: state.lookups.owners.hires,
  equipment: state.lookups.equipment.hires,
  hiringResponses: state.models.hiringResponses,
  favourites: state.models.favourites.hiringReport,
  search: state.search.hiringResponses,
  ui: state.ui.hiringResponses,
});

const mapDispatchToProps = (dispatch) => ({ dispatch });

export default connect(mapStateToProps, mapDispatchToProps)(HiringReport);
