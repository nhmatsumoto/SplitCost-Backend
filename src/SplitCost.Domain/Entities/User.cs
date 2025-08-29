using SplitCost.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SplitCost.Domain.Entities;


[Table("Users")]
public class User : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [MaxLength(200)]
    [Column("Name")]
    public string Name { get; set; }

    public string Username { get; set; }
    public string Email { get; set; }
    public string AvatarUrl { get; set; }

    [ForeignKey("CreatedBy")]
    [Column("CreatedByUserId")]
    public Guid CreatedByUserId { get; set; }
    public User CreatedBy { get; set; }

    public ICollection<Member> Members { get; set; } = new List<Member>();

    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public ICollection<Income> Incomes { get; set; } = new List<Income>();

    internal User(Guid id, string username, string name, string email, string avatarUrl = "")
    {
        SetId(id);
        SetUsername(username);
        SetName(name);
        SetEmail(email);
        SetAvatarUrl(avatarUrl);
    }

    public User() { }

    public User SetId(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("User Id cannot be empty.", nameof(id));
        Id = id;
        return this;
    }

    public User SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.", nameof(name));
        Name = name;
        return this;
    }

    public User SetUsername(string username)
    {
        if(string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be empty.", nameof(username));
        Username = username;
        return this;
    }

    public User SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty.", nameof(email));
        // validação de formato de email
        Email = email;
        return this;
    }

    public User SetAvatarUrl(string avatarUrl)
    {
        AvatarUrl = avatarUrl ?? string.Empty;
        return this;
    }
}
