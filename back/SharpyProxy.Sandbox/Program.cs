using System.Security.Cryptography;
using System.Text.Json;
using SharpyProxy.Acme;

const string accountPemFile = "account.pem";
const string emailAddress = "test@sharpyproxy.dev";
const string domain = "sharpyproxy.dev";

using var client = new AcmeClient(new AcmeSettings
{
    ServerUrl = AcmeKnownServers.LetsEncryptV2StagingUrl
});

await client.InitializeAsync();

AcmeAccount account;

if (File.Exists(accountPemFile))
{
    var key = RSA.Create();
    key.ImportFromPem(await File.ReadAllTextAsync(accountPemFile));
    account = await client.LoadAccountFromKeyAsync(key);
}
else
{
    account = await client.CreateAccountAsync(emailAddress);
    await File.WriteAllTextAsync(accountPemFile, account.Key.ExportRSAPrivateKeyPem());
}


var order = await client.NewOrderAsync(account, domain);
var challenges = await client.GetAuthorizationChallengesAsync(account, order.Authorizations.Single());

Console.WriteLine(JsonSerializer.Serialize(challenges));