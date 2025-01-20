import PropTypes from 'prop-types';
import React from 'react';
import classNames from 'classnames';
import { Modal, Button } from 'react-bootstrap';

import Form from './Form.jsx';
import Spinner from './Spinner.jsx';
import Authorize from './Authorize.jsx';

import * as Constant from '../constants';

const FormDialog = ({
  show,
  title,
  id,
  className,
  size,
  isReadOnly,
  isSaving,
  onClose,
  onSubmit,
  saveButtonLabel,
  closeButtonLabel,
  children,
}) => {
  const closeDialog = () => {
    onClose();
  };

  const formSubmitted = () => {
    if (!isSaving && onSubmit) {
      onSubmit();
    }
  };

  const renderBody = () => (
    <Form onSubmit={formSubmitted}>
      <Modal.Body>{children}</Modal.Body>
      <Modal.Footer>
        <Button className="btn-custom" onClick={closeDialog}>
          {closeButtonLabel || 'Close'}
        </Button>
        {!isReadOnly && (
          <Authorize requires={Constant.PERMISSION_WRITE_ACCESS}>
            <Button variant="primary" type="submit" disabled={isSaving}>
              {saveButtonLabel || 'Save'}
              {isSaving && <Spinner />}
            </Button>
          </Authorize>
        )}
      </Modal.Footer>
    </Form>
  );

  return (
    <Modal
      backdrop="static"
      id={id}
      size={size}
      className={classNames('form-dialog', className)}
      show={show}
      onHide={closeDialog}
    >
      <Modal.Header closeButton>{title && <Modal.Title>{title}</Modal.Title>}</Modal.Header>
      {show && renderBody()}
    </Modal>
  );
};

FormDialog.propTypes = {
  show: PropTypes.bool.isRequired,
  title: PropTypes.node,
  id: PropTypes.string,
  className: PropTypes.string,
  size: PropTypes.string,
  isReadOnly: PropTypes.bool,
  isSaving: PropTypes.bool,
  onClose: PropTypes.func.isRequired,
  onSubmit: PropTypes.func,
  saveButtonLabel: PropTypes.string,
  closeButtonLabel: PropTypes.string,
  children: PropTypes.node,
};

export default FormDialog;
