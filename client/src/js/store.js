import { configureStore } from '@reduxjs/toolkit';

import allReducers from './reducers/all';
import { serverApi } from './services/serverApi';

const setupStore = () => {
  const store = configureStore({
    reducer: allReducers,
    devTools: !import.meta.env.PROD,
    middleware: (getDefaultMiddleware) =>
      getDefaultMiddleware({
        // The legacy user model intentionally stores hasPermission as a function.
        serializableCheck: false,
      }).concat(serverApi.middleware),
  });

  if (import.meta.hot) {
    import.meta.hot.accept('./reducers/all', (nextModule) => {
      store.replaceReducer(nextModule.default);
    });
  }

  return store;
};

const store = setupStore();

export { store, setupStore };
