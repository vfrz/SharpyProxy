namespace SharpyProxy.Models.Certificate;

public class UploadCertificateModel
{
    public string Id { get; set; }
    
    public string Pem { get; set; }
    
    public string Key { get; set; }
}