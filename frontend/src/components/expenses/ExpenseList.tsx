import { ExpenseDto } from '../../hooks/useResidences';

interface ExpenseListProps {
  expenses?: ExpenseDto[];
}

const ExpenseList: React.FC<ExpenseListProps> = ({ expenses }) => {
  return (
    <div className="bg-white shadow rounded p-4 space-y-3">
      <h2 className="text-lg font-semibold">Despesas</h2>
      {expenses && expenses.length > 0 ? (
        <ul className="divide-y divide-gray-200">
          {expenses.map((expense, index) => (
            <li key={expense.expenseId ?? index} className="py-2">
              <div className="flex justify-between text-sm">
                <span className="font-medium">{expense.expenseType}</span>
                <span className="text-gray-600">
                  {Number(expense.amount).toLocaleString('pt-BR', {
                    style: 'currency',
                    currency: 'JPY',
                  })}
                </span>
              </div>
              <div className="text-xs text-gray-500">
                {new Date(expense.date).toLocaleDateString('pt-BR')}
              </div>
            </li>
          ))}
        </ul>
      ) : (
        <p className="text-sm text-gray-600">Nenhuma despesa registrada.</p>
      )}
    </div>
  );
};

export default ExpenseList;
