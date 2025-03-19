import PropTypes from 'prop-types';
import React from 'react';
import { Badge } from 'react-bootstrap';

class BadgeLabel extends React.Component {
  static propTypes = {
    bsPrefix: PropTypes.string,
    variant: PropTypes.string,
    className: PropTypes.string,
    children: PropTypes.node,
  };

  render() {
    return (
      <Badge
        bsPrefix={this.props.bsPrefix}
        variant={this.props.variant}
        className={`badge-label ${this.props.className || ''}`}
      >
        {this.props.children}
      </Badge>
    );
  }
}

export default BadgeLabel;
