using System.Globalization;
using System.Security.Cryptography.X509Certificates;

namespace SharpyProxy.Extensions;

public static class X509Certificate2Extensions
{
    public static string GetDomain(this X509Certificate2 certificate) => certificate.GetNameInfo(X509NameType.DnsName, false);

    public static DateTime GetExpiration(this X509Certificate2 certificate) => DateTime.Parse(certificate.GetExpirationDateString(), CultureInfo.InvariantCulture);
}