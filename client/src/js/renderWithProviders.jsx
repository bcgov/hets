import React from "react";
import { render } from "@testing-library/react";
import { Provider } from "react-redux";

import { setupStore } from "./store";

export const renderWithProviders = (ui, {
  store = setupStore(),
  ...renderOptions
} = {}) => {
  const Wrapper = ({ children }) => (
    <Provider store={store}>{children}</Provider>
  );

  return {
    store,
    ...render(ui, { wrapper: Wrapper, ...renderOptions }),
  };
};