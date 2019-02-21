import React from 'react';

import { Well, Dropdown, FormControl, MenuItem } from 'react-bootstrap';

import RootCloseMenu from './RootCloseMenu.jsx';

import _ from 'lodash';

var FilterDropdown = React.createClass({
  propTypes: {
    id: React.PropTypes.string.isRequired,
    items: React.PropTypes.array.isRequired,
    selectedId: React.PropTypes.number,
    className: React.PropTypes.string,
    // Assumes the 'name' field unless specified here.
    fieldName: React.PropTypes.string,
    placeholder: React.PropTypes.string,
    // If blankLine is supplied, include an "empty" line at the top;
    // If it has a string value, use that in place of blank.
    blankLine: React.PropTypes.any,
    disabled: React.PropTypes.bool,
    onSelect: React.PropTypes.func,
    updateState: React.PropTypes.func,
  },

  getInitialState() {
    return {
      selectedId: this.props.selectedId || 0,
      title: '',
      filterTerm: '',
      fieldName: this.props.fieldName || 'name',
      open: false,
    };
  },

  componentDidMount() {
    // Have to wait until state is ready before initializing title.
    var title = this.buildTitle(this.props.items, this.state.selectedId);
    this.setState({ title: title });
  },

  componentDidUpdate(prevProps, prevState) {
    if (!_.isEqual(this.props.items, prevProps.items)) {
      var items = this.props.items || [];
      var id = this.props.selectedId === undefined ? prevState.selectedId : this.props.selectedId;
      this.setState({
        items: items,
        title: this.buildTitle(items, id),
      });
    } else if (this.props.selectedId !== prevProps.selectedId) {
      this.setState({
        selectedId: this.props.selectedId,
        title: this.buildTitle(prevProps.items, this.props.selectedId),
      });
    }
  },

  buildTitle(items, selectedId) {
    if (selectedId) {
      var selected = _.find(items, { id: selectedId });
      if (selected) {
        return selected[this.state.fieldName].toString();
      }
    }
    return this.props.placeholder || 'Select item';
  },

  itemSelected(selectedId) {
    this.toggle(false);

    this.setState({
      selectedId: selectedId || '',
      title: this.buildTitle(this.props.items, selectedId),
    });

    this.sendSelected(selectedId);
  },

  sendSelected(selectedId) {
    var selected = _.find(this.props.items, { id: selectedId });

    // Send selected item to change listener
    if (this.props.onSelect) {
      this.props.onSelect(selected, this.props.id);
    }

    // Update state with selected Id
    if (this.props.updateState) {
      this.props.updateState({
        [this.props.id]: selectedId,
      });
    }
  },

  toggle(open) {
    this.setState({
      open: open,
      filterTerm: '',
    }, () => {
      if (open) {
        this.input.focus();
        this.input.value = '';
      }
    });
  },

  filter(e) {
    this.setState({
      filterTerm: e.target.value.toLowerCase().trim(),
    });
  },

  render() {
    var items = this.props.items;

    if (this.state.filterTerm.length > 0) {
      items = _.filter(items, item => {
        return item[this.state.fieldName].toLowerCase().indexOf(this.state.filterTerm) !== -1;
      });
    }

    return <Dropdown className={ `filter-dropdown ${this.props.className || ''}` } id={ this.props.id } title={ this.state.title }
      disabled={ this.props.disabled } open={ this.state.open } onToggle={ this.toggle }
    >
      <Dropdown.Toggle title={ this.state.title } />
      <RootCloseMenu bsRole="menu">
        <Well bsSize="small">
          <FormControl type="text" placeholder="Search" onChange={ this.filter } inputRef={ ref => { this.input = ref; }}/>
        </Well>
        { items.length > 0 &&
          <ul>
            { this.props.blankLine && this.state.filterTerm.length === 0 &&
              <MenuItem key={ 0 } eventKey={ 0 } onSelect={ this.itemSelected }>
                { typeof this.props.blankLine === 'string' ? this.props.blankLine : ' ' }
              </MenuItem>
            }
            {
              _.map(items, item => {
                return <MenuItem key={ item.id } eventKey={ item.id } onSelect={ this.itemSelected }>
                  { item[this.state.fieldName] }
                </MenuItem>;
              })
            }
          </ul>
        }
      </RootCloseMenu>
    </Dropdown>;
  },
});

export default FilterDropdown;
