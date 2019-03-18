import React from 'react';
import classNames from 'classnames';
import { Glyphicon } from 'react-bootstrap';

import TooltipButton from './TooltipButton.jsx';


class PrintButton extends React.Component {
  static propTypes = {
    id: React.PropTypes.string,
    className: React.PropTypes.string,
    title: React.PropTypes.string,
    disabled: React.PropTypes.bool,
    disabledTooltip: React.PropTypes.node,
    children: React.PropTypes.node,
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
