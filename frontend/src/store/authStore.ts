// import { create } from 'zustand';


// export interface UserProfile {
//   id?: string;
//   username?: string;
//   email?: string;
// }

// export interface UserRoles {
//   isAdmin?: boolean;
//   isOwner?: boolean;
//   isMember?: boolean;
// }

// interface AuthState {
//   isAuthenticated: boolean;
//   userProfile: UserProfile | null;
//   userRoles: UserRoles | null;
//   setAuthInfo: (isAuthenticated: boolean, userProfile: UserProfile | null, userRoles: UserRoles | null) => void;
//   clearAuthInfo: () => void;
// }

// const useAuthStore = create<AuthState>((set) => ({
//   isAuthenticated: false,
//   userProfile: null,
//   userRoles: null,
//   setAuthInfo: (isAuthenticated, userProfile, userRoles) => set({ isAuthenticated, userProfile, userRoles }),
//   clearAuthInfo: () => set({ isAuthenticated: false, userProfile: null, userRoles: null }),
// }));

// export default useAuthStore;