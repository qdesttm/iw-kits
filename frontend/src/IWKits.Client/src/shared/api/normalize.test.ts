import { describe, expect, it } from 'vitest';
import { readBody, readEnvelopeError, splitMessages, statusFallback } from './normalize';

describe('splitMessages', () => {
  it('splits the newline-joined messages the backend produces', () => {
    expect(splitMessages('First failure\nSecond failure')).toEqual([
      'First failure',
      'Second failure',
    ]);
  });

  it('drops blank lines and trims whitespace', () => {
    expect(splitMessages('  padded  \n\n\n  other  ')).toEqual(['padded', 'other']);
  });

  it('returns nothing for an empty string', () => {
    expect(splitMessages('   ')).toEqual([]);
  });
});

describe('readBody', () => {
  it('reads the project error_message contract', () => {
    expect(readBody({ error_message: 'Invalid credentials' })).toEqual(['Invalid credentials']);
  });

  it('splits a multi-message error_message into separate entries', () => {
    const body = { error_message: 'Latitude is required\nSubtotal must be positive' };
    expect(readBody(body)).toEqual(['Latitude is required', 'Subtotal must be positive']);
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

  it('falls back to ProblemDetails detail', () => {
    expect(readBody({ detail: 'Access denied', status: 403 })).toEqual(['Access denied']);
  });

  it('prefers error_message over ProblemDetails fields', () => {
    expect(readBody({ error_message: 'Specific', detail: 'Generic' })).toEqual(['Specific']);
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
    expect(readBody({ error_message: '   ' })).toEqual([]);
  });
});

describe('readEnvelopeError', () => {
  it('detects a failure reported inside a 200 OK body', () => {
    expect(readEnvelopeError({ error_message: 'Geocoding failed' })).toEqual(['Geocoding failed']);
  });

  it('ignores a successful payload', () => {
    expect(readEnvelopeError({ created_order: { id: '1' } })).toEqual([]);
    expect(readEnvelopeError({ items: [], total_count: 0 })).toEqual([]);
  });

  it('ignores a non-string error_message', () => {
    expect(readEnvelopeError({ error_message: null })).toEqual([]);
    expect(readEnvelopeError({ error_message: 123 })).toEqual([]);
  });

  it('ignores non-object bodies', () => {
    expect(readEnvelopeError('plain text')).toEqual([]);
    expect(readEnvelopeError(null)).toEqual([]);
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
