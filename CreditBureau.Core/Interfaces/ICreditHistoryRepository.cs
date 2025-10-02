using CreditBureau.Core.Models;

namespace CreditBureau.Core.Interfaces
{
    public interface ICreditHistoryRepository
    {
        Task<IEnumerable<CreditHistory>> GetAllAsync();
        Task<CreditHistory?> GetByIdAsync(int id);
        Task<IEnumerable<CreditHistory>> GetByBorrowerIdAsync(int borrowerId);
        Task<IEnumerable<CreditHistory>> GetByLenderIdAsync(int lenderId);
        Task<CreditHistory> AddAsync(CreditHistory creditHistory);
        Task<CreditHistory?> UpdateAsync(CreditHistory creditHistory);
        Task<bool> DeleteAsync(int id);
    }
}