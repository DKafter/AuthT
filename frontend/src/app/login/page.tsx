"use client";

import { useState } from "react";
import { IUser } from "../models/IUser";
import { useRouter } from "next/navigation";
import { authUser } from "../services/UserService";
import { IUserResponse } from "../models/IUserResponse";
import { IUserRequest } from "../models/IUserRequest";
import { RolesEnum } from "../models/RoleEnum";

export default function LoginPage() {
  const [defaultUser, _] = useState<IUser>({
    id: Math.random(),
    username: "",
    email: "",
    password: "",
    roles: [RolesEnum.Quest],
    token: "",
    refreshToken: "",
  } as IUser);

  const [user, setUser] = useState<IUser>(defaultUser as IUser);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const router = useRouter();

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;

    setUser({ ...user, [name]: value });
  };

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();

    setError("");
    setSuccess("");

    const data: IUserRequest = await authUser({
      username: user.username,
      password: user.password,
      email: user.email,
    });
    if (data.success) {
      setSuccess(data.message as string);
      router.push("/");
    } else {
      setError(data.error as string);
    }
    if (data.token) {
      localStorage.setItem("token", data.token as string);
      localStorage.setItem("user", JSON.stringify(data.user));
    }

    setLoading(true);
  };
  return (
    <>
      {error && (
        <div className="mb-4 p-3 bg-red-100 text-red-700 rounded">{error}</div>
      )}
      {success && (
        <div className="mb-4 p-3 bg-green-100 text-green-700 rounded">
          {success}
        </div>
      )}
      <form onSubmit={handleLogin} method="POST" className="max-w-md mx-auto" >
        <div className="mb-4">
          <label htmlFor="email" className="block text-gray-700 mb-2">
            Email
          </label>
          <input
            type="email"
            id="email"
            name="email"
            value={user.email}
            onChange={handleChange}
            className="w-full px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500"
            required
          />
        </div>

        <div className="mb-6">
          <label htmlFor="password" className="block text-gray-700 mb-2">
            Password
          </label>
          <input
            type="password"
            id="password"
            name="password"
            value={user.password}
            onChange={handleChange}
            className="w-full px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500"
            required
          />
        </div>

        <button
          type="submit"
          disabled={success ? true : false}
          className={`w-full py-2 px-4 bg-blue-600 text-white rounded hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 ${
            loading ? "opacity-70 cursor-not-allowed" : ""
          }`}
        >
          {success ? "Logging in..." : "Login"}
        </button>
      </form>
    </>
  );
}
