import { useCallback, useMemo } from 'react';
import { useKeycloak } from '@react-keycloak/web';
import { createApiClient } from '../api/client';

export interface IngredientDto {
  ingredientId: string;
  name: string;
  unitPriceAmount: number;
  unitPriceCurrency: string;
  unitSymbol: string;
  createdAt: string;
  updatedAt: string;
}

export interface CreateIngredientDto {
  name: string;
  unitPriceAmount: number;
  unitPriceCurrency: string;
  unitSymbol: string;
}

export interface UpdateIngredientDto {
  id: string;
  name: string;
  unitPriceAmount: number;
  unitPriceCurrency: string;
  unitSymbol: string;
}

export const useIngredients = () => {
  const { keycloak } = useKeycloak();
  const api = useMemo(() => createApiClient(keycloak), [keycloak]);

  const create = useCallback(
    async (data: CreateIngredientDto) => {
      await api.post('/ingredients', data);
    },
    [api]
  );

  const update = useCallback(
    async (ingredientId: string, data: UpdateIngredientDto) => {
      await api.put(`/ingredients/${ingredientId}`, data);
    },
    [api]
  );

  const getById = useCallback(
    async (id: string): Promise<IngredientDto | null> => {
      try {
        const response = await api.get<IngredientDto>(`/ingredients/${id}`);
        return response.data;
      } catch (err) {
        console.error('Erro ao buscar ingrediente:', err);
        return null;
      }
    },
    [api]
  );

  const getAll = useCallback(
    async (): Promise<IngredientDto[]> => {
      try {
        const response = await api.get<IngredientDto[]>('/ingredients');
        return response.data;
      } catch (err) {
        console.error('Erro ao buscar ingredientes:', err);
        return [];
      }
    },
    [api]
  );

  const remove = useCallback(
    async (id: string) => {
      await api.delete(`/ingredients/${id}`);
    },
    [api]
  );

  return { create, update, getById, getAll, remove };
};