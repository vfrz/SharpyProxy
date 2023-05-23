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
    var findAccountResult = await client.FindAccountByKeyAsync(rsaKey);
    account = findAccountResult.Account;
}
else
{
    var newAccountResult = await client.NewAccountAsync(emailAddress);
    account = newAccountResult.Account;
    using var rsaKey = account.CreateKeyFromParameters();
    await File.WriteAllTextAsync(accountPemFile, rsaKey.ExportRSAPrivateKeyPem());
}

var order = await client.NewOrderAsync(account, domain);
Console.WriteLine(JsonSerializer.Serialize(order));

var authorization = order.Authorizations.Single();
var challenges = await client.GetAuthorizationChallengesAsync(account, authorization);
Console.WriteLine(JsonSerializer.Serialize(challenges));

var httpChallenge = challenges.Challenges.First(c => c.Type.Contains("http"));
Console.WriteLine($"Key: {client.GetAuthorizationKey(account, httpChallenge.Token)}");

Console.ReadLine();

var authorizationReadyResponse = await client.AuthorizationReadyForCheckAsync(account, httpChallenge.Url);
Console.WriteLine(JsonSerializer.Serialize(authorizationReadyResponse));
Console.ReadLine();

var certificateKey = RSA.Create(2048);
if (File.Exists("certificate-key.pem"))
    certificateKey.ImportFromPem(await File.ReadAllTextAsync("certificate-key.pem"));
else
    await File.WriteAllTextAsync("certificate-key.pem", certificateKey.ExportRSAPrivateKeyPem());

var updatedOrderResponse = await client.FinalizeChallengeAsync(account, order.Finalize, domain, certificateKey);
Console.WriteLine(JsonSerializer.Serialize(updatedOrderResponse));
Console.ReadLine();

var certUrl = updatedOrderResponse.Certificate;

if (certUrl is null)
{
    var orderAfter = await client.GetOrderAsync(account, order.Location);
    Console.WriteLine(JsonSerializer.Serialize(orderAfter));
    Console.ReadLine();

    certUrl = orderAfter.Certificate;
}

var certificate = await client.DownloadCertificateAsync(account, certUrl!);

Console.WriteLine("Certificate downloaded");
await File.WriteAllTextAsync("certificate.pem", certificate);