import { useParams, useNavigate } from 'react-router-dom';
import { useResidences, ResidenceDto } from '../../../hooks/useResidences';
import { useEffect, useState } from 'react';
import { ResidenceForm } from '../../../components/residence/ResidenceForm';
import ConfirmModal from '../../../components/ui/ConfirmModal';
import Loader from '../../../components/ui/Loader';
import ExpenseList from '../../../components/expenses/ExpenseList';

const ResidenceDetailsPage = () => {
  const { residenceId } = useParams<{ residenceId: string }>();
  const navigate = useNavigate();
  const { getById, remove } = useResidences();

  const [residence, setResidence] = useState<ResidenceDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [showDeleteModal, setShowDeleteModal] = useState(false);

  useEffect(() => {
    const fetchResidence = async () => {
      try {
        if (!residenceId) throw new Error('ID da residência não fornecido.');
        const data = await getById(residenceId);
        if (!data) throw new Error('Residência não encontrada.');
        setResidence(data);
      } catch (err) {
        console.error('Erro ao carregar residência:', err);
        setResidence(null);
      } finally {
        setLoading(false);
      }
    };

    fetchResidence();
  }, [residenceId, getById]);

  const handleDelete = async () => {
    if (!residenceId) return;
    try {
      await remove(residenceId);
      navigate('/residencias');
    } catch (err) {
      console.error('Erro ao excluir residência:', err);
    }
  };

  if (loading) return <Loader />;
  if (!residence) return <p>Residência não encontrada.</p>;

  return (
    <div className="max-w-7xl mx-auto space-y-6">

      <div className="flex justify-between items-center">
        <h1 className="text-2xl font-bold">Detalhes da Residência</h1>
        <button
          onClick={() => setShowDeleteModal(true)}
          className="text-sm text-red-600 hover:underline"
        >
          Deletar
        </button>
      </div>

      <div className="flex flex-col lg:flex-row gap-6">
        {/* Formulário */}
        <div className="w-full lg:w-1/2">
          <ResidenceForm initialData={residence} />
        </div>

        <div className="w-full lg:w-1/2">
          <ExpenseList expenses={residence.expenses} />
        </div>
      </div>

      <ConfirmModal
        isOpen={showDeleteModal}
        title="Remover residência?"
        description="Essa ação é irreversível. Tem certeza que deseja excluir?"
        onCancel={() => setShowDeleteModal(false)}
        onConfirm={handleDelete}
      />
    </div>
  );
};

export default ResidenceDetailsPage;
