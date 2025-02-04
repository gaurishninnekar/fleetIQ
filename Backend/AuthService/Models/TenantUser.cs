using System;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Models;

public class TenantUser: IdentityUser
{
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; }
    public string Role { get; set; }
}
