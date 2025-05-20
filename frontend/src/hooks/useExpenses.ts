import { useCallback } from "react";
import { createApiClient } from "../api/client";

export interface ExpenseDto {
    expenseId: string;
    expenseType: string;
    amount: number;
    date: string;
    IsSharedAmongMembers?: boolean;
}

// export interface ExpenseDto {
//   type: string;
//   category: string;
//   amount: number;
//   date: string;
//   IsSharedAmongMembers?: boolean;
//   description?: string;
//   residenceId: string;
//   registeredByUserId?: string; // Usually the logged-in user
//   paidByUserId?: string;       // Who paid for it (can be the same as registeredByUserId)
// }

export const useExpenses = () => {

  const api = createApiClient();

  const create = useCallback(
    async (data: ExpenseDto) => {
      await api.post('/expense', data);
    },
    [api]
  );

  return { 
    create,
  };
};
