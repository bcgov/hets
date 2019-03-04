import React from 'react';
import { Table, Glyphicon } from 'react-bootstrap';
import _ from 'lodash';


const SortTable = React.createClass({
  propTypes: {
    // Array of objects with key, title, style, children fields
    headers: React.PropTypes.array.isRequired,
    // This should be a from a state.ui object
    sortField: React.PropTypes.oneOfType([
      React.PropTypes.string,
      React.PropTypes.array,
    ]).isRequired,
    // This should be a from a state.ui object
    sortDesc: React.PropTypes.bool.isRequired,
    onSort: React.PropTypes.func.isRequired,
    id: React.PropTypes.string,
    children: React.PropTypes.node,
  },

  sort(field) {
    this.props.onSort({
      sortField: field,
      sortDesc: !this.props.sortDesc,
    });
  },

  preventSelection(e) {
    e.preventDefault();
  },

  render() {
    const columnHeaders = this.props.headers.map((header) => {
      const key = Array.isArray(header.field) ? header.field.join('-') : header.field;
      if (header.node) {
        return <th key={key} style={ header.style }>{ header.node }</th>;
      }

      var sortGlyph = '';
      if (_.isEqual(this.props.sortField, header.field)) {
        sortGlyph = <span>&nbsp;<Glyphicon glyph={ this.props.sortDesc ? 'sort-by-attributes-alt' : 'sort-by-attributes' }/></span>;
      }
      return (
        <th
          key={key}
          onMouseDown={this.preventSelection}
          onClick={ header.noSort ? '' : () => this.sort(header.field) }
          className={ header.class }
          style={{ ...header.style, cursor: header.noSort ? 'default' : 'pointer' }}>
            { header.title }{ sortGlyph }
        </th>
      );
    });

    return (
      <div id={ this.props.id } className="table-container">
        <Table condensed striped hover>
          <thead>
            <tr>{ columnHeaders }</tr>
          </thead>
          <tbody>
            { this.props.children }
          </tbody>
        </Table>
      </div>
    );
  },
});

export default SortTable;
