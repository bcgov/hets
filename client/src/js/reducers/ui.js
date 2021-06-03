import * as Action from "../actionTypes";

const DEFAULT_STATE = {
  requests: {
    waiting: false,
    error: null, // ApiError
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
  ownersCoverage: {},
  aitResponses: {},
  roles: {},
  history: {},
  documents: {},
  showSessionTimeoutDialog: false,
  districtEquipment: {},
  appError: null,
  showErrorDialog: false,
  activeRentalAgreementId: null,
  activeProjectId: null,
  activeRentalRequestId: null,
  activeOwnerId: null,
  activeEquipmentId: null,
};

export default function uiReducer(state = DEFAULT_STATE, action) {
  switch (action.type) {
    // Requests

    case Action.REQUESTS_BEGIN:
      return { ...state, requests: { ...state.requests, waiting: true } };

    case Action.REQUESTS_END:
      return { ...state, requests: { ...state.requests, waiting: false } };

    case Action.REQUESTS_ERROR:
      return {
        ...state,
        requests: { ...state.requests, error: action.error },
        showErrorDialog: true,
      };

    // Screens

    case Action.UPDATE_EQUIPMENT_LIST_UI:
      return { ...state, equipmentList: action.equipmentList };

    case Action.UPDATE_PHYSICAL_ATTACHMENTS_UI:
      return {
        ...state,
        equipmentPhysicalAttachments: action.equipmentPhysicalAttachments,
      };

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

    case Action.UPDATE_OWNERS_COVERAGE_UI:
      return { ...state, ownersCoverage: action.ownersCoverage };

    case Action.UPDATE_ROLES_UI:
      return { ...state, roles: action.roles };

    case Action.UPDATE_HISTORY_UI:
      return { ...state, history: action.history };

    case Action.UPDATE_DOCUMENTS_UI:
      return { ...state, documents: action.documents };

    case Action.UPDATE_DISTRICT_EQUIPMENT_UI:
      return { ...state, districtEquipment: action.districtEquipment };

    case Action.SET_ACTIVE_RENTAL_AGREEMENT_ID_UI:
      return { ...state, activeRentalAgreementId: action.rentalAgreementId };

    case Action.SET_ACTIVE_PROJECT_ID_UI:
      return { ...state, activeProjectId: action.projectId };

    case Action.SET_ACTIVE_RENTAL_REQUEST_ID_UI:
      return { ...state, activeRentalRequestId: action.rentalRequestId };

    case Action.SET_ACTIVE_OWNER_ID_UI:
      return { ...state, activeOwnerId: action.ownerId };

    case Action.SET_ACTIVE_EQUIPMENT_ID_UI:
      return { ...state, activeEquipmentId: action.equipmentId };

    case Action.UPDATE_AIT_REPORT_UI:
      return { ...state, aitResponses: action.aitResponses };

    // case Action.GENERATE_ANOTHER_RENTAL_AGREEMENT:
    //   return { ...state, activeRentalAgreementId: action.rentalAgreement.id };

    // Modals

    case Action.SHOW_SESSION_TIMEOUT_DIALOG:
      return { ...state, showSessionTimeoutDialog: true };

    case Action.CLOSE_SESSION_TIMEOUT_DIALOG:
      return { ...state, showSessionTimeoutDialog: false };

    case Action.SHOW_ERROR_DIALOG:
      return { ...state, appError: { ...action }, showErrorDialog: true };

    case Action.CLOSE_ERROR_DIALOG:
      return { ...state, showErrorDialog: false };
    default:
      return state;
  }
}

export const uiSelector = (state) => state.ui;
