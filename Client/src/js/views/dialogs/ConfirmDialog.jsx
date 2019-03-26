import React from 'react';

import EditDialog from '../../components/EditDialog.jsx';

var ConfirmDialog = React.createClass({
  propTypes: {
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
    title: React.PropTypes.string,
    saveText: React.PropTypes.string,
    closeText: React.PropTypes.string,
    children: React.PropTypes.node,
  },

  didChange() {
    return true;
  },

  isValid() {
    return true;
  },

  onSave() {
    this.props.onSave();
  },

  render() {
    return <EditDialog id="confirm-dialog" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title={<strong>{ this.props.title }</strong>}
      closeText={ this.props.closeText || 'Cancel' } saveText={ this.props.saveText || 'Confirm' } backdropClassName="confirm"
    >
      { this.props.children }
    </EditDialog>;
  },
});

export default ConfirmDialog;
