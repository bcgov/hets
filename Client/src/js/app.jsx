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
import Notifications from './views/Notifications.jsx';
import UserManagement from './views/UserManagement.jsx';
import UserManagementEdit from './views/UserManagementEdit.jsx';
import RolesPermissions from './views/RolesPermissions.jsx';
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
      <Route path="notifications" component={ Notifications }/>
      <Route path="user-management" component={ UserManagement }/>
      <Route path="user-management/:userId" component={ UserManagementEdit }/>
      <Route path="roles-permissions" component={ RolesPermissions }/>
      <Route path="version" component={ Version }/>
      <Route path="*" component={ FourOhFour }/>
    </Route>
  </Router>
</Provider>;


export default App;
