using CreditBureau.Core.Interfaces;
using CreditBureau.Core.Models;
using CreditBureau.Services.Interfaces;

namespace CreditBureau.Services
{
    public class LenderService : ILenderService
    {
        private readonly ILenderRepository _lenderRepository;

        public LenderService(ILenderRepository lenderRepository)
        {
            _lenderRepository = lenderRepository;
        }

        public async Task<IEnumerable<Lender>> GetAllLendersAsync()
        {
            return await _lenderRepository.GetAllAsync();
        }

        public async Task<Lender?> GetLenderByIdAsync(int id)
        {
            return await _lenderRepository.GetByIdAsync(id);
        }

        public async Task<Lender> CreateLenderAsync(Lender lender)
        {
            ValidateLender(lender);
            return await _lenderRepository.AddAsync(lender);
        }

        public async Task<Lender?> UpdateLenderAsync(int id, Lender lender)
        {
            if (!await _lenderRepository.ExistsAsync(id))
                return null;

            lender.Id = id;
            return await _lenderRepository.UpdateAsync(lender);
        }

        public async Task<bool> DeleteLenderAsync(int id)
        {
            return await _lenderRepository.DeleteAsync(id);
        }

        private void ValidateLender(Lender lender)
        {
            if (string.IsNullOrWhiteSpace(lender.Name))
                throw new ArgumentException("Название кредитора обязательно");

            if (string.IsNullOrWhiteSpace(lender.LicenseNumber))
                throw new ArgumentException("Номер лицензии обязателен");
        }
    }
}