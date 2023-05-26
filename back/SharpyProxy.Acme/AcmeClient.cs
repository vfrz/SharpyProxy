using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using SharpyProxy.Acme.Account;
using SharpyProxy.Acme.Authorization;
using SharpyProxy.Acme.Challenge;
using SharpyProxy.Acme.Exceptions;
using SharpyProxy.Acme.Extensions;
using SharpyProxy.Acme.Order;

namespace SharpyProxy.Acme;

public sealed class AcmeClient : IDisposable
{
    private readonly HttpClient _httpClient;

    private AcmeDirectory Directory => _directory ?? throw new Exception($"Directory is missing, you should initialize the client first by calling {nameof(InitializeAsync)}().");
    private AcmeDirectory? _directory;
    private string? _nonce;

    public AcmeClient(AcmeSettings settings)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(settings.ServerUrl)
        };
    }

    /// <summary>
    /// Initialize the client by fetching the directory and initial nonce
    /// </summary>
    public async Task InitializeAsync()
    {
        var directoryResponse = await _httpClient.GetAsync("/directory");
        directoryResponse.EnsureSuccessStatusCode();
        _directory = await directoryResponse.Content.ReadJsonAsync<AcmeDirectory>();
        await FetchNewNonceAsync();
    }

    public async Task<AcmeAccount> NewAccountAsync(params string[] emailAddresses)
    {
        using var rsaKey = RSA.Create(2048);

        var payload = new
        {
            termsOfServiceAgreed = true,
            contact = emailAddresses.Select(e => $"mailto:{e}").ToArray()
        };

        var responseMessage = await SendSignedRequestAsync(Directory.NewAccount, payload, rsaKey);
        var dto = await responseMessage.Content.ReadJsonAsync<AcmeAccountDto>();

        var account = new AcmeAccount
        {
            RSAParameters = rsaKey.ExportParameters(true),
            Url = responseMessage.Headers.Location!.ToString(),
            Contact = dto.Contact,
            Status = AcmeAccount.ParseStatus(dto.Status),
            CreatedAt = dto.CreatedAt
        };

        return account;
    }

    public async Task<AcmeAccount> FindAccountByKeyAsync(RSA rsaKey, bool onlyReturnExisting = true)
    {
        var payload = new
        {
            onlyReturnExisting
        };

        var responseMessage = await SendSignedRequestAsync(Directory.NewAccount, payload, rsaKey);
        var dto = await responseMessage.Content.ReadJsonAsync<AcmeAccountDto>();

        var account = new AcmeAccount
        {
            RSAParameters = rsaKey.ExportParameters(true),
            Url = responseMessage.Headers.Location!.ToString(),
            Contact = dto.Contact,
            Status = AcmeAccount.ParseStatus(dto.Status),
            CreatedAt = dto.CreatedAt
        };

        return account;
    }

    public async Task<AcmeOrder> NewOrderAsync(AcmeAccount account, string domain)
    {
        var payload = new
        {
            identifiers = new[]
            {
                new
                {
                    type = "dns",
                    value = domain
                }
            }
        };

        using var accountRsaKey = account.CreateKeyFromParameters();
        var response = await SendSignedRequestAsync(Directory.NewOrder, payload, accountRsaKey, account.Url);

        var dto = await response.Content.ReadJsonAsync<AcmeOrderDto>();

        var order = new AcmeOrder
        {
            Url = response.Headers.Location!.ToString(),
            Error = dto.Error,
            Expires = dto.Expires,
            NotAfter = dto.NotAfter,
            NotBefore = dto.NotBefore,
            Identifiers = dto.Identifiers,
            Status = AcmeOrder.ParseStatus(dto.Status),
            AuthorizationUrls = dto.Authorizations,
            CertificateUrl = dto.Certificate,
            FinalizeUrl = dto.Finalize
        };

        return order;
    }

    public async Task<AcmeOrder> FetchOrderAsync(AcmeAccount account, string orderUrl)
    {
        using var accountRsaKey = account.CreateKeyFromParameters();
        var response = await SendSignedRequestAsync(orderUrl, null, accountRsaKey, account.Url);

        var dto = await response.Content.ReadJsonAsync<AcmeOrderDto>();

        var order = new AcmeOrder
        {
            Url = orderUrl,
            Error = dto.Error,
            Expires = dto.Expires,
            NotAfter = dto.NotAfter,
            NotBefore = dto.NotBefore,
            Identifiers = dto.Identifiers,
            Status = AcmeOrder.ParseStatus(dto.Status),
            AuthorizationUrls = dto.Authorizations,
            CertificateUrl = dto.Certificate,
            FinalizeUrl = dto.Finalize
        };

        return order;
    }

    public async Task<AcmeAuthorization> FetchAuthorizationAsync(AcmeAccount account, string authorizationUrl)
    {
        using var accountRsaKey = account.CreateKeyFromParameters();
        var response = await SendSignedRequestAsync(authorizationUrl, null, accountRsaKey, account.Url);

        var dto = await response.Content.ReadJsonAsync<AcmeAuthorizationDto>();

        var authorization = new AcmeAuthorization
        {
            Url = authorizationUrl,
            Identifier = dto.Identifier,
            Status = AcmeAuthorization.ParseStatus(dto.Status),
            Expires = dto.Expires,
            Challenges = dto.Challenges.Select(c => new AcmeAuthorizationChallenge
            {
                Type = AcmeAuthorizationChallenge.ParseType(c.Type),
                Status = AcmeAuthorizationChallenge.ParseStatus(c.Status),
                Url = c.Url,
                Token = c.Token,
                Validated = c.Validated,
                Error = c.Error
            }).ToArray(),
            Wildcard = dto.Wildcard
        };

        return authorization;
    }

    public async Task<AcmeChallenge> ChallengeReadyForValidationAsync(AcmeAccount account, string challengeUrl)
    {
        using var accountRsaKey = account.CreateKeyFromParameters();
        var response = await SendSignedRequestAsync(challengeUrl, new { }, accountRsaKey, account.Url);

        var dto = await response.Content.ReadJsonAsync<AcmeChallengeDto>();

        return new AcmeChallenge
        {
            Url = dto.Url,
            Error = dto.Error,
            Status = AcmeChallenge.ParseStatus(dto.Status),
            Type = AcmeChallenge.ParseType(dto.Type),
            Token = dto.Token,
            Validated = dto.Validated,
            ValidationRecord = dto.ValidationRecord.Select(v => new AcmeChallengeValidationRecord
            {
                Hostname = v.Hostname,
                Port = v.Port,
                Url = v.Url,
                AddressesResolved = v.AddressesResolved,
                AddressUsed = v.AddressUsed
            }).ToArray()
        };
    }

    public async Task<AcmeChallenge> FetchChallengeAsync(AcmeAccount account, string challengeUrl)
    {
        using var accountRsaKey = account.CreateKeyFromParameters();
        var response = await SendSignedRequestAsync(challengeUrl, null, accountRsaKey, account.Url);

        var dto = await response.Content.ReadJsonAsync<AcmeChallengeDto>();

        return new AcmeChallenge
        {
            Url = dto.Url,
            Error = dto.Error,
            Status = AcmeChallenge.ParseStatus(dto.Status),
            Type = AcmeChallenge.ParseType(dto.Type),
            Token = dto.Token,
            Validated = dto.Validated,
            ValidationRecord = dto.ValidationRecord.Select(v => new AcmeChallengeValidationRecord
            {
                Hostname = v.Hostname,
                Port = v.Port,
                Url = v.Url,
                AddressesResolved = v.AddressesResolved,
                AddressUsed = v.AddressUsed
            }).ToArray()
        };
    }

    public async Task<AcmeOrder> FinalizeOrderAsync(AcmeAccount account, string finalizeUrl, string domain, RSA certificateKey)
    {
        var csr = new CertificateRequest($"CN={domain}", certificateKey, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        var csrBytes = csr.CreateSigningRequest();

        var payload = new
        {
            csr = Base64UrlEncoder.Encode(csrBytes)
        };

        using var accountRsaKey = account.CreateKeyFromParameters();
        var response = await SendSignedRequestAsync(finalizeUrl, payload, accountRsaKey, account.Url);

        var dto = await response.Content.ReadJsonAsync<AcmeOrderDto>();

        var order = new AcmeOrder
        {
            Url = response.Headers.Location!.ToString(),
            Error = dto.Error,
            Expires = dto.Expires,
            NotAfter = dto.NotAfter,
            NotBefore = dto.NotBefore,
            Identifiers = dto.Identifiers,
            Status = AcmeOrder.ParseStatus(dto.Status),
            AuthorizationUrls = dto.Authorizations,
            CertificateUrl = dto.Certificate,
            FinalizeUrl = dto.Finalize
        };

        return order;
    }

    public async Task<string> DownloadCertificateAsync(AcmeAccount account, string certificateUrl)
    {
        using var accountRsaKey = account.CreateKeyFromParameters();
        var response = await SendSignedRequestAsync(certificateUrl, null, accountRsaKey, account.Url);

        var certificate = await response.Content.ReadAsStringAsync();

        return certificate;
    }

    public string GetAuthorizationKey(AcmeAccount account, string token)
    {
        using var accountRsaKey = account.CreateKeyFromParameters();
        return GetAuthorizationKey(accountRsaKey, token);
    }

    public string GetAuthorizationKey(RSA accountRsaKey, string token)
    {
        //TODO Maybe use this: var jwkThumbprint = new RsaSecurityKey(account.RSAParameters).ComputeJwkThumbprint();
        return $"{token}.{Base64UrlEncoder.Encode(accountRsaKey.GetJWKThumbprint())}";
    }

    private async Task FetchNewNonceAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Head, Directory.NewNonce);
        var response = await _httpClient.SendAsync(request);
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