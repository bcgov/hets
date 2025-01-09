import React, { useEffect } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { Row, Col, Button } from 'react-bootstrap';
import _ from 'lodash';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';

import PageHeader from '../components/ui/PageHeader.jsx';
import SubHeader from '../components/ui/SubHeader.jsx';

const Home = ({ currentUser, searchSummaryCounts, history, dispatch }) => {
  
  useEffect(() => {
    dispatch(Api.getSearchSummaryCounts());
  }, [dispatch]);

  const goToUnapprovedOwners = () => {
    const unapprovedStatus = Constant.OWNER_STATUS_CODE_PENDING;

    // Update search parameters
    dispatch({
      type: Action.UPDATE_OWNERS_SEARCH,
      owners: { statusCode: unapprovedStatus },
    });

    // Perform search
    dispatch(Api.searchOwners({ status: unapprovedStatus }));

    // Navigate to search page
    history.push({ pathname: Constant.OWNERS_PATHNAME });
  };

  const goToUnapprovedEquipment = () => {
    const unapprovedStatus = Constant.EQUIPMENT_STATUS_CODE_PENDING;

    // Update search parameters
    dispatch({
      type: Action.UPDATE_EQUIPMENT_LIST_SEARCH,
      equipmentList: { statusCode: unapprovedStatus },
    });

    // Perform search
    dispatch(Api.searchEquipmentList({ status: unapprovedStatus }));

    // Navigate to search page
    history.push({ pathname: Constant.EQUIPMENT_PATHNAME });
  };

  const goToHiredEquipment = () => {
    // Update search parameters
    dispatch({
      type: Action.UPDATE_EQUIPMENT_LIST_SEARCH,
      equipmentList: {
        statusCode: Constant.EQUIPMENT_STATUS_CODE_APPROVED,
        hired: true,
      },
    });

    // Perform search
    dispatch(Api.searchEquipmentList({
      status: Constant.EQUIPMENT_STATUS_CODE_APPROVED,
      hired: true,
    }));

    // Navigate to search page
    history.push({ pathname: Constant.EQUIPMENT_PATHNAME });
  };

  const goToBlockedRotationLists = () => {
    // Update search parameters
    dispatch({
      type: Action.UPDATE_RENTAL_REQUESTS_SEARCH,
      rentalRequests: {
        statusCode: Constant.RENTAL_REQUEST_STATUS_CODE_IN_PROGRESS,
      },
    });

    // Perform search
    dispatch(Api.searchRentalRequests({
      status: Constant.RENTAL_REQUEST_STATUS_CODE_IN_PROGRESS,
    }));

    // Navigate to search page
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
            <Button
              variant="primary"
              className="btn-custom"
              onClick={goToUnapprovedOwners}
            >
              Unapproved owners {!_.isEmpty(counts) && `(${counts.unapprovedOwners})`}
            </Button>
            <Button
              variant="primary"
              className="btn-custom"
              onClick={goToUnapprovedEquipment}
            >
              Unapproved equipment {!_.isEmpty(counts) && `(${counts.unapprovedEquipment})`}
            </Button>
            <Button
              variant="primary"
              className="btn-custom"
              onClick={goToHiredEquipment}
            >
              Currently hired equipment {!_.isEmpty(counts) && `(${counts.hiredEquipment})`}
            </Button>
            <Button
              variant="primary"
              className="btn-custom"
              onClick={goToBlockedRotationLists}
            >
              Blocked rotation lists {!_.isEmpty(counts) && `(${counts.inProgressRentalRequests})`}
            </Button>
          </Col>
        </Row>
      </div>
    </div>
  );
};

Home.propTypes = {
  currentUser: PropTypes.object,
  searchSummaryCounts: PropTypes.object,
  history: PropTypes.object,
  dispatch: PropTypes.func.isRequired,
};

const mapStateToProps = (state) => ({
  currentUser: state.user,
  searchSummaryCounts: state.lookups.searchSummaryCounts,
});

export default connect(mapStateToProps)(Home);
