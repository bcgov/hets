import React from 'react';

import { connect } from 'react-redux';

import { Link } from 'react-router';

import { PageHeader, Well, Alert, Row, Col, ButtonToolbar, Button, ButtonGroup, Glyphicon, Form  } from 'react-bootstrap';

import _ from 'lodash';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import store from '../store';

import Favourites from '../components/Favourites.jsx';
import MultiDropdown from '../components/MultiDropdown.jsx';
import SortTable from '../components/SortTable.jsx';
import Spinner from '../components/Spinner.jsx';
import TooltipButton from '../components/TooltipButton.jsx';

import { formatDateTime } from '../utils/date';

var HiringReport = React.createClass({
  propTypes: {
    projects: React.PropTypes.object,
    localAreas: React.PropTypes.object,
    owners: React.PropTypes.object,
    equipment: React.PropTypes.object,
    hiringResponses: React.PropTypes.object,
    favourites: React.PropTypes.object,
    search: React.PropTypes.object,
    ui: React.PropTypes.object,
    router: React.PropTypes.object,
  },

  getInitialState() {
    return {
      loaded: false,
      search: {
        projectIds: this.props.search.projectIds || [],
        localAreaIds: this.props.search.localAreaIds || [],
        ownerIds: this.props.search.ownerIds || [],
        equipmentIds: this.props.search.equipmentIds || [],
      },
      ui : {
        sortField: this.props.ui.sortField || 'name',
        sortDesc: this.props.ui.sortDesc === true,
      },
    };
  },

  buildSearchParams() {
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
  },

  componentDidMount() {
    var projectsPromise = Api.getProjectsCurrentFiscal();
    var ownersPromise = Api.getOwnersLiteHires();
    var equipmentPromise = Api.getEquipmentLiteHires();
    var favouritesPromise = Api.getFavourites('hiringReport');

    return Promise.all([ projectsPromise, ownersPromise, equipmentPromise, favouritesPromise]).then(() => {
      this.setState({ loaded: true });

      // If this is the first load, then look for a default favourite
      if (_.isEmpty(this.props.search)) {
        var defaultFavourite = _.find(this.props.favourites.data, f => f.isDefault);
        if (defaultFavourite) {
          this.loadFavourite(defaultFavourite);
          return;
        }
      }
    });
  },

  fetch() {
    Api.searchHiringReport(this.buildSearchParams());
  },

  search(e) {
    e.preventDefault();
    this.fetch();
  },

  clearSearch() {
    var defaultSearchParameters = {
      projectIds: [],
      localAreaIds: [],
      ownerIds: [],
      equipmentIds: [],
    };

    this.setState({ search: defaultSearchParameters }, () => {
      store.dispatch({ type: Action.UPDATE_HIRING_RESPONSES_SEARCH, hiringResponses: this.state.search });
      store.dispatch({ type: Action.CLEAR_HIRING_RESPONSES });
    });
  },

  updateSearchState(state, callback) {
    this.setState({ search: { ...this.state.search, ...state, ...{ loaded: true } }}, () =>{
      store.dispatch({ type: Action.UPDATE_HIRING_RESPONSES_SEARCH, hiringResponses: this.state.search });
      if (callback) { callback(); }
    });
  },

  updateUIState(state, callback) {
    this.setState({ ui: { ...this.state.ui, ...state }}, () =>{
      store.dispatch({ type: Action.UPDATE_HIRING_RESPONSES_UI, hiringResponses: this.state.ui });
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
    if (Object.keys(this.props.hiringResponses.data).length === 0) {
      return <Alert bsStyle="success">No results</Alert>;
    }

    var hiringResponses = _.sortBy(this.props.hiringResponses.data, response => {
      var sortValue = response[this.state.ui.sortField];
      if (typeof sortValue === 'string') {
        return sortValue.toLowerCase();
      }
      return sortValue;
    });

    if (this.state.ui.sortDesc) {
      _.reverse(hiringResponses);
    }

    return <SortTable sortField={ this.state.ui.sortField } sortDesc={ this.state.ui.sortDesc } onSort={ this.updateUIState } headers={[
      { field: 'localAreaLabel',          title: 'Local Area'                               },
      { field: 'ownerCode',               title: 'Owner Code'                               },
      { field: 'companyName',             title: 'Company Name'                             },
      { field: 'sortableEquipmentCode',   title: 'Equip. ID'                                },
      { field: 'equipmentDetails',        title: 'Make/Model/Size/Year'                     },
      { field: 'projectNumber',           title: 'Project #'                                },
      { field: 'noteDate',                title: 'Note Date'                                },
      { field: 'noteType',                title: 'Note Type'                                },
      { field: 'reason',                  title: 'Reason'                                   },
      { field: 'userId',                  title: 'User ID'                                  },
    ]}>
      {
        _.map(hiringResponses, (entry) => {
          var reason = entry.reason == Constant.HIRING_REFUSAL_OTHER ? entry.offerResponseNote : entry.reason;

          return <tr key={ entry.id }>
            <td>{ entry.localAreaLabel }</td>
            <td>{ entry.ownerCode }</td>
            <td><Link to={`${Constant.OWNERS_PATHNAME}/${entry.ownerId}`}>{ entry.companyName }</Link></td>
            <td><Link to={`${Constant.EQUIPMENT_PATHNAME}/${entry.equipmentId}`}>{ entry.equipmentCode }</Link></td>
            <td>{ entry.equipmentDetails }</td>
            <td><Link to={`${Constant.PROJECTS_PATHNAME}/${entry.projectId}`}>{ entry.projectNumber }</Link></td>
            <td>{ formatDateTime(entry.noteDate, 'YYYY-MMM-DD') }</td>
            <td>{ entry.noteType }</td>
            <td>{ reason }</td>
            <td>{ entry.userName } ({ entry.userId })</td>
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

  matchesLocalAreaFilter(localAreaId) {
    if (this.state.search.localAreaIds.length == 0) {
      return true;
    }

    return _.includes(this.state.search.localAreaIds, localAreaId);
  },

  matchesOwnerFilter(ownerId) {
    if (this.state.search.ownerIds.length == 0) {
      return true;
    }

    return _.includes(this.state.search.ownerIds, ownerId);
  },

  updateProjectSearchState(state) {
    this.updateSearchState(state, this.filterSelectedOwners);
  },

  updateLocalAreaSearchState(state) {
    this.updateSearchState(state, this.filterSelectedOwners);
  },

  updateOwnerSearchState(state) {
    this.updateSearchState(state, this.filterSelectedEquipment);
  },

  filterSelectedOwners() {
    var acceptableOwnerIds = _.map(this.getFilteredOwners(), 'id');
    var ownerIds = _.intersection(this.state.search.ownerIds, acceptableOwnerIds);
    this.updateSearchState({ ownerIds: ownerIds }, this.filterSelectedEquipment);
  },

  filterSelectedEquipment() {
    var acceptableEquipmentIds = _.map(this.getFilteredEquipment(), 'id');
    var equipmentIds = _.intersection(this.state.search.equipmentIds, acceptableEquipmentIds);
    this.updateSearchState({ equipmentIds: equipmentIds });
  },

  getFilteredOwners() {
    return _.chain(this.props.owners)
      .filter(x => this.matchesProjectFilter(x.projectIds) && this.matchesLocalAreaFilter(x.localAreaId))
      .sortBy('organizationName')
      .value();
  },

  getFilteredEquipment() {
    return _.chain(this.props.equipment)
      .filter(x => this.matchesProjectFilter(x.projectIds) && this.matchesOwnerFilter(x.ownerId))
      .sortBy('equipmentCode')
      .value();
  },

  render() {
    var resultCount = '';
    if (this.props.hiringResponses.loaded) {
      resultCount = '(' + Object.keys(this.props.hiringResponses.data).length + ')';
    }

    var projects = _.sortBy(this.props.projects, 'name');
    var localAreas = _.sortBy(this.props.localAreas, 'name');
    var owners = this.getFilteredOwners();
    var equipment = this.getFilteredEquipment();

    return <div id="hiring-report">
      <PageHeader>Hiring Report - Not Hired / Force Hire { resultCount }
        <ButtonGroup id="hiring-report-buttons">
          <TooltipButton onClick={ this.print } disabled={ !this.props.hiringResponses.loaded } disabledTooltip={ 'Please complete the search to enable this function.' }>
            <Glyphicon glyph="print" title="Print" />
          </TooltipButton>
        </ButtonGroup>
      </PageHeader>
      <Well id="hiring-report-bar" bsSize="small" className="clearfix">
        <Row>
          <Form onSubmit={ this.search }>
            <Col xs={9} sm={10}>
              <ButtonToolbar id="hiring-report-filters">
                <MultiDropdown id="projectIds" placeholder="Projects" fieldName="label"
                  items={ projects } selectedIds={ this.state.search.projectIds } updateState={ this.updateProjectSearchState } showMaxItems={ 2 } />
                <MultiDropdown id="localAreaIds" placeholder="Local Areas"
                  items={ localAreas } selectedIds={ this.state.search.localAreaIds } updateState={ this.updateLocalAreaSearchState } showMaxItems={ 2 } />
                <MultiDropdown id="ownerIds" placeholder="Companies" fieldName="organizationName"
                  items={ owners } selectedIds={ this.state.search.ownerIds } updateState={ this.updateOwnerSearchState } showMaxItems={ 2 } />
                <MultiDropdown id="equipmentIds" placeholder="Equipment" fieldName="equipmentCode"
                  items={ equipment } selectedIds={ this.state.search.equipmentIds } updateState={ this.updateSearchState } showMaxItems={ 2 } />
                <Button id="search-button" bsStyle="primary" type="submit">Search</Button>
                <Button id="clear-search-button" onClick={ this.clearSearch }>Clear</Button>
              </ButtonToolbar>
            </Col>
          </Form>
          <Col xs={3} sm={2}>
            <Favourites id="hiring-report-faves-dropdown" type="hiringReport" favourites={ this.props.favourites.data } data={ this.state.search } onSelect={ this.loadFavourite } pullRight />
          </Col>
        </Row>
      </Well>

      {(() => {
        if (this.props.hiringResponses.loading || !this.state.loaded) {
          return <div style={{ textAlign: 'center' }}><Spinner/></div>;
        }

        if (this.props.hiringResponses.loaded) {
          return this.renderResults();
        }
      })()}
    </div>;
  },
});


function mapStateToProps(state) {
  return {
    projects: state.lookups.projectsCurrentFiscal,
    localAreas: state.lookups.localAreas,
    owners: state.lookups.ownersLite,
    equipment: state.lookups.equipmentLite,
    hiringResponses: state.models.hiringResponses,
    favourites: state.models.favourites,
    search: state.search.hiringResponses,
    ui: state.ui.hiringResponses,
  };
}

export default connect(mapStateToProps)(HiringReport);
