import { create } from 'zustand';

// Ou definir tipo mais específico para o objeto de usuário
interface UserState {
  user: any | null; 
  setUser: (user: any | null) => void;
  logoutUser: () => void;
}

export const useUserStore = create<UserState>((set) => ({
  user: null,
  setUser: (user) => set({ user }),
  logoutUser: () => set({ user: null }),
}));