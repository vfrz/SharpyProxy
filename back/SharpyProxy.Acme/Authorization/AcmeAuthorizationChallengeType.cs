namespace SharpyProxy.Acme.Authorization;

public enum AcmeAuthorizationChallengeType
{
    Http01,
    Dns01,
    TlsAlpn01,
    TlsSni01
}