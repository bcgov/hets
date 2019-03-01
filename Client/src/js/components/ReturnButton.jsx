import React from 'react';
import classNames from 'classnames';
import { Link } from 'react-router';
import { Glyphicon } from 'react-bootstrap';


var ReturnButton = React.createClass({
  propTypes: {
    id: React.PropTypes.string,
    className: React.PropTypes.string,
    title: React.PropTypes.string,
    to: React.PropTypes.string.isRequired,
  },

  render() {
    const { id, className, to, title } = this.props;

    return (
      <Link id={id} className={classNames('return-button', 'btn', 'btn-default', className)} to={to} title={title}>
        <Glyphicon glyph="arrow-left" /> Return
      </Link>
    );
  },
});


export default ReturnButton;
