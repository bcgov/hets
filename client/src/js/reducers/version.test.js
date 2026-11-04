import { describe, expect, test } from 'vitest';

import versionReducer, { versionUpdated } from './version';

describe('version slice', () => {
  test('retains defaults and merges the version response', () => {
    const payload = {
      applicationVersions: [{ title: 'HETS API', informationalVersion: '2.0.0' }],
    };

    expect(versionReducer(undefined, versionUpdated(payload))).toEqual({
      applicationVersions: payload.applicationVersions,
      databaseVersions: [{}],
    });
  });
});
