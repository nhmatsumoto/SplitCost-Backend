import { useKeycloak } from "@react-keycloak/web";
import { Link } from "react-router-dom";

const HomePage = () => {

  const keycloak = useKeycloak();

  return (
    <div className="min-h-screen bg-gray-100 py-8 px-4">
      <Link to="/register" className="text-blue-500 hover:underline">Register</Link>
      <a onClick={() => keycloak.keycloak.login()} className="text-blue-500 hover:underline">Login</a>
    </div>
  );
};

export default HomePage;
