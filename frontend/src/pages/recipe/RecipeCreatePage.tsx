import { useNavigate } from 'react-router-dom';
import { CreateRecipeDto, useRecipes } from '../../hooks/useRecipes';
import { RecipeForm } from '../../components/recipe/recipeForm';
import { useState } from 'react';

const RecipeCreatePage = () => {
  const { create } = useRecipes();
  const navigate = useNavigate();
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (data: CreateRecipeDto) => {
    try {
      await create(data);
      navigate('/receitas');
    } catch (err) {
      console.error('Erro ao criar receita:', err);
      setError('Não foi possível criar a receita. Verifique os dados e tente novamente.');
    }
  };

  return (
    <div className="max-w-2xl mx-auto p-6">
      <h1 className="text-3xl font-bold text-gray-800 mb-6">Nova Receita</h1>
      {error && (
        <div className="mb-4 p-4 bg-red-100 text-red-700 rounded-lg">
          {error}
        </div>
      )}
      <RecipeForm onSubmit={handleSubmit} />
    </div>
  );
};

export default RecipeCreatePage;