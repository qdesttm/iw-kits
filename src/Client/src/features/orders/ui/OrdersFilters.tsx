import { useEffect, useState, type ComponentProps } from 'react';
import { Button, Space, Card, DatePicker, InputNumber, Row, Col } from 'antd';
import { ClearOutlined } from '@ant-design/icons';
import dayjs from 'dayjs';
import { colors, radius } from '@/shared/config/theme';
import type { OrdersFiltersState } from '../model/order.types';

const { RangePicker } = DatePicker;

const DEBOUNCE_MS = 500;

const labelStyle = { marginBottom: 4, fontSize: 13, color: colors.textMuted };

interface OrdersFiltersProps {
  filters: OrdersFiltersState;
  onChange: (filters: OrdersFiltersState) => void;
  onReset: () => void;
}

export function OrdersFilters({ filters, onChange, onReset }: OrdersFiltersProps) {
  const [localFilters, setLocalFilters] = useState<OrdersFiltersState>(filters);
  const [syncedFrom, setSyncedFrom] = useState<OrdersFiltersState>(filters);

  if (filters !== syncedFrom) {
    setSyncedFrom(filters);
    setLocalFilters(filters);
  }

  useEffect(() => {
    if (localFilters === filters) return;

    const timer = setTimeout(() => onChange(localFilters), DEBOUNCE_MS);
    return () => clearTimeout(timer);
  }, [localFilters, filters, onChange]);

  const edit = (next: OrdersFiltersState) => {
    setLocalFilters(next);
  };

  const handleReset = () => {
    onReset();
  };

  return (
    <Card
      style={{ marginBottom: 16, borderRadius: radius.card }}
      styles={{ body: { padding: '16px 24px' } }}
    >
      <Row gutter={[16, 12]} align="bottom">
        <Col>
          <div style={labelStyle}>Date Range</div>
          <RangePicker
            value={localFilters.dateRange as Exclude<ComponentProps<typeof RangePicker>['value'], undefined>}
            onChange={(dates) =>
              edit({
                ...localFilters,
                dateRange: dates as [dayjs.Dayjs | null, dayjs.Dayjs | null] | null,
              })
            }
            showTime
            format="YYYY-MM-DD HH:mm"
          />
        </Col>
        <Col>
          <div style={labelStyle}>Min Total ($)</div>
          <InputNumber<number>
            value={localFilters.minAmount}
            onChange={(value) => edit({ ...localFilters, minAmount: value })}
            min={0}
            placeholder="0.00"
            style={{ width: 120 }}
          />
        </Col>
        <Col>
          <div style={labelStyle}>Max Total ($)</div>
          <InputNumber<number>
            value={localFilters.maxAmount}
            onChange={(value) => edit({ ...localFilters, maxAmount: value })}
            min={0}
            placeholder="999.99"
            style={{ width: 120 }}
          />
        </Col>
        <Col>
          <Space>
            <Button icon={<ClearOutlined />} onClick={handleReset}>
              Reset
            </Button>
          </Space>
        </Col>
      </Row>
    </Card>
  );
}
