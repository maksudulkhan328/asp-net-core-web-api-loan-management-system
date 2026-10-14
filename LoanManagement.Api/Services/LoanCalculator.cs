namespace LoanManagement.Api.Services;

public static class LoanCalculator
{
    public static decimal CalculateEmi(decimal principal, decimal annualRatePercent, int termMonths)
    {
        if (annualRatePercent == 0)
            return Math.Round(principal / termMonths, 2);

        double r = (double)annualRatePercent / 100 / 12;
        double power = Math.Pow(1 + r, termMonths);
        double emi = (double)principal * r * power / (power - 1);
        return Math.Round((decimal)emi, 2);
    }

    public static decimal CalculateTotalInterest(decimal emi, int termMonths, decimal principal)
        => Math.Round(emi * termMonths - principal, 2);
}
