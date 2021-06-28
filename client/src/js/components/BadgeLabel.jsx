import PropTypes from 'prop-types';
import React from 'react';
import { Badge } from 'react-bootstrap';

class BadgeLabel extends React.Component {
  static propTypes = {
    bsClass: PropTypes.string,
    bsStyle: PropTypes.string,
    className: PropTypes.string,
    children: PropTypes.node,
  };

  render() {
    return (
      //temporary fix when migrating from Label -> Badge. Will need to change bsStyle and bsClass
      <Badge
        bsClass={this.props.bsClass}
        bsStyle={this.props.bsStyle}
        className={`badge-label ${this.props.className || ''}`}
      >
        {this.props.children}
      </Badge>
    );
  }
}

export default BadgeLabel;
