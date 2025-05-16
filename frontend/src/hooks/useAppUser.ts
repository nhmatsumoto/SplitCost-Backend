import { useCallback } from 'react';
import { createApiClient } from '../api/client';

export interface RegisterUserDto {
  Username: string;
  FirstName: string;
  LastName: string;
  Email: string;
  Password?: string;
  ConfirmPassword?: string;
}

export const useAppUser = () => {

  const api = createApiClient();

  const create = useCallback(
    async (data: RegisterUserDto) => {
      await api.post('/users/register', data);
    },
    [api]
  );

  return { 
    create
  };
};
