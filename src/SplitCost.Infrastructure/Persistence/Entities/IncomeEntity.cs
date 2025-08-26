using SplitCost.Domain.Enums;

namespace SplitCost.Infrastructure.Persistence.Entities
{
    public class IncomeEntity
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public IncomeCategory Category { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid ResidenceId { get; set; }
        public Guid RegisteredByUserId { get; set; }
        public ResidenceEntity Residence { get; set; } = null!;
        public UserEntity RegisteredByUser { get; set; } = null!;
    }
}
