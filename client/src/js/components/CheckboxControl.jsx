import PropTypes from 'prop-types';
import React from 'react';
import { FormCheck, FormGroup } from 'react-bootstrap';
import _ from 'lodash';

const CheckboxControl = ({ type, label, updateState, onChange, children, ...props }) => {
  const handleChange = (e) => {
    // On change listener
    if (onChange) {
      onChange(e);
    }

    // Update state
    if (updateState && e.target.id) {
      updateState({ [e.target.id]: e.target.checked });
    }
  };

  const filteredProps = _.omit(props, 'updateState');

  return (
    <FormGroup controlId={`checkboxControl-${label}`}>
      <FormCheck type="checkbox" className="checkbox-control" {...filteredProps} onChange={handleChange} />
    </FormGroup>
  );
};

CheckboxControl.propTypes = {
  type: PropTypes.string,
  label: PropTypes.oneOfType([PropTypes.string, PropTypes.object]),
  updateState: PropTypes.func,
  onChange: PropTypes.func,
  children: PropTypes.node,
};

export default CheckboxControl;
