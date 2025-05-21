import React, { useState, useCallback, useEffect } from 'react';
import { useExpenses, EnumOptions } from '../../hooks/useExpenses';
import { useAuth } from 'react-oidc-context';

interface CreateExpenseFormProps {
  onSuccess?: () => void;
  onError?: (message: string) => void;
}

interface CreateExpenseDto {
  type: string;
  category: string;
  amount: number;
  date: string;
  isSharedAmongMembers: boolean;
  description?: string;
  residenceId: string;
  registerByUserId: string;
  paidByUserId: string;
}

const CreateExpenseForm = ({ onSuccess, onError }: CreateExpenseFormProps) => {
  const { create: createExpense, getTypes, getCategories } = useExpenses();
  const { user } = useAuth();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const [expenseData, setExpenseData] = useState<CreateExpenseDto>({
    type: "",
    category: "",
    amount: 0,
    date: new Date().toISOString().slice(0, 10),
    isSharedAmongMembers: true,
    description: "",
    residenceId: "",
    registerByUserId: user?.profile?.sub || "",
    paidByUserId: user?.profile?.sub || "",
  });

  const [typeOptions, setTypeOptions] = useState<EnumOptions[]>([]);
  const [categoryOptions, setCategoryOptions] = useState<EnumOptions[]>([]);

  useEffect(() => {
    getTypes().then(setTypeOptions);
    getCategories().then(setCategoryOptions);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [getTypes, getCategories]);

  // Preencher o valor inicial dos selects assim que as opções forem carregadas
  useEffect(() => {
    if (typeOptions.length && !expenseData.type) {
      setExpenseData(prev => ({ ...prev, type: String(typeOptions[0].value) }));
    }
    if (categoryOptions.length && !expenseData.category) {
      setExpenseData(prev => ({ ...prev, category: String(categoryOptions[0].value) }));
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [typeOptions, categoryOptions]);

  const handleChange = useCallback((e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    const { name, value, type } = e.target;
    const target = e.target as HTMLInputElement;
    setExpenseData(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? target.checked : value,
    }));
  }, []);

  const handleSubmit = useCallback(
    async (e: React.FormEvent) => {
      e.preventDefault();
      setLoading(true);
      setError(null);

      if (!expenseData.residenceId || !expenseData.registerByUserId || !expenseData.paidByUserId) {
        setError("Residência e usuário são obrigatórios.");
        setLoading(false);
        return;
      }

      try {
        await createExpense({
          ...expenseData,
          amount: Number(expenseData.amount),
        });
        if (onSuccess) onSuccess();
        setExpenseData({
          type: typeOptions.length ? String(typeOptions[0].value) : "",
          category: categoryOptions.length ? String(categoryOptions[0].value) : "",
          amount: 0,
          date: new Date().toISOString().slice(0, 10),
          isSharedAmongMembers: true,
          description: "",
          residenceId: "",
          registerByUserId: user?.profile?.sub || "",
          paidByUserId: user?.profile?.sub || "",
        });
      } catch (err: any) {
        setError(err?.response?.data?.message || 'Ocorreu um erro ao criar a despesa.');
        if (onError) onError(err?.response?.data?.message || 'Ocorreu um erro ao criar a despesa.');
      } finally {
        setLoading(false);
      }
    },
    [createExpense, expenseData, onSuccess, onError, user?.profile?.sub, typeOptions, categoryOptions]
  );

  return (
    <div className="flex flex-col items-center justify-center bg-gray-100">
      <div className="bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4 w-full max-w-md">
        <h2 className="block text-gray-700 text-xl font-bold mb-4">Adicionar Despesa</h2>
        {error && <p className="text-red-500 text-sm italic mb-4">{error}</p>}
        <form className="space-y-4" onSubmit={handleSubmit}>
          <div>
            <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="type">
              Tipo de Despesa:
            </label>
            <select
              id="type"
              name="type"
              value={expenseData.type}
              onChange={handleChange}
              className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
              required
              disabled={typeOptions.length === 0}
            >
              {typeOptions.map(option => (
                <option key={option.value} value={option.value}>{option.name}</option>
              ))}
            </select>
          </div>
          <div>
            <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="category">
              Categoria:
            </label>
            <select
              id="category"
              name="category"
              value={expenseData.category}
              onChange={handleChange}
              className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
              required
              disabled={categoryOptions.length === 0}
            >
              {categoryOptions.map(option => (
                <option key={option.value} value={option.value}>{option.name}</option>
              ))}
            </select>
          </div>
          <div>
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
              min={0}
              step="0.01"
            />
          </div>
          <div>
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
          <div>
            <label className="inline-flex items-center">
              <input
                type="checkbox"
                className="form-checkbox h-5 w-5 text-blue-600"
                name="isSharedAmongMembers"
                checked={expenseData.isSharedAmongMembers}
                onChange={handleChange}
              />
              <span className="ml-2 text-gray-700">Compartilhar entre membros?</span>
            </label>
          </div>
          <div>
            <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="description">
              Descrição:
            </label>
            <textarea
              id="description"
              name="description"
              value={expenseData.description}
              onChange={handleChange}
              className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
              rows={2}
            />
          </div>
          <div>
            <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="residenceId">
              Residência (ID):
            </label>
            <input
              className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
              id="residenceId"
              type="text"
              name="residenceId"
              value={expenseData.residenceId}
              onChange={handleChange}
              required
            />
          </div>
          <div>
            <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="registerByUserId">
              Usuário que registrou (ID):
            </label>
            <input
              className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
              id="registerByUserId"
              type="text"
              name="registerByUserId"
              value={expenseData.registerByUserId}
              onChange={handleChange}
              required
            />
          </div>
          <div>
            <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="paidByUserId">
              Usuário que pagou (ID):
            </label>
            <input
              className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
              id="paidByUserId"
              type="text"
              name="paidByUserId"
              value={expenseData.paidByUserId}
              onChange={handleChange}
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
            {loading ? 'Adicionando...' : 'Adicionar Despesa'}
          </button>
        </form>
      </div>
    </div>
  );
};

export default CreateExpenseForm;