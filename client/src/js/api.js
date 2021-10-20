import * as Action from './actionTypes';
import * as Constant from './constants';
import * as History from './history';
import * as Log from './history';
import store from './store';

import { ApiRequest } from './utils/http';
import { lastFirstName, firstLastName, concat } from './utils/string';
import { daysAgo, sortableDateTime, today } from './utils/date';

import _ from 'lodash';
import Moment from 'moment';

function normalize(response) {
  return _.fromPairs(response.map((object) => [object.id, object]));
}

////////////////////
// Users
////////////////////

function parseUser(user) {
  if (!user.district) {
    user.district = { id: 0, name: '' };
  }
  if (!user.userRoles) {
    user.userRoles = [];
  }

  user.name = lastFirstName(user.surname, user.givenName);
  user.fullName = firstLastName(user.givenName, user.surname);
  user.districtName = user.district.name;

  _.each(user.userRoles, (userRole) => {
    userRole.roleId = userRole.role && userRole.role.id ? userRole.role.id : 0;
    userRole.roleName = userRole.role && userRole.role.name ? userRole.role.name : '';
    userRole.effectiveDateSort = sortableDateTime(user.effectiveDate);
    userRole.expiryDateSort = sortableDateTime(user.expiryDate);
  });

  user.path = `${Constant.USERS_PATHNAME}/${user.id}`;
  user.url = `${user.path}`;
  user.historyEntity = History.makeHistoryEntity(Constant.HISTORY_USER, user);

  user.canEdit = true;
  user.canDelete = true;
}

export function getCurrentUser() {
  return new ApiRequest('/users/current').get().then((response) => {
    var user = response.data;

    // Add display fields
    parseUser(user);

    // Get permissions
    var permissions = [];
    _.each(user.userRoles, (userRole) => {
      _.each(userRole.role.rolePermissions, (rolePermission) => {
        permissions.push(rolePermission.permission.code);
      });
    });
    user.permissions = _.uniq(permissions);

    user.hasPermission = function (permission) {
      return user.permissions.indexOf(permission) !== -1;
    };

    store.dispatch({ type: Action.UPDATE_CURRENT_USER, user: user });
    return user;
  });
}

export function searchUsers(params) {
  store.dispatch({ type: Action.USERS_REQUEST });
  return new ApiRequest('/users/search').get(params).then((response) => {
    var users = normalize(response.data);

    // Add display fields
    _.map(users, (user) => {
      parseUser(user);
    });

    store.dispatch({ type: Action.UPDATE_USERS, users: users });
  });
}

export function getUsers() {
  return new ApiRequest('/users').get().then((response) => {
    var users = normalize(response.data);

    // Add display fields
    _.map(users, (user) => {
      parseUser(user);
    });

    store.dispatch({ type: Action.UPDATE_USERS_LOOKUP, users: users });
  });
}

export function getUser(userId) {
  return new ApiRequest(`/users/${userId}`).get().then((response) => {
    var user = response.data;

    // Add display fields
    parseUser(user);

    store.dispatch({ type: Action.UPDATE_USER, user: user });
  });
}

export function addUser(user) {
  return new ApiRequest('/users').post(user).then((response) => {
    var user = response.data;

    // Add display fields
    parseUser(user);

    store.dispatch({ type: Action.ADD_USER, user: user });

    return user;
  });
}

export function updateUser(user) {
  return new ApiRequest(`/users/${user.id}`).put(user).then((response) => {
    var user = response.data;

    // Add display fields
    parseUser(user);

    store.dispatch({ type: Action.UPDATE_USER, user: user });

    return user;
  });
}

export function deleteUser(user) {
  return new ApiRequest(`/users/${user.id}/delete`).post().then((response) => {
    var user = response.data;

    // Add display fields
    parseUser(user);

    store.dispatch({ type: Action.DELETE_USER, user: user });
  });
}

export function addUserRole(userId, userRole) {
  return new ApiRequest(`/users/${userId}/roles`).post(userRole).then(() => {
    // After updating the user's role, refresh the user state.
    return getUser(userId);
  });
}

export function updateUserRoles(userId, userRoleArray) {
  return new ApiRequest(`/users/${userId}/roles`).put(userRoleArray).then(() => {
    // After updating the user's role, refresh the user state.
    return getUser(userId);
  });
}

export function getCurrentUserDistricts() {
  return new ApiRequest('/userdistricts').get().then((response) => {
    store.dispatch({
      type: Action.CURRENT_USER_DISTRICTS,
      currentUserDistricts: response.data,
    });
    return response;
  });
}

export function getUserDistricts(userId) {
  return new ApiRequest(`/users/${userId}/districts`).get().then((response) => {
    store.dispatch({
      type: Action.USER_DISTRICTS,
      userDistricts: response.data,
    });
    return response;
  });
}

export function addUserDistrict(district) {
  return new ApiRequest(`/userdistricts/${district.id}`).put(district).then((response) => {
    store.dispatch({
      type: Action.USER_DISTRICTS,
      userDistricts: response.data,
    });
    return response;
  });
}

export function editUserDistrict(district) {
  return new ApiRequest(`/userdistricts/${district.id}`).post(district).then((response) => {
    store.dispatch({
      type: Action.USER_DISTRICTS,
      userDistricts: response.data,
    });
    return response;
  });
}

export function deleteUserDistrict(district) {
  return new ApiRequest(`/userdistricts/${district.id}/delete`).post().then((response) => {
    store.dispatch({
      type: Action.USER_DISTRICTS,
      userDistricts: response.data,
    });
    return response;
  });
}

export function switchUserDistrict(districtId) {
  return new ApiRequest(`/userdistricts/${districtId}/switch`).post();
}

export function getSearchSummaryCounts() {
  return new ApiRequest('/counts').get().then((response) => {
    store.dispatch({
      type: Action.UPDATE_SEARCH_SUMMARY_COUNTS,
      searchSummaryCounts: response.data,
    });
    return response;
  });
}

////////////////////
// Roles,  Permissions
////////////////////

function parseRole(role) {
  role.path = `${Constant.ROLES_PATHNAME}/${role.id}`;
  role.url = `${role.path}`;
  role.historyEntity = History.makeHistoryEntity(Constant.HISTORY_ROLE, role);

  role.canEdit = true;
  role.canDelete = false;
}

export function searchRoles(params) {
  return new ApiRequest('/roles').get(params).then((response) => {
    var roles = normalize(response.data);

    // Add display fields
    _.map(roles, (role) => {
      parseRole(role);
    });

    store.dispatch({ type: Action.UPDATE_ROLES, roles: roles });
  });
}

export function getRole(roleId) {
  return new ApiRequest(`/roles/${roleId}`).get().then((response) => {
    var role = response.data;

    // Add display fields
    parseRole(role);

    store.dispatch({ type: Action.UPDATE_ROLE, role: role });
  });
}

export function addRole(role) {
  return new ApiRequest('/roles').post(role).then((response) => {
    var role = response.data;

    // Add display fields
    parseRole(role);

    store.dispatch({ type: Action.ADD_ROLE, role: role });
  });
}

export function updateRole(role) {
  return new ApiRequest(`/roles/${role.id}`).put(role).then((response) => {
    var role = response.data;

    // Add display fields
    parseRole(role);

    store.dispatch({ type: Action.UPDATE_ROLE, role: role });
  });
}

export function deleteRole(role) {
  return new ApiRequest(`/roles/${role.id}/delete`).post().then((response) => {
    var role = response.data;

    // Add display fields
    parseRole(role);

    store.dispatch({ type: Action.DELETE_ROLE, role: role });
  });
}

export function getRolePermissions(roleId) {
  return new ApiRequest(`/roles/${roleId}/permissions`).get().then((response) => {
    var permissions = normalize(response.data);

    store.dispatch({
      type: Action.UPDATE_ROLE_PERMISSIONS,
      rolePermissions: permissions,
    });
  });
}

export function updateRolePermissions(roleId, permissionsArray) {
  return new ApiRequest(`/roles/${roleId}/permissions`).put(permissionsArray).then(() => {
    // After updating the role's permissions, refresh the permissions state.
    return getRolePermissions(roleId);
  });
}

////////////////////
// Favourites
////////////////////

export function getFavourites() {
  return new ApiRequest('/users/current/favourites').get().then((response) => {
    var favourites = _.chain(response.data)
      .groupBy('type')
      .mapValues((type) =>
        _.chain(type)
          .values()
          .map((object) => [object.id, object])
          .fromPairs()
          .value()
      )
      .value();

    store.dispatch({ type: Action.UPDATE_FAVOURITES, favourites: favourites });
  });
}

export function addFavourite(favourite) {
  return new ApiRequest('/users/current/favourites').post(favourite).then((response) => {
    store.dispatch({ type: Action.ADD_FAVOURITE, favourite: response.data });
  });
}

export function updateFavourite(favourite) {
  return new ApiRequest('/users/current/favourites').put(favourite).then((response) => {
    store.dispatch({
      type: Action.UPDATE_FAVOURITE,
      favourite: response.data,
    });
  });
}

export function deleteFavourite(favourite) {
  return new ApiRequest(`/users/current/favourites/${favourite.id}/delete`).post().then((response) => {
    store.dispatch({
      type: Action.DELETE_FAVOURITE,
      favourite: response.data,
    });
  });
}

////////////////////
// Equipment
////////////////////
function getBlockDisplayName(blockNumber, numberOfBlocks, seniority) {
  if (blockNumber === numberOfBlocks) {
    return `Open - ${seniority}`;
  } else if (blockNumber === 1) {
    return `1 - ${seniority}`;
  } else if (blockNumber === 2) {
    return `2 - ${seniority}`;
  } else if (seniority != null) {
    return `Open - ${seniority}`;
  }
  return 'Open';
}

