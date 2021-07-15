import PropTypes from 'prop-types';
import React from 'react';
import { RootCloseWrapper } from 'react-overlays';

class RootCloseMenu extends React.Component {
  static propTypes = {
    open: PropTypes.bool,
    floatRight: PropTypes.bool,
    onClose: PropTypes.func,
    children: PropTypes.node,
  };

  render() {
    return (
      <RootCloseWrapper disabled={!this.props.open} onRootClose={this.props.onClose}>
        <div className={`dropdown-menu ${this.props.floatRight ? 'dropdown-menu-right' : ''}`}>
          {this.props.children}
        </div>
      </RootCloseWrapper>
    );
  }
}

export default RootCloseMenu;
