using CreditBureau.Core.Interfaces;
using CreditBureau.Core.Models;

namespace CreditBureau.Infrastructure.Repositories
{
    public class LenderRepository : ILenderRepository
    {
        private readonly List<Lender> _lenders = new();
        private int _nextId = 1;

        public Task<IEnumerable<Lender>> GetAllAsync()
        {
            return Task.FromResult(_lenders.AsEnumerable());
        }

        public Task<Lender?> GetByIdAsync(int id)
        {
            var lender = _lenders.FirstOrDefault(l => l.Id == id);
            return Task.FromResult(lender);
        }

        public Task<Lender> AddAsync(Lender lender)
        {
            lender.Id = _nextId++;
            lender.CreatedAt = DateTime.UtcNow;
            _lenders.Add(lender);
            return Task.FromResult(lender);
        }

        public Task<Lender?> UpdateAsync(Lender lender)
        {
            var existingLender = _lenders.FirstOrDefault(l => l.Id == lender.Id);
            if (existingLender == null)
                return Task.FromResult<Lender?>(null);

            existingLender.Name = lender.Name;
            existingLender.LicenseNumber = lender.LicenseNumber;
            existingLender.Address = lender.Address;
            existingLender.PhoneNumber = lender.PhoneNumber;
            existingLender.Email = lender.Email;
            existingLender.UpdatedAt = DateTime.UtcNow;

            return Task.FromResult<Lender?>(existingLender);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var lender = _lenders.FirstOrDefault(l => l.Id == id);
            if (lender == null)
                return Task.FromResult(false);

            _lenders.Remove(lender);
            return Task.FromResult(true);
        }

        public Task<bool> ExistsAsync(int id)
        {
            return Task.FromResult(_lenders.Any(l => l.Id == id));
        }
    }
}