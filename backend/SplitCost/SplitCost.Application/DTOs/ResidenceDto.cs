namespace SplitCost.Application.DTOs
{
    public class ResidenceDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<ResidenceMemberDto> Members { get; set; }
        public List<CreateExpenseDto> Expenses { get; set; }
        public AddressDto Address { get; set; }

        public ResidenceDto()
        {
            Members = new List<ResidenceMemberDto>();
            Expenses = new List<CreateExpenseDto>();
        }
    }
}