function parseEquipment(equipment) {
  if (!equipment.owner) {
    equipment.owner = { id: 0, organizationName: '' };
  }
  if (!equipment.districtEquipmentType) {
    equipment.districtEquipmentType = { id: 0, districtEquipmentName: '' };
  }
  if (!equipment.localArea) {
    equipment.localArea = { id: 0, name: '' };
  }
  if (!equipment.localArea.serviceArea) {
    equipment.localArea.serviceArea = { id: 0, name: '' };
  }
  if (!equipment.localArea.serviceArea.district) {
    equipment.localArea.serviceArea.district = { id: 0, name: '' };
  }
  if (!equipment.localArea.serviceArea.district.region) {
    equipment.localArea.serviceArea.district.region = { id: 0, name: '' };
  }
  if (!equipment.status) {
    equipment.status = Constant.EQUIPMENT_STATUS_CODE_PENDING;
  }
  if (!equipment.equipmentAttachments) {
    equipment.equipmentAttachments = [];
  }

  equipment.isApproved = equipment.status === Constant.EQUIPMENT_STATUS_CODE_APPROVED;
  equipment.isNew = equipment.status === Constant.EQUIPMENT_STATUS_CODE_PENDING;
  equipment.isArchived = equipment.status === Constant.EQUIPMENT_STATUS_CODE_ARCHIVED;
  equipment.isMaintenanceContractor = equipment.owner.isMaintenanceContractor === true;
  equipment.isDumpTruck =
    (equipment.districtEquipmentType.equipmentType && equipment.districtEquipmentType.equipmentType.isDumpTruck) ||
    false;

  equipment.ownerStatus = equipment.owner.status;

  // UI display fields
  equipment.serialNumber = equipment.serialNumber || '';
  equipment.equipmentCode = equipment.equipmentCode || '';
  equipment.licencePlate = equipment.licencePlate || '';
  equipment.operator = equipment.operator || ''; // TODO Needs review from business
  equipment.organizationName = equipment.owner.organizationName;
  equipment.ownerPath = equipment.owner.id ? `/owners/${equipment.owner.id}` : '';
  equipment.typeName = equipment.districtEquipmentType ? equipment.districtEquipmentType.districtEquipmentName : '';
  equipment.localAreaName = equipment.localArea.name;
  equipment.districtName = equipment.localArea.serviceArea.district.name;
  equipment.lastVerifiedDate = equipment.lastVerifiedDate || '';
  equipment.daysSinceVerified = daysAgo(equipment.lastVerifiedDate);
  equipment.details = [
    equipment.make || '-',
    equipment.model || '-',
    equipment.size || '-',
    equipment.year || '-',
  ].join('/');

  // Seniority data
  equipment.serviceHoursThisYear = equipment.serviceHoursThisYear || 0;
  equipment.serviceHoursLastYear = equipment.serviceHoursLastYear || 0;
  equipment.serviceHoursTwoYearsAgo = equipment.serviceHoursTwoYearsAgo || 0;
  equipment.serviceHoursThreeYearsAgo = equipment.serviceHoursThreeYearsAgo || 0;

  equipment.isSeniorityOverridden = equipment.isSeniorityOverridden || false;
  equipment.seniorityOverrideReason = equipment.seniorityOverrideReason || '';

  // The number of years of active service of this piece of equipment at the time seniority is calculated - April 1 of the current fiscal year
  equipment.yearsOfService = equipment.yearsOfService || 0;
  equipment.receivedDate = equipment.receivedDate || '';
  equipment.approvedDate = equipment.approvedDate || '';
  // The max date of a time card for this fiscal year - can be null if there are none.
  equipment.lastTimeRecordDateThisYear = equipment.lastTimeRecordDateThisYear || '';
  // e.g. "Open-500" or "1-744"
  equipment.seniorityText = getBlockDisplayName(equipment.blockNumber, equipment.numberOfBlocks, equipment.seniority);

  equipment.currentYear = Moment().year();
  equipment.lastYear = equipment.currentYear - 1;
  equipment.twoYearsAgo = equipment.currentYear - 2;
  equipment.threeYearsAgo = equipment.currentYear - 3;

  // It is possible to have multiple instances of the same piece of equipment registered with HETS.
  // However, the HETS clerks would like to know about it via this flag so they can deal with the duplicates.
  equipment.hasDuplicates = equipment.hasDuplicates || false;
  equipment.duplicateEquipment = equipment.duplicateEquipment || [];

  equipment.isHired = equipment.isHired || false;
  // TODO Descriptive text for time entries. Needs to be added to backend
  equipment.currentWorkDescription = equipment.currentWorkDescription || '';

  equipment.path = `${Constant.EQUIPMENT_PATHNAME}/${equipment.id}`;
  equipment.url = `${equipment.path}`;
  equipment.name = `code ${equipment.equipmentCode}`;
  equipment.historyEntity = History.makeHistoryEntity(Constant.HISTORY_EQUIPMENT, equipment);
  equipment.documentAdded = Log.equipmentDocumentAdded;
  equipment.documentsAdded = Log.equipmentDocumentsAdded;
  equipment.documentDeleted = Log.equipmentDocumentDeleted;

  equipment.getDocumentsPromise = getEquipmentDocuments;
  equipment.uploadDocumentPath = `/equipment/${equipment.id}/attachments`;

  equipment.canView = true;
  equipment.canEdit = true;
  equipment.canDelete = false; // TODO Needs input from Business whether this is needed.
}

function generateSortableEquipmentCode(equipment) {
  return equipment.equipmentPrefix && equipment.equipmentNumber
    ? `${equipment.equipmentPrefix}${_.padStart(equipment.equipmentNumber, 3, '0')}`
    : equipment.equipmentCode;
}

export function searchEquipmentList(params) {
  store.dispatch({ type: Action.EQUIPMENT_LIST_REQUEST });
  return new ApiRequest('/equipment/search').get(params).then((response) => {
    var equipmentList = normalize(response.data);

    _.map(equipmentList, (equipment) => {
      equipment.details = [
        equipment.make || '-',
        equipment.model || '-',
        equipment.size || '-',
        equipment.year || '-',
      ].join('/');
      equipment.sortableEquipmentCode = generateSortableEquipmentCode(equipment);
    });

    store.dispatch({
      type: Action.UPDATE_EQUIPMENT_LIST,
      equipmentList: equipmentList,
    });
  });
}

export function getEquipment(equipmentId) {
  return new ApiRequest(`/equipment/${equipmentId}`).get().then((response) => {
    var equipment = response.data;

    // Add display fields
    parseEquipment(equipment);

    store.dispatch({ type: Action.UPDATE_EQUIPMENT, equipment: equipment });
  });
}

export function getEquipmentLite() {
  const silent = store.getState().lookups.equipment.lite.loaded;
  return new ApiRequest('/equipment/lite', { silent }).get().then((response) => {
    var equipment = normalize(response.data);

    store.dispatch({
      type: Action.UPDATE_EQUIPMENT_LITE_LOOKUP,
      equipment: equipment,
    });
  });
}

export function getEquipmentAgreementSummary() {
  const silent = store.getState().lookups.equipment.ts.loaded;
  return new ApiRequest('/equipment/agreementSummary', { silent }).get().then((response) => {
    var equipment = normalize(response.data);

    store.dispatch({
      type: Action.UPDATE_EQUIPMENT_AGREEMENT_SUMMARY_LOOKUP,
      equipment: equipment,
    });
  });
}

export function getEquipmentTs() {
  const silent = store.getState().lookups.equipment.ts.loaded;
  return new ApiRequest('/equipment/liteTs', { silent }).get().then((response) => {
    var equipment = normalize(response.data);

    store.dispatch({
      type: Action.UPDATE_EQUIPMENT_TS_LOOKUP,
      equipment: equipment,
    });
  });
}

export function getEquipmentHires() {
  const silent = store.getState().lookups.equipment.hires.loaded;
  return new ApiRequest('/equipment/liteHires', { silent }).get().then((response) => {
    var equipment = normalize(response.data);

    store.dispatch({
      type: Action.UPDATE_EQUIPMENT_HIRES_LOOKUP,
      equipment: equipment,
    });
  });
}

export function addEquipment(equipment) {
  return new ApiRequest('/equipment').post(equipment).then((response) => {
    var equipment = response.data;

    // Add display fields
    parseEquipment(equipment);

    store.dispatch({ type: Action.UPDATE_EQUIPMENT, equipment: equipment });

    getEquipmentLite();
    getEquipmentTs();
    getEquipmentHires();

    return equipment;
  });
}

export function updateEquipment(equipment) {
  return new ApiRequest(`/equipment/${equipment.id}`).put(equipment).then((response) => {
    var equipment = response.data;

    // Add display fields
    parseEquipment(equipment);

    store.dispatch({ type: Action.UPDATE_EQUIPMENT, equipment: equipment });

    getEquipmentLite();
    getEquipmentTs();
    getEquipmentHires();
  });
}

export function verifyEquipmentActive(id) {
  return new ApiRequest(`/equipment/${id}/verifyactive`).put(id).then((response) => {
    var equipment = response.data;

    // Add display fields
    parseEquipment(equipment);

    store.dispatch({ type: Action.UPDATE_EQUIPMENT, equipment: equipment });

    getEquipmentLite();
    getEquipmentTs();
    getEquipmentHires();
  });
}

export function addEquipmentHistory(equipmentId, history) {
  return new ApiRequest(`/equipment/${equipmentId}/history`).post(history).then((response) => {
    var history = normalize(response.data);
    // Add display fields
    _.map(history, (history) => {
      parseHistory(history);
    });

    store.dispatch({
      type: Action.UPDATE_EQUIPMENT_HISTORY,
      history,
      id: equipmentId,
    });
  });
}

export function getEquipmentHistory(equipmentId, params) {
  return new ApiRequest(`/equipment/${equipmentId}/history`).get(params).then((response) => {
    var history = normalize(response.data);

    // Add display fields
    _.map(history, (history) => {
      parseHistory(history);
    });

    store.dispatch({
      type: Action.UPDATE_EQUIPMENT_HISTORY,
      history,
      id: equipmentId,
    });
  });
}

export function getEquipmentDocuments(equipmentId) {
  return new ApiRequest(`/equipment/${equipmentId}/attachments`).get().then((response) => {
    var documents = normalize(response.data);

    // Add display fields
    _.map(documents, (document) => {
      parseDocument(document);
    });

    store.dispatch({ type: Action.UPDATE_DOCUMENTS, documents: documents });
  });
}

// XXX: Looks like this is unused
// export function addEquipmentDocument(equipmentId, files) {
//   return new ApiRequest(`/equipment/${ equipmentId }/attachments`).post(files);
// }

export function getEquipmentNotes(equipmentId) {
  return new ApiRequest(`/equipment/${equipmentId}/notes`).get().then((response) => {
    store.dispatch({
      type: Action.UPDATE_EQUIPMENT_NOTES,
      notes: response.data,
    });
    return response.data;
  });
}

export function addEquipmentNote(equipmentId, note) {
  return new ApiRequest(`/equipment/${equipmentId}/note`).post(note).then((response) => {
    store.dispatch({
      type: Action.UPDATE_EQUIPMENT_NOTES,
      notes: response.data,
    });
    return response.data;
  });
}

export function equipmentDuplicateCheck(id, serialNumber) {
  return new ApiRequest(`/equipment/${id}/duplicates/${serialNumber}`).get();
}

export function changeEquipmentStatus(status) {
  return new ApiRequest(`/equipment/${status.id}/status`).put(status).then((response) => {
    var equipment = response.data;
    // Add display fields
    parseEquipment(equipment);
    store.dispatch({ type: Action.UPDATE_EQUIPMENT, equipment: equipment });
    return response;
  });
}

export function getEquipmentRentalAgreements(equipmentId) {
  return new ApiRequest(`/equipment/${equipmentId}/rentalAgreements`).get().then((response) => {
    var rentalAgreements = normalize(response.data);
    store.dispatch({
      type: Action.UPDATE_EQUIPMENT_RENTAL_AGREEMENTS,
      rentalAgreements: rentalAgreements,
    });
    return rentalAgreements;
  });
}

export function cloneEquipmentRentalAgreement(data) {
  return new ApiRequest(`/equipment/${data.equipmentId}/rentalAgreementClone`).post(data).then((response) => {
    var agreement = response.data;
    // Add display fields
    parseRentalAgreement(agreement);
    store.dispatch({
      type: Action.UPDATE_RENTAL_AGREEMENT,
      rentalAgreement: agreement,
    });
    return response;
  });
}

export function equipmentSeniorityListDoc(localAreas, types, counterCopy) {
  var params = { localareas: localAreas, types: types };
  if (counterCopy) {
    params.counterCopy = counterCopy;
  }

  return new ApiRequest('/equipment/seniorityListDoc')
    .get(params, { responseType: Constant.RESPONSE_TYPE_BLOB })
    .then((response) => {
      return response;
    });
}

////////////////////
// Physical Attachments
////////////////////

// Introduce later
// function parsePhysicalAttachment(attachment) {
//   if (!attachment.type) { attachment.type = { id: 0, code: '', description: ''}; }

//   attachment.typeName = attachment.type.description;
//   // TODO Add grace period logic to editing/deleting attachments
//   attachment.canEdit = true;
//   attachment.canDelete = true;
// }

// XXX: Looks like this is unused
// export function getPhysicalAttachment(id) {
//   return new ApiRequest(`/equipment/${id}/equipmentAttachments`).get().then(response => {
//     store.dispatch({ type: Action.UPDATE_EQUIPMENT_ATTACHMENTS, physicalAttachments: response.data });
//   });
// }

// XXX: Looks like this is unused
// export function addPhysicalAttachment(attachment) {
//   return new ApiRequest('/equipmentAttachments').post(attachment).then(response => {
//     store.dispatch({ type: Action.ADD_EQUIPMENT_ATTACHMENT, physicalAttachment: response.data });
//   });
// }

export function addPhysicalAttachments(equipmentId, attachmentTypeNames) {
  const attachments = attachmentTypeNames.map((typeName) => {
    // "concurrencyControlNumber": 0,
    return {
      typeName,
      description: '',
      equipmentId,
      equipment: { id: equipmentId },
    };
  });

  return new ApiRequest('/equipmentAttachments/bulk').post(attachments).then((response) => {
    store.dispatch({
      type: Action.ADD_EQUIPMENT_ATTACHMENTS,
      physicalAttachments: response.data,
    });
  });
}

