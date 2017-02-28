import * as Action from './actionTypes';
import * as Constant from './constants';
import store from './store';

import { ApiRequest } from './utils/http';
import { lastFirstName, firstLastName, concat } from './utils/string';
import { daysAgo } from './utils/date';

import _ from 'lodash';
import Moment from 'moment';

////////////////////
// Current User
////////////////////

export function getCurrentUser() {
  return new ApiRequest('/users/current').get().then(response => {
    store.dispatch({ type: Action.UPDATE_CURRENT_USER, user: response });
  });
}

////////////////////
// Users
////////////////////

function parseUser(user) {
  user.name = lastFirstName(user.surname, user.givenName);
}

export function getUsers() {
  return new ApiRequest('/users').get().then(response => {
    // Normalize the response
    var users = _.fromPairs(response.map(user => [ user.id, user ]));

    // Add display fields
    _.map(users, user => { parseUser(user); });

    store.dispatch({ type: Action.UPDATE_USERS, users: users });
  });
}

export function getUser(userId) {
  return new ApiRequest(`/users/${ userId }`).get().then(response => {
    var user = response;

    // Add display fields
    parseUser(user);

    store.dispatch({ type: Action.UPDATE_USER, user: user });
  });
}

////////////////////
// Favourites
////////////////////

export function getFavourites(type) {
  return new ApiRequest(`/users/current/favourites/${ type }`).get().then(response => {
    // Normalize the response
    var favourites = _.fromPairs(response.map(favourite => [ favourite.id, favourite ]));

    store.dispatch({ type: Action.UPDATE_FAVOURITES, favourites: favourites });
  });
}

export function addFavourite(favourite) {
  return new ApiRequest('/users/current/favourites').post(favourite).then(response => {
    // Normalize the response
    var favourite = _.fromPairs([[ response.id, response ]]);

    store.dispatch({ type: Action.ADD_FAVOURITE, favourite: favourite });
  });
}

export function updateFavourite(favourite) {
  return new ApiRequest('/users/current/favourites').put(favourite).then(response => {
    // Normalize the response
    var favourite = _.fromPairs([[ response.id, response ]]);

    store.dispatch({ type: Action.UPDATE_FAVOURITE, favourite: favourite });
  });
}

export function deleteFavourite(favourite) {
  return new ApiRequest(`/users/current/favourites/${ favourite.id }/delete`).post().then(response => {
    // No needs to normalize, as we just want the id from the response.
    store.dispatch({ type: Action.DELETE_FAVOURITE, id: response.id });
  });
}

////////////////////
// Equipment
////////////////////

function parseEquipment(equipment) {
  if (!equipment.owner) { equipment.owner = { id: '', organizationName: '' }; }
  if (!equipment.equipmentType) { equipment.equipmentType = { id: '', name: '', description: '' }; }
  if (!equipment.localArea) { equipment.localArea = { id: '', name: '' }; }
  if (!equipment.localArea.serviceArea) { equipment.localArea.serviceArea = { id: '', name: '' }; }
  if (!equipment.localArea.serviceArea.district) { equipment.localArea.serviceArea.district = { id: '', name: '' }; }
  if (!equipment.localArea.serviceArea.district.region) { equipment.localArea.serviceArea.district.region = { id: '', name: '' }; }
  if (!equipment.status) { equipment.status = Constant.EQUIPMENT_STATUS_CODE_PENDING; }
  if (!equipment.equipmentAttachments) { equipment.equipmentAttachments = []; }

  equipment.isApproved = equipment.status === Constant.EQUIPMENT_STATUS_CODE_APPROVED;
  equipment.isNew = equipment.status === Constant.EQUIPMENT_STATUS_CODE_PENDING;
  equipment.isArchived = equipment.status === Constant.EQUIPMENT_STATUS_CODE_ARCHIVED;
  equipment.isMaintenanceContractor = equipment.owner.isMaintenanceContractor === true;

  // UI display fields
  equipment.serialNumber = equipment.serialNumber || '';
  equipment.equipmentCode = equipment.equipmentCode || '';
  equipment.licencePlate = equipment.licencePlate || '';
  equipment.operator = equipment.operator || ''; // TODO Needs review from business
  equipment.organizationName = equipment.owner.organizationName;
  equipment.ownerPath = equipment.owner.id ? `#/owners/${ equipment.owner.id }` : '';
  equipment.typeName = equipment.equipmentType ? equipment.equipmentType.name : '';
  equipment.localAreaName = equipment.localArea.name;
  equipment.districtName = equipment.localArea.serviceArea.district.name;
  equipment.lastVerifiedDate = equipment.lastVerifiedDate || '';
  equipment.daysSinceVerified = daysAgo(equipment.lastVerifiedDate);

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
  // TODO Replace "3-500"" with "Open-500"
  equipment.seniorityText = concat(equipment.blockNumber, equipment.seniority, ' - ');

  equipment.currentYear = Moment().year();
  equipment.lastYear = equipment.currentYear - 1;
  equipment.twoYearsAgo = equipment.currentYear - 2;
  equipment.threeYearsAgo = equipment.currentYear - 3;

  // It is possible to have multiple instances of the same piece of equipment registered with HETS.
  // However, the HETS clerks would like to know about it via this flag so they can deal with the duplicates.
  equipment.hasDuplicates = equipment.hasDuplicates || false;
  equipment.duplicateEquipment = equipment.duplicateEquipment || [];

  equipment.isWorking = equipment.isWorking || false;  
  // TODO Descriptive text for time entries. Needs to be added to backend
  equipment.currentWorkDescription = equipment.currentWorkDescription || '' ;
}

