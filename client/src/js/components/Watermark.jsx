import PropTypes from 'prop-types';
import React from 'react';

const Watermark = ({ enable }) => {
  if (enable) {
    return (
      <div className="d-none d-print-block watermark">
        View Only
        <br />
        Not for Hiring
      </div>
    );
  }
  return null;
};

Watermark.propTypes = {
  enable: PropTypes.bool,
};

export default Watermark;