export function updatePhysicalAttachment(attachment) {
  return new ApiRequest(`/equipmentAttachments/${attachment.id}`).put(attachment).then((response) => {
    store.dispatch({
      type: Action.UPDATE_EQUIPMENT_ATTACHMENT,
      physicalAttachment: response.data,
    });
  });
}

export function deletePhysicalAttachment(attachmentId) {
  return new ApiRequest(`/equipmentAttachments/${attachmentId}/delete`).post().then((response) => {
    store.dispatch({
      type: Action.DELETE_EQUIPMENT_ATTACHMENT,
      physicalAttachment: response.data,
    });
  });
}

////////////////////
// Owners
////////////////////

function parseOwner(owner) {
  // Rename properties
  owner.equipmentList = owner.equipment;
  delete owner.equipment;

  owner.workSafeBCExpiryDate = owner.workSafeBcexpiryDate;
  delete owner.workSafeBcexpiryDate;

  owner.workSafeBCPolicyNumber = owner.workSafeBcpolicyNumber;
  delete owner.workSafeBcpolicyNumber;

  owner.cglEndDate = owner.cglendDate;
  delete owner.cglendDate;

  if (!owner.localArea) {
    owner.localArea = { id: 0, name: '' };
  }
  if (!owner.localArea.serviceArea) {
    owner.localArea.serviceArea = { id: 0, name: '' };
  }
  if (!owner.localArea.serviceArea.district) {
    owner.localArea.serviceArea.district = { id: 0, name: '' };
  }
  if (!owner.localArea.serviceArea.district.region) {
    owner.localArea.serviceArea.district.region = { id: 0, name: '' };
  }
  if (!owner.contacts) {
    owner.contacts = [];
  }
  if (!owner.documents) {
    owner.documents = [];
  }
  if (!owner.equipmentList) {
    owner.equipmentList = [];
  }

  owner.organizationName = owner.organizationName || '';
  owner.ownerCode = owner.ownerCode || '';
  owner.doingBusinessAs = owner.doingBusinessAs || '';
  owner.registeredCompanyNumber = owner.registeredCompanyNumber || '';
  owner.meetsResidency = owner.meetsResidency || false;
  owner.workSafeBCPolicyNumber = owner.workSafeBCPolicyNumber || '';
  owner.workSafeBCExpiryDate = owner.workSafeBCExpiryDate || '';
  owner.cglEndDate = owner.cglEndDate || '';
  owner.address1 = owner.address1 || '';
  owner.address2 = owner.address2 || '';
  owner.city = owner.city || '';
  owner.province = owner.province || '';
  owner.postalCode = owner.postalCode || '';
  owner.fullAddress = `${owner.address1} ${owner.address2} ${owner.city} ${owner.province} ${owner.postalCode}`;
  owner.ownerName = owner.givenName && owner.surname ? `${owner.givenName} ${owner.surname}` : '';

  owner.path = `${Constant.OWNERS_PATHNAME}/${owner.id}`;
  owner.url = `${owner.path}`;
  owner.name = owner.organizationName;
  owner.historyEntity = History.makeHistoryEntity(Constant.HISTORY_OWNER, owner);
  owner.documentAdded = Log.ownerDocumentAdded;
  owner.documentsAdded = Log.ownerDocumentsAdded;
  owner.documentDeleted = Log.ownerDocumentDeleted;

  // Add display fields for owner contacts
  owner.contacts = owner.contacts.map((contact) => parseContact(contact, owner));

  _.map(owner.documents, (document) => {
    parseDocument(document);
  });
  _.map(owner.equipmentList, (equipment) => {
    parseEquipment(equipment);
  });

  // TODO Owner status needs to be populated in sample data. Setting to Approved for the time being...
  owner.status = owner.status || Constant.OWNER_STATUS_CODE_APPROVED;

  // UI display fields
  owner.isMaintenanceContractor = owner.isMaintenanceContractor || false;
  owner.isApproved = owner.status === Constant.OWNER_STATUS_CODE_APPROVED;
  owner.primaryContactName = owner.primaryContact
    ? firstLastName(owner.primaryContact.givenName, owner.primaryContact.surname)
    : '';
  owner.localAreaName = owner.localArea.name;
  owner.districtName = owner.localArea.serviceArea.district.name;
  owner.numberOfEquipment = Object.keys(owner.equipmentList).length;
  owner.numberOfPolicyDocuments = owner.numberOfPolicyDocuments || 0; // TODO

  owner.getDocumentsPromise = getOwnerDocuments;
  owner.uploadDocumentPath = `/owners/${owner.id}/attachments`;

  owner.canView = true;
  owner.canEdit = true;
  owner.canDelete = false; // TODO Needs input from Business whether this is needed.
}

export function searchOwners(params) {
  store.dispatch({ type: Action.OWNERS_REQUEST });
  return new ApiRequest('/owners/search').get(params).then((response) => {
    var owners = normalize(response.data);
    store.dispatch({ type: Action.UPDATE_OWNERS, owners: owners });
  });
}

export function getOwner(ownerId) {
  return new ApiRequest(`/owners/${ownerId}`).get().then((response) => {
    var owner = response.data;

    // Add display fields
    parseOwner(owner);

    store.dispatch({ type: Action.UPDATE_OWNER, owner });

    return owner;
  });
}

export function addOwner(owner) {
  return new ApiRequest('/owners').post(owner).then((response) => {
    var owner = response.data;

    // Add display fields
    parseOwner(owner);

    store.dispatch({ type: Action.ADD_OWNER, owner });

    return owner;
  });
}

export function updateOwner(owner) {
  store.dispatch({ type: Action.UPDATE_OWNER, owner });

  // Omit `contacts` to ensure that the existing contacts don't mess up the PUT call
  return new ApiRequest(`/owners/${owner.id}`).put(_.omit(owner, 'contacts')).then((response) => {
    var owner = response.data;

    // Add display fields
    parseOwner(owner);

    store.dispatch({ type: Action.UPDATE_OWNER, owner });
  });
}

// XXX: Looks like this is unused
// export function deleteOwner(owner) {
//   return new ApiRequest(`/owners/${ owner.id }/delete`).post().then(response => {
//     var owner = response.data;

//     // Add display fields
//     parseOwner(owner);

//     store.dispatch({ type: Action.DELETE_OWNER, owner });
//   });
// }

export function saveOwnerContact(owner, contact) {
  const isNew = contact.id === 0;

  if (!isNew) {
    // don't update if this is a new contact - add after post() completes
    store.dispatch({
      type: Action.UPDATE_OWNER_CONTACT,
      ownerId: owner.id,
      contact,
    });
  }

  return new ApiRequest(`/owners/${owner.id}/contacts/${contact.isPrimary}`).post(contact).then((response) => {
    var updatedContact = response.data;

    // Add display fields
    parseContact(updatedContact, owner); // owner's primary contact could be outdated
    updatedContact.isPrimary = contact.isPrimary;

    if (isNew) {
      // add newly created contact to Redux store's contacts
      store.dispatch({
        type: Action.ADD_OWNER_CONTACT,
        ownerId: owner.id,
        contact: updatedContact,
      });
    } else {
      // Update Redux store's data with the server's data
      store.dispatch({
        type: Action.UPDATE_OWNER_CONTACT,
        ownerId: owner.id,
        contact: updatedContact,
      });
    }

    return updatedContact;
  });
}

export function addOwnerHistory(ownerId, history) {
  return new ApiRequest(`/owners/${ownerId}/history`).post(history).then((response) => {
    var history = normalize(response.data);
    // Add display fields
    _.map(history, (history) => {
      parseHistory(history);
    });

    store.dispatch({
      type: Action.UPDATE_OWNER_HISTORY,
      history,
      id: ownerId,
    });
  });
}

export function getOwnerHistory(ownerId, params) {
  return new ApiRequest(`/owners/${ownerId}/history`).get(params).then((response) => {
    var history = normalize(response.data);
    // Add display fields
    _.map(history, (history) => {
      parseHistory(history);
    });

    store.dispatch({
      type: Action.UPDATE_OWNER_HISTORY,
      history,
      id: ownerId,
    });
  });
}

export function getOwnerDocuments(ownerId) {
  return new ApiRequest(`/owners/${ownerId}/attachments`).get().then((response) => {
    var documents = normalize(response.data);

    // Add display fields
    _.map(documents, (document) => {
      parseDocument(document);
    });

    store.dispatch({ type: Action.UPDATE_DOCUMENTS, documents: documents });
  });
}

// XXX: Looks like this is unused
// export function addOwnerDocument(ownerId, files) {
//   return new ApiRequest(`/owners/${ ownerId }/attachments`).post(files);
// }

export function getOwnerEquipment(ownerId) {
  return new ApiRequest(`/owners/${ownerId}/equipment`).get().then((response) => {
    var equipmentList = normalize(response.data);

    _.map(equipmentList, (equipment) => {
      equipment.details = [
        equipment.make || '-',
        equipment.model || '-',
        equipment.size || '-',
        equipment.year || '-',
      ].join('/');
    });

    store.dispatch({
      type: Action.UPDATE_OWNER_EQUIPMENT,
      equipment: equipmentList,
    });
  });
}

export function updateOwnerEquipment(owner, equipmentArray) {
  return new ApiRequest(`/owners/${owner.id}/equipment`).put(equipmentArray);
}

export function getOwnerNotes(ownerId) {
  return new ApiRequest(`/owners/${ownerId}/notes`).get().then((response) => {
    store.dispatch({
      type: Action.UPDATE_OWNER_NOTES,
      ownerId,
      notes: response.data,
    });
    return response.data;
  });
}

export function addOwnerNote(ownerId, note) {
  store.dispatch({ type: Action.ADD_OWNER_NOTE, ownerId, note });
  return new ApiRequest(`/owners/${ownerId}/note`).post(note).then((response) => {
    return response.data;
  });
}

// XXX: Looks like this is unused
// export function getOwnersByDistrict(districtId) {
//   return new ApiRequest(`/districts/${districtId}/owners`).get().then((response) => {
//     var owners = normalize(response.data);
//     // Add display fields
//     _.map(owners, owner => { parseOwner(owner); });
//     store.dispatch({ type: Action.UPDATE_OWNERS_LOOKUP, owners: owners });
//   });
// }

export function changeOwnerStatus(status) {
  return new ApiRequest(`/owners/${status.id}/status`).put(status).then((response) => {
    var owner = response.data;
    // Add display fields
    parseOwner(owner);
    store.dispatch({ type: Action.UPDATE_OWNER, owner: owner });
    return response;
  });
}

export function getStatusLettersDoc(params) {
  return new ApiRequest('/owners/verificationDoc').post(params, {
    responseType: Constant.RESPONSE_TYPE_BLOB,
  });
}

export function getMailingLabelsDoc(params) {
  return new ApiRequest('/owners/mailingLabelsDoc').post(params, {
    responseType: Constant.RESPONSE_TYPE_BLOB,
  });
}

export function transferEquipment(donorOwnerId, recipientOwnerId, equipment, includeSeniority) {
  return new ApiRequest(`/owners/${donorOwnerId}/equipmentTransfer/${recipientOwnerId}/${includeSeniority}`)
    .post(equipment)
    .then((response) => {
      getEquipmentLite();
      getEquipmentTs();
      getEquipmentHires();

      return response;
    });
}

////////////////////
// Contacts
////////////////////

function parseContact(contact, parent) {
  contact.name = firstLastName(contact.givenName, contact.surname);
  contact.phone = contact.workPhoneNumber
    ? `${contact.workPhoneNumber} (w)`
    : contact.mobilePhoneNumber
    ? `${contact.mobilePhoneNumber} (c)`
    : '';

  var parentPath = '';
  var primaryContactId = 0;
  if (parent) {
    parentPath = parent.path || '';
    primaryContactId = parent.primaryContact ? parent.primaryContact.id : 0;
  }

  contact.isPrimary = contact.id === primaryContactId;

  contact.path = parentPath ? `${parentPath}${Constant.CONTACTS_PATHNAME}/${contact.id}` : null;
  contact.url = contact.path ? `${contact.path}` : null;
  contact.historyEntity = History.makeHistoryEntity(Constant.HISTORY_CONTACT, contact);

  contact.canEdit = true;
  contact.canDelete = true;

  return contact;
}

