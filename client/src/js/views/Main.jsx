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

import { resetSessionTimeoutTimer, keepAlive } from '../App.jsx';
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

  componentDidMount() {
    window.addEventListener('unhandledrejection', this.unhandledRejection);

    if (this.props.user.hasPermission(Constant.PERMISSION_LOGIN)) {
      this.redirectIfRolloverActive(this.props.location.pathname);
      return;
    }
  }

  componentDidUpdate(prevProps) {
    //this is the converted hashHistory change listener to check for rollOverStatus whenever user navigates. Only check if user is not BCeiD
    //by checking for Login permission.
    if (
      prevProps.location.pathname !== this.props.location.pathname &&
      this.props.user.hasPermission(Constant.PERMISSION_LOGIN)
    ) {
      this.redirectIfRolloverActive(this.props.location.pathname);
    }
  }

  // redirects regular users to rollover page if rollover in progress
  redirectIfRolloverActive(path) {
    const onBusinessPage = path.indexOf(Constant.BUSINESS_PORTAL_PATHNAME) === 0;
    const onRolloverPage = path === Constant.ROLLOVER_PATHNAME;
    if (onBusinessPage || onRolloverPage) {
      return;
    }

    const { user, dispatch } = this.props;
    if (!user.district) {
      return;
    }

    const districtId = user.district.id;
    dispatch(Api.getRolloverStatus(districtId)).then(() => {
      const status = this.props.lookups.rolloverStatus;

      if (status.rolloverActive) {
        this.props.history.push(Constant.ROLLOVER_PATHNAME);
      } else if (status.rolloverComplete) {
        // refresh fiscal years
        dispatch(Api.getFiscalYears(districtId));
      }
    });
  }

  unhandledRejection = (e) => {
    var err = e.reason;

    if (err instanceof ApiError) {
      this.props.unhandledApiError(err);
    }
  };

  onCloseSessionTimeoutDialog = async () => {
    try {
      keepAlive(); //function from App.js to keep session alive
      resetSessionTimeoutTimer();
      this.props.closeSessionTimeoutDialog();
    } catch {
      console.log('Failed to refresh the token, or the session has expired');
    }
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
        <div id="screen" className="template container" style={{ paddingTop: 10 }}>
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

const mapStateToProps = (state) => ({
  showSessionTimeoutDialog: state.ui.showSessionTimeoutDialog,
  showErrorDialog: state.ui.showErrorDialog,
  user: state.user,
  lookups: state.lookups,
});

const mapDispatchToProps = (dispatch) => ({
  dispatch,
  ...bindActionCreators({ unhandledApiError, closeSessionTimeoutDialog }, dispatch)
});

export default connect(mapStateToProps, mapDispatchToProps)(withRouter(Main));
