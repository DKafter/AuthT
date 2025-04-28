import { IUserRequest } from "../models/IUserRequest";
import { IUserResponse } from "../models/IUserResponse";
const API_URL = "https://localhost:7010/api"

export const createUser = async (user: IUserResponse): Promise<IUserRequest> => {
    const data = await fetch(`${API_URL}/User/create?username="${user.username}"&password="${user.password}"&email="${user.email}"`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json",
            "Access-Control-Allow-Origin": "*",
        }
    }).then((response) => {
        return response.json();
    }).catch((error) => {
        console.error(error);
    });
    return data;
};

export const authUser = async (user: IUserResponse): Promise<IUserRequest> => {
    const URL = `${API_URL}/User/login?email=${user.email}&password=${user.password}`;
    console.log(URL);
    const data = await fetch(URL, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json",
            "Access-Control-Allow-Origin": "*",
        }
    }).then((response) => {
        return response.json();
    }).catch((error) => {
        console.error(`ERROR ${error}`);
    });
    console.table(data);
    return data;
};