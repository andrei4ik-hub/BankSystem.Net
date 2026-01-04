using CreditBureau.Core.Models;
using CreditBureau.Core.DTOs;

namespace CreditBureau.Services.Interfaces
{
    public interface ICreditHistoryService
    {
        Task<IEnumerable<CreditHistoryDto>> GetAllCreditHistoriesAsync();
        Task<CreditHistoryDto?> GetCreditHistoryByIdAsync(int id);
        Task<IEnumerable<CreditHistoryDto>> GetCreditHistoriesByBorrowerAsync(int borrowerId);
        Task<CreditHistory> CreateCreditHistoryAsync(CreateCreditHistoryDto createDto);
        Task<CreditHistory?> UpdateCreditHistoryAsync(int id, CreditHistory creditHistory);
        Task<bool> DeleteCreditHistoryAsync(int id);
    }
}