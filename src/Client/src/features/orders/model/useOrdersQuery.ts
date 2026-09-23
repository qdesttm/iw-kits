import { keepPreviousData, useQuery } from '@tanstack/react-query';
import { ordersApi } from '../api/orders.api';
import { ordersKeys } from './orders.keys';
import type { OrdersQuery, OrdersResponse } from './order.types';

export function useOrdersQuery(query: OrdersQuery) {
  return useQuery<OrdersResponse>({
    queryKey: ordersKeys.list(query),
    queryFn: () => ordersApi.getAll(query),
    placeholderData: keepPreviousData,
  });
}
