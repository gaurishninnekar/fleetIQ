using System;

namespace AuthService.Models.Dtos;

public class RegisterRequest
{
    public Guid TenantId { get; set; } 
    public string Email { get; set; } 
    public string Password { get; set; } 
    public string Role { get; set; } 
}
