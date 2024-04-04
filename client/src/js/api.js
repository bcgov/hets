import * as Action from './actionTypes';
import * as Constant from './constants';
import * as History from './history';
import * as Log from './history';

import { ApiRequest } from './utils/http';
import { lastFirstName, firstLastName, concat } from './utils/string';
import { daysAgo, sortableDateTime, today } from './utils/date';

import _ from 'lodash';
import Moment from 'moment';

const normalize = (response) => _.fromPairs(response.map((object) => [object.id, object]));

////////////////////
// Users
////////////////////

const parseUser = (user) => {
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
};

export const getCurrentUser = () => async (dispatch) => {
  const response = await dispatch(new ApiRequest('/users/current').get());
  let user = response.data;
  
  // Add display fields
  parseUser(user);

  // Get permissions
  const permissions = [];
  _.each(user.userRoles, (userRole) => {
    _.each(userRole.role.rolePermissions, (rolePermission) => {
      permissions.push(rolePermission.permission.code);
    });
  });
  user.permissions = _.uniq(permissions);
  user.hasPermission = (permission) => user.permissions.indexOf(permission) !== -1;

  dispatch({ type: Action.UPDATE_CURRENT_USER, user });
  return user;
};

export const searchUsers = (params) => async (dispatch) => {
  dispatch({ type: Action.USERS_REQUEST });
  const response = await dispatch(new ApiRequest('/users/search').get(params));
  let users = normalize(response.data);

  // Add display fields
  _.map(users, (user) => {
    parseUser(user);
  });

  dispatch({ type: Action.UPDATE_USERS, users });
};

export const getUsers = () => async (dispatch) => {
  const response = await dispatch(new ApiRequest('/users').get());
  let users = normalize(response.data);

  // Add display fields
  _.map(users, (user) => {
    parseUser(user);
  });

  dispatch({ type: Action.UPDATE_USERS_LOOKUP, users });
};

export const getUser = (userId) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/users/${userId}`).get());
  let user = response.data;

  // Add display fields
  parseUser(user);

  dispatch({ type: Action.UPDATE_USER, user });
};

export const addUser = (user) => async (dispatch) => {
  const response = await dispatch(new ApiRequest('/users').post(user));
  let userResponse = response.data;

  // Add display fields
  parseUser(userResponse);

  dispatch({ type: Action.ADD_USER, user: userResponse });

  return userResponse;
};

export const updateUser = (user) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/users/${user.id}`).put(user));
  let userResponse = response.data;

  // Add display fields
  parseUser(userResponse);

  dispatch({ type: Action.UPDATE_USER, user: userResponse });

  return userResponse;
};

export const deleteUser = (user) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/users/${user.id}/delete`).post());
  let userResponse = response.data;

  // Add display fields
  parseUser(userResponse);

  dispatch({ type: Action.DELETE_USER, user: userResponse });
};

export const addUserRole = (userId, userRole) => async (dispatch) => {
  await dispatch(new ApiRequest(`/users/${userId}/roles`).post(userRole));
  // After updating the user's role, refresh the user state.
  dispatch(getUser(userId));
};

export const updateUserRoles = (userId, userRoleArray) => async (dispatch) => {
  await dispatch(new ApiRequest(`/users/${userId}/roles`).put(userRoleArray));
  // After updating the user's role, refresh the user state.
  dispatch(getUser(userId));
};

export const getCurrentUserDistricts = () => async (dispatch) => {
  const response = await dispatch(new ApiRequest('/userdistricts').get());
  dispatch({
    type: Action.CURRENT_USER_DISTRICTS,
    currentUserDistricts: response.data,
  });
  return response;
};

export const getUserDistricts = (userId) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/users/${userId}/districts`).get());
  dispatch({
    type: Action.USER_DISTRICTS,
    userDistricts: response.data,
  });
  return response;
};

export const addUserDistrict = (district) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/userdistricts`).put(district));
  dispatch({
    type: Action.USER_DISTRICTS,
    userDistricts: response.data,
  });
  return response;
};

export const editUserDistrict = (district) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/userdistricts/${district.id}`).post(district));
  dispatch({
    type: Action.USER_DISTRICTS,
    userDistricts: response.data,
  });
  return response;
};

export const deleteUserDistrict = (district) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/userdistricts/${district.id}/delete`).post());
  dispatch({
    type: Action.USER_DISTRICTS,
    userDistricts: response.data,
  });
  return response;
};

export const switchUserDistrict = (districtId) => async (dispatch) => {
  return await dispatch(new ApiRequest(`/userdistricts/${districtId}/switch`).post());
};

export const getSearchSummaryCounts = () => async (dispatch) => {
  const response = await dispatch(new ApiRequest('/counts').get());
  dispatch({
    type: Action.UPDATE_SEARCH_SUMMARY_COUNTS,
    searchSummaryCounts: response.data,
  });
  return response;
};

////////////////////
// Roles,  Permissions
////////////////////

const parseRole = (role) => {
  role.path = `${Constant.ROLES_PATHNAME}/${role.id}`;
  role.url = `${role.path}`;
  role.historyEntity = History.makeHistoryEntity(Constant.HISTORY_ROLE, role);

  role.canEdit = true;
  role.canDelete = false;
};

export const searchRoles = (params) => async (dispatch) => {
  const response = await dispatch(new ApiRequest('/roles').get(params));
  let roles = normalize(response.data);

  // Add display fields
  _.map(roles, (role) => {
    parseRole(role);
  });

  dispatch({ type: Action.UPDATE_ROLES, roles });
};

export const getRole = (roleId) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/roles/${roleId}`).get());
  let role = response.data;

  // Add display fields
  parseRole(role);

  dispatch({ type: Action.UPDATE_ROLE, role });
};

export const addRole = (role) => async (dispatch) => {
  const response = await dispatch(new ApiRequest('/roles').post(role));
  let roleResponse = response.data;

  // Add display fields
  parseRole(roleResponse);

  dispatch({ type: Action.ADD_ROLE, role: roleResponse });
};

export const updateRole = (role) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/roles/${role.id}`).put(role));
  let roleResponse = response.data;

  // Add display fields
  parseRole(roleResponse);

  dispatch({ type: Action.UPDATE_ROLE, role: roleResponse });
};

export const deleteRole = (role) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/roles/${role.id}/delete`).post());
  let roleResponse = response.data;

  // Add display fields
  parseRole(roleResponse);

  dispatch({ type: Action.DELETE_ROLE, role: roleResponse });
};

export const getRolePermissions = (roleId) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/roles/${roleId}/permissions`).get());
  dispatch({
    type: Action.UPDATE_ROLE_PERMISSIONS,
    rolePermissions: normalize(response.data),
  });
};

export const updateRolePermissions = (roleId, permissionsArray) => async (dispatch) => {
  await dispatch(new ApiRequest(`/roles/${roleId}/permissions`).put(permissionsArray));
  // After updating the role's permissions, refresh the permissions state.
  dispatch(getRolePermissions(roleId));
};

////////////////////
// Favourites
////////////////////

export const getFavourites = () => async (dispatch) => {
  const response = await dispatch(new ApiRequest('/users/current/favourites').get());
  const favourites = _.chain(response.data)
    .groupBy('type')
    .mapValues((type) =>
      _.chain(type)
        .values()
        .map((object) => [object.id, object])
        .fromPairs()
        .value()
    )
    .value();

  dispatch({ type: Action.UPDATE_FAVOURITES, favourites });
};

export const addFavourite = (favourite) => async (dispatch) => {
  const response = await dispatch(new ApiRequest('/users/current/favourites').post(favourite));
  dispatch({ type: Action.ADD_FAVOURITE, favourite: response.data });
};

export const updateFavourite = (favourite) => async (dispatch) => {
  const response = await dispatch(new ApiRequest('/users/current/favourites').put(favourite));
  dispatch({
    type: Action.UPDATE_FAVOURITE,
    favourite: response.data,
  });
};

export const deleteFavourite = (favourite) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/users/current/favourites/${favourite.id}/delete`).post());
  dispatch({
    type: Action.DELETE_FAVOURITE,
    favourite: response.data,
  });
};

////////////////////
// Equipment
////////////////////
const getBlockDisplayName = (blockNumber, numberOfBlocks, seniority) => {
  if (blockNumber === numberOfBlocks) {
    return `Open - ${seniority}`;
  }
  if (blockNumber === 1) {
    return `1 - ${seniority}`;
  }
  if (blockNumber === 2) {
    return `2 - ${seniority}`;
  }
  if (seniority != null) {
    return `Open - ${seniority}`;
  }
  return 'Open';
};

