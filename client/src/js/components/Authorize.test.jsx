import { screen } from "@testing-library/react";

import * as Constant from "../constants";
import * as Action from "../actionTypes";
import { getCurrentUser } from "../mock/api/getCurrentUser";
import { renderWithProviders } from "../renderWithProviders";
import { setupStore } from "../store";
import Authorize from "./Authorize";

let store;

beforeEach(() => {
  store = setupStore();
})

describe("show component when authorized", () => {
  test("renders component given enough permissions", () => {
    // Arrange
    const authorizedStr = "authorized";
    const authorizedUser = getCurrentUser().data;
    authorizedUser.hasPermission = (_) => true;

    store.dispatch({
      type: Action.UPDATE_CURRENT_USER,
      user: authorizedUser,
    });

    // Act
    renderWithProviders((
      <Authorize requires={Constant.PERMISSION_WRITE_ACCESS}>
        <p id="auth-str">{authorizedStr}</p>
      </Authorize>
    ), { store });

    // Assert
    expect(screen.getByText(authorizedStr)).toBeVisible();
  });

  test("renders component when permissions not checked and condition is true", () => {
    // Arrange
    const authorizedStr = "authorized";
    const authorizedUser = getCurrentUser().data;
    authorizedUser.hasPermission = (_) => false;

    store.dispatch({
      type: Action.UPDATE_CURRENT_USER,
      user: authorizedUser,
    });

    // Act
    renderWithProviders((
      <Authorize condition={true}>
        <p id="auth-str">{authorizedStr}</p>
      </Authorize>
    ), { store });

    // Assert
    expect(screen.getByText(authorizedStr)).toBeVisible();
  });
});

describe("show nothing when unauthorized", () => {
  test("renders nothing when user doesn't have permissions", () => {
    // Arrange
    const authorizedStr = "authorized";
    const authorizedUser = getCurrentUser().data;
    authorizedUser.hasPermission = (_) => false;

    store.dispatch({
      type: Action.UPDATE_CURRENT_USER,
      user: authorizedUser,
    });

    // Act
    renderWithProviders((
      <Authorize requires={Constant.PERMISSION_WRITE_ACCESS}>
        <p id="auth-str">{authorizedStr}</p>
      </Authorize>
    ), { store });

    // Assert
    expect(screen.queryByText(authorizedStr)).toBeNull();
  });

  test("renders nothing when permissions not checked and condition is false", () => {
    // Arrange
    const authorizedStr = "authorized";
    const authorizedUser = getCurrentUser().data;
    authorizedUser.hasPermission = (_) => true;

    store.dispatch({
      type: Action.UPDATE_CURRENT_USER,
      user: authorizedUser,
    });

    // Act
    renderWithProviders((
      <Authorize condition={false}>
        <p id="auth-str">{authorizedStr}</p>
      </Authorize>
    ), { store });

    // Assert
    expect(screen.queryByText(authorizedStr)).toBeNull();
  });
});
