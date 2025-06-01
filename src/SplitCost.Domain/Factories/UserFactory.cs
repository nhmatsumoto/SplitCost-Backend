using SplitCost.Domain.Entities;

namespace SplitCost.Domain.Factories;

public static class UserFactory
{
    /// <summary>
    /// Cria uma instância de User com os parâmetros fornecidos.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="email"></param>
    /// <param name="avatarUrl"></param>
    /// <returns></returns>
    public static User Create(Guid id, string username, string name, string email, string avatarUrl = "")
    {
        return new User(id, username, name, email, avatarUrl);
    }
}
