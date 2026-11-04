import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

import { unhandledApiError } from '../actions';
import { keycloak } from '../Keycloak';
import { ApiError, decrementRequests, incrementRequests } from '../utils/http';
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
  api.dispatch(incrementRequests());

  try {
    try {
      if (await keycloak.updateToken(5)) {
        resetSessionTimeout();
      }
    } catch {
      console.log('Failed to refresh the token, or the session has expired');
    }

    const result = await request(args, api, extraOptions);
    if (result.error) {
      const method = typeof args === 'string' ? 'GET' : (args.method || 'GET').toUpperCase();
      const path = new URL(typeof args === 'string' ? args : args.url, new URL('/api/', window.location.origin)).pathname;
      const status = typeof result.error.status === 'number' ? result.error.status : null;
      const error = new ApiError(`API ${method} ${path} failed (${status ?? result.error.status})`, method, path, status, null, null, result.error.data);
      api.dispatch(unhandledApiError(error));
    }
    return result;
  } finally {
    api.dispatch(decrementRequests());
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

