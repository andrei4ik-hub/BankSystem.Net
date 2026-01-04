using Microsoft.EntityFrameworkCore;
using CreditBureau.Core.Interfaces;
using CreditBureau.Core.Models;
using CreditBureau.Core.DTOs;
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

        public async Task<IEnumerable<CreditHistoryDto>> GetAllCreditHistoriesAsync()
        {
            var histories = await _creditHistoryRepository.GetAllAsync();
            var result = new List<CreditHistoryDto>();

            foreach (var history in histories)
            {
               
                var borrower = await _borrowerRepository.GetByIdAsync(history.BorrowerId);
                var lender = await _lenderRepository.GetByIdAsync(history.LenderId);

                result.Add(new CreditHistoryDto
                {
                    Id = history.Id,
                    BorrowerId = history.BorrowerId,
                    LenderId = history.LenderId,
                    BorrowerName = borrower != null ? $"{borrower.LastName} {borrower.FirstName}" : "Неизвестно",
                    LenderName = lender?.Name ?? "Неизвестно",
                    LoanAmount = history.LoanAmount,
                    InterestRate = history.InterestRate,
                    LoanTermMonths = history.LoanTermMonths,
                    LoanIssueDate = history.LoanIssueDate,
                    LoanEndDate = history.LoanEndDate,
                    Status = history.Status,
                    RemainingDebt = history.RemainingDebt,
                    DaysPastDue = history.DaysPastDue,
                    CreatedAt = history.CreatedAt,
                    UpdatedAt = history.UpdatedAt
                });
            }

            return result;
        }

        public async Task<CreditHistoryDto?> GetCreditHistoryByIdAsync(int id)
        {
            var history = await _creditHistoryRepository.GetByIdAsync(id);
            if (history == null) return null;

            
            var borrower = await _borrowerRepository.GetByIdAsync(history.BorrowerId);
            var lender = await _lenderRepository.GetByIdAsync(history.LenderId);

            return new CreditHistoryDto
            {
                Id = history.Id,
                BorrowerId = history.BorrowerId,
                LenderId = history.LenderId,
                BorrowerName = borrower != null ? $"{borrower.LastName} {borrower.FirstName}" : "Неизвестно",
                LenderName = lender?.Name ?? "Неизвестно",
                LoanAmount = history.LoanAmount,
                InterestRate = history.InterestRate,
                LoanTermMonths = history.LoanTermMonths,
                LoanIssueDate = history.LoanIssueDate,
                LoanEndDate = history.LoanEndDate,
                Status = history.Status,
                RemainingDebt = history.RemainingDebt,
                DaysPastDue = history.DaysPastDue,
                CreatedAt = history.CreatedAt,
                UpdatedAt = history.UpdatedAt
            };
        }

        public async Task<IEnumerable<CreditHistoryDto>> GetCreditHistoriesByBorrowerAsync(int borrowerId)
        {
            var histories = await _creditHistoryRepository.GetByBorrowerIdAsync(borrowerId);
            var result = new List<CreditHistoryDto>();

            // Получаем данные кредитора один раз
            var lenderCache = new Dictionary<int, string>();

            foreach (var history in histories)
            {
                if (!lenderCache.ContainsKey(history.LenderId))
                {
                    var lender = await _lenderRepository.GetByIdAsync(history.LenderId);
                    lenderCache[history.LenderId] = lender?.Name ?? "Неизвестно";
                }

                result.Add(new CreditHistoryDto
                {
                    Id = history.Id,
                    BorrowerId = history.BorrowerId,
                    LenderId = history.LenderId,
                    BorrowerName = "", // 
                    LenderName = lenderCache[history.LenderId],
                    LoanAmount = history.LoanAmount,
                    InterestRate = history.InterestRate,
                    LoanTermMonths = history.LoanTermMonths,
                    LoanIssueDate = history.LoanIssueDate,
                    LoanEndDate = history.LoanEndDate,
                    Status = history.Status,
                    RemainingDebt = history.RemainingDebt,
                    DaysPastDue = history.DaysPastDue,
                    CreatedAt = history.CreatedAt,
                    UpdatedAt = history.UpdatedAt
                });
            }

            return result;
        }

        public async Task<CreditHistory> CreateCreditHistoryAsync(CreateCreditHistoryDto createDto)
        {
            ValidateCreditHistory(createDto);

        
            var borrower = await _borrowerRepository.GetByIdAsync(createDto.BorrowerId);
            if (borrower == null)
                throw new ArgumentException("Заемщик не найден");

            var lender = await _lenderRepository.GetByIdAsync(createDto.LenderId);
            if (lender == null)
                throw new ArgumentException("Кредитор не найден");

            var creditHistory = new CreditHistory
            {
                BorrowerId = createDto.BorrowerId,
                LenderId = createDto.LenderId,
                LoanAmount = createDto.LoanAmount,
                InterestRate = createDto.InterestRate,
                LoanTermMonths = createDto.LoanTermMonths,
                LoanIssueDate = createDto.LoanIssueDate,
                LoanEndDate = createDto.LoanEndDate,
                Status = createDto.Status,
                RemainingDebt = createDto.RemainingDebt,
                DaysPastDue = createDto.DaysPastDue
            };

            return await _creditHistoryRepository.AddAsync(creditHistory);
        }

        public async Task<CreditHistory?> UpdateCreditHistoryAsync(int id, CreditHistory creditHistory)
        {
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

        private void ValidateCreditHistory(CreateCreditHistoryDto creditHistory)
        {
            if (creditHistory.LoanAmount <= 0)
                throw new ArgumentException("Сумма кредита должна быть положительной");

            if (creditHistory.InterestRate <= 0)
                throw new ArgumentException("Процентная ставка должна быть положительной");

            if (creditHistory.LoanTermMonths <= 0)
                throw new ArgumentException("Срок кредита должен быть положительным");
        }
    }
}