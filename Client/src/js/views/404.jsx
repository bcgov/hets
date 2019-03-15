import React from 'react';

class Main extends React.Component {
  static propTypes = {
    location: React.PropTypes.object,
  };

  constructor(props) {
    super(props);

    this.state = {
      path: props.location.pathname,
    };
  }

  render() {
    return <div id="not-found-screen">
      <h1>Not found <span>:(</span></h1>

      <p>Sorry, but the page you were trying to view ({this.state.path}) does not exist. You can try going to the <a href="#" onClick={this.gotoHome}>home</a> page.</p>
    </div>;
  }
}

export default Main;
