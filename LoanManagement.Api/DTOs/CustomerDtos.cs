using System.ComponentModel.DataAnnotations;

namespace LoanManagement.Api.DTOs;

public record CreateCustomerDto(
    [Required, MaxLength(100)] string Name,
    [Required, Phone] string Phone,
    [Required, EmailAddress] string Email
);

public record UpdateCustomerDto(
    [Required, MaxLength(100)] string Name,
    [Required, Phone] string Phone
);

public record CustomerDto(int Id, string Name, string Phone, string Email, DateTime CreatedAt);
