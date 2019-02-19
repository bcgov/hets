import React from 'react';
import classNames from 'classnames';
import { Button, Glyphicon } from 'react-bootstrap';


var SubHeader = React.createClass({
  propTypes: {
    title: React.PropTypes.string,
    id: React.PropTypes.string,
    className: React.PropTypes.string,
    editButtonTitle: React.PropTypes.string,
    editIcon: React.PropTypes.string,
    onEditClicked: React.PropTypes.func,
    children: React.PropTypes.node,
  },

  render() {
    const { title, id, className, editButtonTitle, editIcon, children, onEditClicked } = this.props;

    var editButton = children;
    if (onEditClicked && !children) {
      editButton = (
        <Button title={ editButtonTitle } bsSize="small" onClick={ onEditClicked }>
          <Glyphicon glyph={ editIcon } />
        </Button>
      );
    }

    return (
      <h3 id={id} className={classNames('clearfix', 'ui-subheader', className)}>
        { title }
        <span className="pull-right">
          { editButton }
        </span>
      </h3>
    );
  },
});


SubHeader.defaultProps = {
  editIcon: 'pencil',
};


export default SubHeader;
