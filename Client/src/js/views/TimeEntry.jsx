import React from 'react';

import { connect } from 'react-redux';

import { Link } from 'react-router';

import { PageHeader, Well, Alert, Row, Col, ButtonToolbar, Button, ButtonGroup, Glyphicon, Form  } from 'react-bootstrap';

import _ from 'lodash';

import TimeEntryDialog from './dialogs/TimeEntryDialog.jsx';

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

var TimeEntry = React.createClass({
  propTypes: {
    projects: React.PropTypes.object,
    localAreas: React.PropTypes.object,
    owners: React.PropTypes.object,
    equipment: React.PropTypes.object,
    timeEntries: React.PropTypes.object,
    favourites: React.PropTypes.object,
    search: React.PropTypes.object,
    ui: React.PropTypes.object,
    router: React.PropTypes.object,
  },

  getInitialState() {
    return {
      showTimeEntryDialog: false,
      allowMultipleTimeEntries: false,
      rentalAgreementId: null,
      search: {
        projectIds: this.props.search.projectIds || [],
        localAreaIds: this.props.search.localAreaIds || [],
        ownerIds: this.props.search.ownerIds || [],
        equipmentIds: this.props.search.equipmentIds || [],
      },
      ui : {
        sortField: this.props.ui.sortField || 'localAreaLabel',
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
    var ownersPromise = Api.getOwnersLiteTs();
    var equipmentPromise = this.props.equipment.loaded ? Promise.resolve() : Api.getEquipmentTs();

    Promise.all([ projectsPromise, ownersPromise, equipmentPromise]);

    // If this is the first load, then look for a default favourite
    if (_.isEmpty(this.props.search)) {
      var defaultFavourite = _.find(this.props.favourites, f => f.isDefault);
      if (defaultFavourite) {
        this.loadFavourite(defaultFavourite);
      }
    }
  },

  fetch() {
    Api.searchTimeEntries(this.buildSearchParams());
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
      store.dispatch({ type: Action.UPDATE_TIME_ENTRIES_SEARCH, timeEntries: this.state.search });
      store.dispatch({ type: Action.CLEAR_TIME_ENTRIES });
    });
  },

  updateSearchState(state, callback) {
    this.setState({ search: { ...this.state.search, ...state, ...{ loaded: true } }}, () =>{
      store.dispatch({ type: Action.UPDATE_TIME_ENTRIES_SEARCH, timeEntries: this.state.search });
      if (callback) { callback(); }
    });
  },

  updateUIState(state, callback) {
    this.setState({ ui: { ...this.state.ui, ...state }}, () =>{
      store.dispatch({ type: Action.UPDATE_TIME_ENTRIES_UI, timeEntries: this.state.ui });
      if (callback) { callback(); }
    });
  },

  loadFavourite(favourite) {
    this.updateSearchState(JSON.parse(favourite.value), this.fetch);
  },

  openTimeEntryDialog(rentalAgreementId) {
    this.setState({
      rentalAgreementId: rentalAgreementId,
      allowMultipleTimeEntries: rentalAgreementId ? false : true,
      showTimeEntryDialog: true,
    });
  },

  closeTimeEntryDialog() {
    this.setState({ showTimeEntryDialog: false });
    if (this.props.timeEntries.loaded) {
      this.fetch();
    }
  },

  print() {
    window.print();
  },

  renderResults(addTimeEntryButton) {
    if (Object.keys(this.props.timeEntries.data).length === 0) {
      return <Alert bsStyle="success">No time entries { addTimeEntryButton }</Alert>;
    }

    var timeEntries = _.sortBy(this.props.timeEntries.data, timeEntry => {
      var sortValue = timeEntry[this.state.ui.sortField];
      if (typeof sortValue === 'string') {
        return sortValue.toLowerCase();
      }
      return sortValue;
    });

    if (this.state.ui.sortDesc) {
      _.reverse(timeEntries);
    }

    return <SortTable sortField={ this.state.ui.sortField } sortDesc={ this.state.ui.sortDesc } onSort={ this.updateUIState } headers={[
      { field: 'localAreaLabel',          title: 'Local Area'                               },
      { field: 'ownerCode',               title: 'Owner Code'                               },
      { field: 'ownerName',               title: 'Company Name'                             },
      { field: 'sortableEquipmentCode',   title: 'Equip. ID'                                },
      { field: 'equipmentDetails',        title: 'Make/Model/Size/Year'                     },
      { field: 'provincialProjectNumber', title: 'Project #'                                },
      { field: 'hours',                   title: 'Hours'                                    },
      { field: 'workedDate',              title: 'Date Worked'                              },
      { field: 'enteredDate',             title: 'Date Entered'                             },
      { field: 'addTime',                 title: 'Add Time Entry', node: addTimeEntryButton },
    ]}>
      {
        _.map(timeEntries, (entry) => {
          return <tr key={ entry.id }>
            <td>{ entry.localAreaLabel }</td>
            <td>{ entry.ownerCode }</td>
            <td><Link to={`${Constant.OWNERS_PATHNAME}/${entry.ownerId}`}>{ entry.ownerName }</Link></td>
            <td><Link to={`${Constant.EQUIPMENT_PATHNAME}/${entry.equipmentId}`}>{ entry.equipmentCode }</Link></td>
            <td>{ entry.equipmentDetails }</td>
            <td><Link to={`${Constant.PROJECTS_PATHNAME}/${entry.projectId}`}>{ entry.provincialProjectNumber }</Link></td>
            <td>{ entry.hours }</td>
            <td>{ formatDateTime(entry.workedDate, 'YYYY-MMM-DD') }</td>
            <td>{ formatDateTime(entry.enteredDate, 'YYYY-MMM-DD') }</td>
            <td style={{ textAlign: 'right' }}>
              <ButtonGroup>
                <Button title="Edit Time" bsSize="xsmall" onClick={ this.openTimeEntryDialog.bind(this, entry.rentalAgreementId) }><Glyphicon glyph="edit" /></Button>
              </ButtonGroup>
            </td>
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
    return _.chain(this.props.equipment.data)
      .filter(x => this.matchesProjectFilter(x.projectIds) && this.matchesOwnerFilter(x.ownerId))
      .sortBy('equipmentCode')
      .value();
  },

  render() {
    var resultCount = '';
    if (this.props.timeEntries.loaded) {
      resultCount = '(' + Object.keys(this.props.timeEntries.data).length + ')';
    }

    var projects = _.sortBy(this.props.projects, 'name');
    var localAreas = _.sortBy(this.props.localAreas, 'name');
    var owners = this.getFilteredOwners();
    var equipment = this.getFilteredEquipment();

    return <div id="time-entry-list">
      <PageHeader>Time Entry { resultCount }
        <ButtonGroup id="time-entry-buttons">
          <TooltipButton onClick={ this.print } disabled={ !this.props.timeEntries.loaded } disabledTooltip={ 'Please complete the search to enable this function.' }>
            <Glyphicon glyph="print" title="Print" />
          </TooltipButton>
        </ButtonGroup>
      </PageHeader>
      <Well id="time-entries-bar" bsSize="small" className="clearfix">
        <Row>
          <Form onSubmit={ this.search }>
            <Col xs={9} sm={10}>
              <ButtonToolbar id="time-entry-filters">
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
            <Favourites id="time-entry-faves-dropdown" type="timeEntry" favourites={ this.props.favourites } data={ this.state.search } onSelect={ this.loadFavourite } pullRight />
          </Col>
        </Row>
      </Well>

      {(() => {
        if (this.props.timeEntries.loading) {
          return <div style={{ textAlign: 'center' }}><Spinner/></div>;
        }

        var addTimeEntryButton = <Button title="Add Time" bsSize="xsmall" onClick={ this.openTimeEntryDialog.bind(this, null) }>
          <Glyphicon glyph="plus" />&nbsp;<strong>Add Time</strong>
        </Button>;

        if (this.props.timeEntries.loaded) {
          return this.renderResults(addTimeEntryButton);
        }

        return <div id="add-button-container">{ addTimeEntryButton }</div>;
      })()}
      { this.state.showTimeEntryDialog &&
      <TimeEntryDialog
        show={ this.state.showTimeEntryDialog }
        onClose={ this.closeTimeEntryDialog }
        multipleEntryAllowed={ this.state.allowMultipleTimeEntries }
        rentalAgreementId={ this.state.rentalAgreementId }
      />
      }
    </div>;
  },
});


function mapStateToProps(state) {
  return {
    projects: state.lookups.projectsCurrentFiscal,
    localAreas: state.lookups.localAreas,
    owners: state.lookups.ownersLite,
    equipment: state.lookups.equipment.ts,
    timeEntries: state.models.timeEntries,
    favourites: state.models.favourites.timeEntry,
    search: state.search.timeEntries,
    ui: state.ui.timeEntries,
  };
}

export default connect(mapStateToProps)(TimeEntry);
