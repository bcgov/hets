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
  projects: {},
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

    case Action.UPDATE_OWNER_CONTACTS_UI:
      return { ...state, ownerContacts: action.ownerContacts };  

    case Action.UPDATE_OWNER_EQUIPMENT_UI:
      return { ...state, ownerEquipment: action.ownerEquipment };  

    case Action.UPDATE_PROJECTS_UI:
      return { ...state, projects: action.projects };
  }

  return { ...state, ...newState };
}
