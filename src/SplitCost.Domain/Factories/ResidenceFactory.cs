using SplitCost.Domain.Entities;

namespace SplitCost.Domain.Factories;

public static class ResidenceFactory
{
    /// <summary>
    ///     Cria uma instância de Residence com os parâmetros fornecidos.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="createdBUserId"></param>
    /// <returns></returns>
    public static Residence Create(
        string name,
        Guid createdBUserId)
    {
        return new Residence(name, createdBUserId);
    }
}
