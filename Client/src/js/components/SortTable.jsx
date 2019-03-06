import React from 'react';
import { Table, Glyphicon } from 'react-bootstrap';
import _ from 'lodash';

import Spinner from '../components/Spinner.jsx';


const SortTable = React.createClass({
  propTypes: {
    // Array of objects with key, title, style, children fields
    headers: React.PropTypes.array.isRequired,
    // This should be a from a state.ui object
    sortField: React.PropTypes.string.isRequired,
    // This should be a from a state.ui object
    sortDesc: React.PropTypes.bool.isRequired,
    onSort: React.PropTypes.func.isRequired,
    id: React.PropTypes.string,
    isRefreshing: React.PropTypes.bool,
    children: React.PropTypes.node,
  },

  sort(e) {
    this.props.onSort({
      sortField: e.currentTarget.id,
      sortDesc: this.props.sortField !== e.currentTarget.id ? false : !this.props.sortDesc,
    });
  },

  preventSelection(e) {
    e.preventDefault();
  },

  renderTableHeader() {
    const {headers, sortField, sortDesc} = this.props;

    return headers.map((header) => {
      const key = Array.isArray(header.field) ? header.field.join('-') : header.field;
      if (header.node) {
        return <th id={header.field} key={key} style={header.style}>{header.node}</th>;
      }

      var sortGlyph = '';
      if (_.isEqual(sortField, header.field)) {
        sortGlyph = <span>&nbsp;<Glyphicon glyph={ sortDesc ? 'sort-by-attributes-alt' : 'sort-by-attributes' }/></span>;
      }

      return (
        <th
          id={ header.field }
          key={key}
          onClick={ header.noSort ? '' : this.sort }
          className={ header.class }
          style={{ ...header.style, cursor: header.noSort ? 'default' : 'pointer' }}>
            { header.title }{ sortGlyph }
        </th>
      );
    });
  },

  render() {
    const {id, isRefreshing, children} = this.props;

    return (
      <div id={id} className="sort-table">
        {isRefreshing && <div id="sort-table-refreshing-overlay"><Spinner/></div>}
        <Table condensed striped hover>
          <thead>
            <tr>
              {this.renderTableHeader()}
            </tr>
          </thead>
          <tbody>
            {children}
          </tbody>
        </Table>
      </div>
    );
  },
});

export default SortTable;
