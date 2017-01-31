import * as Actions from '../actionTypes';

const DEFAULT_LOOKUPS = {
  cities: {},
  districts: {},
  regions: {},
  serviceAreas: {},
  localAreas: {},

  equipmentTypes: {},
  physicalAttachmentTypes: {},

  owners: {},  
};

export default function lookupsReducer(state = DEFAULT_LOOKUPS, action) {
  switch(action.type) {

    // Loaded once at init time, as they do not change very often, and
    // certainly not within the app.

    case Actions.UPDATE_CITIES_LOOKUP:
      return { ...state, cities: action.cities };

    case Actions.UPDATE_DISTRICTS_LOOKUP:
      return { ...state, districts: action.districts };

    case Actions.UPDATE_REGIONS_LOOKUP:
      return { ...state, regions: action.regions };

    case Actions.UPDATE_SERVICE_AREAS_LOOKUP:
      return { ...state, serviceAreas: action.serviceAreas };

    case Actions.UPDATE_LOCAL_AREAS_LOOKUP:
      return { ...state, localAreas: action.localAreas };

    case Actions.UPDATE_EQUIPMENT_TYPES_LOOKUP:
      return { ...state, equipmentTypes: action.equipmentTypes };

    case Actions.UPDATE_PHYSICAL_ATTACHMENT_TYPES_LOOKUP:
      return { ...state, physicalAttachmentTypes: action.physicalAttachmentTypes };

    // Not typical lookups, because they can change within the app, so
    // ensure they're loaded/updated as needed.

    case Actions.UPDATE_OWNERS_LOOKUP:
      return { ...state, owners: action.owners };
  }

  return state;
}
