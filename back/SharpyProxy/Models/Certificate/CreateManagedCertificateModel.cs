namespace SharpyProxy.Models.Certificate;

public class CreateManagedCertificateModel
{
    public string Name { get; set; }
    
    public string Domain { get; set; }
    
    public string Email { get; set; }
}