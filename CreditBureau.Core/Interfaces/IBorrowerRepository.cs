using CreditBureau.Core.Models;

namespace CreditBureau.Core.Interfaces
{
    public interface IBorrowerRepository
    {
        Task<IEnumerable<Borrower>> GetAllAsync();
        Task<Borrower?> GetByIdAsync(int id);
        Task<Borrower> AddAsync(Borrower borrower);
        Task<Borrower?> UpdateAsync(Borrower borrower);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}