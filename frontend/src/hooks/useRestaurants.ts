import { useCallback, useMemo } from 'react';
import { useKeycloak } from '@react-keycloak/web';
import { createApiClient } from '../api/client';

export interface RestaurantDto {
  restaurantId: string;
  name: string;
  totalDishesSold: number;
  indirectCosts: any[]; // Ajustar conforme necessário
  recipes: any[]; // Ajustar conforme necessário
  createdAt: string;
  updatedAt: string;
}

export interface CreateRestaurantDto {
  name: string;
}

export interface UpdateRestaurantDto {
  restaurantId: string;
  name: string;
  totalDishesSold: number;
}

export const useRestaurants = () => {
  const { keycloak } = useKeycloak();
  
  const api = useMemo(() => createApiClient(keycloak), [keycloak]);

  const create = useCallback(
    async (data: CreateRestaurantDto) => {
      await api.post('/restaurants', data);
    },
    [api]
  );

  const update = useCallback(
    async (restaurantId: string, data: UpdateRestaurantDto) => {
      await api.put(`/restaurants/${restaurantId}`, data);
    },
    [api]
  );

  const getById = useCallback(
    async (id: string): Promise<RestaurantDto | null> => {
      const response = await api.get<RestaurantDto>(`/restaurants/${id}`);
      return response.data;
    },
    [api]
  );

  const getAll = useCallback(
    async (): Promise<RestaurantDto[]> => {
      const response = await api.get<RestaurantDto[]>('/restaurants');
      return response.data;
    },
    [api]
  );

  const remove = useCallback(
    async (id: string) => {
      await api.delete(`/restaurants/${id}`);
    },
    [api]
  );

  return { create, update, getById, getAll, remove };
};