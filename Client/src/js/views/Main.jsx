import PropTypes from 'prop-types';
import React from 'react';
import {connect} from 'react-redux';
import * as Api from '../api';
import { unhandledApiError, closeSessionTimeoutDialog } from '../actions';

import TopNav from './TopNav.jsx';
import Footer from './Footer.jsx';
import ConfirmDialog from './dialogs/ConfirmDialog.jsx';
import ErrorDialog from './dialogs/ErrorDialog.jsx';
import Countdown from '../components/Countdown.jsx';

import { resetSessionTimeoutTimer } from '../App.jsx';
import { ApiError } from '../utils/http';
import { bindActionCreators } from 'redux';


class Main extends React.Component {
  static propTypes = {
    children: PropTypes.object,
    showNav: PropTypes.bool,
    showSessionTimeoutDialog: PropTypes.bool,
    showErrorDialog: PropTypes.bool,

    unhandledApiError: PropTypes.func,
    closeSessionTimeoutDialog: PropTypes.func,
  };

  state = {
    headerHeight: 0,
  };

  componentDidMount() {
    const height = document.getElementById('header-main').clientHeight;
    this.setState({ headerHeight: height + 10 });

    window.addEventListener('unhandledrejection', this.unhandledRejection);
  }

  unhandledRejection = (e) => {
    var err = e.detail.reason;

    if (err instanceof ApiError) {
      this.props.unhandledApiError(err);
    }
  };

  onCloseSessionTimeoutDialog = () => {
    Api.keepAlive();
    resetSessionTimeoutTimer();
    this.props.closeSessionTimeoutDialog();
  };

  onEndSession = () => {
    Api.logoffUser().then(logoffUrl => {
      if (logoffUrl) {
        window.location.href = logoffUrl;
      }
    });
    this.props.closeSessionTimeoutDialog();
  };

  componentWillUnmount() {
    window.removeEventListener('unhandledrejection', this.unhandledRejection);
  }

  render() {
    return (
      <div id ="main">
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
          saveText="Keep Session">
            Your session will time out in <Countdown time={300} onEnd={ this.onEndSession }/>. Would you
            like to keep the session active or end the session?
        </ConfirmDialog>
        <ErrorDialog show={this.props.showErrorDialog}/>
      </div>
    );
  }
}

function mapStateToProps(state) {
  return {
    showSessionTimeoutDialog: state.ui.showSessionTimeoutDialog,
    showErrorDialog: state.ui.showErrorDialog,
  };
}

function mapDispatchToProps(dispatch) {
  return bindActionCreators({ unhandledApiError, closeSessionTimeoutDialog }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(Main);
