import React, { useState } from 'react';
import { Link, NavLink } from 'react-router-dom';
import { ArrowRightLeftIcon, BanknoteArrowUpIcon, HouseIcon, JapaneseYen, PersonStandingIcon, UserPlusIcon, WalletIcon, ChevronDownIcon, ChevronUpIcon } from 'lucide-react';

interface LinkItem {
  to?: string;
  label: string;
  icon?: React.ComponentType<React.SVGProps<SVGSVGElement>>;
  children?: Omit<LinkItem, 'children'>[];
}

const links: LinkItem[] = [
  { to: '/house', label: 'House', icon: HouseIcon },
  {
    label: 'Transactions',
    icon: ArrowRightLeftIcon,
    children: [
      { to: '/expenses', label: 'Expenses', icon: WalletIcon },
      { to: '/incomes', label: 'Incomes', icon: BanknoteArrowUpIcon },
    ],
  },
  { to: '/register', label: 'Register', icon: UserPlusIcon },
];

export const Sidebar = () => {
  const [openSubMenu, setOpenSubMenu] = useState<string | null>(null);

  const toggleSubMenu = (label: string) => {
    setOpenSubMenu((prev) => (prev === label ? null : label));
  };

  return (
    <aside className="w-64 bg-[#F4F6F8] border-r border-[#E0E0E0] h-screen p-6 flex flex-col">
      <div className="flex items-center gap-3 mb-8">
        <JapaneseYen className="h-7 w-7 text-[#00796B]" />
        <h1 className="text-2xl font-bold text-[#2E2E2E] tracking-tight">
          <Link to="/">
            Split-Cost
          </Link>
        </h1>
      </div>

      <div className="border-b border-[#E0E0E0] mb-6" />

      <nav className="flex flex-col gap-1">
        {links.map((link) => {
          if (link.children) {
            const isSubMenuOpen = openSubMenu === link.label;
            return (
              <div key={link.label} className="flex flex-col">
                <button
                  onClick={() => toggleSubMenu(link.label)}
                  className={`group flex items-center justify-between gap-3 px-4 py-2 rounded-lg font-medium transition-all duration-200
                    ${isSubMenuOpen ? 'bg-[#D8F3DC] text-[#00796B]' : 'text-[#9EA7AD] hover:bg-[#E9F5EF] hover:text-[#00796B]'}`}
                >
                  <div className="flex items-center gap-3">
                    {link.icon && <link.icon className="w-5 h-5" />}
                    <span className="text-sm tracking-wide">{link.label}</span>
                  </div>
                  {isSubMenuOpen ? <ChevronUpIcon className="w-4 h-4 text-[#00796B]" /> : <ChevronDownIcon className="w-4 h-4 text-[#9EA7AD]" />}
                </button>
                {isSubMenuOpen && (
                  <div className="ml-6 flex flex-col gap-1 mt-1">
                    {link.children.map((child) => (
                      <NavLink
                        key={child.to}
                        to={child.to!}
                        className={({ isActive }) =>
                          `group flex items-center gap-3 px-4 py-2 rounded-lg font-medium transition-all duration-200 text-sm tracking-wide
                          ${
                            isActive
                              ? 'bg-[#D8F3DC] text-[#00796B]'
                              : 'text-[#9EA7AD] hover:bg-[#E9F5EF] hover:text-[#00796B]'
                          }`
                        }
                      >
                        {child.icon && <child.icon className="w-4 h-4" />}
                        <span>{child.label}</span>
                      </NavLink>
                    ))}
                  </div>
                )}
              </div>
            );
          } else {
            return (
              <NavLink
                key={link.to}
                to={link.to!}
                className={({ isActive }) =>
                  `group flex items-center gap-3 px-4 py-2 rounded-lg font-medium transition-all duration-200
                  ${
                    isActive
                      ? 'bg-[#D8F3DC] text-[#00796B]'
                      : 'text-[#9EA7AD] hover:bg-[#E9F5EF] hover:text-[#00796B]'
                  }`
                }
              >
                {link.icon && <link.icon className="w-5 h-5" />}
                <span className="text-sm tracking-wide">{link.label}</span>
              </NavLink>
            );
          }
        })}
      </nav>

      <div className="mt-auto pt-6 border-t border-[#E0E0E0] text-xs text-[#9EA7AD]">
        <p>© {new Date().getFullYear()} Design By NHMatsumoto</p>
      </div>
    </aside>
  );
};