const parseEquipment = (equipment) => (dispatch) => {
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
  equipment.documentAdded = (e, d) => dispatch(Log.equipmentDocumentAdded(e, d));
  equipment.documentsAdded = (e) => dispatch(Log.equipmentDocumentsAdded(e));
  equipment.documentDeleted = (e, d) => dispatch(Log.equipmentDocumentDeleted(e, d));

  equipment.getDocumentsPromise = (equipmentId) => dispatch(getEquipmentDocuments(equipmentId));
  equipment.uploadDocumentPath = `/equipment/${equipment.id}/attachments`;

  equipment.canView = true;
  equipment.canEdit = true;
  equipment.canDelete = false; // TODO Needs input from Business whether this is needed.
};

const generateSortableEquipmentCode = (equipment) => {
  return equipment.equipmentPrefix && equipment.equipmentNumber
    ? `${equipment.equipmentPrefix}${_.padStart(equipment.equipmentNumber, 3, '0')}`
    : equipment.equipmentCode;
};

export const searchEquipmentList = (params) => async (dispatch) => {
  dispatch({ type: Action.EQUIPMENT_LIST_REQUEST });
  const response = await dispatch(new ApiRequest('/equipment/search').get(params));
  let equipmentList = normalize(response.data);

  _.map(equipmentList, (equipment) => {
    equipment.details = [
      equipment.make || '-',
      equipment.model || '-',
      equipment.size || '-',
      equipment.year || '-',
    ].join('/');
    equipment.sortableEquipmentCode = generateSortableEquipmentCode(equipment);
  });

  dispatch({ type: Action.UPDATE_EQUIPMENT_LIST, equipmentList });
};

export const getEquipment = (equipmentId) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/equipment/${equipmentId}`).get());
  let equipment = response.data;

  // Add display fields
  dispatch(parseEquipment(equipment));

  dispatch({ type: Action.UPDATE_EQUIPMENT, equipment });
};

export const getEquipmentLite = () => async (dispatch, getState) => {
  const silent = getState().lookups.equipment.lite.loaded;
  const response = await dispatch(new ApiRequest('/equipment/lite', { silent }).get());
  dispatch({
    type: Action.UPDATE_EQUIPMENT_LITE_LOOKUP,
    equipment: normalize(response.data),
  });
};

export const getEquipmentAgreementSummary = () => async (dispatch, getState) => {
  const silent = getState().lookups.equipment.ts.loaded;
  const response = await dispatch(new ApiRequest('/equipment/agreementSummary', { silent }).get());
  dispatch({
    type: Action.UPDATE_EQUIPMENT_AGREEMENT_SUMMARY_LOOKUP,
    equipment: response.data,
  });
};

export const getEquipmentTs = () => async (dispatch, getState) => {
  const silent = getState().lookups.equipment.ts.loaded;
  const response = await dispatch(new ApiRequest('/equipment/liteTs', { silent }).get());
  dispatch({
    type: Action.UPDATE_EQUIPMENT_TS_LOOKUP,
    equipment: normalize(response.data),
  });
};

export const getEquipmentHires = () => async (dispatch, getState) => {
  const silent = getState().lookups.equipment.hires.loaded;
  const response = await dispatch(new ApiRequest('/equipment/liteHires', { silent }).get());
  dispatch({
    type: Action.UPDATE_EQUIPMENT_HIRES_LOOKUP,
    equipment: normalize(response.data),
  });
};

export const addEquipment = (equipment) => async (dispatch) => {
  const response = await dispatch(new ApiRequest('/equipment').post(equipment));
  let equipmentResponse = response.data;

  // Add display fields
  dispatch(parseEquipment(equipmentResponse));

  dispatch({ type: Action.UPDATE_EQUIPMENT, equipment: equipmentResponse });
  dispatch(getEquipmentLite());
  dispatch(getEquipmentTs());
  dispatch(getEquipmentHires());

  return equipmentResponse;
};

export const updateEquipment = (equipment) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/equipment/${equipment.id}`).put(equipment));
  let equipmentResponse = response.data;

  // Add display fields
  dispatch(parseEquipment(equipmentResponse));

  dispatch({ type: Action.UPDATE_EQUIPMENT, equipment: equipmentResponse });
  dispatch(getEquipmentLite());
  dispatch(getEquipmentTs());
  dispatch(getEquipmentHires()); 
};

export const verifyEquipmentActive = (id) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/equipment/${id}/verifyactive`).put(id));
  let equipment = response.data;

  // Add display fields
  dispatch(parseEquipment(equipment));

  dispatch({ type: Action.UPDATE_EQUIPMENT, equipment });
  dispatch(getEquipmentLite());
  dispatch(getEquipmentTs());
  dispatch(getEquipmentHires());
};

export const addEquipmentHistory = (equipmentId, history) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/equipment/${equipmentId}/history`).post(history));
  let historyResponse = normalize(response.data);
  // Add display fields
  _.map(historyResponse, (h) => {
    parseHistory(h);
  });

  dispatch({
    type: Action.UPDATE_EQUIPMENT_HISTORY,
    history: historyResponse,
    id: equipmentId,
  });
};

export const getEquipmentHistory = (equipmentId, params) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/equipment/${equipmentId}/history`).get(params));
  let history = normalize(response.data);

  // Add display fields
  _.map(history, (h) => {
    parseHistory(h);
  });

  dispatch({
    type: Action.UPDATE_EQUIPMENT_HISTORY,
    history,
    id: equipmentId,
  });
};

export const getEquipmentDocuments = (equipmentId) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/equipment/${equipmentId}/attachments`).get());
  let documents = normalize(response.data);

  // Add display fields
  _.map(documents, (document) => {
    parseDocument(document);
  });

  dispatch({ type: Action.UPDATE_DOCUMENTS, documents });
};

// XXX: Looks like this is unused
// export const addEquipmentDocument = (equipmentId, files) => async (dispatch) => {
//   return await dispatch(new ApiRequest(`/equipment/${ equipmentId }/attachments`).post(files));
// };

export const getEquipmentNotes = (equipmentId) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/equipment/${equipmentId}/notes`).get());
  dispatch({
    type: Action.UPDATE_EQUIPMENT_NOTES,
    notes: response.data,
  });
  return response.data;
};

export const addEquipmentNote = (equipmentId, note) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/equipment/${equipmentId}/note`).post(note));
  dispatch({
    type: Action.UPDATE_EQUIPMENT_NOTES,
    notes: response.data,
  });
  return response.data;
};

export const equipmentDuplicateCheck = (id, serialNumber) => async (dispatch) => {
  return await dispatch(new ApiRequest(`/equipment/${id}/duplicates/${serialNumber}`).get());
};

export const changeEquipmentStatus = (status) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/equipment/${status.id}/status`).put(status));
  let equipment = response.data;
  // Add display fields
  dispatch(parseEquipment(equipment));
  dispatch({ type: Action.UPDATE_EQUIPMENT, equipment });
  return response;
};

export const getEquipmentRentalAgreements = (equipmentId) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/equipment/${equipmentId}/rentalAgreements`).get());
  const rentalAgreements = normalize(response.data);
  dispatch({
    type: Action.UPDATE_EQUIPMENT_RENTAL_AGREEMENTS,
    rentalAgreements,
  });
  return rentalAgreements;
};

export const cloneEquipmentRentalAgreement = (data) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/equipment/${data.equipmentId}/rentalAgreementClone`).post(data));
  let agreement = response.data;
  // Add display fields
  parseRentalAgreement(agreement);
  dispatch({
    type: Action.UPDATE_RENTAL_AGREEMENT,
    rentalAgreement: agreement,
  });
  return response;
};

export const equipmentSeniorityListDoc = (localAreas, types, counterCopy) => async (dispatch) => {
  const params = { localareas: localAreas, types: types };
  if (counterCopy) {
    params.counterCopy = counterCopy;
  }

  return await dispatch(new ApiRequest('/equipment/seniorityListDoc')
    .get(params, { responseType: Constant.RESPONSE_TYPE_BLOB }));
};

////////////////////
// Physical Attachments
////////////////////

// Introduce later
// const parsePhysicalAttachment = (attachment) => {
//   if (!attachment.type) { attachment.type = { id: 0, code: '', description: ''}; }

//   attachment.typeName = attachment.type.description;
//   // TODO Add grace period logic to editing/deleting attachments
//   attachment.canEdit = true;
//   attachment.canDelete = true;
// };

// XXX: Looks like this is unused
// export const getPhysicalAttachment = (id) => async (dispatch) => {
//   const response = await dispatch(new ApiRequest(`/equipment/${id}/equipmentAttachments`).get());
//   dispatch({ type: Action.UPDATE_EQUIPMENT_ATTACHMENTS, physicalAttachments: response.data });
// };

// XXX: Looks like this is unused
// export const addPhysicalAttachment = (attachment) => async (dispatch) => {
//   const response = await dispatch(new ApiRequest('/equipmentAttachments').post(attachment));
//   dispatch({ type: Action.ADD_EQUIPMENT_ATTACHMENT, physicalAttachment: response.data });
// };

export const addPhysicalAttachments = (equipmentId, attachmentTypeNames) => async (dispatch) => {
  const attachments = attachmentTypeNames.map((typeName) => {
    // "concurrencyControlNumber": 0,
    return {
      typeName,
      description: '',
      equipmentId,
      equipment: { id: equipmentId },
    };
  });

  const response = await dispatch(new ApiRequest('/equipmentAttachments/bulk').post(attachments));
  dispatch({
    type: Action.ADD_EQUIPMENT_ATTACHMENTS,
    physicalAttachments: response.data,
  });
};

export const updatePhysicalAttachment = (attachment) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/equipmentAttachments/${attachment.id}`).put(attachment));
  dispatch({
    type: Action.UPDATE_EQUIPMENT_ATTACHMENT,
    physicalAttachment: response.data,
  });
};

