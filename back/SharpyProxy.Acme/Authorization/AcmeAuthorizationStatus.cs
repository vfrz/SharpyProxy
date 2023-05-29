namespace SharpyProxy.Acme.Authorization;

public enum AcmeAuthorizationStatus
{
    Pending,
    Valid,
    Invalid,
    Deactivated,
    Expired,
    Revoked
}