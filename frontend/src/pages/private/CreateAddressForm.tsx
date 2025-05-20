import React, { useState, useCallback } from 'react';
import { AddressDto } from '../../hooks/useAddress';

interface AddressFormProps {
  initialValues?: Partial<AddressDto>;
  onSubmit: (address: Omit<AddressDto, 'id'>) => void;
  onCancel?: () => void;
}

const CreateAddressForm = ({ initialValues, onSubmit, onCancel }: AddressFormProps) => {
  const [address, setAddress] = useState<Omit<AddressDto, 'id'>>({
    street: initialValues?.street || '',
    number: initialValues?.number || '',
    apartment: initialValues?.apartment || '',
    city: initialValues?.city || '',
    prefecture: initialValues?.prefecture || '',
    country: initialValues?.country || '',
    postalCode: initialValues?.postalCode || '',
  });

  const handleChange = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setAddress((prevAddress) => ({
      ...prevAddress,
      [name]: value,
    }));
  }, []);

  const handleSubmit = useCallback(
    (e: React.FormEvent) => {
      e.preventDefault();
      onSubmit(address);
    },
    [onSubmit, address]
  );

  return (
    <div className="bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4 w-full max-w-md">
      <h2 className="block text-gray-700 text-xl font-bold mb-4">
        {initialValues?.id ? 'Editar Endereço' : 'Adicionar Endereço'}
      </h2>
      <form className="space-y-4" onSubmit={handleSubmit}>
        <div>
          <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="street">
            Rua:
          </label>
          <input
            className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            id="street"
            type="text"
            name="street"
            value={address.street}
            onChange={handleChange}
            required
          />
        </div>
        <div>
          <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="number">
            Número:
          </label>
          <input
            className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            id="number"
            type="text"
            name="number"
            value={address.number}
            onChange={handleChange}
            required
          />
        </div>
        <div>
          <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="apartment">
            Apartamento/Complemento:
          </label>
          <input
            className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            id="apartment"
            type="text"
            name="apartment"
            value={address.apartment}
            onChange={handleChange}
          />
        </div>
        <div>
          <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="city">
            Cidade:
          </label>
          <input
            className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            id="city"
            type="text"
            name="city"
            value={address.city}
            onChange={handleChange}
            required
          />
        </div>
        <div>
          <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="prefecture">
            Estado/Província/Prefeitura:
          </label>
          <input
            className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            id="prefecture"
            type="text"
            name="prefecture"
            value={address.prefecture}
            onChange={handleChange}
          />
        </div>
        <div>
          <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="country">
            País:
          </label>
          <input
            className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            id="country"
            type="text"
            name="country"
            value={address.country}
            onChange={handleChange}
            required
          />
        </div>
        <div>
          <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="postalCode">
            CEP/Código Postal:
          </label>
          <input
            className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            id="postalCode"
            type="text"
            name="postalCode"
            value={address.postalCode}
            onChange={handleChange}
            required
          />
        </div>
        <div className="flex items-center justify-between">
          {onCancel && (
            <button
              className="bg-gray-300 hover:bg-gray-400 text-gray-800 font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
              type="button"
              onClick={onCancel}
            >
              Cancelar
            </button>
          )}
          <button
            className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
            type="submit"
          >
            Salvar Endereço
          </button>
        </div>
      </form>
    </div>
  );
};

export default CreateAddressForm;