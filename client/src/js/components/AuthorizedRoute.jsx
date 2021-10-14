import React from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { Route, Redirect } from 'react-router-dom';

import * as Constant from '../constants';

//only renders React-Router-Dom route if user has permission.

const AuthorizedRoute = ({ user, requires, ...rest }) => {
  if (user.permissions.includes(requires)) {
    return <Route {...rest} />;
  }
  return <Redirect to={Constant.UNAUTHORIZED_PATHNAME} />;
};

AuthorizedRoute.propTypes = {
  requires: PropTypes.string.isRequired,
  component: PropTypes.func,
};

const mapStateToProps = (state) => {
  return {
    user: state.user,
  };
};

export default connect(mapStateToProps, null)(AuthorizedRoute);
