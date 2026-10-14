using LoanManagement.Api.Data;
using LoanManagement.Api.DTOs;
using LoanManagement.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanManagement.Api.Services;

public interface ILoanService
{
    Task<List<LoanDto>> GetAllAsync();
    Task<LoanDto?> GetByIdAsync(int id);
    Task<List<LoanDto>> GetByCustomerAsync(int customerId);
    Task<LoanDto> CreateAsync(CreateLoanDto dto);
    Task<LoanDto?> ApproveAsync(int id, ApproveLoanDto dto);
    Task<LoanDto?> RejectAsync(int id);
    Task<(LoanDto? loan, string? error)> AddRepaymentAsync(int id, AddRepaymentDto dto);
    Task<List<RepaymentDto>> GetRepaymentsAsync(int id);
}

public class LoanService(AppDbContext db) : ILoanService
{
    public async Task<List<LoanDto>> GetAllAsync() =>
        await db.LoanApplications.Include(l => l.Customer).Select(l => ToDto(l)).ToListAsync();

    public async Task<LoanDto?> GetByIdAsync(int id)
    {
        var l = await db.LoanApplications.Include(l => l.Customer).FirstOrDefaultAsync(l => l.Id == id);
        return l is null ? null : ToDto(l);
    }

    public async Task<List<LoanDto>> GetByCustomerAsync(int customerId) =>
        await db.LoanApplications.Include(l => l.Customer)
            .Where(l => l.CustomerId == customerId).Select(l => ToDto(l)).ToListAsync();

    public async Task<LoanDto> CreateAsync(CreateLoanDto dto)
    {
        var emi = LoanCalculator.CalculateEmi(dto.Amount, dto.InterestRate, dto.TermMonths);
        var loan = new LoanApplication
        {
            CustomerId = dto.CustomerId,
            Amount = dto.Amount,
            InterestRate = dto.InterestRate,
            TermMonths = dto.TermMonths,
            Emi = emi,
            TotalInterest = LoanCalculator.CalculateTotalInterest(emi, dto.TermMonths, dto.Amount)
        };
        db.LoanApplications.Add(loan);
        await db.SaveChangesAsync();
        await db.Entry(loan).Reference(l => l.Customer).LoadAsync();
        return ToDto(loan);
    }

    public async Task<LoanDto?> ApproveAsync(int id, ApproveLoanDto dto)
    {
        var loan = await db.LoanApplications.Include(l => l.Customer).FirstOrDefaultAsync(l => l.Id == id);
        if (loan is null || loan.Status != LoanStatus.Pending) return null;
        loan.Status = LoanStatus.Active;
        loan.ApprovedDate = dto.ApprovedDate ?? DateTime.UtcNow;
        await db.SaveChangesAsync();
        return ToDto(loan);
    }

    public async Task<LoanDto?> RejectAsync(int id)
    {
        var loan = await db.LoanApplications.Include(l => l.Customer).FirstOrDefaultAsync(l => l.Id == id);
        if (loan is null || loan.Status != LoanStatus.Pending) return null;
        loan.Status = LoanStatus.Rejected;
        await db.SaveChangesAsync();
        return ToDto(loan);
    }

    public async Task<(LoanDto? loan, string? error)> AddRepaymentAsync(int id, AddRepaymentDto dto)
    {
        var loan = await db.LoanApplications.Include(l => l.Customer).FirstOrDefaultAsync(l => l.Id == id);
        if (loan is null) return (null, null);
        if (loan.Status != LoanStatus.Active) return (null, "Loan is not active.");

        var remaining = loan.Amount + loan.TotalInterest - loan.AmountPaid;
        if (dto.Amount > remaining) return (null, $"Payment exceeds remaining balance of {remaining:F2}.");

        db.Repayments.Add(new Repayment { LoanApplicationId = id, Amount = dto.Amount, Note = dto.Note });
        loan.AmountPaid += dto.Amount;

        if (loan.AmountPaid >= loan.Amount + loan.TotalInterest)
            loan.Status = LoanStatus.Completed;

        await db.SaveChangesAsync();
        return (ToDto(loan), null);
    }

    public async Task<List<RepaymentDto>> GetRepaymentsAsync(int id) =>
        await db.Repayments.Where(r => r.LoanApplicationId == id)
            .Select(r => new RepaymentDto(r.Id, r.Amount, r.PaidAt, r.Note)).ToListAsync();

    private static LoanDto ToDto(LoanApplication l) => new(
        l.Id, l.CustomerId, l.Customer?.Name ?? "",
        l.Amount, l.InterestRate, l.TermMonths,
        l.Status, l.ApplicationDate, l.ApprovedDate,
        l.Emi, l.TotalInterest, l.AmountPaid,
        Math.Max(0, l.Amount + l.TotalInterest - l.AmountPaid)
    );
}
