using Microsoft.EntityFrameworkCore;
using CreditBureau.Core.Interfaces;
using CreditBureau.Core.Models;
using CreditBureau.Infrastructure.Data;

namespace CreditBureau.Infrastructure.Repositories
{
    public class LenderRepository : ILenderRepository
    {
        private readonly AppDbContext _context;

        public LenderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Lender>> GetAllAsync()
        {
            return await _context.Lenders
                .OrderBy(l => l.Name)
                .ToListAsync();
        }

        public async Task<Lender?> GetByIdAsync(int id)
        {
            return await _context.Lenders
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Lender> AddAsync(Lender lender)
        {
            lender.CreatedAt = DateTime.UtcNow;
            _context.Lenders.Add(lender);
            await _context.SaveChangesAsync();
            return lender;
        }

        public async Task<Lender?> UpdateAsync(Lender lender)
        {
            var existingLender = await _context.Lenders
                .FirstOrDefaultAsync(l => l.Id == lender.Id);
                
            if (existingLender == null)
                return null;

            existingLender.Name = lender.Name;
            existingLender.LicenseNumber = lender.LicenseNumber;
            existingLender.Address = lender.Address;
            existingLender.PhoneNumber = lender.PhoneNumber;
            existingLender.Email = lender.Email;
            existingLender.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingLender;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var lender = await _context.Lenders
                .FirstOrDefaultAsync(l => l.Id == id);
                
            if (lender == null)
                return false;

            _context.Lenders.Remove(lender);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Lenders
                .AnyAsync(l => l.Id == id);
        }
    }
}