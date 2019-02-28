import React from 'react';

import { connect } from 'react-redux';

import { Link } from 'react-router';

import { PageHeader, Well, Alert, Row, Col, Button, ButtonGroup, Glyphicon, ControlLabel  } from 'react-bootstrap';

import _ from 'lodash';
import Moment from 'moment';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import store from '../store';

import DateControl from '../components/DateControl.jsx';
import Favourites from '../components/Favourites.jsx';
import MultiDropdown from '../components/MultiDropdown.jsx';
import SortTable from '../components/SortTable.jsx';
import Spinner from '../components/Spinner.jsx';
import TooltipButton from '../components/TooltipButton.jsx';
import Form from '../components/Form.jsx';

import { formatDateTime, toZuluTime } from '../utils/date';

var WcbCglCoverage = React.createClass({
  propTypes: {
    localAreas: React.PropTypes.object,
    owners: React.PropTypes.object,
    ownersCoverage: React.PropTypes.object,
    favourites: React.PropTypes.object,
    search: React.PropTypes.object,
    ui: React.PropTypes.object,
    router: React.PropTypes.object,
  },

  getInitialState() {
    return {
      loading: true,
      search: {
        localAreaIds: this.props.search.localAreaIds || [],
        ownerIds: this.props.search.ownerIds || [],
        wcbExpiry: this.props.search.wcbExpiry || '',
        cglExpiry: this.props.search.cglExpiry || '',
      },
      ui : {
        sortField: this.props.ui.sortField || 'localAreaLabel',
        sortDesc: this.props.ui.sortDesc === true,
      },
    };
  },

  buildSearchParams() {
    var searchParams = {};

    if (this.state.search.localAreaIds.length > 0) {
      searchParams.localAreas = this.state.search.localAreaIds;
    }

    if (this.state.search.ownerIds.length > 0) {
      searchParams.owners = this.state.search.ownerIds;
    }

    var wcbExpiryDate = Moment(this.state.search.wcbExpiry);
    if (wcbExpiryDate && wcbExpiryDate.isValid()) {
      searchParams.wcbExpiry = toZuluTime(wcbExpiryDate.startOf('day'));
    }

    var cglExpiryDate = Moment(this.state.search.cglExpiry);
    if (cglExpiryDate && cglExpiryDate.isValid()) {
      searchParams.cglExpiry = toZuluTime(cglExpiryDate.startOf('day'));
    }

    return searchParams;
  },

  componentDidMount() {
    Api.getOwnersLite().then(() => {
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
    Api.searchOwnersCoverage(this.buildSearchParams());
  },

  search(e) {
    e.preventDefault();
    this.fetch();
  },

  clearSearch() {
    var defaultSearchParameters = {
      localAreaIds: [],
      ownerIds: [],
      wcbExpiry: '',
      cglExpiry: '',
    };

    this.setState({ search: defaultSearchParameters }, () => {
      store.dispatch({ type: Action.UPDATE_OWNERS_COVERAGE_SEARCH, ownersCoverage: this.state.search });
      store.dispatch({ type: Action.CLEAR_OWNERS_COVERAGE });
    });
  },

  updateSearchState(state, callback) {
    this.setState({ search: { ...this.state.search, ...state, ...{ loaded: true } }}, () =>{
      store.dispatch({ type: Action.UPDATE_OWNERS_COVERAGE_SEARCH, ownersCoverage: this.state.search });
      if (callback) { callback(); }
    });
  },

  updateUIState(state, callback) {
    this.setState({ ui: { ...this.state.ui, ...state }}, () =>{
      store.dispatch({ type: Action.UPDATE_OWNERS_COVERAGE_UI, ownersCoverage: this.state.ui });
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
    if (Object.keys(this.props.ownersCoverage.data).length === 0) {
      return <Alert bsStyle="success">No results</Alert>;
    }

    var ownersCoverage = _.sortBy(this.props.ownersCoverage.data, entry => {
      var sortValue = entry[this.state.ui.sortField];
      if (typeof sortValue === 'string') {
        return sortValue.toLowerCase();
      }
      return sortValue;
    });

    if (this.state.ui.sortDesc) {
      _.reverse(ownersCoverage);
    }

    return <SortTable sortField={ this.state.ui.sortField } sortDesc={ this.state.ui.sortDesc } onSort={ this.updateUIState } headers={[
      { field: 'localAreaLabel',       title: 'Local Area'   },
      { field: 'ownerCode',            title: 'Owner Code'   },
      { field: 'organizationName',     title: 'Company Name' },
      { field: 'primaryContactNumber', title: 'Phone'        },
      { field: 'primaryContactCell',   title: 'Cell'         },
      { field: 'wcbNumber',            title: 'WCB Number'   },
      { field: 'wcbExpiryDate',        title: 'WCB Expires'  },
      { field: 'cglNumber',            title: 'CGL Policy'   },
      { field: 'cglExpiryDate',        title: 'CGL Expires'  },
    ]}>
      {
        _.map(ownersCoverage, (entry) => {
          return <tr key={ entry.id }>
            <td>{ entry.localAreaLabel }</td>
            <td>{ entry.ownerCode }</td>
            <td><Link to={`${Constant.OWNERS_PATHNAME}/${entry.id}`}>{ entry.organizationName }</Link></td>
            <td>{ entry.primaryContactNumber }</td>
            <td>{ entry.primaryContactCell }</td>
            <td>{ entry.wcbNumber }</td>
            <td>{ formatDateTime(entry.wcbExpiryDate, 'YYYY-MMM-DD') }</td>
            <td>{ entry.cglNumber }</td>
            <td>{ formatDateTime(entry.cglExpiryDate, 'YYYY-MMM-DD') }</td>
          </tr>;
        })
      }
    </SortTable>;
  },

  matchesLocalAreaFilter(localAreaId) {
    if (this.state.search.localAreaIds.length == 0) {
      return true;
    }

    return _.includes(this.state.search.localAreaIds, localAreaId);
  },

  updateLocalAreaSearchState(state) {
    this.updateSearchState(state, this.filterSelectedOwners);
  },

  filterSelectedOwners() {
    var acceptableOwnerIds = _.map(this.getFilteredOwners(), 'id');
    var ownerIds = _.intersection(this.state.search.ownerIds, acceptableOwnerIds);
    this.updateSearchState({ ownerIds: ownerIds }, this.filterSelectedEquipment);
  },

  getFilteredOwners() {
    return _.chain(this.props.owners)
      .filter(x => this.matchesLocalAreaFilter(x.localAreaId))
      .sortBy('organizationName')
      .value();
  },

  render() {
    const { loading } = this.state;

    var resultCount = '';
    if (this.props.ownersCoverage.loaded) {
      resultCount = '(' + Object.keys(this.props.ownersCoverage.data).length + ')';
    }

    var localAreas = _.sortBy(this.props.localAreas, 'name');
    var owners = this.getFilteredOwners();

    return <div id="wcg-cgl-coverage">
      <PageHeader>WCB / CGL Coverage { resultCount }
        <ButtonGroup id="wcg-cgl-coverage-buttons">
          <TooltipButton onClick={ this.print } disabled={ !this.props.ownersCoverage.loaded } disabledTooltip={ 'Please complete the search to enable this function.' }>
            <Glyphicon glyph="print" title="Print" />
          </TooltipButton>
        </ButtonGroup>
      </PageHeader>
      <Well id="wcg-cgl-coverage-bar" bsSize="small" className="clearfix">
        <Row>
          <Form onSubmit={ this.search }>
            <Col xs={9} sm={10} id="wcg-cgl-coverage-filters">
              <div className="input-container">
                <ControlLabel>Local Areas:</ControlLabel>
                <MultiDropdown id="localAreaIds" placeholder="Local Areas"
                  items={ localAreas } selectedIds={ this.state.search.localAreaIds } updateState={ this.updateLocalAreaSearchState } showMaxItems={ 2 } />
              </div>
              <div className="input-container">
                <ControlLabel>Companies:</ControlLabel>
                <MultiDropdown id="ownerIds" disabled={ loading } placeholder="Companies" fieldName="organizationName"
                  items={ owners } selectedIds={ this.state.search.ownerIds } updateState={ this.updateSearchState } showMaxItems={ 2 } />
              </div>
              <DateControl id="wcbExpiry" date={ this.state.search.wcbExpiry } updateState={ this.updateSearchState } label="WCB Expiry Before:" title="WCB Expiry Before"/>
              <DateControl id="cglExpiry" date={ this.state.search.cglExpiry } updateState={ this.updateSearchState } label="CGL Expiry Before:" title="CGL Expiry Before"/>
              <Button id="search-button" bsStyle="primary" type="submit">Search</Button>
              <Button id="clear-search-button" onClick={ this.clearSearch }>Clear</Button>
            </Col>
          </Form>
          <Col xs={3} sm={2}>
            <Favourites id="wcg-cgl-coverage-faves-dropdown" type="ownersCoverage" favourites={ this.props.favourites } data={ this.state.search } onSelect={ this.loadFavourite } pullRight />
          </Col>
        </Row>
      </Well>

      {(() => {
        if (this.props.ownersCoverage.loading) {
          return <div style={{ textAlign: 'center' }}><Spinner/></div>;
        }
        if (this.props.ownersCoverage.loaded) {
          return this.renderResults();
        }
      })()}
    </div>;
  },
});

function mapStateToProps(state) {
  return {
    localAreas: state.lookups.localAreas,
    owners: state.models.ownersLite.data,
    ownersCoverage: state.models.ownersCoverage,
    favourites: state.models.favourites.ownersCoverage,
    search: state.search.ownersCoverage,
    ui: state.ui.ownersCoverage,
  };
}

export default connect(mapStateToProps)(WcbCglCoverage);
