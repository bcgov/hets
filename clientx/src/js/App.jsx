import React from 'react';
import { Provider } from 'react-redux';
import { Router, Route, Redirect, hashHistory } from 'react-router';
import { hot } from 'react-hot-loader/root';

import * as Api from './api';
import * as Constant from './constants';
import * as Action from './actionTypes';
import store from './store';

import Main from './views/Main.jsx';
import Home from './views/Home.jsx';
import BusinessPortal from './views/BusinessPortal.jsx';
import BusinessOwner from './views/BusinessOwner.jsx';
import Equipment from './views/Equipment.jsx';
import EquipmentDetail from './views/EquipmentDetail.jsx';
import Owners from './views/Owners.jsx';
import OwnersDetail from './views/OwnersDetail.jsx';
import Projects from './views/Projects.jsx';
import ProjectsDetail from './views/ProjectsDetail.jsx';
import RentalRequests from './views/RentalRequests.jsx';
import RentalRequestsDetail from './views/RentalRequestsDetail.jsx';
import RentalAgreementsDetail from './views/RentalAgreementsDetail.jsx';
import OvertimeRates from './views/OvertimeRates.jsx';
import Users from './views/Users.jsx';
import UsersDetail from './views/UsersDetail.jsx';
import Roles from './views/Roles.jsx';
import RolesDetail from './views/RolesDetail.jsx';
import Rollover from './views/Rollover.jsx';
import DistrictAdmin from './views/DistrictAdmin.jsx';
import TimeEntry from './views/TimeEntry.jsx';
import SeniorityList from './views/SeniorityList.jsx';
import StatusLetters from './views/StatusLetters.jsx';
import HiringReport from './views/HiringReport.jsx';
import WcbCglCoverage from './views/WcbCglCoverage.jsx';
import AitReport from './views/AitReport.jsx';
import Version from './views/Version.jsx';
import FourOhFour from './views/404.jsx';

hashHistory.listen(location =>  {
  if (location.action !== 'POP') { return; }

  redirectIfRolloverActive(location.pathname);
});

// redirects regular users to rollover page if rollover in progress
function redirectIfRolloverActive(path) {
  var onBusinessPage = path.indexOf(Constant.BUSINESS_PORTAL_PATHNAME) === 0;
  var onRolloverPage = path === '/' + Constant.ROLLOVER_PATHNAME;
  if (onBusinessPage || onRolloverPage) { return; }

  var user = store.getState().user;
  if (!user.district) { return; }

  const districtId = user.district.id;

  Api.getRolloverStatus(districtId).then(() => {
    const status = store.getState().lookups.rolloverStatus;

    if (status.rolloverActive) {
      hashHistory.push('/' + Constant.ROLLOVER_PATHNAME);
    } else if (status.rolloverComplete) {
      // refresh fiscal years
      Api.getFiscalYears(districtId);
    }
  });
}

function onEnterBusiness() {
  // allow access to business users
  if (store.getState().user.hasPermission(Constant.PERMISSION_BUSINESS_LOGIN)) {
    return true;
  }

  // redirect HETS users to home page
  if (store.getState().user.hasPermission(Constant.PERMISSION_LOGIN)) {
    hashHistory.push('/');
    return false;
  }

  return false;

  // TODO: redirect other users to 'unauthorized access' page
  //hashHistory.push('/');
}

function onEnterBusinessDetails(nextState, replace, callback) {
  if (onEnterBusiness()) {
    setActiveOwnerId(nextState, replace, callback);
  }
}

function onEnterApplication() {
  // allow access to HETS users
  if (store.getState().user.hasPermission(Constant.PERMISSION_LOGIN)) {
    redirectIfRolloverActive(hashHistory.getCurrentLocation().pathname);
    return;
  }

  // redirect business users to business page
  if (store.getState().user.hasPermission(Constant.PERMISSION_BUSINESS_LOGIN)) {
    hashHistory.push(Constant.BUSINESS_PORTAL_PATHNAME);
  }

  // TODO: redirect other users to 'unauthorized access' page
  //hashHistory.push('/');
}

function setActiveRentalAgreementId(nextState, replace, callback) {
  store.dispatch({ type: Action.SET_ACTIVE_RENTAL_AGREEMENT_ID_UI, rentalAgreementId: nextState.params.rentalAgreementId });
  // TODO: When react was updated (HETS-1100) it broke how this worked. We now need to delay
  // mounting the <Route> in order that `mapStateToProps` is called with the current store's state.
  Promise.resolve().then(callback);
}

function setActiveRentalRequestId(nextState, replace, callback) {
  store.dispatch({ type: Action.SET_ACTIVE_RENTAL_REQUEST_ID_UI, rentalRequestId: nextState.params.rentalRequestId });
  // TODO: When react was updated (HETS-1100) it broke how this worked. We now need to delay
  // mounting the <Route> in order that `mapStateToProps` is called with the current store's state.
  Promise.resolve().then(callback);
}

function setActiveProjectId(nextState, replace, callback) {
  store.dispatch({ type: Action.SET_ACTIVE_PROJECT_ID_UI, projectId: nextState.params.projectId });
  // TODO: When react was updated (HETS-1100) it broke how this worked. We now need to delay
  // mounting the <Route> in order that `mapStateToProps` is called with the current store's state.
  Promise.resolve().then(callback);
}

