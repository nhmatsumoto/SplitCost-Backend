using SplitCost.Domain.Entities;
using SplitCost.Domain.Factories;
using SplitCost.Infrastructure.Persistence.Entities;

namespace SplitCost.Infrastructure.Persistence.Mapping;

/// <summary>
/// Extensões para converter uma instância de ResidenceEntity para uma instância de Residence do domínio.
/// </summary>
public static class ResidenceEntityExtensions
{

    /// <summary>
    /// Converte uma instância de ResidenceEntity para uma instância de Residence do domínio.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Residence ToDomain(this ResidenceEntity entity)
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

    public static ResidenceEntity ToEntity(this Residence domain)
    {
        if (domain == null) throw new ArgumentNullException(nameof(domain));

        var entity = new ResidenceEntity
        {
            Id = domain.Id,
            Name = domain.Name,
            CreatedByUserId = domain.CreatedByUserId,
            Street = domain.Street,
            Number = domain.Number,
            Apartment = domain.Apartment ?? string.Empty,
            City = domain.City,
            Prefecture = domain.Prefecture,
            Country = domain.Country,
            PostalCode = domain.PostalCode
        };

        return entity;
    }
}
