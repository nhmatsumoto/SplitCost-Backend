import { ReactNode } from "react";
import useAuthGuard from "../hooks/useAuthGuard";
import Loader from "../components/ui/Loader";

interface Props {
  children: ReactNode;
}

const ProtectedRoute = ({ children }: Props) => {
  const { isAuthenticated, initialized } = useAuthGuard();

  if (!initialized) return <Loader />;
  if (!isAuthenticated) return (<section className="flex flex-col gap-4 items-center my-8">
      <div className="w-16 h-16 border-l-2 border-t-2 border-blue-500 font-bold animate-spin rounded-full"></div>
      <span className="bg-gradient-to-r from-blue-500 to-slate-300 bg-clip-text text-transparent animate-teleport">
        <div>Redirecionando para login...</div>
      </span>
  </section>);

  return <>{children}</>;
};

export default ProtectedRoute;
