import React, { lazy, Suspense, useEffect, useState } from 'react';
import { connect } from 'react-redux';
import { BrowserRouter as Router, Navigate, Route, Routes } from 'react-router-dom';
import AuthorizedRoute from './components/AuthorizedRoute';
import { withRouter } from './components/withRouter';
import { keycloak } from './Keycloak';

import * as Api from './api';
import { ApiError } from './utils/http';

import * as Constant from './constants';
import * as Action from './actionTypes';
import { store } from './store';

import { ProgressBar } from 'react-bootstrap';
import ErrorBoundary from './components/ErrorBoundary';
import Main from './views/Main.jsx';

const lazyRoute = (loader) => withRouter(lazy(loader));
const Home = lazyRoute(() => import('./views/Home.jsx'));
const BusinessPortal = lazyRoute(() => import('./views/BusinessPortal.jsx'));
const BusinessOwner = lazyRoute(() => import('./views/BusinessOwner.jsx'));
const Equipment = lazyRoute(() => import('./views/Equipment.jsx'));
const EquipmentDetail = lazyRoute(() => import('./views/EquipmentDetail.jsx'));
const Owners = lazyRoute(() => import('./views/Owners.jsx'));
const OwnersDetail = lazyRoute(() => import('./views/OwnersDetail.jsx'));
const Projects = lazyRoute(() => import('./views/Projects.jsx'));
const ProjectsDetail = lazyRoute(() => import('./views/ProjectsDetail.jsx'));
const RentalRequests = lazyRoute(() => import('./views/RentalRequests.jsx'));
const RentalRequestsDetail = lazyRoute(() => import('./views/RentalRequestsDetail.jsx'));
const RentalAgreementsDetail = lazyRoute(() => import('./views/RentalAgreementsDetail.jsx'));
const OvertimeRates = lazyRoute(() => import('./views/OvertimeRates.jsx'));
const Users = lazyRoute(() => import('./views/Users.jsx'));
const UsersDetail = lazyRoute(() => import('./views/UsersDetail.jsx'));
const Roles = lazyRoute(() => import('./views/Roles.jsx'));
const RolesDetail = lazyRoute(() => import('./views/RolesDetail.jsx'));
const Rollover = lazyRoute(() => import('./views/Rollover.jsx'));
const DistrictAdmin = lazyRoute(() => import('./views/DistrictAdmin.jsx'));
const TimeEntry = lazyRoute(() => import('./views/TimeEntry.jsx'));
const SeniorityList = lazyRoute(() => import('./views/SeniorityList.jsx'));
const StatusLetters = lazyRoute(() => import('./views/StatusLetters.jsx'));
const HiringReport = lazyRoute(() => import('./views/HiringReport.jsx'));
const WcbCglCoverage = lazyRoute(() => import('./views/WcbCglCoverage.jsx'));
const AitReport = lazyRoute(() => import('./views/AitReport.jsx'));
const Version = lazyRoute(() => import('./views/Version.jsx'));
const FourOhFour = lazyRoute(() => import('./views/404.jsx'));

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

const buildRoutes = (user) => {
  //render Routes based on user permissions
  if (user.hasPermission(Constant.PERMISSION_BUSINESS_LOGIN)) {
    return BusinessRoutes(user);
  }

  if (user.hasPermission(Constant.PERMISSION_LOGIN)) {
    return AdminRoutes(user);
  }

  return <Route path="*" element={<Navigate replace to={Constant.UNAUTHORIZED_PATHNAME} />} />;
};

const BusinessRoutes = (user) => {
  return (
    <>
      <Route path="/" element={<Navigate replace to={Constant.BUSINESS_PORTAL_PATHNAME} />} />
      <Route path={Constant.BUSINESS_PORTAL_PATHNAME} element={<BusinessPortal />} />
      <Route path={`${Constant.BUSINESS_DETAILS_PATHNAME}/:ownerId`} element={<BusinessOwner />} />
      {CommonRoutes()}
    </>
  );
};

