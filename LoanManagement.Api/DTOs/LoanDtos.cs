using System.ComponentModel.DataAnnotations;
using LoanManagement.Api.Models;

namespace LoanManagement.Api.DTOs;

public record CreateLoanDto(
    [Required] int CustomerId,
    [Required, Range(1000, 10_000_000)] decimal Amount,
    [Required, Range(1, 36)] decimal InterestRate,
    [Required, Range(1, 360)] int TermMonths
);

public record ApproveLoanDto(DateTime? ApprovedDate);

public record AddRepaymentDto(
    [Required, Range(1, double.MaxValue)] decimal Amount,
    string? Note
);

public record LoanDto(
    int Id, int CustomerId, string CustomerName,
    decimal Amount, decimal InterestRate, int TermMonths,
    LoanStatus Status, DateTime ApplicationDate, DateTime? ApprovedDate,
    decimal Emi, decimal TotalInterest, decimal AmountPaid, decimal RemainingBalance
);

public record RepaymentDto(int Id, decimal Amount, DateTime PaidAt, string? Note);
