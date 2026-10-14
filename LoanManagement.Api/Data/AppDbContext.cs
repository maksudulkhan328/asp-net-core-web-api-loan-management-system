using LoanManagement.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanManagement.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<LoanApplication> LoanApplications => Set<LoanApplication>();
    public DbSet<Repayment> Repayments => Set<Repayment>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Customer>().HasIndex(c => c.Email).IsUnique();
        mb.Entity<LoanApplication>().Property(l => l.Amount).HasPrecision(18, 2);
        mb.Entity<LoanApplication>().Property(l => l.InterestRate).HasPrecision(5, 2);
        mb.Entity<LoanApplication>().Property(l => l.Emi).HasPrecision(18, 2);
        mb.Entity<LoanApplication>().Property(l => l.TotalInterest).HasPrecision(18, 2);
        mb.Entity<LoanApplication>().Property(l => l.AmountPaid).HasPrecision(18, 2);
        mb.Entity<Repayment>().Property(r => r.Amount).HasPrecision(18, 2);
    }
}
