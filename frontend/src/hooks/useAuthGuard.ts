import { useEffect, useState } from "react";
import { useKeycloak } from "@react-keycloak/web";

const useAuthGuard = () => {
  const { keycloak, initialized } = useKeycloak();
  const [hasTriedLogin, setHasTriedLogin] = useState(false);

  useEffect(() => {
    if (initialized && !keycloak.authenticated && !hasTriedLogin) {
      keycloak.login();
      setHasTriedLogin(true); // evita múltiplas chamadas
    }
  }, [initialized, keycloak, hasTriedLogin]);

  return {
    isAuthenticated: !!keycloak.authenticated,
    initialized,
    keycloak,
  };
};

export default useAuthGuard;
