using System.Net;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore;
using SharpyProxy.Acme;
using SharpyProxy.Database;
using SharpyProxy.Services;

namespace SharpyProxy.Middlewares;

public class AcmeChallengeMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Path.StartsWithSegments("/.well-known"))
        {
            if (context.Request.Path.StartsWithSegments("/.well-known/acme-challenge"))
            {
                var dbContext = context.RequestServices.GetRequiredService<AppDbContext>();

                var pathUri = new Uri(context.Request.GetEncodedUrl());
                var domain = pathUri.Host;
                var token = pathUri.Segments.Last();

                var challenge = await dbContext.LetsEncryptChallenges
                    .AsNoTracking()
                    .Include(challenge => challenge.LetsEncryptAccount)
                    .FirstOrDefaultAsync(challenge => challenge.Domain == domain && challenge.Token == token);

                if (challenge is not null)
                {
                    context.Response.StatusCode = (int) HttpStatusCode.OK;
                    context.Response.ContentType = "text/plain";

                    using var rsa = RSA.Create();
                    rsa.ImportRSAPrivateKey(challenge.LetsEncryptAccount.RSABytes, out _);
                    var rsaParameters = rsa.ExportParameters(true);

                    var authorizationKey = AcmeClient.GetAuthorizationKey(rsaParameters, challenge.Token);

                    await context.Response.WriteAsync(authorizationKey);
                    return;
                }
            }
            else if (context.Request.Path.StartsWithSegments("/.well-known/sharpy-proxy-domain-verification"))
            {
                var verificationService = context.RequestServices.GetRequiredService<DomainVerificationService>();

                var pathUri = new Uri(context.Request.GetEncodedUrl());
                var domain = pathUri.Host;

                var key = verificationService.GetKeyForDomain(domain);

                if (key is null)
                {
                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    return;
                }

                context.Response.StatusCode = (int) HttpStatusCode.OK;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync(key);
                return;
            }
        }

        await next(context);
    }
}