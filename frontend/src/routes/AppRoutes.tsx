import { BrowserRouter, Routes, Route } from 'react-router-dom';
import HomePage from '../pages/public/HomePage';
import ProfilePage from '../pages/private/ProfilePage';
import { AppLayout } from '../components/layout/AppLayout';
import ProtectedRoute from './ProtectedRoute';
import ResidencePage from '../pages/private/ResidencePage';
import ExpensesPage from '../pages/private/ExpensesPage';
import RegistrationForm from '../pages/public/RegistrationForm';
import CreateResidenceForm from '../pages/private/CreateResidenceForm';
import CreateExpenseForm from '../pages/private/CreateExpenseForm';
import TransactionsPage from '../pages/private/TransactionsPage';
import IncomesPage from '../pages/private/IcomesPage';
import AppInitializer from '../components/auth/AppInitializer';

const AppRoutes = () => {
  return (
    <BrowserRouter>
      <AppInitializer />
      <Routes>
        <Route
            path="/"
            element={
              <AppLayout>
                <HomePage />
              </AppLayout>
            }
        />
        <Route
            path="/transactions"
            element={
              <AppLayout>
                <ProtectedRoute children={<TransactionsPage />} />
              </AppLayout>  
            }
        />
        <Route
            path="/incomes"
            element={
              <AppLayout>
                <ProtectedRoute children={<IncomesPage />} />
              </AppLayout>  
            }
        />
        <Route
            path="/profile"
            element={
              <AppLayout>
                <ProtectedRoute children={<ProfilePage />} />
              </AppLayout>  
            }
        />
        <Route
            path="/register"
            element={
              <AppLayout>
                <RegistrationForm />
              </AppLayout>  
            }
        />
        <Route
            path="/residence"
            element={
              <AppLayout>
                <ProtectedRoute children={<ResidencePage />} />
              </AppLayout>  
            }
        />
        <Route
            path="/residence/create"
            element={
              <AppLayout>
                <ProtectedRoute children={<CreateResidenceForm />} />
              </AppLayout>  
            }
        />
        <Route
            path="/expenses"
            element={
              <AppLayout>
                <ProtectedRoute children={<ExpensesPage />} />
              </AppLayout>  
            }
        />
        <Route
            path="/expense/create"
            element={
              <AppLayout>
                <ProtectedRoute children={<CreateExpenseForm />} />
              </AppLayout>  
            }
        />
      </Routes>
    </BrowserRouter>
  );
};

export default AppRoutes;