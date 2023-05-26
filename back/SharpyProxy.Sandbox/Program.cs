using System.Security.Cryptography;
using System.Text.Json;
using SharpyProxy.Acme;
using SharpyProxy.Acme.Account;

const string accountPemFile = "account.pem";
const string emailAddress = "";
const string domain = "";

using var client = new AcmeClient(new AcmeSettings
{
    ServerUrl = AcmeKnownServers.LetsEncryptV2StagingUrl
});

await client.InitializeAsync();

AcmeAccount account;

if (File.Exists(accountPemFile))
{
    using var rsaKey = RSA.Create();
    rsaKey.ImportFromPem(await File.ReadAllTextAsync(accountPemFile));
    account = await client.FindAccountByKeyAsync(rsaKey);
}
else
{
    account = await client.NewAccountAsync(emailAddress);
    using var rsaKey = account.CreateKeyFromParameters();
    await File.WriteAllTextAsync(accountPemFile, rsaKey.ExportRSAPrivateKeyPem());
}

var order = await client.NewOrderAsync(account, domain);
var authorizationUrl = order.AuthorizationUrls.Single();
var authorization = await client.FetchAuthorizationAsync(account, authorizationUrl);
Console.WriteLine(JsonSerializer.Serialize(authorization));
Console.WriteLine($"Key: {client.GetAuthorizationKey(account, authorization.HttpChallenge!.Token)}");
Console.ReadLine();

var challenge = await client.ChallengeReadyForValidationAsync(account, authorization.HttpChallenge!.Url);
Console.WriteLine(JsonSerializer.Serialize(challenge));
Console.ReadLine();

var certificateKey = RSA.Create(2048);
if (File.Exists("certificate-key.pem"))
    certificateKey.ImportFromPem(await File.ReadAllTextAsync("certificate-key.pem"));
else
    await File.WriteAllTextAsync("certificate-key.pem", certificateKey.ExportRSAPrivateKeyPem());

var finalizedOrder = await client.FinalizeOrderAsync(account, order.FinalizeUrl, domain, certificateKey);
Console.WriteLine(JsonSerializer.Serialize(finalizedOrder));
Console.ReadLine();

var certificateUrl = finalizedOrder.CertificateUrl;

if (certificateUrl is null)
{
    var orderAfter = await client.FetchOrderAsync(account, order.Url);
    Console.WriteLine(JsonSerializer.Serialize(orderAfter));
    Console.ReadLine();

    certificateUrl = orderAfter.CertificateUrl;
}

var certificates = await client.DownloadCertificateAsync(account, certificateUrl!);

Console.WriteLine("Certificate downloaded");
await File.WriteAllTextAsync("certificate.pem", certificates.EndUserPemCertificate);