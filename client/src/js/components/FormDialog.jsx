import PropTypes from 'prop-types';
import React from 'react';
import classNames from 'classnames';
import { Modal, Button } from 'react-bootstrap';

import Form from './Form.jsx';
import Spinner from './Spinner.jsx';
import Authorize from './Authorize.jsx';

import * as Constant from '../constants';

class FormDialog extends React.Component {
  static propTypes = {
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

  closeDialog = () => {
    this.props.onClose();
  };

  formSubmitted = () => {
    const { isSaving, onSubmit } = this.props;

    if (!isSaving) {
      onSubmit();
    }
  };

  renderBody = () => {
    const { children, saveButtonLabel, closeButtonLabel, isReadOnly, isSaving } = this.props;

    return (
      <Form onSubmit={this.formSubmitted}>
        <Modal.Body>{children}</Modal.Body>
        <Modal.Footer>
          <Button className="btn-custom" onClick={this.closeDialog}>
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
  };

  render() {
    const { id, className, title, show, size } = this.props;

    return (
      <Modal
        backdrop="static"
        id={id}
        size={size}
        className={classNames('form-dialog', className)}
        show={show}
        onHide={this.closeDialog}
      >
        <Modal.Header closeButton>{title && <Modal.Title>{title}</Modal.Title>}</Modal.Header>
        {show && this.renderBody()}
      </Modal>
    );
  }
}

export default FormDialog;
