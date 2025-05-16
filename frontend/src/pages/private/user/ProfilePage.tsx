import { useAuth } from 'react-oidc-context';

const ProfilePage = () => {
  const { isAuthenticated, user } = useAuth();

  if (!isAuthenticated || !user) {
    return <p>Utilizador não autenticado.</p>;
  }

  return (
    <div>
      <h2>Perfil do Utilizador</h2>
      <pre>{JSON.stringify(user.profile, null, 2)}</pre>
    </div>
  );
};

export default ProfilePage;