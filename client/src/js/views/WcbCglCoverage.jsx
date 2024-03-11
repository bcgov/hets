import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import { Alert, Row, Col, ButtonToolbar, Button, ButtonGroup } from 'react-bootstrap';
import _ from 'lodash';
import Moment from 'moment';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';

import PageHeader from '../components/ui/PageHeader.jsx';
import SearchBar from '../components/ui/SearchBar.jsx';
import DateControl from '../components/DateControl.jsx';
import Favourites from '../components/Favourites.jsx';
import Form from '../components/Form.jsx';
import MultiDropdown from '../components/MultiDropdown.jsx';
import SortTable from '../components/SortTable.jsx';
import Spinner from '../components/Spinner.jsx';
import PrintButton from '../components/PrintButton.jsx';

import { formatDateTime, toZuluTime } from '../utils/date';

class WcbCglCoverage extends React.Component {
  static propTypes = {
    localAreas: PropTypes.object,
    owners: PropTypes.object,
    ownersCoverage: PropTypes.object,
    favourites: PropTypes.object,
    search: PropTypes.object,
    ui: PropTypes.object,
    router: PropTypes.object,
  };

  constructor(props) {
    super(props);

    this.state = {
      loading: true,
      search: {
        localAreaIds: props.search.localAreaIds || [],
        ownerIds: props.search.ownerIds || [],
        wcbExpiry: props.search.wcbExpiry || '',
        cglExpiry: props.search.cglExpiry || '',
      },
      ui: {
        sortField: props.ui.sortField || 'localAreaLabel',
        sortDesc: props.ui.sortDesc === true,
      },
    };
  }

  buildSearchParams = () => {
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
  };

  componentDidMount() {
    this.props.dispatch(Api.getOwnersLite());

    // If this is the first load, then look for a default favourite
    if (_.isEmpty(this.props.search)) {
      var defaultFavourite = _.find(this.props.favourites, (f) => f.isDefault);
      if (defaultFavourite) {
        this.loadFavourite(defaultFavourite);
      }
    }
  }

  fetch = () => {
    this.props.dispatch(Api.searchOwnersCoverage(this.buildSearchParams()));
  };

  search = (e) => {
    e.preventDefault();
    this.fetch();
  };

  clearSearch = () => {
    var defaultSearchParameters = {
      localAreaIds: [],
      ownerIds: [],
      wcbExpiry: '',
      cglExpiry: '',
    };

    this.setState({ search: defaultSearchParameters }, () => {
      this.props.dispatch({
        type: Action.UPDATE_OWNERS_COVERAGE_SEARCH,
        ownersCoverage: this.state.search,
      });
      this.props.dispatch({ type: Action.CLEAR_OWNERS_COVERAGE });
    });
  };

  updateSearchState = (state, callback) => {
    this.setState({ search: { ...this.state.search, ...state, ...{ loaded: true } } }, () => {
      this.props.dispatch({
        type: Action.UPDATE_OWNERS_COVERAGE_SEARCH,
        ownersCoverage: this.state.search,
      });
      if (callback) {
        callback();
      }
    });
  };

