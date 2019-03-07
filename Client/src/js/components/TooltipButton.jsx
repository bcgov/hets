import React from 'react';

import { Button, OverlayTrigger, Tooltip } from 'react-bootstrap';

// simplifies adding tooltips for enabled and disabled buttons
var TooltipButton = React.createClass({
  propTypes: {
    disabled: React.PropTypes.bool,
    children: React.PropTypes.node,
    enabledTooltip: React.PropTypes.node,
    disabledTooltip: React.PropTypes.node,
    onClick: React.PropTypes.func,
    className: React.PropTypes.string,
    style: React.PropTypes.object,
    bsSize: React.PropTypes.string,
    bsStyle: React.PropTypes.string,
  },

  getDefaultProps() {
    return {
      style: {},
    };
  },

  render() {
    var buttonStyle = this.props.disabled ? {...this.props.style, pointerEvents : 'none' } : this.props.style;

    var button = (
      <Button style={ buttonStyle } className={ this.props.className } bsStyle={this.props.bsStyle} bsSize={ this.props.bsSize } disabled={ this.props.disabled } onClick={ this.props.onClick }>
        { this.props.children }
      </Button>
    );

    var tooltipContent = this.props.disabled ? this.props.disabledTooltip : this.props.enabledTooltip;
    if (tooltipContent) {
      return (
        <OverlayTrigger placement="bottom" rootClose overlay={ <Tooltip id="button-tooltip">{ tooltipContent }</Tooltip> }>
          <div style={{display: 'inline-block', cursor: 'not-allowed'}}>
            { button }
          </div>
        </OverlayTrigger>
      );
    } else {
      return button;
    }
  },
});


export default TooltipButton;
