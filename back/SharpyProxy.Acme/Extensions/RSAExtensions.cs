using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;

namespace SharpyProxy.Acme.Extensions;

public static class RSAExtensions
{
    public static byte[] GetJWKThumbprint(this RSA key)
    {
        var keyExportParameters = key.ExportParameters(false);
        var jwk = new
        {
            e = Base64UrlEncoder.Encode(keyExportParameters.Exponent),
            kty = "RSA",
            n = Base64UrlEncoder.Encode(keyExportParameters.Modulus)
        };
        var jsonJwk = JsonSerializer.Serialize(jwk);
        var thumbprint = SHA256.HashData(Encoding.UTF8.GetBytes(jsonJwk));
        return thumbprint;
    }
}