export const deletePhysicalAttachment = (attachmentId) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/equipmentAttachments/${attachmentId}/delete`).post());
  dispatch({
    type: Action.DELETE_EQUIPMENT_ATTACHMENT,
    physicalAttachment: response.data,
  });
};

////////////////////
// Owners
////////////////////

const parseOwner = (owner) => (dispatch) => {
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
  owner.documentAdded = (o, d) => dispatch(Log.ownerDocumentAdded(o, d));
  owner.documentsAdded = (o) => dispatch(Log.ownerDocumentsAdded(o));
  owner.documentDeleted = (o, d) => dispatch(Log.ownerDocumentDeleted(o, d));

  // Add display fields for owner contacts
  owner.contacts = owner.contacts.map((contact) => parseContact(contact, owner));

  _.map(owner.documents, (document) => {
    parseDocument(document);
  });
  _.map(owner.equipmentList, (equipment) => {
    dispatch(parseEquipment(equipment));
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

  owner.getDocumentsPromise = (ownerId) => dispatch(getOwnerDocuments(ownerId));
  owner.uploadDocumentPath = `/owners/${owner.id}/attachments`;

  owner.canView = true;
  owner.canEdit = true;
  owner.canDelete = false; // TODO Needs input from Business whether this is needed.
};

export const searchOwners = (params) => async (dispatch) => {
  dispatch({ type: Action.OWNERS_REQUEST });
  const response = await dispatch(new ApiRequest('/owners/search').get(params));
  dispatch({ type: Action.UPDATE_OWNERS, owners: normalize(response.data) });
};

export const getOwner = (ownerId) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/owners/${ownerId}`).get());
  let owner = response.data;

  // Add display fields
  dispatch(parseOwner(owner));

  dispatch({ type: Action.UPDATE_OWNER, owner });

  return owner;
};

export const addOwner = (owner) => async (dispatch) => {
  const response = await dispatch(new ApiRequest('/owners').post(owner));
  let ownerResponse = response.data;

  // Add display fields
  dispatch(parseOwner(ownerResponse));

  dispatch({ type: Action.ADD_OWNER, owner: ownerResponse });

  return ownerResponse;
};

export const updateOwner = (owner) => async (dispatch) => {
  dispatch({ type: Action.UPDATE_OWNER, owner });

  // Omit `contacts` to ensure that the existing contacts don't mess up the PUT call
  const response = await dispatch(new ApiRequest(`/owners/${owner.id}`).put(_.omit(owner, 'contacts')));
  let ownerResponse = response.data;

  // Add display fields
  dispatch(parseOwner(ownerResponse));

  dispatch({ type: Action.UPDATE_OWNER, owner: ownerResponse });
};

// XXX: Looks like this is unused
// export const deleteOwner = (owner) => async (dispatch) => {
//   const response = await dispatch(new ApiRequest(`/owners/${ owner.id }/delete`).post());
//   let ownerResponse = response.data;

//   // Add display fields
//   dispatch(parseOwner(ownerResponse));

//   dispatch({ type: Action.DELETE_OWNER, owner: ownerResponse });
// };

export const saveOwnerContact = (owner, contact) => async (dispatch) => {
  const isNew = contact.id === 0;

  if (!isNew) {
    // don't update if this is a new contact - add after post() completes
    dispatch({
      type: Action.UPDATE_OWNER_CONTACT,
      ownerId: owner.id,
      contact,
    });
  }

  const response = await dispatch(new ApiRequest(`/owners/${owner.id}/contacts/${contact.isPrimary}`).post(contact));
  let updatedContact = response.data;

  // Add display fields
  parseContact(updatedContact, owner); // owner's primary contact could be outdated
  updatedContact.isPrimary = contact.isPrimary;

  if (isNew) {
    // add newly created contact to Redux store's contacts
    dispatch({
      type: Action.ADD_OWNER_CONTACT,
      ownerId: owner.id,
      contact: updatedContact,
    });
  } else {
    // Update Redux store's data with the server's data
    dispatch({
      type: Action.UPDATE_OWNER_CONTACT,
      ownerId: owner.id,
      contact: updatedContact,
    });
  }

  return updatedContact;
};

export const addOwnerHistory = (ownerId, history) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/owners/${ownerId}/history`).post(history));
  let historyResponse = normalize(response.data);
  // Add display fields
  _.map(historyResponse, (h) => {
    parseHistory(h);
  });

  dispatch({
    type: Action.UPDATE_OWNER_HISTORY,
    history: historyResponse,
    id: ownerId,
  });
};

export const getOwnerHistory = (ownerId, params) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/owners/${ownerId}/history`).get(params));
  let history = normalize(response.data);
  // Add display fields
  _.map(history, (h) => {
    parseHistory(h);
  });

  dispatch({
    type: Action.UPDATE_OWNER_HISTORY,
    history,
    id: ownerId,
  });
};

export const getOwnerDocuments = (ownerId) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/owners/${ownerId}/attachments`).get());
  let documents = normalize(response.data);

  // Add display fields
  _.map(documents, (document) => {
    parseDocument(document);
  });

  dispatch({ type: Action.UPDATE_DOCUMENTS, documents });
};

// XXX: Looks like this is unused
// export const addOwnerDocument = (ownerId, files) => async (dispatch) => {
//   return await dispatch(new ApiRequest(`/owners/${ ownerId }/attachments`).post(files));
// };

export const getOwnerEquipment = (ownerId) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/owners/${ownerId}/equipment`).get());
  let equipmentList = normalize(response.data);

  _.map(equipmentList, (equipment) => {
    equipment.details = [
      equipment.make || '-',
      equipment.model || '-',
      equipment.size || '-',
      equipment.year || '-',
    ].join('/');
  });

  dispatch({
    type: Action.UPDATE_OWNER_EQUIPMENT,
    equipment: equipmentList,
  });
};

export const updateOwnerEquipment = (owner, equipmentArray) => async (dispatch) => {
  return await dispatch(new ApiRequest(`/owners/${owner.id}/equipment`).put(equipmentArray));
};

export const getOwnerNotes = (ownerId) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/owners/${ownerId}/notes`).get());
  dispatch({
    type: Action.UPDATE_OWNER_NOTES,
    ownerId,
    notes: response.data,
  });
  return response.data;
};

export const addOwnerNote = (ownerId, note) => async (dispatch) => {
  dispatch({ type: Action.ADD_OWNER_NOTE, ownerId, note });
  const response = await dispatch(new ApiRequest(`/owners/${ownerId}/note`).post(note));
  return response.data;
};

// XXX: Looks like this is unused
// export const getOwnersByDistrict = (districtId) => async (dispatch) => {
//   const response = await dispatch(new ApiRequest(`/districts/${districtId}/owners`).get());
//   let owners = normalize(response.data);
//   // Add display fields
//   _.map(owners, owner => { dispatch(parseOwner(owner)); });
//   dispatch({ type: Action.UPDATE_OWNERS_LOOKUP, owners });
// };

export const changeOwnerStatus = (status) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/owners/${status.id}/status`).put(status));
  let owner = response.data;
  // Add display fields
  dispatch(parseOwner(owner));
  dispatch({ type: Action.UPDATE_OWNER, owner });
  return response;
};

export const getStatusLettersDoc = (params) => async (dispatch) => {
  return await dispatch(new ApiRequest('/owners/verificationDoc').post(params, {
    responseType: Constant.RESPONSE_TYPE_BLOB,
  }));
};

export const getMailingLabelsDoc = (params) => async (dispatch) => {
  return await dispatch(new ApiRequest('/owners/mailingLabelsDoc').post(params, {
    responseType: Constant.RESPONSE_TYPE_BLOB,
  }));
};

