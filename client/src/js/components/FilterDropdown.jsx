import PropTypes from 'prop-types';
import React from 'react';
import classNames from 'classnames';
import { Dropdown, FormControl, OverlayTrigger, Tooltip } from 'react-bootstrap';
import _ from 'lodash';

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
    isInvalid: PropTypes.oneOfType([PropTypes.string, PropTypes.bool]), //if field is invalid show invalid styles.
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
      return _.filter(items, (item) => {
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
        disabled={disabled}
        open={this.state.open}
        onToggle={this.toggle}
      >
        <Dropdown.Toggle className={classNames('btn-custom', { 'form-control is-invalid': this.props.isInvalid })}>
          {' '}
          {this.state.title}
        </Dropdown.Toggle>
        <Dropdown.Menu>
          <div className="well well-sm">
            <FormControl
              type="text"
              placeholder="Search"
              onChange={this.filter}
              ref={(ref) => {
                if (ref) {
                  //without a delay focus will not shift to input box when using keyboard shortcuts. I think the focus is called before input is rendered.
                  setTimeout(() => ref.focus(), 100);
                }
              }}
              onKeyDown={this.keyDown}
              autoComplete="off"
            />
          </div>
          {items.length > 0 && (
            <ul>
              {blankLine && this.state.filterTerm.length === 0 && (
                <Dropdown.Item key={0} eventKey={0} onSelect={() => this.itemSelected(0)}>
                  {typeof blankLine === 'string' ? blankLine : ' '}
                </Dropdown.Item>
              )}
              {_.map(items, (item) => {
                return (
                  //() => itemSelected(item.id) is required rather than this.itemSelected since react-bootstrap v1.6.1 always returns eventKey as a string.
                  //This breaks the _.find function to update title. Since the id's are Number. Number !== String.
                  //git issue: https://github.com/react-bootstrap/react-bootstrap/issues/3957
                  //source code: https://github.com/react-bootstrap/react-bootstrap/blob/master/src/DropdownItem.tsx refer to makeEventKey function that returns String()
                  <Dropdown.Item key={item.id} eventKey={item.id} onSelect={() => this.itemSelected(item.id)}>
                    {item[this.state.fieldName]}
                  </Dropdown.Item>
                );
              })}
            </ul>
          )}
        </Dropdown.Menu>
      </Dropdown>
    );

    if (disabled && disabledTooltip) {
      const tooltip = <Tooltip id="button-tooltip">{disabledTooltip}</Tooltip>;

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
