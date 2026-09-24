export interface ApiErrorInfo {
  code: string;
  message: string;
}

interface ErrorEnvelopeLike {
  error?: unknown;
}

interface LegacyProblemLike {
  detail?: unknown;
  title?: unknown;
  message?: unknown;
  errors?: Record<string, unknown>;
}

export const ErrorCode = {
  InvalidCredentials: 'invalid_credentials',
  UsernameAlreadyTaken: 'username_already_taken',
  SessionNotFound: 'session_not_found',
  SessionExpired: 'session_expired',
  UserNotFound: 'user_not_found',
  PostalCodeMismatch: 'postal_code_mismatch',
  OutsideServiceArea: 'outside_service_area',
  JurisdictionNotResolved: 'jurisdiction_not_resolved',
  TaxDataUnavailable: 'tax_data_unavailable',
  InvalidFile: 'invalid_file',
  OperationFailed: 'operation_failed',
  BadRequest: 'bad_request',
  InternalServerError: 'internal_server_error',
} as const;

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

function readText(value: unknown): string | null {
  if (typeof value !== 'string') return null;
  const trimmed = value.trim();
  return trimmed.length > 0 ? trimmed : null;
}

export function readErrorInfo(body: unknown): ApiErrorInfo | null {
  if (!body || typeof body !== 'object') return null;

  const envelope = (body as ErrorEnvelopeLike).error;
  if (!envelope || typeof envelope !== 'object') return null;

  const candidate = envelope as Partial<ApiErrorInfo>;
  const message = readText(candidate.message);
  const code = readText(candidate.code);

  if (!message && !code) return null;

  return {
    code: code ?? ErrorCode.OperationFailed,
    message: message ?? statusFallback(400),
  };
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

  const errorInfo = readErrorInfo(body);
  if (errorInfo) return splitMessages(errorInfo.message);

  const payload = body as LegacyProblemLike;

  for (const key of ['detail', 'title', 'message'] as const) {
    const value = readText(payload[key]);
    if (value) return splitMessages(value);
  }

  if (payload.errors && typeof payload.errors === 'object') {
    const collected = Object.values(payload.errors)
      .flatMap((value) => (Array.isArray(value) ? value : [value]))
      .filter((value): value is string => typeof value === 'string' && value.trim().length > 0);
    if (collected.length > 0) return collected;
  }

  return [];
}
