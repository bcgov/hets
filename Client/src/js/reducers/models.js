import * as Action from '../actionTypes';

import _ from 'lodash';

const DEFAULT_MODELS = {
  users: {},
  user: {},

  favourites: {},

  equipmentList: {},
  equipment: {},
  equipmentPhysicalAttachments: {},
  equipmentSeniorityHistory: {},
  equipmentNotes: {},
  equipmentAttachments: {},
  equipmentHistory: {},

  owners: {},
  owner: {},
  ownerNotes: {},
  ownerAttachments: {},
  ownerHistory: {},

  projects: {},
  project: {},
};

export default function modelsReducer(state = DEFAULT_MODELS, action) {
  switch(action.type) {
    // Users
    case Action.UPDATE_USERS:
      return { ...state, users: action.users };

    case Action.UPDATE_USER:
      return { ...state, user: action.user };

    // Favourites
    case Action.UPDATE_FAVOURITES:
      return { ...state, favourites: action.favourites };

    case Action.ADD_FAVOURITE:
      return { ...state, favourites: { ...state.favourites, ...action.favourite } };

    case Action.UPDATE_FAVOURITE:
      return { ...state, favourites: { ...state.favourites, ...action.favourite } };

    case Action.DELETE_FAVOURITE:
      return { ...state, favourites: _.omit(state.favourites, [ action.id ]) };

    // Equipment
    case Action.UPDATE_EQUIPMENT_LIST:
      return { ...state, equipmentList: action.equipmentList };

    case Action.UPDATE_EQUIPMENT:
      return { ...state, equipment: action.equipment };

    // Owners
    case Action.UPDATE_OWNERS:
      return { ...state, owners: action.owners };

    case Action.UPDATE_OWNER:
      return { ...state, owner: action.owner };

    // Projects
    case Action.UPDATE_PROJECTS:
      return { ...state, projects: action.projects };

    case Action.UPDATE_PROJECT:
      return { ...state, project: action.project };
  }

  return state;
}
