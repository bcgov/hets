import PropTypes from 'prop-types';
import React from 'react';

class SearchBar extends React.Component {
  static propTypes = {
    children: PropTypes.node,
    id: PropTypes.string,
  };

  render() {
    return (
      <div className="search-bar clearfix well well-sm" id={this.props.id}>
        {this.props.children}
      </div>
    );
  }
}

export default SearchBar;
