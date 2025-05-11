import { useState } from 'react';
import { useRestaurants, RestaurantDto } from '../../hooks/useRestaurants';
import SuccessToast from '../ui/SuccessToast';
import ErrorToast from '../ui/ErrorToast';
import { useNavigate } from 'react-router-dom';

export const RestaurantForm = ({ initialData }: { initialData: RestaurantDto }) => {
  const { update } = useRestaurants();

  const [name, setName] = useState(initialData.name);
  const [totalDishesSold, setTotalDishesSold] = useState(initialData.totalDishesSold.toString());
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const handleSave = async () => {
    if (!name) {
      setError('O nome é obrigatório.');
      return;
    }

    const dishesSold = parseInt(totalDishesSold, 10);
    if (isNaN(dishesSold)) {
      setError('Total de pratos vendidos inválido.');
      return;
    }

    try {
      await update(initialData.restaurantId, {
        restaurantId: initialData.restaurantId,
        name, 
        totalDishesSold: dishesSold,
      });
      SuccessToast('Atualizado com sucesso!');
      navigate("/restaurantes");
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

      <div className="space-y-2">
        <label className="block text-sm font-medium text-gray-700">Total de Pratos Vendidos</label>
        <input
          type="number"
          value={totalDishesSold}
          onChange={(e) => setTotalDishesSold(e.target.value)}
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