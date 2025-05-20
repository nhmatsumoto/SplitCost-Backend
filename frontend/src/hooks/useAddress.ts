import { useCallback } from "react";
import { createApiClient } from "../api/client";

export interface AddressDto {
  id: number;
  street: string;
  number: string;
  apartment: string;
  city: string;
  prefecture: string;
  country: string;
  postalCode: string;
}

export const useAddress = () => {

  const api = createApiClient();

  const create = useCallback(
    async (data: AddressDto) => {
      await api.post('/address', data);
    },
    [api]
  );

  return { 
    create,
  };
};