export function searchEquipmentList(params) {
  return new ApiRequest('/equipment/search').get(params).then(response => {
    // Normalize the response
    var equipmentList = _.fromPairs(response.map(equip => [ equip.id, equip ]));

    // Add display fields
    _.map(equipmentList, equip => { parseEquipment(equip); });

    store.dispatch({ type: Action.UPDATE_EQUIPMENT_LIST, equipmentList: equipmentList });
  });
}

export function getEquipmentList() {
  return new ApiRequest('/equipment').get().then(response => {
    // Normalize the response
    var equipmentList = _.fromPairs(response.map(equip => [ equip.id, equip ]));

    // Add display fields
    _.map(equipmentList, equip => { parseEquipment(equip); });

    store.dispatch({ type: Action.UPDATE_EQUIPMENT_LIST, equipmentList: equipmentList });
  });
}

export function getEquipment(equipmentId) {
  return new ApiRequest(`/equipment/${ equipmentId }`).get().then(response => {
    var equipment = response;

    // Add display fields
    parseEquipment(equipment);

    store.dispatch({ type: Action.UPDATE_EQUIPMENT, equipment: equipment });
  });
}

export function updateEquipment(equipment) {
  return new ApiRequest(`/equipment/${ equipment.id }`).put(equipment).then(response => {
    var equipment = response;

    // Add display fields
    parseEquipment(equipment);

    store.dispatch({ type: Action.UPDATE_EQUIPMENT, equipment: equipment });
  });
}

////////////////////
// Physical Attachments
////////////////////

function parsePhysicalAttachment(attachment) {
  if (!attachment.type) { attachment.type = { id: '', code: '', description: ''}; }
  
  attachment.typeName = attachment.type.description;
  // TODO Add grace period logic to editing/deleting attachments
  attachment.canEdit = true;
  attachment.canDelete = true;
}

export function getPhysicalAttachment(id) {
  // TODO Implement back-end endpoints
  return Promise.resolve({ id: id }).then(response => {
    var attachment = response;

    // Add display fields
    parsePhysicalAttachment(attachment);
  });
}

export function addPhysicalAttachment(attachment) {
  // TODO Implement back-end endpoints
  return Promise.resolve(attachment).then(response => {
    var attachment = response;

    // Add display fields
    parsePhysicalAttachment(attachment);
  });
}

export function updatePhysicalAttachment(attachment) {
  // TODO Implement back-end endpoints
  return Promise.resolve(attachment).then(response => {
    var attachment = response;

    // Add display fields
    parsePhysicalAttachment(attachment);
  });
}

export function deletePhysicalAttachment(attachment) {
  // TODO Implement back-end endpoints
  return Promise.resolve(attachment).then(response => {
    var attachment = response;

    // Add display fields
    parsePhysicalAttachment(attachment);
  });
}

////////////////////
// Owners
////////////////////

