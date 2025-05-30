using SplitCost.Domain.Entities;

namespace SplitCost.Domain.Factories;

public static class AddressFactory
{
    /// <summary>
    /// Cria uma instância de Address com os parâmetros fornecidos.
    /// </summary>
    /// <param name="street">Rua do endereço.</param>
    /// <param name="number">Número do endereço.</param>
    /// <param name="apartment">Apartamento do endereço (opcional).</param>
    /// <param name="city">Cidade do endereço.</param>
    /// <param name="prefecture">Estado ou província do endereço.</param>
    /// <param name="country">País do endereço.</param>
    /// <param name="postalCode">Código postal do endereço.</param>
    public static Address Create(
        string street,
        string number,
        string? apartment,
        string city,
        string prefecture,
        string country,
        string postalCode)
    {
        return new Address()
            .SetStreet(street)
            .SetNumber(number)
            .SetApartment(apartment)
            .SetCity(city)
            .SetPrefecture(prefecture)
            .SetCountry(country)
            .SetPostalCode(postalCode);
    }
}
