import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

import * as Action from '../actionTypes';
import { keycloak } from '../Keycloak';
import { resetSessionTimeout } from '../utils/sessionTimeout';

const request = fetchBaseQuery({
  baseUrl: new URL('/api/', window.location.origin).toString(),
  prepareHeaders: (headers) => {
    if (keycloak.token) {
      headers.set('authorization', `Bearer ${keycloak.token}`);
    }
    return headers;
  },
});

const authenticatedRequest = async (args, api, extraOptions) => {
  api.dispatch({ type: Action.REQUESTS_BEGIN });

  try {
    try {
      if (await keycloak.updateToken(5)) {
        resetSessionTimeout();
      }
    } catch {
      console.log('Failed to refresh the token, or the session has expired');
    }

    return await request(args, api, extraOptions);
  } finally {
    api.dispatch({ type: Action.REQUESTS_END });
  }
};

export const serverApi = createApi({
  reducerPath: 'serverApi',
  baseQuery: authenticatedRequest,
  endpoints: (build) => ({
    getSearchSummaryCounts: build.query({
      query: () => 'counts',
      transformResponse: (response) => response.data,
    }),
  }),
});

export const { useGetSearchSummaryCountsQuery } = serverApi;

