import React, { lazy, Suspense, useEffect, useState } from 'react';
import { connect } from 'react-redux';
import { BrowserRouter as Router, Route, Redirect, Switch } from 'react-router-dom';
import AuthorizedRoute from './components/AuthorizedRoute';
import { keycloak } from './Keycloak';

import * as Api from './api';
import { ApiError } from './utils/http';

import * as Constant from './constants';
import * as Action from './actionTypes';
import { store } from './store';

import { ProgressBar } from 'react-bootstrap';
import ErrorBoundary from './components/ErrorBoundary';
import Main from './views/Main.jsx';

const Home = lazy(() => import('./views/Home.jsx'));
const BusinessPortal = lazy(() => import('./views/BusinessPortal.jsx'));
const BusinessOwner = lazy(() => import('./views/BusinessOwner.jsx'));
const Equipment = lazy(() => import('./views/Equipment.jsx'));
const EquipmentDetail = lazy(() => import('./views/EquipmentDetail.jsx'));
const Owners = lazy(() => import('./views/Owners.jsx'));
const OwnersDetail = lazy(() => import('./views/OwnersDetail.jsx'));
const Projects = lazy(() => import('./views/Projects.jsx'));
const ProjectsDetail = lazy(() => import('./views/ProjectsDetail.jsx'));
const RentalRequests = lazy(() => import('./views/RentalRequests.jsx'));
const RentalRequestsDetail = lazy(() => import('./views/RentalRequestsDetail.jsx'));
const RentalAgreementsDetail = lazy(() => import('./views/RentalAgreementsDetail.jsx'));
const OvertimeRates = lazy(() => import('./views/OvertimeRates.jsx'));
const Users = lazy(() => import('./views/Users.jsx'));
const UsersDetail = lazy(() => import('./views/UsersDetail.jsx'));
const Roles = lazy(() => import('./views/Roles.jsx'));
const RolesDetail = lazy(() => import('./views/RolesDetail.jsx'));
const Rollover = lazy(() => import('./views/Rollover.jsx'));
const DistrictAdmin = lazy(() => import('./views/DistrictAdmin.jsx'));
const TimeEntry = lazy(() => import('./views/TimeEntry.jsx'));
const SeniorityList = lazy(() => import('./views/SeniorityList.jsx'));
const StatusLetters = lazy(() => import('./views/StatusLetters.jsx'));
const HiringReport = lazy(() => import('./views/HiringReport.jsx'));
const WcbCglCoverage = lazy(() => import('./views/WcbCglCoverage.jsx'));
const AitReport = lazy(() => import('./views/AitReport.jsx'));
const Version = lazy(() => import('./views/Version.jsx'));
const FourOhFour = lazy(() => import('./views/404.jsx'));

import addIconsToLibrary from './fontAwesome';

export const keepAlive = async () => {
  try {
    await keycloak.updateToken(70);
  } catch {
    console.log('Failed to refresh the token, or the session has expired');
  }
};

window.setInterval(keepAlive, Constant.SESSION_KEEP_ALIVE_INTERVAL);

const showSessionTimoutDialog = () => {
  store.dispatch({ type: Action.SHOW_SESSION_TIMEOUT_DIALOG });
};

var sessionTimeoutTimer = window.setInterval(showSessionTimoutDialog, Constant.SESSION_TIMEOUT);

export const resetSessionTimeoutTimer = () => {
  window.clearInterval(sessionTimeoutTimer);
  sessionTimeoutTimer = window.setInterval(showSessionTimoutDialog, Constant.SESSION_TIMEOUT);
};

export const getLookups = (user) => (dispatch) => {
  if (user.businessUser) {
    return Promise.resolve();
  }

  const districtId = user.district.id;
  return Promise.all([
    dispatch(Api.getDistricts()),
    dispatch(Api.getRegions()),
    dispatch(Api.getServiceAreas()),
    dispatch(Api.getLocalAreas(districtId)),
    dispatch(Api.getFiscalYears(districtId)),
    dispatch(Api.getPermissions()),
    dispatch(Api.getCurrentUserDistricts()),
    dispatch(Api.getFavourites()),
  ]);
};

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
      <Route exact path="/">
        <Redirect to={Constant.BUSINESS_PORTAL_PATHNAME} />
      </Route>
      <Route path={Constant.BUSINESS_PORTAL_PATHNAME} exact component={BusinessPortal} />
      <Route path={`${Constant.BUSINESS_DETAILS_PATHNAME}/:ownerId`} component={BusinessOwner} />
      {CommonRoutes()}
    </Switch>
  );
};

