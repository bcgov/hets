import _ from 'lodash';
import produce from 'immer';

import * as Action from '../actionTypes';
import { findAndUpdate } from '../utils/array';

const DEFAULT_MODELS = {
  users: {
    data: {},
    loading: false,
    loaded: false,
  },
  user: {},
  userDistricts: {
    data: {},
    loading: false,
  },
  currentUserDistricts: {
    data: {},
    loading: false,
  },

  favourites: {
    equipment: {},
    hiringReport: {},
    owner: {},
    ownersCoverage: {},
    project: {},
    rentalRequests: {},
    timeEntry: {},
    user: {},
    aitReport: {},
  },

  equipmentList: {
    data: {},
    loading: false,
    loaded: false,
  },
  equipment: {},
  equipmentSeniorityHistory: {},
  equipmentNotes: [],
  equipmentAttachments: {},
  equipmentHistory: {},
  equipmentRentalAgreements: {
    data: {},
  },

  owners: {
    data: {},
    loading: false,
    loaded: false,
  },
  owner: {},
  ownerEquipment: {
    data: {},
    loading: false,
    loaded: false,
  },
  ownerAttachments: {},
  ownerHistory: {},

  projects: {
    data: {},
    loading: false,
    loaded: false,
  },
  project: {},
  // projectEquipment: {
  //   data: {},
  //   loading: false,
  //   success: false,
  // },
  // projectTimeRecords: {
  //   data: {},
  //   loading: false,
  //   success: false,
  // },
  // projectNotes: {},
  // projectAttachments: {},
  // projectHistory: {},
  projectRentalAgreements: {
    data: {},
  },

  rentalRequests: {
    data: {},
    loading: false,
    loaded: false,
  },
  rentalRequest: {},
  // rentalRequestNotes: {},
  // rentalRequestAttachments: {},
  // rentalRequestHistory: {},
  // rentalRequestRotationList: {
  //   data: {},
  //   loading: false,
  //   success: false,
  // },

  rentalAgreement: {},
  rentalAgreementTimeRecords: {},
  // rentalRate: {},
  rentalCondition: {},
  rentalConditions: {},

  timeEntries: {
    data: {},
    loading: false,
    loaded: false,
  },

  hiringResponses: {
    data: {},
    loading: false,
    loaded: false,
  },

  ownersCoverage: {
    data: {},
    loading: false,
    loaded: false,
  },

  aitResponses: {
    data: {},
    loading: false,
    loaded: false,
  },

  roles: {},
  role: {},
  rolePermissions: {},

  // XXX: Looks like this is unused
  // contacts: {},
  // contact: {},

  documents: {},
  document: {},

  history: {
    equipment: {},
    owner: {},
    project: {},
    rentalRequest: {},
  },

  timeRecord: {
    data: {},
  },

  business: null,
};

