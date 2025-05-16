import { Link } from "react-router-dom";
import LogoutButton from "../../components/auth/LogoutButton";

const HomePage = () => {
  return (
    <div className="min-h-screen bg-gray-100 py-8 px-4">
      <Link to="/login">
        <button className="bg-blue-500 text-white px-4 py-2 rounded">
          Login
        </button>
      </Link>
      <LogoutButton />
      <h1 className="text-3xl font-bold mb-4">Bem-vindo à nossa aplicação!</h1>
    </div>
  );
};

export default HomePage;
