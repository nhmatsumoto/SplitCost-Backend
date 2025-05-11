import { IngredientsList } from '../../components/Ingredient/IngredientsList';

const Dashboard = () => {
  return (
    <div className="min-h-screen bg-gray-100 py-8 px-4">
      <div className="max-w-5xl mx-auto">
        <h1 className="text-3xl font-bold text-gray-800 mb-6">Dashboard</h1>

        <section className="bg-white rounded-xl shadow-md p-6">
          <h2 className="text-xl font-semibold text-gray-700 mb-4">Lista de Ingredientes</h2>
          <IngredientsList />
        </section>
      </div>
    </div>
  );
};

export default Dashboard;