const AdminRoutes = (user) => {
  return (
    <>
      <Route path="/" element={<Navigate replace to={Constant.HOME_PATHNAME} />} />
      <Route path={Constant.HOME_PATHNAME} element={<AuthorizedRoute requires={Constant.PERMISSION_LOGIN} component={Home} />} />
      <Route path={Constant.EQUIPMENT_PATHNAME} element={<AuthorizedRoute requires={Constant.PERMISSION_LOGIN} component={Equipment} />} />
      <Route path={`${Constant.EQUIPMENT_PATHNAME}/:equipmentId`} element={<AuthorizedRoute requires={Constant.PERMISSION_LOGIN} component={EquipmentDetail} />} />
      <Route path={Constant.OWNERS_PATHNAME} element={<AuthorizedRoute requires={Constant.PERMISSION_LOGIN} component={Owners} />} />
      <Route path={`${Constant.OWNERS_PATHNAME}/:ownerId`} element={<AuthorizedRoute requires={Constant.PERMISSION_LOGIN} component={OwnersDetail} />} />
      <Route path={`${Constant.OWNERS_PATHNAME}/:ownerId${Constant.CONTACTS_PATHNAME}/:contactId`} element={<AuthorizedRoute requires={Constant.PERMISSION_LOGIN} component={OwnersDetail} />} />
      <Route path={Constant.PROJECTS_PATHNAME} element={<AuthorizedRoute requires={Constant.PERMISSION_LOGIN} component={Projects} />} />
      <Route path={`${Constant.PROJECTS_PATHNAME}/:projectId`} element={<AuthorizedRoute requires={Constant.PERMISSION_LOGIN} component={ProjectsDetail} />} />
      <Route path={`${Constant.PROJECTS_PATHNAME}/:projectId${Constant.CONTACTS_PATHNAME}/:contactId`} element={<AuthorizedRoute requires={Constant.PERMISSION_LOGIN} component={ProjectsDetail} />} />
      <Route path={Constant.RENTAL_REQUESTS_PATHNAME} element={<AuthorizedRoute requires={Constant.PERMISSION_LOGIN} component={RentalRequests} />} />
      <Route path={`${Constant.RENTAL_REQUESTS_PATHNAME}/:rentalRequestId`} element={<AuthorizedRoute requires={Constant.PERMISSION_LOGIN} component={RentalRequestsDetail} />} />
      <Route path={`${Constant.RENTAL_AGREEMENTS_PATHNAME}/:rentalAgreementId`} element={<AuthorizedRoute requires={Constant.PERMISSION_LOGIN} component={RentalAgreementsDetail} />} />
      <Route path={Constant.OVERTIME_RATES_PATHNAME} element={<AuthorizedRoute requires={Constant.PERMISSION_ADMIN} component={OvertimeRates} />} />
      <Route path={Constant.USERS_PATHNAME} element={<AuthorizedRoute requires={Constant.PERMISSION_USER_MANAGEMENT} component={Users} />} />
      <Route path={`${Constant.USERS_PATHNAME}/:userId`} element={<AuthorizedRoute requires={Constant.PERMISSION_USER_MANAGEMENT} component={UsersDetail} />} />
      <Route path={Constant.ROLES_PATHNAME} element={<AuthorizedRoute requires={Constant.PERMISSION_ROLES_AND_PERMISSIONS} component={Roles} />} />
      <Route path={`${Constant.ROLES_PATHNAME}/:roleId`} element={<AuthorizedRoute requires={Constant.PERMISSION_ROLES_AND_PERMISSIONS} component={RolesDetail} />} />
      <Route path={Constant.ROLLOVER_PATHNAME} element={<AuthorizedRoute requires={Constant.PERMISSION_DISTRICT_ROLLOVER} component={Rollover} />} />
      <Route path={Constant.DISTRICT_ADMIN_PATHNAME} element={<AuthorizedRoute requires={Constant.PERMISSION_DISTRICT_CODE_TABLE_MANAGEMENT} component={DistrictAdmin} />} />
      <Route path={Constant.TIME_ENTRY_PATHNAME} element={<AuthorizedRoute requires={Constant.PERMISSION_LOGIN} component={TimeEntry} />} />
      <Route path={Constant.SENIORITY_LIST_PATHNAME} element={<AuthorizedRoute requires={Constant.PERMISSION_LOGIN} component={SeniorityList} />} />
      <Route path={Constant.STATUS_LETTERS_REPORT_PATHNAME} element={<AuthorizedRoute requires={Constant.PERMISSION_LOGIN} component={StatusLetters} />} />
      <Route path={Constant.HIRING_REPORT_PATHNAME} element={<AuthorizedRoute requires={Constant.PERMISSION_LOGIN} component={HiringReport} />} />
      <Route path={Constant.OWNERS_COVERAGE_PATHNAME} element={<AuthorizedRoute requires={Constant.PERMISSION_LOGIN} component={WcbCglCoverage} />} />
      <Route path={Constant.AIT_REPORT_PATHNAME} element={<AuthorizedRoute requires={Constant.PERMISSION_LOGIN} component={AitReport} />} />
      <Route path={Constant.VERSION_PATHNAME} element={<AuthorizedRoute requires={Constant.PERMISSION_VERSION} component={Version} />} />
      {CommonRoutes()}
    </>
  );
};

const CommonRoutes = () => {
  return (
    <>
      <Route path={Constant.UNAUTHORIZED_PATHNAME} element={<Unauthorized />} />
      <Route path="*" element={<FourOhFour />} />
    </>
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
            <Routes>{buildRoutes(user)}</Routes>
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
