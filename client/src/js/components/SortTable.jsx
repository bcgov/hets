import PropTypes from 'prop-types';
import React from 'react';
import { Table } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import _ from 'lodash';

import Spinner from '../components/Spinner.jsx';

class SortTable extends React.Component {
  static propTypes = {
    // Array of objects with key, title, style, children fields
    headers: PropTypes.array.isRequired,
    // This should be a from a state.ui object
    sortField: PropTypes.oneOfType([PropTypes.string, PropTypes.array]).isRequired,
    // This should be a from a state.ui object
    sortDesc: PropTypes.bool.isRequired,
    onSort: PropTypes.func.isRequired,
    id: PropTypes.string,
    isRefreshing: PropTypes.bool,
    children: PropTypes.node,
  };

  sort = (field) => {
    this.props.onSort({
      sortField: field,
      sortDesc: !this.props.sortDesc,
    });
  };

  preventSelection = (e) => {
    e.preventDefault();
  };

  renderTableHeader = () => {
    const { headers, sortField, sortDesc } = this.props;

    return headers.map((header) => {
      const key = Array.isArray(header.field) ? header.field.join('-') : header.field;
      if (header.node) {
        return (
          <th id={header.field} key={key} style={header.style}>
            {header.node}
          </th>
        );
      }

      var sortGlyph = '';
      if (_.isEqual(sortField, header.field)) {
        sortGlyph = (
          <span>
            &nbsp;
            <FontAwesomeIcon icon={sortDesc ? 'sort-amount-down-alt' : 'sort-amount-down'} />
          </span>
        );
      }

      return (
        <th
          key={key}
          onMouseDown={this.preventSelection}
          onClick={header.noSort ? null : () => this.sort(header.field)}
          className={header.class}
          style={{ ...header.style, cursor: header.noSort ? 'default' : 'pointer' }}
        >
          {header.title}
          {sortGlyph}
        </th>
      );
    });
  };

  render() {
    const { id, isRefreshing, children } = this.props;

    return (
      <div id={id} className="sort-table">
        {isRefreshing && (
          <div id="sort-table-refreshing-overlay">
            <Spinner />
          </div>
        )}
        <Table striped hover>
          <thead>
            <tr>{this.renderTableHeader()}</tr>
          </thead>
          <tbody>{children}</tbody>
        </Table>
      </div>
    );
  }
}

export default SortTable;
