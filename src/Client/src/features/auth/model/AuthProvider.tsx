import { useCallback, useEffect, useState, type ReactNode } from 'react';
import { toApiError } from '@/shared/api';
import { clearSession, getAccessToken, setTokens } from '@/shared/storage/tokens';
import { authApi } from '../api/auth.api';
import { AuthContext } from './auth-context';
import type { AuthUser } from './auth.types';

const ADMIN_ROLE = 'admin';
const NOT_ADMIN_MESSAGE = 'Only admin users are allowed to access this application';

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<AuthUser | null>(null);
  const [loading, setLoading] = useState(() => getAccessToken() !== null);

  useEffect(() => {
    if (!getAccessToken()) return;

    let cancelled = false;

    void (async () => {
      try {
        const profile = await authApi.getProfile();

        if (cancelled) return;

        if (profile && profile.role === ADMIN_ROLE) {
          setUser(profile);
        } else {
          clearSession();
        }
      } catch {
        if (!cancelled) clearSession();
      } finally {
        if (!cancelled) setLoading(false);
      }
    })();

    return () => {
      cancelled = true;
    };
  }, []);

  const login = useCallback(async (username: string, password: string): Promise<string | null> => {
    try {
      const tokens = await authApi.issueTokens(username, password);
      setTokens(tokens.accessToken, tokens.refreshToken);

      const profile = await authApi.getProfile();

      if (!profile) {
        clearSession();
        return 'Could not load your profile. Please try again.';
      }

      if (profile.role !== ADMIN_ROLE) {
        clearSession();
        return NOT_ADMIN_MESSAGE;
      }

      setUser(profile);
      return null;
    } catch (error: unknown) {
      clearSession();
      return toApiError(error).message;
    }
  }, []);

  const register = useCallback(
    async (username: string, password: string): Promise<string | null> => {
      try {
        await authApi.register(username, password);
      } catch (error: unknown) {
        return toApiError(error).message;
      }

      return login(username, password);
    },
    [login],
  );

  const logout = useCallback(() => {
    clearSession();
    setUser(null);
  }, []);

  return (
    <AuthContext.Provider
      value={{
        user,
        isAuthenticated: !!user,
        loading,
        login,
        register,
        logout,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
}
