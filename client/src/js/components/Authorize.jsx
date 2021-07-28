import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';

class Authorize extends React.Component {
  static propTypes = {
    currentUser: PropTypes.object,
    children: PropTypes.node,
    requires: PropTypes.string,
  };

  render() {
    var authorized = this.props.currentUser.hasPermission(this.props.requires);

    if (!authorized) {
      return <></>;
    } else {
      return this.props.children;
    }
  }
}

function mapStateToProps(state) {
  return {
    currentUser: state.user,
  };
}

export default connect(mapStateToProps)(Authorize);
