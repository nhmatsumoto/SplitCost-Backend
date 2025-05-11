import { useCallback, useMemo } from 'react';
import { useKeycloak } from '@react-keycloak/web';
import { createApiClient } from '../api/client';

export interface ResidenceMember {
  userId: string,
  userName: string,
  isPrimary: boolean
}

export interface Expense {
  expenseId: string
}

export interface ResidenceDto {
  id: string;
  name: string;
  members: ResidenceMember[]; 
  expenses: Expense[]; 
  createdAt: string;
  updatedAt: string;
}

export interface CreateResidenceDto {
  name: string;
}

export interface UpdateResidenceDto {
  residenceId: string;
  name: string;
}

export const useResidences = () => {
  const { keycloak } = useKeycloak();
  
  const api = useMemo(() => createApiClient(keycloak), [keycloak]);

  const create = useCallback(
    async (data: CreateResidenceDto) => {
      await api.post('/residences', data);
    },
    [api]
  );

  const update = useCallback(
    async (restaurantId: string, data: UpdateResidenceDto) => {
      await api.put(`/residences/${restaurantId}`, data);
    },
    [api]
  );

  const getById = useCallback(
    async (id: string): Promise<ResidenceDto | null> => {
      const response = await api.get<ResidenceDto>(`/residences/${id}`);
      return response.data;
    },
    [api]
  );

  const getAll = useCallback(
    async (): Promise<ResidenceDto[]> => {
      const response = await api.get<ResidenceDto[]>('/residences');
      return response.data;
    },
    [api]
  );

  const remove = useCallback(
    async (id: string) => {
      await api.delete(`/residences/${id}`);
    },
    [api]
  );

  return { create, update, getById, getAll, remove };
};