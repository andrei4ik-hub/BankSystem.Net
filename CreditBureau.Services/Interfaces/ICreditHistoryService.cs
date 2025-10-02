using CreditBureau.Core.Models;

namespace CreditBureau.Services.Interfaces
{
    public interface ICreditHistoryService
    {
        Task<IEnumerable<CreditHistory>> GetAllCreditHistoriesAsync();
        Task<CreditHistory?> GetCreditHistoryByIdAsync(int id);
        Task<IEnumerable<CreditHistory>> GetCreditHistoriesByBorrowerAsync(int borrowerId);
        Task<CreditHistory> CreateCreditHistoryAsync(CreditHistory creditHistory);
        Task<CreditHistory?> UpdateCreditHistoryAsync(int id, CreditHistory creditHistory);
        Task<bool> DeleteCreditHistoryAsync(int id);
    }
}