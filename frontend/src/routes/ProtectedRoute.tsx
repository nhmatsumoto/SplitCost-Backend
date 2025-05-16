import React from 'react';
import { useAuth } from 'react-oidc-context';
import { useLocation } from 'react-router-dom';
import Loader from '../components/ui/Loader';

interface ProtectedRouteProps {
  children: React.ReactNode;
}

const ProtectedRoute: React.FC<ProtectedRouteProps> = ({ children }) => {
  const { isAuthenticated, isLoading, signinRedirect } = useAuth();
  const location = useLocation();
  if (isLoading) {
    return <Loader />; 
  }

  if (isAuthenticated) {
    return <>{children}</>;
  }

  return (
    <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', minHeight: '300px' }}>
      <h2>É necessário fazer login para acessar esta página.</h2>
      <button onClick={() => signinRedirect({ state: { from:  location.pathname } })} style={{ marginTop: '20px', padding: '10px 20px', backgroundColor: '#007BFF', color: '#fff', border: 'none', borderRadius: '5px', cursor: 'pointer' }}>
        Fazer Login
      </button>
    </div>
  );
};

export default ProtectedRoute;