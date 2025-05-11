import { NavLink } from 'react-router-dom';
import { PackageOpen, Pizza, Store, UtensilsCrossed } from 'lucide-react';

const links = [
  { to: '/ingredientes', label: 'Ingredientes', icon: PackageOpen },
  { to: '/receitas', label: 'Receitas', icon: Pizza },
  { to: '/restaurantes', label: 'Restaurantes', icon: Store },
];

export const Sidebar = () => {
  return (
    <aside className="w-64 bg-[#F4F6F8] border-r border-[#E0E0E0] h-screen p-6 flex flex-col">
      {/* Logo */}
      <div className="flex items-center gap-3 mb-8">
        <UtensilsCrossed className="h-7 w-7 text-[#00796B]" />
        <h1 className="text-2xl font-bold text-[#2E2E2E] tracking-tight">DishFlow</h1>
      </div>

      {/* Divider */}
      <div className="border-b border-[#E0E0E0] mb-6" />

      {/* Navegação */}
      <nav className="flex flex-col gap-1">
        {links.map(({ to, label, icon: Icon }) => (
          <NavLink
            key={to}
            to={to}
            className={({ isActive }) =>
              `group flex items-center gap-3 px-4 py-2 rounded-lg font-medium transition-all duration-200
              ${
                isActive
                  ? 'bg-[#D8F3DC] text-[#00796B]'
                  : 'text-[#9EA7AD] hover:bg-[#E9F5EF] hover:text-[#00796B]'
              }`
            }
          >
            <Icon className="w-5 h-5" />
            <span className="text-sm tracking-wide">{label}</span>
          </NavLink>
        ))}
      </nav>

      {/* Espaço para expansão futura */}
      <div className="mt-auto pt-6 border-t border-[#E0E0E0] text-xs text-[#9EA7AD]">
        {/* <button className="hover:underline">Colapsar</button> */}
        <p>© {new Date().getFullYear()} DishFlow</p>
      </div>
    </aside>
  );
};