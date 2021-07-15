import PropTypes from 'prop-types';
import React from 'react';
import classNames from 'classnames';
import { useHistory } from 'react-router-dom';
import { Button } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

function ReturnButton({ id, className, title }) {
  const history = useHistory();
  return (
    <Button
      id={id}
      className={classNames('return-button', 'btn', 'btn-custom', className)}
      title={title}
      onClick={history.goBack}
    >
      <FontAwesomeIcon icon="arrow-left" /> Return
    </Button>
  );
}

ReturnButton.propTypes = {
  id: PropTypes.string,
  className: PropTypes.string,
  title: PropTypes.string,
};

export default ReturnButton;
