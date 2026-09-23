import { useCallback, useState, type ReactNode } from 'react';
import { toApiError } from '@/shared/api';
import { clearSession, getAccessToken, readStoredUser, setTokens, writeStoredUser } from '@/shared/storage/tokens';
import { authApi, sanitizeUser } from '../api/auth.api';
import { AuthContext } from './auth-context';
import type { AuthUser } from './auth.types';

function restoreStoredUser(): AuthUser | null {
  if (!getAccessToken()) return null;

  const parsed = sanitizeUser(readStoredUser() as AuthUser | null);
  if (!parsed || parsed.role !== 'admin') return null;

  writeStoredUser(parsed);
  return parsed;
}

function restoreSession(): AuthUser | null {
  const restored = restoreStoredUser();

  if (!restored) {
    clearSession();
  }

  return restored;
}

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<AuthUser | null>(restoreSession);

  const login = useCallback(async (username: string, password: string): Promise<string | null> => {
    try {
      const response = await authApi.login(username, password);

      if (response.error_message) {
        return response.error_message;
      }

      if (!response.user || response.user.role !== 'admin') {
        return 'Only admin users are allowed to access this application';
      }

      if (response.access_token && response.refresh_token) {
        setTokens(response.access_token, response.refresh_token);
        writeStoredUser(response.user);
        setUser(response.user);
      }

      return null;
    } catch (error: unknown) {
      return toApiError(error).message;
    }
  }, []);

  const register = useCallback(async (username: string, password: string): Promise<string | null> => {
    try {
      const response = await authApi.register(username, password);

      if (response.error_message) {
        return response.error_message;
      }

      if (!response.user || !response.access_token || !response.refresh_token) {
        return 'Registration failed. Please try again.';
      }

      setTokens(response.access_token, response.refresh_token);
      writeStoredUser(response.user);
      setUser(response.user);

      return null;
    } catch (error: unknown) {
      return toApiError(error).message;
    }
  }, []);

  const logout = useCallback(() => {
    clearSession();
    setUser(null);
  }, []);

  return (
    <AuthContext.Provider
      value={{
        user,
        isAuthenticated: !!user,
        loading: false,
        login,
        register,
        logout,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
}
