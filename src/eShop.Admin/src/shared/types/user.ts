export interface UserDto {
    id: string;
    userName: string;
    firstName: string;
    lastName: string;
    email: string;
    roles: string[];
    joinedDate: string | null;
}