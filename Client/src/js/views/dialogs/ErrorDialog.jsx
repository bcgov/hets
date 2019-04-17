import PropTypes from 'prop-types';
import React from 'react';
import { Button } from 'react-bootstrap';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

import ModalDialog from '../../components/ModalDialog.jsx';
import {closeErrorDialog} from '../../actions';

class ErrorDialog extends React.Component {
  static propTypes = {
    show: PropTypes.bool,
    title: PropTypes.string,
    apiError: PropTypes.object,
    appError: PropTypes.object,
    closeErrorDialog: PropTypes.func.isRequired,
  };

  closeDialog = () => {
    this.props.closeErrorDialog();
  };

  reloadButtonClicked = () => {
    window.location.reload();
  };

  render() {
    const {apiError, appError, show} = this.props;

    var modalTitle = 'Unexpected Error';
    var errorMessage =  'An error occured';
    var details = null;
    var shouldReload = false;
    var buttonText = 'Close';

    if (appError) {
      errorMessage = appError.message;

      if (appError.unrecoverable) {
        shouldReload = true;
        buttonText = 'Reload';
      }
    } else if (apiError) {
      // NOTE: If we get a status code of `null` we only know that the request has failed. We will
      // guess that it is a SiteMinder timeout that failed the request. What has happened is that
      // the original request will get a 302 and the browser will make a subsequent request to
      // https://logontest7.gov.bc.ca/clp-cgi/int/logon.cgi. The request will fail because of CORS
      // and the browser will erase any response data for security so we can't inspect any headers
      // or response data.
      const possibleSMError = apiError.status === 401 || apiError.status === 403 || apiError.status === null;
      shouldReload = true;

      if (possibleSMError) {
        modalTitle = 'Session expired';
        errorMessage = 'It looks like your session has expired. You will need to log in again.';
        buttonText = 'Log in';
      } else {
        modalTitle = 'Server Error';
        errorMessage = apiError.message;
        buttonText = 'Reload';

        if (apiError.json) {
          if (apiError.json.error) {
            errorMessage = <span>Error message: {errorMessage}</span>;
          }

          details = (
            <div id="api-error">
              <p>
                A HTTP {apiError.method.toUpperCase()} request to <span id="api-url-path">
                  {apiError.path}
                </span> returned a <span id="api-http-status">
                  {apiError.status}
                </span> status code.
              </p>
              <div id="api-error-response">
                <p>Response body:</p>
                <pre>
                  {JSON.stringify(apiError.json, null, 2)}
                </pre>
              </div>
            </div>
          );
        }
      }
    }

    const footer = (
      <span>
        <Button onClick={shouldReload ? this.reloadButtonClicked : this.closeDialog}>{buttonText}</Button>
      </span>
    );

    return (
      <ModalDialog
        id="error-dialog"
        show={show}
        backdrop="static"
        title={<strong>{modalTitle}</strong>}
        footer={footer}
        onClose={this.closeDialog}>
        <div id="error-message">{errorMessage}</div>
        {details}
      </ModalDialog>
    );
  }
}

function mapStateToProps(state) {
  return {
    showSessionTimeoutDialog: state.ui.showSessionTimeoutDialog,
    appError: state.ui.appError,
    apiError: state.ui.requests.error,
  };
}

function mapDispatchToProps(dispatch) {
  return bindActionCreators({
    closeErrorDialog,
  }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(ErrorDialog);
