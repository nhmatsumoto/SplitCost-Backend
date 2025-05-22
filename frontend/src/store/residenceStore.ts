import { create } from 'zustand';
import { ResidenceDto } from '../types/residenceTypes';

interface ResidenceState {
  residence: ResidenceDto | null;
  setResidence: (residence: ResidenceDto | null) => void;
  clearResidenceState: () => void;
}

export const useResidenceStore = create<ResidenceState>((set) => ({
  residence: null,
  setResidence: (residence) => set({ residence }),
  clearResidenceState: () => set({ residence: null }),
}));