// XXX: Looks like this is unused
// export function getContacts() {
//   return new ApiRequest('/contacts').get().then(response => {
//     var contacts = normalize(response.data);

//     // Add display fields
//     _.map(contacts, contact => { parseContact(contact); });

//     store.dispatch({ type: Action.UPDATE_CONTACTS, contacts: contacts });
//   });
// }

// XXX: Looks like this is unused
// export function getContact(contactId) {
//   return new ApiRequest(`/contacts/${ contactId }`).get().then(response => {
//     var contact = response.data;

//     // Add display fields
//     parseContact(contact);

//     store.dispatch({ type: Action.UPDATE_CONTACT, contact: contact });
//   });
// }

// XXX: Looks like this is unused
// export function addContact(parent, contact) {
//   return new ApiRequest('/contacts').post(contact).then(response => {
//     var contact = response.data;

//     // Add display fields
//     parseContact(contact, parent);

//     store.dispatch({ type: Action.ADD_CONTACT, contact: contact });
//   });
// }

// export function updateContact(parent, contact) {
//   return new ApiRequest(`/contacts/${ contact.id }`).put(contact).then(response => {
//     var contact = response.data;

//     // Add display fields
//     parseContact(contact, parent);

//     store.dispatch({ type: Action.UPDATE_CONTACT, contact: contact });
//   });
// }

export function deleteContact(contact) {
  store.dispatch({ type: Action.DELETE_CONTACT, contact });
  return new ApiRequest(`/contacts/${contact.id}/delete`).post().then((response) => {
    var contact = response.data;

    // Add display fields
    parseContact(contact);

    store.dispatch({ type: Action.DELETE_CONTACT, contact });
  });
}

////////////////////
// Documents
////////////////////

function getFileSizeString(fileSizeInBytes) {
  var bytes = parseInt(fileSizeInBytes, 10) || 0;
  var kbytes = bytes >= 1024 ? bytes / 1024 : 0;
  var mbytes = kbytes >= 1024 ? kbytes / 1024 : 0;
  var gbytes = mbytes >= 1024 ? mbytes / 1024 : 0;

  var ceiling10 = function (num) {
    var adjusted = Math.ceil(num * 10) / 10;
    return adjusted.toFixed(1);
  };

  return gbytes
    ? `${ceiling10(gbytes)} GB`
    : mbytes
    ? `${ceiling10(mbytes)} MB`
    : kbytes
    ? `${Math.ceil(kbytes)} KB`
    : `${bytes} bytes`;
}

function parseDocument(document) {
  document.fileSizeDisplay = getFileSizeString(document.fileSize);
  document.timestampSort = sortableDateTime(document.lastUpdateTimestamp);
  document.name = document.fileName;

  document.canDelete = true;
  document.historyEntity = History.makeHistoryEntity(Constant.HISTORY_DOCUMENT, document);
}

export function deleteDocument(document) {
  return new ApiRequest(`/attachments/${document.id}/delete`).post();
}

export function getDownloadDocument(document) {
  return new ApiRequest(`/attachments/${document.id}/download`);
}

export function getDownloadDocumentURL(document) {
  //XXX: Not used in the application. Last checked 17 Jun 2021.
  // Not an API call, per se, as it must be called from the browser window.
  return `${window.location.origin}${window.location.pathname}api/attachments/${document.id}/download`;
}

////////////////////
// History
////////////////////

function parseHistory(history) {
  history.timestampSort = sortableDateTime(history.lastUpdateTimestamp);
}

////////////////////
// Projects
////////////////////

function parseProject(project) {
  if (!project.district) {
    project.district = { id: 0, name: '' };
  }
  if (!project.district.region) {
    project.district.region = { id: 0, name: '' };
  }
  if (!project.contacts) {
    project.contacts = [];
  }
  if (!project.rentalRequests) {
    project.rentalRequests = [];
  }
  if (!project.rentalAgreements) {
    project.rentalAgreements = [];
  }

  project.name = project.name || '';
  project.provincialProjectNumber = project.provincialProjectNumber || '';
  project.information = project.information || '';

  project.path = `${Constant.PROJECTS_PATHNAME}/${project.id}`;
  project.url = `${project.path}`;
  project.historyEntity = History.makeHistoryEntity(Constant.HISTORY_PROJECT, project);
  project.documentAdded = Log.projectDocumentAdded;
  project.documentsAdded = Log.projectDocumentsAdded;
  project.documentDeleted = Log.projectDocumentDeleted;

  // Add display fields for contacts
  project.contacts = project.contacts.map((contact) => parseContact(contact, project));

  // Add display fields for rental requests and rental agreements
  _.map(project.rentalRequests, (obj) => {
    parseRentalRequest(obj);
  });
  _.map(project.rentalAgreements, (obj) => {
    parseRentalAgreement(obj);
  });

  project.numberOfRequests = project.numberOfRequests || Object.keys(project.rentalRequests).length;
  project.numberOfHires = project.numberOfHires || Object.keys(project.rentalAgreements).length;

  // UI display fields
  project.label = `${project.provincialProjectNumber} - ${project.name}`;
  project.status = project.status || Constant.PROJECT_STATUS_CODE_ACTIVE;
  project.isActive = project.status === Constant.PROJECT_STATUS_CODE_ACTIVE;
  project.districtName = project.district.name;

  project.primaryContactName = project.primaryContact
    ? firstLastName(project.primaryContact.givenName, project.primaryContact.surname)
    : '';
  project.primaryContactRole = project.primaryContact ? project.primaryContact.role : '';
  project.primaryContactEmail = project.primaryContact ? project.primaryContact.emailAddress : '';
  project.primaryContactPhone = project.primaryContact
    ? project.primaryContact.workPhoneNumber || project.primaryContact.mobilePhoneNumber || ''
    : '';

  project.getDocumentsPromise = getProjectDocuments;
  project.uploadDocumentPath = `/projects/${project.id}/attachments`;

  project.canView = true;
  project.canEdit = true;
  project.canDelete = false; // TODO Needs input from Business whether this is needed.
}

function formatTimeRecords(timeRecords, rentalRequestId) {
  let formattedTimeRecords = Object.keys(timeRecords).map((key) => {
    let timeRecord = {};
    timeRecord.workedDate = timeRecords[key].date;
    timeRecord.hours = timeRecords[key].hours;
    timeRecord.timePeriod = 'Week';
    timeRecord.rentalAgreement = { id: rentalRequestId };
    return timeRecord;
  });
  return formattedTimeRecords;
}

export function searchProjects(params) {
  store.dispatch({ type: Action.PROJECTS_REQUEST });
  return new ApiRequest('/projects/search').get(params).then((response) => {
    var projects = normalize(response.data);

    // Add display fields
    _.map(projects, (project) => {
      parseProject(project);
    });

    store.dispatch({ type: Action.UPDATE_PROJECTS, projects: projects });
  });
}

export function searchTimeEntries(params) {
  store.dispatch({ type: Action.TIME_ENTRIES_REQUEST });
  return new ApiRequest('/timeRecords/search').get(params).then((response) => {
    var timeEntries = normalize(response.data);

    _.map(timeEntries, (entry) => {
      entry.localAreaLabel = `${entry.serviceAreaId} - ${entry.localAreaName}`;
      entry.equipmentDetails = [entry.make || '-', entry.model || '-', entry.size || '-', entry.year || '-'].join('/');
      entry.sortableEquipmentCode = generateSortableEquipmentCode(entry);
    });

    store.dispatch({
      type: Action.UPDATE_TIME_ENTRIES,
      timeEntries: timeEntries,
    });
  });
}

export function searchHiringReport(params) {
  store.dispatch({ type: Action.HIRING_RESPONSES_REQUEST });
  return new ApiRequest('/rentalRequests/hireReport').get(params).then((response) => {
    var hiringResponses = normalize(response.data);

    _.map(hiringResponses, (entry) => {
      entry.localAreaLabel = `${entry.serviceAreaId} - ${entry.localAreaName}`;
      entry.equipmentDetails = [
        entry.equipmentMake || '-',
        entry.equipmentModel || '-',
        entry.equipmentSize || '-',
        entry.equipmentYear || '-',
      ].join('/');
      entry.sortableEquipmentCode = generateSortableEquipmentCode(entry);
    });

    store.dispatch({
      type: Action.UPDATE_HIRING_RESPONSES,
      hiringResponses: hiringResponses,
    });
  });
}

export function searchOwnersCoverage(params) {
  store.dispatch({ type: Action.OWNERS_COVERAGE_REQUEST });
  return new ApiRequest('/owners/wcbCglReport').get(params).then((response) => {
    var ownersCoverage = normalize(response.data);

    _.map(ownersCoverage, (entry) => {
      entry.localAreaLabel = `${entry.serviceAreaId} - ${entry.localAreaName}`;
    });

    store.dispatch({
      type: Action.UPDATE_OWNERS_COVERAGE,
      ownersCoverage: ownersCoverage,
    });
  });
}

export function getProjects() {
  const silent = store.getState().lookups.projects.loaded;
  return new ApiRequest('/projects', { silent }).get({ currentFiscal: false }).then((response) => {
    var projects = normalize(response.data);

    // Add display fields
    _.map(projects, (project) => {
      parseProject(project);
    });

    store.dispatch({
      type: Action.UPDATE_PROJECTS_LOOKUP,
      projects: projects,
    });
  });
}

export function getProjectsAgreementSummary() {
  const silent = store.getState().lookups.projectsAgreementSummary.loaded;
  return new ApiRequest('/projects/agreementSummary', { silent }).get().then((response) => {
    var projects = normalize(response.data);

    store.dispatch({
      type: Action.UPDATE_PROJECTS_AGREEMENT_SUMMARY_LOOKUP,
      projects: projects,
    });
  });
}

export function getProjectsCurrentFiscal() {
  const silent = store.getState().lookups.projectsCurrentFiscal.loaded;
  return new ApiRequest('/projects', { silent }).get({ currentFiscal: true }).then((response) => {
    var projects = normalize(response.data);

    // Add display fields
    _.map(projects, (project) => {
      parseProject(project);
    });

    store.dispatch({
      type: Action.UPDATE_PROJECTS_CURRENT_FISCAL_LOOKUP,
      projects: projects,
    });
  });
}

export function getProject(projectId) {
  return new ApiRequest(`/projects/${projectId}`).get().then((response) => {
    var project = response.data;

    // Add display fields
    parseProject(project);

    store.dispatch({ type: Action.UPDATE_PROJECT, project: project });

    return project;
  });
}

export function addProject(project) {
  return new ApiRequest('/projects').post(project).then((response) => {
    var project = response.data;

    // Add display fields
    parseProject(project);

    store.dispatch({ type: Action.ADD_PROJECT, project: project });

    return project;
  });
}

export function updateProject(project) {
  const projectData = _.omit(
    project,
    'notes',
    'contacts',
    'historyEntity',
    'primaryContact',
    'rentalAgreements',
    'rentalRequests'
  );
  projectData.projectId = project.id;

  return new ApiRequest(`/projects/${project.id}`).put(projectData).then((response) => {
    var project = response.data;

    // Add display fields
    parseProject(project);

    store.dispatch({ type: Action.UPDATE_PROJECT, project: project });
  });
}

// XXX: Looks like this is unused
// export function getProjectEquipment(projectId) {
//   return new ApiRequest(`/projects/${projectId}/equipment`).get().then(response => {
//     var projectEquipment = normalize(response.data);

//     store.dispatch({ type: Action.UPDATE_PROJECT_EQUIPMENT, projectEquipment: projectEquipment });
//   });
// }

// XXX: Looks like this is unused
// export function getProjectTimeRecords(projectId) {
//   return new ApiRequest(`projects/${projectId}/timeRecords`).get().then(response => {
//     var projectTimeRecords = normalize(response.data);

//     store.dispatch({ type: Action.UPDATE_PROJECT_TIME_RECORDS, projectTimeRecords: projectTimeRecords });
//   });
// }

// XXX: Looks like this is unused
// export function addProjectTimeRecords(projectId, rentalRequestId, timeRecords) {
//   let formattedTimeRecords = formatTimeRecords(timeRecords, rentalRequestId);
//   return new ApiRequest(`projects/${projectId}/timeRecords`).post(formattedTimeRecords).then(response => {
//     var projectTimeRecords = normalize(response.data);

