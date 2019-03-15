import React from 'react';
import classNames from 'classnames';
import { browserHistory } from 'react-router';
import { Button, Glyphicon } from 'react-bootstrap';


class ReturnButton extends React.Component {
  static propTypes = {
    id: React.PropTypes.string,
    className: React.PropTypes.string,
    title: React.PropTypes.string,
  };

  render() {
    const { id, className, title } = this.props;

    return (
      <Button
        id={id}
        className={classNames('return-button', 'btn', 'btn-default', className)}
        title={title}
        onClick={browserHistory.goBack}>
        <Glyphicon glyph="arrow-left" /> Return
      </Button>
    );
  }
}


export default ReturnButton;
