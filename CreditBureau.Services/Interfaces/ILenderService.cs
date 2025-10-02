using CreditBureau.Core.Models;

namespace CreditBureau.Services.Interfaces
{
    public interface ILenderService
    {
        Task<IEnumerable<Lender>> GetAllLendersAsync();
        Task<Lender?> GetLenderByIdAsync(int id);
        Task<Lender> CreateLenderAsync(Lender lender);
        Task<Lender?> UpdateLenderAsync(int id, Lender lender);
        Task<bool> DeleteLenderAsync(int id);
    }
}