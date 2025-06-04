using SplitCost.Domain.Common;

namespace SplitCost.Domain.Entities;

public class Residence : BaseEntity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Guid CreatedByUserId { get; private set; }
    public User CreatedBy { get; private set; }


    //Address
    public string Street { get; private set; }
    public string Number { get; private set; }
    public string Apartment { get; private set; }
    public string City { get; private set; }
    public string Prefecture { get; private set; }
    public string Country { get; private set; }
    public string PostalCode { get; private set; }


    private readonly List<Member> _members = new();
    public IReadOnlyCollection<Member> Members => _members;

    private readonly List<Expense> _expenses = new();
    public IReadOnlyCollection<Expense> Expenses => _expenses;

    internal Residence() { }

    internal Residence(string name, Guid createdByUserId, string street, string number, string apartment, string city, string prefecture, string country, string postalCode)
    {
        SetName(name);
        SetCreatedByUser(createdByUserId);
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

    public Residence SetCreatedByUser(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("Usuário criador inválido.");
        CreatedByUserId = userId;
        return this;
    }

    public Residence AddMember(Member member)
    {
        if (member == null)
            throw new ArgumentNullException(nameof(member));
        if (_members.Any(m => m.UserId == member.UserId))
            throw new InvalidOperationException("Usuário já é membro da residência.");
        _members.Add(member);
        return this;
    }

    public Residence RemoveMember(Guid userId)
    {
        var member = _members.FirstOrDefault(m => m.UserId == userId);
        if (member == null)
            throw new InvalidOperationException("Membro não encontrado.");
        _members.Remove(member);
        return this;
    }

    public Residence AddExpense(Expense expense)
    {
        if (expense == null)
            throw new ArgumentNullException(nameof(expense));
        if (expense.ResidenceId != Id)
            throw new InvalidOperationException("Despesa não pertence a esta residência.");
        _expenses.Add(expense);
        return this;
    }

    public Residence SetStreet(string street)
    {
        if (string.IsNullOrWhiteSpace(street)) throw new ArgumentException("O nome da rua não pode ser vazio.");
        Street = street.Trim();
        return this;
    }

    public Residence SetNumber(string number)
    {
        if (string.IsNullOrWhiteSpace(number)) throw new ArgumentException("O número do endereço não pode ser vazio.");
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
        if (string.IsNullOrWhiteSpace(city)) throw new ArgumentException("A cidade não pode ser vazia.");
        City = city.Trim();
        return this;
    }

    public Residence SetPrefecture(string prefecture)
    {
        if (string.IsNullOrWhiteSpace(prefecture)) throw new ArgumentException("A província não pode ser vazia.");
        Prefecture = prefecture.Trim();
        return this;
    }

    public Residence SetCountry(string country)
    {
        if (string.IsNullOrWhiteSpace(country)) throw new ArgumentException("O país não pode ser vazio.");
        Country = country.Trim();
        return this;
    }

    public Residence SetPostalCode(string postalCode)
    {
        if (string.IsNullOrWhiteSpace(postalCode)) throw new ArgumentException("O código postal não pode ser vazio.");
        PostalCode = postalCode.Trim();
        return this;
    }
}

