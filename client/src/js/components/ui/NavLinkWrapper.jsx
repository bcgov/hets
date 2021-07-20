import React from "react";
import PropTypes from "prop-types";
import { withRouter } from "react-router-dom";

//Takes react component (Tag) and gives it React-Router-Dom <Link> behaviour. Includes path matching
//Returns custom component

const NavLinkWrapper = ({
  history,
  location,
  to,
  onClick,
  tag: Tag,
  ...rest
}) => {
  return (
    <Tag
      {...rest}
      onClick={(event) => {
        onClick(event);
        history.push(to);
      }}
    />
  );
};

NavLinkWrapper.propTypes = {
  to: PropTypes.string.isRequired,
  children: PropTypes.node.isRequired,
  history: PropTypes.shape({
    push: PropTypes.func.isRequired,
  }).isRequired,
  onClick: PropTypes.func,
  disabled: PropTypes.bool, //need to check if navigation should be disabled during rollover.
};
NavLinkWrapper.defaultProps = {
  onClick: () => {},
};
export default withRouter(NavLinkWrapper);
