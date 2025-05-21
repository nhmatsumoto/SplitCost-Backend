import { create } from 'zustand';
import { ResidenceDto } from '../hooks/useResidences';

interface ResidenceState {
  residence: ResidenceDto | null;
  setResidence: (residence: any | null) => void;
  clearResidenceState: () => void;
}

export const useResidenceStore = create<ResidenceState>((set) => ({
  residence: null,
  setResidence: (residence) => set({ residence }),
  clearResidenceState: () => set({ residence: null }),
}));