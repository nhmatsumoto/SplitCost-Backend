import { BrowserRouter, Routes, Route } from 'react-router-dom';
import HomePage from '../pages/public/HomePage';
import ProfilePage from '../pages/private/ProfilePage';
import { AppLayout } from '../components/layout/AppLayout';
import ProtectedRoute from './ProtectedRoute';
import HousePage from '../pages/private/HousePage';
import ExpensesPage from '../pages/private/ExpensesPage';
import RegistrationForm from '../pages/public/RegistrationForm';
import CreateResidenceForm from '../pages/private/CreateResidenceForm';
import CreateExpenseForm from '../pages/private/CreateExpenseForm';
import TransactionsPage from '../pages/private/TransactionsPage';
import IncomesPage from '../pages/private/IcomesPage';

const AppRoutes = () => {
  return (
    <BrowserRouter>
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
            path="/house"
            element={
              <AppLayout>
                <ProtectedRoute children={<HousePage />} />
              </AppLayout>  
            }
        />
        <Route
            path="/house/create"
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