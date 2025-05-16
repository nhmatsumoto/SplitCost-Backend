import { useCallback, useMemo } from 'react';
import { useKeycloak } from '@react-keycloak/web';
import { createApiClient } from '../api/client';

export interface UserProfile {
  id?: string;
  username?: string;
  email?: string;
  firstName?: string;
  lastName?: string;
}

export interface UserRoles {
  isAdmin?: boolean;
  isOwner?: boolean;
  isMember?: boolean;
}

export interface RegisterUserDto {
  name: string;
  email: string;
  password: string;
}

export const useUser = () => {
  const { keycloak } = useKeycloak();
  const api = useMemo(() => createApiClient(keycloak), [keycloak]);

  const register = useCallback(
    async (data: RegisterUserDto): Promise<void> => {
      await api.post('/users/register', data);
    },
    [api]
  );

  const getUserProfile = useCallback(
    async (): Promise<UserProfile | undefined> => {
      if (keycloak.authenticated) {
        // Assuming Keycloak's tokenParsed contains user profile info
        return {
          id: keycloak.tokenParsed?.sub,
          username: keycloak.tokenParsed?.preferred_username,
          email: keycloak.tokenParsed?.email,
          firstName: keycloak.tokenParsed?.given_name,
          lastName: keycloak.tokenParsed?.family_name,
        };
      }
      return undefined;
    },
    [keycloak]
  );

  // Example of fetching user roles from your backend (if needed)
  // const getUserRoles = useCallback(
  //   async (userId: string): Promise<UserRoles> => {
  //     const response = await api.get<UserRoles>(`/users/${userId}/roles`);
  //     return response.data;
  //   },
  //   [api]
  // );

  return {
    register,
    getUserProfile,
    // getUserRoles,
  };
};