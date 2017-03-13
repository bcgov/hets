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
  projectNotes: {},
  projectAttachments: {},
  projectHistory: {},

  rentalRequests: {},
  rentalRequest: {},
  rentalRequestNotes: {},
  rentalRequestAttachments: {},
  rentalRequestHistory: {},

  roles: {},
  role: {},
  rolePermissions: {},
};

export default function modelsReducer(state = DEFAULT_MODELS, action) {
  switch(action.type) {
    // Users
    case Action.UPDATE_USERS:
      return { ...state, users: action.users };

    case Action.UPDATE_USER:
      return { ...state, user: action.user };

    case Action.ADD_USER:
      return { ...state, user: action.user };

    case Action.DELETE_USER:
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

    case Action.ADD_OWNER:
      return { ...state, owner: action.owner };

    case Action.UPDATE_OWNER:
      return { ...state, owner: action.owner };

    case Action.DELETE_OWNER:
      return { ...state, owner: action.owner };

    // Projects
    case Action.UPDATE_PROJECTS:
      return { ...state, projects: action.projects };

    case Action.ADD_PROJECT:
      return { ...state, project: action.project };

    case Action.UPDATE_PROJECT:
      return { ...state, project: action.project };

    // Rental Requests
    case Action.UPDATE_RENTAL_REQUESTS:
      return { ...state, rentalRequests: action.rentalRequests };

    case Action.ADD_RENTAL_REQUEST:
      return { ...state, rentalRequest: action.rentalRequest };

    case Action.UPDATE_RENTAL_REQUEST:
      return { ...state, rentalRequest: action.rentalRequest };

    // Roles, Permissions
    case Action.UPDATE_ROLES:
      return { ...state, roles: action.roles };

    case Action.ADD_ROLE:
      return { ...state, role: action.role };

    case Action.UPDATE_ROLE:
      return { ...state, role: action.role };

    case Action.DELETE_ROLE:
      return { ...state, role: action.role };

    case Action.UPDATE_ROLE_PERMISSIONS:
      return { ...state, rolePermissions: action.rolePermissions };
  }

  return state;
}
