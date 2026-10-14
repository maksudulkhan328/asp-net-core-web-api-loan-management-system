using LoanManagement.Api.Data;
using LoanManagement.Api.DTOs;
using LoanManagement.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanManagement.Api.Services;

public interface ICustomerService
{
    Task<List<CustomerDto>> GetAllAsync();
    Task<CustomerDto?> GetByIdAsync(int id);
    Task<CustomerDto> CreateAsync(CreateCustomerDto dto);
    Task<CustomerDto?> UpdateAsync(int id, UpdateCustomerDto dto);
    Task<bool> DeleteAsync(int id);
}

public class CustomerService(AppDbContext db) : ICustomerService
{
    public async Task<List<CustomerDto>> GetAllAsync() =>
        await db.Customers.Select(c => new CustomerDto(c.Id, c.Name, c.Phone, c.Email, c.CreatedAt)).ToListAsync();

    public async Task<CustomerDto?> GetByIdAsync(int id)
    {
        var c = await db.Customers.FindAsync(id);
        return c is null ? null : new CustomerDto(c.Id, c.Name, c.Phone, c.Email, c.CreatedAt);
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto)
    {
        var c = new Customer { Name = dto.Name, Phone = dto.Phone, Email = dto.Email };
        db.Customers.Add(c);
        await db.SaveChangesAsync();
        return new CustomerDto(c.Id, c.Name, c.Phone, c.Email, c.CreatedAt);
    }

    public async Task<CustomerDto?> UpdateAsync(int id, UpdateCustomerDto dto)
    {
        var c = await db.Customers.FindAsync(id);
        if (c is null) return null;
        c.Name = dto.Name; c.Phone = dto.Phone;
        await db.SaveChangesAsync();
        return new CustomerDto(c.Id, c.Name, c.Phone, c.Email, c.CreatedAt);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var c = await db.Customers.FindAsync(id);
        if (c is null) return false;
        db.Customers.Remove(c);
        await db.SaveChangesAsync();
        return true;
    }
}
