import { useParams, useNavigate } from 'react-router-dom';
import { CreateRecipeDto, RecipeDto, useRecipes } from '../../hooks/useRecipes';
import { useEffect, useState } from 'react';
import { RecipeForm } from '../../components/recipe/recipeForm';
import ConfirmModal from '../../components/ui/ConfirmModal';
import ErrorToast from '../../components/ui/ErrorToast';

const RecipeDetailsPage = () => {

  const params = useParams();
  const recipeId = params.id as string;

  const navigate = useNavigate();
  const { getById, update, remove } = useRecipes();

  const [recipe, setRecipe] = useState<RecipeDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showDeleteModal, setShowDeleteModal] = useState(false);

  useEffect(() => {
    const fetchRecipe = async () => {
      try {
        if (!recipeId) {
          ErrorToast("ID da receita não fornecido.");
          setError("ID da receita não fornecido.");
          setLoading(false);
          return;
        }
        const data = await getById(recipeId);
        if (data) {
          setRecipe(data);
        } else {
          ErrorToast("Receita não encontrada.");
        }
      } catch (err) {
        ErrorToast("Não foi possível carregar a receita.");
      } finally {
        setLoading(false);
      }
    };

    fetchRecipe();
  }, [recipeId, getById]);

  const handleSubmit = async (data: CreateRecipeDto) => {
    try {
      if (!recipeId) return;

      await update(recipeId, {
        id: recipeId,
        name: data.name,
        profitMargin: data.profitMargin,
        restaurantId: data.restaurantId,
        ingredients: data.ingredients,
      });
      navigate('/receitas');
    } catch (err) {
      ErrorToast("Não foi possível atualizar a receita. Verifique os dados e tente novamente.");
    }
  };

  const handleDelete = async () => {
    try {
      if (!recipeId) return;
      await remove(recipeId);
      navigate('/receitas');
    } catch (err) {
      ErrorToast("Não foi possível excluir a receita.");
    }
  };

  if (loading) {
    return (
      <div className="flex justify-center items-center h-64">
        <p className="text-gray-600">Carregando receita...</p>
      </div>
    );
  }

  if (error || !recipe) {
    return (
      <div className="flex justify-center items-center h-64">
        <p className="text-red-600">{error || 'Receita não encontrada.'}</p>
      </div>
    );
  }

  return (
    <div className="max-w-2xl mx-auto p-6">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold text-gray-800">Detalhes da Receita</h1>
        <button
          onClick={() => setShowDeleteModal(true)}
          className="text-sm text-red-600 hover:underline"
        >
          Deletar
        </button>
      </div>
      {error && (
        <div className="mb-4 p-4 bg-red-100 text-red-700 rounded-lg">
          {error}
        </div>
      )}
      <RecipeForm initialData={recipe} onSubmit={handleSubmit} />

      <ConfirmModal
        isOpen={showDeleteModal}
        title="Remover receita?"
        description="Essa ação é irreversível. Tem certeza que deseja excluir?"
        onCancel={() => setShowDeleteModal(false)}
        onConfirm={handleDelete}
      />
    </div>
  );
};

export default RecipeDetailsPage;