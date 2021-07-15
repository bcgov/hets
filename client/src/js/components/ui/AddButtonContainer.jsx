import PropTypes from 'prop-types';
import React from 'react';


class AddButtonContainer extends React.Component {
  static propTypes = {
    children: PropTypes.node,
  };

  render() {
    return (
      <div id="add-button-container">
        { this.props.children }
      </div>
    );
  }
}


export default AddButtonContainer;
