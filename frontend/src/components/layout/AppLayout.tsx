import { ReactNode } from 'react';
import { Sidebar } from '../layout/Sidebar';
import { Header } from '../layout/Header';
import { Toaster } from 'react-hot-toast';

export const AppLayout = ({ children }: { children: ReactNode }) => {
  return (
    <div className="min-h-screen flex bg-[#F4F6F8] text-[#2E2E2E]">
      <div className="w-64 shrink-0">
        <Sidebar />
      </div>

      <div className="flex-1 flex flex-col min-h-screen">
        <Header />
        <main className="flex-1 p-6 overflow-y-auto">{children}</main>
      </div>

      <Toaster position="top-right" />
    </div>
  );
};