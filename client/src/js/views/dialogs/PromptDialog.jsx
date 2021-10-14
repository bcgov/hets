import React, { useRef } from 'react';
import PropTypes from 'prop-types';

import { Modal, Button } from 'react-bootstrap';

//Purpose to serve as a message prompt for users to confirm before proceeding to the next dialog.

const PromptDialog = ({ show, toggle, onConfirm, title, children, ...props }) => {
  const ref = useRef();

  const focus = () => {
    if (ref.current) {
      ref.current.focus();
    }
  };

  const dialogToggle = (event, callback) => {
    toggle();
    if (callback) {
      callback();
    }
  };
  return (
    <Modal show={show} onHide={dialogToggle} onEntered={focus} {...props}>
      <Modal.Header closeButton>
        <Modal.Title>{title}</Modal.Title>
      </Modal.Header>
      <Modal.Body>{children}</Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={dialogToggle}>
          Close
        </Button>
        <Button variant="primary" onClick={() => dialogToggle(null, onConfirm)} ref={ref}>
          Continue
        </Button>
      </Modal.Footer>
    </Modal>
  );
};

PromptDialog.propTypes = {
  show: PropTypes.bool.isRequired,
  toggle: PropTypes.func.isRequired, // should be function to toggle this dialog in component.
  onConfirm: PropTypes.func.isRequired, //should be function to open up next modal if users clicks to continue
  title: PropTypes.string,
};

PromptDialog.defaultProps = {
  size: 'md',
  title: 'Please Confirm',
};

export default PromptDialog;
