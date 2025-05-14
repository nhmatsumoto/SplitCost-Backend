// components/residence/ExpenseForm.tsx
import { useState } from 'react';
import { ExpenseDto } from '../../hooks/useResidences';
import ErrorToast from '../ui/ErrorToast';

interface Props {
  expenses: ExpenseDto[];
  setExpenses: (expenses: ExpenseDto[]) => void;
}

const ExpenseForm = ({ expenses, setExpenses }: Props) => {
  const [newExpense, setNewExpense] = useState<Partial<ExpenseDto>>({});

  const handleAddExpense = () => {
    if (!newExpense.expenseType || !newExpense.amount || !newExpense.date) {
      ErrorToast('Preencha todos os campos da despesa.');
      return;
    }

    const expenseToAdd: ExpenseDto = {
      expenseId: crypto.randomUUID(),
      expenseType: newExpense.expenseType,
      amount: Number(newExpense.amount),
      date: newExpense.date,
    };

    setExpenses([...expenses, expenseToAdd]);
    setNewExpense({});
  };

  const handleRemoveExpense = (expenseId: string) => {
    setExpenses(expenses.filter((e) => e.expenseId !== expenseId));
  };

  return (
    <div className="bg-white shadow rounded p-4 space-y-3">
      <h2 className="text-lg font-semibold text-gray-800">Despesas</h2>

      {/* Inputs */}
      <div className="grid grid-cols-1 sm:grid-cols-3 gap-2">
        <select
          value={newExpense.expenseType || ''}
          onChange={(e) => setNewExpense({ ...newExpense, expenseType: e.target.value })}
          className="border rounded px-3 py-2 text-sm"
        >
          <option value="">Tipo</option>
          <option value="Fixed">Fixa</option>
          <option value="Variable">Variável</option>
          <option value="Occasional">Ocasional</option>
        </select>

        <input
          type="number"
          placeholder="Valor"
          value={newExpense.amount || ''}
          onChange={(e) => setNewExpense({ ...newExpense, amount: parseFloat(e.target.value) })}
          className="border rounded px-3 py-2 text-sm"
        />

        <input
          type="date"
          value={newExpense.date || ''}
          onChange={(e) => setNewExpense({ ...newExpense, date: e.target.value })}
          className="border rounded px-3 py-2 text-sm"
        />
      </div>

      <button
        type="button"
        onClick={handleAddExpense}
        className="bg-blue-600 text-white px-4 py-2 text-sm rounded hover:bg-blue-700"
      >
        Adicionar Despesa
      </button>

      {expenses.length > 0 && (
        <ul className="divide-y divide-gray-200 text-sm mt-3">
          {expenses.map((expense) => (
            <li key={expense.expenseId} className="py-2 flex justify-between items-center">
              <div>
                <span>{expense.expenseType}</span>
                <div className="text-xs text-gray-500">
                  {new Date(expense.date).toLocaleDateString('pt-BR')}
                </div>
              </div>
              <div className="flex items-center gap-2">
                <span className="text-gray-600">
                  {expense.amount.toLocaleString('pt-BR', {
                    style: 'currency',
                    currency: 'JPY',
                  })}
                </span>
                <button
                  onClick={() => handleRemoveExpense(expense.expenseId)}
                  className="text-xs text-red-500 hover:underline"
                >
                  Remover
                </button>
              </div>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};

export default ExpenseForm;