export const transferEquipment = (donorOwnerId, recipientOwnerId, equipment, includeSeniority) => async (dispatch) => {
  const response = await dispatch(
    new ApiRequest(`/owners/${donorOwnerId}/equipmentTransfer/${recipientOwnerId}/${includeSeniority}`)
      .post(equipment));
  
  dispatch(getEquipmentLite());
  dispatch(getEquipmentTs());
  dispatch(getEquipmentHires());

  return response;
};

////////////////////
// Contacts
////////////////////

const parseContact = (contact, parent) => {
  contact.name = firstLastName(contact.givenName, contact.surname);
  contact.phone = contact.workPhoneNumber
    ? `${contact.workPhoneNumber} (w)`
    : contact.mobilePhoneNumber
    ? `${contact.mobilePhoneNumber} (c)`
    : '';

  let parentPath = '';
  let primaryContactId = 0;
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
};

// XXX: Looks like this is unused
// export const getContacts = () => async (dispatch) => {
//   const response = await dispatch(new ApiRequest('/contacts').get());
//   let contacts = normalize(response.data);

//   // Add display fields
//   _.map(contacts, contact => { parseContact(contact); });

//   dispatch({ type: Action.UPDATE_CONTACTS, contacts });
// };

// XXX: Looks like this is unused
// export const getContact = (contactId) => async (dispatch) => {
//   const response = await dispatch(new ApiRequest(`/contacts/${ contactId }`).get());
//   let contact = response.data;

//   // Add display fields
//   parseContact(contact);

//   dispatch({ type: Action.UPDATE_CONTACT, contact });
// };

// XXX: Looks like this is unused
// export const addContact = (parent, contact) => async (dispatch) => {
//   const response = await dispatch(new ApiRequest('/contacts').post(contact));
//   let contact = response.data;

//   // Add display fields
//   parseContact(contact, parent);

//   dispatch({ type: Action.ADD_CONTACT, contact });
// };

// export const updateContact = (parent, contact) => async (dispatch) => {
//   const response = await dispatch(new ApiRequest(`/contacts/${ contact.id }`).put(contact));
//   let contact = response.data;

//   // Add display fields
//   parseContact(contact, parent);

//   dispatch({ type: Action.UPDATE_CONTACT, contact });
// };

export const deleteContact = (contact) => async (dispatch) => {
  dispatch({ type: Action.DELETE_CONTACT, contact });
  const response = await dispatch(new ApiRequest(`/contacts/${contact.id}/delete`).post());
  let contactResponse = response.data;

  // Add display fields
  parseContact(contactResponse);

  dispatch({ type: Action.DELETE_CONTACT, contact: contactResponse });
};

////////////////////
// Documents
////////////////////

const getFileSizeString = (fileSizeInBytes) => {
  const bytes = parseInt(fileSizeInBytes, 10) || 0;
  const kbytes = bytes >= 1024 ? bytes / 1024 : 0;
  const mbytes = kbytes >= 1024 ? kbytes / 1024 : 0;
  const gbytes = mbytes >= 1024 ? mbytes / 1024 : 0;

  const ceiling10 = (num) => {
    const adjusted = Math.ceil(num * 10) / 10;
    return adjusted.toFixed(1);
  };

  return gbytes
    ? `${ceiling10(gbytes)} GB`
    : mbytes
    ? `${ceiling10(mbytes)} MB`
    : kbytes
    ? `${Math.ceil(kbytes)} KB`
    : `${bytes} bytes`;
};

const parseDocument = (document) => {
  document.fileSizeDisplay = getFileSizeString(document.fileSize);
  document.timestampSort = sortableDateTime(document.lastUpdateTimestamp);
  document.name = document.fileName;

  document.canDelete = true;
  document.historyEntity = History.makeHistoryEntity(Constant.HISTORY_DOCUMENT, document);
};

export const deleteDocument = (document) => async (dispatch) => {
  return await dispatch(new ApiRequest(`/attachments/${document.id}/delete`).post());
};

export const getDownloadDocument = (document) => async (dispatch) => {
  return await dispatch(new ApiRequest(`/attachments/${document.id}/download`).getBlob());
};

export const getDownloadDocumentURL = (document) => {
  //XXX: Not used in the application. Last checked 17 Jun 2021.
  // Not an API call, per se, as it must be called from the browser window.
  return `${window.location.origin}${window.location.pathname}api/attachments/${document.id}/download`;
};

////////////////////
// History
////////////////////

const parseHistory = (history) => {
  history.timestampSort = sortableDateTime(history.lastUpdateTimestamp);
};

////////////////////
// Projects
////////////////////

const parseProject = (project) => (dispatch) => {
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
  project.documentAdded = (p, d) => dispatch(Log.projectDocumentAdded(p, d));
  project.documentsAdded = (p) => dispatch(Log.projectDocumentsAdded(p));
  project.documentDeleted = (p, d) => dispatch(Log.projectDocumentDeleted(p, d));

  // Add display fields for contacts
  project.contacts = project.contacts.map((contact) => parseContact(contact, project));

  // Add display fields for rental requests and rental agreements
  _.map(project.rentalRequests, (obj) => {
    dispatch(parseRentalRequest(obj));
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

  project.getDocumentsPromise = (projectId) => dispatch(getProjectDocuments(projectId));
  project.uploadDocumentPath = `/projects/${project.id}/attachments`;

  project.canView = true;
  project.canEdit = true;
  project.canDelete = false; // TODO Needs input from Business whether this is needed.
};

const formatTimeRecords = (timeRecords, rentalRequestId) => {
  let formattedTimeRecords = Object.keys(timeRecords).map((key) => {
    let timeRecord = {};
    timeRecord.workedDate = timeRecords[key].date;
    timeRecord.hours = timeRecords[key].hours;
    timeRecord.timePeriod = 'Week';
    timeRecord.rentalAgreement = { id: rentalRequestId };
    return timeRecord;
  });
  return formattedTimeRecords;
};

export const searchProjects = (params) => async (dispatch) => {
  dispatch({ type: Action.PROJECTS_REQUEST });
  const response = await dispatch(new ApiRequest('/projects/search').get(params));
  let projects = normalize(response.data);

  // Add display fields
  _.map(projects, (project) => {
    dispatch(parseProject(project));
  });

  dispatch({ type: Action.UPDATE_PROJECTS, projects });
};

export const searchTimeEntries = (params) => async (dispatch) => {
  dispatch({ type: Action.TIME_ENTRIES_REQUEST });
  const response = await dispatch(new ApiRequest('/timeRecords/search').get(params));
  let timeEntries = normalize(response.data);

  _.map(timeEntries, (entry) => {
    entry.localAreaLabel = `${entry.serviceAreaId} - ${entry.localAreaName}`;
    entry.equipmentDetails = [entry.make || '-', entry.model || '-', entry.size || '-', entry.year || '-'].join('/');
    entry.sortableEquipmentCode = generateSortableEquipmentCode(entry);
  });

  dispatch({
    type: Action.UPDATE_TIME_ENTRIES,
    timeEntries,
  });
};

export const searchHiringReport = (params) => async (dispatch) => {
  dispatch({ type: Action.HIRING_RESPONSES_REQUEST });
  const response = await dispatch(new ApiRequest('/rentalRequests/hireReport').get(params));
  let hiringResponses = normalize(response.data);

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

  dispatch({
    type: Action.UPDATE_HIRING_RESPONSES,
    hiringResponses,
  });
};

export const searchOwnersCoverage = (params) => async (dispatch) => {
  dispatch({ type: Action.OWNERS_COVERAGE_REQUEST });
  const response = await dispatch(new ApiRequest('/owners/wcbCglReport').get(params));
  let ownersCoverage = normalize(response.data);

  _.map(ownersCoverage, (entry) => {
    entry.localAreaLabel = `${entry.serviceAreaId} - ${entry.localAreaName}`;
  });

  dispatch({
    type: Action.UPDATE_OWNERS_COVERAGE,
    ownersCoverage,
  });
};

export const getProjects = () => async (dispatch, getState) => {
  const silent = getState().lookups.projects.loaded;
  const response = await dispatch(new ApiRequest('/projects', { silent }).get({ currentFiscal: false }));
  let projects = normalize(response.data);

  // Add display fields
  _.map(projects, (project) => {
    dispatch(parseProject(project));
  });

  dispatch({
    type: Action.UPDATE_PROJECTS_LOOKUP,
    projects,
  });
};

export const getProjectsAgreementSummary = () => async (dispatch, getState) => {
  const silent = getState().lookups.projectsAgreementSummary.loaded;
  const response = await dispatch(new ApiRequest('/projects/agreementSummary', { silent }).get());
  dispatch({
    type: Action.UPDATE_PROJECTS_AGREEMENT_SUMMARY_LOOKUP,
    projects: response.data,
  });
};

export const getProjectsCurrentFiscal = () => async (dispatch, getState) => {
  const silent = getState().lookups.projectsCurrentFiscal.loaded;
  const response = await dispatch(new ApiRequest('/projects', { silent }).get({ currentFiscal: true }));
  let projects = normalize(response.data);

  // Add display fields
  _.map(projects, (project) => {
    dispatch(parseProject(project));
  });

  dispatch({
    type: Action.UPDATE_PROJECTS_CURRENT_FISCAL_LOOKUP,
    projects,
  });
};

export const getProject = (projectId) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/projects/${projectId}`).get());
  let project = response.data;

  // Add display fields
  dispatch(parseProject(project));

  dispatch({ type: Action.UPDATE_PROJECT, project });

  return project;
};

export const addProject = (project) => async (dispatch) => {
  const response = await dispatch(new ApiRequest('/projects').post(project));
  let projectResponse = response.data;

  // Add display fields
  dispatch(parseProject(projectResponse));

  dispatch({ type: Action.ADD_PROJECT, project: projectResponse });

  return projectResponse;
};

export const updateProject = (project) => async (dispatch) => {
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

  const response = await dispatch(new ApiRequest(`/projects/${project.id}`).put(projectData));
  let projectResponse = response.data;

  // Add display fields
  dispatch(parseProject(projectResponse));

  dispatch({ type: Action.UPDATE_PROJECT, project: projectResponse });
};

// XXX: Looks like this is unused
// export const getProjectEquipment = (projectId) => async (dispatch) => {
//   const response = await dispatch(new ApiRequest(`/projects/${projectId}/equipment`).get());
//   dispatch({ type: Action.UPDATE_PROJECT_EQUIPMENT, projectEquipment: normalize(response.data) });
// };

// XXX: Looks like this is unused
// export const getProjectTimeRecords = (projectId) => async (dispatch) => {
//   const response = await dispatch(new ApiRequest(`projects/${projectId}/timeRecords`).get());
//   dispatch({ type: Action.UPDATE_PROJECT_TIME_RECORDS, projectTimeRecords: normalize(response.data) });
// };

// XXX: Looks like this is unused
// export const addProjectTimeRecords = (projectId, rentalRequestId, timeRecords) => async (dispatch) => {
//   const formattedTimeRecords = formatTimeRecords(timeRecords, rentalRequestId);
//   const response = await dispatch(new ApiRequest(`projects/${projectId}/timeRecords`).post(formattedTimeRecords));
//   const projectTimeRecords = normalize(response.data);

//   dispatch({ type: Action.UPDATE_PROJECT_TIME_RECORDS, projectTimeRecords });
//   return projectTimeRecords;
// };

export const saveProjectContact = (project, contact) => async (dispatch) => {
  const isNew = contact.id === 0;

  if (!isNew) {
    // don't update if this is a new contact - add after post() completes
    dispatch({
      type: Action.UPDATE_PROJECT_CONTACT,
      projectId: project.id,
      contact,
    });
  }

  const response = await dispatch(new ApiRequest(`/projects/${project.id}/contacts/${contact.isPrimary}`).post(contact));
  let updatedContact = response.data;

  // Add display fields
  parseContact(updatedContact, project); // project's primary contact could be outdated
  updatedContact.isPrimary = contact.isPrimary;

  dispatch({
    type: isNew ? Action.ADD_PROJECT_CONTACT : Action.UPDATE_PROJECT_CONTACT,
    projectId: project.id,
    contact: updatedContact,
  });

  return updatedContact;
};

export const addProjectHistory = (projectId, history) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/projects/${projectId}/history`).post(history));
  let historyResponse = normalize(response.data);
  // Add display fields
  _.map(historyResponse, (h) => {
    parseHistory(h);
  });

  dispatch({
    type: Action.UPDATE_PROJECT_HISTORY,
    history: historyResponse,
    id: projectId,
  });
};

