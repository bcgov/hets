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
  timeEntries: {},
  hiringResponses: {},
  roles: {},
  history: {},
  documents: {},
  blankRentalAgreements: {},
  showSessionTimeoutDialog: false,
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

    case Action.UPDATE_TIME_ENTRIES_UI:
      return { ...state, timeEntries: action.timeEntries };

    case Action.UPDATE_HIRING_RESPONSES_UI:
      return { ...state, hiringResponses: action.hiringResponses };

    case Action.UPDATE_ROLES_UI:
      return { ...state, roles: action.roles };

    case Action.UPDATE_HISTORY_UI:
      return { ...state, history: action.history };

    case Action.UPDATE_DOCUMENTS_UI:
      return { ...state, documents: action.documents };

    case Action.UPDATE_BLANK_RENTAL_AGREEMENTS_UI:
      return { ...state, blankRentalAgreements: action.blankRentalAgreements };

    // Modals

    case Action.SHOW_SESSION_TIMEOUT_DIALOG: 
      return { ...state, showSessionTimeoutDialog: true };

    case Action.CLOSE_SESSION_TIMEOUT_DIALOG: 
      return { ...state, showSessionTimeoutDialog: false };
  }

  return { ...state, ...newState };
}
