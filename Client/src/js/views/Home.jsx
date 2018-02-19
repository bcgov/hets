import React from 'react';

import { connect } from 'react-redux';

import { PageHeader, Row, Col, Button } from 'react-bootstrap';

import * as Action from '../actionTypes';

var Home = React.createClass({
  propTypes: {
    currentUser: React.PropTypes.object,
  },

  goUnapprovedOwners() {
    var search = {
      hired: false,
      loaded: true,
      ownerId: 0,
      ownerName: "Owner",
      selectedEquipmentTypesIds: [],
      selectedLocalAreasIds: [],
    }
    this.setState({ search: { ...search, ...{ loaded: true } }}, () => {
      store.dispatch({ type: Action.UPDATE_OWNERS_SEARCH, owners: this.state.search });
      if (callback) { callback(); }
    });
  },

  render: function() {
    return <div id="home">
      <PageHeader>{this.props.currentUser.fullName}<br/>{this.props.currentUser.districtName} District</PageHeader>
      <Row>
        <Col md={8}>
          <h2>Home Page</h2>
          <Button onClick={ this.goUnapprovedOwners }>See all unapproved owners</Button>
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
  };
}

export default connect(mapStateToProps)(Home);
