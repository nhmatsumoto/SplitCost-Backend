namespace SplitCost.Application.DTOs
{
    public class ResidenceMemberDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public bool IsPrimary { get; set; }
    }
}
