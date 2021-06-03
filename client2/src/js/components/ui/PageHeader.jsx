import PropTypes from 'prop-types';
import React from 'react';
import classNames from 'classnames';


class PageHeader extends React.Component {
  static propTypes = {
    title: PropTypes.node,
    subTitle: PropTypes.node,
    id: PropTypes.string,
    className: PropTypes.string,
    children: PropTypes.node,
  };

  render() {
    const { title, subTitle,id, className, children } = this.props;

    return (
      <h1 id={id} className={classNames('clearfix', 'page-header', 'ui-page-header', className)}>
        {title}
        {subTitle ? ': ' : ''}{subTitle && <small>{ subTitle }</small>}
        {children}
      </h1>
    );
  }
}


export default PageHeader;
