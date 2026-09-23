import { Suspense, lazy } from 'react';
import { Routes, Route, Navigate } from 'react-router-dom';
import { ProtectedRoute, useAuth } from '@/features/auth';
import { PageLoader } from '@/shared/ui';
import { AppProviders } from './providers';

const AppLayout = lazy(() => import('@/layouts/AppLayout'));
const OrdersPage = lazy(() => import('@/pages/OrdersPage'));
const AuthPage = lazy(() => import('@/pages/AuthPage'));

function RootRedirect() {
  const { isAuthenticated, loading } = useAuth();

  if (loading) {
    return <PageLoader />;
  }

  return <Navigate to={isAuthenticated ? '/order' : '/auth'} replace />;
}

const App = () => (
  <AppProviders>
    <Suspense fallback={<PageLoader />}>
      <Routes>
        <Route path="/auth" element={<AuthPage />} />
        <Route path="/" element={<RootRedirect />} />
        <Route
          path="/*"
          element={
            <ProtectedRoute>
              <AppLayout>
                <Routes>
                  <Route path="/order" element={<OrdersPage />} />
                </Routes>
              </AppLayout>
            </ProtectedRoute>
          }
        />
      </Routes>
    </Suspense>
  </AppProviders>
);

export default App;
