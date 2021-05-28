import PropTypes from 'prop-types';
import React from 'react';
import { Well } from 'react-bootstrap';


class SearchBar extends React.Component {
  static propTypes = {
    children: PropTypes.node,
    id: PropTypes.string,
  };

  render() {
    return (
      <Well className="search-bar clearfix" id={ this.props.id } bsSize="small">
        { this.props.children }
      </Well>
    );
  }
}


export default SearchBar;
