import axios, { type AxiosError } from 'axios';
import { readBody, statusFallback } from './normalize.js';

export class ApiError extends Error {
  readonly status: number | null;
  readonly details: string[];

  constructor(message: string, status: number | null = null, details: string[] = []) {
    super(message);
    this.name = 'ApiError';
    this.status = status;
    this.details = details;
  }
}

export function fromMessages(messages: string[], status: number | null): ApiError | null {
  const [primary, ...rest] = messages;
  if (!primary) return null;
  return new ApiError(primary, status, [primary, ...rest]);
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
      );
    }

    return (
      fromMessages(readBody(response.data), response.status) ??
      new ApiError(statusFallback(response.status), response.status)
    );
  }

  if (error instanceof Error && error.message.trim().length > 0) {
    return new ApiError(error.message);
  }

  return new ApiError('An unexpected error occurred.');
}
