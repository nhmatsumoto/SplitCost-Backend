using SplitCost.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SplitCost.Domain.Entities;

public class User : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; private set; }

    [Required]
    [MaxLength(200)]
    [Column("Name")]
    public string Name { get; private set; }

    [Required]
    [EmailAddress]
    [Column("Email")]
    public string Email { get; private set; }

    [Column("AvatarUrl")]
    public string AvatarUrl { get; private set; } = string.Empty;

    // Navigation

    // Participação em residências (via tabela intermediária)
    public ICollection<Member> Residences { get; set; } = new List<Member>();

    // Despesas registradas por este usuário
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    // Despesas pagas por este usuário
    public ICollection<Expense> ResidenceExpensesPaid { get; set; } = new List<Expense>();

    // Compartilhamentos de despesas em que este usuário está envolvido
    public ICollection<ExpenseShare> ExpenseShares { get; set; } = new List<ExpenseShare>();

    public User()
    {
        
    }

    public User(string name, string email, string avatarUrl)
    {
        SetName(name);
        SetEmail(email);
        SetAvatarUrl(avatarUrl);
    }

    public User SetUserId(Guid userId)
    {
        if(userId == Guid.Empty) throw new ArgumentNullException("userId");
        this.Id = userId;
        return this;
    }

    public User SetName(string name)
    {
        if(string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("name");
        Name = name;
        return this;
    }

    public User SetEmail(string email)
    {
        if(string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException("email");
        Email = email;
        return this;
    }

    public User SetAvatarUrl(string avatarUrl)
    {
        if (string.IsNullOrWhiteSpace(avatarUrl)) throw new ArgumentNullException("avatarUrl");
        AvatarUrl = avatarUrl; 
        return this;
    }
}
