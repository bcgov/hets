import * as Actions from '../actionTypes';

import _ from 'lodash';

const DEFAULT_MODELS = {
  users: {},
  user: {},

  favourites: {},

  equipmentList: {},
  equipment: {},
  physicalAttachments: {},
  equipmentAttachments: {},
  equipmentHistories: {},
  equipmentNotes: {},

  owners: {},
  owner: {},
};

export default function modelsReducer(state = DEFAULT_MODELS, action) {
  switch(action.type) {
    // Users
    case Actions.UPDATE_USERS:
      return { ...state, users: action.users };

    case Actions.UPDATE_USER:
      return { ...state, user: action.user };

    // Favourites
    case Actions.UPDATE_FAVOURITES:
      return { ...state, favourites: action.favourites };

    case Actions.ADD_FAVOURITE:
      return { ...state, favourites: { ...state.favourites, ...action.favourite } };

    case Actions.UPDATE_FAVOURITE:
      return { ...state, favourites: { ...state.favourites, ...action.favourite } };

    case Actions.DELETE_FAVOURITE:
      return { ...state, favourites: _.omit(state.favourites, [ action.id ]) };

    // Equipment
    case Actions.UPDATE_EQUIPMENT_LIST:
      return { ...state, equipmentList: action.equipmentList };

    case Actions.UPDATE_EQUIPMENT:
      return { ...state, equipment: action.equipment };

    // Owners
    case Actions.UPDATE_OWNERS:
      return { ...state, owners: action.owners };

    case Actions.UPDATE_OWNER:
      return { ...state, owner: action.owner };
  }

  return state;
}
