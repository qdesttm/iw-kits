import { http } from '@/shared/api';
import type { AuthUser, TokensResponse } from '../model/auth.types';

export function sanitizeUser(user: AuthUser | null | undefined): AuthUser | null {
  if (!user) return null;

  return {
    id: user.id,
    username: user.username,
    role: user.role,
  };
}

function readTokens(data: unknown): TokensResponse {
  const payload = data as Partial<TokensResponse> | undefined;

  if (typeof payload?.accessToken !== 'string' || typeof payload?.refreshToken !== 'string') {
    throw new Error('The server did not return a valid token pair.');
  }

  return { accessToken: payload.accessToken, refreshToken: payload.refreshToken };
}

export const authApi = {
  issueTokens: async (username: string, password: string): Promise<TokensResponse> => {
    const response = await http.post('/tokens', null, { params: { username, password } });
    return readTokens(response.data);
  },

  refreshTokens: async (refreshToken: string): Promise<TokensResponse> => {
    const response = await http.post('/tokens/refresh', null, { params: { refreshToken } });
    return readTokens(response.data);
  },

  register: async (username: string, password: string): Promise<void> => {
    await http.post('/users', null, { params: { username, password } });
  },

  getProfile: async (): Promise<AuthUser | null> => {
    const response = await http.get('/users/profile');
    return sanitizeUser(response.data?.data as AuthUser | undefined);
  },
};