function parseOwner(owner) {
  if (!owner.localArea) { owner.localArea = { id: '', name: '' }; }
  if (!owner.localArea.serviceArea) { owner.localArea.serviceArea = { id: '', name: '' }; }
  if (!owner.localArea.serviceArea.district) { owner.localArea.serviceArea.district = { id: '', name: '' }; }
  if (!owner.localArea.serviceArea.district.region) { owner.localArea.serviceArea.district.region = { id: '', name: '' }; }
  if (!owner.contacts) { owner.contacts = []; }
  if (!owner.equipmentList) { owner.equipmentList = []; }

  // Add display fields for owner contacts
  _.map(owner.contacts, contact => { parseContact(contact); });

  // TODO Owner status needs to be populated in sample data. Setting to Approved for the time being...
  owner.status = owner.status || Constant.OWNER_STATUS_CODE_APPROVED;

  owner.organizationName = owner.organizationName || '';
  owner.ownerEquipmentCodePrefix = owner.ownerEquipmentCodePrefix || '';
  owner.doingBusinessAs = owner.doingBusinessAs || '';
  owner.registeredCompanyNumber = owner.registeredCompanyNumber || '';
  owner.meetsResidency = owner.meetsResidency || false;
  owner.workSafeBCPolicyNumber = owner.workSafeBCPolicyNumber || '';
  owner.workSafeBCExpiryDate = owner.workSafeBCExpiryDate || '';
  owner.cglEndDate = owner.cglEndDate || '';

  // UI display fields
  owner.isMaintenanceContractor = owner.isMaintenanceContractor || false;
  owner.isApproved = owner.status === Constant.OWNER_STATUS_CODE_APPROVED;
  owner.primaryContactName = owner.primaryContact ? firstLastName(owner.primaryContact.givenName, owner.primaryContact.surname) : '';
  owner.localAreaName = owner.localArea.name;
  owner.districtName = owner.localArea.serviceArea.district.name;
  owner.numberOfEquipment = Object.keys(owner.equipmentList).length;
  owner.numberOfPolicyDocuments = owner.numberOfPolicyDocuments || 0;  // TODO
}

export function searchOwners(params) {
  return new ApiRequest('/owners/search').get(params).then(response => {
    // Normalize the response
    var owners = _.fromPairs(response.map(owner => [ owner.id, owner ]));

    // Add display fields
    _.map(owners, owner => { parseOwner(owner); });

    store.dispatch({ type: Action.UPDATE_OWNERS, owners: owners });
  });
}

export function getOwner(ownerId) {
  return new ApiRequest(`/owners/${ ownerId }`).get().then(response => {
    var owner = response;

    // Add display fields
    parseOwner(owner);

    store.dispatch({ type: Action.UPDATE_OWNER, owner: owner });
  });
}

export function getOwners() {
  return new ApiRequest('/owners').get().then(response => {
    // Normalize the response
    var owners = _.fromPairs(response.map(owner => [ owner.id, owner ]));

    // Add display fields
    _.map(owners, owner => { parseOwner(owner); });

    store.dispatch({ type: Action.UPDATE_OWNERS_LOOKUP, owners: owners });
  });
}

export function updateOwner(owner) {
  return new ApiRequest(`/owners/${ owner.id }`).put(owner).then(response => {
    var owner = response;

    // Add display fields
    parseOwner(owner);

    store.dispatch({ type: Action.UPDATE_OWNER, owner: owner });
  });
}

////////////////////
// Contacts
////////////////////

function parseContact(contact) {
  contact.name = firstLastName(contact.givenName, contact.surname);

  // TODO
  contact.canEdit = true;
  contact.canDelete = true;
}

////////////////////
// Projects
////////////////////

function parseProject(project) {
  if (!project.localArea) { project.localArea = { id: '', name: '' }; }
  if (!project.localArea.serviceArea) { project.localArea.serviceArea = { id: '', name: '' }; }
  if (!project.localArea.serviceArea.district) { project.localArea.serviceArea.district = { id: '', name: '' }; }
  if (!project.localArea.serviceArea.district.region) { project.localArea.serviceArea.district.region = { id: '', name: '' }; }
  if (!project.contacts) { project.contacts = []; }
  if (!project.rentalRequests) { project.rentalRequests = []; }
  if (!project.rentalAgreements) { project.rentalAgreements = []; }  // TODO Server needs to send this (HETS-153)

  // Add display fields for rental requests and rental agreements
  _.map(project.rentalRequests, obj => { parseRentalRequest(obj); });
  _.map(project.rentalAgreements, obj => { parseRentalAgreement(obj); });

  project.name = project.name || '';
  project.provincialProjectNumber = project.provincialProjectNumber || '';
  project.information = project.information || '';

  project.numberOfRequests = project.numberOfRequests || Object.keys(project.rentalRequests).length;
  project.numberOfHires = project.numberOfHires || Object.keys(project.rentalAgreements).length;

  // UI display fields
  project.status = project.status || Constant.PROJECT_STATUS_CODE_ACTIVE;
  project.isActive = project.status === Constant.PROJECT_STATUS_CODE_ACTIVE;
  project.localAreaName = project.localArea.name;

  project.primaryContactName = project.primaryContact ? firstLastName(project.primaryContact.givenName, project.primaryContact.surname) : '';
  project.primaryContactRole = project.primaryContact ? project.primaryContact.role : '';
  project.primaryContactEmail = project.primaryContact ? project.primaryContact.emailAddress : '';
  project.primaryContactRole = project.primaryContact ? project.primaryContact.role : '';
  project.primaryContactPhone = project.primaryContact ? project.primaryContact.workPhoneNumber || project.primaryContact.mobilePhoneNumber || '' : '';
}

