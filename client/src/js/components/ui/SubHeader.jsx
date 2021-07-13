import PropTypes from 'prop-types';
import React from 'react';
import classNames from 'classnames';
import { Glyphicon } from 'react-bootstrap';

import TooltipButton from '../TooltipButton.jsx';


class SubHeader extends React.Component {
  static propTypes = {
    title: PropTypes.string,
    id: PropTypes.string,
    className: PropTypes.string,
    editButtonTitle: PropTypes.string,
    editButtonDisabled: PropTypes.bool,
    editButtonDisabledTooltip: PropTypes.node,
    editIcon: PropTypes.string,
    onEditClicked: PropTypes.func,
    children: PropTypes.node,
  };

  render() {
    const {
      title,
      id,
      className,
      editButtonTitle,
      editButtonDisabled,
      editButtonDisabledTooltip,
      editIcon,
      children,
      onEditClicked,
    } = this.props;

    var editButton = children;
    if (onEditClicked && !children) {
      editButton = (
        <TooltipButton
          title={editButtonTitle}
          disabled={editButtonDisabled}
          disabledTooltip={editButtonDisabledTooltip}
          bsSize="small"
          onClick={onEditClicked}>
          <Glyphicon glyph={editIcon} />
        </TooltipButton>
      );
    }

    return (
      <h3 id={id} className={classNames('clearfix', 'ui-subheader', className)}>
        {title}
        <span className="pull-right">
          {editButton}
        </span>
      </h3>
    );
  }
}


SubHeader.defaultProps = {
  editIcon: 'pencil',
};


export default SubHeader;
