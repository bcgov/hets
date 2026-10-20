import { createSlice } from '@reduxjs/toolkit';

const DEFAULT_VERSION = {
  applicationVersions: [{}],
  databaseVersions: [{}],
};

const versionSlice = createSlice({
  name: 'version',
  initialState: DEFAULT_VERSION,
  reducers: {
    versionUpdated: (state, action) => ({ ...state, ...action.payload }),
  },
});

export const { versionUpdated } = versionSlice.actions;
export default versionSlice.reducer;
