import PropTypes from 'prop-types';
import React from 'react';
import { Row, Col } from 'react-bootstrap';
import _ from 'lodash';


class ColDisplay extends React.Component {
  static propTypes = {
    label: PropTypes.node,
    children: PropTypes.node,
    labelProps: PropTypes.object,
    fieldProps: PropTypes.object,
  };

  render() {
    var props = _.omit(this.props, 'label', 'labelProps', 'fieldProps');

    return <Row { ...props }>
      <Col { ...this.props.labelProps } style={{ paddingRight: 15, wordWrap: 'break-word' }}>
        <strong>{ this.props.label }</strong>
      </Col>
      <Col { ...this.props.fieldProps } style={{ paddingRight: 15, wordWrap: 'break-word' }}>
        { this.props.children }
      </Col>
    </Row>;
  }
}


export default ColDisplay;