export const getProjectHistory = (projectId, params) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/projects/${projectId}/history`).get(params));
  let history = normalize(response.data);

  // Add display fields
  _.map(history, (h) => {
    parseHistory(h);
  });

  dispatch({
    type: Action.UPDATE_PROJECT_HISTORY,
    history,
    id: projectId,
  });
};

export const getProjectDocuments = (projectId) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/projects/${projectId}/attachments`).get());
  let documents = normalize(response.data);

  // Add display fields
  _.map(documents, (document) => {
    parseDocument(document);
  });

  dispatch({ type: Action.UPDATE_DOCUMENTS, documents });
};

// XXX: Looks like this is unused
// export const addProjectDocument = (projectId, files) => async (dispatch) => {
//   return await dispatch(new ApiRequest(`/projects/${ projectId }/attachments`).post(files));
// };

export const getProjectNotes = (projectId) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/projects/${projectId}/notes`).get());
  dispatch({
    type: Action.UPDATE_PROJECT_NOTES,
    projectId,
    notes: response.data,
  });
  return response.data;
};

export const addProjectNote = (projectId, note) => async (dispatch) => {
  dispatch({ type: Action.ADD_PROJECT_NOTE, projectId, note });
  return await dispatch(new ApiRequest(`/projects/${projectId}/note`).post(note));
};

export const getProjectRentalAgreements = (projectId) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/projects/${projectId}/rentalAgreements`).get());
  const rentalAgreements = normalize(response.data);
  dispatch({
    type: Action.UPDATE_PROJECT_RENTAL_AGREEMENTS,
    rentalAgreements,
  });
  return rentalAgreements;
};

export const cloneProjectRentalAgreement = (data) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/projects/${data.projectId}/rentalAgreementClone`).post(data));
  let agreement = response.data;
  // Add display fields
  parseRentalAgreement(agreement);
  dispatch({
    type: Action.UPDATE_RENTAL_AGREEMENT,
    rentalAgreement: agreement,
  });
  return response;
};

////////////////////
// Rental Requests
////////////////////

const parseRentalRequest = (rentalRequest) => (dispatch) => {
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
  rentalRequest.documentAdded = (r, d) => dispatch(Log.rentalRequestDocumentAdded(r, d));
  rentalRequest.documentsAdded = (r) => dispatch(Log.rentalRequestDocumentsAdded(r));
  rentalRequest.documentDeleted = (r, d) => dispatch(Log.rentalRequestDocumentDeleted(r, d));

  rentalRequest.getDocumentsPromise = (requestRentalId) => dispatch(getRentalRequestDocuments(requestRentalId));
  rentalRequest.uploadDocumentPath = `/rentalrequests/${rentalRequest.id}/attachments`;

  rentalRequest.canView = true;
  rentalRequest.canEdit = true;
  // HETS-894: view-only requests and requests that have yet to be acted on can be deleted
  rentalRequest.canDelete = rentalRequest.projectId === 0 || rentalRequest.yesCount === 0;
};

export const searchRentalRequests = (params) => async (dispatch) => {
  dispatch({ type: Action.RENTAL_REQUESTS_REQUEST });
  const response = await dispatch(new ApiRequest('/rentalrequests/search').get(params));
  let rentalRequests = normalize(response.data);

  // Add display fields
  _.map(rentalRequests, (req) => {
    dispatch(parseRentalRequest(req));
  });

  dispatch({
    type: Action.UPDATE_RENTAL_REQUESTS,
    rentalRequests,
  });
};

export const getRentalRequest = (id) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/rentalrequests/${id}`).get());
  let rentalRequest = response.data;
  // Add display fields
  dispatch(parseRentalRequest(rentalRequest));

  dispatch({
    type: Action.UPDATE_RENTAL_REQUEST,
    rentalRequest,
    rentalRequestId: id,
  });

  return rentalRequest;
};

export const addRentalRequest = (rentalRequest, viewOnly) => async (dispatch) => {
  const path = viewOnly ? '/rentalrequests/viewOnly' : '/rentalrequests';

  const response = await dispatch(new ApiRequest(path).post(rentalRequest));
  let rentalRequestResponse = response.data;
  // Add display fields
  dispatch(parseRentalRequest(rentalRequestResponse));
  dispatch({
    type: Action.ADD_RENTAL_REQUEST,
    rentalRequest: rentalRequestResponse,
    rentalRequestId: rentalRequestResponse.id,
  });

  dispatch(getEquipmentHires());
  rentalRequest.id = rentalRequestResponse.id;
  return rentalRequest;
};

export const updateRentalRequest = (rentalRequest) => async (dispatch) => {
  // remove properties that interfere with deserialization
  const rentalRequestId = rentalRequest.id;
  dispatch({
    type: Action.UPDATE_RENTAL_REQUEST,
    rentalRequest,
    rentalRequestId,
  });

  const response = await dispatch(
    new ApiRequest(`/rentalrequests/${rentalRequest.id}`)
      .put(_.omit(rentalRequest, 'primaryContact')));
  
  let rentalRequestResponse = response.data;
  // Add display fields
  dispatch(parseRentalRequest(rentalRequestResponse));

  dispatch({
    type: Action.UPDATE_RENTAL_REQUEST,
    rentalRequest: rentalRequestResponse,
    rentalRequestId,
  });
};

