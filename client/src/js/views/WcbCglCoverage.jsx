import PropTypes from 'prop-types';
import React, { useState, useEffect } from 'react';
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

const WcbCglCoverage = (props) => {
  const [loading, setLoading] = useState(true);
  const [search, setSearch] = useState({
    localAreaIds: props.search.localAreaIds || [],
    ownerIds: props.search.ownerIds || [],
    wcbExpiry: props.search.wcbExpiry || '',
    cglExpiry: props.search.cglExpiry || '',
  });
  const [ui, setUi] = useState({
    sortField: props.ui.sortField || 'localAreaLabel',
    sortDesc: props.ui.sortDesc === true,
  });

  useEffect(() => {
    props.dispatch(Api.getOwnersLite());

    if (_.isEmpty(props.search)) {
      const defaultFavourite = _.find(props.favourites, (f) => f.isDefault);
      if (defaultFavourite) {
        loadFavourite(defaultFavourite);
      }
    }
  }, [props.search, props.favourites, props.dispatch]);

  const buildSearchParams = () => {
    const searchParams = {};

    if (search.localAreaIds.length > 0) {
      searchParams.localAreas = search.localAreaIds;
    }

    if (search.ownerIds.length > 0) {
      searchParams.owners = search.ownerIds;
    }

    const wcbExpiryDate = Moment(search.wcbExpiry);
    if (wcbExpiryDate && wcbExpiryDate.isValid()) {
      searchParams.wcbExpiry = toZuluTime(wcbExpiryDate.startOf('day'));
    }

    const cglExpiryDate = Moment(search.cglExpiry);
    if (cglExpiryDate && cglExpiryDate.isValid()) {
      searchParams.cglExpiry = toZuluTime(cglExpiryDate.startOf('day'));
    }

    return searchParams;
  };

  const fetch = () => {
    props.dispatch(Api.searchOwnersCoverage(buildSearchParams()));
  };

  const searchHandler = (e) => {
    e.preventDefault();
    fetch();
  };

  const clearSearch = () => {
    const defaultSearchParameters = {
      localAreaIds: [],
      ownerIds: [],
      wcbExpiry: '',
      cglExpiry: '',
    };

    setSearch(defaultSearchParameters);
    props.dispatch({
      type: Action.UPDATE_OWNERS_COVERAGE_SEARCH,
      ownersCoverage: defaultSearchParameters,
    });
    props.dispatch({ type: Action.CLEAR_OWNERS_COVERAGE });
  };

  const updateSearchState = (state, callback) => {
    setSearch((prevSearch) => {
      const updatedSearch = { ...prevSearch, ...state, loaded: true };
      props.dispatch({
        type: Action.UPDATE_OWNERS_COVERAGE_SEARCH,
        ownersCoverage: updatedSearch,
      });
      if (callback) callback();
      return updatedSearch;
    });
  };

  const updateUIState = (state, callback) => {
    setUi((prevUi) => {
      const updatedUi = { ...prevUi, ...state };
      props.dispatch({
        type: Action.UPDATE_OWNERS_COVERAGE_UI,
        ownersCoverage: updatedUi,
      });
      if (callback) callback();
      return updatedUi;
    });
  };

  const loadFavourite = (favourite) => {
    updateSearchState(JSON.parse(favourite.value), fetch);
  };

  const renderResults = () => {
    if (Object.keys(props.ownersCoverage.data).length === 0) {
      return <Alert variant="success">No results</Alert>;
    }

    let ownersCoverage = _.sortBy(props.ownersCoverage.data, (entry) => {
      const sortValue = entry[ui.sortField];
      if (typeof sortValue === 'string') {
        return sortValue.toLowerCase();
      }
      return sortValue;
    });

    if (ui.sortDesc) {
      _.reverse(ownersCoverage);
    }

    return (
      <SortTable
        sortField={ui.sortField}
        sortDesc={ui.sortDesc}
        onSort={updateUIState}
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
        {_.map(ownersCoverage, (entry) => (
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
        ))}
      </SortTable>
    );
  };

  const matchesLocalAreaFilter = (localAreaId) => {
    return search.localAreaIds.length === 0 || _.includes(search.localAreaIds, localAreaId);
  };

  const updateLocalAreaSearchState = (state) => {
    updateSearchState(state, filterSelectedOwners);
  };

  const filterSelectedOwners = () => {
    const acceptableOwnerIds = _.map(getFilteredOwners(), 'id');
    const ownerIds = _.intersection(search.ownerIds, acceptableOwnerIds);
    updateSearchState({ ownerIds }, filterSelectedEquipment);
  };

  const filterSelectedEquipment = () => {
    // Logic to filter equipment based on the selected owners
  };

  const getFilteredOwners = () => {
    return _.chain(props.owners.data)
      .filter((x) => matchesLocalAreaFilter(x.localAreaId))
      .sortBy('organizationName')
      .value();
  };

  let resultCount = '';
  if (props.ownersCoverage.loaded) {
    resultCount = `(${Object.keys(props.ownersCoverage.data).length})`;
  }

  const localAreas = _.sortBy(props.localAreas, 'name');
  const owners = getFilteredOwners();

  return (
    <div id="wcg-cgl-coverage">
      <PageHeader>
        WCB / CGL Coverage {resultCount}
        <ButtonGroup>
          <PrintButton disabled={!props.ownersCoverage.loaded} />
        </ButtonGroup>
      </PageHeader>
      <SearchBar>
        <Form onSubmit={searchHandler}>
          <Row>
            <Col xs={9} sm={10} id="filters">
              <ButtonToolbar>
                <MultiDropdown
                  id="localAreaIds"
                  placeholder="Local Areas"
                  items={localAreas}
                  selectedIds={search.localAreaIds}
                  updateState={updateLocalAreaSearchState}
                  showMaxItems={2}
                />
                <MultiDropdown
                  id="ownerIds"
                  disabled={!props.owners.loaded}
                  placeholder="Companies"
                  fieldName="organizationName"
                  items={owners}
                  selectedIds={search.ownerIds}
                  updateState={updateSearchState}
                  showMaxItems={2}
                />
                <DateControl
                  id="wcbExpiry"
                  date={search.wcbExpiry}
                  updateState={updateSearchState}
                  label="WCB Exp Before:"
                  title="WCB Expiry Before"
                />
                <DateControl
                  id="cglExpiry"
                  date={search.cglExpiry}
                  updateState={updateSearchState}
                  label="CGL Exp Before:"
                  title="CGL Expiry Before"
                />
                <Button id="search-button" variant="primary" type="submit">
                  Search
                </Button>
                <Button className="btn-custom" id="clear-search-button" onClick={clearSearch}>
                  Clear
                </Button>
              </ButtonToolbar>
            </Col>
            <Col xs={3} sm={2} id="search-buttons">
              <Row className="float-right">
                <Favourites
                  id="wcg-cgl-coverage-faves-dropdown"
                  type="ownersCoverage"
                  favourites={props.favourites}
                  data={search}
                  onSelect={loadFavourite}
                />
              </Row>
            </Col>
          </Row>
        </Form>
      </SearchBar>

      {(() => {
        if (props.ownersCoverage.loading) {
          return (
            <div style={{ textAlign: 'center' }}>
              <Spinner />
            </div>
          );
        }
        if (props.ownersCoverage.loaded) {
          return renderResults();
        }
      })()}
    </div>
  );
};

WcbCglCoverage.propTypes = {
  localAreas: PropTypes.object,
  owners: PropTypes.object,
  ownersCoverage: PropTypes.object,
  favourites: PropTypes.object,
  search: PropTypes.object,
  ui: PropTypes.object,
  router: PropTypes.object,
};

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
