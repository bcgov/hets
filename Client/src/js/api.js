import * as Action from './actionTypes';
import store from './store';

import { ApiRequest } from './utils/http';
import { lastFirstName, firstLastName } from './utils/string';

import _ from 'lodash';


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
  return new ApiRequest(`/users/${userId}`).get().then(response => {
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
  return new ApiRequest(`/users/current/favourites/${type}`).get().then(response => {
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
  return new ApiRequest(`/users/current/favourites/${favourite.id}/delete`).post().then(response => {
    // No needs to normalize, as we just want the id from the response.
    store.dispatch({ type: Action.DELETE_FAVOURITE, id: response.id });
  });
}

////////////////////
// Equipment
////////////////////

function parseEquipment(equip) {
  equip.isApproved = equip.statusCd === 'Approved';
  equip.isNew = equip.statusCd === 'New';
  equip.isArchived = equip.statusCd === 'Archived';

  equip.ownerName = equip.owner ? firstLastName(equip.owner.ownerFirstName, equip.owner.ownerLastName) : '';
  equip.ownerPath = equip.owner ? ('#/owners/' + equip.owner.id) : '';
  equip.typeName = equip.equipmentType ? equip.equipmentType.description : '';

  if (equip.blockNumber && equip.seniority) {
    equip.seniorityDisplayNumber = `${equip.blockNumber} - ${equip.seniority}`;
  } else {
    equip.seniorityDisplayNumber = '';
  }

  // TODO Implement (TBD)
  equip.hiredStatus = 'N/A';
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
  return new ApiRequest(`/equipment/${equipmentId}`).get().then(response => {
    var equipment = response;

    // Add display fields
    parseEquipment(equipment);

    store.dispatch({ type: Action.UPDATE_EQUIPMENT, equipment: equipment });
  });
}

////////////////////
// Owners
////////////////////

function parseOwner(owner) {
  owner.name = firstLastName(owner.ownerFirstName, owner.ownerLastName);
  owner.primaryContactName = owner.primaryContact ? firstLastName(owner.primaryContact.givenName, owner.primaryContact.surname) : '';
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
  return new ApiRequest(`/owners/${ownerId}`).get().then(response => {
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

export function getPhysicalAttachmentTypes() {
  return new ApiRequest('/equipmentattachmenttypes').get().then(response => {
    // Normalize the response
    var physicalAttachmentTypes = _.fromPairs(response.map(attachType => [ attachType.id, attachType ]));

    store.dispatch({ type: Action.UPDATE_PHYSICAL_ATTACHMENT_TYPES_LOOKUP, physicalAttachmentTypes: physicalAttachmentTypes });
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
