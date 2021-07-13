import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import * as Constant from '../constants';

class Authorize extends React.Component {
  static propTypes = {
    currentUser: PropTypes.object,
    children: PropTypes.node,
  };

  render() {
    var authorized = this.props.currentUser.hasPermission(Constant.PERMISSION_WRITE_ACCESS);

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
