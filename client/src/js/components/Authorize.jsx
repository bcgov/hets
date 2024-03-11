import React from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';

//helper functions to use in other components

export const any = (...permissions) => (_, getState) => {
  //returns true if user has any of the listed permissions. Equivelent to user has permission1 OR permission2.
  const currentUserPermissions = getState().user.permissions;
  let result = permissions.some((permission) => currentUserPermissions?.includes(permission));
  return result;
};

export const all = (...permissions) => (_, getState) => {
  //returns true if user has all of the listed permissions. Equivelent to user has permission1 AND permission2.
  const currentUserPermissions = getState().user.permissions;
  let result = permissions.every((permission) => currentUserPermissions?.includes(permission));
  return result;
};

//end helper functions

const Authorize = ({ currentUser, requires, condition, children }) => {
  let authorized = requires ? currentUser.hasPermission(requires) : condition;

  if (!authorized) {
    return <></>;
  }
  return children;
};

const mapStateToProps = (state) => ({
  currentUser: state.user,
});

Authorize.propTypes = {
  currentUser: PropTypes.object,
  children: PropTypes.node,
  requires: PropTypes.string, //takes precedence over condition. Shortcut when there's just one permission needed.
  condition: PropTypes.bool, //allows for custom logic. Can use with 'any' or 'all' helper functions. ie. any('permission1', 'permission2')
};

export default connect(mapStateToProps)(Authorize);
