import { describe, expect, it } from 'vitest';
import { ErrorCode, readBody, readErrorInfo, splitMessages, statusFallback } from './normalize';

describe('readErrorInfo', () => {
  it('reads the API error envelope', () => {
    const body = { error: { code: 'invalid_credentials', message: 'Invalid username or password.' } };
    expect(readErrorInfo(body)).toEqual({
      code: 'invalid_credentials',
      message: 'Invalid username or password.',
    });
  });

  it('falls back to a generic code when only a message is present', () => {
    const info = readErrorInfo({ error: { message: 'Something went wrong' } });
    expect(info?.message).toBe('Something went wrong');
    expect(info?.code).toBe(ErrorCode.OperationFailed);
  });

  it('returns null for a successful payload', () => {
    expect(readErrorInfo({ data: { id: '1' } })).toBeNull();
    expect(readErrorInfo({ data: [], page: 1, size: 24, itemsCount: 0 })).toBeNull();
  });

  it('returns null when the envelope carries nothing usable', () => {
    expect(readErrorInfo({ error: null })).toBeNull();
    expect(readErrorInfo({ error: {} })).toBeNull();
    expect(readErrorInfo({ error: 'oops' })).toBeNull();
    expect(readErrorInfo(null)).toBeNull();
    expect(readErrorInfo('plain text')).toBeNull();
  });
});

describe('readBody', () => {
  it('prefers the API error envelope', () => {
    const body = { error: { code: 'bad_request', message: 'Subtotal must be greater than 0' } };
    expect(readBody(body)).toEqual(['Subtotal must be greater than 0']);
  });

  it('splits a multi-line validation message into separate entries', () => {
    const body = { error: { code: 'bad_request', message: 'Bad latitude\nBad subtotal' } };
    expect(readBody(body)).toEqual(['Bad latitude', 'Bad subtotal']);
  });

  it('reads a bare string body', () => {
    expect(readBody('File is empty or missing.')).toEqual(['File is empty or missing.']);
  });

  it('ignores an HTML error page rather than showing markup to the user', () => {
    expect(readBody('<!DOCTYPE html><html><body>500</body></html>')).toEqual([]);
  });

  it('ignores a JSON-looking string body', () => {
    expect(readBody('{"unparsed":true}')).toEqual([]);
  });

  it('falls back to ProblemDetails emitted before the app handler runs', () => {
    expect(readBody({ detail: 'Access denied', status: 403 })).toEqual(['Access denied']);
  });

  it('flattens ASP.NET model-binding validation errors', () => {
    const body = { errors: { Latitude: ['Must be a number'], Subtotal: ['Required', 'Positive'] } };
    expect(readBody(body)).toEqual(['Must be a number', 'Required', 'Positive']);
  });

  it('returns nothing for bodies with no readable message', () => {
    expect(readBody(null)).toEqual([]);
    expect(readBody(undefined)).toEqual([]);
    expect(readBody(42)).toEqual([]);
    expect(readBody({ unrelated: 'value' })).toEqual([]);
  });
});

describe('splitMessages', () => {
  it('drops blank lines and trims whitespace', () => {
    expect(splitMessages('  padded  \n\n\n  other  ')).toEqual(['padded', 'other']);
  });

  it('returns nothing for an empty string', () => {
    expect(splitMessages('   ')).toEqual([]);
  });
});

describe('statusFallback', () => {
  it('gives a human message for known statuses', () => {
    expect(statusFallback(401)).toMatch(/session has expired/i);
    expect(statusFallback(403)).toMatch(/permission/i);
  });

  it('degrades gracefully for unknown statuses', () => {
    expect(statusFallback(418)).toBe('Request failed with status 418.');
  });
});
