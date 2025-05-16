import React from 'react'
import { createRoot } from 'react-dom/client'
import AppRoutes from './routes/AppRoutes'
import { AuthProvider } from 'react-oidc-context';
import './index.css'
import UserDataLoader from './components/auth/UserDataLoader';
import oidcConfig from './configuration/oidcConfig';

createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <AuthProvider {...oidcConfig}>
      <UserDataLoader />
      <AppRoutes />
    </AuthProvider>
  </React.StrictMode>
)
