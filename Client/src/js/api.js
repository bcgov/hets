import store from './store';

import { ApiRequest } from './utils/http';
import { lastFirstName } from './utils/string';

import _ from 'lodash';

export function getCurrentUser() {
  return new ApiRequest('/users/current').get().then(response => {
    store.dispatch({ type: 'UPDATE_CURRENT_USER', user: response });
  });
}

export function getUsers(params) {
  return new ApiRequest('/users').get(params).then(response => {
    // Normalize the response
    var users = _.fromPairs(response.map(user => [ user.id, user ]));

    // Add display fields
    _.map(users, user => {
      user.name = lastFirstName(user.surname, user.givenName);
    });

    store.dispatch({ type: 'UPDATE_USERS', users: users });
  });
}

export function getUser(userId) {
  return new ApiRequest(`/users/${userId}`).get().then(response => {
    var user = response;

    // Add display fields
    user.name = lastFirstName(user.surname, user.givenName);

    store.dispatch({ type: 'UPDATE_USER', user: user });
  });
}

// Look Ups

export function getCities() {
  return new ApiRequest('/cities').get().then(response => {
    // Normalize the response
    var cities = _.fromPairs(response.map(city => [ city.id, city ]));

    store.dispatch({ type: 'UPDATE_CITIES', cities: cities });
  });
}

export function getDistricts() {
  return new ApiRequest('/districts').get().then(response => {
    // Normalize the response
    var districts = _.fromPairs(response.map(district => [ district.id, district ]));

    store.dispatch({ type: 'UPDATE_DISTRICTS', districts: districts });
  });
}

export function getRegions() {
  return new ApiRequest('/regions').get().then(response => {
    // Normalize the response
    var regions = _.fromPairs(response.map(region => [ region.id, region ]));

    store.dispatch({ type: 'UPDATE_REGIONS', regions: regions });
  });
}

export function getServiceAreas() {
  return new ApiRequest('/serviceareas').get().then(response => {
    // Normalize the response
    var serviceAreas = _.fromPairs(response.map(serviceArea => [ serviceArea.id, serviceArea ]));

    store.dispatch({ type: 'UPDATE_SERVICE_AREAS', serviceAreas: serviceAreas });
  });
}
