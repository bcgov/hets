import PropTypes from 'prop-types';
import React from 'react';
import { Popover, ButtonGroup, Button, Glyphicon } from 'react-bootstrap';
import _ from 'lodash';

class Confirm extends React.Component {
  static propTypes = {
    onConfirm: PropTypes.func.isRequired,
    onCancel: PropTypes.func,
    hide: PropTypes.func,
    children: PropTypes.node,
    title: PropTypes.string,
  };

  confirmed = () => {
    this.props.onConfirm();
    this.props.hide();
  };

  canceled = () => {
    if (this.props.onCancel) {
      this.props.onCancel();
    }
    this.props.hide();
  };

  render() {
    var props = _.omit(this.props, 'onConfirm', 'onCancel', 'hide', 'children');

    return (
      <Popover id="confirm" title={this.props.title ? this.props.title : 'Are you sure?'} {...props}>
        {this.props.children}
        <div style={{ textAlign: 'center', marginTop: '6px' }}>
          <ButtonGroup>
            <Button variant="primary" onClick={this.confirmed}>
              <FontAwesomeIcon icon={['far', 'check-circle']} /> Yes
            </Button>
            <Button onClick={this.canceled}>
              <FontAwesomeIcon icon={['far', 'times-circle']} /> No
            </Button>
          </ButtonGroup>
        </div>
      </Popover>
    );
  }
}

export default Confirm;
