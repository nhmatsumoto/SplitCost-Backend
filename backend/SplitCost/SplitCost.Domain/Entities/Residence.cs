using SplitCost.Domain.Common;

namespace SplitCost.Domain.Entities;

public class Residence : BaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;

    // Navigation EF Core
    public Guid CreatedByUserId { get; set; }
    public User CreatedBy { get; set; }
    public ICollection<ResidenceMember> Members { get; set; } = new List<ResidenceMember>();
    public ICollection<ResidenceExpense> Expenses { get; set; } = new List<ResidenceExpense>();

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

    public void AddExpense(ResidenceExpense expense)
    {
        if (expense == null) throw new ArgumentNullException(nameof(expense));
        expense.ResidenceId = this.Id;
        Expenses.Add(expense);
    }
}
