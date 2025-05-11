import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { RecipeDto, useRecipes } from '../../hooks/useRecipes';
import { RecipeList } from '../../components/recipe/RecipeList';

const RecipesPage = () => {
  const { getAll } = useRecipes();
  const navigate = useNavigate();

  const [recipes, setRecipes] = useState<RecipeDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchRecipes = async () => {
      try {
        const data = await getAll();
        setRecipes(data);
      } catch (error) {
        console.error('Erro ao buscar receitas:', error);
        setError('Não foi possível carregar as receitas. Tente novamente.');
      } finally {
        setLoading(false);
      }
    };

    fetchRecipes();
  }, [getAll]);

  if (loading) {
    return (
      <div className="flex justify-center items-center h-64">
        <p className="text-sm text-[#9EA7AD]">Carregando receitas...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="flex justify-center items-center h-64">
        <p className="text-sm text-[#FF6B6B]">{error}</p>
      </div>
    );
  }

  return (
    <div className="flex flex-col gap-6 p-6">
      {/* Header da Página */}
      <div className="flex flex-wrap items-center justify-between gap-4">
        <h1 className="text-3xl font-bold text-[#2E2E2E] tracking-tight">Receitas</h1>
        <button
          onClick={() => navigate('/receitas/nova')}
          className="bg-[#00796B] hover:bg-[#005B4D] text-white px-5 py-2 rounded-lg text-sm font-medium transition-colors duration-200"
        >
          + Nova Receita
        </button>
      </div>

      {/* Lista de Receitas */}
      <RecipeList recipes={recipes} />
    </div>
  );
};

export default RecipesPage;