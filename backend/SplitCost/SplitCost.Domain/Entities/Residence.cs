using SplitCost.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SplitCost.Domain.Entities;

[Table("Residences")]
public class Residence : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [MaxLength(200)]
    [Column("Name")]
    public string? Name { get; set; }

    [ForeignKey("Address")]
    [Column("AddressId")]
    public Guid? AddressId { get; set; }
    public Address? Address { get; set; }

    [ForeignKey("CreatedBy")]
    [Column("CreatedByUserId")]
    public Guid? CreatedByUserId { get; set; }
    public User? CreatedBy { get; set; }

    public ICollection<Member> Members { get; set; } = new List<Member>();
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public Residence() { }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome da casa não pode ser vazio.");
        Name = name;
    }

    public void AddMember(Member residenceMember)
    {
        if (residenceMember == null) 
            throw new ArgumentNullException(nameof(residenceMember));
        Members.Add(residenceMember);
    }

    public void RemoveMember(Member residenceMember)
    {
        if (!Members.Remove(residenceMember)) 
            throw new ArgumentException(nameof(residenceMember));
    }

    public void AddExpense(Expense expense)
    {
        if (expense == null) 
            throw new ArgumentNullException(nameof(expense));
        expense.ResidenceId = this.Id;
        Expenses.Add(expense);
    }

    public Residence SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome da casa não pode ser vazio.");
        Name = name;
        return this;
    }

    public Residence SetAddress(Address address)
    {
        if (address == null) 
            throw new ArgumentNullException(nameof(address));
        Address = address;
        AddressId = address.Id;
        return this;
    }

    public Residence SetCreatedByUser(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("É necessário informar um usuário");
        CreatedByUserId = userId;

        return this;
    }
}
