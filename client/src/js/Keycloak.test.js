import { afterEach, describe, expect, test, vi } from 'vitest';

import { init, keycloak, logout } from './Keycloak';

afterEach(() => {
  vi.restoreAllMocks();
  keycloak.onAuthLogout = undefined;
});

describe('Keycloak adapter', () => {
  test('uses the current login and PKCE initialization contract', async () => {
    const onSuccess = vi.fn();
    const initialize = vi.spyOn(keycloak, 'init').mockResolvedValue(true);

    await expect(init(onSuccess)).resolves.toBe(true);

    expect(initialize).toHaveBeenCalledWith({ onLoad: 'login-required', pkceMethod: 'S256' });
    expect(onSuccess).toHaveBeenCalledOnce();
    expect(keycloak.onAuthLogout).toBeTypeOf('function');
  });

  test('does not render the application when authentication fails', async () => {
    const onSuccess = vi.fn();
    vi.spyOn(keycloak, 'init').mockResolvedValue(false);

    await expect(init(onSuccess)).resolves.toBe(false);

    expect(onSuccess).not.toHaveBeenCalled();
  });

  test('delegates logout to the Keycloak client', () => {
    const keycloakLogout = vi.spyOn(keycloak, 'logout').mockResolvedValue(undefined);

    logout();

    expect(keycloakLogout).toHaveBeenCalledOnce();
  });
});
