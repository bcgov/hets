import PropTypes from 'prop-types';
import React from 'react';

class Watermark extends React.Component {
  static propTypes = {
    enable: PropTypes.bool,
  };

  render() {
    if (this.props.enable) {
      return (
        <div className="visible-print watermark">
          View Only
          <br />
          Not for Hiring
        </div>
      );
    } else {
      return <></>;
    }
  }
}

export default Watermark;
