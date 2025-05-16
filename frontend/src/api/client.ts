import axios from 'axios';
import { useAuth } from 'react-oidc-context';

export const createApiClient = () => {

  const { isAuthenticated, user } = useAuth();

  const api = axios.create({
    baseURL: 'https://localhost:7041/api',
  });

  api.interceptors.request.use((config) => {
    if (isAuthenticated && user) {
      config.headers.Authorization = `Bearer ${user.refresh_token}`;
    }
    return config;
  });

  return api;
};
