import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useKeycloak } from '@react-keycloak/web';
import { useUser, RegisterUserDto } from '../../hooks/useUser'; // Importe o hook useUser

export const RegisterForm = () => {
  const { register } = useUser(); // Use a função register do seu hook useUser
  const navigate = useNavigate();
  const { keycloak } = useKeycloak();

  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!name || !email || !password) {
      setError('Todos os campos são obrigatórios.');
      return;
    }

    const payload: RegisterUserDto = { name, email, password };

    try {
      await register(payload);
      // Redirecionar para alguma página após o registro bem-sucedido
      navigate('/login');
    } catch (err: any) {
      setError(err?.message || 'Erro ao registrar usuário.');
    }
  };

  return (
    <form onSubmit={handleSubmit} className="bg-white p-6 rounded shadow space-y-4 max-w-md mx-auto">
      <h2 className="text-xl font-semibold text-gray-800 mb-4">Criar uma Conta</h2>
      {error && <p className="text-red-600 text-sm">{error}</p>}

      <div>
        <label htmlFor="name" className="block text-sm font-medium text-gray-700">Nome</label>
        <input
          type="text"
          id="name"
          value={name}
          onChange={(e) => setName(e.target.value)}
          className="w-full border px-3 py-2 rounded text-sm focus:ring-blue-500 focus:border-blue-500"
        />
      </div>

      <div>
        <label htmlFor="email" className="block text-sm font-medium text-gray-700">Email</label>
        <input
          type="email"
          id="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          className="w-full border px-3 py-2 rounded text-sm focus:ring-blue-500 focus:border-blue-500"
        />
      </div>

      <div>
        <label htmlFor="password" className="block text-sm font-medium text-gray-700">Senha</label>
        <input
          type="password"
          id="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          className="w-full border px-3 py-2 rounded text-sm focus:ring-blue-500 focus:border-blue-500"
        />
      </div>

      <div>
        <button
          type="submit"
          className="w-full bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700 text-sm focus:outline-none focus:ring-2 focus:ring-green-500 focus:ring-offset-2"
        >
          Registrar
        </button>
      </div>
    </form>
  );
};