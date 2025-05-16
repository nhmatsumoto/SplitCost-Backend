import { useEffect } from 'react';
import { useAuth } from 'react-oidc-context';
import { useUserStore } from '../../store/userStore';

const UserDataLoader = () => {
    const { isAuthenticated, user } = useAuth();
    const setUser = useUserStore((state) => state.setUser);

    useEffect(() => {
        if (isAuthenticated && user) {
            setUser(user);
            alert(`Usuário autenticado: ${user.profile.name}`);
        } else if (!isAuthenticated) {
            // Limpa o usuário no logout
            setUser(null);
        }
    }, [isAuthenticated, user, setUser]);

    return null; 
};

export default UserDataLoader;