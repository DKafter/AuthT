"use client";

import { useEffect, useRef, useState } from "react";
import { IUser } from "../models/IUser";
import { HomeView } from "../views/HomeView";
import { RolesEnum } from "../models/RoleEnum";

export default function HomeComponent() {
  const [defaultUser, _] = useState<IUser>({
    id: Math.random(),
    username: "",
    email: "",
    password: "",
    roles: [RolesEnum.Quest],
    token: "",
  } as IUser);

  const [user, setUser] = useState<IUser>(defaultUser);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  useEffect(() => {
    const token = localStorage.getItem("token");
    const user = JSON.parse(localStorage.getItem("user") as string);
    if (token) {
      setUser({
        ...user,
        token: token,
      });
    }
  }, []);
  
  const handleLogin = async () => {
    setLoading(true);
    setError("");
    setSuccess("");
  };
  return loading ? (
    <div>
      <h1>Home Component</h1>
      <HomeView user={user} />
    </div>
  ) : (
    <div className="flex justify-center items-center min-h-screen">
      Loading...
    </div>
  );
}
