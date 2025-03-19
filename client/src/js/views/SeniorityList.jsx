import PropTypes from 'prop-types';
import React, { useState, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Row, Col, ButtonToolbar, Button } from 'react-bootstrap';
import _ from 'lodash';
import { saveAs } from 'file-saver';

import * as Api from '../api';
import * as Constant from '../constants';

import PageHeader from '../components/ui/PageHeader.jsx';
import SearchBar from '../components/ui/SearchBar.jsx';
import MultiDropdown from '../components/MultiDropdown.jsx';

import { formatDateTimeUTCToLocal } from '../utils/date';

const SeniorityList = () => {
  const dispatch = useDispatch();
  const currentUser = useSelector((state) => state.user);
  const districtEquipmentTypes = useSelector((state) => state.lookups.districtEquipmentTypes);
  const localAreas = useSelector((state) => state.lookups.localAreas);

  const [selectedEquipmentTypeIds, setSelectedEquipmentTypeIds] = useState([]);
  const [selectedLocalAreaIds, setSelectedLocalAreaIds] = useState([]);

  useEffect(() => {
    fetch();
  }, []);

  const fetch = () => {
    dispatch(Api.getDistrictEquipmentTypes());
  };

  const updateState = (state) => {
    if (state.selectedEquipmentTypeIds !== undefined) setSelectedEquipmentTypeIds(state.selectedEquipmentTypeIds);
    if (state.selectedLocalAreaIds !== undefined) setSelectedLocalAreaIds(state.selectedLocalAreaIds);
  };

  const onLocalAreasChanged = () => {
    setSelectedEquipmentTypeIds([]);
  };

  const getFilteredEquipmentTypes = (localAreaIds) => {
    return _.chain(districtEquipmentTypes.data)
      .filter(
        (type) =>
          (type.equipmentCount > 0 && localAreaIds.length === 0) ||
          _.filter(
            type.localAreas,
            (localArea) => _.includes(localAreaIds, localArea.id) && localArea.equipmentCount > 0
          ).length > 0
      )
      .sortBy('districtEquipmentName')
      .value();
  };

  const getRotationList = async (counterCopy) => {
    const filename =
      'SeniorityList-' +
      formatDateTimeUTCToLocal(new Date(), Constant.DATE_TIME_FILENAME) +
      (counterCopy ? '-(CounterCopy)' : '') +
      '.docx';

    try {
      const res = await dispatch(
        Api.equipmentSeniorityListDoc(selectedLocalAreaIds, selectedEquipmentTypeIds, counterCopy)
      );

      saveAs(res, filename);
    } catch (error) {
      console.log(error);
    }
  };

  const localAreasSorted = _.sortBy(localAreas, 'name');
  const filteredEquipmentTypes = getFilteredEquipmentTypes(selectedLocalAreaIds);

  return (
    <div id="seniority-list">
      <PageHeader>Seniority List</PageHeader>
      <SearchBar>
        <Row>
          <Col md={12} id="filters">
            <ButtonToolbar className="btn-container">
              <MultiDropdown
                id="selectedLocalAreaIds"
                className="fixed-width small"
                placeholder="Local Areas"
                items={localAreasSorted}
                selectedIds={selectedLocalAreaIds}
                updateState={updateState}
                onChange={onLocalAreasChanged}
                showMaxItems={2}
              />
              <MultiDropdown
                id="selectedEquipmentTypeIds"
                className="fixed-width"
                placeholder="Equipment Types"
                fieldName="districtEquipmentName"
                items={filteredEquipmentTypes}
                disabled={!districtEquipmentTypes.loaded}
                selectedIds={selectedEquipmentTypeIds}
                updateState={updateState}
                showMaxItems={2}
              />
              <Button onClick={() => getRotationList(false)} variant="primary">
                Seniority List
              </Button>
              <Button onClick={() => getRotationList(true)} variant="primary">
                Seniority List (Counter Copy)
              </Button>
            </ButtonToolbar>
          </Col>
        </Row>
      </SearchBar>
    </div>
  );
};

SeniorityList.propTypes = {
  currentUser: PropTypes.object,
  districtEquipmentTypes: PropTypes.object,
  localAreas: PropTypes.object,
};

export default SeniorityList;