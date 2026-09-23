import { useState } from 'react';
import { Button, Space, Upload, Typography, message, Card } from 'antd';
import { UploadOutlined, PlusOutlined, ReloadOutlined, FilterOutlined, LoadingOutlined } from '@ant-design/icons';
import { toApiError } from '@/shared/api';
import { radius } from '@/shared/config/theme';
import { useImportOrders } from '../model/useImportOrders';
import type { ImportOrdersResponse } from '../model/order.types';
import { ImportResultModal } from './ImportResultModal';

const { Title } = Typography;

interface OrdersToolbarProps {
  showFilters: boolean;
  onToggleFilters: () => void;
  onCreateClick: () => void;
  onRefresh: () => void;
}

export function OrdersToolbar({ showFilters, onToggleFilters, onCreateClick, onRefresh }: OrdersToolbarProps) {
  const [importResult, setImportResult] = useState<ImportOrdersResponse | null>(null);
  const { importFile, progress, isImporting } = useImportOrders();

  const handleImport = async (file: File) => {
    void message.loading({ content: 'Uploading and processing CSV…', key: 'upload', duration: 0 });

    try {
      const result = await importFile(file);
      message.destroy('upload');
      setImportResult(result);
    } catch (error) {
      message.destroy('upload');
      void message.error(toApiError(error).message);
    }
  };

  const uploadLabel = isImporting
    ? progress > 0 && progress < 100
      ? `Uploading ${progress}%`
      : 'Processing…'
    : 'Import CSV';

  return (
    <>
      <Card style={{ marginBottom: 16, borderRadius: radius.card }} styles={{ body: { padding: '16px 24px' } }}>
        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <Title level={4} style={{ margin: 0 }}>Orders List</Title>
          <Space>
            <Button
              icon={<FilterOutlined />}
              onClick={onToggleFilters}
              type={showFilters ? 'primary' : 'default'}
            >
              Filters
            </Button>

            <Upload
              name="file"
              accept=".csv,text/csv"
              showUploadList={false}
              disabled={isImporting}
              beforeUpload={(file) => {
                void handleImport(file as unknown as File);
                return false;
              }}
            >
              <Button
                icon={isImporting ? <LoadingOutlined /> : <UploadOutlined />}
                loading={isImporting}
                disabled={isImporting}
              >
                {uploadLabel}
              </Button>
            </Upload>

            <Button type="primary" icon={<PlusOutlined />} onClick={onCreateClick} disabled={isImporting}>
              Create Manually
            </Button>
            <Button icon={<ReloadOutlined />} onClick={onRefresh} disabled={isImporting} />
          </Space>
        </div>
      </Card>

      <ImportResultModal result={importResult} onClose={() => setImportResult(null)} />
    </>
  );
}
