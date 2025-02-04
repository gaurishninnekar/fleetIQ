using System;

namespace AuthService.Models.Dtos;

public class LoginRequest
{
    public string Email { get; set; } // Email address
    public string Password { get; set; } // Password
}

