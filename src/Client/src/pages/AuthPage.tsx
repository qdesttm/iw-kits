import { useState } from 'react';
import { Card, Typography } from 'antd';
import { Navigate } from 'react-router-dom';
import { LoginForm, RegisterForm, useAuth } from '@/features/auth';
import { authGradient, radius } from '@/shared/config/theme';

const { Title, Text } = Typography;

type AuthMode = 'login' | 'register';

export default function AuthPage() {
  const { isAuthenticated } = useAuth();
  const [mode, setMode] = useState<AuthMode>('login');

  if (isAuthenticated) {
    return <Navigate to="/order" replace />;
  }

  return (
    <div
      style={{
        minHeight: '100vh',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        background: authGradient,
        padding: 16,
      }}
    >
      <Card
        style={{
          width: 400,
          maxWidth: '100%',
          borderRadius: radius.panel,
          boxShadow: '0 8px 32px rgba(0,0,0,0.2)',
        }}
      >
        <div style={{ textAlign: 'center', marginBottom: 24 }}>
          <Title level={3} style={{ marginBottom: 4 }}>
            IW Kits
          </Title>
          <Text type="secondary">
            {mode === 'login' ? 'Sign in to continue' : 'Create a new account'}
          </Text>
        </div>

        {mode === 'login' ? (
          <LoginForm onSwitchToRegister={() => setMode('register')} />
        ) : (
          <RegisterForm onSwitchToLogin={() => setMode('login')} />
        )}
      </Card>
    </div>
  );
}
