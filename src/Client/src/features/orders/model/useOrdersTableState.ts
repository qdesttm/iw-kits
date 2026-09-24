import { useCallback, useMemo } from 'react';
import { useSearchParams } from 'react-router-dom';
import dayjs from 'dayjs';
import type {
  OrderSortField,
  OrdersFiltersState,
  OrdersQuery,
  SortDirection,
} from './order.types';

const DEFAULT_PAGE_SIZE = 24;
const DEFAULT_SORT_BY: OrderSortField = 'timestamp';
const DEFAULT_SORT_DIRECTION: SortDirection = 'descending';

const SORT_FIELDS: readonly OrderSortField[] = [
  'subtotal',
  'compositeTaxRate',
  'taxAmount',
  'totalAmount',
  'timestamp',
];

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

function readSortField(value: string | null): OrderSortField {
  return SORT_FIELDS.includes(value as OrderSortField)
    ? (value as OrderSortField)
    : DEFAULT_SORT_BY;
}

function readSortDirection(value: string | null): SortDirection {
  return value === 'ascending' || value === 'descending' ? value : DEFAULT_SORT_DIRECTION;
}

export interface OrdersTableState {
  page: number;
  pageSize: number;
  sortBy: OrderSortField;
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
  const pageSize = readNumber(searchParams.get('size'), DEFAULT_PAGE_SIZE);
  const sortBy = readSortField(searchParams.get('sortBy'));
  const sortDirection = readSortDirection(searchParams.get('sortDirection'));
  const descending = sortDirection === 'descending';

  const filters = useMemo<OrdersFiltersState>(() => {
    const after = readDate(searchParams.get('after'));
    const before = readDate(searchParams.get('before'));

    return {
      dateRange: after || before ? [after, before] : null,
      minAmount: readOptionalNumber(searchParams.get('minTotalAmount')),
      maxAmount: readOptionalNumber(searchParams.get('maxTotalAmount')),
    };
  }, [searchParams]);

  const query = useMemo<OrdersQuery>(() => {
    const next: OrdersQuery = { page, size: pageSize, sortBy, sortDirection };

    if (filters.dateRange?.[0]) next.after = filters.dateRange[0].toISOString();
    if (filters.dateRange?.[1]) next.before = filters.dateRange[1].toISOString();
    if (filters.minAmount !== null) next.minTotalAmount = filters.minAmount;
    if (filters.maxAmount !== null) next.maxTotalAmount = filters.maxAmount;

    return next;
  }, [page, pageSize, sortBy, sortDirection, filters]);

  const setPagination = useCallback(
    (nextPage: number, nextPageSize: number, nextSortBy?: string, nextDescending?: boolean) => {
      setSearchParams(
        (current) => {
          const params = new URLSearchParams(current);
          params.set('page', String(nextPage));
          params.set('size', String(nextPageSize));

          if (nextSortBy !== undefined) {
            params.set('sortBy', readSortField(nextSortBy));
          }

          if (nextDescending !== undefined) {
            params.set('sortDirection', nextDescending ? 'descending' : 'ascending');
          }

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

          applyValue('after', next.dateRange?.[0]?.toISOString() ?? null);
          applyValue('before', next.dateRange?.[1]?.toISOString() ?? null);
          applyValue('minTotalAmount', next.minAmount === null ? null : String(next.minAmount));
          applyValue('maxTotalAmount', next.maxAmount === null ? null : String(next.maxAmount));
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
        for (const key of ['after', 'before', 'minTotalAmount', 'maxTotalAmount']) {
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
