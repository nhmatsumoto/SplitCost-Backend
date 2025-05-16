import React from 'react';
import { useAuth } from 'react-oidc-context';
import { useUserStore } from '../../store/userStore';

const LogoutButton: React.FC = () => {
    const { signoutRedirect } = useAuth();
    const logoutUser = useUserStore((state) => state.logoutUser);

    const handleLogout = () => {
        logoutUser();
        signoutRedirect();
    };
  return <button className="" onClick={handleLogout}>Sair</button>;
};

export default LogoutButton;