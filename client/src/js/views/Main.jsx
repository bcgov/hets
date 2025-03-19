import React, { useEffect, useCallback } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useHistory, useLocation } from 'react-router-dom';
import PropTypes from 'prop-types';

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

const Main = ({ children, showNav }) => {
  const dispatch = useDispatch();
  const history = useHistory();
  const location = useLocation();

  const {
    showSessionTimeoutDialog,
    showErrorDialog,
    user,
    lookups,
  } = useSelector((state) => ({
    showSessionTimeoutDialog: state.ui.showSessionTimeoutDialog,
    showErrorDialog: state.ui.showErrorDialog,
    user: state.user,
    lookups: state.lookups,
  }));

  const redirectIfRolloverActive = useCallback(
    (path) => {
      const onBusinessPage = path.startsWith(Constant.BUSINESS_PORTAL_PATHNAME);
      const onRolloverPage = path === Constant.ROLLOVER_PATHNAME;
      if (onBusinessPage || onRolloverPage) return;

      if (!user.district) return;

      const districtId = user.district.id;
      dispatch(Api.getRolloverStatus(districtId)).then(() => {
        const status = lookups.rolloverStatus;

        if (status.rolloverActive) {
          history.push(Constant.ROLLOVER_PATHNAME);
        } else if (status.rolloverComplete) {
          dispatch(Api.getFiscalYears(districtId));
        }
      });
    },
    [dispatch, history, lookups.rolloverStatus, user.district]
  );

  useEffect(() => {
    const unhandledRejection = (e) => {
      const err = e.reason;
      if (err instanceof ApiError) {
        dispatch(unhandledApiError(err));
      }
    };

    window.addEventListener('unhandledrejection', unhandledRejection);

    if (user.hasPermission(Constant.PERMISSION_LOGIN)) {
      redirectIfRolloverActive(location.pathname);
    }

    return () => {
      window.removeEventListener('unhandledrejection', unhandledRejection);
    };
  }, [dispatch, location.pathname, redirectIfRolloverActive, user]);

  useEffect(() => {
    if (user.hasPermission(Constant.PERMISSION_LOGIN)) {
      redirectIfRolloverActive(location.pathname);
    }
  }, [location.pathname, redirectIfRolloverActive, user]);

  const onCloseSessionTimeoutDialog = async () => {
    try {
      keepAlive();
      resetSessionTimeoutTimer();
      dispatch(closeSessionTimeoutDialog());
    } catch {
      console.log('Failed to refresh the token, or the session has expired');
    }
  };

  const onEndSession = () => {
    logout();
    dispatch(closeSessionTimeoutDialog());
  };

  return (
    <div id="main">
      <TopNav showNav={showNav} />
      <div id="screen" className="template container" style={{ paddingTop: 10 }}>
        {children}
      </div>
      <Footer />
      <ConfirmDialog
        title="Session Expiry"
        show={showSessionTimeoutDialog}
        onClose={onEndSession}
        onSave={onCloseSessionTimeoutDialog}
        closeText="End Session"
        saveText="Keep Session"
      >
        Your session will time out in <Countdown time={300} onEnd={onEndSession} />. Would you like to keep the session
        active or end the session?
      </ConfirmDialog>
      <ErrorDialog show={showErrorDialog} />
    </div>
  );
};

Main.propTypes = {
  children: PropTypes.node,
  showNav: PropTypes.bool,
};

export default Main;
