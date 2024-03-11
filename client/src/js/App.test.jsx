/* eslint-disable testing-library/no-node-access */
/* eslint-disable testing-library/no-container */
import { rest } from 'msw';
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
  rest.get("/api/users/current", (_, res, ctx) => {
    return res(ctx.json(getCurrentUser()));
  }),
  rest.get("/api/districts", (_, res, ctx) => {
    return res(ctx.json(getDistricts()));
  }),
  rest.get("/api/regions", (_, res, ctx) => {
    return res(ctx.json(getRegions()));
  }),
  rest.get("/api/serviceareas", (_, res, ctx) => {
    return res(ctx.json(getServiceAreas()));
  }),
  rest.get("/api/districts/:district/localAreas", (_, res, ctx) => {
    return res(ctx.json(getLocalAreas()));
  }),
  rest.get("/api/districts/:district/fiscalYears", (_, res, ctx) => {
    return res(ctx.json(getFiscalYears()));
  }),
  rest.get("/api/permissions", (_, res, ctx) => {
    return res(ctx.json(getPermissions()));
  }),
  rest.get("/api/userdistricts", (_, res, ctx) => {
    return res(ctx.json(getUserDistricts()));
  }),
  rest.get("/api/users/current/favourites", (_, res, ctx) => {
    return res(ctx.json(getCurrentUserFavourites()));
  }),
  rest.get("/api/districts/:district/rolloverStatus", (_, res, ctx) => {
    return res(ctx.json(getRolloverStatus()));
  }),
  rest.get("/api/counts", (_, res, ctx) => {
    return res(ctx.json(getCounts()));
  }),
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
