import PropTypes from 'prop-types';
import React from 'react';
import { Button, OverlayTrigger } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import _ from 'lodash';

import Confirm from '../components/Confirm.jsx';

class DeleteButton extends React.Component {
  static propTypes = {
    onConfirm: PropTypes.func.isRequired,
    name: PropTypes.string,
    hide: PropTypes.bool,
  };

  render() {
    var props = _.omit(this.props, 'onConfirm', 'hide', 'name');

    return (
      <OverlayTrigger placement="top" trigger={['focus']} overlay={<Confirm onConfirm={this.props.onConfirm} />}>
        <Button
          title={`Delete ${this.props.name}`}
          size="sm"
          className={this.props.hide ? 'hidden' : 'btn-custom'}
          {...props}
        >
          <FontAwesomeIcon icon="trash-alt" />
        </Button>
      </OverlayTrigger>
    );
  }
}

export default DeleteButton;
