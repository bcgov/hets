import PropTypes from 'prop-types';
import React from 'react';
import classNames from 'classnames';
import { Glyphicon } from 'react-bootstrap';

import TooltipButton from './TooltipButton.jsx';


class PrintButton extends React.Component {
  static propTypes = {
    id: PropTypes.string,
    className: PropTypes.string,
    title: PropTypes.string,
    disabled: PropTypes.bool,
    disabledTooltip: PropTypes.node,
    children: PropTypes.node,
  };

  print = () => {
    window.print();
  };

  render() {
    const { id, className, disabled, disabledTooltip, children } = this.props;

    return (
      <TooltipButton
        id={id}
        className={classNames('print-button', 'hidden-print', className)}
        onClick={this.print}
        disabled={disabled}
        disabledTooltip={disabledTooltip}>
        <Glyphicon glyph="print" title="Print" />
        <span>{children}</span>
      </TooltipButton>
    );
  }
}

PrintButton.defaultProps = {
  disabledTooltip: 'Please perform a search to print',
};


export default PrintButton;