export const addRentalRequestHistory = (requestId, history) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/rentalrequests/${requestId}/history`).post(history));
  let historyResponse = normalize(response.data);
  // Add display fields
  _.map(historyResponse, (h) => {
    parseHistory(h);
  });

  dispatch({
    type: Action.UPDATE_RENTAL_REQUEST_HISTORY,
    history: historyResponse,
    id: requestId,
  });
};

export const getRentalRequestHistory = (requestId, params) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/rentalrequests/${requestId}/history`).get(params));
  let history = normalize(response.data);

  // Add display fields
  _.map(history, (h) => {
    parseHistory(h);
  });

  dispatch({
    type: Action.UPDATE_RENTAL_REQUEST_HISTORY,
    history,
    id: requestId,
  });
};

export const getRentalRequestDocuments = (rentalRequestId) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/rentalrequests/${rentalRequestId}/attachments`).get());
  let documents = normalize(response.data);

  // Add display fields
  _.map(documents, (document) => {
    parseDocument(document);
  });

  dispatch({ type: Action.UPDATE_DOCUMENTS, documents });
};

// XXX: Looks like this is unused
// export const addRentalRequestDocument = (rentalRequestId, files) => async (dispatch) => {
//   return await dispatch(new ApiRequest(`/rentalrequests/${ rentalRequestId }/attachments`).post(files));
// };

export const getRentalRequestNotes = (rentalRequestId) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/rentalrequests/${rentalRequestId}/notes`).get());
  dispatch({
    type: Action.UPDATE_RENTAL_REQUEST_NOTES,
    notes: response.data,
    rentalRequestId,
  });
  return response.data;
};

export const addRentalRequestNote = (rentalRequestId, note) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/rentalRequests/${rentalRequestId}/note`).post(note));
  dispatch({
    type: Action.UPDATE_RENTAL_REQUEST_NOTES,
    notes: response.data,
    rentalRequestId,
  });
  return response.data;
};

export const cancelRentalRequest = (rentalRequestId) => async (dispatch) => {
  await dispatch(new ApiRequest(`/rentalrequests/${rentalRequestId}/cancel`).get());
  dispatch(getEquipmentHires());
};

export const rentalRequestSeniorityList = (rentalRequestId, counterCopy) => async (dispatch) => {
  const params = {};
  if (counterCopy) {
    params.counterCopy = counterCopy;
  }

  return await dispatch(
    new ApiRequest(`/rentalrequests/${rentalRequestId}/senioritylist`)
      .get(params, { responseType: Constant.RESPONSE_TYPE_BLOB }));
};

////////////////////
// Rental Request Rotation List
////////////////////

const getSeniorityDisplayName = (blockNumber, numberOfBlocks, seniority, numberInBlock) => {
  if (blockNumber === numberOfBlocks) {
    return `Open-${seniority && seniority.toFixed(3)} (${numberInBlock})`;
  }
  if (blockNumber === 1) {
    return `1-${seniority && seniority.toFixed(3)} (${numberInBlock})`;
  }
  if (blockNumber === 2) {
    return `2-${seniority && seniority.toFixed(3)} (${numberInBlock})`;
  }
  return `Open-${seniority && seniority.toFixed(3)} (${numberInBlock})`;
};

const parseRentalRequestRotationList = (rotationListItem, rentalRequest = {}) => {
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
};

const parseRotationListItem = (item, numberOfBlocks, districtEquipmentType) => {
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

  let primaryContact = item.equipment.owner && item.equipment.owner.primaryContact;
  item.displayFields.primaryContactName = primaryContact
    ? firstLastName(primaryContact.givenName, primaryContact.surname)
    : '';
};

export const getRentalRequestRotationList = (id) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/rentalrequests/${id}/rotationList`).get());
  const rentalRequest = response.data;
  let rotationList = rentalRequest.rentalRequestRotationList;

  rotationList.map((item) =>
    parseRotationListItem(item, rentalRequest.numberOfBlocks, rentalRequest.districtEquipmentType)
  );

  dispatch({
    type: Action.UPDATE_RENTAL_REQUEST_ROTATION_LIST,
    rotationList,
    rentalRequestId: id,
  });
};

export const updateRentalRequestRotationList = (rentalRequestRotationList, rentalRequest) => async (dispatch) => {
  const rentalRequestId = rentalRequest.id;
  const response = await dispatch(
    new ApiRequest(`/rentalrequests/${rentalRequestId}/rentalRequestRotationList`)
      .put(rentalRequestRotationList));

  let rotationList = response.data.rentalRequestRotationList;

  rotationList.map((item) =>
    parseRotationListItem(item, rentalRequest.numberOfBlocks, rentalRequest.districtEquipmentType)
  );

  dispatch({
    type: Action.UPDATE_RENTAL_REQUEST_ROTATION_LIST,
    rotationList,
    rentalRequestId,
  });

  dispatch(getEquipmentTs());
  dispatch(getEquipmentHires());
};

////////////////////
// Rental Agreements
////////////////////

const parseRentalAgreement = (agreement) => {
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
};

// reverse the transformations applied by the parse function
const convertRentalAgreement = (agreement) => ({
  ...agreement,
  rentalAgreementConditions: _.values(agreement.rentalAgreementConditions),
  rentalAgreementRates: _.values(agreement.rentalAgreementRates),
  overtimeRates: _.values(agreement.overtimeRates),
  equipmentId: agreement.equipmentId || null,
  projectId: agreement.projectId || null,
});

export const getRentalAgreementSummaryLite = () => async (dispatch) => {
  const response = await dispatch(new ApiRequest('/rentalagreements/summaryLite').get());
  dispatch({
    type: Action.UPDATE_AGREEMENT_SUMMARY_LITE_LOOKUP,
    agreements: response.data,
  });
};

