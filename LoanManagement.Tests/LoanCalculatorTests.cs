using LoanManagement.Api.Services;
using Xunit;

namespace LoanManagement.Tests;

public class LoanCalculatorTests
{
    [Theory]
    [InlineData(100000, 12, 12, 8884.88)]
    [InlineData(500000, 10, 60, 10623.52)]
    [InlineData(200000, 8, 24, 9045.46)]
    public void CalculateEmi_ReturnsCorrectValue(decimal principal, decimal rate, int months, decimal expected)
    {
        var emi = LoanCalculator.CalculateEmi(principal, rate, months);
        Assert.Equal(expected, emi);
    }

    [Fact]
    public void CalculateEmi_ZeroInterest_ReturnsPrincipalDividedByTerm()
    {
        var emi = LoanCalculator.CalculateEmi(12000, 0, 12);
        Assert.Equal(1000, emi);
    }

    [Fact]
    public void CalculateTotalInterest_IsPositive()
    {
        var emi = LoanCalculator.CalculateEmi(100000, 12, 12);
        var interest = LoanCalculator.CalculateTotalInterest(emi, 12, 100000);
        Assert.True(interest > 0);
    }

    [Fact]
    public void CalculateTotalInterest_ZeroRate_IsZero()
    {
        var emi = LoanCalculator.CalculateEmi(12000, 0, 12);
        var interest = LoanCalculator.CalculateTotalInterest(emi, 12, 12000);
        Assert.Equal(0, interest);
    }

    [Theory]
    [InlineData(100000, 12, 12)]
    [InlineData(500000, 8, 60)]
    public void TotalPayable_GreaterThanPrincipal_WhenRatePositive(decimal principal, decimal rate, int months)
    {
        var emi = LoanCalculator.CalculateEmi(principal, rate, months);
        var totalPayable = emi * months;
        Assert.True(totalPayable > principal);
    }
}
