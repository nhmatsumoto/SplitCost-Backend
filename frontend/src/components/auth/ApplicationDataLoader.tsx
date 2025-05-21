import { useEffect } from 'react';
import { useAuth } from 'react-oidc-context';
import { useUserStore } from '../../store/userStore';
import { useResidences } from '../../hooks/useResidences';
import { useResidenceStore } from '../../store/residenceStore';

const ApplicationDataLoader = () => {
    const { isAuthenticated, user } = useAuth();
    const { getByUserId } = useResidences();

    const setUser = useUserStore((state) => state.setUser);
    const setResidence = useResidenceStore((state) => state.setResidence);

    useEffect(() => {
        if (isAuthenticated && user) {
            setUser(user);
            const residence = getByUserId(user.profile.sub);

            if(residence){
                setResidence(residence);
            }
            
        } else if (!isAuthenticated) {
            // Limpa o usuário no logout
            setUser(null);
        }
    }, [isAuthenticated, user, setUser]);

    return null; 
};

export default ApplicationDataLoader;