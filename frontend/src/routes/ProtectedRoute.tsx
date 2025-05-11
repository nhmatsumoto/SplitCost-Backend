import { ReactNode } from "react";
import useAuthGuard from "../hooks/useAuthGuard";
import Loader from "../components/ui/Loader";

interface Props {
  children: ReactNode;
}

const ProtectedRoute = ({ children }: Props) => {
  const { isAuthenticated, initialized } = useAuthGuard();

  if (!initialized) return <Loader />;
  if (!isAuthenticated) return <div>Redirecionando para login...</div>;

  return <>{children}</>;
};

export default ProtectedRoute;
