namespace SharpyProxy.Acme.Exceptions;

public class AcmeException : Exception
{
    public string Type { get; }

    public string Detail { get; }

    public int Status { get; }

    public AcmeException(string type, string detail, int status) : base($"[{type}] {detail} ({status})")
    {
        Type = type;
        Detail = detail;
        Status = status;
    }

    public AcmeException(AcmeErrorResponse errorResponse)
        : this(errorResponse.Type, errorResponse.Detail, errorResponse.Status)
    {
    }
}