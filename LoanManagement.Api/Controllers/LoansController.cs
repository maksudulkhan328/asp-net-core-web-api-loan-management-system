using LoanManagement.Api.DTOs;
using LoanManagement.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoansController(ILoanService svc) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await svc.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var l = await svc.GetByIdAsync(id);
        return l is null ? NotFound() : Ok(l);
    }

    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetByCustomer(int customerId)
        => Ok(await svc.GetByCustomerAsync(customerId));

    [HttpPost]
    public async Task<IActionResult> Create(CreateLoanDto dto)
    {
        var l = await svc.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = l.Id }, l);
    }

    [HttpPost("{id}/approve")]
    public async Task<IActionResult> Approve(int id, ApproveLoanDto dto)
    {
        var l = await svc.ApproveAsync(id, dto);
        return l is null ? BadRequest("Loan not found or not in Pending status.") : Ok(l);
    }

    [HttpPost("{id}/reject")]
    public async Task<IActionResult> Reject(int id)
    {
        var l = await svc.RejectAsync(id);
        return l is null ? BadRequest("Loan not found or not in Pending status.") : Ok(l);
    }

    [HttpPost("{id}/repayments")]
    public async Task<IActionResult> AddRepayment(int id, AddRepaymentDto dto)
    {
        var (loan, error) = await svc.AddRepaymentAsync(id, dto);
        if (loan is null && error is null) return NotFound();
        if (error is not null) return BadRequest(error);
        return Ok(loan);
    }

    [HttpGet("{id}/repayments")]
    public async Task<IActionResult> GetRepayments(int id)
        => Ok(await svc.GetRepaymentsAsync(id));
}
