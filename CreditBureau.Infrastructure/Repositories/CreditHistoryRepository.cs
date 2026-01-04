using Microsoft.EntityFrameworkCore;
using CreditBureau.Core.Interfaces;
using CreditBureau.Core.Models;
using CreditBureau.Infrastructure.Data;

namespace CreditBureau.Infrastructure.Repositories
{
    public class CreditHistoryRepository : ICreditHistoryRepository
    {
        private readonly AppDbContext _context;

        public CreditHistoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CreditHistory>> GetAllAsync()
        {
         
            return await _context.CreditHistories
                .OrderByDescending(ch => ch.LoanIssueDate)
                .ToListAsync();
        }

        public async Task<CreditHistory?> GetByIdAsync(int id)
        {
            
            return await _context.CreditHistories
                .FirstOrDefaultAsync(ch => ch.Id == id);
        }

        public async Task<IEnumerable<CreditHistory>> GetByBorrowerIdAsync(int borrowerId)
        {
        
            return await _context.CreditHistories
                .Where(ch => ch.BorrowerId == borrowerId)
                .OrderByDescending(ch => ch.LoanIssueDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<CreditHistory>> GetByLenderIdAsync(int lenderId)
        {
           
            return await _context.CreditHistories
                .Where(ch => ch.LenderId == lenderId)
                .OrderByDescending(ch => ch.LoanIssueDate)
                .ToListAsync();
        }

        public async Task<CreditHistory> AddAsync(CreditHistory creditHistory)
        {
            creditHistory.CreatedAt = DateTime.UtcNow;
            _context.CreditHistories.Add(creditHistory);
            await _context.SaveChangesAsync();
            return creditHistory;
        }

        public async Task<CreditHistory?> UpdateAsync(CreditHistory creditHistory)
        {
            var existingHistory = await _context.CreditHistories
                .FirstOrDefaultAsync(ch => ch.Id == creditHistory.Id);
                
            if (existingHistory == null)
                return null;

            existingHistory.LoanAmount = creditHistory.LoanAmount;
            existingHistory.InterestRate = creditHistory.InterestRate;
            existingHistory.LoanTermMonths = creditHistory.LoanTermMonths;
            existingHistory.LoanIssueDate = creditHistory.LoanIssueDate;
            existingHistory.LoanEndDate = creditHistory.LoanEndDate;
            existingHistory.Status = creditHistory.Status;
            existingHistory.RemainingDebt = creditHistory.RemainingDebt;
            existingHistory.DaysPastDue = creditHistory.DaysPastDue;
            existingHistory.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingHistory;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var creditHistory = await _context.CreditHistories
                .FirstOrDefaultAsync(ch => ch.Id == id);
                
            if (creditHistory == null)
                return false;

            _context.CreditHistories.Remove(creditHistory);
            await _context.SaveChangesAsync();
            return true;
        }

        // ДОБАВЛЯЕМ метод для получения с связанными данными
        public async Task<CreditHistory?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.CreditHistories
                .Include(ch => ch.Borrower)
                .Include(ch => ch.Lender)
                .FirstOrDefaultAsync(ch => ch.Id == id);
        }
    }
}