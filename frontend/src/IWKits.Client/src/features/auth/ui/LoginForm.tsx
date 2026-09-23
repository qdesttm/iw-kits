import { useState } from 'react';
import { Form, Input, Button, Typography, message } from 'antd';
import { UserOutlined, LockOutlined } from '@ant-design/icons';
import { useAuth } from '../model/useAuth';

const { Text, Link } = Typography;

interface LoginFormProps {
  onSwitchToRegister: () => void;
}

export function LoginForm({ onSwitchToRegister }: LoginFormProps) {
  const { login } = useAuth();
  const [submitting, setSubmitting] = useState(false);

  const handleLogin = async (values: { username: string; password: string }) => {
    setSubmitting(true);
    try {
      const error = await login(values.username, values.password);
      if (error) {
        void message.error(error);
      }
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <>
      <Form onFinish={(values) => void handleLogin(values)} layout="vertical" size="large">
        <Form.Item name="username" rules={[{ required: true, message: 'Enter username' }]}>
          <Input prefix={<UserOutlined />} placeholder="Username" autoComplete="username" />
        </Form.Item>
        <Form.Item name="password" rules={[{ required: true, message: 'Enter password' }]}>
          <Input.Password
            prefix={<LockOutlined />}
            placeholder="Password"
            autoComplete="current-password"
          />
        </Form.Item>
        <Form.Item>
          <Button type="primary" htmlType="submit" loading={submitting} block>
            Log in
          </Button>
        </Form.Item>
      </Form>

      <div style={{ textAlign: 'center' }}>
        <Text type="secondary">Don&apos;t have an account? </Text>
        <Link onClick={onSwitchToRegister}>Sign up</Link>
      </div>
    </>
  );
}
