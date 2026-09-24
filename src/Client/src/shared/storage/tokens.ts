const ACCESS_TOKEN_KEY = 'access_token';
const REFRESH_TOKEN_KEY = 'refresh_token';
const LEGACY_USER_KEY = 'user';

function readItem(key: string): string | null {
  try {
    return localStorage.getItem(key);
  } catch {
    return null;
  }
}

function writeItem(key: string, value: string): void {
  try {
    localStorage.setItem(key, value);
  } catch {
    return;
  }
}

function removeItem(key: string): void {
  try {
    localStorage.removeItem(key);
  } catch {
    return;
  }
}

export function getAccessToken(): string | null {
  return readItem(ACCESS_TOKEN_KEY);
}

export function getRefreshToken(): string | null {
  return readItem(REFRESH_TOKEN_KEY);
}

export function setTokens(accessToken: string, refreshToken: string): void {
  writeItem(ACCESS_TOKEN_KEY, accessToken);
  writeItem(REFRESH_TOKEN_KEY, refreshToken);
}

export function clearSession(): void {
  removeItem(ACCESS_TOKEN_KEY);
  removeItem(REFRESH_TOKEN_KEY);
  removeItem(LEGACY_USER_KEY);
}
