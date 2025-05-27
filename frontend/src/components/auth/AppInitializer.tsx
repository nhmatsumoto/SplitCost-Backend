import { useEffect } from 'react';
import { useAuth } from 'react-oidc-context';
import { useUserStore } from '../../store/userStore';
import { useResidences } from '../../hooks/useResidences';
import { useResidenceStore } from '../../store/residenceStore';

const AppInitializer = () => {
    const { isAuthenticated, user } = useAuth();
    const { getByUserId } = useResidences();

    const setUser = useUserStore((state) => state.setUser);
    const {setResidence, residence} = useResidenceStore();

    useEffect(() => {
        if (isAuthenticated && user) {
            setUser(user);

            // verificar o que está acontecendo com o residence que está fazendo multiplos requests periodicamente 
            if(!residence){
                const residenceData =  getByUserId(user.profile.sub);
                try {
                    residenceData.then((res) => {
                        if (res) {
                            setResidence(res);
                        }
                    });
                } catch (error) {
                    console.error("Error fetching residence data:", error);
                }
            }
        } else if (!isAuthenticated) {
            setUser(null);
        }
    }, [isAuthenticated]);

    return null; 
};

export default AppInitializer;