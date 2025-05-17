import React, { useState, useCallback } from 'react';
import { CreateResidenceDto, useResidences } from '../../hooks/useResidences'; 
import { useAuth } from "react-oidc-context";
interface CreateResidenceFormProps {
  onSuccess?: () => void;
  onError?: (message: string) => void;
}

const CreateResidenceForm: React.FC<CreateResidenceFormProps> = ({ onSuccess, onError }) => {
  const [residenceName, setResidenceName] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const { create } = useResidences();
  const { user } = useAuth();

  const handleResidenceNameChange = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    setResidenceName(e.target.value);
  }, []);

  const handleSubmit = useCallback(
    async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setError(null);

        const userId = user?.profile?.sub;

        if (!userId) {
        setError('ID do usuário não encontrado. Por favor, faça login novamente.');
        setLoading(false);
        return;
        }

        const payload: CreateResidenceDto = {
        residenceName: residenceName,
        userId: userId,
        };

        try {
            await create(payload);
            console.log('Residência criada com sucesso!', payload);
            if (onSuccess) {
                onSuccess();
            }
            setResidenceName(''); // Clear residence name input
        } catch (err: any) {
            console.error('Erro ao criar residência:', err);
            setError(err?.response?.data?.message || 'Ocorreu um erro ao criar a residência.');
            if (onError) {
                onError(err?.response?.data?.message || 'Ocorreu um erro ao criar a residência.');
            }
        } finally {
            setLoading(false);
        }
    },
    [create, residenceName, user?.profile.sub, onSuccess, onError]
  );

  return (
    <div className="flex flex-col items-center justify-center bg-gray-100">
      <div className="bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4 w-full max-w-md">
        <h2 className="block text-gray-700 text-xl font-bold mb-4">Cadastrar Residência</h2>
        {error && <p className="text-red-500 text-sm italic mb-4">{error}</p>}
        <form className="space-y-4" onSubmit={handleSubmit}>
          <div className="mb-4">
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