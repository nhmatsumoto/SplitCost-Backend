import { useKeycloak } from '@react-keycloak/web';
import { createApiClient } from '../api/client';
import { useCallback } from 'react';

export interface StockItem {
  id: string;
  ingredient: {
    id: string;
    name: string;
    unit: string;
  };
  quantity: number;
}

export const useStock = () => {
  const { keycloak } = useKeycloak();
  const api = createApiClient(keycloak);

  const getAll = useCallback(async (): Promise<StockItem[]> => {
    const response = await api.get<StockItem[]>('/stock');
    return response.data;
  }, [api]);

  return { getAll };
};
