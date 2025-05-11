import { RestaurantsList } from '../../components/restaurant/RestaurantsList';
import { useNavigate } from 'react-router-dom';

const RestaurantsPage = () => {
  const navigate = useNavigate();

  return (
    <div className="flex flex-col gap-6 p-6">
      {/* Header da página */}
      <div className="flex flex-wrap items-center justify-between">
        <h1 className="text-3xl font-bold text-[#2E2E2E] tracking-tight">Restaurantes</h1>
        <button
          onClick={() => navigate('/restaurantes/novo')}
          className="mt-2 md:mt-0 bg-[#00796B] hover:bg-[#005B4D] text-white px-5 py-2 rounded-lg text-sm font-medium transition-colors duration-200"
        >
          + Novo Restaurante
        </button>
      </div>

      {/* Lista de restaurantes */}
      <RestaurantsList />
    </div>
  );
};

export default RestaurantsPage;
