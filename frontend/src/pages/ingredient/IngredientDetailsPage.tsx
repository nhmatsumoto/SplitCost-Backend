import { useParams, useNavigate } from 'react-router-dom';
import { useIngredients, IngredientDto } from '../../hooks/useIngredients';
import { useEffect, useState } from 'react';
import { IngredientForm } from '../../components/Ingredient/IngredientForm';
import ConfirmModal from '../../components/ui/ConfirmModal';
import Loader from '../../components/ui/Loader';


const IngredientDetailsPage = () => {
  const params = useParams();
  const id = params.id as string;

  const navigate = useNavigate();
  const { getById, remove } = useIngredients();

  const [ingredient, setIngredient] = useState<IngredientDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [showDeleteModal, setShowDeleteModal] = useState(false);

  useEffect(() => {
    const fetch = async () => {
      try {
        if (id) {
          const data = await getById(id);
          setIngredient(data);
        }
      } catch (err) {
        console.error('Erro ao carregar ingrediente:', err);
      } finally {
        setLoading(false);
      }
    };

    fetch();
  }, [id, getById]);

  const handleDelete = async () => {
    if (!id) return;
    try {
      await remove(id);
      navigate('/ingredientes');
    } catch (err) {
      console.error('Erro ao excluir ingrediente:', err);
    }
  };

  if (loading) return <Loader />;
  if (!ingredient) return <p>Ingrediente não encontrado.</p>;

  return (
    <div className="max-w-2xl mx-auto space-y-6">
      <div className="flex justify-between items-center">
        <h1 className="text-2xl font-bold">Detalhes do Ingrediente</h1>
        <button
          onClick={() => setShowDeleteModal(true)}
          className="text-sm text-red-600 hover:underline"
        >
          Deletar
        </button>
      </div>

      <IngredientForm initialData={ingredient} />

      <ConfirmModal
        isOpen={showDeleteModal}
        title="Remover ingrediente?"
        description="Essa ação é irreversível. Tem certeza que deseja excluir?"
        onCancel={() => setShowDeleteModal(false)}
        onConfirm={handleDelete}
      />
    </div>
  );
};

export default IngredientDetailsPage;