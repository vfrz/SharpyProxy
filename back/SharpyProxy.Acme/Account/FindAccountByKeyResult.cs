namespace SharpyProxy.Acme.Account;

public class FindAccountByKeyResult
{
    public required FindAccountByKeyHttpResponse HttpResponse { get; init; }
    
    public required AcmeAccount Account { get; init; }
}