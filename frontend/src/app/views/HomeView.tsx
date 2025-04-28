import { useRouter } from "next/navigation";
import { RolesEnum } from "../models/RoleEnum";
import { IUser } from "../models/IUser";

interface Props {
  user: IUser;
}

export const HomeView = ({ user }: Props) => {
  const router = useRouter();
  const role = user.roles[0];
  return role === RolesEnum.Quest ? (
    <div className="flex flex-col items-center justify-center min-h-screen">
      <h1 className="text-2xl mb-4">You are not logged in</h1>
      <button
        onClick={() => router.push("/login")}
        className="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600"
      >
        Login
      </button>
    </div>
  ) : role === RolesEnum.Admin || role === RolesEnum.User ? (
    <div className="flex flex-col items-center justify-center min-h-screen">
      <h1 className="text-2xl mb-4">Welcome, {user.username}!</h1>
      <button
        onClick={() => router.push("/logout")}
        className="px-4 py-2 bg-red-500 text-white rounded hover:bg-red-600"
      >
        Logout
      </button>
    </div>
  ) : (
    role === RolesEnum.Admin && (
      <div className="flex flex-col items-center justify-center min-h-screen">
        <h1 className="text-2xl mb-4">Welcome, {user.username}!</h1>
        <button
          onClick={() => router.push("/logout")}
          className="px-4 py-2 bg-red-500 text-white rounded hover:bg-red-600"
        >
          Logout
        </button>
      </div>
    )
  );
};
