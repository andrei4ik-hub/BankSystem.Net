using CreditBureau.Core.Interfaces;
using CreditBureau.Core.Models;
using CreditBureau.Services.Interfaces;

namespace CreditBureau.Services
{
    public class CreditHistoryService : ICreditHistoryService
    {
        private readonly ICreditHistoryRepository _creditHistoryRepository;
        private readonly IBorrowerRepository _borrowerRepository;
        private readonly ILenderRepository _lenderRepository;

        public CreditHistoryService(
            ICreditHistoryRepository creditHistoryRepository,
            IBorrowerRepository borrowerRepository,
            ILenderRepository lenderRepository)
        {
            _creditHistoryRepository = creditHistoryRepository;
            _borrowerRepository = borrowerRepository;
            _lenderRepository = lenderRepository;
        }

        public async Task<IEnumerable<CreditHistory>> GetAllCreditHistoriesAsync()
        {
            return await _creditHistoryRepository.GetAllAsync();
        }

        public async Task<CreditHistory?> GetCreditHistoryByIdAsync(int id)
        {
            return await _creditHistoryRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<CreditHistory>> GetCreditHistoriesByBorrowerAsync(int borrowerId)
        {
            return await _creditHistoryRepository.GetByBorrowerIdAsync(borrowerId);
        }

        public async Task<CreditHistory> CreateCreditHistoryAsync(CreditHistory creditHistory)
        {
            ValidateCreditHistory(creditHistory);
            return await _creditHistoryRepository.AddAsync(creditHistory);
        }

        public async Task<CreditHistory?> UpdateCreditHistoryAsync(int id, CreditHistory creditHistory)
        {
            // ИСПРАВЛЕННАЯ СТРОКА - была неправильная проверка
            var existingHistory = await _creditHistoryRepository.GetByIdAsync(id);
            if (existingHistory == null)
                return null;

            creditHistory.Id = id;
            return await _creditHistoryRepository.UpdateAsync(creditHistory);
        }

        public async Task<bool> DeleteCreditHistoryAsync(int id)
        {
            return await _creditHistoryRepository.DeleteAsync(id);
        }

        private void ValidateCreditHistory(CreditHistory creditHistory)
        {
            if (creditHistory.LoanAmount <= 0)
                throw new ArgumentException("Сумма кредита должна быть положительной");

            if (creditHistory.InterestRate <= 0)
                throw new ArgumentException("Процентная ставка должна быть положительной");

            if (creditHistory.LoanTermMonths <= 0)
                throw new ArgumentException("Срок кредита должен быть положительным");

            // Можно добавить проверки существования заемщика и кредитора
            // if (!await _borrowerRepository.ExistsAsync(creditHistory.BorrowerId))
            //     throw new ArgumentException("Заемщик не найден");
            
            // if (!await _lenderRepository.ExistsAsync(creditHistory.LenderId))
            //     throw new ArgumentException("Кредитор не найден");
        }
    }
}