  updateUIState = (state, callback) => {
    this.setState({ ui: { ...this.state.ui, ...state } }, () => {
      this.props.dispatch({
        type: Action.UPDATE_OWNERS_COVERAGE_UI,
        ownersCoverage: this.state.ui,
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
    if (Object.keys(this.props.ownersCoverage.data).length === 0) {
      return <Alert variant="success">No results</Alert>;
    }

    var ownersCoverage = _.sortBy(this.props.ownersCoverage.data, (entry) => {
      var sortValue = entry[this.state.ui.sortField];
      if (typeof sortValue === 'string') {
        return sortValue.toLowerCase();
      }
      return sortValue;
    });

    if (this.state.ui.sortDesc) {
      _.reverse(ownersCoverage);
    }

    return (
      <SortTable
        sortField={this.state.ui.sortField}
        sortDesc={this.state.ui.sortDesc}
        onSort={this.updateUIState}
        headers={[
          { field: 'localAreaLabel', title: 'Local Area' },
          { field: 'ownerCode', title: 'Owner Code' },
          { field: 'organizationName', title: 'Company Name' },
          { field: 'primaryContactNumber', title: 'Phone' },
          { field: 'primaryContactCell', title: 'Cell' },
          { field: 'wcbNumber', title: 'WCB Number' },
          { field: 'wcbExpiryDate', title: 'WCB Expires' },
          { field: 'cglNumber', title: 'CGL Policy' },
          { field: 'cglExpiryDate', title: 'CGL Expires' },
        ]}
      >
        {_.map(ownersCoverage, (entry) => {
          return (
            <tr key={entry.id}>
              <td>{entry.localAreaLabel}</td>
              <td>{entry.ownerCode}</td>
              <td>
                <Link to={`${Constant.OWNERS_PATHNAME}/${entry.id}`}>{entry.organizationName}</Link>
              </td>
              <td>{entry.primaryContactNumber}</td>
              <td>{entry.primaryContactCell}</td>
              <td>{entry.wcbNumber}</td>
              <td>{formatDateTime(entry.wcbExpiryDate, 'YYYY-MMM-DD')}</td>
              <td>{entry.cglNumber}</td>
              <td>{formatDateTime(entry.cglExpiryDate, 'YYYY-MMM-DD')}</td>
            </tr>
          );
        })}
      </SortTable>
    );
  };

  matchesLocalAreaFilter = (localAreaId) => {
    if (this.state.search.localAreaIds.length === 0) {
      return true;
    }

    return _.includes(this.state.search.localAreaIds, localAreaId);
  };

  updateLocalAreaSearchState = (state) => {
    this.updateSearchState(state, this.filterSelectedOwners);
  };

  filterSelectedOwners = () => {
    var acceptableOwnerIds = _.map(this.getFilteredOwners(), 'id');
    var ownerIds = _.intersection(this.state.search.ownerIds, acceptableOwnerIds);
    this.updateSearchState({ ownerIds: ownerIds }, this.filterSelectedEquipment);
  };

  getFilteredOwners = () => {
    return _.chain(this.props.owners.data)
      .filter((x) => this.matchesLocalAreaFilter(x.localAreaId))
      .sortBy('organizationName')
      .value();
  };

  render() {
    var resultCount = '';
    if (this.props.ownersCoverage.loaded) {
      resultCount = '(' + Object.keys(this.props.ownersCoverage.data).length + ')';
    }

    var localAreas = _.sortBy(this.props.localAreas, 'name');
    var owners = this.getFilteredOwners();

    return (
      <div id="wcg-cgl-coverage">
        <PageHeader>
          WCB / CGL Coverage {resultCount}
          <ButtonGroup>
            <PrintButton disabled={!this.props.ownersCoverage.loaded} />
          </ButtonGroup>
        </PageHeader>
        <SearchBar>
          <Form onSubmit={this.search}>
            <Row>
              <Col xs={9} sm={10} id="filters">
                <ButtonToolbar>
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
                    updateState={this.updateSearchState}
                    showMaxItems={2}
                  />
                  <DateControl
                    id="wcbExpiry"
                    date={this.state.search.wcbExpiry}
                    updateState={this.updateSearchState}
                    label="WCB Exp Before:"
                    title="WCB Expiry Before"
                  />
                  <DateControl
                    id="cglExpiry"
                    date={this.state.search.cglExpiry}
                    updateState={this.updateSearchState}
                    label="CGL Exp Before:"
                    title="CGL Expiry Before"
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
                    id="wcg-cgl-coverage-faves-dropdown"
                    type="ownersCoverage"
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
          if (this.props.ownersCoverage.loading) {
            return (
              <div style={{ textAlign: 'center' }}>
                <Spinner />
              </div>
            );
          }
          if (this.props.ownersCoverage.loaded) {
            return this.renderResults();
          }
        })()}
      </div>
    );
  }
}

const mapStateToProps = (state) => ({
  localAreas: state.lookups.localAreas,
  owners: state.lookups.owners.lite,
  ownersCoverage: state.models.ownersCoverage,
  favourites: state.models.favourites.ownersCoverage,
  search: state.search.ownersCoverage,
  ui: state.ui.ownersCoverage,
});

const mapDispatchToProps = (dispatch) => ({ dispatch });

export default connect(mapStateToProps, mapDispatchToProps)(WcbCglCoverage);
