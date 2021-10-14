import * as Action from "../actionTypes";

const DEFAULT_SEARCHES = {
  equipmentList: {},
  owners: {},
  projects: {},
  rentalRequests: {},
  timeEntries: {},
  hiringResponses: {},
  aitResponses: {},
  ownersCoverage: {},
  users: {},
  roles: {},
};

export default function searchReducer(state = DEFAULT_SEARCHES, action) {
  switch (action.type) {
    case Action.UPDATE_EQUIPMENT_LIST_SEARCH:
      return { ...state, equipmentList: action.equipmentList };

    case Action.UPDATE_OWNERS_SEARCH:
      return { ...state, owners: action.owners };

    case Action.UPDATE_PROJECTS_SEARCH:
      return { ...state, projects: action.projects };

    case Action.UPDATE_RENTAL_REQUESTS_SEARCH:
      return { ...state, rentalRequests: action.rentalRequests };

    case Action.UPDATE_TIME_ENTRIES_SEARCH:
      return { ...state, timeEntries: action.timeEntries };

    case Action.UPDATE_HIRING_RESPONSES_SEARCH:
      return { ...state, hiringResponses: action.hiringResponses };

    case Action.UPDATE_OWNERS_COVERAGE_SEARCH:
      return { ...state, ownersCoverage: action.ownersCoverage };

    case Action.UPDATE_USERS_SEARCH:
      return { ...state, users: action.users };

    case Action.UPDATE_ROLES_SEARCH:
      return { ...state, roles: action.roles };

    case Action.UPDATE_AIT_SEARCH:
      return { ...state, aitResponses: action.aitResponses };
    default:
      return state;
  }
}
