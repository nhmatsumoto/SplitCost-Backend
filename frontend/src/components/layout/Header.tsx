import { useAuth } from "react-oidc-context";
import LogoutButton from "../auth/LogoutButton";
import LoginButton from "../auth/LoginButton";

export const Header = () => {

  const { isAuthenticated, user } = useAuth();
  
  return (
    <header className="bg-[#F4F6F8] border-b border-[#E0E0E0] shadow-sm">
      <div className="px-6 py-4 flex justify-between items-center"> {/* Removido max-w-7xl */}
        <h1 className="text-2xl font-bold text-[#2E2E2E] tracking-tight">Painel</h1>
        <div className="flex items-center gap-5 justify-end">
          <span className="text-sm text-[#9EA7AD]">
            {isAuthenticated ? (
              <div className="flex items-center gap-2">
                <span className="text-[#2E2E2E]">{user?.profile.name}</span>
                <LogoutButton />
              </div>
            ) : (
              <div className="flex items-center gap-2">
                <LoginButton />
              </div>
            )}
          </span>
        </div>
      </div>
    </header>
  );
};