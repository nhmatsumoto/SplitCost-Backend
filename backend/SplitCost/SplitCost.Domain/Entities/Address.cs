using SplitCost.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SplitCost.Domain.Entities;

[Table("Addresses")]
public class Address : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; private set; }

    [Required]
    [MaxLength(200)]
    [Column("Street")]
    public string Street { get; private set; }

    [Required]
    [MaxLength(20)]
    [Column("Number")]
    public string Number { get; private set; }

    [MaxLength(200)]
    [Column("Apartment")]
    public string Apartment { get; private set; }

    [Required]
    [MaxLength(200)]
    [Column("City")]
    public string City { get; private set; }

    [MaxLength(200)]
    [Column("Prefecture")] 
    public string Prefecture { get; private set; }

    [MaxLength(200)]
    [Column("Country")]
    public string Country { get; private set; }

    [Required]
    [MaxLength(20)]
    [Column("PostalCode")]
    public string PostalCode { get; private set; } 

    public Residence Residence { get; private set; }

    public Address()
    {

    }

    public Address SetStreet(string street)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("O nome da rua não pode ser vazio.");
        Street = street;
        return this;
    }

    public Address SetNumber(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentException("O número do endereço não pode ser vazio.");
        Number = number;
        return this;
    }

    // Apartamento pode ser nulo ou vazio
    public Address SetApartment(string apartment)
    {
        Apartment = apartment ?? ""; 
        return this;
    }

    public Address SetCity(string city)
    {
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("A cidade não pode ser vazia.");
        City = city;
        return this;
    }

    public Address SetPrefecture(string prefecture)
    {
        if (string.IsNullOrWhiteSpace(prefecture))
            throw new ArgumentException("A província/estado não pode ser vazio.");
        Prefecture = prefecture;
        return this;
    }

    public Address SetCountry(string country)
    {
        if (string.IsNullOrWhiteSpace(country))
            throw new ArgumentException("O país não pode ser vazio.");
        Country = country;
        return this;
    }

    public Address SetPostalCode(string postalCode)
    {
        if (string.IsNullOrWhiteSpace(postalCode))
            throw new ArgumentException("O código postal não pode ser vazio.");
        PostalCode = postalCode;
        return this;
    }
}