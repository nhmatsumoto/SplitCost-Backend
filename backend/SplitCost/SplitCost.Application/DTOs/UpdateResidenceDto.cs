namespace SplitCost.Application.DTOs
{
    public class UpdateResidenceDto
    {
        public Guid ResidenceId { get; set; }
        public string Name { get; set; }
        public List<ResidenceMemberDto> Members { get; set; }
        public List<ResidenceExpenseDto> Expenses { get; set; }
    }
}
