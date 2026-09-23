import { http } from '@/shared/api';
import type { AuthResponse, AuthUser, RefreshResponse } from '../model/auth.types';

export function sanitizeUser(user: AuthUser | null | undefined): AuthUser | null {
  if (!user) return null;

  return {
    id: user.id,
    username: user.username,
    role: user.role,
  };
}

export const authApi = {
  login: async (username: string, password: string): Promise<AuthResponse> => {
    const response = await http.post('/auth/login', { username, password });
    return { ...response.data, user: sanitizeUser(response.data?.user) };
  },

  register: async (username: string, password: string): Promise<AuthResponse> => {
    const response = await http.post('/auth/register', { username, password });
    return { ...response.data, user: sanitizeUser(response.data?.user) };
  },

  refresh: async (refreshToken: string): Promise<RefreshResponse> => {
    const response = await http.post('/auth/refresh', { refresh_token: refreshToken });
    return response.data;
  },
};
