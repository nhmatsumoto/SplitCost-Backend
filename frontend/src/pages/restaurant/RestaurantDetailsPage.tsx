import { useParams, useNavigate } from 'react-router-dom';
import { useRestaurants, RestaurantDto } from '../../hooks/useRestaurants';
import { useEffect, useState } from 'react';
import { RestaurantForm } from '../../components/restaurant/RestaurantForm';
import ConfirmModal from '../../components/ui/ConfirmModal';

const RestaurantDetailsPage = () => {
  const { id } = useParams<{ id: string }>(); // Tipagem explícita para id
  const navigate = useNavigate();
  const { getById, remove } = useRestaurants();

  const [restaurant, setRestaurant] = useState<RestaurantDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [showDeleteModal, setShowDeleteModal] = useState(false);

  useEffect(() => {
    const fetchRestaurant = async () => {
      try {
        if (!id) {
          throw new Error('ID do restaurante não fornecido.');
        }
        const data = await getById(id);
        if (!data) {
          throw new Error('Restaurante não encontrado.');
        }
        setRestaurant(data);
      } catch (err) {
        console.error('Erro ao carregar restaurante:', err);
        setRestaurant(null); // Garante que a UI reflita o erro
      } finally {
        setLoading(false);
      }
    };

    fetchRestaurant();
  }, [id, getById]);

  const handleDelete = async () => {
    if (!id) return;
    try {
      await remove(id);
      navigate('/restaurantes');
    } catch (err) {
      console.error('Erro ao excluir restaurante:', err);
    }
  };

  if (loading) return <p>Carregando restaurante...</p>;
  if (!restaurant) return <p>Restaurante não encontrado.</p>;

  return (
    <div className="max-w-2xl mx-auto space-y-6">
      <div className="flex justify-between items-center">
        <h1 className="text-2xl font-bold">Detalhes do Restaurante</h1>
        <button
          onClick={() => setShowDeleteModal(true)}
          className="text-sm text-red-600 hover:underline"
        >
          Deletar
        </button>
      </div>

      <RestaurantForm initialData={restaurant} />

      <ConfirmModal
        isOpen={showDeleteModal}
        title="Remover restaurante?"
        description="Essa ação é irreversível. Tem certeza que deseja excluir?"
        onCancel={() => setShowDeleteModal(false)}
        onConfirm={handleDelete}
      />
    </div>
  );
};

export default RestaurantDetailsPage;