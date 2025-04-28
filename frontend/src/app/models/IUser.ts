import { RolesEnum } from "./RoleEnum";

export interface IUser {
    id: number;
    username: string;
    email: string;
    password: string;
    roles: RolesEnum[];
    token: string;
    refreshToken?: string;
}