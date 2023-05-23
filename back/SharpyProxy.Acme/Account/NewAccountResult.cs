namespace SharpyProxy.Acme.Account;

public class NewAccountResult
{
    public required NewAccountHttpResponse HttpResponse { get; init; }
    
    public required AcmeAccount Account { get; init; }
}