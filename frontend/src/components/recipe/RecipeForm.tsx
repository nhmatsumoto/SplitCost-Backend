import { useState, useEffect } from 'react';
import { CreateRecipeDto, RecipeDto, RecipeIngredientDto } from '../../hooks/useRecipes';
import { useRestaurants } from '../../hooks/useRestaurants';
import { useIngredients } from '../../hooks/useIngredients';

interface RecipeFormProps {
  initialData?: RecipeDto;
  onSubmit: (data: RecipeFormProps['initialData'] extends RecipeDto ? RecipeDto : CreateRecipeDto) => Promise<void>;
}

export const RecipeForm = ({ initialData, onSubmit }: RecipeFormProps) => {
  const { getAll: getAllRestaurants } = useRestaurants();
  const { getAll: getAllIngredients } = useIngredients();

  const [restaurants, setRestaurants] = useState<{ restaurantId: string; name: string }[]>([]);
  const [ingredients, setIngredients] = useState<{ ingredientId: string; name: string }[]>([]);
  const [formData, setFormData] = useState<CreateRecipeDto | RecipeDto>(
    initialData || {
      name: '',
      profitMargin: 0,
      restaurantId: '',
      ingredients: [],
    }
  );
  const [loading, setLoading] = useState(true);
  const [errors, setErrors] = useState<{ [key: string]: string }>({});

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [restaurantsData, ingredientsData] = await Promise.all([
          getAllRestaurants(),
          getAllIngredients(),
        ]);
        setRestaurants(restaurantsData);
        setIngredients(ingredientsData);
      } catch (err) {
        console.error('Erro ao carregar dados:', err);
        setErrors({ global: 'Não foi possível carregar restaurantes ou ingredientes.' });
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [getAllRestaurants, getAllIngredients]);

  const validateForm = () => {
    const newErrors: { [key: string]: string } = {};

    if (!formData.name.trim()) newErrors.name = 'O nome da receita é obrigatório.';
    if (formData.profitMargin < 0 || isNaN(formData.profitMargin)) newErrors.profitMargin = 'A margem de lucro deve ser um número positivo.';
    if (!formData.restaurantId) newErrors.restaurantId = 'Selecione um restaurante.';

    if (formData.ingredients.length === 0) {
      newErrors.ingredients = 'Adicione pelo menos um ingrediente.';
    } else {
      formData.ingredients.forEach((ing, index) => {
        if (!ing.ingredientId) newErrors[`ingredient_${index}`] = `Selecione um ingrediente para a posição ${index + 1}.`;
        if (ing.quantityUsed <= 0 || isNaN(ing.quantityUsed)) newErrors[`quantity_${index}`] = `A quantidade deve ser maior que zero.`;
        if (!['kg', 'g', 'L', 'ml', 'unit'].includes(ing.quantityUnitSymbol)) newErrors[`unit_${index}`] = `Unidade inválida.`;
      });

      const ids = formData.ingredients.map(i => i.ingredientId);
      const duplicates = ids.filter((id, i) => id && ids.indexOf(id) !== i);
      if (duplicates.length) newErrors.ingredients = 'Não é permitido repetir ingredientes.';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: name === 'profitMargin' ? parseFloat(value) : value,
    }));
    setErrors((prev) => ({ ...prev, [name]: '' }));
  };

  const handleIngredientChange = (index: number, field: keyof RecipeIngredientDto, value: string | number) => {
    const updated = [...formData.ingredients];
    updated[index] = { ...updated[index], [field]: value };
    setFormData((prev) => ({ ...prev, ingredients: updated }));
    setErrors((prev) => ({
      ...prev,
      [`ingredient_${index}`]: '',
      [`quantity_${index}`]: '',
      [`unit_${index}`]: '',
      ingredients: '',
    }));
  };

  const addIngredient = () => {
    setFormData((prev) => ({
      ...prev,
      ingredients: [...prev.ingredients, { ingredientId: '', quantityUsed: 0, quantityUnitSymbol: 'unit' }],
    }));
    setErrors((prev) => ({ ...prev, ingredients: '' }));
  };

  const removeIngredient = (index: number) => {
    setFormData((prev) => ({
      ...prev,
      ingredients: prev.ingredients.filter((_, i) => i !== index),
    }));
    setErrors((prev) => ({
      ...prev,
      [`ingredient_${index}`]: '',
      [`quantity_${index}`]: '',
      [`unit_${index}`]: '',
      ingredients: '',
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (validateForm()) await onSubmit(formData);
  };

  if (loading) return <p className="text-[#9EA7AD]">Carregando dados...</p>;

  return (
    <form onSubmit={handleSubmit} className="space-y-6">
      {errors.global && (
        <div className="p-4 bg-red-100 text-red-700 rounded-lg">{errors.global}</div>
      )}

      {/* Nome */}
      <div>
        <label htmlFor="name" className="block text-sm font-medium text-[#2E2E2E]">Nome</label>
        <input
          type="text"
          name="name"
          value={formData.name}
          onChange={handleChange}
          required
          className={`mt-1 w-full px-4 py-2 border rounded-lg shadow-sm focus:ring-2 focus:ring-[#00796B] ${errors.name ? 'border-red-500' : 'border-[#E0E0E0]'}`}
        />
        {errors.name && <p className="mt-1 text-sm text-red-600">{errors.name}</p>}
      </div>

      {/* Margem de Lucro */}
      <div>
        <label htmlFor="profitMargin" className="block text-sm font-medium text-[#2E2E2E]">Margem de Lucro (%)</label>
        <input
          type="number"
          name="profitMargin"
          value={formData.profitMargin * 100}
          onChange={(e) =>
            handleChange({
              target: {
                name: 'profitMargin',
                value: (parseFloat(e.target.value) / 100).toString(),
              },
            } as any)
          }
          step="0.1"
          required
          className={`mt-1 w-full px-4 py-2 border rounded-lg shadow-sm focus:ring-2 focus:ring-[#00796B] ${errors.profitMargin ? 'border-red-500' : 'border-[#E0E0E0]'}`}
        />
        {errors.profitMargin && <p className="mt-1 text-sm text-red-600">{errors.profitMargin}</p>}
      </div>

      {/* Restaurante */}
      <div>
        <label htmlFor="restaurantId" className="block text-sm font-medium text-[#2E2E2E]">Restaurante</label>
        <select
          name="restaurantId"
          value={formData.restaurantId}
          onChange={handleChange}
          required
          className={`mt-1 w-full px-4 py-2 border rounded-lg shadow-sm focus:ring-2 focus:ring-[#00796B] ${errors.restaurantId ? 'border-red-500' : 'border-[#E0E0E0]'}`}
        >
          <option value="">Selecione um restaurante</option>
          {restaurants.map((r) => (
            <option key={r.restaurantId} value={r.restaurantId}>{r.name}</option>
          ))}
        </select>
        {errors.restaurantId && <p className="mt-1 text-sm text-red-600">{errors.restaurantId}</p>}
      </div>

      {/* Ingredientes */}
      <div>
        <label className="block text-sm font-medium text-[#2E2E2E]">Ingredientes</label>
        {errors.ingredients && <p className="mt-1 text-sm text-red-600">{errors.ingredients}</p>}

        {formData.ingredients.map((ingredient, index) => (
          <div key={index} className="mt-4 p-4 bg-white border border-[#E0E0E0] rounded-lg shadow-sm space-y-4">
            {/* Ingrediente */}
            <div>
              <label className="block text-sm font-medium text-[#2E2E2E]">Ingrediente</label>
              <select
                value={ingredient.ingredientId}
                onChange={(e) => handleIngredientChange(index, 'ingredientId', e.target.value)}
                className={`mt-1 w-full px-4 py-2 border rounded-lg shadow-sm focus:ring-2 focus:ring-[#00796B] ${errors[`ingredient_${index}`] ? 'border-red-500' : 'border-[#E0E0E0]'}`}
              >
                <option value="">Selecione um ingrediente</option>
                {ingredients.map((i) => (
                  <option key={i.ingredientId} value={i.ingredientId}>{i.name}</option>
                ))}
              </select>
              {errors[`ingredient_${index}`] && <p className="mt-1 text-sm text-red-600">{errors[`ingredient_${index}`]}</p>}
            </div>

            {/* Quantidade */}
            <div>
              <label className="block text-sm font-medium text-[#2E2E2E]">Quantidade</label>
              <input
                type="number"
                value={ingredient.quantityUsed}
                onChange={(e) => handleIngredientChange(index, 'quantityUsed', parseFloat(e.target.value))}
                step="0.01"
                className={`mt-1 w-full px-4 py-2 border rounded-lg shadow-sm focus:ring-2 focus:ring-[#00796B] ${errors[`quantity_${index}`] ? 'border-red-500' : 'border-[#E0E0E0]'}`}
              />
              {errors[`quantity_${index}`] && <p className="mt-1 text-sm text-red-600">{errors[`quantity_${index}`]}</p>}
            </div>

            {/* Unidade */}
            <div>
              <label className="block text-sm font-medium text-[#2E2E2E]">Unidade</label>
              <select
                value={ingredient.quantityUnitSymbol}
                onChange={(e) => handleIngredientChange(index, 'quantityUnitSymbol', e.target.value)}
                className={`mt-1 w-full px-4 py-2 border rounded-lg shadow-sm focus:ring-2 focus:ring-[#00796B] ${errors[`unit_${index}`] ? 'border-red-500' : 'border-[#E0E0E0]'}`}
              >
                <option value="kg">Quilograma (kg)</option>
                <option value="g">Grama (g)</option>
                <option value="L">Litro (L)</option>
                <option value="ml">Mililitro (ml)</option>
                <option value="unit">Unidade (unit)</option>
              </select>
              {errors[`unit_${index}`] && <p className="mt-1 text-sm text-red-600">{errors[`unit_${index}`]}</p>}
            </div>

            <button
              type="button"
              onClick={() => removeIngredient(index)}
              className="text-sm text-[#FF6B6B] hover:underline"
            >
              Remover
            </button>
          </div>
        ))}

        <button
          type="button"
          onClick={addIngredient}
          className="mt-2 text-sm text-[#00796B] hover:underline"
        >
          + Adicionar Ingrediente
        </button>
      </div>

      <button
        type="submit"
        className="w-full bg-[#00796B] hover:bg-[#005B4D] text-white px-4 py-2 rounded-lg transition-colors duration-200 font-medium"
      >
        {initialData ? 'Salvar Alterações' : 'Criar Receita'}
      </button>
    </form>
  );
};
