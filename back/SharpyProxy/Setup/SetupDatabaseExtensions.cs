using Microsoft.EntityFrameworkCore;
using SharpyProxy.Database;

namespace SharpyProxy.Setup;

public static class SetupDatabaseExtensions
{
    public static WebApplicationBuilder SetupDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("Main"));
        });
        return builder;
    }
}