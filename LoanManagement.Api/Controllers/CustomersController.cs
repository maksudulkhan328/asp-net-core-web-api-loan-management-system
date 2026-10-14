using LoanManagement.Api.DTOs;
using LoanManagement.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController(ICustomerService svc) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await svc.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var c = await svc.GetByIdAsync(id);
        return c is null ? NotFound() : Ok(c);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCustomerDto dto)
    {
        var c = await svc.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = c.Id }, c);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateCustomerDto dto)
    {
        var c = await svc.UpdateAsync(id, dto);
        return c is null ? NotFound() : Ok(c);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
        => await svc.DeleteAsync(id) ? NoContent() : NotFound();
}
