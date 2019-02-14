/* global require, module */

import React from 'react';
import ReactDOM from 'react-dom';
import Promise from 'bluebird';

import './utils/shims';


Promise.config({
  cancellation: true,
});

import App from './app.jsx';
import * as Api from './api';
import { ApiError } from './utils/http';

var initializationEl = document.querySelector('#initialization');
var progressBarEl = initializationEl.querySelector('.progress-bar');
var progress = +progressBarEl.getAttribute('aria-valuenow');

function incrementProgressBar(gotoPercent) {
  progress = Math.min(gotoPercent || (progress + 20), 100); // cap to 100%
  progressBarEl.style.width = `${progress}%`;
  progressBarEl.setAttribute('aria-valuenow', progress);
  progressBarEl.querySelector('span').textContent = `${progress}% Complete`;
}

function renderApp(AppComponent) {
  const appElement = document.querySelector('#app');

  ReactDOM.render(AppComponent, appElement);
}


if(module.hot) {
  module.hot.accept('./app.jsx', () => {
    const UpdatedApp = require('./app.jsx').default;
    renderApp(UpdatedApp);
  });
}

export default function startApp() {
  if (process.env.NODE_ENV === 'development' && process.env.DEV_USER) { //eslint-disable-line
    return Api.setDevUser(process.env.DEV_USER).finally(() => { //eslint-disable-line
      initializeApp();
    });
  }

  return initializeApp();
}

function initializeApp() {
  incrementProgressBar(5);

  Api.getCurrentUser().then(user => {
    incrementProgressBar(33);

    return getLookups(user).then(() => {
      incrementProgressBar(100);

      initializationEl.addEventListener('transitionend', () => {
        renderApp(App);
        initializationEl.classList.add('done');
        initializationEl.addEventListener('transitionend', () => {
          initializationEl.remove();
        });
      });
    });
  }).catch(err => {
    showError(err);
  });
}

function getLookups(user) {
  if (user.businessUser) {
    return Promise.resolve();
  } else {
    var districtId = user.district.id;
    var districtsPromise = Api.getDistricts();
    var regionsPromise = Api.getRegions();
    var serviceAreasPromise = Api.getServiceAreas();
    var localAreasPromise = Api.getLocalAreas(districtId);
    var fiscalYearsPromise = Api.getFiscalYears(districtId);
    var permissionsPromise = Api.getPermissions();
    var currentUserDistrictsPromise = Api.getCurrentUserDistricts();

    return Promise.all([districtsPromise, regionsPromise, serviceAreasPromise, localAreasPromise, fiscalYearsPromise, permissionsPromise, currentUserDistrictsPromise]);
  }
}

function showError(err) {
  progressBarEl.classList.add('progress-bar-danger');
  progressBarEl.classList.remove('active');
  console.error(err);
  var errorMessage = String(err);
  if (err instanceof ApiError) {
    errorMessage = err.message;
  }

  ReactDOM.render((
    <div id="loading-error-message">
      <h4>Error loading application</h4>
      <p>{errorMessage}</p>
    </div>
  ), document.getElementById('init-error'));
}

window.onload = startApp;
