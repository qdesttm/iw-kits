import axios, { type InternalAxiosRequestConfig } from 'axios';
import { env } from '../config/env.js';
import { clearSession, getAccessToken, getRefreshToken, setTokens } from '../storage/tokens.js';
import { ApiError, fromMessages, toApiError } from './api-error.js';
import { readEnvelopeError } from './normalize.js';

export const http = axios.create({ baseURL: env.apiBaseUrl });

const refreshClient = axios.create({ baseURL: env.apiBaseUrl });

function isAuthRequest(url: string | undefined): boolean {
  if (!url) return false;
  return url.startsWith('/auth') || url.startsWith(`${env.apiBaseUrl}/auth`);
}

http.interceptors.request.use((config) => {
  const token = getAccessToken();
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

http.interceptors.response.use((response) => {
  const envelopeError = fromMessages(readEnvelopeError(response.data), response.status);
  if (envelopeError) {
    throw envelopeError;
  }
  return response;
});

let refreshInFlight: Promise<string | null> | null = null;

async function refreshAccessToken(): Promise<string | null> {
  const refreshToken = getRefreshToken();
  if (!refreshToken) return null;

  try {
    const response = await refreshClient.post('/auth/refresh', { refresh_token: refreshToken });
    const accessToken = response.data?.access_token;
    const nextRefreshToken = response.data?.refresh_token;

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

    if (!request || isAuthRequest(request.url) || error.response?.status !== 401 || request._retry) {
      return Promise.reject(toApiError(error));
    }

    request._retry = true;

    refreshInFlight ??= refreshAccessToken().finally(() => {
      refreshInFlight = null;
    });
    const accessToken = await refreshInFlight;

    if (!accessToken) {
      redirectToLogin();
      return Promise.reject(new ApiError('Your session has expired. Please sign in again.', 401));
    }

    request.headers.Authorization = `Bearer ${accessToken}`;
    return http(request);
  },
);
