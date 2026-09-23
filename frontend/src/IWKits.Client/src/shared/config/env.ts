const DEFAULT_API_BASE_URL = '/api/v1';

function readApiBaseUrl(): string {
  const configured = import.meta.env.VITE_API_BASE_URL;

  if (typeof configured === 'string' && configured.trim().length > 0) {
    return configured.trim().replace(/\/+$/, '');
  }

  return DEFAULT_API_BASE_URL;
}

export const env = {
  apiBaseUrl: readApiBaseUrl(),
} as const;
