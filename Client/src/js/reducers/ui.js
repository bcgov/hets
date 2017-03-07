import * as Action from '../actionTypes';

const DEFAULT_STATE = {
  requests: {
    waiting: false,
    error: {}, // ApiError
  },

  equipmentList: {},
  equipmentPhysicalAttachments: {},
  owners: {},
  ownerContacts: {},
  ownerEquipment: {},
  users: {},
  userRoles: {},
  projects: {},
  projectContacts: {},
  rentalRequests: {},
  roles: {},
};

export default function uiReducer(state = DEFAULT_STATE, action) {
  var newState = {};

  switch(action.type) {
    // Requests

    case Action.REQUESTS_BEGIN:
      return { ...state, requests: {
        waiting: true,
        error: {},
      }};

    case Action.REQUESTS_END:
      return { ...state, requests: { ...state.requests, ...{ waiting: false } } };

    case Action.REQUESTS_ERROR:
      return { ...state, requests: { ...state.requests, ...{ error: action.error } } };

    // Screens

    case Action.UPDATE_EQUIPMENT_LIST_UI:
      return { ...state, equipmentList: action.equipmentList };

    case Action.UPDATE_PHYSICAL_ATTACHMENTS_UI:
      return { ...state, equipmentPhysicalAttachments: action.equipmentPhysicalAttachments };

    case Action.UPDATE_OWNERS_UI:
      return { ...state, owners: action.owners };

    case Action.UPDATE_USERS_UI:
      return { ...state, users: action.users };

    case Action.UPDATE_USER_ROLES_UI:
      return { ...state, userRoles: action.userRoles };

    case Action.UPDATE_OWNER_CONTACTS_UI:
      return { ...state, ownerContacts: action.ownerContacts };  

    case Action.UPDATE_OWNER_EQUIPMENT_UI:
      return { ...state, ownerEquipment: action.ownerEquipment };  

    case Action.UPDATE_PROJECTS_UI:
      return { ...state, projects: action.projects };

    case Action.UPDATE_PROJECT_CONTACTS_UI:
      return { ...state, projectContacts: action.projectContacts };  

    case Action.UPDATE_RENTAL_REQUESTS_UI:
      return { ...state, rentalRequests: action.rentalRequests };

    case Action.UPDATE_ROLES_UI:
      return { ...state, roles: action.roles };
  }

  return { ...state, ...newState };
}
