import React from 'react';
import { useAuth } from 'react-oidc-context';
import { LogIn } from 'lucide-react';

interface LoginButtonProps {
  children?: React.ReactNode;
  className?: string;
  iconOnly?: boolean; 
}

const LoginButton = ({ children, className, iconOnly }: LoginButtonProps) => {
  const { signinRedirect } = useAuth();

  const handleClick = () => {
    signinRedirect();
  };

  const baseClasses = "focus:outline-none focus:ring-2 focus:ring-blue-500 rounded-md transition duration-150 ease-in-out";
  const combinedClasses = className ? `${baseClasses} ${className}` : baseClasses;

  return (
    <button className={combinedClasses} onClick={handleClick}>
      {iconOnly ? <LogIn className="h-5 w-5" /> : (children || 'Fazer Login')}
    </button>
  );
};

export default LoginButton;