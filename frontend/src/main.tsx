import React from 'react'
import { createRoot } from 'react-dom/client'
import AppRoutes from './routes/AppRoutes'
import { AuthProvider } from 'react-oidc-context';
import './index.css'
import oidcConfig from './configuration/oidcConfig';
import { Log } from "oidc-client-ts";

Log.setLevel(Log.DEBUG);
Log.setLogger(console);

createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <AuthProvider {...oidcConfig}>
      <AppRoutes />
    </AuthProvider>
  </React.StrictMode>
)
