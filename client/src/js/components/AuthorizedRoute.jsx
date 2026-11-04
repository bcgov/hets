import React from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { Navigate } from 'react-router-dom';

import * as Constant from '../constants';

//only renders React-Router-Dom route if user has permission.

const AuthorizedRoute = ({ user, requires, component: Component }) => {
  if (user.permissions.includes(requires)) {
    return <Component />;
  }
  return <Navigate replace to={Constant.UNAUTHORIZED_PATHNAME} />;
};

AuthorizedRoute.propTypes = {
  requires: PropTypes.string.isRequired,
  component: PropTypes.elementType,
};

const mapStateToProps = (state) => {
  return {
    user: state.user,
  };
};

export default connect(mapStateToProps, null)(AuthorizedRoute);
