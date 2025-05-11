import axios from 'axios';
import Keycloak from 'keycloak-js';

export const createApiClient = (keycloak?: Keycloak) => {
  const api = axios.create({
    baseURL: 'https://localhost:7041/api',
  });

  api.interceptors.request.use((config) => {
    if (keycloak?.token) {
      config.headers.Authorization = `Bearer ${keycloak.token}`;
    }
    return config;
  });

  return api;
};