//     store.dispatch({ type: Action.UPDATE_PROJECT_TIME_RECORDS, projectTimeRecords: projectTimeRecords });
//     return projectTimeRecords;
//   });
// }

export function saveProjectContact(project, contact) {
  const isNew = contact.id === 0;

  if (!isNew) {
    // don't update if this is a new contact - add after post() completes
    store.dispatch({
      type: Action.UPDATE_PROJECT_CONTACT,
      projectId: project.id,
      contact,
    });
  }

  return new ApiRequest(`/projects/${project.id}/contacts/${contact.isPrimary}`).post(contact).then((response) => {
    var updatedContact = response.data;

    // Add display fields
    parseContact(updatedContact, project); // project's primary contact could be outdated
    updatedContact.isPrimary = contact.isPrimary;

    if (isNew) {
      // add newly created contact to Redux store's contacts
      store.dispatch({
        type: Action.ADD_PROJECT_CONTACT,
        projectId: project.id,
        contact: updatedContact,
      });
    } else {
      // Update Redux store's data with the server's data
      store.dispatch({
        type: Action.UPDATE_PROJECT_CONTACT,
        projectId: project.id,
        contact: updatedContact,
      });
    }

    return updatedContact;
  });
}

export function addProjectHistory(projectId, history) {
  return new ApiRequest(`/projects/${projectId}/history`).post(history).then((response) => {
    var history = normalize(response.data);
    // Add display fields
    _.map(history, (history) => {
      parseHistory(history);
    });

    store.dispatch({
      type: Action.UPDATE_PROJECT_HISTORY,
      history,
      id: projectId,
    });
  });
}

export function getProjectHistory(projectId, params) {
  return new ApiRequest(`/projects/${projectId}/history`).get(params).then((response) => {
    var history = normalize(response.data);

    // Add display fields
    _.map(history, (history) => {
      parseHistory(history);
    });

    store.dispatch({
      type: Action.UPDATE_PROJECT_HISTORY,
      history,
      id: projectId,
    });
  });
}

export function getProjectDocuments(projectId) {
  return new ApiRequest(`/projects/${projectId}/attachments`).get().then((response) => {
    var documents = normalize(response.data);

    // Add display fields
    _.map(documents, (document) => {
      parseDocument(document);
    });

    store.dispatch({ type: Action.UPDATE_DOCUMENTS, documents: documents });
  });
}

// XXX: Looks like this is unused
// export function addProjectDocument(projectId, files) {
//   return new ApiRequest(`/projects/${ projectId }/attachments`).post(files);
// }

export function getProjectNotes(projectId) {
  return new ApiRequest(`/projects/${projectId}/notes`).get().then((response) => {
    store.dispatch({
      type: Action.UPDATE_PROJECT_NOTES,
      projectId,
      notes: response.data,
    });
    return response.data;
  });
}

export function addProjectNote(projectId, note) {
  store.dispatch({ type: Action.ADD_PROJECT_NOTE, projectId, note });
  return new ApiRequest(`/projects/${projectId}/note`).post(note);
}

export function getProjectRentalAgreements(projectId) {
  return new ApiRequest(`/projects/${projectId}/rentalAgreements`).get().then((response) => {
    var rentalAgreements = normalize(response.data);
    store.dispatch({
      type: Action.UPDATE_PROJECT_RENTAL_AGREEMENTS,
      rentalAgreements: rentalAgreements,
    });
    return rentalAgreements;
  });
}

export function cloneProjectRentalAgreement(data) {
  return new ApiRequest(`/projects/${data.projectId}/rentalAgreementClone`).post(data).then((response) => {
    var agreement = response.data;
    // Add display fields
    parseRentalAgreement(agreement);
    store.dispatch({
      type: Action.UPDATE_RENTAL_AGREEMENT,
      rentalAgreement: agreement,
    });
    return response;
  });
}

////////////////////
// Rental Requests
////////////////////

function parseRentalRequest(rentalRequest) {
  if (!rentalRequest.localArea) {
    rentalRequest.localArea = { id: 0, name: '' };
  }
  if (!rentalRequest.localArea.serviceArea) {
    rentalRequest.localArea.serviceArea = { id: 0, name: '' };
  }
  if (!rentalRequest.localArea.serviceArea.district) {
    rentalRequest.localArea.serviceArea.district = { id: 0, name: '' };
  }
  if (!rentalRequest.localArea.serviceArea.district.region) {
    rentalRequest.localArea.serviceArea.district.region = { id: 0, name: '' };
  }
  if (!rentalRequest.project) {
    rentalRequest.project = { id: 0, name: '' };
  }
  if (!rentalRequest.districtEquipmentType) {
    rentalRequest.districtEquipmentType = { id: 0, districtEquipmentName: '' };
  }
  if (!rentalRequest.primaryContact) {
    rentalRequest.primaryContact = { id: 0, givenName: '', surname: '' };
  }
  if (!rentalRequest.rentalRequestAttachments) {
    rentalRequest.rentalRequestAttachments = [];
  }
  if (!rentalRequest.rentalRequestRotationList) {
    rentalRequest.rentalRequestRotationList = [];
  }

  // Add display fields for primary contact
  parseContact(rentalRequest.primaryContact);

  // Add display fields for rotation list items
  _.map(rentalRequest.rentalRequestRotationList, (listItem) => {
    parseRentalRequestRotationList(listItem, rentalRequest);
  });

  rentalRequest.status = rentalRequest.status || Constant.RENTAL_REQUEST_STATUS_CODE_IN_PROGRESS;
  rentalRequest.equipmentCount = rentalRequest.equipmentCount || 0;
  rentalRequest.expectedHours = rentalRequest.expectedHours || 0;
  rentalRequest.expectedStartDate = rentalRequest.expectedStartDate || '';
  rentalRequest.expectedEndDate = rentalRequest.expectedEndDate || '';

  rentalRequest.projectId = rentalRequest.projectId || rentalRequest.project.id;
  rentalRequest.projectName = rentalRequest.projectName || rentalRequest.project.name;
  rentalRequest.projectPath = rentalRequest.projectId ? `/projects/${rentalRequest.projectId}` : '';

  // UI display fields
  rentalRequest.isActive = rentalRequest.status === Constant.RENTAL_REQUEST_STATUS_CODE_IN_PROGRESS;
  rentalRequest.isCompleted = rentalRequest.status === Constant.RENTAL_REQUEST_STATUS_CODE_COMPLETED;
  rentalRequest.isCancelled = rentalRequest.status === Constant.RENTAL_REQUEST_STATUS_CODE_CANCELLED;
  rentalRequest.localAreaName = rentalRequest.localAreaName || rentalRequest.localArea.name;
  rentalRequest.equipmentTypeName =
    rentalRequest.equipmentTypeName || rentalRequest.districtEquipmentType.districtEquipmentName;

  // Primary contact for the rental request/project
  rentalRequest.primaryContactName = rentalRequest.primaryContact
    ? firstLastName(rentalRequest.primaryContact.givenName, rentalRequest.primaryContact.surname)
    : '';
  rentalRequest.primaryContactEmail = rentalRequest.primaryContact ? rentalRequest.primaryContact.emailAddress : '';
  rentalRequest.primaryContactRole = rentalRequest.primaryContact ? rentalRequest.primaryContact.role : '';
  rentalRequest.primaryContactPhone = rentalRequest.primaryContact
    ? rentalRequest.primaryContact.workPhoneNumber || rentalRequest.primaryContact.mobilePhoneNumber || ''
    : '';

  rentalRequest.projectPrimaryContactName = rentalRequest.project.primaryContact
    ? firstLastName(rentalRequest.project.primaryContact.givenName, rentalRequest.project.primaryContact.surname)
    : '';
  rentalRequest.projectPrimaryContactEmail = rentalRequest.project.primaryContact
    ? rentalRequest.project.primaryContact.emailAddress
    : '';
  rentalRequest.projectPrimaryContactRole = rentalRequest.project.primaryContact
    ? rentalRequest.project.primaryContact.role
    : '';
  rentalRequest.projectPrimaryContactPhone = rentalRequest.project.primaryContact
    ? rentalRequest.project.primaryContact.workPhoneNumber ||
      rentalRequest.project.primaryContact.mobilePhoneNumber ||
      ''
    : '';
  // Flag element as a rental request.
  // Rental requests and rentals are merged and shown in a single list on Project Details screen
  rentalRequest.isRentalRequest = true;

  rentalRequest.path = `${Constant.RENTAL_REQUESTS_PATHNAME}/${rentalRequest.id}`;
  rentalRequest.url = `${rentalRequest.path}`;
  rentalRequest.name = 'TBD';
  rentalRequest.historyEntity = History.makeHistoryEntity(Constant.HISTORY_REQUEST, rentalRequest);
  rentalRequest.documentAdded = Log.rentalRequestDocumentAdded;
  rentalRequest.documentsAdded = Log.rentalRequestDocumentsAdded;
  rentalRequest.documentDeleted = Log.rentalRequestDocumentDeleted;

  rentalRequest.getDocumentsPromise = getRentalRequestDocuments;
  rentalRequest.uploadDocumentPath = `/rentalrequests/${rentalRequest.id}/attachments`;

  rentalRequest.canView = true;
  rentalRequest.canEdit = true;
  // HETS-894: view-only requests and requests that have yet to be acted on can be deleted
  rentalRequest.canDelete = rentalRequest.projectId === 0 || rentalRequest.yesCount === 0;
}

export function searchRentalRequests(params) {
  store.dispatch({ type: Action.RENTAL_REQUESTS_REQUEST });
  return new ApiRequest('/rentalrequests/search').get(params).then((response) => {
    var rentalRequests = normalize(response.data);

    // Add display fields
    _.map(rentalRequests, (req) => {
      parseRentalRequest(req);
    });

    store.dispatch({
      type: Action.UPDATE_RENTAL_REQUESTS,
      rentalRequests: rentalRequests,
    });
  });
}

export function getRentalRequest(id) {
  return new ApiRequest(`/rentalrequests/${id}`).get().then((response) => {
    var rentalRequest = response.data;
    // Add display fields
    parseRentalRequest(rentalRequest);

    store.dispatch({
      type: Action.UPDATE_RENTAL_REQUEST,
      rentalRequest,
      rentalRequestId: id,
    });

    return rentalRequest;
  });
}

export function addRentalRequest(rentalRequest, viewOnly) {
  var path = viewOnly ? '/rentalrequests/viewOnly' : '/rentalrequests';

  return new ApiRequest(path).post(rentalRequest).then((response) => {
    var rentalRequest = response.data;
    // Add display fields
    parseRentalRequest(rentalRequest);
    store.dispatch({
      type: Action.ADD_RENTAL_REQUEST,
      rentalRequest,
      rentalRequestId: rentalRequest.id,
    });

    getEquipmentHires();

    return rentalRequest;
  });
}

export function updateRentalRequest(rentalRequest) {
  // remove properties that interfere with deserialization
  const rentalRequestId = rentalRequest.id;
  store.dispatch({
    type: Action.UPDATE_RENTAL_REQUEST,
    rentalRequest,
    rentalRequestId,
  });

  return new ApiRequest(`/rentalrequests/${rentalRequest.id}`)
    .put(_.omit(rentalRequest, 'primaryContact'))
    .then((response) => {
      var rentalRequest = response.data;
      // Add display fields
      parseRentalRequest(rentalRequest);

      store.dispatch({
        type: Action.UPDATE_RENTAL_REQUEST,
        rentalRequest,
        rentalRequestId,
      });
    });
}

export function addRentalRequestHistory(requestId, history) {
  return new ApiRequest(`/rentalrequests/${requestId}/history`).post(history).then((response) => {
    var history = normalize(response.data);
    // Add display fields
    _.map(history, (history) => {
      parseHistory(history);
    });

    store.dispatch({
      type: Action.UPDATE_RENTAL_REQUEST_HISTORY,
      history,
      id: requestId,
    });
  });
}

