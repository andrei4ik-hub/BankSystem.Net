using CreditBureau.Core.Models;

namespace CreditBureau.Services.Interfaces
{
    public interface IBorrowerService
    {
        Task<IEnumerable<Borrower>> GetAllBorrowersAsync();
        Task<Borrower?> GetBorrowerByIdAsync(int id);
        Task<Borrower> CreateBorrowerAsync(Borrower borrower);
        Task<Borrower?> UpdateBorrowerAsync(int id, Borrower borrower);
        Task<bool> DeleteBorrowerAsync(int id);
    }
}