namespace CreditBureau.Core.Models
{
    public class CreditHistory
    {
        public int Id { get; set; }
        public int BorrowerId { get; set; }
        public int LenderId { get; set; }
        
        public decimal LoanAmount { get; set; }
        public decimal InterestRate { get; set; }
        public int LoanTermMonths { get; set; }
        public DateTime LoanIssueDate { get; set; }
        public DateTime? LoanEndDate { get; set; }
        
        public string Status { get; set; } = "Active";
        public decimal RemainingDebt { get; set; }
        public int DaysPastDue { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Навигационные свойства
        public Borrower Borrower { get; set; } = null!;
        public Lender Lender { get; set; } = null!;
    }
}