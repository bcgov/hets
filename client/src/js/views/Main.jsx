import PropTypes from 'prop-types';
import React from 'react';
import { withRouter } from 'react-router-dom';
import { connect } from 'react-redux';
import * as Api from '../api';
import * as Constant from '../constants';
import { logout } from '../Keycloak';
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
    user: PropTypes.object,
    lookups: PropTypes.object,

    unhandledApiError: PropTypes.func,
    closeSessionTimeoutDialog: PropTypes.func,
  };

  constructor(props) {
    super(props);

    this.state = {
      headerHeight: 0,
    };
  }

  componentDidMount() {
    const height = document.getElementById('header-main').clientHeight;
    this.setState({ headerHeight: height + 10 });

    window.addEventListener('unhandledrejection', this.unhandledRejection);

    if (this.props.user.hasPermission(Constant.PERMISSION_LOGIN)) {
      this.redirectIfRolloverActive(this.props.location.pathname);
      return;
    }

    // redirect business users to business page
    if (this.props.user.hasPermission(Constant.PERMISSION_BUSINESS_LOGIN)) {
      this.props.history.push(Constant.BUSINESS_PORTAL_PATHNAME);
    }
  }

  componentDidUpdate(prevProps) {
    //this is the converted hashHistory change listener to check for rollOverStatus whenever user navigates.
    if (prevProps.location.pathname !== this.props.location.pathname) {
      this.redirectIfRolloverActive(this.props.location.pathname);
    }
  }

  // redirects regular users to rollover page if rollover in progress
  redirectIfRolloverActive(path) {
    var onBusinessPage = path.indexOf(Constant.BUSINESS_PORTAL_PATHNAME) === 0;
    var onRolloverPage = path === Constant.ROLLOVER_PATHNAME;
    if (onBusinessPage || onRolloverPage) {
      return;
    }

    var { user } = this.props;
    if (!user.district) {
      return;
    }

    const districtId = user.district.id;

    Api.getRolloverStatus(districtId).then(() => {
      const status = this.props.lookups.rolloverStatus;

      if (status.rolloverActive) {
        this.props.history.push(Constant.ROLLOVER_PATHNAME);
      } else if (status.rolloverComplete) {
        // refresh fiscal years
        Api.getFiscalYears(districtId);
      }
    });
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
    logout();
    this.props.closeSessionTimeoutDialog();
  };

  componentWillUnmount() {
    window.removeEventListener('unhandledrejection', this.unhandledRejection);
  }

  render() {
    return (
      <div id="main">
        <TopNav showNav={this.props.showNav} />
        <div id="screen" className="template container" style={{ paddingTop: this.state.headerHeight }}>
          {this.props.children}
        </div>
        <Footer />
        <ConfirmDialog
          title="Session Expiry"
          show={this.props.showSessionTimeoutDialog}
          onClose={this.onEndSession}
          onSave={this.onCloseSessionTimeoutDialog}
          closeText="End Session"
          saveText="Keep Session"
        >
          Your session will time out in <Countdown time={300} onEnd={this.onEndSession} />. Would you like to keep the
          session active or end the session?
        </ConfirmDialog>
        <ErrorDialog show={this.props.showErrorDialog} />
      </div>
    );
  }
}

function mapStateToProps(state) {
  return {
    showSessionTimeoutDialog: state.ui.showSessionTimeoutDialog,
    showErrorDialog: state.ui.showErrorDialog,
    user: state.user,
    lookups: state.lookups,
  };
}

function mapDispatchToProps(dispatch) {
  return bindActionCreators({ unhandledApiError, closeSessionTimeoutDialog }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(withRouter(Main));
