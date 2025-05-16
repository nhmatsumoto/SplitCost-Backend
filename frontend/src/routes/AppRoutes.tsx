import { BrowserRouter, Routes, Route } from 'react-router-dom';
import ProtectedRoute from './ProtectedRoute';
import { AppLayout } from '../components/layout/AppLayout';
import ResidenceCreatePage from '../pages/private/residence/ResidenceCreatePage';
import ResidenceDetailsPage from '../pages/private/residence/ResidenceDetailsPage';
import HomePage from '../pages/public/HomePage';
import { RegisterForm } from '../components/user/RegisterForm';

const AppRoutes = () => {
  return (
    <BrowserRouter>
      <Routes>
        <Route
            path="/"
            element={
                <HomePage />
            }
        />
        <Route
            path="/register"
            element={
                <RegisterForm />
            }
        />
        <Route
            path="/residence"
            element={
                <ProtectedRoute>
                    <AppLayout>
                        <ResidenceDetailsPage />
                    </AppLayout>
                </ProtectedRoute>
            }
        />
        <Route
            path="/residence/create"
            element={
                <ProtectedRoute>
                    <AppLayout>
                        <ResidenceCreatePage />
                    </AppLayout>
                </ProtectedRoute>
            }
        />

      </Routes>
    </BrowserRouter>
  );
};

export default AppRoutes;
