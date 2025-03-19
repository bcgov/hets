import PropTypes from 'prop-types';
import React from 'react';
import { Button } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Link } from 'react-router-dom';
import _ from 'lodash';

const EditButton = ({ view, hide, pathname, onClick, name, ...rest }) => {
  const Buttonprops = _.omit(rest, 'view', 'name', 'hide', 'pathname');

  const button = (
    <Button
      title={`${view ? 'View' : 'Edit'} ${name}`}
      size="sm"
      className={hide ? 'd-none' : 'btn-custom'}
      onClick={onClick}
      {...Buttonprops}
    >
      <FontAwesomeIcon icon={view ? 'edit' : 'pencil-alt'} />
    </Button>
  );

  return pathname ? <Link to={pathname}> {button} </Link> : button;
};
EditButton.propTypes = {
  pathname: PropTypes.string,
  onClick: PropTypes.func,
  view: PropTypes.bool,
  name: PropTypes.string,
  hide: PropTypes.bool,
};

export default EditButton;
