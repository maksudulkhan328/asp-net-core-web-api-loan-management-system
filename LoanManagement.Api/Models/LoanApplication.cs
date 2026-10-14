namespace LoanManagement.Api.Models;

public enum LoanStatus { Pending, Approved, Rejected, Active, Completed }

public class LoanApplication
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public decimal Amount { get; set; }
    public decimal InterestRate { get; set; }   // annual %
    public int TermMonths { get; set; }
    public LoanStatus Status { get; set; } = LoanStatus.Pending;
    public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;
    public DateTime? ApprovedDate { get; set; }
    public decimal Emi { get; set; }
    public decimal TotalInterest { get; set; }
    public decimal AmountPaid { get; set; }
    public ICollection<Repayment> Repayments { get; set; } = new List<Repayment>();
}

public class Repayment
{
    public int Id { get; set; }
    public int LoanApplicationId { get; set; }
    public LoanApplication LoanApplication { get; set; } = null!;
    public decimal Amount { get; set; }
    public DateTime PaidAt { get; set; } = DateTime.UtcNow;
    public string? Note { get; set; }
}
