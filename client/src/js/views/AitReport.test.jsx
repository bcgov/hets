import { rest } from "msw";
import { setupServer } from "msw/lib/node";
import Moment from "moment";

import { setupStore } from "../store";
import * as Action from "../actionTypes";
import { dateIsBetween, endOfCurrentFiscal, endOfPreviousFiscal, startOfCurrentFiscal, startOfPreviousFiscal } from "../utils/date";
import { getProjects } from "../mock/api/AitReport/getProjects";
import { getEquipments } from "../mock/api/AitReport/getEquipments";
import { getEquipmentTypes } from "../mock/api/AitReport/getEquipmentTypes";
import { getRentalAgreementsLite } from "../mock/api/AitReport/getRentalAgreementsLite";
import { searchAitReport } from "../mock/api/AitReport/searchAitReport";
import { keycloak } from "../Keycloak";
import { renderWithProviders } from "../renderWithProviders";
import AitReport from "./AitReport";
import { screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { getCurrentUser } from "../mock/api/getCurrentUser";
import { arrayToSet, hasArrsIntersect } from "../utils/set";
import { setToArray } from "../utils/set";

// Mock Keycloak service
const mockKeycloakUpdateToken = jest.spyOn(keycloak, "updateToken");
mockKeycloakUpdateToken.mockResolvedValue(Promise.resolve(false));

const getStartCurrFiscal = () => {
  const currDateTime = Moment();
  return startOfCurrentFiscal(currDateTime);
};

const getEndCurrFiscal = () => {
  const currDateTime = Moment();
  return endOfCurrentFiscal(currDateTime);
};

const getStartLastFiscal = () => {
  const currDateTime = Moment();
  return startOfPreviousFiscal(currDateTime);
};

const getEndLastFiscal = () => {
  const currDateTime = Moment();
  return endOfPreviousFiscal(currDateTime);
};

const server = setupServer(
  rest.get("/api/projects/agreementSummary", async (_, res, ctx) => {
    return res(ctx.json(getProjects()));
  }),
  rest.get("/api/equipment/agreementSummary", async (_, res, ctx) => {
    return res(ctx.json(getEquipments()));
  }),
  rest.get("/api/districtequipmenttypes/agreementSummary", async (_, res, ctx) => {
    return res(ctx.json(getEquipmentTypes()));
  }),
  rest.get("/api/rentalagreements/summaryLite", async (_, res, ctx) => {
    return res(ctx.json(getRentalAgreementsLite(getStartCurrFiscal())));
  }),
  rest.get("/api/rentalAgreements/aitReport", async (_, res, ctx) => {
    return res(ctx.json(searchAitReport(getStartCurrFiscal())));
  }),
);

let store;

beforeAll(() => {
  server.listen();
});

beforeEach(() => {
  store = setupStore();
  const authorizedUser = getCurrentUser().data;
  authorizedUser.hasPermission = (_) => true;

  store.dispatch({
    type: Action.UPDATE_CURRENT_USER,
    user: authorizedUser,
  });
})

afterEach(() => {
  server.resetHandlers();
});

afterAll(() => {
  server.close();
});

describe("AitReport rendering filters", () => {
  test("AitReport renders date filters correctly", async () => {
    const user = userEvent.setup();
    renderWithProviders(<AitReport />, { store });

    await waitFor(() => {
      expect(store.getState().lookups.agreementSummaryLite.data).not.toEqual({});
    });
    
    const dateFilterBtn = await screen.findByText(/This Fiscal/i);
    await user.click(dateFilterBtn);

    const thisFiscalSels = await screen.findAllByText(/This Fiscal/i);
    expect(thisFiscalSels).toHaveLength(2);
    const lastFiscalSel = await screen.findByText(/Last Fiscal/i);
    expect(lastFiscalSel).not.toBeNull();
    const customDateFilterSel = await screen.findByText(/Custom/i);
    expect(customDateFilterSel).not.toBeNull();
  });

  test("Other filters render correctly with This Fiscal date filter", async () => {
    const user = userEvent.setup();
    renderWithProviders(<AitReport />, { store });

    const dateFilterBtn = await screen.findByText(/This Fiscal/i);
    await user.click(dateFilterBtn);

    const startCurrFiscal = getStartCurrFiscal();
    const endCurrFiscal = getEndCurrFiscal();
    const agreementIds = getRentalAgreementsLite(startCurrFiscal).data
      .filter(agreement => dateIsBetween(Moment.utc(agreement.datedOn), Moment.utc(startCurrFiscal), Moment.utc(endCurrFiscal)))
      .map(agreement => agreement.id);
    
    const projects = getProjects().data.filter(proj => hasArrsIntersect(proj.agreementIds, agreementIds));
    const projectIds = setToArray(arrayToSet(projects.map(proj => proj.id)));
    const equipmentTypes = getEquipmentTypes().data
      .filter(eqType => hasArrsIntersect(eqType.agreementIds, agreementIds));
    const equipmentTypeIds = setToArray(arrayToSet(equipmentTypes.map(eqType => eqType.id)));
    const equipments = getEquipments().data
      .filter(eq => hasArrsIntersect(eq.agreementIds, agreementIds));
    const equipmentIds = setToArray(arrayToSet(equipments.map(equipment => equipment.id)));

    const projectsBtn = screen.getByText(/Projects/i);
    await user.click(projectsBtn);
    expect(screen.queryAllByText(/Test Project \d+/i)).toHaveLength(projectIds.length);

    const equipmentTypesBtn = screen.getByText(/Equipment Types/i);
    await user.click(equipmentTypesBtn);
    expect(screen.queryAllByText(/Eq Type \d+/i)).toHaveLength(equipmentTypeIds.length);

    const equipmentsBtn = screen.getByText("Equipment");
    await user.click(equipmentsBtn);
    expect(screen.queryAllByText(/EC\d+/i)).toHaveLength(equipmentIds.length);
  });

  test("Other filters render correctly with Last Fiscal date filter", async () => {
    const user = userEvent.setup();
    renderWithProviders(<AitReport />, { store });

    const dateFilterBtn = await screen.findByText(/This Fiscal/i);
    await user.click(dateFilterBtn);
    const lastFiscalOption = screen.getByText(/Last Fiscal/i);
    await user.click(lastFiscalOption);

    const startCurrFiscal = getStartCurrFiscal();
    const startPrevFiscal = getStartLastFiscal();
    const endPrevFiscal = getEndLastFiscal();
    const agreementIds = getRentalAgreementsLite(startCurrFiscal).data
      .filter(agreement => dateIsBetween(Moment(agreement.datedOn), Moment(startPrevFiscal), Moment(endPrevFiscal)))
      .map(agreement => agreement.id);
    
    const projects = getProjects().data
      .filter(proj => hasArrsIntersect(proj.agreementIds, agreementIds));
    const projectIds = setToArray(arrayToSet(projects.map(proj => proj.id)));
    const equipmentTypes = getEquipmentTypes().data
      .filter(eqType => 
        hasArrsIntersect(eqType.agreementIds, agreementIds) && 
        hasArrsIntersect(eqType.projectIds, projectIds));
    const equipmentTypeIds = setToArray(arrayToSet(equipmentTypes.map(eqType => eqType.id)));
    const equipments = getEquipments().data
      .filter(eq => 
        hasArrsIntersect(eq.agreementIds, agreementIds) &&
        hasArrsIntersect(eq.projectIds, projectIds) &&
        equipmentTypeIds.includes(eq.districtEquipmentTypeId));
    const equipmentIds = setToArray(arrayToSet(equipments.map(equipment => equipment.id)));

    const projectsBtn = screen.getByText(/Projects/i);
    await user.click(projectsBtn);
    expect(screen.queryAllByText(/Test Project \d+/i)).toHaveLength(projectIds.length);

    const equipmentTypesBtn = screen.getByText(/Equipment Types/i);
    await user.click(equipmentTypesBtn);
    expect(screen.queryAllByText(/Eq Type \d+/i)).toHaveLength(equipmentTypeIds.length);

    const equipmentsBtn = screen.getByText("Equipment");
    await user.click(equipmentsBtn);
    expect(screen.queryAllByText(/EC\d+/i)).toHaveLength(equipmentIds.length);
  });

  test("Other filters render correctly with Custom date filter", async () => {
    const user = userEvent.setup();
    renderWithProviders(<AitReport />, { store });

    const dateFilterBtn = await screen.findByText(/This Fiscal/i);
    await user.click(dateFilterBtn);
    const customDateOption = await screen.findByText(/Custom/i);
    await user.click(customDateOption);

    const startCurrFiscal = getStartCurrFiscal();
    const startPrevFiscal = getStartLastFiscal();
    const endPrevFiscal = getEndLastFiscal();
    const now = Moment();
    const nowDateStr = Moment(now).format("YYYY-MM-DD");

    const dateFromInput = screen.getAllByDisplayValue(nowDateStr)[0];
    await user.click(dateFromInput);
    await user.pointer([{target: dateFromInput, offset: 0, keys: '[MouseLeft>]'}, {offset: 10}]);
    await user.paste(Moment(startPrevFiscal).format("YYYY-MM-DD"));
    
    expect(dateFromInput).toHaveValue(Moment(startPrevFiscal).format("YYYY-MM-DD"));
    
    const dateToInput = screen.getAllByDisplayValue(nowDateStr)[0];
    await user.click(dateToInput);
    await user.pointer([{target: dateToInput, offset: 0, keys: '[MouseLeft>]'}, {offset: 10}]);
    await user.paste(Moment(endPrevFiscal).format("YYYY-MM-DD"));
    
    expect(dateToInput).toHaveValue(Moment(endPrevFiscal).format("YYYY-MM-DD"));
    
    const agreementIds = getRentalAgreementsLite(startCurrFiscal).data
      .filter(agreement => dateIsBetween(Moment(agreement.datedOn), Moment(startPrevFiscal), Moment(endPrevFiscal)))
      .map(agreement => agreement.id);
    
    const projects = getProjects().data
      .filter(proj => hasArrsIntersect(proj.agreementIds, agreementIds));
    const projectIds = setToArray(arrayToSet(projects.map(proj => proj.id)));
    const equipmentTypes = getEquipmentTypes().data
      .filter(eqType => 
        hasArrsIntersect(eqType.agreementIds, agreementIds) && 
        hasArrsIntersect(eqType.projectIds, projectIds));
    const equipmentTypeIds = setToArray(arrayToSet(equipmentTypes.map(eqType => eqType.id)));
    const equipments = getEquipments().data
      .filter(eq => 
        hasArrsIntersect(eq.agreementIds, agreementIds) &&
        hasArrsIntersect(eq.projectIds, projectIds) &&
        equipmentTypeIds.includes(eq.districtEquipmentTypeId));
    const equipmentIds = setToArray(arrayToSet(equipments.map(equipment => equipment.id)));

    const projectsBtn = screen.getByText(/Projects/i);
    await user.click(projectsBtn);
    expect(screen.queryAllByText(/Test Project \d+/i)).toHaveLength(projectIds.length);

    const equipmentTypesBtn = screen.getByText(/Equipment Types/i);
    await user.click(equipmentTypesBtn);
    expect(screen.queryAllByText(/Eq Type \d+/i)).toHaveLength(equipmentTypeIds.length);

    const equipmentsBtn = screen.getByText("Equipment");
    await user.click(equipmentsBtn);
    expect(screen.queryAllByText(/EC\d+/i)).toHaveLength(equipmentIds.length);
  });
});

describe("AitReport filter behaviour", () => {
  test("Other filters update correctly with changing date filter", () => {
    //
  });

  test("Other filters update correctly with changing project filter", () => {
    //
  });

  test("Other filters update correctly with changing equipment type filter", () => {
    //
  });
});
