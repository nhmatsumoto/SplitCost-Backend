using SplitCost.Domain.Common;

namespace SplitCost.Domain.Entities;

public class Residence : BaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<ResidenceMember> Members { get; set; } = new List<ResidenceMember>();
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    private Residence() { }

    public Residence(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome da casa não pode ser vazio.");
        Name = name;
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
}
