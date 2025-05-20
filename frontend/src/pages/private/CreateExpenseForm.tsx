import React, { useState, useCallback } from 'react';
import { ExpenseDto, useExpenses } from '../../hooks/useExpenses';

interface CreateExpenseFormProps {
  onSuccess?: () => void;
  onError?: (message: string) => void;
}

const CreateExpenseForm = ({ onSuccess, onError }: CreateExpenseFormProps) => {
  const [expenseData, setExpenseData] = useState<Omit<ExpenseDto, 'expenseId'>>({
    expenseType: '',
    amount: 0,
    date: new Date().toISOString().slice(0, 10), 
    IsSharedAmongMembers: true, 
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const { create: createExpense } = useExpenses();

  const handleChange = useCallback((e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value, type } = e.target;
    const target = e.target as HTMLInputElement; 
    setExpenseData((prevData) => ({
        ...prevData,
        [name]: type === 'checkbox' ? target.checked : value,
    }));
  }, []);

  const handleSubmit = useCallback(
    async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setError(null);
        try {
            await createExpense({
            ...expenseData,
            date: expenseData.date, // Ensure date is included
            } as ExpenseDto); // Casting to ExpenseDto as the hook expects the full DTO
            console.log('Despesa criada com sucesso!', expenseData);
            if (onSuccess) {
            onSuccess();
            }
            setExpenseData({
            expenseType: '',
            amount: 0,
            date: new Date().toISOString().slice(0, 10),
            IsSharedAmongMembers: true,
            }); // Clear form
        } catch (err: any) {
            console.error('Erro ao criar despesa:', err);
            setError(err?.response?.data?.message || 'Ocorreu um erro ao criar a despesa.');
            if (onError) {
            onError(err?.response?.data?.message || 'Ocorreu um erro ao criar a despesa.');
            }
        } finally {
            setLoading(false);
        }
        },
        [createExpense, expenseData, onSuccess, onError]
  );

  return (
    <div className="flex flex-col items-center justify-center bg-gray-100">
      <div className="bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4 w-full max-w-md">
        <h2 className="block text-gray-700 text-xl font-bold mb-4">Adicionar Despesa</h2>
        {error && <p className="text-red-500 text-sm italic mb-4">{error}</p>}
        <form className="space-y-4" onSubmit={handleSubmit}>
          <div className="mb-4">
            <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="expenseType">
              Tipo de Despesa:
            </label>
            <input
              className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
              id="expenseType"
              type="text"
              name="expenseType"
              value={expenseData.expenseType}
              onChange={handleChange}
              required
            />
          </div>
          <div className="mb-4">
            <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="amount">
              Valor:
            </label>
            <input
              className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
              id="amount"
              type="number"
              name="amount"
              value={expenseData.amount}
              onChange={handleChange}
              required
            />
          </div>
          <div className="mb-4">
            <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="date">
              Data:
            </label>
            <input
              className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
              id="date"
              type="date"
              name="date"
              value={expenseData.date}
              onChange={handleChange}
              required
            />
          </div>
          <div className="mb-6">
            <label className="inline-flex items-center">
              <input
                type="checkbox"
                className="form-checkbox h-5 w-5 text-blue-600"
                name="IsSharedAmongMembers"
                checked={expenseData.IsSharedAmongMembers}
                onChange={handleChange}
              />
              <span className="ml-2 text-gray-700">Compartilhar entre membros?</span>
            </label>
          </div>
          <button
            className={`bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline ${
              loading ? 'opacity-50 cursor-not-allowed' : ''
            }`}
            type="submit"
            disabled={loading}
          >
            {loading ? 'Adicionando...' : 'Adicionar Despesa'}
          </button>
        </form>
      </div>
    </div>
  );
};

export default CreateExpenseForm;