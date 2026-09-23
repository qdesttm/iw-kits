import { Modal, Form, InputNumber, message } from 'antd';
import { toApiError } from '@/shared/api';
import { useCreateOrder } from '../model/useCreateOrder';

interface CreateOrderModalProps {
  isOpen: boolean;
  onClose: () => void;
}

export default function CreateOrderModal({ isOpen, onClose }: CreateOrderModalProps) {
  const [form] = Form.useForm();
  const createOrder = useCreateOrder();

  const handleManualCreate = async (values: { lat: number; lon: number; subtotal: number }) => {
    try {
      await createOrder.mutateAsync({
        latitude: values.lat,
        longitude: values.lon,
        subtotal: values.subtotal,
      });

      void message.success('Order successfully created!');
      form.resetFields();
      onClose();
    } catch (error) {
      void message.error(toApiError(error).message);
    }
  };

  const handleCancel = () => {
    form.resetFields();
    onClose();
  };

  return (
    <Modal
      title="Create Order"
      open={isOpen}
      onCancel={handleCancel}
      onOk={() => form.submit()}
      okText="Create & Calculate"
      cancelText="Cancel"
      confirmLoading={createOrder.isPending}
    >
      <Form form={form} layout="vertical" onFinish={handleManualCreate}>
        <Form.Item label="Latitude" name="lat" rules={[{ required: true, message: 'Please enter latitude' }]}>
          <InputNumber style={{ width: '100%' }} placeholder="e.g., 40.7128" />
        </Form.Item>
        <Form.Item label="Longitude" name="lon" rules={[{ required: true, message: 'Please enter longitude' }]}>
          <InputNumber style={{ width: '100%' }} placeholder="e.g., -74.0060" />
        </Form.Item>
        <Form.Item label="Subtotal ($)" name="subtotal" rules={[{ required: true, message: 'Please enter subtotal' }]}>
          <InputNumber style={{ width: '100%' }} min={0} placeholder="Kit value" />
        </Form.Item>
      </Form>
    </Modal>
  );
}
