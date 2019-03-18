import React from 'react';
import {DropdownButton, MenuItem } from 'react-bootstrap';

import * as Constant from '../constants';

import TooltipButton from './TooltipButton.jsx';


class StatusDropdown extends React.Component {
  static propTypes = {
    id: React.PropTypes.string,
    className: React.PropTypes.string,
    status: React.PropTypes.string.isRequired,
    statuses: React.PropTypes.array.isRequired,
    disabled: React.PropTypes.bool,
    disabledTooltip: React.PropTypes.node,
    onSelect: React.PropTypes.func.isRequired,
  };

  computeBsStyle = () => {
    switch(this.props.status) {
      case Constant.EQUIPMENT_STATUS_CODE_APPROVED:
      case Constant.OWNER_STATUS_CODE_APPROVED:
        return 'success';
      case Constant.EQUIPMENT_STATUS_CODE_PENDING:
      case Constant.OWNER_STATUS_CODE_PENDING:
        return 'danger';
      default:
        return 'default';
    }
  };

  render() {
    const {
      id,
      className,
      status,
      statuses,
      disabled,
      disabledTooltip,
    } = this.props;

    const bsStyle = this.computeBsStyle();
    const title = status || '';

    if (disabled) {
      return (
        <TooltipButton
          disabled={disabled}
          disabledTooltip={disabledTooltip}
          className={className}
          bsStyle={bsStyle}>
          {title}
        </TooltipButton>
      );
    } else {
      return (
        <DropdownButton
          id={id}
          className={className}
          bsStyle={bsStyle}
          title={status || ''}
          onSelect={this.props.onSelect}>
          {statuses.map((item) => {
            return <MenuItem key={item} eventKey={item}>{item}</MenuItem>;
          })}
        </DropdownButton>
      );
    }
  }
}


export default StatusDropdown;
