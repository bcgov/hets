import PropTypes from 'prop-types';
import React from 'react';
import { Button } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Link } from 'react-router-dom';
import _ from 'lodash';

class EditButton extends React.Component {
  static propTypes = {
    pathname: PropTypes.string,
    onClick: PropTypes.func,
    view: PropTypes.bool,
    name: PropTypes.string,
    hide: PropTypes.bool,
  };

  render() {
    var props = _.omit(this.props, 'view', 'name', 'hide', 'pathname');

    var button = (
      <Button
        title={`${this.props.view ? 'View' : 'Edit'} ${this.props.name}`}
        size="sm"
        className={this.props.hide ? 'd-none' : 'btn-custom'}
        {...props}
      >
        <FontAwesomeIcon icon={this.props.view ? 'edit' : 'pencil-alt'} />
      </Button>
    );

    return this.props.pathname ? <Link to={this.props.pathname}> {button} </Link> : button;
  }
}

export default EditButton;