export default function modelsReducer(state = DEFAULT_MODELS, action) {
  switch (action.type) {
    // Users
    case Action.USERS_REQUEST:
      return {
        ...state,
        users: { ...state.users, loading: true, loaded: false },
      };

    case Action.UPDATE_USERS:
      return {
        ...state,
        users: { data: action.users, loading: false, loaded: true },
      };

    case Action.CLEAR_USERS:
      return { ...state, users: { data: {}, loading: false, loaded: false } };

    case Action.UPDATE_USER:
      return { ...state, user: action.user };

    case Action.ADD_USER:
      return { ...state, user: action.user };

    case Action.DELETE_USER:
      return { ...state, user: action.user };

    case Action.USER_DISTRICTS:
      return {
        ...state,
        userDistricts: { data: action.userDistricts, loading: false },
      };

    case Action.CURRENT_USER_DISTRICTS:
      return {
        ...state,
        currentUserDistricts: {
          data: action.currentUserDistricts,
          loading: false,
        },
      };

    // Favourites
    case Action.UPDATE_FAVOURITES:
      return {
        ...state,
        favourites: { ...state.favourites, ...action.favourites },
      };

    case Action.ADD_FAVOURITE:
    case Action.UPDATE_FAVOURITE:
      return {
        ...state,
        favourites: {
          ...state.favourites,
          [action.favourite.type]: {
            ...state.favourites[action.favourite.type],
            [action.favourite.id]: action.favourite,
          },
        },
      };

    case Action.DELETE_FAVOURITE:
      return {
        ...state,
        favourites: {
          ...state.favourites,
          [action.favourite.type]: _.omit(state.favourites[action.favourite.type], [action.favourite.id]),
        },
      };

    // Contacts

    // XXX: Looks like this is unused
    // case Action.ADD_CONTACT:
    //   return { ...state, contact: action.contact };

    // XXX: Looks like this is unused
    // case Action.UPDATE_CONTACT:
    //   return { ...state, contact: action.contact };

    case Action.DELETE_CONTACT:
      return produce(state, (draftState) => {
        const contact = action.contact;
        const contactId = contact.id;

        const existingOwner = draftState.owner[contact.ownerId];
        if (existingOwner) {
          const updatedList = existingOwner.contacts.filter((contact) => contact.id !== contactId);
          existingOwner.contacts = updatedList;
        }

        const existingProject = draftState.project[contact.projectId];
        if (existingProject) {
          const updatedList = existingProject.contacts.filter((contact) => contact.id !== contactId);
          existingProject.contacts = updatedList;
        }
      });

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
      return {
        ...state,
        equipmentList: { ...state.equipmentList, loading: true, loaded: false },
      };

    case Action.UPDATE_EQUIPMENT_LIST:
      return {
        ...state,
        equipmentList: {
          data: action.equipmentList,
          loading: false,
          loaded: true,
        },
      };

    case Action.CLEAR_EQUIPMENT_LIST:
      return {
        ...state,
        equipmentList: { data: {}, loading: false, loaded: false },
      };

    case Action.ADD_EQUIPMENT:
    case Action.UPDATE_EQUIPMENT:
      return produce(state, (draftState) => {
        draftState.equipment[action.equipment.id] = action.equipment;
      });

    case Action.UPDATE_EQUIPMENT_NOTES:
      return { ...state, equipmentNotes: action.notes };

    case Action.UPDATE_EQUIPMENT_RENTAL_AGREEMENTS:
      return {
        ...state,
        equipmentRentalAgreements: { data: action.rentalAgreements },
      };

    // Owners
    case Action.OWNERS_REQUEST:
      return {
        ...state,
        owners: { ...state.owners, loading: true, loaded: false },
      };

    case Action.UPDATE_OWNERS:
      return {
        ...state,
        owners: { data: action.owners, loading: false, loaded: true },
      };

    case Action.OWNER_EQUIPMENT_REQUEST:
      return {
        ...state,
        ownerEquipment: {
          data: action.equipment,
          loading: true,
          loaded: false,
        },
      };

    case Action.UPDATE_OWNER_EQUIPMENT:
      return {
        ...state,
        ownerEquipment: {
          data: action.equipment,
          loading: false,
          loaded: true,
        },
      };

    case Action.CLEAR_OWNERS:
      return { ...state, owners: { data: {}, loading: false, loaded: false } };

    // XXX: Looks like `Action.DELETE_OWNER` is unused
    case Action.ADD_OWNER:
    case Action.UPDATE_OWNER: /*  case Action.DELETE_OWNER: */ {
      const ownerId = action.owner.id;
      const owner = { notes: [], ...state.owner[ownerId], ...action.owner };
      return { ...state, owner: { ...state.owner, [ownerId]: owner } };
    }

    case Action.UPDATE_OWNER_NOTES:
      return produce(state, (draftState) => {
        const existingOwner = draftState.owner[action.ownerId] || {};
        existingOwner.notes = action.notes;
        draftState.owner[action.ownerId] = existingOwner;
      });

    case Action.ADD_OWNER_NOTE:
      return produce(state, (draftState) => {
        const existingOwner = draftState.owner[action.ownerId];
        const existingNotes = existingOwner.notes || [];
        existingNotes.push(action.note);
        existingOwner.notes = existingNotes;
      });

    case Action.ADD_OWNER_CONTACT:
      return produce(state, (draftState) => {
        const existingOwner = draftState.owner[action.ownerId] || {};
        const updatedContacts = existingOwner.contacts || [];
        updatedContacts.push(action.contact);

        existingOwner.contacts = updatedContacts;
      });

    case Action.UPDATE_OWNER_CONTACT:
      return produce(state, (draftState) => {
        const existingOwner = draftState.owner[action.ownerId] || {};
        const updatedContacts = existingOwner.contacts || [];

        if (action.contact.isPrimary) {
          const previousPrimaryContact = _.find(updatedContacts, {
            isPrimary: true,
          });
          if (previousPrimaryContact) {
            previousPrimaryContact.isPrimary = false;
          }
        }

        const pos = _.findIndex(updatedContacts, (contact) => contact.id === action.contact.id);
        updatedContacts[pos] = action.contact;

        existingOwner.contacts = updatedContacts;
      });

    // Projects
    case Action.PROJECTS_REQUEST:
      return {
        ...state,
        projects: { ...state.projects, loading: true, loaded: false },
      };

    case Action.UPDATE_PROJECTS:
      return {
        ...state,
        projects: { data: action.projects, loading: false, loaded: true },
      };

    case Action.CLEAR_PROJECTS:
      return {
        ...state,
        projects: { data: {}, loading: false, loaded: false },
      };

    case Action.ADD_PROJECT:
    case Action.UPDATE_PROJECT: {
      const isProjectUndefined = action.project;
      let projectId = undefined;
      if(isProjectUndefined !== undefined){
        projectId = action.project.id;
      }
      else{
        projectId = action.updatedProject.id;
      }
      // const projectId = action.project !== undefined? action.project.id: action.updatedProject.id;
      const project = {
        notes: [],
        ...state.project[projectId],
        ...action.project,
      };

      return { ...state, project: { ...state.project, [projectId]: project } };
    }

    // case Action.UPDATE_PROJECT_EQUIPMENT:
    //   return { ...state, projectEquipment: { data: action.projectEquipment, loading: false, success: true } };

    // XXX: Looks like this is unused
    // case Action.UPDATE_PROJECT_TIME_RECORDS:
    //   return { ...state, projectTimeRecords: { data: action.projectTimeRecords, loading: false, success: true } };

    case Action.ADD_PROJECT_NOTE: {
      const existingProject = { ...(state.project[action.projectId] || {}) };
      const notes = (existingProject.notes || []).slice();
      notes.push(action.note);

      return {
        ...state,
        project: { ...state.project, [action.projectId]: existingProject },
      };
    }

    case Action.UPDATE_PROJECT_NOTES: {
      const existingProject = { ...(state.project[action.projectId] || {}) };
      existingProject.notes = action.notes;

      return {
        ...state,
        project: { ...state.project, [action.projectId]: existingProject },
      };
    }

    case Action.UPDATE_PROJECT_RENTAL_AGREEMENTS:
      return {
        ...state,
        projectRentalAgreements: { data: action.rentalAgreements },
      };

    case Action.DELETE_PROJECT_RENTAL_REQUEST: {
      const existingProject = { ...(state.project[action.projectId] || {}) };
      const updatedList = existingProject.rentalRequests.filter(
        (rentalRequest) => rentalRequest.id !== action.requestId
      );
      existingProject.rentalRequests = updatedList;
      return {
        ...state,
        project: { ...state.project, [action.projectId]: existingProject },
      };
    }

    case Action.ADD_PROJECT_CONTACT:
      return produce(state, (draftState) => {
        const existingProject = draftState.project[action.projectId] || {};
        const updatedContacts = existingProject.contacts || [];
        updatedContacts.push(action.contact);

        existingProject.contacts = updatedContacts;
      });

    case Action.UPDATE_PROJECT_CONTACT:
      return produce(state, (draftState) => {
        const existingProject = draftState.project[action.projectId] || {};
        const updatedContacts = existingProject.contacts || [];

        if (action.contact.isPrimary) {
          const previousPrimaryContact = _.find(updatedContacts, {
            isPrimary: true,
          });
          if (previousPrimaryContact) {
            previousPrimaryContact.isPrimary = false;
          }
        }

        const pos = _.findIndex(updatedContacts, (contact) => contact.id === action.contact.id);
        updatedContacts[pos] = action.contact;

        existingProject.contacts = updatedContacts;
      });

    // Rental Requests
    case Action.RENTAL_REQUESTS_REQUEST:
      return {
        ...state,
        rentalRequests: {
          ...state.rentalRequests,
          loading: true,
          loaded: false,
        },
      };

    case Action.UPDATE_RENTAL_REQUESTS:
      return {
        ...state,
        rentalRequests: {
          data: action.rentalRequests,
          loading: false,
          loaded: true,
        },
      };

    case Action.CLEAR_RENTAL_REQUESTS:
      return {
        ...state,
        rentalRequests: { data: {}, loading: false, loaded: false },
      };

    // Rental Request
    case Action.ADD_RENTAL_REQUEST:
    case Action.UPDATE_RENTAL_REQUEST:
      return produce(state, (draftState) => {
        const rentalRequests = draftState.rentalRequest;
        const rentalRequestId = action.rentalRequestId;
        rentalRequests[rentalRequestId] = {
          notes: [],
          rotationList: [],
          ...rentalRequests[rentalRequestId],
          ...action.rentalRequest,
        };
      });

    case Action.UPDATE_RENTAL_REQUEST_NOTES:
      return produce(state, (draftState) => {
        const rentalRequests = draftState.rentalRequest;
        const rentalRequestId = action.rentalRequestId;
        rentalRequests[rentalRequestId] = {
          ...rentalRequests[rentalRequestId],
          notes: action.notes,
        };
      });

    case Action.UPDATE_RENTAL_REQUEST_ROTATION_LIST:
      return produce(state, (draftState) => {
        const rentalRequests = draftState.rentalRequest;
        const rentalRequestId = action.rentalRequestId;
        rentalRequests[rentalRequestId] = {
          ...rentalRequests[rentalRequestId],
          rotationList: action.rotationList,
        };
      });

    // Time Entries
    case Action.TIME_ENTRIES_REQUEST:
      return { ...state, timeEntries: { ...state.timeEntries, loading: true } };

    case Action.UPDATE_TIME_ENTRIES:
      return {
        ...state,
        timeEntries: { data: action.timeEntries, loading: false, loaded: true },
      };

    case Action.CLEAR_TIME_ENTRIES:
      return {
        ...state,
        timeEntries: { data: {}, loading: false, loaded: false },
      };

    // Hiring Responses
    case Action.HIRING_RESPONSES_REQUEST:
      return {
        ...state,
        hiringResponses: {
          ...state.hiringResponses,
          loading: true,
          loaded: false,
        },
      };

    case Action.UPDATE_HIRING_RESPONSES:
      return {
        ...state,
        hiringResponses: {
          data: action.hiringResponses,
          loading: false,
          loaded: true,
        },
      };

    case Action.CLEAR_HIRING_RESPONSES:
      return {
        ...state,
        hiringResponses: { data: {}, loading: false, loaded: false },
      };

    // Owners' Coverage
    case Action.OWNERS_COVERAGE_REQUEST:
      return {
        ...state,
        ownersCoverage: {
          ...state.ownersCoverage,
          loading: true,
          loaded: false,
        },
      };

    case Action.UPDATE_OWNERS_COVERAGE:
      return {
        ...state,
        ownersCoverage: {
          data: action.ownersCoverage,
          loading: false,
          loaded: true,
        },
      };

    case Action.CLEAR_OWNERS_COVERAGE:
      return {
        ...state,
        ownersCoverage: { data: {}, loading: false, loaded: false },
      };

    // Rental Agreements
    // XXX: Looks like this is unused
    // case Action.ADD_RENTAL_AGREEMENT:
    //   return { ...state, rentalAgreement: { ...state.rentalAgreement, [action.rentalAgreement.id]: action.rentalAgreement } };

    // case Action.GENERATE_ANOTHER_RENTAL_AGREEMENT:
    //   return { ...state, rentalAgreement: { ...state.rentalAgreement, [action.rentalAgreement.id]: action.rentalAgreement } };

    case Action.UPDATE_RENTAL_AGREEMENT:
      return produce(state, (draftState) => {
        draftState.rentalAgreement[action.rentalAgreement.id] = action.rentalAgreement;
      });

    case Action.RENTAL_AGREEMENT_TIME_RECORDS:
      return {
        ...state,
        rentalAgreementTimeRecords: action.rentalAgreementTimeRecords,
      };

    // Rental Rates, Conditions
    case Action.ADD_RENTAL_RATES:
      return produce(state, (draftState) => {
        const rentalAgreementRates = draftState.rentalAgreement[action.rentalAgreementId].rentalAgreementRates;
        rentalAgreementRates.push(...action.rentalRates);
      });

    case Action.UPDATE_RENTAL_RATES:
      return produce(state, (draftState) => {
        const rentalAgreementRates = draftState.rentalAgreement[action.rentalAgreementId].rentalAgreementRates;
        action.rentalRates.forEach((rentalRate) => {
          findAndUpdate(rentalAgreementRates, rentalRate, 'id');
        });
      });

    case Action.DELETE_RENTAL_RATE:
      return produce(state, (draftState) => {
        const rentalAgreementRates = draftState.rentalAgreement[action.rentalAgreementId].rentalAgreementRates;
        _.remove(rentalAgreementRates, { id: action.rentalRate.id });
      });

    case Action.ADD_RENTAL_CONDITIONS:
      return produce(state, (draftState) => {
        const rentalAgreementConditions =
          draftState.rentalAgreement[action.rentalAgreementId].rentalAgreementConditions;
        rentalAgreementConditions.push(...action.rentalConditions);
      });

    case Action.UPDATE_RENTAL_CONDITIONS:
      return produce(state, (draftState) => {
        const rentalAgreementConditions =
          draftState.rentalAgreement[action.rentalAgreementId].rentalAgreementConditions;
        action.rentalConditions.forEach((rentalCondition) => {
          findAndUpdate(rentalAgreementConditions, rentalCondition, 'id');
        });
      });

    case Action.DELETE_RENTAL_CONDITION:
      return produce(state, (draftState) => {
        const rentalAgreementConditions =
          draftState.rentalAgreement[action.rentalAgreementId].rentalAgreementConditions;
        _.remove(rentalAgreementConditions, { id: action.rentalCondition.id });
      });

    // AIT Report
    case Action.AIT_REPORT_REQUEST:
      return {
        ...state,
        aitResponses: { ...state.aitResponses, loading: true, loaded: false },
      };

    case Action.UPDATE_AIT_REPORT:
      return {
        ...state,
        aitResponses: {
          data: action.aitResponses,
          loading: false,
          loaded: true,
        },
      };

    case Action.CLEAR_AIT_REPORT:
      return {
        ...state,
        aitResponses: { data: {}, loading: false, loaded: false },
      };

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
    case Action.UPDATE_EQUIPMENT_HISTORY:
      return produce(state, (draftState) => {
        draftState.history.equipment[action.id] = action.history;
      });

    case Action.UPDATE_OWNER_HISTORY:
      return produce(state, (draftState) => {
        draftState.history.owner[action.id] = action.history;
      });

    case Action.UPDATE_PROJECT_HISTORY:
      return produce(state, (draftState) => {
        draftState.history.project[action.id] = action.history;
      });

    case Action.UPDATE_RENTAL_REQUEST_HISTORY:
      return produce(state, (draftState) => {
        draftState.history.rentalRequest[action.id] = action.history;
      });

    // Notes
    case Action.DELETE_NOTE: {
      let notesCollectionName = null;

      if (action.noteId in state.equipmentNotes) {
        notesCollectionName = 'equipmentNotes';
      }

      if (notesCollectionName) {
        const notes = state[notesCollectionName].slice();
        _.remove(notes, { id: action.noteId });

        return { ...state, [notesCollectionName]: notes };
      }

      return state;
    }

    // Time Record
    case Action.DELETE_TIME_RECORD:
      return { ...state, timeRecord: { data: action.timeRecord } };

    // Businesses
    case Action.UPDATE_BUSINESS:
      return { ...state, business: action.business };
    default:
      return state;
  }
}

export const modelsSelector = (state) => state.models;
