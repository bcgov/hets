import PropTypes from 'prop-types';
import React from 'react';
import { Badge } from 'react-bootstrap';

const BadgeLabel = ({ bsPrefix, variant, className, children }) => {
  return (
    <Badge bsPrefix={bsPrefix} variant={variant} className={`badge-label ${className || ''}`}>
      {children}
    </Badge>
  );
};

BadgeLabel.propTypes = {
  bsPrefix: PropTypes.string,
  variant: PropTypes.string,
  className: PropTypes.string,
  children: PropTypes.node,
};

export default BadgeLabel;
