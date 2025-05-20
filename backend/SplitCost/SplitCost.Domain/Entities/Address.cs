using SplitCost.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SplitCost.Domain.Entities;

[Table("Addresses")]
public class Address : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    [Column("Street")]
    public string Street { get; set; }

    [Required]
    [MaxLength(20)]
    [Column("Number")]
    public string Number { get; set; }

    [MaxLength(200)]
    [Column("Apartment")]
    public string Apartment { get; set; } // For apartments, units, etc.

    [Required]
    [MaxLength(200)]
    [Column("City")]
    public string City { get; set; }

    [MaxLength(200)]
    [Column("Prefecture")] // State, Province, Prefecture, etc.
    public string Prefecture { get; set; }

    [MaxLength(200)]
    [Column("Country")]
    public string Country { get; set; }

    [Required]
    [MaxLength(20)]
    [Column("PostalCode")]
    public string PostalCode { get; set; } // Can handle various formats

    // Navigation property to Residence
    public Residence Residence { get; set; }

    public Address()
    {
        
    }

    public Address(string street, string number, string apartment, string city, string prefecture, string country, string postalCode)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("O nome da rua não pode ser vazio.");
        Street = street;

        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentException("O número do endereço não pode ser vazio.");
        Number = number;

        Apartment = apartment ?? ""; // Apartamento pode ser nulo ou vazio

        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("A cidade não pode ser vazia.");
        City = city;

        if (string.IsNullOrWhiteSpace(prefecture))
            throw new ArgumentException("A província/estado não pode ser vazio.");
        Prefecture = prefecture;

        if (string.IsNullOrWhiteSpace(country))
            throw new ArgumentException("O país não pode ser vazio.");
        Country = country;

        if (string.IsNullOrWhiteSpace(postalCode))
            throw new ArgumentException("O código postal não pode ser vazio.");
        PostalCode = postalCode;
    }

}
