import React from 'react';
import { Provider } from 'react-redux';
import { Router, Route, Redirect, hashHistory } from 'react-router';

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
import Users from './views/Users.jsx';
import UsersDetail from './views/UsersDetail.jsx';
import Roles from './views/Roles.jsx';
import RolesDetail from './views/RolesDetail.jsx';
import Rollover from './views/Rollover.jsx';
import DistrictAdmin from './views/DistrictAdmin.jsx';
import Version from './views/Version.jsx';
import FourOhFour from './views/404.jsx';

hashHistory.listen(location =>  {
  if (location.action !== 'POP') {
    return;
  }

  redirectIfRolloverActive(location.pathname);
});

// redirects regular users to rollover page if rollover in progress
function redirectIfRolloverActive(path) { 
  var onBusinessPage = path.indexOf(Constant.BUSINESS_PORTAL_PATHNAME) === 0;
  var onRolloverPage = path === '/' + Constant.ROLLOVER_PATHNAME;
  if (onBusinessPage || onRolloverPage) {
    return;
  }
  
  var user = store.getState().user;
  if (!user.district) {
    return;
  }
  
  Api.getRolloverStatus(user.district.id).then(() => {
    var rolloverActive = store.getState().lookups.rolloverStatus.rolloverActive;
    if (rolloverActive) {
      hashHistory.push('/' + Constant.ROLLOVER_PATHNAME);
    }
  });
}

function onEnterBusiness() {
  // allow access to business users
  if (store.getState().user.hasPermission(Constant.PERMISSION_BUSINESS_LOGIN)) {
    return;
  }

  // redirect HETS users to home page
  if (store.getState().user.hasPermission(Constant.PERMISSION_LOGIN)) {
    hashHistory.push('/');
  }
  
  // TODO: redirect other users to 'unauthorized access' page
  //hashHistory.push('/');
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

export var showSessionTimoutDialog = function() {
  store.dispatch({ type: Action.SHOW_SESSION_TIMEOUT_DIALOG });
};

export var sessionTimer = window.setInterval(showSessionTimoutDialog, Constant.SESSION_TIMOUT);

export function setTimerInterval() {
  sessionTimer = window.setInterval(showSessionTimoutDialog, Constant.SESSION_TIMOUT);
}

const App = <Provider store={ store }>
  <Router history={ hashHistory }>
    <Redirect from="/" to="/home"/>
    <Route path={ Constant.BUSINESS_PORTAL_PATHNAME } component={ BusinessPortal } onEnter={ onEnterBusiness } />
    <Route path={ `${Constant.BUSINESS_DETAILS_PATHNAME }/:ownerId` } component={ BusinessOwner } onEnter={ onEnterBusiness } />
    <Route path="/" component={ Main } onEnter={ onEnterApplication }>
      <Route path={ Constant.HOME_PATHNAME } component={ Home }/>
      <Route path={ Constant.EQUIPMENT_PATHNAME } component={ Equipment }/>
      <Route path={ `${ Constant.EQUIPMENT_PATHNAME }/:equipmentId` } component={ EquipmentDetail }/>
      <Route path={ Constant.OWNERS_PATHNAME } component={ Owners }/>
      <Route path={ `${ Constant.OWNERS_PATHNAME }/:ownerId` } component={ OwnersDetail }/>
      <Route path={ `${ Constant.OWNERS_PATHNAME }/:ownerId/${ Constant.CONTACTS_PATHNAME }/:contactId` } component={ OwnersDetail }/>
      <Route path={ Constant.PROJECTS_PATHNAME } component={ Projects }/>
      <Route path={ `${ Constant.PROJECTS_PATHNAME }/:projectId` } component={ ProjectsDetail }/>
      <Route path={ `${ Constant.PROJECTS_PATHNAME }/:projectId/${ Constant.CONTACTS_PATHNAME }/:contactId` } component={ ProjectsDetail }/>
      <Route path={ Constant.RENTAL_REQUESTS_PATHNAME } component={ RentalRequests }/>
      <Route path={ `${ Constant.RENTAL_REQUESTS_PATHNAME }/:rentalRequestId` } component={ RentalRequestsDetail }/>
      <Route path={ `${ Constant.RENTAL_AGREEMENTS_PATHNAME }/:rentalAgreementId` } component={ RentalAgreementsDetail }/>
      <Route path={ Constant.USERS_PATHNAME } component={ Users }/>
      <Route path={ `${ Constant.USERS_PATHNAME }/:userId` } component={ UsersDetail }/>
      <Route path={ Constant.ROLES_PATHNAME } component={ Roles }/>
      <Route path={ `${ Constant.ROLES_PATHNAME }/:roleId` } component={ RolesDetail }/>
      <Route path={ Constant.ROLLOVER_PATHNAME } component={ Rollover } />
      <Route path={ Constant.DISTRICT_ADMIN_PATHNAME } component={ DistrictAdmin } />
      <Route path={ Constant.VERSION_PATHNAME } component={ Version }/>
      <Route path="*" component={ FourOhFour }/>
    </Route>
  </Router>
</Provider>;


export default App;