export const getRentalAgreement = (id) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/rentalagreements/${id}`).get());
  let agreement = response.data;

  // Add display fields
  parseRentalAgreement(agreement);

  dispatch({
    type: Action.UPDATE_RENTAL_AGREEMENT,
    rentalAgreement: agreement,
  });
};

export const getLatestRentalAgreement = (equipmentId, projectId) => async (dispatch) => {
  const response = await dispatch(
    new ApiRequest(`/rentalagreements/latest/${projectId}/${equipmentId}`).get());

  const agreement = response.data;

  dispatch({
    type: Action.UPDATE_RENTAL_AGREEMENT,
    rentalAgreement: agreement,
  });

  return agreement;
};

// XXX: Looks like this is unused
// export const addRentalAgreement = (agreement) => async (dispatch) => {
//   const response = await dispatch(new ApiRequest('/rentalagreements').post(agreement));
//   let agreement = response.data;

//   // Add display fields
//   parseRentalAgreement(agreement);

//   dispatch({ type: Action.ADD_RENTAL_AGREEMENT, rentalAgreement: agreement });
// };

export const updateRentalAgreement = (agreement) => async (dispatch) => {
  const preparedAgreement = convertRentalAgreement(agreement);

  dispatch({
    type: Action.UPDATE_RENTAL_AGREEMENT,
    rentalAgreement: preparedAgreement,
  });

  const response = await dispatch(new ApiRequest(`/rentalagreements/${agreement.id}`).put(preparedAgreement));
  let agreementResponse = response.data;

  // Add display fields
  parseRentalAgreement(agreementResponse);

  dispatch({
    type: Action.UPDATE_RENTAL_AGREEMENT,
    rentalAgreement: agreementResponse,
  });
};

export const getRentalAgreementTimeRecords = (rentalAgreementId) => async (dispatch) => {
  const response = await dispatch(
    new ApiRequest(`/rentalagreements/${rentalAgreementId}/timeRecords`).get());
  
  dispatch({
    type: Action.RENTAL_AGREEMENT_TIME_RECORDS,
    rentalAgreementTimeRecords: response.data,
  });
};

export const addRentalAgreementTimeRecords = (rentalRequestId, timeRecords) => async (dispatch) => {
  const formattedTimeRecords = formatTimeRecords(timeRecords, rentalRequestId);
  const response = await dispatch(
    new ApiRequest(`/rentalagreements/${rentalRequestId}/timeRecords`)
      .post(formattedTimeRecords));
    
  const rentalAgreementTimeRecords = normalize(response.data.timeRecords);

  dispatch({
    type: Action.RENTAL_AGREEMENT_TIME_RECORDS,
    rentalAgreementTimeRecords,
  });
  return rentalAgreementTimeRecords;
};

export const releaseRentalAgreement = (rentalAgreementId) => async (dispatch) => {
  return await dispatch(new ApiRequest(`/rentalagreements/${rentalAgreementId}/release`).post());
};

export const generateRentalAgreementDocument = (rentalAgreementId) => async (dispatch) => {
  return await dispatch(new ApiRequest(`/rentalagreements/${rentalAgreementId}/doc`).getBlob());
};

export const searchAitReport = (params) => async (dispatch) => {
  dispatch({ type: Action.AIT_REPORT_REQUEST });
  const response = await dispatch(new ApiRequest('/rentalAgreements/aitReport').get(params));
  dispatch({
    type: Action.UPDATE_AIT_REPORT,
    aitResponses: normalize(response.data),
  });
};

////////////////////
// Rental Rates
////////////////////

const parseRentalRate = (rentalRate, parent = {}) => {
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
};

// XXX: Looks like this is unused
// export const getRentalRate = (id) => async (dispatch) => {
//   const response = await dispatch(new ApiRequest(`/rentalagreementrates/${ id }`).get());
//   let rentalRate = response.data;

//   // Add display fields
//   parseRentalRate(rentalRate);

//   dispatch({ type: Action.UPDATE_RENTAL_RATE, rentalRate });
// };

// export const addRentalRate = (rentalRate) => async (dispatch) => {
//   const response = await dispatch(
//     new ApiRequest('/rentalagreementrates')
//       .post({ ...rentalRate, rentalAgreement: { id: rentalRate.rentalAgreement.id } }));

//   let rentalRate = response.data;

//   // Add display fields
//   parseRentalRate(rentalRate);

//   dispatch({ type: Action.ADD_RENTAL_RATE, rentalRate });
// };

export const addRentalRates = (rentalAgreementId, rentalRates) => async (dispatch) => {
  dispatch({
    type: Action.ADD_RENTAL_RATES,
    rentalRates,
    rentalAgreementId,
  });

  const response = await dispatch(
    new ApiRequest(`/rentalagreements/${rentalAgreementId}/rateRecords`).post(rentalRates));
  
  const data = _.find(response.data, { rentalAgreementId });
  let rates = data.rentalAgreement.rentalAgreementRates;

  // Add display fields
  rates.forEach((rentalRate) => parseRentalRate(rentalRate, data.rentalAgreement));

  dispatch({
    type: Action.UPDATE_RENTAL_RATES,
    rentalRates: rates,
    rentalAgreementId,
  });

  return rates;
};

export const updateRentalRate = (rentalRate) => async (dispatch) => {
  const rentalAgreementId = rentalRate.rentalAgreement.id;
  dispatch({
    type: Action.UPDATE_RENTAL_RATES,
    rentalRates: [rentalRate],
    rentalAgreementId,
  });

  const response = await dispatch(new ApiRequest(`/rentalagreementrates/${rentalRate.id}`).put(rentalRate));
  let rate = response.data;

  // Add display fields
  parseRentalRate(rate);

  dispatch({
    type: Action.UPDATE_RENTAL_RATES,
    rentalRates: [rate],
    rentalAgreementId,
  });

  return rentalRate;
};

export const deleteRentalRate = (rentalRate) => async (dispatch) => {
  const rentalAgreementId = rentalRate.rentalAgreement.id;
  dispatch({
    type: Action.DELETE_RENTAL_RATE,
    rentalRate,
    rentalAgreementId,
  });

  const response = await dispatch(new ApiRequest(`/rentalagreementrates/${rentalRate.id}/delete`).post());
  let rate = response.data;

  // Add display fields
  parseRentalRate(rate);

  dispatch({
    type: Action.DELETE_RENTAL_RATE,
    rentalRate: rate,
    rentalAgreementId,
  });

  return rate;
};

////////////////////
// Rental Conditions
////////////////////

const parseRentalCondition = (rentalCondition, parent = {}) => {
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
};

// XXX: Looks like this is unused
// export const getRentalCondition = (id) => async (dispatch) => {
//   const response = await dispatch(new ApiRequest(`/rentalagreementconditions/${ id }`).get());
//   let rentalCondition = response.data;

//   // Add display fields
//   parseRentalCondition(rentalCondition);

//   dispatch({ type: Action.UPDATE_RENTAL_CONDITION, rentalCondition });
// };

// XXX: Looks like this is unused
// export const addRentalCondition = (rentalCondition) => async (dispatch) => {
//   const response = await dispatch(
//     new ApiRequest('/rentalagreementconditions')
//       .post({ ...rentalCondition, rentalAgreement: { id: rentalCondition.rentalAgreement.id } }));

//   let rentalCondition = response.data;

//   // Add display fields
//   parseRentalCondition(rentalCondition);

//   dispatch({ type: Action.ADD_RENTAL_CONDITION, rentalCondition });
// };

export const addRentalConditions = (rentalAgreementId, rentalConditions) => async (dispatch) => {
  dispatch({
    type: Action.ADD_RENTAL_CONDITIONS,
    rentalConditions,
    rentalAgreementId,
  });

  const response = await dispatch(
    new ApiRequest(`/rentalagreements/${rentalAgreementId}/conditionRecords`)
      .post(rentalConditions));

  const data = _.find(response.data, { rentalAgreementId });
  let conditions = data.rentalAgreement.rentalAgreementConditions;

  // Add display fields
  conditions.forEach((rentalCondition) => parseRentalCondition(rentalCondition, data.rentalAgreement));

  dispatch({
    type: Action.UPDATE_RENTAL_CONDITIONS,
    rentalConditions: conditions,
    rentalAgreementId,
  });

  return conditions;
};

export const updateRentalCondition = (rentalCondition) => async (dispatch) => {
  const rentalAgreementId = rentalCondition.rentalAgreement.id;
  dispatch({
    type: Action.UPDATE_RENTAL_CONDITIONS,
    rentalConditions: [rentalCondition],
    rentalAgreementId,
  });

  const response = await dispatch(
    new ApiRequest(`/rentalagreementconditions/${rentalCondition.id}`).put(rentalCondition));
  
  let condition = response.data;

  // Add display fields
  parseRentalCondition(condition);

  dispatch({
    type: Action.UPDATE_RENTAL_CONDITIONS,
    rentalConditions: [condition],
    rentalAgreementId,
  });

  return condition;
};

export const deleteRentalCondition = (rentalCondition) => async (dispatch) => {
  const rentalAgreementId = rentalCondition.rentalAgreement.id;
  dispatch({
    type: Action.DELETE_RENTAL_CONDITION,
    rentalCondition,
    rentalAgreementId,
  });

  const response = await dispatch(
    new ApiRequest(`/rentalagreementconditions/${rentalCondition.id}/delete`).post());
  
  let condition = response.data;

  // Add display fields
  parseRentalCondition(condition);

  dispatch({
    type: Action.DELETE_RENTAL_CONDITION,
    rentalCondition: condition,
    rentalAgreementId,
  });

  return condition;
};

export const deleteCondition = (id) => async (dispatch) => {
  return await dispatch(new ApiRequest(`/conditiontypes/${id}/delete`).post());
};

export const addCondition = (condition) => async (dispatch) => {
  return await dispatch(new ApiRequest('/conditiontypes/0').post(condition));
};

export const updateCondition = (condition) => async (dispatch) => {
  return await dispatch(new ApiRequest(`/conditiontypes/${condition.id}`).post(condition));
};

////////////////////
// Business
////////////////////

export const getBusiness = () => async (dispatch) => {
  const response = await dispatch(new ApiRequest('/business').get());
  let business = response.data;

  if (!_.isObject(business)) {
    business = {};
  }

  _.map(business.owners, (owner) => {
    dispatch(parseOwner(owner));
  });

  dispatch({ type: Action.UPDATE_BUSINESS, business });
};

export const getOwnerForBusiness = (ownerId) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/business/owner/${ownerId}`).get());
  let owner = response.data;

  dispatch(parseOwner(owner));
  dispatch({ type: Action.UPDATE_OWNER, owner });
};

export const validateOwner = (secretKey, postalCode) => async (dispatch) => {
  const response = await dispatch(
    new ApiRequest('/business/validateOwner')
      .get({ sharedKey: secretKey, postalCode }));

  let business = response.data;
  dispatch(parseOwner(business.linkedOwner));
  dispatch({ type: Action.UPDATE_BUSINESS, business });
};

////////////////////
// Rollovers
////////////////////

const parseRolloverStatus = (status) => {
  status.rolloverActive =
    status.progressPercentage != null && status.progressPercentage >= 0 && status.progressPercentage < 100;
  status.rolloverComplete = status.progressPercentage === 100;
};

