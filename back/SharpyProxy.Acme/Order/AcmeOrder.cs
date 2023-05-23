namespace SharpyProxy.Acme.Order;

public class AcmeOrder
{
    public required string Url { get; init; } 
    
    public required AcmeOrderStatus Status { get; init; }
    
    public required DateTime? Expires { get; init; }
    
    public required AcmeIdentifier[] Identifiers { get; init; }
    
    public required DateTime? NotBefore { get; init; }
    
    public required DateTime? NotAfter { get; init; }

    public required AcmeError? Error { get; init; }
    
    public required string[] AuthorizationUrls { get; init; }
    
    public required string FinalizeUrl { get; init; }
    
    public required string? CertificateUrl { get; init; }

    public static AcmeOrderStatus ParseStatus(string status)
    {
        return status switch
        {
            "pending" => AcmeOrderStatus.Pending,
            "ready" => AcmeOrderStatus.Ready,
            "processing" => AcmeOrderStatus.Processing,
            "valid" => AcmeOrderStatus.Valid,
            "invalid" => AcmeOrderStatus.Invalid,
            _ => throw new Exception($"Failed to parse order status from value: {status}")
        };
    }
}