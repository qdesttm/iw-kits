import { http } from '@/shared/api';
import type {
  CreateOrderDto,
  ImportOrdersResponse,
  Order,
  OrdersQuery,
  OrdersResponse,
} from '../model/order.types';

const ORDERS_URL = '/orders';

export const ordersApi = {
  getAll: async (query?: OrdersQuery): Promise<OrdersResponse> => {
    const response = await http.get(ORDERS_URL, { params: query });
    return {
      items: response.data?.items || [],
      total_count: response.data?.total_count || 0,
      total_pages: response.data?.total_pages || 0,
    };
  },

  create: async (data: CreateOrderDto): Promise<Order> => {
    const response = await http.post(ORDERS_URL, data);
    return response.data?.created_order;
  },

  importCsv: async (
    file: File,
    onProgress?: (percent: number) => void,
  ): Promise<ImportOrdersResponse> => {
    const formData = new FormData();
    formData.append('file', file);

    const response = await http.post(`${ORDERS_URL}/import`, formData, {
      onUploadProgress: (event) => {
        if (!onProgress || !event.total) return;
        onProgress(Math.round((event.loaded * 100) / event.total));
      },
    });

    return {
      imported_total: response.data?.imported_total ?? 0,
      errors: Array.isArray(response.data?.errors) ? response.data.errors : [],
    };
  },
};
