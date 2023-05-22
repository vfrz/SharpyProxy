using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using SharpyProxy.Acme.Exceptions;
using SharpyProxy.Acme.Extensions;
using SharpyProxy.Acme.Order;
using SharpyProxy.Extensions;

namespace SharpyProxy.Acme;

public class AcmeClient : IDisposable
{
    private readonly AcmeSettings _settings;
    private readonly HttpClient _httpClient;

    private AcmeDirectory Directory => _directory ?? throw new Exception($"Directory is missing, you should initialize the client first by calling {nameof(InitializeAsync)}().");
    private AcmeDirectory? _directory;
    private string? _nonce;

    public AcmeClient(AcmeSettings settings)
    {
        _settings = settings;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(settings.ServerUrl)
        };
    }

    public async Task InitializeAsync()
    {
        _directory = await _httpClient.GetFromJsonAsync<AcmeDirectory>("/directory") ?? throw new Exception();
        await FetchNewNonceAsync();
    }

    public async Task<AcmeAccount> CreateAccountAsync(string emailAddress)
    {
        var key = RSA.Create(2048);

        var payload = new
        {
            termsOfServiceAgreed = true,
            contact = new[]
            {
                $"mailto:{emailAddress}"
            }
        };

        var response = await SendSignedRequestAsync(Directory.NewAccount, payload, key);

        var accountUrl = response.Headers.Location!.ToString();

        return new AcmeAccount
        {
            Key = key,
            Url = accountUrl
        };
    }

    public async Task<AcmeAccount> LoadAccountFromKeyAsync(RSA key)
    {
        var payload = new
        {
            onlyReturnExisting = true
        };

        var response = await SendSignedRequestAsync(Directory.NewAccount, payload, key);

        var accountUrl = response.Headers.Location!.ToString();

        return new AcmeAccount
        {
            Key = key,
            Url = accountUrl
        };
    }

    public async Task<OrderResponse> NewOrderAsync(AcmeAccount account, params string[] domains)
    {
        var payload = new
        {
            identifiers = domains.Select(domain => new
            {
                type = "dns",
                value = domain
            }).ToArray()
        };

        var response = await SendSignedRequestAsync(Directory.NewOrder, payload, account.Key, account.Url);

        var orderResponse = await response.Content.ReadJsonAsync<OrderResponse>();

        orderResponse.Location = response.Headers.Location!.ToString();

        return orderResponse;
    }
    
    public async Task<OrderResponse> GetOrderAsync(AcmeAccount account, string order)
    {
        var response = await SendSignedRequestAsync(order, null, account.Key, account.Url);

        var orderResponse = await response.Content.ReadJsonAsync<OrderResponse>();

        orderResponse.Location = order;

        return orderResponse;
    }

    public async Task<AuthorizationChallengesResponse> GetAuthorizationChallengesAsync(AcmeAccount account, string authorization)
    {
        var response = await SendSignedRequestAsync(authorization, null, account.Key, account.Url);

        var challengesResponse = await response.Content.ReadJsonAsync<AuthorizationChallengesResponse>();

        return challengesResponse;
    }

    public async Task<AuthorizationReadyChallenge> AuthorizationReadyForCheckAsync(AcmeAccount account, string authorizationUrl)
    {
        var response = await SendSignedRequestAsync(authorizationUrl, new { }, account.Key, account.Url);

        var authorizationReadyChallengeResponse = await response.Content.ReadJsonAsync<AuthorizationReadyChallenge>();

        return authorizationReadyChallengeResponse;
    }

    public async Task<UpdatedOrderResponse> GetChallengeUpdatedAsync(AcmeAccount account, string authorizationUrl)
    {
        var response = await SendSignedRequestAsync(authorizationUrl, null, account.Key, account.Url);

        var updatedOrderResponse = await response.Content.ReadJsonAsync<UpdatedOrderResponse>();

        return updatedOrderResponse;
    }

    public async Task<UpdatedOrderResponse> FinalizeChallengeAsync(AcmeAccount account, string url, string domain, RSA certificateKey)
    {
        var csr = new CertificateRequest($"CN={domain}", certificateKey, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        var csrBytes = csr.CreateSigningRequest();

        var payload = new
        {
            csr = Base64UrlEncoder.Encode(csrBytes)
        };

        var response = await SendSignedRequestAsync(url, payload, account.Key, account.Url);

        var updatedOrderResponse = await response.Content.ReadJsonAsync<UpdatedOrderResponse>();

        updatedOrderResponse.Location = response.Headers.Location!.ToString();

        return updatedOrderResponse;
    }

    public async Task<string> DownloadCertificateAsync(AcmeAccount account, string certificateUrl)
    {
        var response = await SendSignedRequestAsync(certificateUrl, null, account.Key, account.Url);

        var certificate = await response.Content.ReadAsStringAsync();

        return certificate;
    }

    public string GetAuthorizationKey(AcmeAccount account, string token)
    {
        return $"{token}.{Base64UrlEncoder.Encode(account.Key.GetJWKThumbprint())}";
    }

    private async Task FetchNewNonceAsync()
    {
        var response = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, Directory.NewNonce));
        response.EnsureSuccessStatusCode();
        _nonce = response.Headers.GetValues("Replay-Nonce").First();
    }

    private async Task<HttpResponseMessage> SendSignedRequestAsync(string url, object? payload, RSA key, string? accountUrl = null)
    {
        var accountKeyExportParameters = key.ExportParameters(false);

        string encodedProtected;

        if (accountUrl is null)
        {
            var jwk = new
            {
                e = Base64UrlEncoder.Encode(accountKeyExportParameters.Exponent),
                kty = "RSA",
                n = Base64UrlEncoder.Encode(accountKeyExportParameters.Modulus)
            };
            var @protected = new
            {
                alg = "RS256",
                jwk,
                nonce = _nonce,
                url
            };
            encodedProtected = Base64UrlEncoder.Encode(JsonSerializer.Serialize(@protected));
        }
        else
        {
            var @protected = new
            {
                alg = "RS256",
                kid = accountUrl,
                nonce = _nonce,
                url
            };
            encodedProtected = Base64UrlEncoder.Encode(JsonSerializer.Serialize(@protected));
        }

        var encodedPayload = payload is null ? string.Empty : Base64UrlEncoder.Encode(JsonSerializer.Serialize(payload));

        var messageBytes = Encoding.UTF8.GetBytes($"{encodedProtected}.{encodedPayload}");
        var signature = key.SignData(messageBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

        var request = new
        {
            @protected = encodedProtected,
            payload = encodedPayload,
            signature = Base64UrlEncoder.Encode(signature)
        };

        var serializedRequest = JsonSerializer.Serialize(request);

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
        var content = new StringContent(serializedRequest, Encoding.UTF8);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/jose+json");
        httpRequest.Content = content;

        var response = await _httpClient.SendAsync(httpRequest);

        if (response.Headers.Contains("Replay-Nonce"))
            _nonce = response.Headers.GetValues("Replay-Nonce").First();
        else
            await FetchNewNonceAsync();

        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadJsonAsync<AcmeErrorResponse>();
            throw new AcmeException(errorResponse);
        }

        return response;
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}