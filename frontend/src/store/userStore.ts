import { create } from 'zustand';

export interface UserProfile {
  id?: string;
  username?: string;
  email?: string;
}

export interface UserRoles {
  isAdmin?: boolean;
  isOwner?: boolean;
  isMember?: boolean;
}

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