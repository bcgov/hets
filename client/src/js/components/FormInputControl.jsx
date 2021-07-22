import PropTypes from 'prop-types';
import React from 'react';
import { FormControl } from 'react-bootstrap';
import _ from 'lodash';

class FormInputControl extends React.Component {
  static propTypes = {
    type: PropTypes.string,
    updateState: PropTypes.func,
    autoFocus: PropTypes.bool,
    autoComplete: PropTypes.string,
    onChange: PropTypes.func,
    children: PropTypes.node,
  };

  componentDidMount() {
    //timeout needed to set focus. Otherwise the focus is called before input is rendered.
    if (this.props.autoFocus) {
      setTimeout(() => {
        this.input.focus();
      }, 200);
    }
  }

  changed = (e) => {
    const { type, updateState, onChange } = this.props;

    // On change listener
    if (onChange) {
      onChange(e);
    }

    if (updateState && e.target.id) {
      // Use e.target.id insted of this.props.id because it comes from the controlId.
      var value = e.target.value;
      if (type === 'number') {
        value = parseInt(value, 10);
        if (_.isNaN(value)) {
          value = '';
        }
      }
      if (type === 'float') {
        value = parseFloat(value);
        if (_.isNaN(value)) {
          value = '';
        }
      }

      // Update state
      updateState({ [e.target.id]: value });
    }
  };

  refChanged = (ref) => {
    this.input = ref;
    if (this.props.ref) {
      this.props.ref(ref);
    }
  };

  render() {
    const { type, autoComplete, children } = this.props;
    // XXX: eslint doesn't like `const { type, autoComplete, children, ...rest } = this.props;`
    // using lodash omit for now
    const rest = _.omit(this.props, 'type', 'autoComplete', 'children', 'updateState');

    return (
      <FormControl
        {...rest}
        type={type === 'float' ? 'number' : type}
        step={type === 'float' ? '0.01' : null}
        onChange={this.changed}
        ref={this.refChanged}
        autoComplete={autoComplete || 'off'}
        autoFocus={this.props.autoFocus}
      >
        {children}
      </FormControl>
    );
  }
}

export default FormInputControl;