export const getRolloverStatus = (districtId) => async (dispatch) => {
  const response = await dispatch(
    new ApiRequest(`/districts/${districtId}/rolloverStatus`)
      .get(null, { silent: true }));
  
  let status = response.data;
  parseRolloverStatus(status);
  dispatch({
    type: Action.UPDATE_ROLLOVER_STATUS_LOOKUP,
    status,
  });
};

export const initiateRollover = (districtId) => async (dispatch) => {
  const response = await dispatch(
    new ApiRequest(`/districts/${districtId}/annualRollover`).get());
  
  let status = response;
  parseRolloverStatus(status);
  dispatch({
    type: Action.UPDATE_ROLLOVER_STATUS_LOOKUP,
    status,
  });
};

export const dismissRolloverMessage = (districtId) => async (dispatch) => {
  const response = await dispatch(
    new ApiRequest(`/districts/${districtId}/dismissRolloverMessage`).post());
  
  let status = response.data;
  parseRolloverStatus(status);
  dispatch({
    type: Action.UPDATE_ROLLOVER_STATUS_LOOKUP,
    status,
  });
};

////////////////////
// Look-ups
////////////////////

// XXX: Looks like this is unused
// export const getCities = () => async (dispatch) => {
//   const response = await dispatch(new ApiRequest('/cities').get());
//   dispatch({ type: Action.UPDATE_CITIES_LOOKUP, cities: normalize(response.data) });
// };

export const getDistricts = () => async (dispatch) => {
  const response = await dispatch(new ApiRequest('/districts').get());
  dispatch({
    type: Action.UPDATE_DISTRICTS_LOOKUP,
    districts: normalize(response.data),
  });
};

export const getRegions = () => async (dispatch) => {
  const response = await dispatch(new ApiRequest('/regions').get());
  dispatch({ type: Action.UPDATE_REGIONS_LOOKUP, regions: normalize(response.data) });
};

export const getLocalAreas = (id) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/districts/${id}/localAreas`).get());
  let localAreas = normalize(response.data);
  _.map(localAreas, (area) => (area.name = `${area.serviceAreaId} - ${area.name}`));

  dispatch({
    type: Action.UPDATE_LOCAL_AREAS_LOOKUP,
    localAreas,
  });
};

export const getServiceAreas = () => async (dispatch) => {
  const response = await dispatch(new ApiRequest('/serviceareas').get());
  dispatch({
    type: Action.UPDATE_SERVICE_AREAS_LOOKUP,
    serviceAreas: normalize(response.data),
  });
};

export const getEquipmentTypes = () => async (dispatch, getState) => {
  const silent = getState().lookups.equipmentTypes.loaded;
  const response = await dispatch(new ApiRequest('/equipmenttypes', { silent }).get());
  let equipmentTypes = _.mapValues(normalize(response.data), (x) => {
    x.blueBookSectionAndName = `${x.blueBookSection} - ${x.name}`;
    return x;
  });

  dispatch({
    type: Action.UPDATE_EQUIPMENT_TYPES_LOOKUP,
    equipmentTypes,
  });
};

export const getDistrictEquipmentTypes = () => async (dispatch, getState) => {
  const silent = getState().lookups.districtEquipmentTypes.loaded;
  const response = await dispatch(new ApiRequest('/districtequipmenttypes', { silent }).get());
  dispatch({
    type: Action.UPDATE_DISTRICT_EQUIPMENT_TYPES_LOOKUP,
    districtEquipmentTypes: normalize(response.data),
  });
};

export const getDistrictEquipmentTypesAgreementSummary = () => async (dispatch, getState) => {
  const silent = getState().lookups.districtEquipmentTypesAgreementSummary.loaded;
  const response = await dispatch(
    new ApiRequest('/districtequipmenttypes/agreementSummary', { silent }).get());
  
  dispatch({
    type: Action.UPDATE_DISTRICT_EQUIPMENT_TYPES_AGREEMENT_SUMMARY_LOOKUP,
    districtEquipmentTypes: response.data,
  });
};

export const getFiscalYears = (districtId) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/districts/${districtId}/fiscalYears`).get());
  dispatch({
    type: Action.UPDATE_FISCAL_YEARS_LOOKUP,
    fiscalYears: response.data,
  });
};

export const getOwnersLite = () => async (dispatch, getState) => {
  const silent = getState().lookups.owners.lite.loaded;
  const response = await dispatch(new ApiRequest('/owners/lite', { silent }).get());
  dispatch({ type: Action.UPDATE_OWNERS_LITE_LOOKUP, owners: normalize(response.data) });
};

export const getOwnersLiteHires = () => async (dispatch, getState) => {
  const silent = getState().lookups.owners.hires.loaded;
  const response = await dispatch(new ApiRequest('/owners/liteHires', { silent }).get());
  dispatch({
    type: Action.UPDATE_OWNERS_LITE_HIRES_LOOKUP,
    owners: normalize(response.data),
  });
};

export const getOwnersLiteTs = () => async (dispatch, getState) => {
  const silent = getState().lookups.owners.ts.loaded;
  const response = await dispatch(new ApiRequest('/owners/liteTs', { silent }).get());
  dispatch({
    type: Action.UPDATE_OWNERS_LITE_TS_LOOKUP,
    owners: normalize(response.data),
  });
};

export const addDistrictEquipmentType = (equipment) => async (dispatch) => {
  return await dispatch(new ApiRequest(`/districtequipmenttypes/${equipment.id}`).post(equipment));
};

export const updateDistrictEquipmentType = (equipment) => async (dispatch) => {
  return await dispatch(new ApiRequest(`/districtequipmenttypes/${equipment.id}`).post(equipment));
};

export const deleteDistrictEquipmentType = (equipment) => async (dispatch) => {
  return await dispatch(new ApiRequest(`/districtequipmenttypes/${equipment.id}/delete`).post());
};

export const getRoles = () => async (dispatch) => {
  const response = await dispatch(new ApiRequest('/roles').get());
  dispatch({ type: Action.UPDATE_ROLES_LOOKUP, roles: normalize(response.data) });
};

export const getPermissions = () => async (dispatch) => {
  const response = await dispatch(new ApiRequest('/permissions').get());
  dispatch({
    type: Action.UPDATE_PERMISSIONS_LOOKUP,
    permissions: normalize(response.data),
  });
};

// XXX: Looks like this is unused
// export const getProvincialRateTypes = () => async (dispatch) => {
//   const response = await dispatch(new ApiRequest('/provincialratetypes').get());
//   const rateTypeOther = {
//     id: 10000,
//     rateType: 'OTHER',
//     description: Constant.NON_STANDARD_CONDITION,
//     rate: null,
//     isPercentRate: false,
//     isRateEditable: true,
//     isIncludedInTotal: false,
//     isInTotalEditable: true,
//   };
//   const provincialRateTypes = [ ...response.data, rateTypeOther ];

//   dispatch({ type: Action.UPDATE_PROVINCIAL_RATE_TYPES_LOOKUP, provincialRateTypes });
// };

export const getOvertimeRateTypes = () => async (dispatch) => {
  const response = await dispatch(new ApiRequest('/provincialratetypes/overtime').get());
  dispatch({
    type: Action.UPDATE_OVERTIME_RATE_TYPES_LOOKUP,
    overtimeRateTypes: response.data,
  });
};

export const updateOvertimeRateType = (rate) => async (dispatch) => {
  return await dispatch(new ApiRequest(`/provincialratetypes/${rate.id}`).put(rate));
};

export const getRentalConditions = () => async (dispatch) => {
  dispatch({ type: Action.RENTAL_CONDITIONS_LOOKUP_REQUEST });
  const response = await dispatch(new ApiRequest('/conditiontypes').get());
  dispatch({
    type: Action.UPDATE_RENTAL_CONDITIONS_LOOKUP,
    rentalConditions: response.data,
  });
};

////////////////////
// Version
////////////////////

export const getVersion = () => async (dispatch) => {
  const response = await dispatch(new ApiRequest('/version').get());
  dispatch({ type: Action.UPDATE_VERSION, version: response });
};

////////////////////
// Notes
////////////////////

export const deleteNote = (id) => async (dispatch) => {
  dispatch({ type: Action.DELETE_NOTE, noteId: id });
  const response = await dispatch(new ApiRequest(`/notes/${id}/delete`).post());
  return response.data;
};

export const updateNote = (note) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/notes/${note.id}`).put(note));
  return response.data;
};

////////////////////
// Set User
////////////////////

export const setDevUser = (user) => async (dispatch) => {
  return await dispatch(new ApiRequest(`/authentication/dev/token/${user}`).get());
};

////////////////////
// Time Records
////////////////////

export const deleteTimeRecord = (timeRecordId) => async (dispatch) => {
  const response = await dispatch(new ApiRequest(`/timerecords/${timeRecordId}/delete`).post());
  dispatch({
    type: Action.DELETE_TIME_RECORD,
    timeRecord: response.data,
  });
};
