import { IngredientCreateForm } from '../../components/Ingredient/IngredientCreateForm';

const IngredientCreatePage = () => {
  return (
    <div className="max-w-xl mx-auto">
      <h1 className="text-2xl font-bold text-gray-800 mb-4">Novo Ingrediente</h1>
      <IngredientCreateForm />
    </div>
  );
};

export default IngredientCreatePage;
