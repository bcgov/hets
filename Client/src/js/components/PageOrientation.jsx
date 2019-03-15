import React from 'react';


class PageOrientation extends React.Component {
  static propTypes = {
    type: React.PropTypes.string.isRequired,
  };

  render() {
    var size = 'portrait';
    if (this.props.type === 'landscape') {
      size = 'landscape';
    }

    return <style type="text/css">
      { `@page { size: ${ size } }` }
    </style>;
  }
}

export default PageOrientation;
