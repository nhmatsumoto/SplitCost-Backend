import { RestaurantCreateForm } from '../../components/residence/ResidenceCreateForm';

const ResidenceCreatePage = () => {
  return (
    <div className="max-w-xl mx-auto">
      <h1 className="text-2xl font-bold text-gray-800 mb-4">Create new residence</h1>
      <RestaurantCreateForm />
    </div>
  );
};

export default ResidenceCreatePage;