import { useKeycloak } from '@react-keycloak/web';

export const Header = () => {
  const { keycloak } = useKeycloak();
  
  return (
    <header className="bg-[#F4F6F8] border-b border-[#E0E0E0] shadow-sm">
      <div className="px-6 py-4 flex justify-between items-center"> {/* Removido max-w-7xl */}
        <h1 className="text-2xl font-bold text-[#2E2E2E] tracking-tight">Painel</h1>
        <div className="flex items-center gap-5 justify-end">
          <span className="text-sm text-[#9EA7AD]">
            {keycloak.tokenParsed?.preferred_username || 'Usuário'}
          </span>
          <button
            className="text-sm font-medium text-[#FF6B6B] hover:text-[#C94A4A] transition-colors duration-200"
            onClick={() => keycloak.logout()}
          >
            Sair
          </button>
        </div>
      </div>
    </header>
  );
};