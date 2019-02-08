import React from 'react';

import { connect } from 'react-redux';

import { Well, PageHeader, Row, Col, Button } from 'react-bootstrap';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import store from '../store';

var Home = React.createClass({
  propTypes: {
    currentUser: React.PropTypes.object,
    owners: React.PropTypes.object,
    unapprovedOwners: React.PropTypes.object,
    unapprovedEquipment: React.PropTypes.object,
    hiredEquipment: React.PropTypes.object,
    blockedRotationLists: React.PropTypes.object,
    router: React.PropTypes.object,
  },

  componentDidMount() {
    this.fetch();
  },

  fetch() {
    Api.getUnapprovedOwners();
    Api.getUnapprovedEquipment();
    Api.getHiredEquipment();
    Api.getBlockedRotationLists();
  },

  goToUnapprovedOwners() {
    var unapprovedStatus = Constant.OWNER_STATUS_CODE_PENDING;

    // update search parameters
    store.dispatch({ type: Action.UPDATE_OWNERS_SEARCH, owners: { statusCode: unapprovedStatus } });

    // perform search
    Api.searchOwners({ status: unapprovedStatus });

    // navigate to search page
    this.props.router.push({ pathname: Constant.OWNERS_PATHNAME });
  },

  goToUnapprovedEquipment() {
    var unapprovedStatus = Constant.EQUIPMENT_STATUS_CODE_PENDING;

    // update search parameters
    store.dispatch({ type: Action.UPDATE_EQUIPMENT_LIST_SEARCH, equipmentList: { statusCode: Constant.EQUIPMENT_STATUS_CODE_PENDING } });

    // perform search
    Api.searchEquipmentList({ status: unapprovedStatus });

    // navigate to search page
    this.props.router.push({ pathname: Constant.EQUIPMENT_PATHNAME });
  },

  goToHiredEquipment() {
    // update search parameters
    store.dispatch({ type: Action.UPDATE_EQUIPMENT_LIST_SEARCH, equipmentList: { statusCode: Constant.EQUIPMENT_STATUS_CODE_APPROVED, hired: true  } });

    // perform search
    Api.searchEquipmentList({ status: Constant.EQUIPMENT_STATUS_CODE_APPROVED, hired: true  });

    // navigate to search page
    this.props.router.push({ pathname: Constant.EQUIPMENT_PATHNAME });
  },

  goToBlockedRotationLists() {
    // update search parameters
    store.dispatch({ type: Action.UPDATE_RENTAL_REQUESTS_SEARCH, rentalRequests: { statusCode: Constant.RENTAL_REQUEST_STATUS_CODE_IN_PROGRESS  } });

    // perform search
    Api.searchRentalRequests({ status: Constant.RENTAL_REQUEST_STATUS_CODE_IN_PROGRESS  });

    // navigate to search page
    this.props.router.push({ pathname: Constant.RENTAL_REQUESTS_PATHNAME });
  },

  render() {
    return <div id="home">
      <PageHeader>{this.props.currentUser.fullName}<br/>{this.props.currentUser.districtName} District</PageHeader>
      <Well>
        <h3>Summary</h3>
        <Row>
          <Col md={12} className="btn-container">
            <Button onClick={ this.goToUnapprovedOwners }>Unapproved owners { this.props.unapprovedOwners.loaded && `(${ Object.keys(this.props.unapprovedOwners.data).length })` }</Button>
            <Button onClick={ this.goToUnapprovedEquipment }>Unapproved equipment { this.props.unapprovedEquipment.loaded && `(${ Object.keys(this.props.unapprovedEquipment.data).length })` }</Button>
            <Button onClick={ this.goToHiredEquipment }>Currently hired equipment { this.props.hiredEquipment.loaded && `(${ Object.keys(this.props.hiredEquipment.data).length })` }</Button>
            <Button onClick={ this.goToBlockedRotationLists }>Blocked rotation lists { this.props.blockedRotationLists.loaded && `(${ Object.keys(this.props.blockedRotationLists.data).length })` }</Button>
          </Col>
        </Row>
      </Well>
    </div>;
  },
});

function mapStateToProps(state) {
  return {
    currentUser: state.user,
    search: state.search.owners,
    unapprovedOwners: state.models.unapprovedOwners,
    unapprovedEquipment: state.models.unapprovedEquipmentList,
    hiredEquipment: state.models.hiredEquipmentList,
    blockedRotationLists: state.models.blockedRotationLists,
  };
}

export default connect(mapStateToProps)(Home);
