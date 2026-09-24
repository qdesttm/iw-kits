import { beforeEach, describe, expect, it, vi } from 'vitest';
import { render, screen, waitFor } from '@testing-library/react';
import { AuthProvider } from './AuthProvider';
import { useAuth } from './useAuth';

const issueTokensMock = vi.fn();
const getProfileMock = vi.fn();
const registerMock = vi.fn();

vi.mock('../api/auth.api', () => ({
  sanitizeUser: (user: unknown) => user ?? null,
  authApi: {
    issueTokens: (...args: unknown[]) => issueTokensMock(...args),
    refreshTokens: vi.fn(),
    register: (...args: unknown[]) => registerMock(...args),
    getProfile: () => getProfileMock(),
  },
}));

let lastResult: string | null | undefined;

function Probe({ action }: { action: 'login' | 'register' }) {
  const auth = useAuth();

  return (
    <div>
      <span data-testid="authenticated">{String(auth.isAuthenticated)}</span>
      <span data-testid="loading">{String(auth.loading)}</span>
      <span data-testid="username">{auth.user?.username ?? ''}</span>
      <button
        onClick={() => {
          void auth[action]('someone', 'secret123').then((result) => {
            lastResult = result;
          });
        }}
      >
        go
      </button>
    </div>
  );
}

function renderProbe(action: 'login' | 'register' = 'login') {
  return render(
    <AuthProvider>
      <Probe action={action} />
    </AuthProvider>,
  );
}

const adminProfile = { id: '1', username: 'someone', role: 'admin' };
const tokens = { accessToken: 'access-1', refreshToken: 'refresh-1' };

describe('AuthProvider', () => {
  beforeEach(() => {
    localStorage.clear();
    issueTokensMock.mockReset();
    getProfileMock.mockReset();
    registerMock.mockReset();
    lastResult = undefined;
  });

  it('logs in by issuing tokens and then loading the profile', async () => {
    issueTokensMock.mockResolvedValue(tokens);
    getProfileMock.mockResolvedValue(adminProfile);

    renderProbe();
    screen.getByText('go').click();

    await waitFor(() => expect(lastResult).toBeNull());
    expect(localStorage.getItem('access_token')).toBe('access-1');
    expect(localStorage.getItem('refresh_token')).toBe('refresh-1');
    await waitFor(() => expect(screen.getByTestId('authenticated').textContent).toBe('true'));
    expect(screen.getByTestId('username').textContent).toBe('someone');
  });

  it('stores tokens before requesting the profile', async () => {
    issueTokensMock.mockResolvedValue(tokens);
    getProfileMock.mockImplementation(() => {
      expect(localStorage.getItem('access_token')).toBe('access-1');
      return Promise.resolve(adminProfile);
    });

    renderProbe();
    screen.getByText('go').click();

    await waitFor(() => expect(lastResult).toBeNull());
    expect(getProfileMock).toHaveBeenCalled();
  });

  it('rejects a non-admin account and clears the session', async () => {
    issueTokensMock.mockResolvedValue(tokens);
    getProfileMock.mockResolvedValue({ id: '2', username: 'someone', role: 'user' });

    renderProbe();
    screen.getByText('go').click();

    await waitFor(() =>
      expect(lastResult).toBe('Only admin users are allowed to access this application'),
    );
    expect(localStorage.getItem('access_token')).toBeNull();
    expect(screen.getByTestId('authenticated').textContent).toBe('false');
  });

  it('surfaces a failure from the token endpoint', async () => {
    issueTokensMock.mockRejectedValue(new Error('Invalid username or password.'));

    renderProbe();
    screen.getByText('go').click();

    await waitFor(() => expect(lastResult).toBe('Invalid username or password.'));
    expect(localStorage.getItem('access_token')).toBeNull();
    expect(getProfileMock).not.toHaveBeenCalled();
  });

  it('registers and then signs in with the same credentials', async () => {
    registerMock.mockResolvedValue(undefined);
    issueTokensMock.mockResolvedValue(tokens);
    getProfileMock.mockResolvedValue(adminProfile);

    renderProbe('register');
    screen.getByText('go').click();

    await waitFor(() => expect(lastResult).toBeNull());
    expect(registerMock).toHaveBeenCalledWith('someone', 'secret123');
    expect(issueTokensMock).toHaveBeenCalledWith('someone', 'secret123');
    await waitFor(() => expect(screen.getByTestId('authenticated').textContent).toBe('true'));
  });

  it('does not attempt sign-in when registration fails', async () => {
    registerMock.mockRejectedValue(new Error('Username is already taken.'));

    renderProbe('register');
    screen.getByText('go').click();

    await waitFor(() => expect(lastResult).toBe('Username is already taken.'));
    expect(issueTokensMock).not.toHaveBeenCalled();
  });

  it('restores a stored session from the profile endpoint', async () => {
    localStorage.setItem('access_token', 'access-1');
    getProfileMock.mockResolvedValue(adminProfile);

    renderProbe();

    await waitFor(() => expect(screen.getByTestId('authenticated').textContent).toBe('true'));
    expect(screen.getByTestId('loading').textContent).toBe('false');
  });

  it('clears a stored session whose profile cannot be loaded', async () => {
    localStorage.setItem('access_token', 'access-1');
    localStorage.setItem('refresh_token', 'refresh-1');
    getProfileMock.mockRejectedValue(new Error('unauthorized'));

    renderProbe();

    await waitFor(() => expect(localStorage.getItem('access_token')).toBeNull());
    expect(screen.getByTestId('authenticated').textContent).toBe('false');
  });

  it('does not call the profile endpoint without a stored token', async () => {
    renderProbe();

    await waitFor(() => expect(screen.getByTestId('loading').textContent).toBe('false'));
    expect(getProfileMock).not.toHaveBeenCalled();
  });
});
