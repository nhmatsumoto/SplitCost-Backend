import { useState } from 'react';
import { useResidences, ResidenceDto, ExpenseDto, MemberDto } from '../../hooks/useResidences';
import SuccessToast from '../ui/SuccessToast';
import ErrorToast from '../ui/ErrorToast';
import { useNavigate } from 'react-router-dom';
import MemberSection from './MemberSection';
import ExpenseForm from '../expenses/ExpenseForm';

export const ResidenceForm = ({ initialData }: { initialData: ResidenceDto }) => {
  const { update } = useResidences();
  const navigate = useNavigate();

  const [name, setName] = useState(initialData.name);
  const [expenses, setExpenses] = useState<ExpenseDto[]>(initialData.expenses || []);
  const [members, setMembers] = useState<MemberDto[]>(initialData.members || []);
  const [error, setError] = useState('');

  const handleSave = async () => {
    if (!name) {
      setError('O nome é obrigatório.');
      return;
    }

    try {
      await update(initialData.id, {
        residenceId: initialData.id,
        name,
        expenses,
        members,
      });
      SuccessToast('Atualizado com sucesso!');
      navigate('/residences');
    } catch (err) {
      ErrorToast('Erro ao atualizar residência!');
    }
  };

  return (
    <div className="space-y-6">
      {error && <p className="text-red-600 text-sm">{error}</p>}
      <div className="bg-white shadow rounded p-4 space-y-3">

        <div className="bg-white shadow rounded p-4 space-y-3">
          <h2 className="text-lg font-semibold text-gray-800">Informações da Residência</h2>
          <label className="block text-sm font-medium text-gray-700">Nome</label>
          <input
            value={name}
            onChange={(e) => setName(e.target.value)}
            className="w-full border rounded px-3 py-2 text-sm"
          />
        </div>
        
        <MemberSection members={members} setMembers={setMembers} />

        <ExpenseForm expenses={expenses} setExpenses={setExpenses} />

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
