import { BrowserRouter, Routes, Route } from 'react-router-dom';
import HomePage from '../pages/public/HomePage';
import LoginPage from '../pages/public/LoginPage';
import ProfilePage from '../pages/private/user/ProfilePage';
import { AppLayout } from '../components/layout/AppLayout';

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
            path="/login"
            element={
              <LoginPage />
            }
        />
        <Route
            path="/profile"
            element={
              <AppLayout>
                <ProfilePage />
              </AppLayout>  
            }
        />
      </Routes>
    </BrowserRouter>
  );
};

export default AppRoutes;