export function getRentalRequestHistory(requestId, params) {
  return new ApiRequest(`/rentalrequests/${requestId}/history`).get(params).then((response) => {
    var history = normalize(response.data);

    // Add display fields
    _.map(history, (history) => {
      parseHistory(history);
    });

    store.dispatch({
      type: Action.UPDATE_RENTAL_REQUEST_HISTORY,
      history,
      id: requestId,
    });
  });
}

export function getRentalRequestDocuments(rentalRequestId) {
  return new ApiRequest(`/rentalrequests/${rentalRequestId}/attachments`).get().then((response) => {
    var documents = normalize(response.data);

    // Add display fields
    _.map(documents, (document) => {
      parseDocument(document);
    });

    store.dispatch({ type: Action.UPDATE_DOCUMENTS, documents: documents });
  });
}

// XXX: Looks like this is unused
// export function addRentalRequestDocument(rentalRequestId, files) {
//   return new ApiRequest(`/rentalrequests/${ rentalRequestId }/attachments`).post(files);
// }

export function getRentalRequestNotes(rentalRequestId) {
  return new ApiRequest(`/rentalrequests/${rentalRequestId}/notes`).get().then((response) => {
    store.dispatch({
      type: Action.UPDATE_RENTAL_REQUEST_NOTES,
      notes: response.data,
      rentalRequestId,
    });
    return response.data;
  });
}

export function addRentalRequestNote(rentalRequestId, note) {
  return new ApiRequest(`/rentalRequests/${rentalRequestId}/note`).post(note).then((response) => {
    store.dispatch({
      type: Action.UPDATE_RENTAL_REQUEST_NOTES,
      notes: response.data,
      rentalRequestId,
    });
    return response.data;
  });
}

export function cancelRentalRequest(rentalRequestId) {
  return new ApiRequest(`/rentalrequests/${rentalRequestId}/cancel`).get().then(() => {
    getEquipmentHires();
  });
}

export function rentalRequestSeniorityList(rentalRequestId, counterCopy) {
  var params = {};
  if (counterCopy) {
    params.counterCopy = counterCopy;
  }

  return new ApiRequest(`/rentalrequests/${rentalRequestId}/senioritylist`)
    .get(params, { responseType: Constant.RESPONSE_TYPE_BLOB })
    .then((response) => {
      return response;
    });
}

////////////////////
// Rental Request Rotation List
////////////////////

function getSeniorityDisplayName(blockNumber, numberOfBlocks, seniority, numberInBlock) {
  if (blockNumber === numberOfBlocks) {
    return `Open-${seniority && seniority.toFixed(3)} (${numberInBlock})`;
  } else if (blockNumber === 1) {
    return `1-${seniority && seniority.toFixed(3)} (${numberInBlock})`;
  } else if (blockNumber === 2) {
    return `2-${seniority && seniority.toFixed(3)} (${numberInBlock})`;
  }
  return `Open-${seniority && seniority.toFixed(3)} (${numberInBlock})`;
}

function parseRentalRequestRotationList(rotationListItem, rentalRequest = {}) {
  if (!rotationListItem.rentalRequest) {
    rotationListItem.rentalRequest = _.extend({ id: 0 }, _.pick(rentalRequest, 'id'));
  }
  if (!rotationListItem.equipment) {
    rotationListItem.equipment = { id: 0, equipmentCode: '' };
  }
  if (!rotationListItem.equipment.districtEquipmentType) {
    rotationListItem.equipment.districtEquipmentType = {
      id: 0,
      districtEquipmentName: '',
    };
  }
  if (!rotationListItem.equipment.owner) {
    rotationListItem.equipment.owner = { id: 0, organizationName: '' };
  }

  // The rental agreement (if any) created for an accepted hire offer.
  rotationListItem.rentalAgreement = rotationListItem.rentalAgreement || null;

  // The sort order of the piece of equipment on the rotaton list at the time the request was created.
  // This is the order the equipment will be offered the available work.
  rotationListItem.rotationListSortOrder = rotationListItem.rotationListSortOrder || 0;

  rotationListItem.isForceHire = rotationListItem.isForceHire || false;
  rotationListItem.wasAsked = rotationListItem.wasAsked || false;
  rotationListItem.askedDateTime = rotationListItem.askedDateTime || '';
  rotationListItem.offerResponseDatetime = rotationListItem.offerResponseDatetime || '';
  rotationListItem.offerResponse = rotationListItem.offerResponse || '';
  rotationListItem.offerRefusalReason = rotationListItem.offerRefusalReason || '';
  rotationListItem.offerResponseNote = rotationListItem.offerResponseNote || '';
  rotationListItem.note = rotationListItem.note || '';

  var equipment = rotationListItem.equipment;

  // UI display fields
  rotationListItem.isHired = rotationListItem.isHired || false;
  rotationListItem.seniority = getSeniorityDisplayName(
    equipment.blockNumber,
    equipment.numberOfBlocks,
    equipment.seniority,
    equipment.numberInBlock
  );
  rotationListItem.serviceHoursThisYear = rotationListItem.serviceHoursThisYear || equipment.serviceHoursThisYear || 0; // TODO calculated field from the server
  rotationListItem.equipmentId = equipment.id;
  rotationListItem.equipmentCode = equipment.equipmentCode;

  // String format: "{year} {make}/{model}/{serialNumber}/{size}" - e.g. "1991 Bobcat/KOM450/442K00547/Medium"
  rotationListItem.equipmentDetails = concat(
    equipment.year,
    concat(equipment.make, concat(equipment.model, concat(equipment.serialNumber, equipment.size, '/'), '/'), '/'),
    ' '
  );

  // Primary contact for the owner of the piece of equipment
  rotationListItem.contact = rotationListItem.contact || (equipment.owner ? equipment.owner.primaryContact : null);
  rotationListItem.contactName = rotationListItem.contact
    ? firstLastName(rotationListItem.contact.givenName, rotationListItem.contact.surname)
    : '';
  rotationListItem.contactEmail = rotationListItem.contact ? rotationListItem.contact.emailAddress : '';
  rotationListItem.contactPhone = rotationListItem.contact
    ? rotationListItem.contact.workPhoneNumber || rotationListItem.contact.mobilePhoneNumber || ''
    : '';

  // TODO Status TBD
  rotationListItem.status = 'N/A';
}

function parseRotationListItem(item, numberOfBlocks, districtEquipmentType) {
  item.districtEquipmentType = districtEquipmentType;
  item.equipment = item.equipment || {};
  item.equipment = {
    ...item.equipment,
    historyEntity: History.makeHistoryEntity(Constant.HISTORY_EQUIPMENT, {
      ...item.equipment,
      name: item.equipment.equipmentCode,
      path: `${Constant.EQUIPMENT_PATHNAME}/${item.equipment.id}`,
      url: `${Constant.EQUIPMENT_PATHNAME}/${item.equipment.id}`,
    }),
  };

  item.displayFields = {};
  item.displayFields.equipmentDetails = concat(
    item.equipment.year,
    concat(
      item.equipment.make,
      concat(item.equipment.model, concat(item.equipment.serialNumber, item.equipment.size, '/'), '/'),
      '/'
    ),
    ' '
  );
  item.displayFields.seniority = getSeniorityDisplayName(
    item.equipment.blockNumber,
    numberOfBlocks,
    item.equipment.seniority,
    item.equipment.numberInBlock
  );

  var primaryContact = item.equipment.owner && item.equipment.owner.primaryContact;
  item.displayFields.primaryContactName = primaryContact
    ? firstLastName(primaryContact.givenName, primaryContact.surname)
    : '';
}

export function getRentalRequestRotationList(id) {
  return new ApiRequest(`/rentalrequests/${id}/rotationList`).get().then((response) => {
    const rentalRequest = response.data;
    const rotationList = rentalRequest.rentalRequestRotationList;

    rotationList.map((item) =>
      parseRotationListItem(item, rentalRequest.numberOfBlocks, rentalRequest.districtEquipmentType)
    );

    store.dispatch({
      type: Action.UPDATE_RENTAL_REQUEST_ROTATION_LIST,
      rotationList,
      rentalRequestId: id,
    });
  });
}

export function updateRentalRequestRotationList(rentalRequestRotationList, rentalRequest) {
  const rentalRequestId = rentalRequest.id;
  return new ApiRequest(`/rentalrequests/${rentalRequestId}/rentalRequestRotationList`)
    .put(rentalRequestRotationList)
    .then((response) => {
      var rotationList = response.data.rentalRequestRotationList;

      rotationList.map((item) =>
        parseRotationListItem(item, rentalRequest.numberOfBlocks, rentalRequest.districtEquipmentType)
      );

      store.dispatch({
        type: Action.UPDATE_RENTAL_REQUEST_ROTATION_LIST,
        rotationList,
        rentalRequestId,
      });

      getEquipmentTs();
      getEquipmentHires();
    });
}

////////////////////
// Rental Agreements
////////////////////

function parseRentalAgreement(agreement) {
  if (!agreement.district) {
    agreement.district = { id: 0, name: '' };
  }
  if (!agreement.equipment) {
    agreement.equipment = { id: 0, equipmentCode: '' };
  }
  if (!agreement.equipment.owner) {
    agreement.equipment.owner = { id: 0, organizationName: '' };
  }
  if (!agreement.equipment.districtEquipmentType) {
    agreement.equipment.districtEquipmentType = {
      id: 0,
      districtEquipmentName: '',
    };
  }
  if (!agreement.equipment.equipmentAttachments) {
    agreement.equipment.equipmentAttachments = [];
  }
  if (!agreement.equipment.localArea) {
    agreement.equipment.localArea = { id: 0, name: '' };
  }
  if (!agreement.equipment.localArea.serviceArea) {
    agreement.equipment.localArea.serviceArea = { id: 0, name: '' };
  }
  if (!agreement.equipment.localArea.serviceArea.district) {
    agreement.equipment.localArea.serviceArea.district = { id: 0, name: '' };
  }
  if (!agreement.equipment.localArea.serviceArea.district.region) {
    agreement.equipment.localArea.serviceArea.district.region = {
      id: 0,
      name: '',
    };
  }
  if (!agreement.project) {
    agreement.project = { id: 0, name: '' };
  }
  if (!agreement.rentalAgreementRates) {
    agreement.rentalAgreementRates = [];
  }
  if (!agreement.rentalAgreementConditions) {
    agreement.rentalAgreementConditions = [];
  }
  if (!agreement.timeRecords) {
    agreement.timeRecords = [];
  }

  agreement.path = `${Constant.RENTAL_AGREEMENTS_PATHNAME}/${agreement.id}`;
  agreement.url = `${agreement.path}`;

  agreement.number = agreement.number || '';
  agreement.note = agreement.note || '';
  agreement.datedOn = agreement.datedOn || today();
  agreement.equipmentRate = agreement.equipmentRate || 0.0;
  agreement.ratePeriod = agreement.ratePeriod || ''; // e.g. hourly, daily, etc.
  agreement.rateComment = agreement.rateComment || '';

  agreement.estimateStartWork = agreement.estimateStartWork || '';
  agreement.estimateHours = agreement.estimateHours || 0;

  agreement.rentalAgreementRates.forEach((obj) => parseRentalRate(obj, agreement));
  agreement.rentalAgreementConditions.forEach((obj) => parseRentalCondition(obj, agreement));

  agreement.equipment = {
    ...agreement.equipment,
    historyEntity: History.makeHistoryEntity(Constant.HISTORY_EQUIPMENT, {
      ...agreement.equipment,
      name: agreement.equipment.equipmentCode,
      path: `${Constant.EQUIPMENT_PATHNAME}/${agreement.equipment.id}`,
      url: `${Constant.EQUIPMENT_PATHNAME}/${agreement.equipment.id}`,
    }),
  };

  // UI display fields
  agreement.status = agreement.status || Constant.RENTAL_AGREEMENT_STATUS_CODE_ACTIVE; // TODO
  agreement.isActive = agreement.status === Constant.RENTAL_AGREEMENT_STATUS_CODE_ACTIVE;
  agreement.isCompleted = agreement.status === Constant.RENTAL_AGREEMENT_STATUS_CODE_COMPLETED;
  agreement.equipmentId = agreement.equipment.id;
  agreement.equipmentCode = agreement.equipment.equipmentCode;
  agreement.equipmentMake = agreement.equipment.make;
  agreement.equipmentModel = agreement.equipment.model;
  agreement.equipmentSize = agreement.equipment.size;
  agreement.equipmentTypeName = agreement.equipment.districtEquipmentType.districtEquipmentName;
  agreement.ownerId = agreement.equipment.owner.id || 0;
  agreement.ownerName = agreement.equipment.owner.organizationName || '';
  agreement.workSafeBCPolicyNumber = agreement.equipment.owner.workSafeBCPolicyNumber || '';
  agreement.pointOfHire = agreement.equipment.localArea.name || '';

  agreement.projectId = agreement.projectId || agreement.project.id;
  agreement.projectName = agreement.projectName || agreement.project.name;

  agreement.projectPath = agreement.projectId ? `${Constant.PROJECTS_PATHNAME}/${agreement.projectId}` : '';
  agreement.projectUrl = agreement.projectPath ? `${agreement.projectPath}` : '';

  agreement.canEdit = true;

  // Flag element as a rental agreement
  // Rental requests and rentals are merged and shown in a single list on Project Details screen
  agreement.isRentalAgreement = true;

  // TODO HETS-115 Server needs to send this
  agreement.lastTimeRecord = agreement.lastTimeRecord || '';
}

