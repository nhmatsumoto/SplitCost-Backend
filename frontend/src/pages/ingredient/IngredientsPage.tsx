import { IngredientsList } from '../../components/Ingredient/IngredientsList';
import { useNavigate } from 'react-router-dom';

const IngredientsPage = () => {
  const navigate = useNavigate();

  return (
    <div className="flex flex-col gap-6 p-6">
      {/* Cabeçalho da Página */}
      <div className="flex flex-wrap items-center justify-between gap-4">
        <h1 className="text-3xl font-bold text-[#2E2E2E] tracking-tight">Ingredientes</h1>
        <button
          onClick={() => navigate('/ingredientes/novo')}
          className="bg-[#00796B] hover:bg-[#005B4D] text-white px-5 py-2 rounded-lg text-sm font-medium transition-colors duration-200"
        >
          + Novo Ingrediente
        </button>
      </div>

      {/* Lista de Ingredientes */}
      <IngredientsList />
    </div>
  );
};

export default IngredientsPage;