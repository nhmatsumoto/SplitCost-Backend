import { ReactNode, useEffect } from "react";
import useAuthGuard from "../hooks/useAuthGuard";
import Loader from "../components/ui/Loader";
import { useLocation, useNavigate } from "react-router-dom";
import useAuthStore from "../store/authStore"; 

interface Props {
  children: ReactNode;
}

const ProtectedRoute = ({ children }: Props) => {
  const { isAuthenticated, initialized } = useAuthGuard(); 
  const location = useLocation();
  const navigate = useNavigate();

  const residenceId = useAuthStore((state) => state.userProfile?.residence?.id); // Acessa o ID do usuário do Zustand

  useEffect(() => {
    if (initialized && isAuthenticated) {
      const redirectTo = `/residence/${residenceId}`;
      if (location.pathname !== redirectTo) {
        navigate("/residence", { replace: true });
      }
    } else if (initialized && !isAuthenticated) {
     
      navigate("/SemPermissao", { state: { from: location }, replace: true });
    }
  }, [initialized, isAuthenticated, navigate, location]);

  if (!initialized) {
    return <Loader />;
  }

  if (isAuthenticated) {
    return <>{children}</>;
  }

  return null;
};

export default ProtectedRoute;