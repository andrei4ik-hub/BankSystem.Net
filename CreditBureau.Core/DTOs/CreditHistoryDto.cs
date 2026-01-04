namespace CreditBureau.Core.DTOs
{
    public class CreditHistoryDto
    {
        public int Id { get; set; }
        public int BorrowerId { get; set; }
        public int LenderId { get; set; }
        public string BorrowerName { get; set; } = string.Empty;
        public string LenderName { get; set; } = string.Empty;
        public decimal LoanAmount { get; set; }
        public decimal InterestRate { get; set; }
        public int LoanTermMonths { get; set; }
        public DateTime LoanIssueDate { get; set; }
        public DateTime? LoanEndDate { get; set; }
        public string Status { get; set; } = "Active";
        public decimal RemainingDebt { get; set; }
        public int DaysPastDue { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}