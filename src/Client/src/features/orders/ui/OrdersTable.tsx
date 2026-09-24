import { Table, Space, Tag, Tooltip } from 'antd';
import { InfoCircleOutlined } from '@ant-design/icons';
import type { ColumnsType, TablePaginationConfig } from 'antd/es/table/index.js';
import type { FilterValue, SorterResult } from 'antd/es/table/interface.js';
import { colors } from '@/shared/config/theme';
import type { Order, OrderSortField } from '../model/order.types';

const columns: ColumnsType<Order> = [
  { title: 'ID', dataIndex: 'id', key: 'id', width: 80, render: (id: string) => <span title={id}>{id?.substring(0, 8)}...</span> },
  {
    title: 'Date & Time',
    dataIndex: 'timestamp',
    key: 'timestamp',
    width: 180,
    sorter: true,
    render: (ts) => ts ? new Date(ts).toLocaleString() : '-'
  },
  {
    title: 'Location',
    key: 'coordinates',
    width: 160,
    render: (_, record) => `${record.latitude?.toFixed(4)}, ${record.longitude?.toFixed(4)}`
  },
  { title: 'Subtotal', dataIndex: 'subtotal', key: 'subtotal', width: 100, sorter: true, render: (val) => `$${val?.toFixed(2) || 0}` },
  {
    title: 'Tax',
    key: 'taxAmount',
    dataIndex: 'taxAmount',
    width: 110,
    sorter: true,
    render: (_, record) => (
      <Tooltip title={`Rate: ${((record.compositeTaxRate || 0) * 100).toFixed(3)}%`}>
        <span>${record.taxAmount?.toFixed(2) || 0} <InfoCircleOutlined style={{ color: colors.primary, fontSize: 12 }} /></span>
      </Tooltip>
    )
  },
  {
    title: 'Total',
    dataIndex: 'totalAmount',
    key: 'totalAmount',
    width: 100,
    sorter: true,
    render: (val) => <strong style={{ color: colors.primary }}>${val?.toFixed(2) || 0}</strong>
  },
  {
    title: 'Breakdown',
    key: 'breakdown',
    width: 320,
    render: (_, record) => (
      <Space orientation="horizontal" size={[0, 4]} wrap>
        {record.breakdown?.stateRate > 0 && <Tag color="blue">State: {record.breakdown.stateRate}</Tag>}
        {record.breakdown?.cityRate > 0 && <Tag color="cyan">City: {record.breakdown.cityRate}</Tag>}
        {record.breakdown?.countyRate > 0 && <Tag color="purple">County: {record.breakdown.countyRate}</Tag>}
        {record.breakdown?.specialRate > 0 && <Tag color="orange">Special: {record.breakdown.specialRate}</Tag>}
      </Space>
    )
  },
  {
    title: 'Jurisdictions',
    key: 'jurisdictions',
    width: 320,
    render: (_, record) => (
      <Space orientation="horizontal" size={[0, 4]} wrap>
        {record.jurisdictions?.map((j, idx) => (
          <Tooltip key={idx} title={`Rate: ${j.rate}`}>
            <Tag variant="filled">{j.name}</Tag>
          </Tooltip>
        ))}
      </Space>
    )
  },
];

const sortFieldMap: Record<string, OrderSortField> = {
  timestamp: 'timestamp',
  subtotal: 'subtotal',
  taxAmount: 'taxAmount',
  totalAmount: 'totalAmount',
};

interface OrdersTableProps {
  orders: Order[];
  loading: boolean;
  total: number;
  currentPage: number;
  pageSize: number;
  onTableChange: (page: number, pageSize: number, sortBy?: string, descending?: boolean) => void;
}

export function OrdersTable({ orders, loading, total, currentPage, pageSize, onTableChange }: OrdersTableProps) {
  const handleChange = (
    pagination: TablePaginationConfig,
    _filters: Record<string, FilterValue | null>,
    sorter: SorterResult<Order> | SorterResult<Order>[]
  ) => {
    const newPage = pagination.current || 1;
    const newPageSize = pagination.pageSize || pageSize;

    const singleSorter = Array.isArray(sorter) ? sorter[0] : sorter;
    const columnKey = singleSorter?.columnKey as string | undefined;
    const sortField = columnKey ? sortFieldMap[columnKey] : undefined;
    const descending = singleSorter?.order === 'descend' ? true
      : singleSorter?.order === 'ascend' ? false
        : undefined;

    onTableChange(newPage, newPageSize, sortField, descending);
  };

  return (
    <Table
      columns={columns}
      dataSource={orders}
      rowKey="id"
      loading={loading}
      onChange={handleChange}
      pagination={{
        current: currentPage,
        pageSize: pageSize,
        total: total,
        showSizeChanger: true,
        pageSizeOptions: ['10', '25', '50', '100'],
        showTotal: (total, range) => `${range[0]}-${range[1]} of ${total} orders`,
      }}
      scroll={{ x: 'max-content' }}
      locale={{ emptyText: 'No orders found.' }}
    />
  );
}
