using CreditBureau.Core.Interfaces;
using CreditBureau.Core.Models;

namespace CreditBureau.Infrastructure.Repositories
{
    public class CreditHistoryRepository : ICreditHistoryRepository
    {
        private readonly List<CreditHistory> _creditHistories = new();
        private int _nextId = 1;

        public Task<IEnumerable<CreditHistory>> GetAllAsync()
        {
            return Task.FromResult(_creditHistories.AsEnumerable());
        }

        public Task<CreditHistory?> GetByIdAsync(int id)
        {
            var history = _creditHistories.FirstOrDefault(ch => ch.Id == id);
            return Task.FromResult(history);
        }

        public Task<IEnumerable<CreditHistory>> GetByBorrowerIdAsync(int borrowerId)
        {
            var histories = _creditHistories.Where(ch => ch.BorrowerId == borrowerId);
            return Task.FromResult(histories);
        }

        public Task<IEnumerable<CreditHistory>> GetByLenderIdAsync(int lenderId)
        {
            var histories = _creditHistories.Where(ch => ch.LenderId == lenderId);
            return Task.FromResult(histories);
        }

        public Task<CreditHistory> AddAsync(CreditHistory creditHistory)
        {
            creditHistory.Id = _nextId++;
            creditHistory.CreatedAt = DateTime.UtcNow;
            _creditHistories.Add(creditHistory);
            return Task.FromResult(creditHistory);
        }

        public Task<CreditHistory?> UpdateAsync(CreditHistory creditHistory)
        {
            var existingHistory = _creditHistories.FirstOrDefault(ch => ch.Id == creditHistory.Id);
            if (existingHistory == null)
                return Task.FromResult<CreditHistory?>(null);

            existingHistory.LoanAmount = creditHistory.LoanAmount;
            existingHistory.InterestRate = creditHistory.InterestRate;
            existingHistory.LoanTermMonths = creditHistory.LoanTermMonths;
            existingHistory.LoanIssueDate = creditHistory.LoanIssueDate;
            existingHistory.LoanEndDate = creditHistory.LoanEndDate;
            existingHistory.Status = creditHistory.Status;
            existingHistory.RemainingDebt = creditHistory.RemainingDebt;
            existingHistory.DaysPastDue = creditHistory.DaysPastDue;
            existingHistory.UpdatedAt = DateTime.UtcNow;

            return Task.FromResult<CreditHistory?>(existingHistory);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var history = _creditHistories.FirstOrDefault(ch => ch.Id == id);
            if (history == null)
                return Task.FromResult(false);

            _creditHistories.Remove(history);
            return Task.FromResult(true);
        }
    }
}