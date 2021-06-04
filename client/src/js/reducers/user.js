import * as Action from "../actionTypes";

const DEFAULT_USER = {
  firstName: null,
  lastName: null,
  fullName: null,
  districtName: null,
};

export default function userReducer(state = DEFAULT_USER, action) {
  switch (action.type) {
    case Action.UPDATE_CURRENT_USER:
      return { ...state, ...action.user };
    default:
      return state;
  }
}
