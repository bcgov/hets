import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import { BrowserRouter as Router, Route, Redirect, Switch } from 'react-router-dom';

import * as Api from './api';
import { ApiError } from './utils/http';

import * as Constant from './constants';
import * as Action from './actionTypes';
import store from './store';

import { ProgressBar } from 'react-bootstrap';
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

export function getLookups(user) {
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

const Routes = (user) => {
  //render Routes based on user permissions
  if (user.hasPermission(Constant.PERMISSION_BUSINESS_LOGIN)) {
    return BusinessRoutes(user);
  }

  if (user.hasPermission(Constant.PERMISSION_LOGIN)) {
    return AdminRoutes(user);
  }

  return <Redirect to={Constant.UNAUTHORIZED_PATHNAME} />;
};

const BusinessRoutes = (user) => {
  return (
    <Switch>
      <Route path={Constant.BUSINESS_PORTAL_PATHNAME} component={BusinessPortal} />
      <Route path={`${Constant.BUSINESS_DETAILS_PATHNAME}/:ownerId`} component={BusinessOwner} />
      {CommonRoutes()}
    </Switch>
  );
};

const AdminRoutes = (user) => {
  return (
    <Main>
      <Switch>
        <Route path={Constant.HOME_PATHNAME} exact component={Home} />
        <Route path={Constant.EQUIPMENT_PATHNAME} exact component={Equipment} />
        <Route path={`${Constant.EQUIPMENT_PATHNAME}/:equipmentId`} exact component={EquipmentDetail} />
        <Route path={Constant.OWNERS_PATHNAME} exact component={Owners} />
        <Route path={`${Constant.OWNERS_PATHNAME}/:ownerId`} exact component={OwnersDetail} />
        <Route
          path={`${Constant.OWNERS_PATHNAME}/:ownerId${Constant.CONTACTS_PATHNAME}/:contactId`}
          exact
          component={OwnersDetail}
        />
        <Route path={Constant.PROJECTS_PATHNAME} exact component={Projects} />
        <Route path={`${Constant.PROJECTS_PATHNAME}/:projectId`} exact component={ProjectsDetail} />
        <Route
          path={`${Constant.PROJECTS_PATHNAME}/:projectId${Constant.CONTACTS_PATHNAME}/:contactId`}
          exact
          component={ProjectsDetail}
        />
        <Route path={Constant.RENTAL_REQUESTS_PATHNAME} exact component={RentalRequests} />
        <Route path={`${Constant.RENTAL_REQUESTS_PATHNAME}/:rentalRequestId`} exact component={RentalRequestsDetail} />
        <Route
          path={`${Constant.RENTAL_AGREEMENTS_PATHNAME}/:rentalAgreementId`}
          exact
          component={RentalAgreementsDetail}
        />
        <Route path={Constant.OVERTIME_RATES_PATHNAME} exact component={OvertimeRates} />
        <Route path={Constant.USERS_PATHNAME} exact component={Users} />
        <Route path={`${Constant.USERS_PATHNAME}/:userId`} exact component={UsersDetail} />
        <Route path={Constant.ROLES_PATHNAME} exact component={Roles} />
        <Route path={`${Constant.ROLES_PATHNAME}/:roleId`} exact component={RolesDetail} />
        <Route path={Constant.ROLLOVER_PATHNAME} component={Rollover} />
        <Route path={Constant.DISTRICT_ADMIN_PATHNAME} component={DistrictAdmin} />
        <Route path={Constant.TIME_ENTRY_PATHNAME} component={TimeEntry} />
        <Route path={Constant.SENIORITY_LIST_PATHNAME} component={SeniorityList} />
        <Route path={Constant.STATUS_LETTERS_REPORT_PATHNAME} component={StatusLetters} />
        <Route path={Constant.HIRING_REPORT_PATHNAME} component={HiringReport} />
        <Route path={Constant.OWNERS_COVERAGE_PATHNAME} component={WcbCglCoverage} />
        <Route path={Constant.AIT_REPORT_PATHNAME} component={AitReport} />
        <Route path={Constant.VERSION_PATHNAME} component={Version} />
        {CommonRoutes()}
      </Switch>
    </Main>
  );
};

const CommonRoutes = () => {
  return (
    <>
      <Route path={Constant.UNAUTHORIZED_PATHNAME} components={Unauthorized} />
      <Route path="*" component={FourOhFour} />{' '}
    </>
  );
};

//additional components
const Unauthorized = () => <>Unauthorized</>;

const App = ({ user }) => {
  const [loading, setLoading] = useState(true);
  const [apiError, setApiError] = useState(null);
  const [loadProgress, setLoadProgress] = useState(5);

  useEffect(() => {
    Api.getCurrentUser()
      .then((user) => {
        setLoadProgress(33);

        return getLookups(user);
      })
      .then(() => {
        setLoadProgress(75);
        setLoading(false);
      })
      .catch((error) => {
        console.log(error);

        if (error instanceof ApiError) {
          setApiError(error.message);
        }
      });
  }, []);

  if (loading)
    return (
      <div id="initialization">
        <p id="init-message">Loading HETS&hellip;</p>

        <div id="init-process" className="progress">
          <ProgressBar
            variant={apiError === null ? 'info' : 'danger'}
            striped
            className={apiError === null ? 'active' : 'progress-bar-danger'}
            now={loadProgress}
            min={0}
            max={100}
            label={`${loadProgress}% Complete`}
            srOnly
          ></ProgressBar>
        </div>

        <div id="init-error">
          {apiError != null && (
            <div id="loading-error-message">
              <h4>Error loading application</h4>
              <p>
                {apiError}
                <a href="mailto:tranit@gov.bc.ca" target="_top">
                  TRANIT
                </a>
              </p>
            </div>
          )}
        </div>
      </div>
    );

  return (
    <Router>
      <Route exact path="/">
        <Redirect to="/home" />
      </Route>
      {/* <Main> */}
      <Switch>{Routes(user)}</Switch>
      {/* </Main> */}
    </Router>
  );
};

const mapStateToProps = (state) => {
  return {
    user: state.user,
  };
};

export default connect(mapStateToProps, null)(App);
