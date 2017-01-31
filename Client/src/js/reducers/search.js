import * as Actions from '../actionTypes';

const DEFAULT_SEARCHES = {
  equipmentList: {},
};

export default function searchReducer(state = DEFAULT_SEARCHES, action) {
  switch(action.type) {
    case Actions.UPDATE_EQUIPMENT_LIST_SEARCH:
      return { ...state, equipmentList: action.equipmentList };
  }

  return state;
}