export function searchProjects(params) {
  return new ApiRequest('/projects/search').get(params).then(response => {
    // Normalize the response
    var projects = _.fromPairs(response.map(project => [ project.id, project ]));

    // Add display fields
    _.map(projects, project => { parseProject(project); });

    store.dispatch({ type: Action.UPDATE_PROJECTS, projects: projects });
  });
}

export function getProject(projectId) {
  return new ApiRequest(`/projects/${ projectId }`).get().then(response => {
    var project = response;

    // Add display fields
    parseProject(project);

    store.dispatch({ type: Action.UPDATE_PROJECT, project: project });
  });
}

export function updateProject(project) {
  return new ApiRequest(`/projects/${ project.id }`).put(project).then(response => {
    var project = response;

    // Add display fields
    parseProject(project);

    store.dispatch({ type: Action.UPDATE_PROJECT, project: project });
  });
}

////////////////////
// Rental Requests
////////////////////

function parseRentalRequest(request) {
  if (!request.localArea) { request.localArea = { id: '', name: '' }; }
  if (!request.localArea.serviceArea) { request.localArea.serviceArea = { id: '', name: '' }; }
  if (!request.localArea.serviceArea.district) { request.localArea.serviceArea.district = { id: '', name: '' }; }
  if (!request.localArea.serviceArea.district.region) { request.localArea.serviceArea.district.region = { id: '', name: '' }; }
  if (!request.project) { request.project = { id: '', name: '' }; }
  if (!request.equipmentType) { request.equipmentType = { id: '', name: '' }; }
  if (!request.primaryContact) { request.primaryContact = { id: '', givenName: '', surname: '' }; }
  if (!request.rentalRequestRotationList) { request.rentalRequestRotationList = []; }

  request.status = request.status || Constant.RENTAL_REQUEST_STATUS_CODE_IN_PROGRESS;
  request.equipmentCount = request.equipmentCount || 0;
  request.expectedHours = request.expectedHours || 0;

  request.projectId = request.projectId || request.project.id;
  request.projectName = request.projectName || request.project.name;
  request.projectPath = request.projectId ? `projects/${ request.projectId }`: '';

  request.expectedStartDate = request.expectedStartDate || '';
  request.expectedEndDate = request.expectedEndDate || '';

  // UI display fields
  request.isActive = request.status === Constant.RENTAL_REQUEST_STATUS_CODE_IN_PROGRESS;
  request.isCompleted = request.status === Constant.RENTAL_REQUEST_STATUS_CODE_COMPLETED;
  request.isCancelled = request.status === Constant.RENTAL_REQUEST_STATUS_CODE_CANCELLED;
  request.localAreaName = request.localArea.name;
  request.equipmentTypeName = request.equipmentTypeName || request.equipmentType.name;
  request.primaryContactName = request.primaryContact ? firstLastName(request.primaryContact.givenName, request.primaryContact.surname) : '';
  request.primaryContactEmail = request.primaryContact ? request.primaryContact.emailAddress : '';

  // Flag element as a rental request. 
  // Rental requests and rentals are merged and shown in a single list on Project Details screen
  request.isRentalRequest = true;
}

export function searchRentalRequests(params) {
  return new ApiRequest('/rentalrequests/search').get(params).then(response => {
    // Normalize the response
    var rentalRequests = _.fromPairs(response.map(req => [ req.id, req ]));

    // Add display fields
    _.map(rentalRequests, req => { parseRentalRequest(req); });

    store.dispatch({ type: Action.UPDATE_RENTAL_REQUESTS, rentalRequests: rentalRequests });
  });
}

export function getRentalRequest(id) {
  return new ApiRequest(`/rentalrequests/${ id }`).get().then(response => {
    var rentalRequest = response;

    // Add display fields
    parseRentalRequest(rentalRequest);

    store.dispatch({ type: Action.UPDATE_RENTAL_REQUEST, rentalRequest: rentalRequest });
  });
}

export function updateRentalRequest(rentalRequest) {
  return new ApiRequest(`/rentalrequests/${ rentalRequest.id }`).put(rentalRequest).then(response => {
    var rentalRequest = response;

    // Add display fields
    parseRentalRequest(rentalRequest);

    store.dispatch({ type: Action.UPDATE_RENTAL_REQUEST, rentalRequest: rentalRequest });
  });
}

