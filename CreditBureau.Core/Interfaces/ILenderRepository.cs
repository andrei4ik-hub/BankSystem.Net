using CreditBureau.Core.Models;

namespace CreditBureau.Core.Interfaces
{
    public interface ILenderRepository
    {
        Task<IEnumerable<Lender>> GetAllAsync();
        Task<Lender?> GetByIdAsync(int id);
        Task<Lender> AddAsync(Lender lender);
        Task<Lender?> UpdateAsync(Lender lender);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}