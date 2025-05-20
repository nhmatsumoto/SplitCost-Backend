using SplitCost.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics.Metrics;
using System.IO;

namespace SplitCost.Domain.Entities;

[Table("Residences")]
public class Residence : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    [Column("Name")]
    public string Name { get; set; } = null!;

    // Navigation EF Core
    // Foreign Key for Address
    [ForeignKey("Address")]
    [Column("AddressId")]
    public Guid AddressId { get; set; }
    public Address Address { get; set; } = null!;

    [ForeignKey("CreatedBy")]
    [Column("CreatedByUserId")]
    public Guid CreatedByUserId { get; set; }
    public User CreatedBy { get; set; } = null!;

    public ICollection<ResidenceMember> Members { get; set; } = new List<ResidenceMember>();
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    private Residence() { }

    public Residence(string name, Guid userId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome da casa não pode ser vazio.");
        Name = name;

        if (userId == Guid.Empty)
            throw new ArgumentException("É necessário informar um usuário");
        CreatedByUserId = userId;
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome da casa não pode ser vazio.");
        Name = name;
    }

    public void AddMember(ResidenceMember residenceMember)
    {
        if (residenceMember == null) throw new ArgumentNullException(nameof(residenceMember));
        Members.Add(residenceMember);
    }

    public void AddExpense(Expense expense)
    {
        if (expense == null) throw new ArgumentNullException(nameof(expense));
        expense.ResidenceId = this.Id;
        Expenses.Add(expense);
    }

    public void SetAddress(string street, string number, string apartment, string city, string prefecture, string country, string postalCode)
    {
        Address = new Address(street, number, apartment, city, prefecture, country, postalCode);
    }
    
}