import * as Actions from '../actionTypes';

const DEFAULT_STATE = {
  requests: {
    waiting: false,
    error: {}, // ApiError
  },

  equipmentList: {},
};

export default function uiReducer(state = DEFAULT_STATE, action) {
  var newState = {};

  switch(action.type) {
    case Actions.REQUESTS_BEGIN:
      return { ...state, requests: {
        waiting: true,
        error: {},
      }};

    case Actions.REQUESTS_END:
      return { ...state, requests: { ...state.requests, ...{ waiting: false } } };

    case Actions.REQUESTS_ERROR:
      return { ...state, requests: { ...state.requests, ...{ error: action.error } } };

    case Actions.UPDATE_EQUIPMENT_LIST_UI:
      return { ...state, equipmentList: action.equipmentList };
  }

  return { ...state, ...newState };
}
