import React from 'react';
import { Col } from 'react-bootstrap';

import _ from 'lodash';


var ColDisplay = React.createClass({
  propTypes: {
    label: React.PropTypes.node,
    children: React.PropTypes.node,
    labelProps: React.PropTypes.object,
    fieldProps: React.PropTypes.object,
  },

  render() {
    var props = _.omit(this.props, 'label', 'labelProps', 'fieldProps');

    return <Col { ...props }>
        <Col { ...this.props.labelProps } style={{ float: 'left', textAlign: 'right'}} className="col-display-label">
          <strong>{ this.props.label }</strong>
        </Col>
        <Col { ...this.props.fieldProps } style={{ float: 'left', paddingLeft: 10 }} className="col-display-field">
          { this.props.children }
        </Col>
    </Col>;
  },
});


export default ColDisplay;
