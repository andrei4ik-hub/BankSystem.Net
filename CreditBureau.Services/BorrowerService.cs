using CreditBureau.Core.Interfaces;
using CreditBureau.Core.Models;
using CreditBureau.Services.Interfaces;

namespace CreditBureau.Services
{
    public class BorrowerService : IBorrowerService
    {
        private readonly IBorrowerRepository _borrowerRepository;

        public BorrowerService(IBorrowerRepository borrowerRepository)
        {
            _borrowerRepository = borrowerRepository;
        }

        public async Task<IEnumerable<Borrower>> GetAllBorrowersAsync()
        {
            return await _borrowerRepository.GetAllAsync();
        }

        public async Task<Borrower?> GetBorrowerByIdAsync(int id)
        {
            return await _borrowerRepository.GetByIdAsync(id);
        }

        public async Task<Borrower> CreateBorrowerAsync(Borrower borrower)
        {
            ValidateBorrower(borrower);
            return await _borrowerRepository.AddAsync(borrower);
        }

        public async Task<Borrower?> UpdateBorrowerAsync(int id, Borrower borrower)
        {
            if (!await _borrowerRepository.ExistsAsync(id))
                return null;

            borrower.Id = id;
            return await _borrowerRepository.UpdateAsync(borrower);
        }

        public async Task<bool> DeleteBorrowerAsync(int id)
        {
            return await _borrowerRepository.DeleteAsync(id);
        }

        private void ValidateBorrower(Borrower borrower)
        {
            if (string.IsNullOrWhiteSpace(borrower.FirstName))
                throw new ArgumentException("Имя обязательно для заполнения");

            if (string.IsNullOrWhiteSpace(borrower.LastName))
                throw new ArgumentException("Фамилия обязательна для заполнения");

            if (string.IsNullOrWhiteSpace(borrower.PassportNumber))
                throw new ArgumentException("Номер паспорта обязателен для заполнения");
        }
    }
}