import React, { useState, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import {
  Alert,
  Row,
  Col,
  ButtonToolbar,
  Button,
  ButtonGroup,
  OverlayTrigger,
  Tooltip,
} from 'react-bootstrap';
import _ from 'lodash';
import Moment from 'moment';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';

import PageHeader from '../components/ui/PageHeader.jsx';
import SearchBar from '../components/ui/SearchBar.jsx';
import CheckboxControl from '../components/CheckboxControl.jsx';
import DateControl from '../components/DateControl.jsx';
import DropdownControl from '../components/DropdownControl.jsx';
import Favourites from '../components/Favourites.jsx';
import FormInputControl from '../components/FormInputControl.jsx';
import MultiDropdown from '../components/MultiDropdown.jsx';
import Spinner from '../components/Spinner.jsx';
import Form from '../components/Form.jsx';
import EquipmentTable from './EquipmentTable.jsx';
import PrintButton from '../components/PrintButton.jsx';

import { toZuluTime } from '../utils/date';

const Equipment = () => {
  const dispatch = useDispatch();
  const {
    currentUser,
    equipmentList,
    localAreas,
    districtEquipmentTypes,
    favourites,
    search: reduxSearch,
    ui: reduxUi,
  } = useSelector((state) => ({
    currentUser: state.user,
    equipmentList: state.models.equipmentList,
    localAreas: state.lookups.localAreas,
    districtEquipmentTypes: state.lookups.districtEquipmentTypes,
    favourites: state.models.favourites.equipment,
    search: state.search.equipmentList,
    ui: state.ui.equipmentList,
  }));

  const [search, setSearch] = useState({
    selectedLocalAreasIds: reduxSearch.selectedLocalAreasIds || [],
    selectedEquipmentTypesIds: reduxSearch.selectedEquipmentTypesIds || [],
    equipmentAttachment: reduxSearch.equipmentAttachment || '',
    ownerName: reduxSearch.ownerName || '',
    lastVerifiedDate: reduxSearch.lastVerifiedDate || '',
    hired: reduxSearch.hired || false,
    twentyYears: reduxSearch.twentyYears || false,
    statusCode: reduxSearch.statusCode || Constant.EQUIPMENT_STATUS_CODE_APPROVED,
    equipmentId: reduxSearch.equipmentId || '',
    projectName: reduxSearch.projectName || '',
  });

  const [ui, setUi] = useState({
    sortField: reduxUi.sortField || 'seniorityText',
    sortDesc: reduxUi.sortDesc === true,
  });

  useEffect(() => {
    dispatch(Api.getDistrictEquipmentTypes());

    if (_.isEmpty(reduxSearch)) {
      const defaultFavourite = _.find(favourites, (f) => f.isDefault);
      if (defaultFavourite) {
        loadFavourite(defaultFavourite);
      }
    }
  }, []);

  const buildSearchParams = () => {
    const searchParams = {};

    if (search.equipmentAttachment) searchParams.equipmentAttachment = search.equipmentAttachment;
    if (search.ownerName) searchParams.ownerName = search.ownerName;
    if (search.hired) searchParams.hired = search.hired;
    if (search.twentyYears) searchParams.twentyYears = search.twentyYears;
    if (search.statusCode) searchParams.status = search.statusCode;
    if (search.selectedLocalAreasIds.length > 0) searchParams.localareas = search.selectedLocalAreasIds;
    if (search.selectedEquipmentTypesIds.length > 0) searchParams.types = search.selectedEquipmentTypesIds;
    if (search.equipmentId) searchParams.equipmentId = search.equipmentId;
    if (search.projectName) searchParams.projectName = search.projectName;

    const notVerifiedSinceDate = Moment(search.lastVerifiedDate);
    if (notVerifiedSinceDate && notVerifiedSinceDate.isValid()) {
      searchParams.notverifiedsincedate = toZuluTime(notVerifiedSinceDate.startOf('day'));
    }

    return searchParams;
  };

  const fetch = () => {
    dispatch(Api.searchEquipmentList(buildSearchParams()));
  };

  const searchHandler = (e) => {
    e.preventDefault();
    fetch();
  };

  const clearSearch = () => {
    const defaultSearchParameters = {
      selectedLocalAreasIds: [],
      selectedEquipmentTypesIds: [],
      equipmentAttachment: '',
      ownerName: '',
      lastVerifiedDate: '',
      hired: false,
      twentyYears: false,
      statusCode: Constant.EQUIPMENT_STATUS_CODE_APPROVED,
      equipmentId: '',
      projectName: '',
    };
    setSearch(defaultSearchParameters);
    dispatch({ type: Action.UPDATE_EQUIPMENT_LIST_SEARCH, equipmentList: defaultSearchParameters });
    dispatch({ type: Action.CLEAR_EQUIPMENT_LIST });
  };

  const updateSearchState = (newState, callback) => {
    setSearch((prev) => {
      const updated = { ...prev, ...newState };
      dispatch({ type: Action.UPDATE_EQUIPMENT_LIST_SEARCH, equipmentList: updated });
      if (callback) callback();
      return updated;
    });
  };

  const updateUiState = (newState, callback) => {
    setUi((prev) => {
      const updated = { ...prev, ...newState };
      dispatch({ type: Action.UPDATE_EQUIPMENT_LIST_UI, equipmentList: updated });
      if (callback) callback();
      return updated;
    });
  };

  const loadFavourite = (favourite) => {
    updateSearchState(JSON.parse(favourite.value), fetch);
  };

  const renderResults = () => {
    if (Object.keys(equipmentList.data).length === 0) {
      return <Alert variant="success">No equipment</Alert>;
    }
    return <EquipmentTable ui={ui} updateUIState={updateUiState} equipmentList={equipmentList.data} />;
  };

  const localAreasSorted = _.sortBy(localAreas, 'name');
  const districtEquipmentTypesFiltered = _.sortBy(
    _.filter(districtEquipmentTypes.data, (type) => type.district.id === currentUser.district.id),
    'districtEquipmentName'
  );

  const resultCount = equipmentList.loaded ? `(${Object.keys(equipmentList.data).length})` : '';

  return (
    <div id="equipment-list">
      <PageHeader>
        Equipment {resultCount}
        <ButtonGroup>
          <PrintButton disabled={!equipmentList.loaded} />
        </ButtonGroup>
      </PageHeader>
      <SearchBar>
        <Form onSubmit={searchHandler}>
          <Row>
            <Col xs={9} sm={10} id="filters">
              {/* Filters Row */}
              <Row>
                <ButtonToolbar>
                  <MultiDropdown
                    id="selectedLocalAreasIds"
                    placeholder="Local Areas"
                    items={localAreasSorted}
                    selectedIds={search.selectedLocalAreasIds}
                    updateState={updateSearchState}
                    showMaxItems={2}
                  />
                  <DropdownControl
                    id="statusCode"
                    title={search.statusCode}
                    updateState={updateSearchState}
                    blankLine="(All)"
                    placeholder="Status"
                    items={[
                      Constant.EQUIPMENT_STATUS_CODE_APPROVED,
                      Constant.EQUIPMENT_STATUS_CODE_PENDING,
                      Constant.EQUIPMENT_STATUS_CODE_ARCHIVED,
                    ]}
                  />
                  <MultiDropdown
                    id="selectedEquipmentTypesIds"
                    placeholder="Equipment Types"
                    fieldName="districtEquipmentName"
                    items={districtEquipmentTypesFiltered}
                    disabled={!districtEquipmentTypes.loaded}
                    selectedIds={search.selectedEquipmentTypesIds}
                    updateState={updateSearchState}
                    showMaxItems={2}
                  />
                  <FormInputControl
                    id="ownerName"
                    type="text"
                    placeholder="Company Name or DBA"
                    title="Searches Company Name And Doing Business As Fields."
                    value={search.ownerName}
                    updateState={updateSearchState}
                  />
                  <CheckboxControl
                    inline
                    id="hired"
                    checked={search.hired}
                    updateState={updateSearchState}
                    label="Hired"
                  />
                  <OverlayTrigger
                    placement="top"
                    trigger={['hover', 'focus']}
                    rootClose
                    overlay={<Tooltip id="old-equipment-tooltip">Equipment 20 years or older</Tooltip>}
                  >
                    <CheckboxControl
                      inline
                      id="twentyYears"
                      checked={search.twentyYears}
                      updateState={updateSearchState}
                      label="20+ Years"
                    />
                  </OverlayTrigger>
                </ButtonToolbar>
              </Row>
              {/* Additional Filters Row */}
              <Row>
                <ButtonToolbar>
                  <DateControl
                    id="lastVerifiedDate"
                    date={search.lastVerifiedDate}
                    label="Not Verified Since:"
                    title="Last Verified Date"
                    updateState={updateSearchState}
                  />
                  <FormInputControl
                    id="equipmentAttachment"
                    placeholder="Attachment"
                    type="text"
                    value={search.equipmentAttachment}
                    updateState={updateSearchState}
                  />
                  <FormInputControl
                    id="equipmentId"
                    placeholder="Equipment Id"
                    type="text"
                    value={search.equipmentId}
                    updateState={updateSearchState}
                  />
                  <FormInputControl
                    id="projectName"
                    placeholder="Project Name"
                    type="text"
                    value={search.projectName}
                    updateState={updateSearchState}
                  />
                </ButtonToolbar>
              </Row>
            </Col>
            <Col xs={3} sm={2} id="search-buttons">
              <Row className="float-right">
                <Favourites
                  id="faves-dropdown"
                  type="equipment"
                  favourites={favourites}
                  data={search}
                  onSelect={loadFavourite}
                />
              </Row>
              <Row className="float-right">
                <Button id="search-button" variant="primary" type="submit">
                  Search
                </Button>
              </Row>
              <Row className="float-right">
                <Button id="clear-search-button" className="btn-custom" onClick={clearSearch}>
                  Clear
                </Button>
              </Row>
            </Col>
          </Row>
        </Form>
      </SearchBar>

      {equipmentList.loading && (
        <div style={{ textAlign: 'center' }}>
          <Spinner />
        </div>
      )}

      {equipmentList.loaded && renderResults()}
    </div>
  );
};

export default Equipment;