const AdminRoutes = (user) => {
  return (
    <Switch>
      <Route exact path="/">
        <Redirect to={Constant.HOME_PATHNAME} />
      </Route>
      <AuthorizedRoute requires={Constant.PERMISSION_LOGIN} path={Constant.HOME_PATHNAME} exact component={Home} />
      <AuthorizedRoute
        requires={Constant.PERMISSION_LOGIN}
        path={Constant.EQUIPMENT_PATHNAME}
        exact
        component={Equipment}
      />
      <AuthorizedRoute
        requires={Constant.PERMISSION_LOGIN}
        path={`${Constant.EQUIPMENT_PATHNAME}/:equipmentId`}
        exact
        component={EquipmentDetail}
      />
      <AuthorizedRoute requires={Constant.PERMISSION_LOGIN} path={Constant.OWNERS_PATHNAME} exact component={Owners} />
      <AuthorizedRoute
        requires={Constant.PERMISSION_LOGIN}
        path={`${Constant.OWNERS_PATHNAME}/:ownerId`}
        exact
        component={OwnersDetail}
      />
      <AuthorizedRoute
        requires={Constant.PERMISSION_LOGIN}
        path={`${Constant.OWNERS_PATHNAME}/:ownerId${Constant.CONTACTS_PATHNAME}/:contactId`}
        exact
        component={OwnersDetail}
      />
      <AuthorizedRoute
        requires={Constant.PERMISSION_LOGIN}
        path={Constant.PROJECTS_PATHNAME}
        exact
        component={Projects}
      />
      <AuthorizedRoute
        requires={Constant.PERMISSION_LOGIN}
        path={`${Constant.PROJECTS_PATHNAME}/:projectId`}
        exact
        component={ProjectsDetail}
      />
      <AuthorizedRoute
        requires={Constant.PERMISSION_LOGIN}
        path={`${Constant.PROJECTS_PATHNAME}/:projectId${Constant.CONTACTS_PATHNAME}/:contactId`}
        exact
        component={ProjectsDetail}
      />
      <AuthorizedRoute
        requires={Constant.PERMISSION_LOGIN}
        path={Constant.RENTAL_REQUESTS_PATHNAME}
        exact
        component={RentalRequests}
      />
      <AuthorizedRoute
        requires={Constant.PERMISSION_LOGIN}
        path={`${Constant.RENTAL_REQUESTS_PATHNAME}/:rentalRequestId`}
        exact
        component={RentalRequestsDetail}
      />
      <AuthorizedRoute
        requires={Constant.PERMISSION_LOGIN}
        path={`${Constant.RENTAL_AGREEMENTS_PATHNAME}/:rentalAgreementId`}
        exact
        component={RentalAgreementsDetail}
      />
      <AuthorizedRoute
        requires={Constant.PERMISSION_ADMIN}
        path={Constant.OVERTIME_RATES_PATHNAME}
        exact
        component={OvertimeRates}
      />
      <AuthorizedRoute
        requires={Constant.PERMISSION_USER_MANAGEMENT}
        path={Constant.USERS_PATHNAME}
        exact
        component={Users}
      />
      <AuthorizedRoute
        requires={Constant.PERMISSION_USER_MANAGEMENT}
        path={`${Constant.USERS_PATHNAME}/:userId`}
        exact
        component={UsersDetail}
      />
      <AuthorizedRoute
        requires={Constant.PERMISSION_ROLES_AND_PERMISSIONS}
        path={Constant.ROLES_PATHNAME}
        exact
        component={Roles}
      />
      <AuthorizedRoute
        requires={Constant.PERMISSION_ROLES_AND_PERMISSIONS}
        path={`${Constant.ROLES_PATHNAME}/:roleId`}
        exact
        component={RolesDetail}
      />
      <AuthorizedRoute
        requires={Constant.PERMISSION_DISTRICT_ROLLOVER}
        path={Constant.ROLLOVER_PATHNAME}
        component={Rollover}
      />
      <AuthorizedRoute
        requires={Constant.PERMISSION_DISTRICT_CODE_TABLE_MANAGEMENT}
        path={Constant.DISTRICT_ADMIN_PATHNAME}
        component={DistrictAdmin}
      />
      <AuthorizedRoute requires={Constant.PERMISSION_LOGIN} path={Constant.TIME_ENTRY_PATHNAME} component={TimeEntry} />
      <AuthorizedRoute
        requires={Constant.PERMISSION_LOGIN}
        path={Constant.SENIORITY_LIST_PATHNAME}
        component={SeniorityList}
      />
      <AuthorizedRoute
        requires={Constant.PERMISSION_LOGIN}
        path={Constant.STATUS_LETTERS_REPORT_PATHNAME}
        component={StatusLetters}
      />
      <AuthorizedRoute
        requires={Constant.PERMISSION_LOGIN}
        path={Constant.HIRING_REPORT_PATHNAME}
        component={HiringReport}
      />
      <AuthorizedRoute
        requires={Constant.PERMISSION_LOGIN}
        path={Constant.OWNERS_COVERAGE_PATHNAME}
        component={WcbCglCoverage}
      />
      <AuthorizedRoute requires={Constant.PERMISSION_LOGIN} path={Constant.AIT_REPORT_PATHNAME} component={AitReport} />
      <AuthorizedRoute requires={Constant.PERMISSION_VERSION} path={Constant.VERSION_PATHNAME} component={Version} />
      {CommonRoutes()}
    </Switch>
  );
};

