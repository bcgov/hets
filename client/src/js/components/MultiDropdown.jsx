import PropTypes from 'prop-types';
import React from 'react';
import { Dropdown, FormControl, FormCheck, Form } from 'react-bootstrap';
import _ from 'lodash';

const MAX_ITEMS_FOR_TITLE = 3;

class MultiDropdown extends React.Component {
  static propTypes = {
    items: PropTypes.array.isRequired,
    placeholder: PropTypes.string,
    id: PropTypes.string.isRequired,
    className: PropTypes.string,
    fieldName: PropTypes.string,
    selectedIds: PropTypes.array,
    onChange: PropTypes.func,
    updateState: PropTypes.func,
    showMaxItems: PropTypes.number,
    disabled: PropTypes.bool,
  };

  constructor(props) {
    super(props);

    var selectedIds = props.selectedIds || [];
    var items = props.items || [];
    var fieldName = props.fieldName || 'name';
    this.myRef = React.createRef();

    this.state = {
      selectedIds: selectedIds,
      title: '',
      filterTerm: '',
      maxItemsForTitle: props.showMaxItems || MAX_ITEMS_FOR_TITLE,
      allSelected: selectedIds.length === items.length && selectedIds.length > 0,
      fieldName: fieldName,
      open: false,
    };
  }

  componentDidMount() {
    // Have to wait until state is ready before initializing title.
    var title = this.buildTitle(this.props.items, this.state.selectedIds);
    this.setState({ title: title });
  }

  componentDidUpdate(prevProps) {
    if (!_.isEqual(prevProps.items, this.props.items)) {
      var items = this.props.items || [];
      this.setState({
        items: items,
        title: this.buildTitle(items, this.state.selectedIds),
      });
    }
    if (!_.isEqual(prevProps.selectedIds, this.props.selectedIds)) {
      this.setState({
        selectedIds: this.props.selectedIds,
        title: this.buildTitle(this.props.items, this.props.selectedIds),
      });
    }
  }

  buildTitle = (items, selectedIds) => {
    var num = selectedIds.length;

    if (items.length === 0 || num === 0) {
      return this.props.placeholder || 'Select items';
    } else if (num === items.length) {
      return 'All selected';
    } else if (num > this.state.maxItemsForTitle) {
      return `(${num}) selected`;
    } else {
      return _.map(
        _.pickBy(items, (item) => selectedIds.includes(item.id)),
        this.state.fieldName
      ).join(', ');
    }
  };

  itemSelected = (e) => {
    var id = parseInt(e.target.value, 10);
    var selectedIds = this.state.selectedIds.slice();

    if (e.target.checked) {
      selectedIds.push(id);
    } else {
      _.pull(selectedIds, id);
    }

    this.setState({
      selectedIds: selectedIds,
      title: this.buildTitle(this.props.items, selectedIds),
      allSelected: selectedIds.length === this.props.items.length && selectedIds.length > 0,
    });

    this.sendSelected(selectedIds);
  };

  selectAll = (e) => {
    var selectedIds = e.target.checked ? _.map(this.props.items, 'id') : [];

    this.setState({
      selectedIds: selectedIds,
      allSelected: e.target.checked,
      title: this.buildTitle(this.props.items, selectedIds),
    });

    this.sendSelected(selectedIds);
  };

  sendSelected = (selectedIds) => {
    var selected = this.props.items.filter((item) => selectedIds.includes(item.id));

    // Send selected items to change listener
    if (this.props.onChange) {
      this.props.onChange(selected, this.props.id);
    }

    // Update state with selected Ids
    if (this.props.updateState) {
      this.props.updateState({
        [this.props.id]: selectedIds,
      });
    }
  };

  toggle = (open) => {
    this.setState({ open: open }, () => {
      if (this.myRef.current) {
        //without a delay focus will not shift to input box when using keyboard shortcuts. I think the focus is called before input is rendered.
        setTimeout(() => this.myRef.current.focus(), 100);
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

  render() {
    var items = this.props.items;

    if (this.state.filterTerm.length > 0) {
      items = _.filter(items, (item) => {
        return item[this.state.fieldName].toLowerCase().indexOf(this.state.filterTerm) !== -1;
      });
    }

    return (
      <Dropdown
        className={`multi-dropdown ${this.props.className || ''}`}
        id={this.props.id}
        title={this.state.title}
        open={this.state.open}
        onToggle={this.toggle}
        disabled={this.props.disabled}
      >
        <Dropdown.Toggle className="btn-custom" title={this.state.title}>
          {' '}
          {this.state.title}{' '}
        </Dropdown.Toggle>
        <Dropdown.Menu>
          <div className="well well-sm">
            <FormControl
              type="text"
              placeholder="Search"
              onChange={this.filter}
              ref={this.myRef}
              autoComplete="off"
              onKeyDown={this.keyDown}
            />

            <Form.Group controlId={`formSelectAllCheckbox-${this.state.fieldName}`}>
              <FormCheck
                className="select-all"
                checked={this.state.allSelected}
                onChange={this.selectAll}
                label="Select All"
              />
            </Form.Group>
          </div>
          {items.length > 0 && (
            <ul>
              {_.map(items, (item) => {
                return (
                  <Form.Group key={item.id} controlId={`formCheckboxLabel${item.id}`} as="li">
                    <FormCheck
                      value={item.id}
                      checked={this.state.selectedIds.includes(item.id)}
                      onChange={this.itemSelected}
                      label={item[this.state.fieldName]}
                    />
                  </Form.Group>
                );
              })}
            </ul>
          )}
        </Dropdown.Menu>
      </Dropdown>
    );
  }
}

export default MultiDropdown;
