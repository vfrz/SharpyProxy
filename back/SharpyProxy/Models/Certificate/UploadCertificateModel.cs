namespace SharpyProxy.Models.Certificate;

public class UploadCertificateModel
{
    public string Name { get; set; }
    
    public string Pem { get; set; }
    
    public string Key { get; set; }
}