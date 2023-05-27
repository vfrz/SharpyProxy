using System.Net;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore;
using SharpyProxy.Acme;
using SharpyProxy.Database;

namespace SharpyProxy.Middlewares;

public class AcmeChallengeMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Path.StartsWithSegments("/.well-known/acme-challenge"))
        {
            var dbContext = context.RequestServices.GetRequiredService<AppDbContext>();
            var acmeClient = context.RequestServices.GetRequiredService<AcmeClient>();
            
            var pathUri = new Uri(context.Request.GetEncodedUrl());
            var domain = pathUri.Host;
            var token = pathUri.Segments.Last();

            var challenge = await dbContext.LetsEncryptChallenges
                .AsNoTracking()
                .Include(challenge => challenge.LetsEncryptAccount)
                .FirstOrDefaultAsync(challenge => challenge.Domain == domain && challenge.Token == token);

            if (challenge is not null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.ContentType = "text/plain";

                using var rsa = RSA.Create();
                rsa.ImportRSAPrivateKey(challenge.LetsEncryptAccount.RSABytes, out _);

                var authorizationKey = acmeClient.GetAuthorizationKey(rsa, challenge.Token);
                
                await context.Response.WriteAsync(authorizationKey);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
        }
        else
        {
            await next(context);
        }
    }
}