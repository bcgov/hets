import PropTypes from 'prop-types';
import React from 'react';
import Promise from 'bluebird';
import { Button } from 'react-bootstrap';
import _ from 'lodash';

import ModalDialog from './ModalDialog.jsx';


class EditDialog extends React.Component {
  static propTypes = {
    title: PropTypes.node,
    didChange: PropTypes.func.isRequired,
    isValid: PropTypes.func.isRequired,
    onSave: PropTypes.func.isRequired,
    onClose: PropTypes.func.isRequired,
    show: PropTypes.bool.isRequired,
    className: PropTypes.string,
    readOnly: PropTypes.bool,
    saveText: PropTypes.string,
    closeText: PropTypes.string,
    children: PropTypes.node,
  };

  state = {
    saving: false,
    savePromise: Promise.resolve(),
  };

  componentWillUnmount() {
    this.state.savePromise.cancel();
  }

  save = () => {
    if (this.props.isValid()) {
      if (this.props.didChange()) {
        var onSaveCompleted = () => this.setState({ saving: false });
        this.setState({
          saving: true,
          savePromise: Promise.resolve(this.props.onSave()).delay(1000).then(onSaveCompleted, onSaveCompleted),
        });
      } else {
        this.props.onClose();
      }
    }
  };

  render() {
    var props = _.omit(this.props, 'className', 'onSave', 'didChange', 'isValid', 'updateState', 'saveText', 'closeText');

    return (
      <ModalDialog
        backdrop="static"
        className={ `edit-dialog ${ this.props.className || '' }` }
        { ...props }
        footer={
          <span>
            <Button onClick={ this.props.onClose }>{ this.props.closeText || 'Close' }</Button>
            {
              this.props.readOnly || <Button bsStyle="primary" onClick={ this.save } disabled={ this.state.saving }>{ this.props.saveText || 'Save' }</Button>
            }
          </span>
        }
      />
    );
  }
}

export default EditDialog;
