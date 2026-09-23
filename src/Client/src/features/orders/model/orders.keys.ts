import type { OrdersQuery } from './order.types';

export const ordersKeys = {
  all: ['orders'] as const,
  list: (query: OrdersQuery) => ['orders', 'list', query] as const,
};
