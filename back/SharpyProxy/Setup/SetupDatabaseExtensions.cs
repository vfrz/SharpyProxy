using Microsoft.EntityFrameworkCore;
using SharpyProxy.Database;
using SharpyProxy.Database.Tracking;

namespace SharpyProxy.Setup;

public static class SetupDatabaseExtensions
{
    public static WebApplicationBuilder SetupDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options
                .AddInterceptors(new TrackedEntitySaveChangesInterceptor())
                .UseNpgsql(builder.Configuration.GetConnectionString("Main"));
        });
        return builder;
    }
}