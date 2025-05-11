import { createRoot } from 'react-dom/client'
import { ReactKeycloakProvider } from '@react-keycloak/web'
import keycloak from './keycloak'
import './index.css'
import AppRoutes from './routes/AppRoutes'

createRoot(document.getElementById('root')!).render(
    <ReactKeycloakProvider authClient={keycloak}>
      <AppRoutes />
    </ReactKeycloakProvider>
)
