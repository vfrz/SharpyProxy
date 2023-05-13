namespace SharpyProxy.Models.Certificate;

public class ListCertificateModel
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public DateTime Expiration { get; set; }
    
    public string Domain { get; set; }
}