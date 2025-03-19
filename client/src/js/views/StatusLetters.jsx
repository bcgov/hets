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

const StatusLetters = () => {
  const dispatch = useDispatch();
  const localAreas = useSelector((state) => state.lookups.localAreas);
  const owners = useSelector((state) => state.lookups.owners.lite);

  const [localAreaIds, setLocalAreaIds] = useState([]);
  const [ownerIds, setOwnerIds] = useState([]);

  useEffect(() => {
    dispatch(Api.getOwnersLite());
  }, [dispatch]);

  const updateState = (state, callback) => {
    if (state.localAreaIds !== undefined) setLocalAreaIds(state.localAreaIds);
    if (state.ownerIds !== undefined) setOwnerIds(state.ownerIds);
    if (callback) callback();
  };

  const getStatusLetters = async () => {
    const filename = 'StatusLetters-' + formatDateTimeUTCToLocal(new Date(), Constant.DATE_TIME_FILENAME) + '.docx';
    try {
      const res = await dispatch(
        Api.getStatusLettersDoc({
          localAreas: localAreaIds,
          owners: ownerIds,
        })
      );
      saveAs(res, filename);
    } catch (error) {
      console.log(error);
    }
  };

  const getMailingLabel = async () => {
    const filename = 'MailingLabels-' + formatDateTimeUTCToLocal(new Date(), Constant.DATE_TIME_FILENAME) + '.docx';
    try {
      const res = await dispatch(
        Api.getMailingLabelsDoc({
          localAreas: localAreaIds,
          owners: ownerIds,
        })
      );
      saveAs(res, filename);
    } catch (error) {
      console.log(error);
    }
  };

  const matchesLocalAreaFilter = (localAreaId) => {
    if (localAreaIds.length === 0) {
      return true;
    }
    return _.includes(localAreaIds, localAreaId);
  };

  const updateLocalAreaState = (state) => {
    updateState(state, filterSelectedOwners);
  };

  const filterSelectedOwners = () => {
    const acceptableOwnerIds = _.map(getFilteredOwners(), 'id');
    const filteredOwnerIds = _.intersection(ownerIds, acceptableOwnerIds);
    setOwnerIds(filteredOwnerIds);
  };

  const getFilteredOwners = () => {
    return _.chain(owners.data)
      .filter((x) => matchesLocalAreaFilter(x.localAreaId))
      .sortBy('organizationName')
      .value();
  };

  const sortedLocalAreas = _.sortBy(localAreas, 'name');
  const filteredOwners = getFilteredOwners();

  return (
    <div id="status-letters">
      <PageHeader>Status Letters</PageHeader>
      <SearchBar>
        <Row>
          <Col md={12} id="filters">
            <ButtonToolbar>
              <MultiDropdown
                id="localAreaIds"
                placeholder="Local Areas"
                items={sortedLocalAreas}
                selectedIds={localAreaIds}
                updateState={updateLocalAreaState}
                showMaxItems={2}
              />
              <MultiDropdown
                id="ownerIds"
                placeholder="Companies"
                fieldName="organizationName"
                items={filteredOwners}
                disabled={!owners.loaded}
                selectedIds={ownerIds}
                updateState={updateState}
                showMaxItems={2}
              />
              <Button onClick={getStatusLetters} variant="primary">
                Status Letters
              </Button>
              <Button onClick={getMailingLabel} variant="primary">
                Mailing Labels
              </Button>
            </ButtonToolbar>
          </Col>
        </Row>
      </SearchBar>
    </div>
  );
};

StatusLetters.propTypes = {
  localAreas: PropTypes.object,
  owners: PropTypes.object,
};

export default StatusLetters;