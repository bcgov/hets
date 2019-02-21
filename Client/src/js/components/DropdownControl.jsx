import React from 'react';

import { Dropdown, MenuItem, Popover, OverlayTrigger } from 'react-bootstrap';

import _ from 'lodash';

var DropdownControl = React.createClass({
  propTypes: {
    // This is used to update state
    id: React.PropTypes.string.isRequired,

    // This can be an array of strings or objects. If the latter, will use id/name by default.
    items: React.PropTypes.array.isRequired,

    // If present, then items is an array of objects with ids
    selectedId: React.PropTypes.any,

    // If present. then items is an array of strings
    title: React.PropTypes.string,

    // Assumes item field is 'name' unless specified here.
    fieldName: React.PropTypes.string,

    // Displayed when there's no selection
    placeholder: React.PropTypes.string,

    // If blankLine is supplied, include an "empty" line at the top;
    // If it has a string value, use that in place of blank.
    blankLine: React.PropTypes.any,

    className: React.PropTypes.string,
    disabled: React.PropTypes.bool,
    onSelect: React.PropTypes.func,
    updateState: React.PropTypes.func,
    staticTitle: React.PropTypes.bool,
  },

  getInitialState() {
    return {
      simple: _.has(this.props, 'title'),

      selectedId: this.props.selectedId || '',
      title: this.buildTitle(this.props.title),
      fieldName: this.props.fieldName || 'name',
      open: false,
    };
  },

  componentDidMount() {
    if (!this.state.simple) {
      // Have to wait until state is ready before initializing title.
      this.setState({
        title: this.buildTitle(this.state.selectedId, this.props.items),
      });
    }
  },

  componentWillReceiveProps(nextProps) {
    if (!_.isEqual(nextProps.items, this.props.items)) {
      var items = nextProps.items || [];
      this.setState({
        items: items,
        title: this.buildTitle(this.state.simple ? this.state.title : this.state.selectedId, items),
      });
    } else if (nextProps.selectedId !== this.props.selectedId) {
      this.setState({
        selectedId: nextProps.selectedId,
        title: this.buildTitle(nextProps.selectedId, this.props.items),
      });
    } else if (!_.isEqual(nextProps.title, this.props.title)) {
      this.setState({ title: this.buildTitle(nextProps.title) });
    }
  },

  buildTitle(keyEvent, items) {
    if (keyEvent) {
      if (!items || this.state.simple) {
        return keyEvent;
      } else {
        var selected = _.find(items, { id: keyEvent });
        if (selected) {
          return selected[this.state.fieldName].toString();
        }
      }
    }
    return this.props.placeholder || 'Select item';
  },

  itemSelected(keyEvent) {
    this.toggle(false);

    if (!this.props.staticTitle) {
      this.setState({
        selectedId: keyEvent || '',
        title: this.buildTitle(keyEvent, this.props.items),
      });
    }

    var selected = this.state.simple ? keyEvent : _.find(this.props.items, { id: keyEvent });

    // Send selected item to change listener
    if (this.props.onSelect) {
      this.props.onSelect(selected, this.props.id);
    }

    // Update state with selected key
    if (this.props.updateState) {
      this.props.updateState({
        [this.props.id]: keyEvent,
      });
    }
  },

  toggle(open) {
    this.setState({ open: open });
  },

  render() {
    var props = _.omit(this.props, 'updateState', 'onSelect', 'items', 'selectedId', 'blankLine', 'fieldName', 'placeholder', 'staticTitle');

    return <Dropdown { ...props } className={ `dropdown-control ${this.props.className || ''}` }
      title={ this.state.title } open={ this.state.open } onToggle={ this.toggle }
    >
      <Dropdown.Toggle title={ this.state.title } />
      <Dropdown.Menu bsRole="menu">
        { this.props.items.length > 0 &&
          <ul>
            { this.props.blankLine &&
              <MenuItem key={ this.state.simple ? '' : 0 } eventKey={ this.state.simple ? '' : 0 } onSelect={ this.itemSelected }>
                { typeof this.props.blankLine === 'string' ? this.props.blankLine : ' ' }
              </MenuItem>
            }
            {
              _.map(this.props.items, item => {
                var menuItem = <MenuItem key={ this.state.simple ? item : item.id } eventKey={ this.state.simple ? item : item.id } onSelect={ this.itemSelected }>
                  { this.state.simple ? item : item[this.state.fieldName] }
                </MenuItem>;
                // Check for hover items
                if (!this.state.simple && item.hoverText) {
                  return <OverlayTrigger trigger="hover" placement="right" rootClose
                    overlay={ <Popover id={ `popover-${ item.id }` } title={ item[this.state.fieldName] }>{ item.hoverText }</Popover> }
                  >
                    { menuItem }
                  </OverlayTrigger>;
                }
                return menuItem;
              })
            }
          </ul>
        }
      </Dropdown.Menu>
    </Dropdown>;
  },
});

export default DropdownControl;
