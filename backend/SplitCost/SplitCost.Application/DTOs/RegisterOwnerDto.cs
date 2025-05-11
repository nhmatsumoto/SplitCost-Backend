namespace SplitCost.Application.DTOs
{
    public class RegisterOwnerDto
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public Guid ResidenceId { get; set; }
    }
}
