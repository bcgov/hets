import React from 'react';
import classNames from 'classnames';
import { Modal, Button } from 'react-bootstrap';

import Form from './Form.jsx';
import Spinner from './Spinner.jsx';


var FormDialog = React.createClass({
  propTypes: {
    show: React.PropTypes.bool.isRequired,
    title: React.PropTypes.node,
    id: React.PropTypes.string,
    className: React.PropTypes.string,
    bsSize: React.PropTypes.string,
    isReadOnly: React.PropTypes.bool,
    isSaving: React.PropTypes.bool,
    onClose: React.PropTypes.func.isRequired,
    onSubmit: React.PropTypes.func,
    saveButtonLabel: React.PropTypes.string,
    closeButtonLabel: React.PropTypes.string,
    children: React.PropTypes.node,
  },

  closeDialog() {
    this.props.onClose();
  },

  formSubmitted() {
    const { isSaving, onSubmit } = this.props;

    if (!isSaving) {
      onSubmit();
    }
  },

  renderBody() {
    const { children, saveButtonLabel, closeButtonLabel, isReadOnly, isSaving } = this.props;

    return (
      <Form onSubmit={this.formSubmitted}>
        <Modal.Body>
          {children}
        </Modal.Body>
        <Modal.Footer>
          <Button onClick={this.closeDialog}>{closeButtonLabel || 'Close'}</Button>
          {!isReadOnly && (
            <Button bsStyle="primary" type="submit" disabled={isSaving}>
              {saveButtonLabel || 'Save'}
              {isSaving && <Spinner/>}
            </Button>
          )}
        </Modal.Footer>
      </Form>
    );
  },

  render() {
    const { id, className, title, show, bsSize } = this.props;

    return (
      <Modal
        backdrop="static"
        id={id}
        bsSize={bsSize}
        className={classNames('form-dialog', className)}
        show={show}
        onHide={this.closeDialog}>
        <Modal.Header closeButton>
          {title && (
            <Modal.Title>{title}</Modal.Title>
          )}
        </Modal.Header>
        {show && this.renderBody()}
      </Modal>
    );
  },
});

export default FormDialog;
