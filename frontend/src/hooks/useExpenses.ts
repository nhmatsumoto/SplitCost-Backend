import { useCallback } from "react";
import { createApiClient } from "../api/client";
import { ExpenseDto } from "../types/residenceTypes";
import { UsersDictionary } from "../types/expenseTypes";
import { Result } from "../types/commonTypes";


export interface EnumOptions {
  value: number;
  name: string;
}

export const useExpenses = () => {

  const api = createApiClient();

  const create = useCallback(
    async (data: ExpenseDto) => {
      await api.post('/expense', data);
    },
    [api]
  );

  const get = useCallback(
    async (): Promise<ExpenseDto[]> => {
      const response = await api.get('/expense');
      return response.data;
    },
    [api]
  );

  const getTypes = useCallback(
    async (): Promise<EnumOptions[]> => {
      const response = await api.get('/expense/types');
      return response.data;
    },
    [api]
  );

  const getCategories = useCallback(
    async (): Promise<EnumOptions[]> => {
      const response = await api.get('/expense/categories');
      return response.data;
    },
    [api]
  );

  //Carrega os usuários que podem pagar a despesa
  const getUsers = useCallback(
    async (): Promise<UsersDictionary> => {
    const response = await api.get<Result<UsersDictionary>>('/expense/users');

      if (!response.data.success) {
        throw new Error(response.data.error ?? 'Erro desconhecido ao buscar usuários');
      }

      return response.data.data!;
    },
    [api]
  );

  return { 
    create,
    get,
    getTypes,
    getCategories,
    getUsers
  };
};
