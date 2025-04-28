import { IUserDto } from "./IUserDto";

export interface IUserRequest {
    success: boolean;
    message: string;
    error: string | null,
    user: IUserDto,
    token: string,
    refreshToken: string,
}