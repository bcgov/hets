import PropTypes from 'prop-types';
import React, { useEffect } from 'react';

import { Row, Col, Button } from 'react-bootstrap';
import _ from 'lodash';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';

import PageHeader from '../components/ui/PageHeader.jsx';
import SubHeader from '../components/ui/SubHeader.jsx';
import { useDispatch, useSelector } from 'react-redux';

const Home = ({ history }) => {
  const dispatch = useDispatch();
  const currentUser = useSelector((state) => state.user);
  const searchSummaryCounts = useSelector((state) => state.lookups.searchSummaryCounts);

  useEffect(() => {
    
    fetch();
  }, []); 

  const fetch = () => {
    dispatch(Api.getSearchSummaryCounts());
  };

  const goToUnapprovedOwners = () => {
    const unapprovedStatus = Constant.OWNER_STATUS_CODE_PENDING;

    dispatch({ type: Action.UPDATE_OWNERS_SEARCH, owners: { statusCode: unapprovedStatus } });
    dispatch(Api.searchOwners({ status: unapprovedStatus }));

    history.push({ pathname: Constant.OWNERS_PATHNAME });
  };

  const goToUnapprovedEquipment = () => {
    const unapprovedStatus = Constant.EQUIPMENT_STATUS_CODE_PENDING;

    dispatch({ type: Action.UPDATE_EQUIPMENT_LIST_SEARCH, equipmentList: { statusCode: unapprovedStatus } });
    dispatch(Api.searchEquipmentList({ status: unapprovedStatus }));

    history.push({ pathname: Constant.EQUIPMENT_PATHNAME });
  };

  const goToHiredEquipment = () => {
    dispatch({
      type: Action.UPDATE_EQUIPMENT_LIST_SEARCH,
      equipmentList: { statusCode: Constant.EQUIPMENT_STATUS_CODE_APPROVED, hired: true },
    });
    dispatch(Api.searchEquipmentList({
      status: Constant.EQUIPMENT_STATUS_CODE_APPROVED,
      hired: true,
    }));

    history.push({ pathname: Constant.EQUIPMENT_PATHNAME });
  };

  const goToBlockedRotationLists = () => {
    dispatch({
      type: Action.UPDATE_RENTAL_REQUESTS_SEARCH,
      rentalRequests: { statusCode: Constant.RENTAL_REQUEST_STATUS_CODE_IN_PROGRESS },
    });
    dispatch(Api.searchRentalRequests({
      status: Constant.RENTAL_REQUEST_STATUS_CODE_IN_PROGRESS,
    }));

    history.push({ pathname: Constant.RENTAL_REQUESTS_PATHNAME });
  };

  const counts = searchSummaryCounts;

  return (
    <div id="home">
      <PageHeader>
        {currentUser.fullName}
        <br />
        {currentUser.districtName} District
      </PageHeader>
      <div className="well">
        <SubHeader title="Summary" />
        <Row>
          <Col md={12} className="btn-container">
            <Button variant="primary" className="btn-custom" onClick={goToUnapprovedOwners}>
              Unapproved owners {!_.isEmpty(counts) && `(${counts.unapprovedOwners})`}
            </Button>
            <Button variant="primary" className="btn-custom" onClick={goToUnapprovedEquipment}>
              Unapproved equipment {!_.isEmpty(counts) && `(${counts.unapprovedEquipment})`}
            </Button>
            <Button variant="primary" className="btn-custom" onClick={goToHiredEquipment}>
              Currently hired equipment {!_.isEmpty(counts) && `(${counts.hiredEquipment})`}
            </Button>
            <Button variant="primary" className="btn-custom" onClick={goToBlockedRotationLists}>
              Blocked rotation lists {!_.isEmpty(counts) && `(${counts.inProgressRentalRequests})`}
            </Button>
          </Col>
        </Row>
      </div>
    </div>
  );
};

Home.propTypes = {
  history: PropTypes.object,
};

export default Home;
