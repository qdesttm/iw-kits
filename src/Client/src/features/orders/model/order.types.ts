import type dayjs from 'dayjs';

export interface TaxJurisdiction {
  name: string;
  type: string;
  rate: number;
}

export interface Order {
  id: string;
  latitude: number;
  longitude: number;
  subtotal: number;
  composite_tax_rate: number;
  tax_amount: number;
  total_amount: number;
  breakdown: {
    state_rate: number;
    county_rate: number;
    city_rate: number;
    special_rate: number;
  };
  jurisdictions: TaxJurisdiction[];
  timestamp: string;
}

export interface CreateOrderDto {
  latitude: number;
  longitude: number;
  subtotal: number;
}

export interface OrdersQuery {
  page?: number;
  page_size?: number;
  sort_by?: string;
  descending?: boolean;
  min_total_amount?: number;
  max_total_amount?: number;
  from_date?: string;
  to_date?: string;
}

export interface OrdersResponse {
  items: Order[];
  total_count: number;
  total_pages: number;
}

export interface ImportOrdersResponse {
  imported_total: number;
  errors: string[];
}

export interface OrdersFiltersState {
  dateRange: [dayjs.Dayjs | null, dayjs.Dayjs | null] | null;
  minAmount: number | null;
  maxAmount: number | null;
}
