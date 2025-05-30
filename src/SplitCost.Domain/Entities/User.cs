using SplitCost.Domain.Common;

namespace SplitCost.Domain.Entities
{
    public class User : BaseEntity
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string AvatarUrl { get; private set; }

        private readonly List<Member> _residences = new();
        public IReadOnlyCollection<Member> Residences => _residences.AsReadOnly();

        private readonly List<Expense> _expenses = new();
        public IReadOnlyCollection<Expense> Expenses => _expenses.AsReadOnly();

        private readonly List<Expense> _residenceExpensesPaid = new();
        public IReadOnlyCollection<Expense> ResidenceExpensesPaid => _residenceExpensesPaid.AsReadOnly();

        private readonly List<ExpenseShare> _expenseShares = new();
        public IReadOnlyCollection<ExpenseShare> ExpenseShares => _expenseShares.AsReadOnly();

        internal User(Guid id, string name, string email, string avatarUrl = "")
        {
            SetId(id);
            SetName(name);
            SetEmail(email);
            SetAvatarUrl(avatarUrl);
        }

        // Apenas para frameworks de serialização/deserialização
        protected User() { }

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

        // Métodos para manipular coleções podem ser expostos aqui se houver lógica de negócio relevante
        public void AddResidence(Member member)
        {
            if (member == null) throw new ArgumentNullException(nameof(member));
            _residences.Add(member);
        }

        public void AddExpense(Expense expense)
        {
            if (expense == null) throw new ArgumentNullException(nameof(expense));
            _expenses.Add(expense);
        }

        public void AddResidenceExpensePaid(Expense expense)
        {
            if (expense == null) throw new ArgumentNullException(nameof(expense));
            _residenceExpensesPaid.Add(expense);
        }

        public void AddExpenseShare(ExpenseShare expenseShare)
        {
            if (expenseShare == null) throw new ArgumentNullException(nameof(expenseShare));
            _expenseShares.Add(expenseShare);
        }
    }
}
