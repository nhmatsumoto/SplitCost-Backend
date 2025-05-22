export interface AddressDto {
  street: string;
  number: string;
  apartment?: string;
  city: string;
  prefecture: string;
  country: string;
  postalCode: string;
}

export interface MemberDto {
  userId: string;
  userName: string;
  isPrimary: boolean;
}

export interface ExpenseDto {
  expenseId: string;
  expenseType: string;
  amount: number;
  date: string;
  IsSharedAmongMembers?: boolean;
}

export interface ResidenceDto {
  id: string;
  name: string;
  createdAt: string;
  updatedAt?: string;
  members: MemberDto[];
  expenses: ExpenseDto[];
  address: AddressDto;
}

export interface CreateResidenceDto {
  residenceName: string;
  userId: string | undefined;
  address: AddressDto;
}

export interface UpdateResidenceDto {
  residenceId: string;
  name: string;
  expenses: ExpenseDto[];
  members?: Members[]; 
}

export interface Members {
  userId: string;
  userName: string;
  isPrimary: boolean;
}