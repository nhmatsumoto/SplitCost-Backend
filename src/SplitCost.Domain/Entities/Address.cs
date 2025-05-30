using SplitCost.Domain.Common;

namespace SplitCost.Domain.Entities;

public class Address : BaseEntity
{
    public Guid Id { get; private set; }
    public string Street { get; private set; }
    public string Number { get; private set; }
    public string Apartment { get; private set; }
    public string City { get; private set; }
    public string Prefecture { get; private set; }
    public string Country { get; private set; }
    public string PostalCode { get; private set; }

    public Residence Residence { get; private set; }

    internal Address() { }

    internal Address(string street, string number, string apartment, string city, string prefecture, string country, string postalCode)
    {
        SetStreet(street);
        SetNumber(number);
        SetApartment(apartment);
        SetCity(city);
        SetPrefecture(prefecture);
        SetCountry(country);
        SetPostalCode(postalCode);
    }
    public Address SetStreet(string street)
    {
        if (string.IsNullOrWhiteSpace(street)) throw new ArgumentException("O nome da rua não pode ser vazio.");
        Street = street.Trim();
        return this;
    }

    public Address SetNumber(string number)
    {
        if (string.IsNullOrWhiteSpace(number)) throw new ArgumentException("O número do endereço não pode ser vazio.");
        Number = number.Trim();
        return this;
    }

    public Address SetApartment(string apartment)
    {
        Apartment = apartment?.Trim() ?? string.Empty;
        return this;
    }

    public Address SetCity(string city)
    {
        if (string.IsNullOrWhiteSpace(city)) throw new ArgumentException("A cidade não pode ser vazia.");
        City = city.Trim();
        return this;
    }

    public Address SetPrefecture(string prefecture)
    {
        if (string.IsNullOrWhiteSpace(prefecture)) throw new ArgumentException("A província não pode ser vazia.");
        Prefecture = prefecture.Trim();
        return this;
    }

    public Address SetCountry(string country)
    {
        if (string.IsNullOrWhiteSpace(country)) throw new ArgumentException("O país não pode ser vazio.");
        Country = country.Trim();
        return this;
    }

    public Address SetPostalCode(string postalCode)
    {
        if (string.IsNullOrWhiteSpace(postalCode)) throw new ArgumentException("O código postal não pode ser vazio.");
        PostalCode = postalCode.Trim();
        return this;
    }
}
