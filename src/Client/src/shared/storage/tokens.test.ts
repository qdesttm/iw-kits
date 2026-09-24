import { beforeEach, describe, expect, it, vi } from 'vitest';
import { clearSession, getAccessToken, getRefreshToken, setTokens } from './tokens';

describe('token storage', () => {
  beforeEach(() => {
    localStorage.clear();
    vi.restoreAllMocks();
  });

  it('round-trips tokens', () => {
    setTokens('access-1', 'refresh-1');
    expect(getAccessToken()).toBe('access-1');
    expect(getRefreshToken()).toBe('refresh-1');
  });

  it('returns null when nothing is stored', () => {
    expect(getAccessToken()).toBeNull();
    expect(getRefreshToken()).toBeNull();
  });

  it('clearSession removes the tokens', () => {
    setTokens('access-1', 'refresh-1');

    clearSession();

    expect(getAccessToken()).toBeNull();
    expect(getRefreshToken()).toBeNull();
  });

  it('clearSession also drops the legacy cached user', () => {
    localStorage.setItem('user', '{"id":"1"}');

    clearSession();

    expect(localStorage.getItem('user')).toBeNull();
  });

  it('survives localStorage being unavailable', () => {
    vi.spyOn(Storage.prototype, 'getItem').mockImplementation(() => {
      throw new Error('access denied');
    });
    vi.spyOn(Storage.prototype, 'setItem').mockImplementation(() => {
      throw new Error('access denied');
    });

    expect(() => setTokens('a', 'b')).not.toThrow();
    expect(getAccessToken()).toBeNull();
  });
});
