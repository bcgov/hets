import * as Action from '../actionTypes';

const DEFAULT_LOOKUPS = {
  cities: {},
  districts: {},
  regions: {},
  serviceAreas: {},
  localAreas: {},
  equipmentTypes: {},
  districtEquipmentTypes: {
    data: [],
    loading: false,
  },
  groups: {},
  permissions: {},
  rentalConditions: {},

  owners: { 
    data: [],
    loading: false,
  },
  roles: {},
  projects: {},
  users: {},
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

    case Action.DISTRICT_EQUIPMENT_TYPES_LOOKUP_REQUEST: 
      return { ...state, districtEquipmentTypes: { ...state.districtEquipmentTypes, loading: true } };

    case Action.UPDATE_DISTRICT_EQUIPMENT_TYPES_LOOKUP:
      return { ...state, districtEquipmentTypes: { data: action.districtEquipmentTypes, loading: false } };

    case Action.UPDATE_GROUPS_LOOKUP:
      return { ...state, groups: action.groups };

    case Action.UPDATE_PERMISSIONS_LOOKUP:
      return { ...state, permissions: action.permissions };

    // Not typical lookups, because they can change within the app, so
    // ensure they're loaded/updated as needed.
    case Action.OWNERS_LOOKUP_REQUEST:
      return { ...state, owners: { ...state.owners, loading: true } };

    case Action.UPDATE_OWNERS_LOOKUP:
      return { ...state, owners: { data: action.owners, loading: false } };

    case Action.UPDATE_ROLES_LOOKUP:
      return { ...state, roles: action.roles };

    case Action.UPDATE_PROJECTS_LOOKUP:
      return { ...state, projects: action.projects };

    case Action.UPDATE_USERS_LOOKUP:
      return { ...state, users: action.users };
  }

  return state;
}
