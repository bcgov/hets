/* eslint-disable testing-library/no-node-access */
/* eslint-disable testing-library/no-container */
import { http, HttpResponse } from 'msw';
import { setupServer } from "msw/node";
import { waitFor } from '@testing-library/react';

import App from './App';
import { keycloak } from './Keycloak';
import { renderWithProviders } from './renderWithProviders';
import { setupStore } from './store';
import { getCurrentUser, getCurrentUserFavourites } from './mock/api/getCurrentUser';
import { getRegions } from './mock/api/getRegions';
import { getDistricts } from './mock/api/getDistricts';
import { getServiceAreas } from './mock/api/getServiceAreas';
import { getLocalAreas } from './mock/api/getLocalAreas';
import { getFiscalYears } from './mock/api/getFiscalYears';
import { getPermissions } from './mock/api/getPermissions';
import { getUserDistricts } from './mock/api/getUserDistricts';
import { getRolloverStatus } from './mock/api/getRolloverStatus';
import { getCounts } from './mock/api/getCounts';

// Mock Keycloak service
const mockKeycloakUpdateToken = jest.spyOn(keycloak, "updateToken");
mockKeycloakUpdateToken.mockResolvedValue(Promise.resolve(false));

const server = setupServer(
  http.get("/api/users/current", () => HttpResponse.json(getCurrentUser())),
  http.get("/api/districts", () => HttpResponse.json(getDistricts())),
  http.get("/api/regions", () => HttpResponse.json(getRegions())),
  http.get("/api/serviceareas", () => HttpResponse.json(getServiceAreas())),
  http.get("/api/districts/:district/localAreas", () => HttpResponse.json(getLocalAreas())),
  http.get("/api/districts/:district/fiscalYears", () => HttpResponse.json(getFiscalYears())),
  http.get("/api/permissions", () => HttpResponse.json(getPermissions())),
  http.get("/api/userdistricts", () => HttpResponse.json(getUserDistricts())),
  http.get("/api/users/current/favourites", () => HttpResponse.json(getCurrentUserFavourites())),
  http.get("/api/districts/:district/rolloverStatus", () => HttpResponse.json(getRolloverStatus())),
  http.get("/api/counts", () => HttpResponse.json(getCounts())),
);

let store;

beforeAll(() => {
  server.listen();
});

beforeEach(() => {
  store = setupStore(); 
})

afterEach(() => {
  server.resetHandlers();
});

afterAll(() => {
  server.close();
});

test('renders main component', async () => {
  const view = renderWithProviders(<App />, { store });
  await waitFor(() => {
    const loadingComponent = view.container.querySelector("#initialization");
    expect(loadingComponent).not.toBeInTheDocument();
  });

  const mainComponent = view.container.querySelector("#main");
  expect(mainComponent).toBeInTheDocument();
});
