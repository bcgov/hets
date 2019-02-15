import { SHOW_ERROR_DIALOG, CLOSE_ERROR_DIALOG } from './actionTypes';


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
