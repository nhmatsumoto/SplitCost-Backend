import { useState } from 'react';
import { MemberDto } from '../../hooks/useResidences';
import ErrorToast from '../ui/ErrorToast';

interface Props {
  members: MemberDto[];
  setMembers: (members: MemberDto[]) => void;
}

const MemberSection = ({ members, setMembers }: Props) => {
  const [newMemberName, setNewMemberName] = useState('');

  const handleAddMember = () => {
    if (!newMemberName.trim()) {
      ErrorToast('Digite o nome do membro.');
      return;
    }

    const newMember: MemberDto = {
      userId: crypto.randomUUID(),
      userName: newMemberName.trim(),
      isPrimary: false,
    };

    setMembers([...members, newMember]);
    setNewMemberName('');
  };

  return (
    <div className="bg-white shadow rounded p-4 space-y-3">
      <h2 className="text-lg font-semibold text-gray-800">Membros da Residência</h2>
      <ul className="list-disc pl-5 text-sm text-gray-800">
        {members.map((member, index) => (
          <li key={index}>
            {member.userName}{' '}
            {member.isPrimary && <span className="text-xs text-blue-600">(Proprietário)</span>}
          </li>
        ))}
      </ul>

      <div className="flex gap-2">
        <input
          type="text"
          placeholder="Nome do novo membro"
          value={newMemberName}
          onChange={(e) => setNewMemberName(e.target.value)}
          className="border rounded px-3 py-2 text-sm flex-1"
        />
        <button
          type="button"
          onClick={handleAddMember}
          className="bg-blue-600 text-white px-3 py-2 text-sm rounded hover:bg-blue-700"
        >
          Adicionar
        </button>
      </div>
    </div>
  );
};

export default MemberSection;
