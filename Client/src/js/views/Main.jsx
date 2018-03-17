import React from 'react';

import {connect} from 'react-redux';

import $ from 'jquery';

import TopNav from './TopNav.jsx';
import Footer from './Footer.jsx';

var Main = React.createClass({
  propTypes: {
    children: React.PropTypes.object,
    showNav: React.PropTypes.bool,
  },

  getInitialState() {
    return {
      headerHeight: 0,
    };
  },

  componentDidMount() {
    this.setState({ headerHeight: ($('#header-main').height() + 10) });
  },

  render: function() {
    return <div id ="main">
      <TopNav showNav={this.props.showNav}/>
      <div id="screen" className="template container" style={{paddingTop: this.state.headerHeight}}>
        {this.props.children}
      </div>
      <Footer/>
    </div>;
  },
});

export default connect()(Main);
