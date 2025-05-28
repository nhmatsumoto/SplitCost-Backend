namespace SplitCost.Application.DTOs
{
    public class UpdateResidenceDto
    {
        public Guid ResidenceId { get; set; }
        public string Name { get; set; }
        public List<CreateResidenceMemberDto> Members { get; set; }
        public List<CreateExpenseDto> Expenses { get; set; }
    }
}
