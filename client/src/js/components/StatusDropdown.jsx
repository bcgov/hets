import PropTypes from 'prop-types';
import React from 'react';
import { DropdownButton, Dropdown } from 'react-bootstrap';

import * as Constant from '../constants';

import TooltipButton from './TooltipButton.jsx';

class StatusDropdown extends React.Component {
  static propTypes = {
    id: PropTypes.string,
    className: PropTypes.string,
    status: PropTypes.string.isRequired,
    statuses: PropTypes.array.isRequired,
    disabled: PropTypes.bool,
    disabledTooltip: PropTypes.node,
    onSelect: PropTypes.func.isRequired,
  };

  computeVariant = () => {
    switch (this.props.status) {
      case Constant.EQUIPMENT_STATUS_CODE_APPROVED:
      case Constant.OWNER_STATUS_CODE_APPROVED:
      case Constant.PROJECT_STATUS_CODE_ACTIVE:
        return 'success';
      case Constant.EQUIPMENT_STATUS_CODE_PENDING:
      case Constant.OWNER_STATUS_CODE_PENDING:
      case Constant.PROJECT_STATUS_CODE_COMPLETED:
        return 'danger';
      default:
        return 'default';
    }
  };

  render() {
    const { id, className, status, statuses, disabled, disabledTooltip } = this.props;

    const variant = this.computeVariant();
    const title = status || '';

    if (disabled) {
      return (
        <TooltipButton disabled={disabled} disabledTooltip={disabledTooltip} className={className} variant={variant}>
          {title}
        </TooltipButton>
      );
    } else {
      return (
        <DropdownButton
          id={id}
          className={className}
          variant={variant}
          title={status || ''}
          onSelect={this.props.onSelect}
        >
          {statuses.map((item) => {
            return (
              <Dropdown.Item key={item} eventKey={item}>
                {item}
              </Dropdown.Item>
            );
          })}
        </DropdownButton>
      );
    }
  }
}

export default StatusDropdown;
