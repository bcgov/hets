import PropTypes from 'prop-types';
import React from 'react';
import { Badge } from 'react-bootstrap';

class BadgeLabel extends React.Component {
  static propTypes = {
    bsPrefix: PropTypes.string,
    bg: PropTypes.string,
    className: PropTypes.string,
    children: PropTypes.node,
  };

  render() {
    return (
      <Badge
        bsPrefix={this.props.bsPrefix}
        bg={this.props.bg}
        className={`badge-label ${this.props.className || ''}`}
      >
        {this.props.children}
      </Badge>
    );
  }
}

export default BadgeLabel;
