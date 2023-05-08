namespace SharpyProxy.Models.Certificate;

public class ListCertificateModel
{
    public string Id { get; set; }
    
    public DateTime Expiration { get; set; }
    
    public string Domain { get; set; }
}