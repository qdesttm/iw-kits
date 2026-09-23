interface ProblemDetailsLike {
  error_message?: unknown;
  detail?: unknown;
  title?: unknown;
  message?: unknown;
  errors?: Record<string, unknown>;
}

const STATUS_FALLBACKS: Record<number, string> = {
  400: 'The request was rejected as invalid.',
  401: 'Your session has expired. Please sign in again.',
  403: 'You do not have permission to perform this action.',
  404: 'The requested resource was not found.',
  409: 'That action conflicts with the current state of the data.',
  413: 'The uploaded file is too large.',
  500: 'The server encountered an unexpected error.',
  502: 'The server is unreachable. Please try again shortly.',
  503: 'The service is temporarily unavailable. Please try again shortly.',
};

export function statusFallback(status: number): string {
  return STATUS_FALLBACKS[status] ?? `Request failed with status ${status}.`;
}

export function splitMessages(raw: string): string[] {
  return raw
    .split('\n')
    .map((line) => line.trim())
    .filter((line) => line.length > 0);
}

function readStringBody(body: string): string[] {
  const trimmed = body.trim();
  if (trimmed.length === 0) return [];
  if (trimmed.startsWith('<') || trimmed.startsWith('{') || trimmed.startsWith('[')) return [];
  return splitMessages(trimmed);
}

export function readBody(body: unknown): string[] {
  if (typeof body === 'string') return readStringBody(body);
  if (!body || typeof body !== 'object') return [];

  const payload = body as ProblemDetailsLike;

  for (const key of ['error_message', 'detail', 'title', 'message'] as const) {
    const value = payload[key];
    if (typeof value === 'string' && value.trim().length > 0) {
      return splitMessages(value);
    }
  }

  if (payload.errors && typeof payload.errors === 'object') {
    const collected = Object.values(payload.errors)
      .flatMap((value) => (Array.isArray(value) ? value : [value]))
      .filter((value): value is string => typeof value === 'string' && value.trim().length > 0);
    if (collected.length > 0) return collected;
  }

  return [];
}

export function readEnvelopeError(body: unknown): string[] {
  if (!body || typeof body !== 'object') return [];

  const envelope = body as ProblemDetailsLike;
  if (typeof envelope.error_message !== 'string') return [];

  return splitMessages(envelope.error_message);
}
