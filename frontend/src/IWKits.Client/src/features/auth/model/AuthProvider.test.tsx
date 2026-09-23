import { beforeEach, describe, expect, it, vi } from 'vitest';
import { render, screen, waitFor } from '@testing-library/react';
import { AuthProvider } from './AuthProvider';
import { useAuth } from './useAuth';

const registerMock = vi.fn();

vi.mock('../api/auth.api', () => ({
  sanitizeUser: (user: unknown) => user ?? null,
  authApi: {
    login: vi.fn(),
    register: (...args: unknown[]) => registerMock(...args),
    refresh: vi.fn(),
  },
}));

let lastResult: string | null | undefined;

function Probe() {
  const { register, isAuthenticated } = useAuth();

  return (
    <div>
      <span data-testid="authenticated">{String(isAuthenticated)}</span>
      <button
        onClick={() => {
          void register('someone', 'secret123').then((result) => {
            lastResult = result;
          });
        }}
      >
        register
      </button>
    </div>
  );
}

function renderProbe() {
  return render(
    <AuthProvider>
      <Probe />
    </AuthProvider>,
  );
}

describe('AuthProvider.register', () => {
  beforeEach(() => {
    localStorage.clear();
    registerMock.mockReset();
    lastResult = undefined;
  });

  it('signs the new account in and persists the session', async () => {
    registerMock.mockResolvedValue({
      access_token: 'access-1',
      refresh_token: 'refresh-1',
      user: { id: '1', username: 'someone', role: 'admin' },
      error_message: null,
    });

    renderProbe();
    screen.getByText('register').click();

    await waitFor(() => expect(lastResult).toBeNull());
    expect(localStorage.getItem('access_token')).toBe('access-1');
    expect(localStorage.getItem('refresh_token')).toBe('refresh-1');
    await waitFor(() => expect(screen.getByTestId('authenticated').textContent).toBe('true'));
  });

  it('surfaces a server error message and stores nothing', async () => {
    registerMock.mockResolvedValue({
      access_token: null,
      refresh_token: null,
      user: null,
      error_message: 'Username already taken',
    });

    renderProbe();
    screen.getByText('register').click();

    await waitFor(() => expect(lastResult).toBe('Username already taken'));
    expect(localStorage.getItem('access_token')).toBeNull();
    expect(screen.getByTestId('authenticated').textContent).toBe('false');
  });

  it('fails safely when the response is missing tokens', async () => {
    registerMock.mockResolvedValue({
      access_token: null,
      refresh_token: null,
      user: { id: '1', username: 'someone', role: 'admin' },
      error_message: null,
    });

    renderProbe();
    screen.getByText('register').click();

    await waitFor(() => expect(lastResult).toBe('Registration failed. Please try again.'));
    expect(localStorage.getItem('access_token')).toBeNull();
  });

  it('normalizes a thrown error', async () => {
    registerMock.mockRejectedValue(new Error('Network down'));

    renderProbe();
    screen.getByText('register').click();

    await waitFor(() => expect(lastResult).toBe('Network down'));
  });
});
