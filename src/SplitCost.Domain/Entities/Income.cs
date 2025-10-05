using SplitCost.Domain.Common;
using SplitCost.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SplitCost.Domain.Entities
{
    public class Income : BaseTenantEntity
    {
        public Guid Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public IncomeCategory Category { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        // UUID do usuário do Keycloak
        [Required]
        public Guid UserId { get; set; }

        // Relação com residência (tenant)
        public Residence Residence { get; set; } = null!;

        internal Income() { }

        internal Income(
            decimal amount,
            IncomeCategory category,
            DateTime date,
            Guid residenceId,
            Guid registeredByUserId,
            string description = ""
        )
        {
            SetAmount(amount)
                .SetCategory(category)
                .SetDate(date)
                .SetDescription(description)
                .SetResidenceId(residenceId)
                .SetUserId(registeredByUserId);
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
