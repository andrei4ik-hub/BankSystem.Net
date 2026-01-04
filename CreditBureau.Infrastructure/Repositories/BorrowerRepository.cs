using Microsoft.EntityFrameworkCore;
using CreditBureau.Core.Interfaces;
using CreditBureau.Core.Models;
using CreditBureau.Infrastructure.Data;

namespace CreditBureau.Infrastructure.Repositories
{
    public class BorrowerRepository : IBorrowerRepository
    {
        private readonly AppDbContext _context;

        public BorrowerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Borrower>> GetAllAsync()
        {
            return await _context.Borrowers
                .OrderBy(b => b.LastName)
                .ThenBy(b => b.FirstName)
                .ToListAsync();
        }

        public async Task<Borrower?> GetByIdAsync(int id)
        {
            return await _context.Borrowers
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Borrower> AddAsync(Borrower borrower)
        {
            borrower.CreatedAt = DateTime.UtcNow;
            _context.Borrowers.Add(borrower);
            await _context.SaveChangesAsync();
            return borrower;
        }

        public async Task<Borrower?> UpdateAsync(Borrower borrower)
        {
            var existingBorrower = await _context.Borrowers
                .FirstOrDefaultAsync(b => b.Id == borrower.Id);
                
            if (existingBorrower == null)
                return null;

            existingBorrower.FirstName = borrower.FirstName;
            existingBorrower.LastName = borrower.LastName;
            existingBorrower.MiddleName = borrower.MiddleName;
            existingBorrower.PassportSeries = borrower.PassportSeries;
            existingBorrower.PassportNumber = borrower.PassportNumber;
            existingBorrower.BirthDate = borrower.BirthDate;
            existingBorrower.PhoneNumber = borrower.PhoneNumber;
            existingBorrower.Email = borrower.Email;
            existingBorrower.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingBorrower;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var borrower = await _context.Borrowers
                .FirstOrDefaultAsync(b => b.Id == id);
                
            if (borrower == null)
                return false;

            _context.Borrowers.Remove(borrower);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Borrowers
                .AnyAsync(b => b.Id == id);
        }

        public async Task<Borrower?> GetByPassportAsync(string series, string number)
        {
            return await _context.Borrowers
                .FirstOrDefaultAsync(b => 
                    b.PassportSeries == series && 
                    b.PassportNumber == number);
        }
    }
}