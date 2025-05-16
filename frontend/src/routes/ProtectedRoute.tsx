import React from 'react';
import { useAuth } from 'react-oidc-context';
import { Navigate, useLocation } from 'react-router-dom';

interface ProtectedRouteProps {
  children: React.ReactNode;
}

const ProtectedRoute: React.FC<ProtectedRouteProps> = ({ children }) => {
  const { isAuthenticated, isLoading } = useAuth();
  const location = useLocation();

  if (isLoading) {
    return <div>Carregando...</div>; // Ou um componente de loading mais adequado
  }

  if (isAuthenticated) {
    return <>{children}</>;
  }

  // Se não estiver autenticado, redireciona para a página de login
  // com o estado 'from' para que possamos redirecionar de volta após o login
  return <Navigate to="/login" state={{ from: location }} replace />;
};

export default ProtectedRoute;