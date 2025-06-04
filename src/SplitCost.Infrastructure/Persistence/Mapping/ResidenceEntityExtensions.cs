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
            .SetPostalCode(entity.PostalCode);

        return residence;
    }


    public static ResidenceEntity ToEntity(this Residence residence)
    {
        if (residence == null) throw new ArgumentNullException(nameof(residence));

#warning Criar Factory para ResidenceEntity?
        var entity = new ResidenceEntity
        {
            Id = residence.Id,
            Name = residence.Name,
            CreatedByUserId = residence.CreatedByUserId,
            Street = residence.Street,
            Number = residence.Number,
            Apartment = residence.Apartment ?? string.Empty,
            City = residence.City,
            Prefecture = residence.Prefecture,
            Country = residence.Country,
            PostalCode = residence.PostalCode
        };

        return entity;
    }
}
