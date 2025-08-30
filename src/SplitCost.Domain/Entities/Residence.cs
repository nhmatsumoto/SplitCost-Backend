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

    [Required]
    [MaxLength(200)]
    [Column("Name")]
    public string Name { get; set; }

    //Address
    [MaxLength(200)]
    public string Street { get; set; } = null!;

    [MaxLength(20)]
    public string Number { get; set; } = null!;

    [MaxLength(50)]
    public string Apartment { get; set; } = string.Empty;

    [MaxLength(100)]
    public string City { get; set; } = null!;

    [MaxLength(100)]
    public string Prefecture { get; set; } = null!;
   
    [MaxLength(100)]
    public string Country { get; set; } = null!;

    [MaxLength(20)]
    public string PostalCode { get; set; } = null!;

    public List<Member> Members { get; set; } = new List<Member>();

    public List<Expense> Expenses { get; set; } = new List<Expense>();

    public List<Income> Incomes { get; set; } = new List<Income>();

    internal Residence() { }

    internal Residence(string name, Guid createdByUserId, string street, string number, string apartment, string city, string prefecture, string country, string postalCode)
    {
        SetName(name);
        SetStreet(street);
        SetNumber(number);
        SetApartment(apartment);
        SetCity(city);
        SetPrefecture(prefecture);
        SetCountry(country);
        SetPostalCode(postalCode);
    }

    public Residence SetId(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("O ID da residência não pode ser vazio.");
        Id = id;
        return this;
    }

    public Residence SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome da residência não pode ser vazio.");
        Name = name.Trim();
        return this;
    }

    public Residence AddMember(Member member)
    {
        if (member == null)
            throw new ArgumentNullException(nameof(member));
        if (Members.Any(m => m.UserId == member.UserId))
            throw new InvalidOperationException("Usuário já é membro da residência.");
        Members.Add(member);
        return this;
    }

    public Residence RemoveMember(Guid userId)
    {
        var member = Members.FirstOrDefault(m => m.UserId == userId);
        if (member == null)
            throw new InvalidOperationException("Membro não encontrado.");
        Members.Remove(member);
        return this;
    }

    public Residence AddExpense(Expense expense)
    {
        if (expense == null)
            throw new ArgumentNullException(nameof(expense));
        if (expense.ResidenceId != Id)
            throw new InvalidOperationException("Despesa não pertence a esta residência.");
        Expenses.Add(expense);
        return this;
    }

    public Residence SetStreet(string street)
    {
        Street = street.Trim();
        return this;
    }

    public Residence SetNumber(string number)
    {
        Number = number.Trim();
        return this;
    }

    public Residence SetApartment(string apartment)
    {
        Apartment = apartment?.Trim() ?? string.Empty;
        return this;
    }

    public Residence SetCity(string city)
    {
        City = city.Trim();
        return this;
    }

    public Residence SetPrefecture(string prefecture)
    {
        Prefecture = prefecture.Trim();
        return this;
    }

    public Residence SetCountry(string country)
    {
        Country = country.Trim();
        return this;
    }

    public Residence SetPostalCode(string postalCode)
    {
        PostalCode = postalCode.Trim();
        return this;
    }

    public Residence SetMembers(IEnumerable<Member> members)
    {
        if (members is null) throw new ArgumentNullException(nameof(members));
        Members.Clear();
        Members.AddRange(members);
        return this;
    }

    public Residence SetExpenses(IEnumerable<Expense> expenses)
    {
        if (expenses is null) throw new ArgumentNullException(nameof(expenses));
        Expenses.Clear();
        Expenses.AddRange(expenses);
        return this;
    }
}

