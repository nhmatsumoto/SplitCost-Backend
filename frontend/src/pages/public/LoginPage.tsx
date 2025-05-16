import { useAuth } from 'react-oidc-context';

const LoginPage  = () => {
  const { signinRedirect } = useAuth();

  const handleLogin = () => {
    signinRedirect();
  };

  return (
    <div>
      <h1>Faça Login</h1>
      <button onClick={handleLogin}>Entrar com Keycloak</button>
    </div>
  );
};

export default LoginPage;