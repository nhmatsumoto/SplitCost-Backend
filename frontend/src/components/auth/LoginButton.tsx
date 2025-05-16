import React from 'react';
import { useAuth } from 'react-oidc-context';

interface LoginButtonProps {
  children?: React.ReactNode;
  className?: string;
}

const LoginButton: React.FC<LoginButtonProps> = ({ children, className }) => {
  const { signinRedirect } = useAuth();

  const handleClick = () => {
    signinRedirect();
  };

  return (
    <button className={className} onClick={handleClick}>
      {children || 'Fazer Login'}
    </button>
  );
};

export default LoginButton;