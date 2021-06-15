import PropTypes from 'prop-types';
import React from 'react';
import classNames from 'classnames';
import { useHistory } from 'react-router-dom';
import { Button, Glyphicon } from 'react-bootstrap';

function ReturnButton({ id, className, title }) {
  const history = useHistory();
  return (
    <Button
      id={id}
      className={classNames('return-button', 'btn', 'btn-default', className)}
      title={title}
      onClick={history.goBack}
    >
      <Glyphicon glyph="arrow-left" /> Return
    </Button>
  );
}

ReturnButton.propTypes = {
  id: PropTypes.string,
  className: PropTypes.string,
  title: PropTypes.string,
};

export default ReturnButton;
