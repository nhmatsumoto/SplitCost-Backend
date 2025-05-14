import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useResidences, CreateResidenceDto } from '../../hooks/useResidences';
import { useKeycloak } from '@react-keycloak/web';

export const RestaurantCreateForm = () => {
  const { create } = useResidences();
  const navigate = useNavigate();

  const { keycloak } = useKeycloak();

  const [residenceName, setResidenceName] = useState('');
  const [error, setError] = useState('');

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!residenceName) {
      setError('O nome é obrigatório.');
      return;
    }

    try {

      if (keycloak.authenticated) {
        const tokenParsed = keycloak.tokenParsed;
        const payload: CreateResidenceDto = { residenceName, userId: tokenParsed?.sub }; // Substitua 'user-id' pelo ID do usuário atual
        await create(payload);
        navigate('/residences');
      }else {
        setError('Você não está autenticado. Faça login para criar uma residência.');
        keycloak.logout();
      }

    } catch (err) {
      setError('Erro ao criar residência.');
    }
  };

  return (
    <form onSubmit={handleSubmit} className="bg-white p-6 rounded shadow space-y-4">
      {error && <p className="text-red-600 text-sm">{error}</p>}

      <div>
        <label className="block text-sm font-medium text-gray-700">Name</label>
        <input
          value={residenceName}
          onChange={(e) => setResidenceName(e.target.value)}
          className="w-full border px-3 py-2 rounded text-sm"
        />
      </div>

      <button
        type="submit"
        className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700 text-sm"
      >
        Create
      </button>
    </form>
  );
};