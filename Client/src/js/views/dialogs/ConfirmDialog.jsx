import PropTypes from 'prop-types';
import React from 'react';

import EditDialog from '../../components/EditDialog.jsx';

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

  didChange = () => {
    return true;
  };

  isValid = () => {
    return true;
  };

  onSave = () => {
    this.props.onSave();
  };

  render() {
    return <EditDialog id="confirm-dialog" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title={<strong>{ this.props.title }</strong>}
      closeText={ this.props.closeText || 'Cancel' } saveText={ this.props.saveText || 'Confirm' } backdropClassName="confirm"
    >
      { this.props.children }
    </EditDialog>;
  }
}

export default ConfirmDialog;
