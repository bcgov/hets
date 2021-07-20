import _ from "lodash";

import * as Action from "../actionTypes";

const DEFAULT_LOOKUPS = {
  // cities: {},
  districts: {},
  regions: {},
  serviceAreas: {},
  localAreas: {},
  equipmentTypes: {
    data: {},
    loaded: false,
  },
  equipment: {
    lite: {
      data: {},
      loaded: false,
    },
    agreementSummary: {
      data: {},
      loaded: false,
    },
    ts: {
      data: {},
      loaded: false,
    },
    hires: {
      data: {},
      loaded: false,
    },
  },
  districtEquipmentTypes: {
    data: {},
    loaded: false,
  },
  districtEquipmentTypesAgreementSummary: {
    data: {},
    loaded: false,
  },
  fiscalYears: {},
  permissions: {},
  rentalConditions: {
    data: [],
    loaded: false,
    loading: false,
  },
  // provincialRateTypes: [],
  overtimeRateTypes: [],
  // owners: {
  //   data: {},
  //   loading: false,
  // },
  owners: {
    lite: {
      data: {},
      loaded: false,
    },
    ts: {
      data: {},
      loaded: false,
    },
    hires: {
      data: {},
      loaded: false,
    },
  },
  roles: {},
  projects: {
    data: {},
    loaded: false,
  },
  projectsCurrentFiscal: {
    data: {},
    loaded: false,
  },
  projectsAgreementSummary: {
    data: {},
    loaded: false,
  },
  agreementSummaryLite: {
    data: {},
    loaded: false,
  },
  users: {},
  rolloverStatus: {},
  searchSummaryCounts: {},
};

export default function lookupsReducer(state = DEFAULT_LOOKUPS, action) {
  switch (action.type) {
    // Loaded once at init time, as they do not change very often, and
    // certainly not within the app.

    // XXX: Looks like this is unused
    // case Action.UPDATE_CITIES_LOOKUP:
    //   return { ...state, cities: action.cities };

    case Action.UPDATE_DISTRICTS_LOOKUP:
      return { ...state, districts: action.districts };

    case Action.UPDATE_REGIONS_LOOKUP:
      return { ...state, regions: action.regions };

    case Action.UPDATE_SERVICE_AREAS_LOOKUP:
      return { ...state, serviceAreas: action.serviceAreas };

    case Action.UPDATE_LOCAL_AREAS_LOOKUP:
      return { ...state, localAreas: action.localAreas };

    case Action.UPDATE_EQUIPMENT_TYPES_LOOKUP:
      return {
        ...state,
        equipmentTypes: { data: action.equipmentTypes, loaded: true },
      };

    case Action.UPDATE_DISTRICT_EQUIPMENT_TYPES_LOOKUP:
      return {
        ...state,
        districtEquipmentTypes: {
          data: action.districtEquipmentTypes,
          loaded: true,
        },
      };

    case Action.UPDATE_DISTRICT_EQUIPMENT_TYPES_AGREEMENT_SUMMARY_LOOKUP:
      return {
        ...state,
        districtEquipmentTypesAgreementSummary: {
          data: action.districtEquipmentTypes,
          loaded: true,
        },
      };

    case Action.UPDATE_FISCAL_YEARS_LOOKUP:
      return { ...state, fiscalYears: action.fiscalYears };

    case Action.UPDATE_PERMISSIONS_LOOKUP:
      return { ...state, permissions: action.permissions };

    case Action.UPDATE_ROLLOVER_STATUS_LOOKUP:
      return { ...state, rolloverStatus: action.status };

    // Not typical lookups, because they can change within the app, so
    // ensure they're loaded/updated as needed.
    // XXX: Looks like this is unused
    // case Action.OWNERS_LOOKUP_REQUEST:
    //   return { ...state, owners: { ...state.owners, loading: true } };

    // XXX: Looks like this is unused
    // case Action.UPDATE_OWNERS_LOOKUP:
    //   return { ...state, owners: { data: action.owners, loading: false } };

    case Action.UPDATE_OWNERS_LITE_LOOKUP:
      return {
        ...state,
        owners: {
          ...state.owners,
          lite: { data: action.owners, loaded: true },
        },
      };

    case Action.UPDATE_OWNERS_LITE_HIRES_LOOKUP:
      return {
        ...state,
        owners: {
          ...state.owners,
          hires: { data: action.owners, loaded: true },
        },
      };

    case Action.UPDATE_OWNERS_LITE_TS_LOOKUP:
      return {
        ...state,
        owners: { ...state.owners, ts: { data: action.owners, loaded: true } },
      };

    case Action.UPDATE_EQUIPMENT_LITE_LOOKUP:
      return {
        ...state,
        equipment: {
          ...state.equipment,
          lite: { data: action.equipment, loaded: true },
        },
      };

    case Action.UPDATE_EQUIPMENT_AGREEMENT_SUMMARY_LOOKUP:
      return {
        ...state,
        equipment: {
          ...state.equipment,
          agreementSummary: { data: action.equipment, loaded: true },
        },
      };

    case Action.UPDATE_EQUIPMENT_TS_LOOKUP:
      return {
        ...state,
        equipment: {
          ...state.equipment,
          ts: { data: action.equipment, loaded: true },
        },
      };

    case Action.UPDATE_EQUIPMENT_HIRES_LOOKUP:
      return {
        ...state,
        equipment: {
          ...state.equipment,
          hires: { data: action.equipment, loaded: true },
        },
      };

    case Action.UPDATE_ROLES_LOOKUP:
      return { ...state, roles: action.roles };

    case Action.UPDATE_PROJECTS_LOOKUP:
      return {
        ...state,
        projects: { ...state.projects, data: action.projects, loaded: true },
      };

    case Action.UPDATE_PROJECTS_AGREEMENT_SUMMARY_LOOKUP:
      return {
        ...state,
        projectsAgreementSummary: { data: action.projects, loaded: true },
      };

    case Action.UPDATE_PROJECTS_CURRENT_FISCAL_LOOKUP:
      return {
        ...state,
        projectsCurrentFiscal: {
          ...state.projectsCurrentFiscal,
          data: _.sortBy(action.projects, "name"),
          loaded: true,
        },
      };

    case Action.UPDATE_AGREEMENT_SUMMARY_LITE_LOOKUP:
      return {
        ...state,
        agreementSummaryLite: { data: action.agreements, loaded: true },
      };

    case Action.UPDATE_USERS_LOOKUP:
      return { ...state, users: action.users };

    case Action.UPDATE_RENTAL_CONDITIONS_LOOKUP:
      return {
        ...state,
        rentalConditions: {
          data: action.rentalConditions,
          loading: false,
          loaded: true,
        },
      };

    case Action.RENTAL_CONDITIONS_LOOKUP_REQUEST:
      return {
        ...state,
        rentalConditions: { ...state.rentalConditions, loading: true },
      };

    // XXX: Looks like this is unused
    // case Action.UPDATE_PROVINCIAL_RATE_TYPES_LOOKUP:
    //   return { ...state, provincialRateTypes: action.provincialRateTypes };

    case Action.UPDATE_OVERTIME_RATE_TYPES_LOOKUP:
      return { ...state, overtimeRateTypes: action.overtimeRateTypes };

    case Action.UPDATE_SEARCH_SUMMARY_COUNTS:
      return { ...state, searchSummaryCounts: action.searchSummaryCounts };

    default:
      return state;
  }
}
