import React from 'react';

import {connect} from 'react-redux';

import $ from 'jquery';

import store from '../store';
import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';

import TopNav from './TopNav.jsx';
import Footer from './Footer.jsx';
import ConfirmDialog from './dialogs/ConfirmDialog.jsx';
import Countdown from '../components/Countdown.jsx';

var Main = React.createClass({
  propTypes: {
    children: React.PropTypes.object,
    showNav: React.PropTypes.bool,
    showSessionTimeoutDialog: React.PropTypes.bool,
  },

  getInitialState() {
    return {
      headerHeight: 0,
    };
  },

  componentDidMount() {
    this.setState({ headerHeight: ($('#header-main').height() + 10) });
  },
  
  onCloseSessionTimeoutDialog() {
    // Temporary: endpoint to keep session active
    Api.getCurrentUser();
    store.dispatch({ type: Action.CLOSE_SESSION_TIMEOUT_DIALOG });
  },

  onEndSession() {
    window.location.href = Constant.LOGOUT;
  },

  render: function() {
    return <div id ="main">
      <TopNav showNav={this.props.showNav}/>
      <div id="screen" className="template container" style={{paddingTop: this.state.headerHeight}}>
        {this.props.children}
      </div>
      <Footer/>
      <ConfirmDialog
        title="Session Expiry"
        show={ this.props.showSessionTimeoutDialog }
        onClose={ this.onEndSession }
        onSave={ this.onCloseSessionTimeoutDialog }
        closeText="End Session"
        saveText="Keep Session"
      >
        Your session will time out in 
        <Countdown 
          time={300} 
          onEnd={ this.onEndSession }
        />. Would you like to keep the session active or end the session?
      </ConfirmDialog>
    </div>;
  },
});

function mapStateToProps(state) {
  return {
    showSessionTimeoutDialog: state.ui.showSessionTimeoutDialog,
  };
}

export default connect(mapStateToProps)(Main);
