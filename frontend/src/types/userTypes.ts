export interface UserDto {
  id: string;
  name: string;
  email: string;
  roles?: string[];
  createdAt?: string;
  updatedAt?: string;
}