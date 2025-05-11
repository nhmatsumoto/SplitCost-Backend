import { useState } from 'react';
import { useResidences, ResidenceDto } from '../../hooks/useResidences'
import SuccessToast from '../ui/SuccessToast';
import ErrorToast from '../ui/ErrorToast';
import { useNavigate } from 'react-router-dom';

export const RestaurantForm = ({ initialData }: { initialData: ResidenceDto }) => {
  const { update } = useResidences();

  const [name, setName] = useState(initialData.name);
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const handleSave = async () => {
    if (!name) {
      setError('O nome é obrigatório.');
      return;
    }

    try {
      await update(initialData.id, {
        residenceId: initialData.id,
        name
      });
      SuccessToast('Atualizado com sucesso!');
      navigate("/residences");
    } catch (err) {
      ErrorToast('Erro ao atualizar restaurante!');
    }
    
  };

  return (
    <div className="bg-white rounded-xl p-4 shadow space-y-4">
      {error && <p className="text-red-600 text-sm">{error}</p>}

      <div className="space-y-2">
        <label className="block text-sm font-medium text-gray-700">Nome</label>
        <input
          value={name}
          onChange={(e) => setName(e.target.value)}
          className="w-full border rounded px-3 py-2 text-sm"
        />
      </div>

      <div className="pt-2">
        <button
          onClick={handleSave}
          className="bg-green-600 text-white px-4 py-2 text-sm rounded hover:bg-green-700"
        >
          Salvar Alterações
        </button>
      </div>
    </div>
  );
};