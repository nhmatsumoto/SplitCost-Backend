using SplitCost.Domain.Entities;

namespace SplitCost.Domain.Factories;

public static class ResidenceFactory
{
    /// <summary>
    /// Cria uma instância de Residence com os parâmetros fornecidos.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="createdBUserId"></param>
    /// <param name="street"></param>
    /// <param name="number"></param>
    /// <param name="apartment"></param>
    /// <param name="city"></param>
    /// <param name="prefecture"></param>
    /// <param name="country"></param>
    /// <param name="postalCode"></param>
    /// <returns></returns>
    public static Residence Create(
        string name,
        Guid createdBUserId,
        string street,
        string number,
        string? apartment,
        string city,
        string prefecture,
        string country,
        string postalCode)
    {
        return new Residence()
            .SetName(name)
            .SetCreatedByUser(createdBUserId)
            .SetStreet(street)
            .SetNumber(number)
            .SetApartment(apartment)
            .SetCity(city)
            .SetPrefecture(prefecture)
            .SetCountry(country)
            .SetPostalCode(postalCode);
    }
}
