import { useState } from 'react';
import { useIngredients, IngredientDto } from '../../hooks/useIngredients';
import { useUnits } from '../../hooks/useUnits';
import SuccessToast from '../ui/SuccessToast';
import { useNavigate } from 'react-router-dom';
import ErrorToast from '../ui/ErrorToast';

export const IngredientForm = ({ initialData }: { initialData: IngredientDto }) => {
  const { update } = useIngredients();
  const { units, loading: loadingUnits } = useUnits();
  const navigate = useNavigate();

  const [name, setName] = useState(initialData.name);
  const [unitSymbol, setUnitSymbol] = useState(initialData.unitSymbol);
  const [unitPriceAmount, setUnitPriceAmount] = useState(initialData.unitPriceAmount.toString());
  const [error, setError] = useState('');

  const handleSave = async () => {
    if (!name || !unitSymbol || !unitPriceAmount) {
      setError('Preencha todos os campos.');
      return;
    }

    const unitPrice = parseFloat(unitPriceAmount);
    if (isNaN(unitPrice)) {
      setError('Preço inválido.');
      return;
    }

    try {
      await update(initialData.ingredientId, {
        name,
        unitSymbol,
        unitPriceAmount: unitPrice,
        unitPriceCurrency: 'BRL',
        id: initialData.ingredientId,
      });
      SuccessToast('Ingrediente atualizado com sucesso!');
      navigate('/ingredientes');
    } catch (err) {
      ErrorToast('Erro ao atualizar ingrediente!');
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
        <label className="block text-sm font-medium text-gray-700">Unidade</label>
        {loadingUnits ? (
          <p className="text-sm text-gray-500">Carregando unidades...</p>
        ) : (
          <select
            value={unitSymbol}
            onChange={(e) => setUnitSymbol(e.target.value)}
            className="w-full border rounded px-3 py-2 text-sm"
          >
            <option value="">Selecione...</option>
            {units.map((u) => (
              <option key={u.symbol} value={u.symbol}>
                {u.symbol} – {u.description}
              </option>
            ))}
          </select>
        )}
      </div>

      <div className="space-y-2">
        <label className="block text-sm font-medium text-gray-700">Preço Unitário (R$)</label>
        <input
          type="number"
          step="0.01"
          value={unitPriceAmount}
          onChange={(e) => setUnitPriceAmount(e.target.value)}
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