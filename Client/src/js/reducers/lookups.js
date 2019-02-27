import * as Action from '../actionTypes';

const DEFAULT_LOOKUPS = {
  cities: {},
  districts: {},
  regions: {},
  serviceAreas: {},
  localAreas: {},
  equipmentTypes: {},
  equipmentLite: {},
  districtEquipmentTypes: {
    data: {},
    loaded: false,
  },
  fiscalYears: {},
  permissions: {},
  rentalConditions: {
    data: [],
    loading: false,
  },
  provincialRateTypes: [],
  overtimeRateTypes: [],
  owners: {
    data: {},
    loading: false,
  },
  ownersLite: {},
  roles: {},
  projects: {},
  projectsCurrentFiscal: {},
  users: {},
  blankRentalAgreements: {
    data: {},
    loading: false,
  },
  rolloverStatus: {},
  searchSummaryCounts: {},
};

export default function lookupsReducer(state = DEFAULT_LOOKUPS, action) {
  switch(action.type) {

    // Loaded once at init time, as they do not change very often, and
    // certainly not within the app.

    case Action.UPDATE_CITIES_LOOKUP:
      return { ...state, cities: action.cities };

    case Action.UPDATE_DISTRICTS_LOOKUP:
      return { ...state, districts: action.districts };

    case Action.UPDATE_REGIONS_LOOKUP:
      return { ...state, regions: action.regions };

    case Action.UPDATE_SERVICE_AREAS_LOOKUP:
      return { ...state, serviceAreas: action.serviceAreas };

    case Action.UPDATE_LOCAL_AREAS_LOOKUP:
      return { ...state, localAreas: action.localAreas };

    case Action.UPDATE_EQUIPMENT_TYPES_LOOKUP:
      return { ...state, equipmentTypes: action.equipmentTypes };

    case Action.UPDATE_DISTRICT_EQUIPMENT_TYPES_LOOKUP:
      return { ...state, districtEquipmentTypes: { data: action.districtEquipmentTypes, loaded: true } };

    case Action.UPDATE_FISCAL_YEARS_LOOKUP:
      return { ...state, fiscalYears: action.fiscalYears };

    case Action.UPDATE_PERMISSIONS_LOOKUP:
      return { ...state, permissions: action.permissions };

    case Action.UPDATE_ROLLOVER_STATUS_LOOKUP:
      return { ...state, rolloverStatus: action.status };

    // Not typical lookups, because they can change within the app, so
    // ensure they're loaded/updated as needed.
    case Action.OWNERS_LOOKUP_REQUEST:
      return { ...state, owners: { ...state.owners, loading: true } };

    case Action.UPDATE_OWNERS_LOOKUP:
      return { ...state, owners: { data: action.owners, loading: false } };

    case Action.UPDATE_OWNERS_LITE_LOOKUP:
      return { ...state, ownersLite: action.owners };

    case Action.UPDATE_EQUIPMENT_LITE_LOOKUP:
      return { ...state, equipmentLite: action.equipment };

    case Action.UPDATE_ROLES_LOOKUP:
      return { ...state, roles: action.roles };

    case Action.UPDATE_PROJECTS_LOOKUP:
      return { ...state, projects: action.projects };

    case Action.UPDATE_PROJECTS_CURRENT_FISCAL_LOOKUP:
      return { ...state, projectsCurrentFiscal: action.projects };

    case Action.UPDATE_USERS_LOOKUP:
      return { ...state, users: action.users };

    case Action.UPDATE_RENTAL_CONDITIONS_LOOKUP:
      return { ...state, rentalConditions: { data: action.rentalConditions, loading: false } };

    case Action.RENTAL_CONDITIONS_LOOKUP_REQUEST:
      return { ...state, rentalConditions: { ...state.rentalConditions, loading: true } };

    case Action.UPDATE_PROVINCIAL_RATE_TYPES_LOOKUP:
      return { ...state, provincialRateTypes: action.provincialRateTypes };

    case Action.UPDATE_OVERTIME_RATE_TYPES_LOOKUP:
      return { ...state, overtimeRateTypes: action.overtimeRateTypes };

    case Action.BLANK_RENTAL_AGREEMENTS_LOOKUP_REQUEST:
      return { ...state, blankRentalAgreements: { ...state.blankRentalAgreements, loading: true } };

    case Action.UPDATE_BLANK_RENTAL_AGREEMENTS_LOOKUP:
      return { ...state, blankRentalAgreements: { data: action.blankRentalAgreements, loading: false } };

    case Action.UPDATE_SEARCH_SUMMARY_COUNTS:
      return { ...state, searchSummaryCounts: action.searchSummaryCounts };
  }

  return state;
}
