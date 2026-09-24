import { Modal, Alert, List, Typography, Button, Space, Tag } from 'antd';
import { DownloadOutlined } from '@ant-design/icons';
import type { ApiErrorInfo } from '@/shared/api';
import type { ImportOrdersResponse } from '../model/order.types';

const { Text } = Typography;

interface ImportResultModalProps {
  result: ImportOrdersResponse | null;
  onClose: () => void;
}

function formatError(error: ApiErrorInfo): string {
  return error.code ? `${error.code}: ${error.message}` : error.message;
}

function downloadErrors(errors: ApiErrorInfo[]): void {
  const blob = new Blob([errors.map(formatError).join('\n')], {
    type: 'text/plain;charset=utf-8',
  });
  const url = URL.createObjectURL(blob);

  const link = document.createElement('a');
  link.href = url;
  link.download = 'import-errors.txt';
  link.click();

  URL.revokeObjectURL(url);
}

export function ImportResultModal({ result, onClose }: ImportResultModalProps) {
  if (!result) return null;

  const { importedTotal: imported, errors } = result;
  const failed = errors.length;
  const hasFailures = failed > 0;

  const footer = [
    <Button key="close" type="primary" onClick={onClose}>
      Close
    </Button>,
  ];

  if (hasFailures) {
    footer.unshift(
      <Button key="download" icon={<DownloadOutlined />} onClick={() => downloadErrors(errors)}>
        Download errors
      </Button>,
    );
  }

  return (
    <Modal title="CSV import result" open={true} onCancel={onClose} footer={footer} width={640}>
      <Space direction="vertical" size="middle" style={{ width: '100%' }}>
        <Alert
          type={hasFailures ? (imported > 0 ? 'warning' : 'error') : 'success'}
          showIcon
          message={
            hasFailures
              ? `Imported ${imported}, failed ${failed}`
              : `Imported ${imported} order${imported === 1 ? '' : 's'}`
          }
          description={
            hasFailures
              ? 'The rows listed below were rejected and were not saved.'
              : 'All rows in the file were imported successfully.'
          }
        />

        {hasFailures && (
          <div style={{ maxHeight: 320, overflowY: 'auto' }}>
            <List
              size="small"
              bordered
              dataSource={errors}
              renderItem={(error, index) => (
                <List.Item>
                  <Space size={8} wrap>
                    <Text type="secondary" style={{ fontSize: 13 }}>
                      {index + 1}.
                    </Text>
                    {error.code && <Tag color="red">{error.code}</Tag>}
                    <Text type="danger" style={{ fontSize: 13 }}>
                      {error.message}
                    </Text>
                  </Space>
                </List.Item>
              )}
            />
          </div>
        )}
      </Space>
    </Modal>
  );
}
