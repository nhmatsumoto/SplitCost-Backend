import { useCallback } from 'react';
import { createApiClient } from '../api/client';
import { 
  CreateResidenceDto, 
  ResidenceDto, 
  UpdateResidenceDto 
} from '../types/residenceTypes';


// export interface ExpenseDto {
//   expenseId: string;
//   expenseType: string;
//   amount: number;
//   date: string;
//   IsSharedAmongMembers?: boolean;
// }

// export interface MemberDto {
//   userId: string;
//   userName: string;
//   isPrimary: boolean;
// }

// export interface AddressDto {
//   street: string;
//   number: string;
//   apartment: string;
//   city: string;
//   prefecture: string;
//   country: string;
//   postalCode: string;
// }

// export interface ResidenceDto {
//   id: string;
//   name: string;
//   createdAt: string;
//   updatedAt: string;
//   members: MemberDto[];
//   expenses: ExpenseDto[];
// }



export const useResidences = () => {

  const api = createApiClient();

  const create = useCallback(
    async (data: CreateResidenceDto) => {
      await api.post('/residences', data);
    },
    [api]
  );

  const update = useCallback(
    async (residenceId: string, data: UpdateResidenceDto) => {
      await api.put(`/residences/${residenceId}`, data);
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

  const getByUserId = useCallback(
    async (id: string): Promise<ResidenceDto | null> => {
      const response = await api.get<ResidenceDto>(`/residences/user/${id}`);
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

  return { 
    create,
    update,
    getById,
    getAll,
    remove,
    getByUserId
  };
};
