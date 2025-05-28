import { createRoot } from 'react-dom/client'
import AppRoutes from './routes/AppRoutes'
import { AuthProvider } from 'react-oidc-context';
import './index.css'
import oidcConfig from './configuration/oidcConfig';
import { Log } from "oidc-client-ts";

Log.setLevel(Log.DEBUG);

createRoot(document.getElementById('root')!).render(
  <AuthProvider {...oidcConfig}>
    <AppRoutes />
  </AuthProvider>
)
