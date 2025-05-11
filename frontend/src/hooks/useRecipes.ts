import { useCallback, useMemo } from 'react';
import { useKeycloak } from '@react-keycloak/web';
import { createApiClient } from '../api/client';

export interface RecipeDto {
  recipeId: string;
  name: string;
  profitMargin: number;
  restaurantId: string;
  ingredients: RecipeIngredientDto[];
  createdAt: string;
  updatedAt: string;
}

export interface RecipeIngredientDto {
  id: string;
  ingredientId: string;
  quantityUsed: number;
  quantityUnitSymbol: string;
  unitCostAmount: number;
  unitCostCurrency: string;
}

export interface CreateRecipeDto {
  name: string;
  profitMargin: number;
  restaurantId: string;
  ingredients: { ingredientId: string; quantityUsed: number; quantityUnitSymbol: string }[];
}

export interface UpdateRecipeDto {
  id: string;
  name: string;
  profitMargin: number;
  restaurantId: string;
  ingredients: { ingredientId: string; quantityUsed: number; quantityUnitSymbol: string }[];
}

export const useRecipes = () => {
  const { keycloak } = useKeycloak();
  const api = useMemo(() => createApiClient(keycloak), [keycloak]);

  const create = useCallback(
    async (data: CreateRecipeDto) => {
      await api.post('/recipes', data);
    },
    [api]
  );

  const update = useCallback(
    async (id: string, data: UpdateRecipeDto) => {
      await api.put(`/recipes/${id}`, data);
    },
    [api]
  );

  const getById = useCallback(
    async (recipeId: string): Promise<RecipeDto | null> => {
      try {
        const response = await api.get<RecipeDto>(`/recipes/${recipeId}`);
        return response.data;
      } catch (err) {
        console.error('Erro ao buscar receita:', err);
        return null;
      }
    },
    [api]
  );

  const getAll = useCallback(
    async (): Promise<RecipeDto[]> => {
      try {
        const response = await api.get<RecipeDto[]>('/recipes');
        return response.data;
      } catch (err) {
        console.error('Erro ao buscar receitas:', err);
        return [];
      }
    },
    [api]
  );

  const remove = useCallback(
    async (id: string) => {
      await api.delete(`/recipes/${id}`);
    },
    [api]
  );

  return { create, update, getById, getAll, remove };
};