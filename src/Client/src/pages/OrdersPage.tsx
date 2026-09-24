import { useState } from 'react';
import { Breadcrumb, Card, message } from 'antd';
import { HomeOutlined } from '@ant-design/icons';
import { Link } from 'react-router-dom';
import {
  CreateOrderModal,
  OrdersFilters,
  OrdersTable,
  OrdersToolbar,
  useOrdersQuery,
  useOrdersTableState,
} from '@/features/orders';
import { toApiError } from '@/shared/api';
import { colors, radius } from '@/shared/config/theme';

export default function OrdersPage() {
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [showFilters, setShowFilters] = useState(false);

  const table = useOrdersTableState();
  const { data, isFetching, isError, error, refetch } = useOrdersQuery(table.query);

  if (isError) {
    void message.error(toApiError(error).message);
  }

  return (
    <div style={{ background: colors.pageBackground, minHeight: '100vh', padding: '0 0 24px 0' }}>
      <div style={{ marginBottom: 16 }}>
        <Breadcrumb items={[{ title: <Link to="/"><HomeOutlined /></Link> }, { title: 'Orders' }]} />
      </div>

      <OrdersToolbar
        showFilters={showFilters}
        onToggleFilters={() => setShowFilters(!showFilters)}
        onCreateClick={() => setIsModalOpen(true)}
        onRefresh={() => void refetch()}
      />

      {showFilters && (
        <OrdersFilters
          filters={table.filters}
          onChange={table.setFilters}
          onReset={table.resetFilters}
        />
      )}

      <Card style={{ borderRadius: radius.card }} styles={{ body: { padding: 0 } }}>
        <OrdersTable
          orders={data?.items ?? []}
          loading={isFetching}
          total={data?.itemsCount ?? 0}
          currentPage={table.page}
          pageSize={table.pageSize}
          onTableChange={table.setPagination}
        />
      </Card>

      <CreateOrderModal isOpen={isModalOpen} onClose={() => setIsModalOpen(false)} />
    </div>
  );
}
