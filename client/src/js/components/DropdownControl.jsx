import PropTypes from 'prop-types';
import React from 'react';
import classNames from 'classnames';
import { Dropdown, Popover, OverlayTrigger } from 'react-bootstrap';
import _ from 'lodash';

class DropdownControl extends React.Component {
  static propTypes = {
    // This is used to update state
    id: PropTypes.string.isRequired,

    // This can be an array of strings or objects. If the latter, will use id/name by default.
    items: PropTypes.array.isRequired,

    // If present, then items is an array of objects with ids
    selectedId: PropTypes.any,

    // If present. then items is an array of strings
    title: PropTypes.string,

    // Assumes item field is 'name' unless specified here.
    fieldName: PropTypes.string,

    // Displayed when there's no selection
    placeholder: PropTypes.string,

    // If blankLine is supplied, include an "empty" line at the top;
    // If it has a string value, use that in place of blank.
    blankLine: PropTypes.any,

    className: PropTypes.string,
    disabled: PropTypes.bool,
    onSelect: PropTypes.func,
    updateState: PropTypes.func,
    staticTitle: PropTypes.bool,
    isInvalid: PropTypes.oneOfType([PropTypes.string, PropTypes.bool]), //if field is invalid show invalid styles.
  };

  constructor(props) {
    super(props);

    this.state = {
      simple: _.has(props, 'title'),

      selectedId: props.selectedId || '',
      title: this.buildTitle(props.title),
      fieldName: props.fieldName || 'name',
      open: false,
    };
  }

  componentDidMount() {
    if (!this.state.simple) {
      // Have to wait until state is ready before initializing title.
      this.setState({
        title: this.buildTitle(this.state.selectedId, this.props.items),
      });
    }
  }

  componentDidUpdate(prevProps) {
    if (!_.isEqual(this.props.items, prevProps.items)) {
      var items = this.props.items || [];
      this.setState({
        items: items,
        title: this.buildTitle(this.state.simple ? this.state.title : this.state.selectedId, items),
      });
    } else if (this.props.selectedId !== prevProps.selectedId) {
      this.setState({
        selectedId: this.props.selectedId,
        title: this.buildTitle(this.props.selectedId, this.props.items),
      });
    } else if (!_.isEqual(this.props.title, prevProps.title)) {
      this.setState({ title: this.buildTitle(this.props.title) });
    }
  }

  buildTitle = (keyEvent, items) => {
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
  };

  itemSelected = (keyEvent) => {
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
  };

  toggle = (open) => {
    this.setState({ open: open });
  };

  render() {
    var props = _.omit(
      this.props,
      'updateState',
      'onSelect',
      'items',
      'selectedId',
      'blankLine',
      'fieldName',
      'placeholder',
      'staticTitle',
      'isInvalid'
    );

    return (
      <Dropdown
        {...props}
        className={`dropdown-control ${this.props.className || ''}`}
        title={this.state.title}
        open={this.state.open}
        onToggle={this.toggle}
      >
        <Dropdown.Toggle className={classNames('btn-custom', { 'form-control is-invalid': this.props.isInvalid })}>
          {' '}
          {this.state.title}
        </Dropdown.Toggle>
        <Dropdown.Menu>
          {this.props.items.length > 0 && (
            <ul>
              {this.props.blankLine && (
                <Dropdown.Item
                  key={this.state.simple ? '' : 0}
                  eventKey={this.state.simple ? '' : 0}
                  onSelect={() => this.itemSelected(0)}
                >
                  {typeof this.props.blankLine === 'string' ? this.props.blankLine : ' '}
                </Dropdown.Item>
              )}
              {_.map(this.props.items, (item) => {
                var menuItem = (
                  //() => itemSelected(item.id) is required rather than this.itemSelected since react-bootstrap v1.6.1 always returns eventKey as a string.
                  //This breaks the _.find function to update title. Since the id's are Number. Number !== String.
                  //git issue: https://github.com/react-bootstrap/react-bootstrap/issues/3957
                  //source code: https://github.com/react-bootstrap/react-bootstrap/blob/master/src/DropdownItem.tsx refer to makeEventKey function that returns String()
                  <Dropdown.Item
                    key={this.state.simple ? item : item.id}
                    eventKey={this.state.simple ? item : item.id}
                    onSelect={() => this.itemSelected(item?.id || item)}
                  >
                    {this.state.simple ? item : item[this.state.fieldName]}
                  </Dropdown.Item>
                );
                // Check for hover items
                if (!this.state.simple && item.hoverText) {
                  return (
                    <OverlayTrigger
                      trigger="hover"
                      placement="right"
                      rootClose
                      overlay={
                        <Popover id={`popover-${item.id}`}>
                          <Popover.Title>{item[this.state.fieldName]}</Popover.Title>
                          <Popover.Content>{item.hoverText}</Popover.Content>
                        </Popover>
                      }
                    >
                      {menuItem}
                    </OverlayTrigger>
                  );
                }
                return menuItem;
              })}
            </ul>
          )}
        </Dropdown.Menu>
      </Dropdown>
    );
  }
}

export default DropdownControl;
