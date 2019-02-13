import React from 'react';

export const toSearchString = (searchParams = {}) => {
  return Object.keys(searchParams).map(key =>
    `${key}=${encodeURIComponent(searchParams[key])}`
  ).join('&');
};

export const createMailtoLink = (email, headers) => {
  let link = `mailto:${email}`;
  if (headers) {
    link += `?${toSearchString(headers)}`;
  }
  return link;
};

/**
 * A react component to create and display a mailto link.
 */
var Mailto = React.createClass({
  propTypes: {
    children: React.PropTypes.node.isRequired,
    email: React.PropTypes.string.isRequired,
    headers: React.PropTypes.object,
  },

  render() {
    const { email, headers, children, ...others } = this.props;
    return <a href={ createMailtoLink(email, headers) } { ...others }>
      { children }
    </a>;
  },
});

export default Mailto;
