using SplitCost.Domain.Entities;
using SplitCost.Domain.Factories;

namespace SplitCost.Infrastructure.Persistence.Mapping;

/// <summary>
/// Extensões para converter uma instância de Residence para uma instância de Residence do domínio.
/// </summary>
public static class ResidenceEntityExtensions
{

    /// <summary>
    /// Converte uma instância de Residence para uma instância de Residence do domínio.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Residence ToDomain(this Residence entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        var residence = ResidenceFactory
            .Create()
            .SetId(entity.Id)
            .SetName(entity.Name)
            .SetCreatedByUser(entity.CreatedByUserId)
            .SetStreet(entity.Street)
            .SetNumber(entity.Number)
            .SetApartment(entity.Apartment)
            .SetCity(entity.City)
            .SetPrefecture(entity.Prefecture)
            .SetCountry(entity.Country)
            .SetPostalCode(entity.PostalCode)
            .SetMembers(entity.Members.Select(m => m.ToDomain()))
            .SetExpenses(entity.Expenses.Select(e => e.ToDomain()));

        return residence;
    }
}
