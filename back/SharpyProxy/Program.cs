using SharpyProxy.Setup;

var builder = WebApplication.CreateBuilder(args)
    .SetupDatabase()
    .SetupProxyServices();

builder.Services.AddControllers();
builder.Services.AddCors(o => o.AddPolicy("Default", policyBuilder =>
{
    policyBuilder
        .AllowCredentials()
        .WithOrigins("http://localhost:3000")
        .AllowAnyMethod()
        .AllowAnyHeader();
}));


var app = builder.Build();
app.UseCors("Default");
app.MapControllers();
app.MapReverseProxy();
app.Run();