import { beforeEach, describe, expect, it, vi } from 'vitest';
import {
  clearSession,
  getAccessToken,
  getRefreshToken,
  readStoredUser,
  setTokens,
  writeStoredUser,
} from './tokens';

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
    expect(readStoredUser()).toBeNull();
  });

  it('round-trips the stored user', () => {
    writeStoredUser({ id: '1', username: 'admin', role: 'admin' });
    expect(readStoredUser()).toEqual({ id: '1', username: 'admin', role: 'admin' });
  });

  it('returns null rather than throwing on corrupt JSON', () => {
    localStorage.setItem('user', '{not valid json');
    expect(readStoredUser()).toBeNull();
  });

  it('clearSession removes every key', () => {
    setTokens('access-1', 'refresh-1');
    writeStoredUser({ id: '1' });

    clearSession();

    expect(getAccessToken()).toBeNull();
    expect(getRefreshToken()).toBeNull();
    expect(readStoredUser()).toBeNull();
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
