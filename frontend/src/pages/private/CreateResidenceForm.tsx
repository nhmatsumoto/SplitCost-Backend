import React, { useState, useCallback } from 'react';
import { AddressDto, CreateResidenceDto, useResidences } from '../../hooks/useResidences';
import { useAuth } from 'react-oidc-context';
import SuccessToast from '../../components/ui/SuccessToast';
import ErrorToast from '../../components/ui/ErrorToast';

interface CreateResidenceFormProps {
  onSuccess?: () => void;
  onError?: (message: string) => void;
}

const CreateResidenceForm = ({ onSuccess, onError }: CreateResidenceFormProps) => {
  const [residenceName, setResidenceName] = useState('');
  const [address, setAddress] = useState<Omit<AddressDto, 'id'>>({
    street: '',
    number: '',
    apartment: '',
    city: '',
    prefecture: '',
    country: '',
    postalCode: '',
  });
  const [loading, setLoading] = useState(false);
  const { create: createResidence } = useResidences();
  const { user } = useAuth();

  const handleResidenceNameChange = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    setResidenceName(e.target.value);
  }, []);

  const handleAddressChange = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setAddress((prevAddress) => ({
      ...prevAddress,
      [name]: value,
    }));
  }, []);

  const handleSubmit = useCallback(
    async (e: React.FormEvent) => {
      e.preventDefault();
      setLoading(true);
      

      const userId = user?.profile?.sub;

      if (!userId) {
        ErrorToast('ID do usuário não encontrado. Por favor, faça login novamente.');
        setLoading(false);
        return;
      }

      const payload: CreateResidenceDto = {
        residenceName: residenceName,
        userId: userId,
        address: {
          ...address
        },
      };

      try {
        await createResidence(payload);
        SuccessToast('Residência criada com sucesso!');
        if (onSuccess) {
          onSuccess();
        }
        setResidenceName('');
        setAddress({
          street: '',
          number: '',
          apartment: '',
          city: '',
          prefecture: '',
          country: '',
          postalCode: '',
        });
      } catch (err: any) {
        const errorMessage = err?.response?.data?.message || 'Ocorreu um erro ao criar a residência.';
        ErrorToast(errorMessage);
        if (onError) {
          onError(errorMessage);
        }
      } finally {
        setLoading(false);
      }
    },
    [createResidence, residenceName, user?.profile?.sub, address, onSuccess, onError]
  );

  return (
    <div className="flex flex-col items-center justify-center bg-gray-100">
      <div className="bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4 w-full max-w-lg">
        <h2 className="block text-gray-700 text-xl font-bold mb-4">Cadastrar Residência com Endereço</h2>
        <form className="space-y-6" onSubmit={handleSubmit}>
          <div>
            <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="residenceName">
              Nome da Residência:
            </label>
            <input
              className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
              id="residenceName"
              type="text"
              name="residenceName"
              value={residenceName}
              onChange={handleResidenceNameChange}
              required
            />
          </div>

          <div>
            <h3 className="block text-gray-700 text-lg font-bold mb-2">Endereço da Residência</h3>
            <div className="space-y-4">
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
                  onChange={handleAddressChange}
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
                  onChange={handleAddressChange}
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
                  onChange={handleAddressChange}
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
                  onChange={handleAddressChange}
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
                  onChange={handleAddressChange}
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
                  onChange={handleAddressChange}
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
                  onChange={handleAddressChange}
                  required
                />
              </div>
            </div>
          </div>

          <button
            className={`bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline ${
              loading ? 'opacity-50 cursor-not-allowed' : ''
            }`}
            type="submit"
            disabled={loading}
          >
            {loading ? 'Cadastrando...' : 'Cadastrar Residência'}
          </button>
        </form>
      </div>
    </div>
  );
};

export default CreateResidenceForm;