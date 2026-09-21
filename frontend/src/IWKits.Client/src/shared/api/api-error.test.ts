import { AxiosError, AxiosHeaders } from 'axios';
import { describe, expect, it } from 'vitest';
import { ApiError, toApiError } from './api-error';

function axiosErrorWith(status: number, data: unknown): AxiosError {
  const config = { headers: new AxiosHeaders() };
  return new AxiosError('Request failed', 'ERR_BAD_REQUEST', config, null, {
    status,
    statusText: '',
    headers: {},
    config,
    data,
  });
}

function networkError(): AxiosError {
  return new AxiosError('Network Error', 'ERR_NETWORK', { headers: new AxiosHeaders() });
}

describe('toApiError', () => {
  it('passes an existing ApiError through unchanged', () => {
    const original = new ApiError('Already normalized', 400);
    expect(toApiError(original)).toBe(original);
  });

  it('reads the server message from a 400 body', () => {
    const result = toApiError(axiosErrorWith(400, { error_message: 'Subtotal must be positive' }));
    expect(result.message).toBe('Subtotal must be positive');
    expect(result.status).toBe(400);
  });

  it('exposes every validation message in details', () => {
    const result = toApiError(axiosErrorWith(400, { error_message: 'Bad latitude\nBad subtotal' }));
    expect(result.message).toBe('Bad latitude');
    expect(result.details).toEqual(['Bad latitude', 'Bad subtotal']);
  });

  it('reads a bare string body', () => {
    const result = toApiError(axiosErrorWith(400, 'File is empty or missing.'));
    expect(result.message).toBe('File is empty or missing.');
  });

  it('falls back to a status message when the body says nothing useful', () => {
    const result = toApiError(axiosErrorWith(500, { unrelated: true }));
    expect(result.message).toMatch(/unexpected error/i);
    expect(result.status).toBe(500);
  });

  it('does not leak an HTML error page into the message', () => {
    const result = toApiError(axiosErrorWith(502, '<html><body>Bad Gateway</body></html>'));
    expect(result.message).not.toContain('<');
    expect(result.status).toBe(502);
  });

  it('reports an unreachable server when there is no response', () => {
    const result = toApiError(networkError());
    expect(result.message).toMatch(/could not reach the server/i);
    expect(result.status).toBeNull();
  });

  it('uses the message of a plain Error', () => {
    expect(toApiError(new Error('Something broke')).message).toBe('Something broke');
  });

  it('has a last-resort message for non-error values', () => {
    expect(toApiError('just a string').message).toBe('An unexpected error occurred.');
    expect(toApiError(null).message).toBe('An unexpected error occurred.');
  });
});
