using SplitCost.Domain.Common;

namespace SplitCost.Domain.Entities;

public class Residence : BaseEntity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Guid? AddressId { get; private set; }
    public Address? Address { get; private set; }
    public Guid? CreatedByUserId { get; private set; }
    public User? CreatedBy { get; private set; }

    private readonly List<Member> _members = new();
    public IReadOnlyCollection<Member> Members => _members;

    private readonly List<Expense> _expenses = new();
    public IReadOnlyCollection<Expense> Expenses => _expenses;

    internal Residence() { }

    internal Residence(string name, Guid createdByUserId)
    {
        SetName(name);
        SetCreatedByUser(createdByUserId);
    }

    public Residence SetId(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("O ID da residência não pode ser vazio.");
        Id = id;
        return this;
    }

    public Residence SetAddressId(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("O ID do endereço não pode ser vazio.");
        AddressId = id;
        return this;
    }
    public Residence SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome da residência não pode ser vazio.");
        Name = name.Trim();
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
}

