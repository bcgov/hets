import React from 'react';
import { Provider } from 'react-redux';
import { Router, Route, Redirect, hashHistory } from 'react-router';

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
import Users from './views/Users.jsx';
import UsersDetail from './views/UsersDetail.jsx';
import Roles from './views/Roles.jsx';
import RolesDetail from './views/RolesDetail.jsx';
import Version from './views/Version.jsx';
import FourOhFour from './views/404.jsx';


const App = <Provider store={ store }>
  <Router history={ hashHistory }>
    <Redirect from="/" to="/home"/>
    <Route path="/" component={ Main }>
      <Route path="home" component={ Home }/>
      <Route path="equipment" component={ Equipment }/>
      <Route path="equipment/:equipmentId" component={ EquipmentDetail }/>
      <Route path="owners" component={ Owners }/>
      <Route path="owners/:ownerId" component={ OwnersDetail }/>
      <Route path="projects" component={ Projects }/>
      <Route path="projects/:projectId" component={ ProjectsDetail }/>
      <Route path="rentalrequests" component={ RentalRequests }/>
      <Route path="rentalrequests/:rentalRequestId" component={ RentalRequestsDetail }/>
      <Route path="users" component={ Users }/>
      <Route path="users/:userId" component={ UsersDetail }/>
      <Route path="roles" component={ Roles }/>
      <Route path="roles/:roleId" component={ RolesDetail }/>
      <Route path="version" component={ Version }/>
      <Route path="*" component={ FourOhFour }/>
    </Route>
  </Router>
</Provider>;


export default App;
