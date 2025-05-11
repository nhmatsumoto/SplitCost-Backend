namespace SplitCost.Domain.Entities
{
    public class ResidenceMember
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public Guid ResidenceId { get; set; }
        public Residence Residence { get; set; } = null!;

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }

}
