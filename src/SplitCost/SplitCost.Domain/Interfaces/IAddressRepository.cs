using SplitCost.Domain.Entities;

namespace SplitCost.Domain.Interfaces
{
    public interface IAddressRepository
    {
        Task AddAsync(Address address);
        Task<Address?> GetByIdAsync(Guid id);
        Task UpdateAsync(Address address);
    }
}
