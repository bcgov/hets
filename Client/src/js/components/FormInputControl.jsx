import React from 'react';
import { FormControl } from 'react-bootstrap';
import _ from 'lodash';


var FormInputControl = React.createClass({
  propTypes: {
    type: React.PropTypes.string,
    updateState: React.PropTypes.func,
    autoFocus: React.PropTypes.bool,
    autoComplete: React.PropTypes.string,
    onChange: React.PropTypes.func,
    children: React.PropTypes.node,
  },

  componentDidMount() {
    if (this.props.autoFocus) {
      this.input.focus();
    }
  },

  changed(e) {
    const { type, updateState, onChange } = this.props;

    // On change listener
    if (onChange) { onChange(e); }

    if (updateState && e.target.id) {
      // Use e.target.id insted of this.props.id because it comes from the controlId.
      var value = e.target.value;
      if (type === 'number' ) {
        value = parseInt(value, 10);
        if (_.isNaN(value)) {
          value = '';
        }
      }
      if (type === 'float' ) {
        value = parseFloat(value);
        if (_.isNaN(value)) {
          value = '';
        }
      }

      // Update state
      updateState({ [e.target.id]: value });
    }
  },

  render() {
    const { type, autoComplete, children } = this.props;

    return (
      <FormControl
        type={type === 'float' ? 'number' : type}
        onChange={this.changed}
        inputRef={ref => {this.input = ref;}}
        autoComplete={autoComplete || 'off'}>
        {children}
      </FormControl>
    );
  },
});

export default FormInputControl;
