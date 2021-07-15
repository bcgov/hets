import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { Row, Col, Button } from 'react-bootstrap';
import _ from 'lodash';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import store from '../store';

import PageHeader from '../components/ui/PageHeader.jsx';
import SubHeader from '../components/ui/SubHeader.jsx';

class Home extends React.Component {
  static propTypes = {
    currentUser: PropTypes.object,
    searchSummaryCounts: PropTypes.object,
    history: PropTypes.object,
  };

  componentDidMount() {
    this.fetch();
  }

  fetch = () => {
    Api.getSearchSummaryCounts();
  };

  goToUnapprovedOwners = () => {
    var unapprovedStatus = Constant.OWNER_STATUS_CODE_PENDING;

    // update search parameters
    store.dispatch({
      type: Action.UPDATE_OWNERS_SEARCH,
      owners: { statusCode: unapprovedStatus },
    });

    // perform search
    Api.searchOwners({ status: unapprovedStatus });

    // navigate to search page
    this.props.history.push({ pathname: Constant.OWNERS_PATHNAME });
  };

  goToUnapprovedEquipment = () => {
    var unapprovedStatus = Constant.EQUIPMENT_STATUS_CODE_PENDING;

    // update search parameters
    store.dispatch({
      type: Action.UPDATE_EQUIPMENT_LIST_SEARCH,
      equipmentList: { statusCode: Constant.EQUIPMENT_STATUS_CODE_PENDING },
    });

    // perform search
    Api.searchEquipmentList({ status: unapprovedStatus });

    // navigate to search page
    this.props.history.push({ pathname: Constant.EQUIPMENT_PATHNAME });
  };

  goToHiredEquipment = () => {
    // update search parameters
    store.dispatch({
      type: Action.UPDATE_EQUIPMENT_LIST_SEARCH,
      equipmentList: {
        statusCode: Constant.EQUIPMENT_STATUS_CODE_APPROVED,
        hired: true,
      },
    });

    // perform search
    Api.searchEquipmentList({
      status: Constant.EQUIPMENT_STATUS_CODE_APPROVED,
      hired: true,
    });

    // navigate to search page
    this.props.history.push({ pathname: Constant.EQUIPMENT_PATHNAME });
  };

  goToBlockedRotationLists = () => {
    // update search parameters
    store.dispatch({
      type: Action.UPDATE_RENTAL_REQUESTS_SEARCH,
      rentalRequests: {
        statusCode: Constant.RENTAL_REQUEST_STATUS_CODE_IN_PROGRESS,
      },
    });

    // perform search
    Api.searchRentalRequests({
      status: Constant.RENTAL_REQUEST_STATUS_CODE_IN_PROGRESS,
    });

    // navigate to search page
    this.props.history.push({ pathname: Constant.RENTAL_REQUESTS_PATHNAME });
  };

  render() {
    var counts = this.props.searchSummaryCounts;

    return (
      <div id="home">
        <PageHeader>
          {this.props.currentUser.fullName}
          <br />
          {this.props.currentUser.districtName} District
        </PageHeader>
        <div className="well">
          <SubHeader title="Summary" />
          <Row>
            <Col md={12} className="btn-container">
              <Button variant="primary" className="btn-custom" onClick={this.goToUnapprovedOwners}>
                Unapproved owners {!_.isEmpty(counts) && `(${counts.unapprovedOwners})`}
              </Button>
              <Button variant="primary" className="btn-custom" onClick={this.goToUnapprovedEquipment}>
                Unapproved equipment {!_.isEmpty(counts) && `(${counts.unapprovedEquipment})`}
              </Button>
              <Button variant="primary" className="btn-custom" onClick={this.goToHiredEquipment}>
                Currently hired equipment {!_.isEmpty(counts) && `(${counts.hiredEquipment})`}
              </Button>
              <Button variant="primary" className="btn-custom" onClick={this.goToBlockedRotationLists}>
                Blocked rotation lists {!_.isEmpty(counts) && `(${counts.inProgressRentalRequests})`}
              </Button>
            </Col>
          </Row>
        </div>
      </div>
    );
  }
}

function mapStateToProps(state) {
  return {
    currentUser: state.user,
    searchSummaryCounts: state.lookups.searchSummaryCounts,
  };
}

export default connect(mapStateToProps)(Home);
