import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useIngredients, CreateIngredientDto } from '../../hooks/useIngredients';
import { useUnits } from '../../hooks/useUnits';

export const IngredientCreateForm = () => {
  const { create } = useIngredients();
  const navigate = useNavigate();
  const { units, loading: loadingUnits } = useUnits();

  const [name, setName] = useState('');
  const [unitSymbol, setUnitSymbol] = useState('');
  const [unitPriceAmount, setUnitPriceAmount] = useState('');
  const [error, setError] = useState('');

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

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
      const payload: CreateIngredientDto = {
        name,
        unitSymbol,
        unitPriceAmount: unitPrice,
        unitPriceCurrency: 'BRL', // Fixado como BRL, ajuste se necessário
      };
      await create(payload);
      navigate('/ingredientes');
    } catch (err) {
      console.error(err);
      setError('Erro ao criar ingrediente.');
    }
  };

  return (
    <form onSubmit={handleSubmit} className="bg-white p-6 rounded shadow space-y-4">
      {error && <p className="text-red-600 text-sm">{error}</p>}

      <div>
        <label className="block text-sm font-medium text-gray-700">Nome</label>
        <input
          value={name}
          onChange={(e) => setName(e.target.value)}
          className="w-full border px-3 py-2 rounded text-sm"
        />
      </div>

      <div>
        <label className="block text-sm font-medium text-gray-700">Unidade</label>
        {loadingUnits ? (
          <p className="text-sm text-gray-500">Carregando unidades...</p>
        ) : (
          <select
            value={unitSymbol}
            onChange={(e) => setUnitSymbol(e.target.value)}
            className="w-full border px-3 py-2 rounded text-sm"
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

      <div>
        <label className="block text-sm font-medium text-gray-700">Preço Inicial (R$)</label>
        <input
          type="number"
          step="0.01"
          value={unitPriceAmount}
          onChange={(e) => setUnitPriceAmount(e.target.value)}
          className="w-full border px-3 py-2 rounded text-sm"
        />
      </div>

      <button
        type="submit"
        className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700 text-sm"
      >
        Criar Ingrediente
      </button>
    </form>
  );
};