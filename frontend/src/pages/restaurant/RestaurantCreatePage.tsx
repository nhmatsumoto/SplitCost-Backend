import { RestaurantCreateForm } from '../../components/restaurant/RestaurantCreateForm';

const RestaurantCreatePage = () => {
  return (
    <div className="max-w-xl mx-auto">
      <h1 className="text-2xl font-bold text-gray-800 mb-4">Novo Restaurante</h1>
      <RestaurantCreateForm />
    </div>
  );
};

export default RestaurantCreatePage;