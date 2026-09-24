import type dayjs from 'dayjs';
import type { ApiErrorInfo } from '@/shared/api';

export type JurisdictionType = 'state' | 'county' | 'city' | 'special';

export type OrderSortField =
  | 'subtotal'
  | 'compositeTaxRate'
  | 'taxAmount'
  | 'totalAmount'
  | 'timestamp';

export type SortDirection = 'ascending' | 'descending';

export interface OrderTaxJurisdiction {
  name: string;
  type: JurisdictionType;
  rate: number;
}

export interface OrderTaxBreakdown {
  stateRate: number;
  countyRate: number;
  cityRate: number;
  specialRate: number;
}

export interface Order {
  id: string;
  latitude: number;
  longitude: number;
  subtotal: number;
  compositeTaxRate: number;
  taxAmount: number;
  totalAmount: number;
  breakdown: OrderTaxBreakdown;
  jurisdictions: OrderTaxJurisdiction[];
  timestamp: string;
}

export interface OrdersQuery {
  page?: number;
  size?: number;
  sortBy?: OrderSortField;
  sortDirection?: SortDirection;
  minTotalAmount?: number;
  maxTotalAmount?: number;
  after?: string;
  before?: string;
}

export interface CreateOrderDto {
  latitude: number;
  longitude: number;
  subtotal: number;
}

export interface OrdersResponse {
  items: Order[];
  page: number;
  size: number;
  itemsCount: number;
}

export interface ImportOrdersResponse {
  importedTotal: number;
  errors: ApiErrorInfo[];
}

export interface OrdersFiltersState {
  dateRange: [dayjs.Dayjs | null, dayjs.Dayjs | null] | null;
  minAmount: number | null;
  maxAmount: number | null;
}
