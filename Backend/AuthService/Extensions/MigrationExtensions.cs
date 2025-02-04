using System;
using AuthService.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        //creating service scope
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        //resolving database context from this service scope
        using  ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        dbContext.Database.Migrate();
    }
}
