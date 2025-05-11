import { useKeycloak } from '@react-keycloak/web';
import { createApiClient } from '../api/client';
import { useEffect, useState } from 'react';

export interface Unit {
  symbol: string;
  description: string;
}

export const useUnits = () => {
  const { keycloak } = useKeycloak();
  const api = createApiClient(keycloak);

  const [units, setUnits] = useState<Unit[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchUnits = async () => {
      try {
        const response = await api.get<Unit[]>('/units');
        setUnits(response.data);
      } catch (err) {
        console.error('Erro ao buscar unidades:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchUnits();
  }, []);

  return { units, loading };
};
