// import { useCallback, useMemo } from 'react';
// import { useKeycloak } from '@react-keycloak/web';
// import { createApiClient } from '../api/client';

// export interface Members {
//   userId: string;
//   userName: string;
//   isPrimary: boolean;
// }

// export interface ExpenseDto {
//   expenseId: string;
//   expenseType: string;
//   amount: number;
//   date: string;
//   isShared?: boolean; // se ainda quiser usar futuramente
// }

// export interface MemberDto {
//   userId: string;
//   userName: string;
//   isPrimary: boolean;
// }

// export interface ResidenceDto {
//   id: string;
//   name: string;
//   createdAt: string;
//   updatedAt: string;
//   members: MemberDto[];
//   expenses: ExpenseDto[];
// }

// export interface CreateResidenceDto {
//   residenceName: string;
//   userId: string | undefined;
//   members?: Members[] | null;
// }

// export interface UpdateResidenceDto {
//   residenceId: string;
//   name: string;
//   expenses: ExpenseDto[];
//   members?: Members[]; 
// }

// export const useResidences = () => {
//   const { keycloak } = useKeycloak();
//   const api = useMemo(() => createApiClient(keycloak), [keycloak]);

//   const create = useCallback(
//     async (data: CreateResidenceDto) => {
//       await api.post('/residences', data);
//     },
//     [api]
//   );

//   const update = useCallback(
//     async (residenceId: string, data: UpdateResidenceDto) => {
//       await api.put(`/residences/${residenceId}`, data);
//     },
//     [api]
//   );

//   const getById = useCallback(
//     async (id: string): Promise<ResidenceDto | null> => {
//       const response = await api.get<ResidenceDto>(`/residences/${id}`);
//       return response.data;
//     },
//     [api]
//   );

//   const getByUserId = useCallback(
//     async (id: string): Promise<ResidenceDto | null> => {
//       const response = await api.get<ResidenceDto>(`/residences/user/${id}`);
//       return response.data;
//     },
//     [api]
//   );

//   const getAll = useCallback(
//     async (): Promise<ResidenceDto[]> => {
//       const response = await api.get<ResidenceDto[]>('/residences');
//       return response.data;
//     },
//     [api]
//   );

//   const remove = useCallback(
//     async (id: string) => {
//       await api.delete(`/residences/${id}`);
//     },
//     [api]
//   );

//   return { 
//     create,
//     update,
//     getById,
//     getAll,
//     remove,
//     getByUserId
//   };
// };