////////////////////
// Rental Agreements
////////////////////

function parseRentalAgreement(agreement) {
  if (!agreement.equipment) { agreement.equipment = { id: '', equipmentCode: '' }; }
  if (!agreement.equipment.equipmentType) { agreement.equipment.equipmentType = { id: '', name: '' }; }
  if (!agreement.project) { agreement.project = { id: '', name: '' }; }
  if (!agreement.rentalAgreementRates) { agreement.rentalAgreementRates = []; }
  if (!agreement.rentalAgreementConditions) { agreement.rentalAgreementConditions = []; }
  if (!agreement.timeRecords) { agreement.timeRecords = []; }

  agreement.number = agreement.number || '';
  agreement.note = agreement.note || '';
  agreement.estimateStartWork = agreement.estimateStartWork || '';
  agreement.datedOn = agreement.datedOn || '';
  agreement.estimateHours = agreement.estimateHours || 0;
  agreement.equipmentRate = agreement.equipmentRate || 0.0;
  agreement.ratePeriod = agreement.ratePeriod || '';  // e.g. hourly, daily, etc.
  agreement.rateComment = agreement.rateComment || '';

  // UI display fields
  agreement.status = agreement.status || Constant.RENTAL_AGREEMENT_STATUS_CODE_ACTIVE;  // TODO
  agreement.isActive = agreement.status === Constant.RENTAL_AGREEMENT_STATUS_CODE_ACTIVE;
  agreement.isCompleted = agreement.status === Constant.RENTAL_AGREEMENT_STATUS_CODE_COMPLETED;
  agreement.equipmentId = agreement.equipment.id;
  agreement.equipmentCode = agreement.equipment.equipmentCode;
  agreement.equipmentMake = agreement.equipment.make;
  agreement.equipmentModel = agreement.equipment.model;
  agreement.equipmentSize = agreement.equipment.size;
  agreement.equipmentTypeName = agreement.equipment.equipmentType.name;
  agreement.lastTimeRecord = agreement.lastTimeRecord || '';  // TODO Server needs to send this

  // Flag element as a rental agreement
  // Rental requests and rentals are merged and shown in a single list on Project Details screen
  agreement.isRentalAgreement = true;
}

////////////////////
// Look-ups
////////////////////

export function getCities() {
  return new ApiRequest('/cities').get().then(response => {
    // Normalize the response
    var cities = _.fromPairs(response.map(city => [ city.id, city ]));

    store.dispatch({ type: Action.UPDATE_CITIES_LOOKUP, cities: cities });
  });
}

export function getDistricts() {
  return new ApiRequest('/districts').get().then(response => {
    // Normalize the response
    var districts = _.fromPairs(response.map(district => [ district.id, district ]));

    store.dispatch({ type: Action.UPDATE_DISTRICTS_LOOKUP, districts: districts });
  });
}

export function getRegions() {
  return new ApiRequest('/regions').get().then(response => {
    // Normalize the response
    var regions = _.fromPairs(response.map(region => [ region.id, region ]));

    store.dispatch({ type: Action.UPDATE_REGIONS_LOOKUP, regions: regions });
  });
}

export function getLocalAreas() {
  return new ApiRequest('/localareas').get().then(response => {
    // Normalize the response
    var localAreas = _.fromPairs(response.map(area => [ area.id, area ]));

    store.dispatch({ type: Action.UPDATE_LOCAL_AREAS_LOOKUP, localAreas: localAreas });
  });
}

export function getServiceAreas() {
  return new ApiRequest('/serviceareas').get().then(response => {
    // Normalize the response
    var serviceAreas = _.fromPairs(response.map(serviceArea => [ serviceArea.id, serviceArea ]));

    store.dispatch({ type: Action.UPDATE_SERVICE_AREAS_LOOKUP, serviceAreas: serviceAreas });
  });
}

export function getEquipmentTypes() {
  return new ApiRequest('/equipmenttypes').get().then(response => {
    // Normalize the response
    var equipmentTypes = _.fromPairs(response.map(equipType => [ equipType.id, equipType ]));

    store.dispatch({ type: Action.UPDATE_EQUIPMENT_TYPES_LOOKUP, equipmentTypes: equipmentTypes });
  });
}

////////////////////
// Version
////////////////////

export function getVersion() {
  return new ApiRequest('/version').get().then(response => {
    store.dispatch({ type: Action.UPDATE_VERSION, version: response });
  });
}
