import React from 'react';
import { useAuth } from 'react-oidc-context';
import { useUserStore } from '../../store/userStore';

const LogoutButton: React.FC = () => {
    const { signoutRedirect } = useAuth();
    const logoutUser = useUserStore((state) => state.logoutUser);

    const handleLogout = () => {
        signoutRedirect();
        // Limpa as informações do usuário do store ao fazer logout
        logoutUser();
    };
  return <button onClick={handleLogout}>Sair</button>;
};

export default LogoutButton;