namespace CreditBureau.Core.Models
{
    public class Borrower
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string PassportNumber { get; set; } = string.Empty;
        public string PassportSeries { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

    
        public ICollection<CreditHistory> CreditHistories { get; set; } = new List<CreditHistory>();
    }
}