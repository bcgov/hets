import PropTypes from 'prop-types';
import React from 'react';
import classNames from 'classnames';
import { Button, OverlayTrigger, Tooltip } from 'react-bootstrap';

// simplifies adding tooltips for enabled and disabled buttons
class TooltipButton extends React.Component {
  static propTypes = {
    disabled: PropTypes.bool,
    children: PropTypes.node,
    enabledTooltip: PropTypes.node,
    disabledTooltip: PropTypes.node,
    onClick: PropTypes.func,
    className: PropTypes.string,
    style: PropTypes.object,
    size: PropTypes.string,
    variant: PropTypes.string,
  };

  static defaultProps = {
    style: {},
  };

  render() {
    var buttonStyle = this.props.disabled ? { ...this.props.style, pointerEvents: 'none' } : this.props.style;

    var button = (
      <Button
        style={buttonStyle}
        className={classNames(this.props.className, 'btn-custom')}
        variant={this.props.variant}
        size={this.props.size}
        disabled={this.props.disabled}
        onClick={this.props.onClick}
      >
        {this.props.children}
      </Button>
    );
    var tooltipContent = this.props.disabled ? this.props.disabledTooltip : this.props.enabledTooltip;
    if (tooltipContent) {
      return (
        <OverlayTrigger placement="bottom" rootClose overlay={<Tooltip id="button-tooltip">{tooltipContent}</Tooltip>}>
          {(props) => (
            <div style={{ display: 'inline-block', cursor: 'not-allowed' }} {...props}>
              {button}
            </div>
          )}
        </OverlayTrigger>
      );
    } else {
      return button;
    }
  }
}

export default TooltipButton;
