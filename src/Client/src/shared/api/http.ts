import axios, { type InternalAxiosRequestConfig } from 'axios';
import { env } from '../config/env';
import { clearSession, getAccessToken, getRefreshToken, setTokens } from '../storage/tokens';
import { ApiError, toApiError } from './api-error';
import { ErrorCode } from './normalize';

export const http = axios.create({ baseURL: env.apiBaseUrl });

const refreshClient = axios.create({ baseURL: env.apiBaseUrl });

function isTokenRequest(url: string | undefined): boolean {
  if (!url) return false;
  const path = url.startsWith(env.apiBaseUrl) ? url.slice(env.apiBaseUrl.length) : url;
  return path.startsWith('/tokens');
}

http.interceptors.request.use((config) => {
  const token = getAccessToken();
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

let refreshInFlight: Promise<string | null> | null = null;

async function refreshAccessToken(): Promise<string | null> {
  const refreshToken = getRefreshToken();
  if (!refreshToken) return null;

  try {
    const response = await refreshClient.post('/tokens/refresh', null, {
      params: { refreshToken },
    });

    const accessToken = response.data?.accessToken;
    const nextRefreshToken = response.data?.refreshToken;

    if (typeof accessToken === 'string' && typeof nextRefreshToken === 'string') {
      setTokens(accessToken, nextRefreshToken);
      return accessToken;
    }
  } catch {
    return null;
  }

  return null;
}

function redirectToLogin(): void {
  clearSession();
  if (typeof window !== 'undefined' && window.location.pathname !== '/auth') {
    window.location.href = '/auth';
  }
}

http.interceptors.response.use(
  (response) => response,
  async (error: unknown) => {
    if (!axios.isAxiosError(error)) {
      return Promise.reject(toApiError(error));
    }

    const request = error.config as
      | (InternalAxiosRequestConfig & { _retry?: boolean })
      | undefined;

    if (!request || isTokenRequest(request.url) || error.response?.status !== 401 || request._retry) {
      return Promise.reject(toApiError(error));
    }

    request._retry = true;

    refreshInFlight ??= refreshAccessToken().finally(() => {
      refreshInFlight = null;
    });
    const accessToken = await refreshInFlight;

    if (!accessToken) {
      redirectToLogin();
      return Promise.reject(
        new ApiError(
          'Your session has expired. Please sign in again.',
          401,
          ErrorCode.SessionExpired,
        ),
      );
    }

    request.headers.Authorization = `Bearer ${accessToken}`;
    return http(request);
  },
);
