namespace CreditBureau.Core.Models
{
    public class CreditScore
    {
        public int Id { get; set; }
        public int BorrowerId { get; set; }
        
        public int Score { get; set; } // 300-850
        public string Rating { get; set; } = string.Empty; // Poor, Fair, Good, Excellent
        public DateTime CalculationDate { get; set; } = DateTime.UtcNow;
        public string Factors { get; set; } = string.Empty; 
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Borrower Borrower { get; set; } = null!;
    }
}