function setActiveOwnerId(nextState, replace, callback) {
  store.dispatch({ type: Action.SET_ACTIVE_OWNER_ID_UI, ownerId: nextState.params.ownerId });
  // TODO: When react was updated (HETS-1100) it broke how this worked. We now need to delay
  // mounting the <Route> in order that `mapStateToProps` is called with the current store's state.
  Promise.resolve().then(callback);
}

function setActiveEquipmentId(nextState, replace, callback) {
  store.dispatch({ type: Action.SET_ACTIVE_EQUIPMENT_ID_UI, equipmentId: nextState.params.equipmentId });
  // TODO: When react was updated (HETS-1100) it broke how this worked. We now need to delay
  // mounting the <Route> in order that `mapStateToProps` is called with the current store's state.
  Promise.resolve().then(callback);
}

function keepAlive() {
  Api.keepAlive();
}

window.setInterval(keepAlive, Constant.SESSION_KEEP_ALIVE_INTERVAL);

function showSessionTimoutDialog() {
  store.dispatch({ type: Action.SHOW_SESSION_TIMEOUT_DIALOG });
}

var sessionTimeoutTimer = window.setInterval(showSessionTimoutDialog, Constant.SESSION_TIMEOUT);

export function resetSessionTimeoutTimer() {
  window.clearInterval(sessionTimeoutTimer);
  sessionTimeoutTimer = window.setInterval(showSessionTimoutDialog, Constant.SESSION_TIMEOUT);
}

const App = () => (
  <Provider store={ store }>
    <Router history={ hashHistory }>
      <Redirect from="/" to="/home"/>
      <Route path={ Constant.BUSINESS_PORTAL_PATHNAME } component={ BusinessPortal } onEnter={ onEnterBusiness } />
      <Route path={ `${Constant.BUSINESS_DETAILS_PATHNAME }/:ownerId` } component={ BusinessOwner } onEnter={ onEnterBusinessDetails } />
      <Route path="/" component={ Main } onEnter={ onEnterApplication }>
        <Route path={ Constant.HOME_PATHNAME } component={ Home }/>
        <Route path={ Constant.EQUIPMENT_PATHNAME } component={ Equipment }/>
        <Route path={ `${ Constant.EQUIPMENT_PATHNAME }/:equipmentId` } component={ EquipmentDetail } onEnter={ setActiveEquipmentId }/>
        <Route path={ Constant.OWNERS_PATHNAME } component={ Owners }/>
        <Route path={ `${ Constant.OWNERS_PATHNAME }/:ownerId` } component={ OwnersDetail } onEnter={setActiveOwnerId}/>
        <Route path={ `${ Constant.OWNERS_PATHNAME }/:ownerId/${ Constant.CONTACTS_PATHNAME }/:contactId` } component={ OwnersDetail } onEnter={setActiveOwnerId}/>
        <Route path={ Constant.PROJECTS_PATHNAME } component={ Projects }/>
        <Route path={ `${ Constant.PROJECTS_PATHNAME }/:projectId` } component={ ProjectsDetail } onEnter={ setActiveProjectId }/>
        <Route path={ `${ Constant.PROJECTS_PATHNAME }/:projectId/${ Constant.CONTACTS_PATHNAME }/:contactId` } component={ ProjectsDetail } onEnter={ setActiveProjectId }/>
        <Route path={ Constant.RENTAL_REQUESTS_PATHNAME } component={ RentalRequests }/>
        <Route path={ `${ Constant.RENTAL_REQUESTS_PATHNAME }/:rentalRequestId` } component={ RentalRequestsDetail } onEnter={ setActiveRentalRequestId }/>
        <Route path={ `${ Constant.RENTAL_AGREEMENTS_PATHNAME }/:rentalAgreementId` } component={ RentalAgreementsDetail } onEnter={ setActiveRentalAgreementId }/>
        <Route path={ Constant.OVERTIME_RATES_PATHNAME } component={ OvertimeRates } />
        <Route path={ Constant.USERS_PATHNAME } component={ Users }/>
        <Route path={ `${ Constant.USERS_PATHNAME }/:userId` } component={ UsersDetail }/>
        <Route path={ Constant.ROLES_PATHNAME } component={ Roles }/>
        <Route path={ `${ Constant.ROLES_PATHNAME }/:roleId` } component={ RolesDetail }/>
        <Route path={ Constant.ROLLOVER_PATHNAME } component={ Rollover } />
        <Route path={ Constant.DISTRICT_ADMIN_PATHNAME } component={ DistrictAdmin } />
        <Route path={ Constant.TIME_ENTRY_PATHNAME } component={ TimeEntry } />
        <Route path={ Constant.SENIORITY_LIST_PATHNAME } component={ SeniorityList } />
        <Route path={ Constant.STATUS_LETTERS_REPORT_PATHNAME } component={ StatusLetters } />
        <Route path={ Constant.HIRING_REPORT_PATHNAME } component={ HiringReport } />
        <Route path={ Constant.OWNERS_COVERAGE_PATHNAME } component={ WcbCglCoverage } />
        <Route path={ Constant.AIT_REPORT_PATHNAME } component={ AitReport } />
        <Route path={ Constant.VERSION_PATHNAME } component={ Version }/>
        <Route path="*" component={ FourOhFour }/>
      </Route>
    </Router>
  </Provider>
);


export default hot(App);
