import PropTypes from 'prop-types';
import React from 'react';
import classNames from 'classnames';
import * as Constant from '../constants';

class AuthorizeSaveButton extends React.Component {
  static propTypes = {
    currentUser: PropTypes.object,
    children: PropTypes.node,
  };

  render() {
    var authorized = this.props.currentUser.hasPermission(Constant.PERMISSION_WRITE_ACCESS);

    if (!authorized) {
      return (
        <div data-toggle="tooltip" data-placement="bottom" title="Not authorized" className={ classNames('cursor-not-allowed') }>
          <div className={ classNames('authorize-save-button') }>
            { this.props.children }
          </div>
        </div>
      );
    } else {
      return (<div>{ this.props.children }</div>);
    }
  }
}

export default AuthorizeSaveButton;
