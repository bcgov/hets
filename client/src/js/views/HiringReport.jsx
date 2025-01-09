import React, { useState, useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
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

const HiringReport = () => {
  const dispatch = useDispatch();

  const {
    projects,
    localAreas,
    owners,
    equipment,
    hiringResponses,
    favourites,
    search: searchProps,
    ui: uiProps,
  } = useSelector((state) => ({
    projects: state.lookups.projectsCurrentFiscal,
    localAreas: state.lookups.localAreas,
    owners: state.lookups.owners.hires,
    equipment: state.lookups.equipment.hires,
    hiringResponses: state.models.hiringResponses,
    favourites: state.models.favourites.hiringReport,
    search: state.search.hiringResponses,
    ui: state.ui.hiringResponses,
  }));

  const [search, setSearch] = useState({
    projectIds: searchProps.projectIds || [],
    localAreaIds: searchProps.localAreaIds || [],
    ownerIds: searchProps.ownerIds || [],
    equipmentIds: searchProps.equipmentIds || [],
  });

  const [ui, setUI] = useState({
    sortField: uiProps.sortField || 'name',
    sortDesc: uiProps.sortDesc === true,
  });

  useEffect(() => {
    dispatch(Api.getProjectsCurrentFiscal());
    dispatch(Api.getEquipmentHires());
    dispatch(Api.getOwnersLiteHires());

    if (_.isEmpty(searchProps)) {
      const defaultFavourite = _.find(favourites, (f) => f.isDefault);
      if (defaultFavourite) {
        loadFavourite(defaultFavourite);
      }
    }
  }, [dispatch, favourites, searchProps]);

  const buildSearchParams = () => ({
    ...(search.projectIds.length > 0 && { projects: search.projectIds }),
    ...(search.localAreaIds.length > 0 && { localAreas: search.localAreaIds }),
    ...(search.ownerIds.length > 0 && { owners: search.ownerIds }),
    ...(search.equipmentIds.length > 0 && { equipment: search.equipmentIds }),
  });

  const fetch = () => {
    dispatch(Api.searchHiringReport(buildSearchParams()));
  };

  const handleSearch = (e) => {
    e.preventDefault();
    fetch();
  };

  const clearSearch = () => {
    const defaultSearchParameters = {
      projectIds: [],
      localAreaIds: [],
      ownerIds: [],
      equipmentIds: [],
    };
    setSearch(defaultSearchParameters);
    dispatch({ type: Action.UPDATE_HIRING_RESPONSES_SEARCH, hiringResponses: defaultSearchParameters });
    dispatch({ type: Action.CLEAR_HIRING_RESPONSES });
  };

  const updateSearchState = (state, callback) => {
    setSearch((prev) => ({ ...prev, ...state }));
    dispatch({ type: Action.UPDATE_HIRING_RESPONSES_SEARCH, hiringResponses: { ...search, ...state } });
    if (callback) callback();
  };

  const updateUIState = (state, callback) => {
    setUI((prev) => ({ ...prev, ...state }));
    dispatch({ type: Action.UPDATE_HIRING_RESPONSES_UI, hiringResponses: { ...ui, ...state } });
    if (callback) callback();
  };

  const loadFavourite = (favourite) => {
    updateSearchState(JSON.parse(favourite.value), fetch);
  };

  const renderResults = () => {
    if (_.isEmpty(hiringResponses.data)) {
      return <Alert variant="success">No results</Alert>;
    }

    const sortedResponses = _.sortBy(hiringResponses.data, (response) =>
      typeof response[ui.sortField] === 'string' ? response[ui.sortField].toLowerCase() : response[ui.sortField]
    );

    if (ui.sortDesc) _.reverse(sortedResponses);

    return (
      <SortTable
        sortField={ui.sortField}
        sortDesc={ui.sortDesc}
        onSort={updateUIState}
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
        {sortedResponses.map((entry) => {
          const reason =
            entry.reason === Constant.HIRING_REFUSAL_OTHER ? entry.offerResponseNote : entry.reason;
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
                  {entry.projectNumber || 'N/A'}
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

  const projectsSorted = _.sortBy(projects.data, 'name');
  const localAreasSorted = _.sortBy(localAreas, 'name');
  const filteredOwners = _.sortBy(
    owners.data.filter(
      (owner) => _.intersection(search.projectIds, owner.projectIds).length > 0 || search.projectIds.length === 0
    ),
    'organizationName'
  );
  const filteredEquipment = _.sortBy(
    equipment.data.filter(
      (equip) =>
        (_.intersection(search.projectIds, equip.projectIds).length > 0 || search.projectIds.length === 0) &&
        (_.includes(search.ownerIds, equip.ownerId) || search.ownerIds.length === 0)
    ),
    'equipmentCode'
  );

  return (
    <div id="hiring-report">
      <PageHeader>
        Hiring Report - Not Hired / Force Hire ({hiringResponses.loaded && Object.keys(hiringResponses.data).length})
        <ButtonGroup>
          <PrintButton disabled={!hiringResponses.loaded} />
        </ButtonGroup>
      </PageHeader>
      <SearchBar>
        <Form onSubmit={handleSearch}>
          <Row>
            <Col xs={9} sm={10} id="filters">
              <ButtonToolbar>
                <MultiDropdown
                  id="projectIds"
                  disabled={!projects.loaded}
                  placeholder="Projects"
                  fieldName="label"
                  items={projectsSorted}
                  selectedIds={search.projectIds}
                  updateState={(state) => updateSearchState({ projectIds: state })}
                  showMaxItems={2}
                />
                <MultiDropdown
                  id="localAreaIds"
                  placeholder="Local Areas"
                  items={localAreasSorted}
                  selectedIds={search.localAreaIds}
                  updateState={(state) => updateSearchState({ localAreaIds: state })}
                  showMaxItems={2}
                />
                <MultiDropdown
                  id="ownerIds"
                  disabled={!owners.loaded}
                  placeholder="Companies"
                  fieldName="organizationName"
                  items={filteredOwners}
                  selectedIds={search.ownerIds}
                  updateState={(state) => updateSearchState({ ownerIds: state })}
                  showMaxItems={2}
                />
                <MultiDropdown
                  id="equipmentIds"
                  disabled={!equipment.loaded}
                  placeholder="Equipment"
                  fieldName="equipmentCode"
                  items={filteredEquipment}
                  selectedIds={search.equipmentIds}
                  updateState={(state) => updateSearchState({ equipmentIds: state })}
                  showMaxItems={2}
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
                  id="hiring-report-faves-dropdown"
                  type="hiringReport"
                  favourites={favourites}
                  data={search}
                  onSelect={loadFavourite}
                />
              </Row>
            </Col>
          </Row>
        </Form>
      </SearchBar>
      {hiringResponses.loading ? (
        <div style={{ textAlign: 'center' }}>
          <Spinner />
        </div>
      ) : (
        hiringResponses.loaded && renderResults()
      )}
    </div>
  );
};

export default HiringReport;
