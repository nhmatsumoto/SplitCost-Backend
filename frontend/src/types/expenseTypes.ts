export interface ExpenseDto {
  type: string;
  category: string;
  amount: number;
  date: string;
  isSharedAmongMembers?: boolean | undefined;
  description?: string;
  residenceId: string | undefined;
  registerByUserId?: string;
  paidByUserId?: string; 
};


export interface CreateExpenseDto {
  type: string;
  category: string;
  amount: number;
  date: string;
  isSharedAmongMembers: boolean;
  description?: string;
  residenceId: string | undefined;
  registerByUserId: string; 
  paidByUserId: string;
}

export type UsersDictionary = Record<string, string>;
