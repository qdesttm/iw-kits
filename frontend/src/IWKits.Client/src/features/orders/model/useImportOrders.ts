import { useCallback, useState } from 'react';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { ordersApi } from '../api/orders.api';
import { ordersKeys } from './orders.keys';
import type { ImportOrdersResponse } from './order.types';

export function useImportOrders() {
  const queryClient = useQueryClient();
  const [progress, setProgress] = useState(0);

  const mutation = useMutation<ImportOrdersResponse, unknown, File>({
    mutationFn: (file) => ordersApi.importCsv(file, setProgress),
    onSuccess: (result) => {
      if (result.imported_total > 0) {
        void queryClient.invalidateQueries({ queryKey: ordersKeys.all });
      }
    },
    onSettled: () => {
      setProgress(0);
    },
  });

  const importFile = useCallback(
    (file: File) => {
      setProgress(0);
      return mutation.mutateAsync(file);
    },
    [mutation],
  );

  return {
    importFile,
    progress,
    isImporting: mutation.isPending,
  };
}
