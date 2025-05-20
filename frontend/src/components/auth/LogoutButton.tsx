import { useAuth } from 'react-oidc-context';
import { useUserStore } from '../../store/userStore';
import { LogOut } from 'lucide-react';

interface LogoutButtonProps {
  children?: React.ReactNode;
  className?: string;
  iconOnly?: boolean; 
}

const LogoutButton = ({ children, className, iconOnly }: LogoutButtonProps) => {
  const { signoutRedirect } = useAuth();
  const logoutUser = useUserStore((state) => state.logoutUser);

  const handleLogout = () => {
    logoutUser();
    signoutRedirect();
  };

  const baseClasses = "focus:outline-none focus:ring-2 focus:ring-indigo-500 rounded-md transition duration-150 ease-in-out";
  const combinedClasses = className ? `${baseClasses} ${className}` : baseClasses;

  return (
    <button className={combinedClasses} onClick={handleLogout}>
      {iconOnly ? <LogOut className="h-5 w-5" /> : (children || 'Sair')}
    </button>
  );
};

export default LogoutButton;