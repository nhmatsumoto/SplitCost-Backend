import useAuthGuard from "../../hooks/useAuthGuard";

const SecuredContent = () => {
    const { isAuthenticated } = useAuthGuard();
  
    if (!isAuthenticated) return <div>Carregando...</div>; // ou Spinner
  
    return (
      <div>
        <h2>Você está autenticado e pode ver esse conteúdo!</h2>
      </div>
    );
};

export default SecuredContent;