const CommonRoutes = () => {
  return (
    <Switch>
      <Route path={Constant.UNAUTHORIZED_PATHNAME} component={Unauthorized} />
      <Route path="*" component={FourOhFour} />
    </Switch>
  );
};

//additional components
const Unauthorized = () => <>Unauthorized</>;

const App = ({ user, dispatch }) => {
  const [loading, setLoading] = useState(true);
  const [apiError, setApiError] = useState(null);
  const [loadProgress, setLoadProgress] = useState(5);

  useEffect(() => {
    addIconsToLibrary();
    setLoadProgress(33);
    dispatch(Api.getCurrentUser())
      .then((user) => {
        setLoadProgress(75);
        return dispatch(getLookups(user));
      })
      .then(() => {
        setLoading(false);
      })
      .catch((error) => {
        console.error(error);

        if (error instanceof ApiError) {
          setApiError(error.message);
        }
      });
  }, []);

  if (loading)
    return (
      <div id="initialization">
        <p id="init-message">Loading HETS&hellip;</p>

        <ProgressBar
          variant={apiError === null ? 'info' : 'danger'}
          striped
          animated
          now={loadProgress}
          min={0}
          max={100}
          label={`${loadProgress}% Complete`}
        ></ProgressBar>

        <div id="init-error">
          {apiError != null && (
            <div id="loading-error-message">
              <h4>Error loading application</h4>
              <p>{apiError}</p>
              <a href="mailto:tranit@gov.bc.ca" target="_top">
                Email TRANIT
              </a>
            </div>
          )}
        </div>
      </div>
    );

  return (
    <Router>
      <Main showNav={user.hasPermission(Constant.PERMISSION_LOGIN)}>
        <ErrorBoundary>
          <Suspense fallback={<p id="route-loading">Loading page&hellip;</p>}>
            <Switch>{Routes(user)}</Switch>
          </Suspense>
        </ErrorBoundary>
      </Main>
    </Router>
  );
};

const mapStateToProps = (state) => ({
  user: state.user,
});

const mapDispatchToProps = (dispatch) => ({ dispatch });

export default connect(mapStateToProps, mapDispatchToProps)(App);
