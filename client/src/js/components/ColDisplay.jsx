import PropTypes from 'prop-types';
import React from 'react';
import { Row, Col } from 'react-bootstrap';
import _ from 'lodash';

const ColDisplay = ({ label, children, labelProps, fieldProps, ...props }) => {
  const filteredProps = _.omit(props, 'label', 'labelProps', 'fieldProps');

  return (
    <Row {...filteredProps}>
      <Col {...labelProps} style={{ paddingRight: 15, wordWrap: 'break-word' }}>
        <strong>{label}</strong>
      </Col>
      <Col {...fieldProps} style={{ paddingRight: 15, wordWrap: 'break-word' }}>
        {children}
      </Col>
    </Row>
  );
};

ColDisplay.propTypes = {
  label: PropTypes.node,
  children: PropTypes.node,
  labelProps: PropTypes.object,
  fieldProps: PropTypes.object,
};

export default ColDisplay;
