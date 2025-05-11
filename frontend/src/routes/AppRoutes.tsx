import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Dashboard from '../pages/dashboard/DashboardPage';
import ProtectedRoute from './ProtectedRoute';
import { AppLayout } from '../components/layout/AppLayout';
import ResidencePage from '../pages/residence/ResidencePage';
import ResidenceCreatePage from '../pages/residence/ResidenceCreatePage';
import ResidenceDetailsPage from '../pages/residence/ResidenceDetailsPage';

const AppRoutes = () => {
  return (
    <BrowserRouter>
      <Routes>
   
        <Route
            path="/"
            element={
                <ProtectedRoute>
                    <AppLayout>
                        <Dashboard />
                    </AppLayout>
                </ProtectedRoute>
            }
        />

        <Route
            path="/residences"
            element={
                <ProtectedRoute>
                    <AppLayout>
                        <ResidencePage />
                    </AppLayout>
                </ProtectedRoute>
            }
        />

        <Route
            path="/residences/create"
            element={
                <ProtectedRoute>
                    <AppLayout>
                        <ResidenceCreatePage />
                    </AppLayout>
                </ProtectedRoute>
            }
        />

        <Route
            path="/residence/:id"
            element={
                <ProtectedRoute>
                    <AppLayout>
                        <ResidenceDetailsPage />
                    </AppLayout>
                </ProtectedRoute>
            }
        />

      </Routes>
    </BrowserRouter>
  );
};

export default AppRoutes;
