import { AxiosError, AxiosHeaders } from 'axios';
import { describe, expect, it } from 'vitest';
import { ApiError, toApiError } from './api-error';
import { ErrorCode } from './normalize';

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
    const original = new ApiError('Already normalized', 400, ErrorCode.BadRequest);
    expect(toApiError(original)).toBe(original);
  });

  it('reads code and message from the API error envelope', () => {
    const result = toApiError(
      axiosErrorWith(401, {
        error: { code: ErrorCode.InvalidCredentials, message: 'Invalid username or password.' },
      }),
    );

    expect(result.message).toBe('Invalid username or password.');
    expect(result.code).toBe(ErrorCode.InvalidCredentials);
    expect(result.status).toBe(401);
    expect(result.is(ErrorCode.InvalidCredentials)).toBe(true);
  });

  it('identifies a taken username so the UI can react to the code', () => {
    const result = toApiError(
      axiosErrorWith(400, {
        error: { code: ErrorCode.UsernameAlreadyTaken, message: 'Username is already taken.' },
      }),
    );

    expect(result.is(ErrorCode.UsernameAlreadyTaken)).toBe(true);
  });

  it('exposes every line of a multi-message validation error', () => {
    const result = toApiError(
      axiosErrorWith(400, {
        error: { code: ErrorCode.BadRequest, message: 'Bad latitude\nBad subtotal' },
      }),
    );

    expect(result.message).toBe('Bad latitude');
    expect(result.details).toEqual(['Bad latitude', 'Bad subtotal']);
  });

  it('falls back to a status message when the body says nothing useful', () => {
    const result = toApiError(axiosErrorWith(500, { unrelated: true }));
    expect(result.message).toMatch(/unexpected error/i);
    expect(result.status).toBe(500);
    expect(result.code).toBeNull();
  });

  it('handles an empty 401 body from the JWT middleware', () => {
    const result = toApiError(axiosErrorWith(401, ''));
    expect(result.message).toMatch(/session has expired/i);
    expect(result.status).toBe(401);
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
