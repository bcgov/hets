import PropTypes from 'prop-types';
import React from 'react';
import classNames from 'classnames';
import { Well, Dropdown, FormControl, MenuItem, OverlayTrigger, Tooltip } from 'react-bootstrap';
import _ from 'lodash';

import RootCloseMenu from './RootCloseMenu.jsx';


class FilterDropdown extends React.Component {
  static propTypes = {
    id: PropTypes.string.isRequired,
    items: PropTypes.array.isRequired,
    selectedId: PropTypes.number,
    className: PropTypes.string,
    // Assumes the 'name' field unless specified here.
    fieldName: PropTypes.string,
    placeholder: PropTypes.string,
    // If blankLine is supplied, include an "empty" line at the top;
    // If it has a string value, use that in place of blank.
    blankLine: PropTypes.any,
    disabled: PropTypes.bool,
    disabledTooltip: PropTypes.node,
    onSelect: PropTypes.func,
    updateState: PropTypes.func,
  };

  constructor(props) {
    super(props);

    this.state = {
      selectedId: props.selectedId || 0,
      title: '',
      filterTerm: '',
      fieldName: props.fieldName || 'name',
      open: false,
    };
  }

  componentDidMount() {
    // Have to wait until state is ready before initializing title.
    var title = this.buildTitle(this.props.items, this.state.selectedId);
    this.setState({ title: title });
  }

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
  }

  buildTitle = (items, selectedId) => {
    if (selectedId) {
      var selected = _.find(items, { id: selectedId });
      if (selected) {
        return selected[this.state.fieldName].toString();
      }
    }
    return this.props.placeholder || 'Select item';
  };

  itemSelected = (selectedId) => {
    this.toggle(false);

    this.setState({
      selectedId: selectedId || '',
      title: this.buildTitle(this.props.items, selectedId),
    });

    this.sendSelected(selectedId);
  };

  sendSelected = (selectedId) => {
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
  };

  toggle = (open) => {
    this.setState({
      open: open,
      filterTerm: '',
    }, () => {
      if (open) {
        this.input.focus();
        this.input.value = '';
      }
    });
  };

  filter = (e) => {
    this.setState({
      filterTerm: e.target.value.toLowerCase().trim(),
    });
  };

  keyDown = (e) => {
    if (e.key === 'Enter') {
      e.preventDefault();
    }
  };

  filterItems = () => {
    const { items } = this.props;

    if (this.state.filterTerm.length > 0) {
      return _.filter(items, item => {
        return item[this.state.fieldName].toLowerCase().indexOf(this.state.filterTerm) !== -1;
      });
    }

    return items;
  };

  render() {
    const { id, className, disabled, blankLine, disabledTooltip } = this.props;

    const items = this.filterItems();

    const dropdown = (
      <Dropdown
        className={classNames('filter-dropdown', className)}
        id={id}
        title={disabled ? null : this.state.title}
        disabled={ disabled }
        open={ this.state.open }
        onToggle={ this.toggle }>
        <Dropdown.Toggle title={ this.state.title } />
        <RootCloseMenu bsRole="menu">
          <Well bsSize="small">
            <FormControl type="text" placeholder="Search" onChange={ this.filter } inputRef={ ref => { this.input = ref; }} onKeyDown={this.keyDown}/>
          </Well>
          { items.length > 0 && (
            <ul>
              { blankLine && this.state.filterTerm.length === 0 &&
                <MenuItem key={ 0 } eventKey={ 0 } onSelect={ this.itemSelected }>
                  { typeof blankLine === 'string' ? blankLine : ' ' }
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
          )}
        </RootCloseMenu>
      </Dropdown>
    );

    if (disabled && disabledTooltip) {
      const tooltip = <Tooltip id="button-tooltip">{ disabledTooltip }</Tooltip>;

      return (
        <OverlayTrigger placement="bottom" rootClose overlay={tooltip}>
          {dropdown}
        </OverlayTrigger>
      );
    }

    return dropdown;
  }
}

export default FilterDropdown;
