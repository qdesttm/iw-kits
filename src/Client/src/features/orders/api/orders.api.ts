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
    const body = response.data;

    return {
      items: Array.isArray(body?.data) ? (body.data as Order[]) : [],
      page: body?.page ?? 1,
      size: body?.size ?? 0,
      itemsCount: body?.itemsCount ?? 0,
    };
  },

  create: async (data: CreateOrderDto): Promise<string> => {
    const response = await http.post(ORDERS_URL, null, {
      params: {
        latitude: data.latitude,
        longitude: data.longitude,
        subtotal: data.subtotal,
      },
    });

    return String(response.data?.data ?? '');
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
      importedTotal: response.data?.importedTotal ?? 0,
      errors: Array.isArray(response.data?.errors) ? response.data.errors : [],
    };
  },
};
