import { createStore, applyMiddleware, compose } from "redux";
import freeze from "redux-freeze";
import { thunk } from "redux-thunk";

import allReducers from "./reducers/all";

const composeEnhancers =
  typeof window === "object" && window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__
    ? window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__({
        // Specify extension’s options like name, actionsBlacklist, actionsCreators, serialize...
      })
    : compose;

const middleware = [thunk];

if (!import.meta.env.PROD) {
  // Only add this redux store mutation detection middleware in dev
  middleware.push(freeze);
}

const setupStore = () => {
  // Note passing middleware as the last argument to createStore requires redux@>=3.1.0
  const store = createStore(
    allReducers,
    composeEnhancers(applyMiddleware(...middleware))
  );

  if (import.meta.hot) {
    import.meta.hot.accept("./reducers/all", (nextModule) => {
      store.replaceReducer(nextModule.default);
    });
  }

  return store;
};

const store = setupStore();

export {
  store,
  setupStore,
};
