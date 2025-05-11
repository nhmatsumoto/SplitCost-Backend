import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useResidences, CreateResidenceDto } from '../../hooks/useResidences';

export const RestaurantCreateForm = () => {
  const { create } = useResidences();
  const navigate = useNavigate();

  const [name, setName] = useState('');
  const [error, setError] = useState('');

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!name) {
      setError('O nome é obrigatório.');
      return;
    }

    try {
      const payload: CreateResidenceDto = { name };
      await create(payload);
      navigate('/residences');
    } catch (err) {
      console.error(err);
      setError('Erro ao criar residência.');
    }
  };

  return (
    <form onSubmit={handleSubmit} className="bg-white p-6 rounded shadow space-y-4">
      {error && <p className="text-red-600 text-sm">{error}</p>}

      <div>
        <label className="block text-sm font-medium text-gray-700">Name</label>
        <input
          value={name}
          onChange={(e) => setName(e.target.value)}
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