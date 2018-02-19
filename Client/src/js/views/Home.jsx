import React from 'react';

import { connect } from 'react-redux';

import { PageHeader, Row, Col, Button } from 'react-bootstrap';

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
    router: React.PropTypes.object,
  },

  componentDidMount() {
    Api.searchOwners({status: Constant.OWNER_STATUS_CODE_PENDING});
    Api.searchEquipmentList({status: Constant.EQUIPMENT_STATUS_CODE_PENDING});
  },

  goToUnapprovedOwners() {
    var search = {
      statusCode: Constant.OWNER_STATUS_CODE_PENDING,
    };
    store.dispatch({ type: Action.UPDATE_OWNERS_SEARCH, owners: search });
    this.props.router.push({ pathname: Constant.OWNERS_PATHNAME });
  },

  goToUnapprovedEquipment() {
    var search = {
      statusCode: Constant.EQUIPMENT_STATUS_CODE_PENDING,
    };
    store.dispatch({ type: Action.UPDATE_EQUIPMENT_LIST_SEARCH, equipmentList: search });
    this.props.router.push({ pathname: Constant.EQUIPMENT_PATHNAME });
  },

  render() {
    return <div id="home">
      <PageHeader>{this.props.currentUser.fullName}<br/>{this.props.currentUser.districtName} District</PageHeader>
      <Row>
        <Col md={8}>
          <Button onClick={ this.goToUnapprovedOwners }>Unapproved owners ({Object.keys(this.props.unapprovedOwners.data).length})</Button>
          <Button onClick={ this.goToUnapprovedEquipment }>Unapproved equipment ({Object.keys(this.props.unapprovedEquipment.data).length})</Button>          
        </Col>
        <Col md={4}>

        </Col>
      </Row>
    </div>;
  },
});


function mapStateToProps(state) {
  return {
    currentUser: state.user,
    search: state.search.owners,
    unapprovedOwners: state.models.owners,
    unapprovedEquipment: state.models.equipmentList,
  };
}

export default connect(mapStateToProps)(Home);
