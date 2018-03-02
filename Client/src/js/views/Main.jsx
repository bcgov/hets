import React from 'react';

import {connect} from 'react-redux';

import TopNav from './TopNav.jsx';
import Footer from './Footer.jsx';


var Main = React.createClass({
  propTypes: {
    children: React.PropTypes.object,
    showNav: React.PropTypes.bool,
  },

  render: function() {
    return <div id ="main">
      <TopNav showNav={this.props.showNav}/>
      <div id="screen" className="template container">
        {this.props.children}
      </div>
      <Footer/>
    </div>;
  },
});

export default connect()(Main);
