import { useAuth } from "react-oidc-context";
import LogoutButton from "../../components/auth/LogoutButton";

const HomePage = () => {

  const { isAuthenticated } = useAuth();

  return (
    <div className="min-h-screen bg-gray-100 py-8 px-4">
      {isAuthenticated && (
        <div className="mb-4">
          <h1 className="text-3xl font-bold mb-4">Bem-vindo à nossa aplicação!</h1>
          <LogoutButton />
        </div>
      )}
    </div>
  );
};

export default HomePage;