// reverse the transformations applied by the parse function
function convertRentalAgreement(agreement) {
  return {
    ...agreement,
    rentalAgreementConditions: _.values(agreement.rentalAgreementConditions),
    rentalAgreementRates: _.values(agreement.rentalAgreementRates),
    overtimeRates: _.values(agreement.overtimeRates),
    equipmentId: agreement.equipmentId || null,
    projectId: agreement.projectId || null,
  };
}

export function getRentalAgreementSummaryLite() {
  return new ApiRequest('/rentalagreements/summaryLite').get().then((response) => {
    var agreements = response.data;

    store.dispatch({
      type: Action.UPDATE_AGREEMENT_SUMMARY_LITE_LOOKUP,
      agreements,
    });
  });
}

export function getRentalAgreement(id) {
  return new ApiRequest(`/rentalagreements/${id}`).get().then((response) => {
    var agreement = response.data;

    // Add display fields
    parseRentalAgreement(agreement);

    store.dispatch({
      type: Action.UPDATE_RENTAL_AGREEMENT,
      rentalAgreement: agreement,
    });
  });
}

export function getLatestRentalAgreement(equipmentId, projectId) {
  return new ApiRequest(`/rentalagreements/latest/${projectId}/${equipmentId}`).get().then((response) => {
    var agreement = response.data;

    store.dispatch({
      type: Action.UPDATE_RENTAL_AGREEMENT,
      rentalAgreement: agreement,
    });

    return agreement;
  });
}

// XXX: Looks like this is unused
// export function addRentalAgreement(agreement) {
//   return new ApiRequest('/rentalagreements').post(agreement).then(response => {
//     var agreement = response.data;

//     // Add display fields
//     parseRentalAgreement(agreement);

//     store.dispatch({ type: Action.ADD_RENTAL_AGREEMENT, rentalAgreement: agreement });
//   });
// }

export function updateRentalAgreement(agreement) {
  var preparedAgreement = convertRentalAgreement(agreement);

  store.dispatch({
    type: Action.UPDATE_RENTAL_AGREEMENT,
    rentalAgreement: preparedAgreement,
  });

  return new ApiRequest(`/rentalagreements/${agreement.id}`).put(preparedAgreement).then((response) => {
    var agreement = response.data;

    // Add display fields
    parseRentalAgreement(agreement);

    store.dispatch({
      type: Action.UPDATE_RENTAL_AGREEMENT,
      rentalAgreement: agreement,
    });
  });
}

export function getRentalAgreementTimeRecords(rentalAgreementId) {
  return new ApiRequest(`/rentalagreements/${rentalAgreementId}/timeRecords`).get().then((response) => {
    var rentalAgreementTimeRecords = response.data;

    store.dispatch({
      type: Action.RENTAL_AGREEMENT_TIME_RECORDS,
      rentalAgreementTimeRecords: rentalAgreementTimeRecords,
    });
  });
}

export function addRentalAgreementTimeRecords(rentalRequestId, timeRecords) {
  let formattedTimeRecords = formatTimeRecords(timeRecords, rentalRequestId);
  return new ApiRequest(`/rentalagreements/${rentalRequestId}/timeRecords`)
    .post(formattedTimeRecords)
    .then((response) => {
      var rentalAgreementTimeRecords = normalize(response.data.timeRecords);

      store.dispatch({
        type: Action.RENTAL_AGREEMENT_TIME_RECORDS,
        rentalAgreementTimeRecords: rentalAgreementTimeRecords,
      });
      return rentalAgreementTimeRecords;
    });
}

export function releaseRentalAgreement(rentalAgreementId) {
  return new ApiRequest(`/rentalagreements/${rentalAgreementId}/release`).post().then((response) => {
    return response;
  });
}

export function generateRentalAgreementDocument(rentalAgreementId) {
  return new ApiRequest(`/rentalagreements/${rentalAgreementId}/doc`);
}

export function searchAitReport(params) {
  store.dispatch({ type: Action.AIT_REPORT_REQUEST });
  return new ApiRequest('/rentalAgreements/aitReport').get(params).then((response) => {
    var aitResponses = normalize(response.data);
    store.dispatch({
      type: Action.UPDATE_AIT_REPORT,
      aitResponses: aitResponses,
    });
  });
}

////////////////////
// Rental Rates
////////////////////

function parseRentalRate(rentalRate, parent = {}) {
  // Pick only the properties that we need
  if (!rentalRate.rentalAgreement) {
    rentalRate.rentalAgreement = _.extend(
      { id: 0, equipmentRate: 0 },
      _.pick(parent, 'id', 'number', 'path', 'equipmentRate')
    );
  }
  if (!rentalRate.timeRecords) {
    rentalRate.timeRecords = [];
  }

  rentalRate.path = rentalRate.rentalAgreement.path
    ? `${rentalRate.rentalAgreement.path}/${Constant.RENTAL_RATES_PATHNAME}/${rentalRate.id}`
    : null;
  rentalRate.url = rentalRate.path ? `${rentalRate.path}` : null;

  rentalRate.rate = rentalRate.rate || 0.0;
  rentalRate.percentOfEquipmentRate = rentalRate.percentOfEquipmentRate || 0;
  rentalRate.ratePeriod = rentalRate.ratePeriod || rentalRate?.ratePeriodType?.ratePeriodTypeCode;
  rentalRate.comment = rentalRate.comment || '';

  // UI display fields
  rentalRate.rentalAgreementId = rentalRate.rentalAgreement.id;
  rentalRate.rentalAgreementNumber = rentalRate.rentalAgreement.number;

  rentalRate.canEdit = true;
  rentalRate.canDelete = true;
}

// XXX: Looks like this is unused
// export function getRentalRate(id) {
//   return new ApiRequest(`/rentalagreementrates/${ id }`).get().then(response => {
//     var rentalRate = response.data;

//     // Add display fields
//     parseRentalRate(rentalRate);

//     store.dispatch({ type: Action.UPDATE_RENTAL_RATE, rentalRate: rentalRate });
//   });
// }

// export function addRentalRate(rentalRate) {
//   return new ApiRequest('/rentalagreementrates').post({ ...rentalRate, rentalAgreement: { id: rentalRate.rentalAgreement.id } }).then(response => {
//     var rentalRate = response.data;

//     // Add display fields
//     parseRentalRate(rentalRate);

//     store.dispatch({ type: Action.ADD_RENTAL_RATE, rentalRate });
//   });
// }

export function addRentalRates(rentalAgreementId, rentalRates) {
  store.dispatch({
    type: Action.ADD_RENTAL_RATES,
    rentalRates,
    rentalAgreementId,
  });

  return new ApiRequest(`/rentalagreements/${rentalAgreementId}/rateRecords`).post(rentalRates).then((response) => {
    const data = _.find(response.data, { rentalAgreementId });
    var rentalRates = data.rentalAgreement.rentalAgreementRates;

    // Add display fields
    rentalRates.forEach((rentalRate) => parseRentalRate(rentalRate, data.rentalAgreement));

    store.dispatch({
      type: Action.UPDATE_RENTAL_RATES,
      rentalRates,
      rentalAgreementId,
    });

    return rentalRates;
  });
}

export function updateRentalRate(rentalRate) {
  const rentalAgreementId = rentalRate.rentalAgreement.id;
  store.dispatch({
    type: Action.UPDATE_RENTAL_RATES,
    rentalRates: [rentalRate],
    rentalAgreementId,
  });

  return new ApiRequest(`/rentalagreementrates/${rentalRate.id}`).put(rentalRate).then((response) => {
    var rentalRate = response.data;

    // Add display fields
    parseRentalRate(rentalRate);

    store.dispatch({
      type: Action.UPDATE_RENTAL_RATES,
      rentalRates: [rentalRate],
      rentalAgreementId,
    });

    return rentalRate;
  });
}

export function deleteRentalRate(rentalRate) {
  const rentalAgreementId = rentalRate.rentalAgreement.id;
  store.dispatch({
    type: Action.DELETE_RENTAL_RATE,
    rentalRate,
    rentalAgreementId,
  });

  return new ApiRequest(`/rentalagreementrates/${rentalRate.id}/delete`).post().then((response) => {
    const rentalRate = response.data;

    // Add display fields
    parseRentalRate(rentalRate);

    store.dispatch({
      type: Action.DELETE_RENTAL_RATE,
      rentalRate,
      rentalAgreementId,
    });

    return rentalRate;
  });
}

////////////////////
// Rental Conditions
////////////////////

function parseRentalCondition(rentalCondition, parent = {}) {
  // Pick only the properties that we need
  if (!rentalCondition.rentalAgreement) {
    rentalCondition.rentalAgreement = _.extend({ id: 0 }, _.pick(parent, 'id', 'number', 'path'));
  }

  rentalCondition.conditionName = rentalCondition.conditionName || '';
  rentalCondition.comment = rentalCondition.comment || '';

  // UI display fields
  rentalCondition.rentalAgreementId = rentalCondition.rentalAgreement.id;
  rentalCondition.rentalAgreementNumber = rentalCondition.rentalAgreement.number;
  rentalCondition.path = rentalCondition.rentalAgreement.path
    ? `${rentalCondition.rentalAgreement.path}/${Constant.RENTAL_CONDITIONS_PATHNAME}/${rentalCondition.id}`
    : null;
  rentalCondition.url = rentalCondition.path ? `${rentalCondition.path}` : null;

  rentalCondition.canEdit = true;
  rentalCondition.canDelete = true;
}

// XXX: Looks like this is unused
// export function getRentalCondition(id) {
//   return new ApiRequest(`/rentalagreementconditions/${ id }`).get().then(response => {
//     var rentalCondition = response.data;

//     // Add display fields
//     parseRentalCondition(rentalCondition);

//     store.dispatch({ type: Action.UPDATE_RENTAL_CONDITION, rentalCondition: rentalCondition });
//   });
// }

// XXX: Looks like this is unused
// export function addRentalCondition(rentalCondition) {
//   return new ApiRequest('/rentalagreementconditions').post({ ...rentalCondition, rentalAgreement: { id: rentalCondition.rentalAgreement.id } }).then(response => {
//     var rentalCondition = response.data;

//     // Add display fields
//     parseRentalCondition(rentalCondition);

//     store.dispatch({ type: Action.ADD_RENTAL_CONDITION, rentalCondition: rentalCondition });
//   });
// }

export function addRentalConditions(rentalAgreementId, rentalConditions) {
  store.dispatch({
    type: Action.ADD_RENTAL_CONDITIONS,
    rentalConditions,
    rentalAgreementId,
  });

  return new ApiRequest(`/rentalagreements/${rentalAgreementId}/conditionRecords`)
    .post(rentalConditions)
    .then((response) => {
      const data = _.find(response.data, { rentalAgreementId });
      var rentalConditions = data.rentalAgreement.rentalAgreementConditions;

      // Add display fields
      rentalConditions.forEach((rentalCondition) => parseRentalCondition(rentalCondition, data.rentalAgreement));

      store.dispatch({
        type: Action.UPDATE_RENTAL_CONDITIONS,
        rentalConditions,
        rentalAgreementId,
      });

      return rentalConditions;
    });
}

