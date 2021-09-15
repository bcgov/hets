import { SHOW_ERROR_DIALOG, CLOSE_ERROR_DIALOG, REQUESTS_ERROR, CLOSE_SESSION_TIMEOUT_DIALOG } from './actionTypes';

export function showErrorDialog(exception) {
  return {
    type: SHOW_ERROR_DIALOG,
    message: exception.message || String(exception),
  };
}

export function closeErrorDialog() {
  return {
    type: CLOSE_ERROR_DIALOG,
  };
}

export function unhandledApiError(err) {
  var errorMessage = err.message || String(err);

  return {
    type: REQUESTS_ERROR,
    error: {
      message: errorMessage,
      method: err.method,
      path: err.path,
      status: err.status,
      json: err.json,
    },
  };
}

export function closeSessionTimeoutDialog() {
  return {
    type: CLOSE_SESSION_TIMEOUT_DIALOG,
  };
}
