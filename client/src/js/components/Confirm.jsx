import PropTypes from 'prop-types';
import React from 'react';
import { Popover, ButtonGroup, Button } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import _ from 'lodash';

class Confirm extends React.Component {
  static propTypes = {
    onConfirm: PropTypes.func.isRequired,
    onCancel: PropTypes.func,

    children: PropTypes.node,
    title: PropTypes.string,
  };

  confirmed = () => {
    this.props.onConfirm();
  };

  canceled = () => {
    if (this.props.onCancel) {
      this.props.onCancel();
    }
  };

  render() {
    var props = _.omit(this.props, 'onConfirm', 'onCancel', 'hide', 'children');

    return (
      <Popover id="confirm" {...props}>
        <Popover.Title>{this.props.title ? this.props.title : 'Are you sure?'}</Popover.Title>
        <Popover.Content>
          {this.props.children}
          <div style={{ textAlign: 'center', marginTop: '6px' }}>
            <ButtonGroup>
              <Button onClick={this.confirmed}>
                <FontAwesomeIcon icon={['far', 'check-circle']} /> Yes
              </Button>
              <Button className="btn-custom" onClick={this.canceled}>
                <FontAwesomeIcon icon={['far', 'times-circle']} /> No
              </Button>
            </ButtonGroup>
          </div>
        </Popover.Content>
      </Popover>
    );
  }
}

export default Confirm;