export function updateRentalCondition(rentalCondition) {
  const rentalAgreementId = rentalCondition.rentalAgreement.id;
  store.dispatch({
    type: Action.UPDATE_RENTAL_CONDITIONS,
    rentalConditions: [rentalCondition],
    rentalAgreementId,
  });

  return new ApiRequest(`/rentalagreementconditions/${rentalCondition.id}`).put(rentalCondition).then((response) => {
    var rentalCondition = response.data;

    // Add display fields
    parseRentalCondition(rentalCondition);

    store.dispatch({
      type: Action.UPDATE_RENTAL_CONDITIONS,
      rentalConditions: [rentalCondition],
      rentalAgreementId,
    });

    return rentalCondition;
  });
}

export function deleteRentalCondition(rentalCondition) {
  const rentalAgreementId = rentalCondition.rentalAgreement.id;
  store.dispatch({
    type: Action.DELETE_RENTAL_CONDITION,
    rentalCondition,
    rentalAgreementId,
  });

  return new ApiRequest(`/rentalagreementconditions/${rentalCondition.id}/delete`).post().then((response) => {
    var rentalCondition = response.data;

    // Add display fields
    parseRentalCondition(rentalCondition);

    store.dispatch({
      type: Action.DELETE_RENTAL_CONDITION,
      rentalCondition,
      rentalAgreementId,
    });

    return rentalCondition;
  });
}

export function deleteCondition(id) {
  return new ApiRequest(`/conditiontypes/${id}/delete`).post().then((response) => {
    return response;
  });
}

export function addCondition(condition) {
  return new ApiRequest('/conditiontypes/0').post(condition).then((response) => {
    return response;
  });
}

export function updateCondition(condition) {
  return new ApiRequest(`/conditiontypes/${condition.id}`).post(condition).then((response) => {
    return response;
  });
}

////////////////////
// Business
////////////////////

export function getBusiness() {
  return new ApiRequest('/business').get().then((response) => {
    var business = response.data;

    if (!_.isObject(business)) {
      business = {};
    }

    _.map(business.owners, (owner) => {
      parseOwner(owner);
    });
    store.dispatch({ type: Action.UPDATE_BUSINESS, business: business });
  });
}

export function getOwnerForBusiness(ownerId) {
  return new ApiRequest(`/business/owner/${ownerId}`).get().then((response) => {
    var owner = response.data;

    parseOwner(owner);
    store.dispatch({ type: Action.UPDATE_OWNER, owner: owner });
  });
}

export function validateOwner(secretKey, postalCode) {
  return new ApiRequest('/business/validateOwner')
    .get({ sharedKey: secretKey, postalCode: postalCode })
    .then((response) => {
      var business = response.data;
      parseOwner(business.linkedOwner);
      store.dispatch({ type: Action.UPDATE_BUSINESS, business: business });
    });
}

////////////////////
// Rollovers
////////////////////

function parseRolloverStatus(status) {
  status.rolloverActive =
    status.progressPercentage != null && status.progressPercentage >= 0 && status.progressPercentage < 100;
  status.rolloverComplete = status.progressPercentage === 100;
}

export function getRolloverStatus(districtId) {
  return new ApiRequest(`/districts/${districtId}/rolloverStatus`).get(null, { silent: true }).then((response) => {
    var status = response.data;
    parseRolloverStatus(status);
    store.dispatch({
      type: Action.UPDATE_ROLLOVER_STATUS_LOOKUP,
      status: status,
    });
  });
}

export function initiateRollover(districtId) {
  return new ApiRequest(`/districts/${districtId}/annualRollover`).get().then((response) => {
    var status = response;
    parseRolloverStatus(status);
    store.dispatch({
      type: Action.UPDATE_ROLLOVER_STATUS_LOOKUP,
      status: status,
    });
  });
}

export function dismissRolloverMessage(districtId) {
  return new ApiRequest(`/districts/${districtId}/dismissRolloverMessage`).post().then((response) => {
    var status = response.data;
    parseRolloverStatus(status);
    store.dispatch({
      type: Action.UPDATE_ROLLOVER_STATUS_LOOKUP,
      status: status,
    });
  });
}

////////////////////
// Look-ups
////////////////////

// XXX: Looks like this is unused
// export function getCities() {
//   return new ApiRequest('/cities').get().then(response => {
//     var cities = normalize(response.data);

//     store.dispatch({ type: Action.UPDATE_CITIES_LOOKUP, cities: cities });
//   });
// }

export function getDistricts() {
  return new ApiRequest('/districts').get().then((response) => {
    var districts = normalize(response.data);

    store.dispatch({
      type: Action.UPDATE_DISTRICTS_LOOKUP,
      districts: districts,
    });
  });
}

export function getRegions() {
  return new ApiRequest('/regions').get().then((response) => {
    var regions = normalize(response.data);

    store.dispatch({ type: Action.UPDATE_REGIONS_LOOKUP, regions: regions });
  });
}

export function getLocalAreas(id) {
  return new ApiRequest(`/districts/${id}/localAreas`).get().then((response) => {
    var localAreas = normalize(response.data);
    _.map(localAreas, (area) => (area.name = `${area.serviceAreaId} - ${area.name}`));

    store.dispatch({
      type: Action.UPDATE_LOCAL_AREAS_LOOKUP,
      localAreas: localAreas,
    });
  });
}

export function getServiceAreas() {
  return new ApiRequest('/serviceareas').get().then((response) => {
    var serviceAreas = normalize(response.data);

    store.dispatch({
      type: Action.UPDATE_SERVICE_AREAS_LOOKUP,
      serviceAreas: serviceAreas,
    });
  });
}

export function getEquipmentTypes() {
  const silent = store.getState().lookups.equipmentTypes.loaded;
  return new ApiRequest('/equipmenttypes', { silent }).get().then((response) => {
    var equipmentTypes = _.mapValues(normalize(response.data), (x) => {
      x.blueBookSectionAndName = `${x.blueBookSection} - ${x.name}`;
      return x;
    });

    store.dispatch({
      type: Action.UPDATE_EQUIPMENT_TYPES_LOOKUP,
      equipmentTypes,
    });
  });
}

export function getDistrictEquipmentTypes() {
  const silent = store.getState().lookups.districtEquipmentTypes.loaded;
  return new ApiRequest('/districtequipmenttypes', { silent }).get().then((response) => {
    var districtEquipmentTypes = normalize(response.data);

    store.dispatch({
      type: Action.UPDATE_DISTRICT_EQUIPMENT_TYPES_LOOKUP,
      districtEquipmentTypes: districtEquipmentTypes,
    });
  });
}

export function getDistrictEquipmentTypesAgreementSummary() {
  const silent = store.getState().lookups.districtEquipmentTypesAgreementSummary.loaded;
  return new ApiRequest('/districtequipmenttypes/agreementSummary', { silent }).get().then((response) => {
    var districtEquipmentTypes = normalize(response.data);

    store.dispatch({
      type: Action.UPDATE_DISTRICT_EQUIPMENT_TYPES_AGREEMENT_SUMMARY_LOOKUP,
      districtEquipmentTypes,
    });
  });
}

export function getFiscalYears(districtId) {
  return new ApiRequest(`/districts/${districtId}/fiscalYears`).get().then((response) => {
    store.dispatch({
      type: Action.UPDATE_FISCAL_YEARS_LOOKUP,
      fiscalYears: response.data,
    });
  });
}

export function getOwnersLite() {
  const silent = store.getState().lookups.owners.lite.loaded;
  return new ApiRequest('/owners/lite', { silent }).get().then((response) => {
    var owners = normalize(response.data);
    store.dispatch({ type: Action.UPDATE_OWNERS_LITE_LOOKUP, owners: owners });
  });
}

export function getOwnersLiteHires() {
  const silent = store.getState().lookups.owners.hires.loaded;
  return new ApiRequest('/owners/liteHires', { silent }).get().then((response) => {
    var owners = normalize(response.data);
    store.dispatch({
      type: Action.UPDATE_OWNERS_LITE_HIRES_LOOKUP,
      owners: owners,
    });
  });
}

export function getOwnersLiteTs() {
  const silent = store.getState().lookups.owners.ts.loaded;
  return new ApiRequest('/owners/liteTs', { silent }).get().then((response) => {
    var owners = normalize(response.data);

    store.dispatch({
      type: Action.UPDATE_OWNERS_LITE_TS_LOOKUP,
      owners: owners,
    });
  });
}

export function addDistrictEquipmentType(equipment) {
  return new ApiRequest(`/districtequipmenttypes/${equipment.id}`).post(equipment).then((response) => {
    return response;
  });
}

export function updateDistrictEquipmentType(equipment) {
  return new ApiRequest(`/districtequipmenttypes/${equipment.id}`).post(equipment).then((response) => {
    return response;
  });
}

export function deleteDistrictEquipmentType(equipment) {
  return new ApiRequest(`/districtequipmenttypes/${equipment.id}/delete`).post().then((response) => {
    return response;
  });
}

export function getRoles() {
  return new ApiRequest('/roles').get().then((response) => {
    var roles = normalize(response.data);

    store.dispatch({ type: Action.UPDATE_ROLES_LOOKUP, roles: roles });
  });
}

export function getPermissions() {
  return new ApiRequest('/permissions').get().then((response) => {
    var permissions = normalize(response.data);

    store.dispatch({
      type: Action.UPDATE_PERMISSIONS_LOOKUP,
      permissions: permissions,
    });
  });
}

// XXX: Looks like this is unused
// export function getProvincialRateTypes() {
//   return new ApiRequest('/provincialratetypes').get().then(response => {
//     var rateTypeOther = {
//       id: 10000,
//       rateType: 'OTHER',
//       description: Constant.NON_STANDARD_CONDITION,
//       rate: null,
//       isPercentRate: false,
//       isRateEditable: true,
//       isIncludedInTotal: false,
//       isInTotalEditable: true,
//     };
//     var provincialRateTypes = [ ...response.data, rateTypeOther ];

//     store.dispatch({ type: Action.UPDATE_PROVINCIAL_RATE_TYPES_LOOKUP, provincialRateTypes: provincialRateTypes });
//   });
// }

export function getOvertimeRateTypes() {
  return new ApiRequest('/provincialratetypes/overtime').get().then((response) => {
    store.dispatch({
      type: Action.UPDATE_OVERTIME_RATE_TYPES_LOOKUP,
      overtimeRateTypes: response.data,
    });
  });
}

export function updateOvertimeRateType(rate) {
  return new ApiRequest(`/provincialratetypes/${rate.id}`).put(rate).then((response) => {
    return response;
  });
}

export function getRentalConditions() {
  store.dispatch({ type: Action.RENTAL_CONDITIONS_LOOKUP_REQUEST });
  return new ApiRequest('/conditiontypes').get().then((response) => {
    var rentalConditions = response.data;

    store.dispatch({
      type: Action.UPDATE_RENTAL_CONDITIONS_LOOKUP,
      rentalConditions: rentalConditions,
    });
  });
}

////////////////////
// Version
////////////////////

export function getVersion() {
  return new ApiRequest('/version').get().then((response) => {
    store.dispatch({ type: Action.UPDATE_VERSION, version: response });
  });
}

////////////////////
// Notes
////////////////////

export function deleteNote(id) {
  store.dispatch({ type: Action.DELETE_NOTE, noteId: id });
  return new ApiRequest(`/notes/${id}/delete`).post().then((response) => {
    return response.data;
  });
}

export function updateNote(note) {
  return new ApiRequest(`/notes/${note.id}`).put(note).then((response) => {
    return response.data;
  });
}

////////////////////
// Set User
////////////////////

export function setDevUser(user) {
  return new ApiRequest(`/authentication/dev/token/${user}`).get().then((response) => {
    return response;
  });
}

////////////////////
// Time Records
////////////////////

export function deleteTimeRecord(timeRecordId) {
  return new ApiRequest(`/timerecords/${timeRecordId}/delete`).post().then((response) => {
    store.dispatch({
      type: Action.DELETE_TIME_RECORD,
      timeRecord: response.data,
    });
  });
}
