import { useAuth } from "react-oidc-context";
import LogoutButton from "../auth/LogoutButton";
import LoginButton from "../auth/LoginButton";
import { Link } from 'react-router-dom';
import { Coins } from 'lucide-react';

export const Header = () => {
  const { isAuthenticated, user } = useAuth();

  return (
    <header className="bg-[#F4F6F8] border-b border-[#E0E0E0] shadow-sm">
      <div className="px-6 py-4 flex justify-between items-center">
        <Link to="/" className="text-2xl font-bold text-[#2E2E2E] tracking-tight">Painel</Link>
        <div className="flex items-center gap-5">
          {isAuthenticated && (
            <Link to="/profile" className="flex items-center gap-2 text-sm text-[#2E2E2E] hover:underline">
              <Coins className="h-4 w-4" />
              {user?.profile.name}
            </Link>
          )}
          <span className="text-sm text-[#9EA7AD]">
            {isAuthenticated ? (
              <LogoutButton className="bg-red-500 text-white px-4 py-2 rounded" />
            ) : (
              <LoginButton className="bg-blue-500 text-white px-4 py-2 rounded" />
            )}
          </span>
        </div>
      </div>
    </header>
  );
};