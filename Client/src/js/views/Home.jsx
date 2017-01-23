import React from 'react';
import { connect } from 'react-redux';

var Home = React.createClass({
  propTypes: {
    currentUser: React.PropTypes.object,
  },

  render: function() {
    return <div id="home">     
    </div>;
  },
});


function mapStateToProps(state) {
  return {
    currentUser: state.user,
  };
}

export default connect(mapStateToProps)(Home);
