import { useMutation, useQueryClient } from '@tanstack/react-query';
import { ordersApi } from '../api/orders.api';
import { ordersKeys } from './orders.keys';
import type { CreateOrderDto } from './order.types';

export function useCreateOrder() {
  const queryClient = useQueryClient();

  return useMutation<string, unknown, CreateOrderDto>({
    mutationFn: (data) => ordersApi.create(data),
    onSuccess: () => {
      void queryClient.invalidateQueries({ queryKey: ordersKeys.all });
    },
  });
}
