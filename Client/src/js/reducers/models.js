import * as Action from '../actionTypes';

import _ from 'lodash';

const DEFAULT_MODELS = {
  users: {},
  user: {},

  favourites: {
    data: {},
    loading: false,
    success: false,
  },

  equipmentList: {
    data: {}, 
    loading: false,
    success: false,
  },
  equipment: {},
  equipmentPhysicalAttachments: {},
  equipmentSeniorityHistory: {},
  equipmentNotes: {},
  equipmentAttachments: {},
  equipmentHistory: {},

  owners: {
    data: {},
    loading: false,
  },
  owner: {},
  ownerNotes: {},
  ownerAttachments: {},
  ownerHistory: {},

  projects: {
    data: {},
    loading: false, 
    success: false,
  },
  project: {},
  projectEquipment: {
    data: {},
    loading: false,
    success: false,
  },
  projectTimeRecords: {
    data: {},
    loading: false,
    success: false,
  },
  projectNotes: {},
  projectAttachments: {},
  projectHistory: {},

  rentalRequests: {
    data: {},
    loading: false,
    success: false,
  },
  rentalRequest: {
    data: {},
    loading: false,
    success: false,
  },
  rentalRequestNotes: {},
  rentalRequestAttachments: {},
  rentalRequestHistory: {},
  rentalRequestRotationList: {
    data: {},
    loading: false,
    success: false,
  },

  rentalAgreement: {},
  rentalAgreementNotes: {},
  rentalAgreementHistory: {},
  rentalRate: {},
  rentalCondition: {},

  roles: {},
  role: {},
  rolePermissions: {},

  contacts: {},
  contact: {},

  documents: {},
  document: {},

  history: {},

  timeRecord: {
    data: {},
  },
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
    case Action.FAVOURITES_REQUEST:
      return { ...state, favourites: { ...state.favourites, loading: true } };

    case Action.UPDATE_FAVOURITES:
      return { ...state, favourites: { data: action.favourites, loading: false, success: true } };

    case Action.ADD_FAVOURITE: case Action.UPDATE_FAVOURITE:
      return { ...state, favourites: { data: { ...state.favourites.data, ...action.favourite } } };

    case Action.DELETE_FAVOURITE:
      return { ...state, favourites: _.omit(state.favourites, [ action.id ]) };

    // Contacts
    case Action.UPDATE_CONTACTS:
      return { ...state, contacts: action.contacts };

    case Action.ADD_CONTACT:
      return { ...state, contact: action.contact };

    case Action.UPDATE_CONTACT:
      return { ...state, contact: action.contact };

    case Action.DELETE_CONTACT:
      return { ...state, contact: action.contact };

    // Documents
    case Action.UPDATE_DOCUMENTS:
      return { ...state, documents: action.documents };

    case Action.ADD_DOCUMENT:
      return { ...state, document: action.document };

    case Action.UPDATE_DOCUMENT:
      return { ...state, document: action.document };

    case Action.DELETE_DOCUMENT:
      return { ...state, document: action.document };

    // Equipment
    case Action.EQUIPMENT_LIST_REQUEST:
      return { ...state, equipmentList: { ...state.equipmentList, loading: true } };

    case Action.UPDATE_EQUIPMENT_LIST:
      return { ...state, equipmentList: { data: action.equipmentList, loading: false, success: true } };

    case Action.ADD_EQUIPMENT: case Action.UPDATE_EQUIPMENT:
      return { ...state, equipment: action.equipment };

    // Equipment Attachments
    case Action.UPDATE_EQUIPMENT_ATTACHMENTS:
      return { ...state, equipmentPhysicalAttachments: action.physicalAttachments };

    case Action.ADD_EQUIPMENT_ATTACHMENT:
      return { ...state, equipmentPhysicalAttachment: action.physicalAttachment };

    case Action.UPDATE_EQUIPMENT_ATTACHMENT:
      return { ...state, equipmentPhysicalAttachment: action.physicalAttachment };

    case Action.DELETE_EQUIPMENT_ATTACHMENT:
      return { ...state, equipmentPhysicalAttachment: action.physicalAttachment };

    // Owners
    case Action.OWNERS_REQUEST:
      return { ...state, owners: { ...state.owners, loading: true } };

    case Action.UPDATE_OWNERS:
      return { ...state, owners: { data: action.owners, loading: false } };

    case Action.ADD_OWNER: case Action.UPDATE_OWNER: case Action.DELETE_OWNER:
      return { ...state, owner: action.owner };

    // Projects
    case Action.PROJECTS_REQUEST: 
      return { ...state, projects: { ...state.projects, loading: true } };

    case Action.UPDATE_PROJECTS:
      return { ...state, projects: { data: action.projects, loading: false, success: true } };

    case Action.ADD_PROJECT: case Action.UPDATE_PROJECT:
      return { ...state, project: action.project };

    case Action.UPDATE_PROJECT_EQUIPMENT:
      return { ...state, projectEquipment: { data: action.projectEquipment, loading: false, success: true } };

    case Action.UPDATE_PROJECT_TIME_RECORDS:
      return { ...state, projectTimeRecords: { data: action.projectTimeRecords, loading: false, success: true } };

    // Rental Requests
    case Action.RENTAL_REQUESTS_REQUEST: 
      return { ...state, rentalRequests: { ...state.rentalRequests, loading: true } };

    case Action.UPDATE_RENTAL_REQUESTS:
      return { ...state, rentalRequests: { data: action.rentalRequests, loading: false, success: true } };

    case Action.RENTAL_REQUEST_REQUEST: 
      return { ...state, rentalRequest: { ...state.rentalRequest, loading: true } };

    case Action.ADD_RENTAL_REQUEST: case Action.UPDATE_RENTAL_REQUEST:
      return { ...state, rentalRequest: { data: action.rentalRequest, loading: false, success: true } };

    // Rotation List
    case Action.RENTAL_REQUEST_ROTATION_LIST_REQUEST:
      return { ...state, rentalRequestRotationList: { ...state.rentalRequestRotationList, loading: true, success: false, error: {} } };

    case Action.UPDATE_RENTAL_REQUEST_ROTATION_LIST:
      return { ...state, rentalRequestRotationList: { data: action.rentalRequestRotationList, loading: false, success: true } };

    case Action.RENTAL_REQUEST_ROTATION_LIST_ERROR: 
      return { ...state, rentalRequestRotationList: { ...state.rentalRequestRotationList, error: action.error } };

    // Rental Agreements
    case Action.ADD_RENTAL_AGREEMENT:
      return { ...state, rentalAgreement: action.rentalAgreement };

    case Action.UPDATE_RENTAL_AGREEMENT:
      return { ...state, rentalAgreement: action.rentalAgreement };

    // Rental Rates, Conditions
    case Action.ADD_RENTAL_RATE:
      return { ...state, rentalRate: action.rentalRate };

    case Action.UPDATE_RENTAL_RATE:
      return { ...state, rentalRate: action.rentalRate };

    case Action.DELETE_RENTAL_RATE:
      return { ...state, rentalRate: action.rentalRate };

    case Action.ADD_RENTAL_CONDITION:
      return { ...state, rentalCondition: action.rentalCondition };

    case Action.UPDATE_RENTAL_CONDITION:
      return { ...state, rentalCondition: action.rentalCondition };

    case Action.DELETE_RENTAL_CONDITION:
      return { ...state, rentalCondition: action.rentalCondition };

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

      // History
    case Action.UPDATE_HISTORY:
      return { ...state, history: action.history };
    
    // Time Record
    case Action.DELETE_TIME_RECORD:
      return { ...state, timeRecord: { data: action.timeRecord } };
  }

  

  return state;
}
