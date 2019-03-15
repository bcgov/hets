import React from 'react';
import classNames from 'classnames';


class PageHeader extends React.Component {
  static propTypes = {
    title: React.PropTypes.node,
    subTitle: React.PropTypes.node,
    id: React.PropTypes.string,
    className: React.PropTypes.string,
    children: React.PropTypes.node,
  };

  render() {
    const { title, subTitle,id, className, children } = this.props;

    return (
      <h1 id={id} className={classNames('clearfix', 'ui-page-header', className)}>
        {title}
        {subTitle ? ': ' : ''}{subTitle && <small>{ subTitle }</small>}
        {children}
      </h1>
    );
  }
}


export default PageHeader;
