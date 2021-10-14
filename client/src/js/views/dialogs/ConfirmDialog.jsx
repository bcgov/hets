import PropTypes from 'prop-types';
import React from 'react';

import FormDialog from '../../components/FormDialog.jsx';

class ConfirmDialog extends React.Component {
  static propTypes = {
    onSave: PropTypes.func.isRequired,
    onClose: PropTypes.func.isRequired,
    show: PropTypes.bool,
    title: PropTypes.string,
    saveText: PropTypes.string,
    closeText: PropTypes.string,
    children: PropTypes.node,
  };

  render() {
    return (
      <FormDialog
        id="confirm-dialog"
        show={ this.props.show }
        title={ this.props.title }
        closeButtonLabel={ this.props.closeText || 'Cancel' }
        saveButtonLabel={ this.props.saveText || 'Confirm' }
        onClose={ this.props.onClose }
        onSubmit={ this.props.onSave }>
        { this.props.children }
      </FormDialog>
    );
  }
}

export default ConfirmDialog;
