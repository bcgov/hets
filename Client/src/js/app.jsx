import React from 'react';
import { Provider } from 'react-redux';
import { Router, Route, Redirect, hashHistory } from 'react-router';

import * as Constant from './constants';
import store from './store';

import Main from './views/Main.jsx';
import Home from './views/Home.jsx';
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
import Version from './views/Version.jsx';
import FourOhFour from './views/404.jsx';

const hasPermission = () => {
  if (store.getState().user.hasPermission(Constant.PERMISSION_BUSINESS_LOGIN)) {
    hashHistory.push(Constant.BUSINESS_LOGIN);
  } 
};

const App = <Provider store={ store }>
  <Router history={ hashHistory }>
    <Redirect from="/" to="/home"/>
    <Route path={ Constant.BUSINESS_LOGIN }/>
    <Route path="/" component={ Main } onEnter={hasPermission}>
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
      <Route path={ Constant.VERSION_PATHNAME } component={ Version }/>
      <Route path="*" component={ FourOhFour }/>
    </Route>
  </Router>
</Provider>;


export default App;
