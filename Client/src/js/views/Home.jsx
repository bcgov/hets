import React from 'react';

import { connect } from 'react-redux';

import { PageHeader, Row, Col } from 'react-bootstrap';

var Home = React.createClass({
  propTypes: {
    currentUser: React.PropTypes.object,
  },

  render: function() {
    return <div id="home">
      <PageHeader>{this.props.currentUser.fullName}<br/>{this.props.currentUser.districtName} District</PageHeader>
      <Row>
        <Col md={8}>
          <h2>Home Page</h2>
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
