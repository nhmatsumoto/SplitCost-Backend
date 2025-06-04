namespace SplitCost.Application.Common;

public static class Messages
{
    // Generic
    public const string InvalidData = "Dados inválidos.";
    public const string RequiredField = "Campo obrigatório.";
    public const string InternalServerError = "Erro interno do servidor.";

    // Residence
    public const string ResidenceAlreadyExists = "Você já possui uma residência cadastrada.";
    public const string ResidenceNameRequired = "O nome da residência é obrigatório.";
    public const string ResidenceNotFound = "Residência não encontrada.";
    public const string ResidenceCreationFailed = "Erro ao criar residência. Tente novamente.";


    // User
    public const string UserAlreadyExists = "Usuário já cadastrado.";
    public const string UserNotFound = "Usuário não encontrado.";
    public const string UserCreationFailed = "Erro ao criar usuário. Tente novamente.";
}
