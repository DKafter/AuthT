import "./globals.css";
import Link from "next/link";
import { cookies } from 'next/headers';
import 'bootstrap/dist/css/bootstrap.min.css';
// Navigation items
const publicItems = [
  {
    key: "home",
    label: <Link href={"/"}>Home</Link>,
  },
];

// Additional items for authenticated users
const userItems = [
  {
    key: "profile",
    label: <Link href={"/profile"}>Profile</Link>,
  },
];

// Additional items for admin users
const adminItems = [
  {
    key: "admin",
    label: <Link href={"/admin"}>Admin Panel</Link>,
  },
];

export default async function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  // This is a server component, so we can check cookies directly
  const sessionToken = (await cookies()).get('session_token')?.value;
  
  // In a real app, you would verify the token and get the user role
  // This is a simplified example
  let userRole = 'guest';
  if (sessionToken) {
    // Simple simulation - in a real app, you'd decode the token or check against a database
    if (sessionToken === 'admin-token') userRole = 'admin';
    else if (sessionToken === 'user-token') userRole = 'user';
  }

  // Determine which navigation items to show based on role
  let navItems = [...publicItems];
  if (userRole === 'user' || userRole === 'admin') {
    navItems = [...navItems, ...userItems];
  }
  if (userRole === 'admin') {
    navItems = [...navItems, ...adminItems];
  }

  return (
    <html lang="en">
      <body>
        <header className="bg-gray-800 text-white p-4">
          <div className="container mx-auto flex justify-between items-center">
            <div className="text-xl font-bold">My User App</div>
            <nav>
              <ul className="flex space-x-6">
                {navItems.map((item) => (
                  <li key={item.key} className="hover:text-gray-300">
                    {item.label}
                  </li>
                ))}
                {!sessionToken ? (
                  <li className="hover:text-gray-300">
                    <Link href={"/login"}>Login</Link>
                  </li>
                ) : (
                  <li className="hover:text-gray-300">
                    <Link href={"/api/auth/logout"}>Logout</Link>
                  </li>
                )}
              </ul>
            </nav>
          </div>
        </header>
        <main className="container mx-auto p-4">
          {children}
        </main>
        <footer className="bg-gray-800 text-white p-4 mt-8">
          <div className="container mx-auto text-center">
            <p>© {new Date().getFullYear()} My user App. All rights reserved.</p>
          </div>
        </footer>
      </body>
    </html>
  );
}