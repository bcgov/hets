/* global module */

import React from 'react';
import ReactDOM from 'react-dom';
import Promise from 'bluebird';

Promise.config({
  cancellation: true,
  warnings: {
    wForgottenReturn: false,
  },
});

import App from './App.jsx';
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

if (module.hot) {
  // NOTE: this is a terrible hack to work around react-router v3's warning about changing routes
  // because of Webpack HMR. See:
  // https://github.com/gaearon/react-hot-loader/issues/298#issuecomment-236510239 for more
  // information.
  // This can be removed once HETS-1169 is completed.

  const orgError = console.error; // eslint-disable-line no-console
  console.error = (...args) => { // eslint-disable-line no-console
    const routeChangeWarning = args && args.length === 1 && typeof args[0] === 'string' && args[0].indexOf('You cannot change <Router routes>;') > -1;
    if (!routeChangeWarning) {
      // Log the error as normally
      orgError.apply(console, args);
    }
  };
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
        const appElement = document.querySelector('#app');

        ReactDOM.render(<App/>, appElement);
        initializationEl.classList.add('done');
        initializationEl.addEventListener('transitionend', () => {
          initializationEl.parentNode.removeChild(initializationEl);
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
    return Promise.all([
      Api.getDistricts(),
      Api.getRegions(),
      Api.getServiceAreas(),
      Api.getLocalAreas(districtId),
      Api.getFiscalYears(districtId),
      Api.getPermissions(),
      Api.getCurrentUserDistricts(),
      Api.getFavourites(),
    ]);
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
