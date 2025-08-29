using SplitCost.Domain.Common;
using SplitCost.Domain.Enums;

namespace SplitCost.Domain.Entities
{
    public class Income : BaseEntity
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public IncomeCategory Category { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid ResidenceId { get; set; }
        public Guid UserId { get; set; }
        public Residence Residence { get; set; } = null!;
        public User User { get; set; } = null!;

        internal Income() { }

        public Income(
            decimal amount,
            IncomeCategory category,
            DateTime date,
            Guid residenceId,
            Guid registeredByUserId,
            string description = ""
            ) : this(amount, category, date, description, residenceId, registeredByUserId)
        {
        }

        internal Income(
            decimal amount,
            IncomeCategory category,
            DateTime date,
            string description,
            Guid residenceId,
            Guid registeredByUserId
            )
        {
            SetAmount(amount);
            SetCategory(category);
            SetDate(date);
            SetDescription(description);
            SetResidenceId(residenceId);
            SetUserId(registeredByUserId);
        }

        public Income SetId(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentException("Id inválido.");
            Id = id;
            return this;
        }

        public Income SetAmount(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("O valor da renda deve ser maior que zero.");
            Amount = amount;
            return this;
        }

        public Income SetCategory(IncomeCategory category)
        {
            Category = category;
            return this;
        }

        public Income SetDate(DateTime date)
        {
            if (date == default) throw new ArgumentException("Data inválida.");
            Date = date;
            return this;
        }

        public Income SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Descrição não pode ser vazia.");
            Description = description.Trim();
            return this;
        }

        public Income SetResidenceId(Guid residenceId)
        {
            if (residenceId == Guid.Empty) throw new ArgumentException("Residência inválida.");
            ResidenceId = residenceId;
            return this;
        }

        public Income SetUserId(Guid userId)
        {
            if (userId == Guid.Empty) throw new ArgumentException("Usuário inválido.");
            UserId = userId;
            return this;
        }
    }
}