import React, { useState, useCallback } from 'react';
import { RegisterUserDto, useAppUser } from '../../hooks/useAppUser'; // Adjust the import path as needed
import SuccessToast from '../../components/ui/SuccessToast'; // Assuming the path to SuccessToast
import ErrorToast from '../../components/ui/ErrorToast'; // Assuming the path to ErrorToast
import { useAuth } from 'react-oidc-context';
import { Navigate } from 'react-router-dom';

interface RegistrationFormProps {
  onSuccess?: () => void;
}

const RegistrationForm = ({ onSuccess } : RegistrationFormProps) => {

  const { signinRedirect, user, isAuthenticated } = useAuth();
  
  if(user && isAuthenticated) {
    return <Navigate to='/' />
  }

  const [formData, setFormData] = useState<RegisterUserDto>({
    Username: '',
    FirstName: '',
    LastName: '',
    Email: '',
    Password: '',
    ConfirmPassword: '',
  });
  const [passwordMatchError, setPasswordMatchError] = useState<string | null>(null);
  const { create: registerUser } = useAppUser();

  const handleChange = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prevData) => ({
      ...prevData,
      [name]: value,
    }));
  }, []);

  const resetForm = useCallback(() => {
    setFormData({
      Username: '',
      FirstName: '',
      LastName: '',
      Email: '',
      Password: '',
      ConfirmPassword: '',
    });
  }, []);

  const handleSubmit = useCallback(async (e: React.FormEvent) => {
    e.preventDefault();
    if (formData.Password !== formData.ConfirmPassword) {
      setPasswordMatchError('As senhas não coincidem.');
      return;
    }
    setPasswordMatchError(null);

    try {
      await registerUser(formData);
      SuccessToast('Usuário cadastrado com sucesso!');
      resetForm();
      if (onSuccess) {
        onSuccess();
        signinRedirect();
      }
    } catch (error: any) {
      console.error('Erro no registro:', error);
      ErrorToast(error?.response?.data?.message || 'Ocorreu um erro ao cadastrar o usuário.');
    }
  }, [formData, registerUser, onSuccess, resetForm, ErrorToast, SuccessToast]);

  return (
    <div className="flex flex-col items-center justify-center bg-gray-100">
      <div className="bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4 w-full max-w-md">
        <h2 className="block text-gray-700 text-xl font-bold mb-4">Cadastrar Usuário</h2>
        {passwordMatchError && <p className="text-red-500 text-sm italic mb-4">{passwordMatchError}</p>}
        <form className="space-y-4" onSubmit={handleSubmit}>
          {/* ... campos do formulário ... */}
          <div className="mb-4">
            <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="Username">
              Nome de Usuário:
            </label>
            <input
              className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
              id="Username"
              type="text"
              name="Username"
              value={formData.Username}
              onChange={handleChange}
              required
            />
          </div>
          <div className="mb-4">
            <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="FirstName">
              Nome:
            </label>
            <input
              className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
              id="FirstName"
              type="text"
              name="FirstName"
              value={formData.FirstName}
              onChange={handleChange}
            />
          </div>
          <div className="mb-4">
            <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="LastName">
              Sobrenome:
            </label>
            <input
              className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
              id="LastName"
              type="text"
              name="LastName"
              value={formData.LastName}
              onChange={handleChange}
            />
          </div>
          <div className="mb-4">
            <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="Email">
              Email:
            </label>
            <input
              className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
              id="Email"
              type="email"
              name="Email"
              value={formData.Email}
              onChange={handleChange}
              required
            />
          </div>
          <div className="mb-4">
            <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="Password">
              Senha:
            </label>
            <input
              className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
              id="Password"
              type="password"
              name="Password"
              value={formData.Password}
              onChange={handleChange}
              required
            />
          </div>
          <div className="mb-6">
            <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="ConfirmPassword">
              Confirmar Senha:
            </label>
            <input
              className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
              id="ConfirmPassword"
              type="password"
              name="ConfirmPassword"
              value={formData.ConfirmPassword}
              onChange={handleChange}
              required
            />
          </div>
          <button
            className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
            type="submit"
          >
            Cadastrar
          </button>
        </form>
      </div>
    </div>
  );
};

export default RegistrationForm;