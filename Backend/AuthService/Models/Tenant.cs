using System;

namespace AuthService.Models;

public class Tenant
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Domain {get; set; }
    public ICollection<TenantUser> Users { get; set; }
}
