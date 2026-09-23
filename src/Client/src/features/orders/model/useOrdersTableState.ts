import { useCallback, useMemo } from 'react';
import { useSearchParams } from 'react-router-dom';
import dayjs from 'dayjs';
import type { OrdersFiltersState, OrdersQuery } from './order.types';

const DEFAULT_PAGE_SIZE = 24;
const DEFAULT_SORT_BY = 'timestamp';

function readNumber(value: string | null, fallback: number): number {
  if (!value) return fallback;
  const parsed = Number(value);
  return Number.isFinite(parsed) && parsed > 0 ? parsed : fallback;
}

function readOptionalNumber(value: string | null): number | null {
  if (!value) return null;
  const parsed = Number(value);
  return Number.isFinite(parsed) ? parsed : null;
}

function readDate(value: string | null): dayjs.Dayjs | null {
  if (!value) return null;
  const parsed = dayjs(value);
  return parsed.isValid() ? parsed : null;
}

export interface OrdersTableState {
  page: number;
  pageSize: number;
  sortBy: string;
  descending: boolean;
  filters: OrdersFiltersState;
  query: OrdersQuery;
  setPagination: (page: number, pageSize: number, sortBy?: string, descending?: boolean) => void;
  setFilters: (filters: OrdersFiltersState) => void;
  resetFilters: () => void;
}

export function useOrdersTableState(): OrdersTableState {
  const [searchParams, setSearchParams] = useSearchParams();

  const page = readNumber(searchParams.get('page'), 1);
  const pageSize = readNumber(searchParams.get('pageSize'), DEFAULT_PAGE_SIZE);
  const sortBy = searchParams.get('sortBy') ?? DEFAULT_SORT_BY;
  const descending = (searchParams.get('descending') ?? 'true') === 'true';

  const filters = useMemo<OrdersFiltersState>(() => {
    const from = readDate(searchParams.get('from'));
    const to = readDate(searchParams.get('to'));

    return {
      dateRange: from || to ? [from, to] : null,
      minAmount: readOptionalNumber(searchParams.get('min')),
      maxAmount: readOptionalNumber(searchParams.get('max')),
    };
  }, [searchParams]);

  const query = useMemo<OrdersQuery>(() => {
    const next: OrdersQuery = { page, page_size: pageSize, sort_by: sortBy, descending };

    if (filters.dateRange?.[0]) next.from_date = filters.dateRange[0].toISOString();
    if (filters.dateRange?.[1]) next.to_date = filters.dateRange[1].toISOString();
    if (filters.minAmount !== null) next.min_total_amount = filters.minAmount;
    if (filters.maxAmount !== null) next.max_total_amount = filters.maxAmount;

    return next;
  }, [page, pageSize, sortBy, descending, filters]);

  const setPagination = useCallback(
    (nextPage: number, nextPageSize: number, nextSortBy?: string, nextDescending?: boolean) => {
      setSearchParams(
        (current) => {
          const params = new URLSearchParams(current);
          params.set('page', String(nextPage));
          params.set('pageSize', String(nextPageSize));
          if (nextSortBy !== undefined) params.set('sortBy', nextSortBy);
          if (nextDescending !== undefined) params.set('descending', String(nextDescending));
          return params;
        },
        { replace: true },
      );
    },
    [setSearchParams],
  );

  const setFilters = useCallback(
    (next: OrdersFiltersState) => {
      setSearchParams(
        (current) => {
          const params = new URLSearchParams(current);

          const applyValue = (key: string, value: string | null) => {
            if (value === null) {
              params.delete(key);
            } else {
              params.set(key, value);
            }
          };

          applyValue('from', next.dateRange?.[0]?.toISOString() ?? null);
          applyValue('to', next.dateRange?.[1]?.toISOString() ?? null);
          applyValue('min', next.minAmount === null ? null : String(next.minAmount));
          applyValue('max', next.maxAmount === null ? null : String(next.maxAmount));
          params.set('page', '1');

          return params;
        },
        { replace: true },
      );
    },
    [setSearchParams],
  );

  const resetFilters = useCallback(() => {
    setSearchParams(
      (current) => {
        const params = new URLSearchParams(current);
        for (const key of ['from', 'to', 'min', 'max']) {
          params.delete(key);
        }
        params.set('page', '1');
        return params;
      },
      { replace: true },
    );
  }, [setSearchParams]);

  return { page, pageSize, sortBy, descending, filters, query, setPagination, setFilters, resetFilters };
}
