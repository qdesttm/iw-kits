import { useState } from 'react';
import { Form, Input, Button, Typography, message } from 'antd';
import { UserOutlined, LockOutlined } from '@ant-design/icons';
import { useAuth } from '../model/useAuth';

const { Text, Link } = Typography;

const MIN_PASSWORD_LENGTH = 6;

interface RegisterFormProps {
  onSwitchToLogin: () => void;
}

interface RegisterValues {
  username: string;
  password: string;
  confirmPassword: string;
}

export function RegisterForm({ onSwitchToLogin }: RegisterFormProps) {
  const { register } = useAuth();
  const [form] = Form.useForm<RegisterValues>();
  const [submitting, setSubmitting] = useState(false);

  const handleRegister = async (values: RegisterValues) => {
    setSubmitting(true);
    try {
      const error = await register(values.username, values.password);
      if (error) {
        void message.error(error);
      }
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <>
      <Form
        form={form}
        onFinish={(values) => void handleRegister(values)}
        layout="vertical"
        size="large"
      >
        <Form.Item name="username" rules={[{ required: true, message: 'Enter username' }]}>
          <Input prefix={<UserOutlined />} placeholder="Username" autoComplete="username" />
        </Form.Item>
        <Form.Item
          name="password"
          rules={[
            { required: true, message: 'Enter password' },
            { min: MIN_PASSWORD_LENGTH, message: `At least ${MIN_PASSWORD_LENGTH} characters` },
          ]}
        >
          <Input.Password
            prefix={<LockOutlined />}
            placeholder="Password"
            autoComplete="new-password"
          />
        </Form.Item>
        <Form.Item
          name="confirmPassword"
          dependencies={['password']}
          rules={[
            { required: true, message: 'Repeat the password' },
            ({ getFieldValue }) => ({
              validator(_rule, value) {
                if (!value || getFieldValue('password') === value) {
                  return Promise.resolve();
                }
                return Promise.reject(new Error('Passwords do not match'));
              },
            }),
          ]}
        >
          <Input.Password
            prefix={<LockOutlined />}
            placeholder="Repeat password"
            autoComplete="new-password"
          />
        </Form.Item>
        <Form.Item>
          <Button type="primary" htmlType="submit" loading={submitting} block>
            Sign up
          </Button>
        </Form.Item>
      </Form>

      <div style={{ textAlign: 'center' }}>
        <Text type="secondary">Already have an account? </Text>
        <Link onClick={onSwitchToLogin}>Log in</Link>
      </div>
    </>
  );
}
