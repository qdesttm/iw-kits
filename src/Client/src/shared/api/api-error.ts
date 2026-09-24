import axios, { type AxiosError } from 'axios';
import { ErrorCode, readBody, readErrorInfo, statusFallback } from './normalize';

export class ApiError extends Error {
  readonly status: number | null;
  readonly code: string | null;
  readonly details: string[];

  constructor(
    message: string,
    status: number | null = null,
    code: string | null = null,
    details: string[] = [],
  ) {
    super(message);
    this.name = 'ApiError';
    this.status = status;
    this.code = code;
    this.details = details;
  }

  is(code: string): boolean {
    return this.code === code;
  }
}

export function toApiError(error: unknown): ApiError {
  if (error instanceof ApiError) return error;

  if (axios.isAxiosError(error)) {
    const response = (error as AxiosError).response;

    if (!response) {
      const offline = typeof navigator !== 'undefined' && navigator.onLine === false;
      return new ApiError(
        offline
          ? 'You appear to be offline. Check your connection and try again.'
          : 'Could not reach the server. Is the backend running?',
        null,
        null,
      );
    }

    const info = readErrorInfo(response.data);
    if (info) {
      const details = readBody(response.data);
      const [primary] = details;
      return new ApiError(primary ?? info.message, response.status, info.code, details);
    }

    const [primary, ...rest] = readBody(response.data);
    if (primary) {
      return new ApiError(primary, response.status, null, [primary, ...rest]);
    }

    return new ApiError(statusFallback(response.status), response.status, null);
  }

  if (error instanceof Error && error.message.trim().length > 0) {
    return new ApiError(error.message);
  }

  return new ApiError('An unexpected error occurred.', null, ErrorCode.OperationFailed);
}
