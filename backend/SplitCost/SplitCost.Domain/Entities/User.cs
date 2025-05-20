using SplitCost.Domain.Common;

namespace SplitCost.Domain.Entities;

public class User : BaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? AvatarUrl { get; set; } 


    // Navigation

    // Participação em residências (via tabela intermediária)
    public ICollection<ResidenceMember> Residences { get; set; } = new List<ResidenceMember>();

    // Despesas registradas por este usuário
    public ICollection<Expense> ResidenceExpensesRegistered { get; set; } = new List<Expense>();

    // Despesas pagas por este usuário
    public ICollection<Expense> ResidenceExpensesPaid { get; set; } = new List<Expense>();

    // Compartilhamentos de despesas em que este usuário está envolvido
    public ICollection<ResidenceExpenseShare> ExpenseShares { get; set; } = new List<ResidenceExpenseShare>();

}
