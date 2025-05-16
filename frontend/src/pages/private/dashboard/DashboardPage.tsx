import { ResidenceList } from "../../../components/residence/ResidenceList";
import useAuthStore from "../../../store/authStore";

const Dashboard = () => {

  const user = useAuthStore((state) => state.userProfile);

  return (
    <div className="min-h-screen bg-gray-100 py-8 px-4">
      <ResidenceList />
      {user?.username && (
        <div className="mt-4 text-center">
          <h2 className="text-2xl font-bold text-gray-800">
            Welcome, {user.username}!
            {user.id}
          </h2>
          <p className="text-gray-600">
            You are logged in and ready to explore the application.
          </p>
        </div>
      )}
    </div>
  );
};

export default Dashboard;
