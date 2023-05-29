namespace SharpyProxy.Acme.Certificate;

public class AcmeCertificates
{
    public string[] PemCertificates { get; }

    public string EndUserPemCertificate => PemCertificates.First();

    public string[] IntermediatePemCertificates => PemCertificates[1..];

    public AcmeCertificates(string[] pemCertificates)
    {
        PemCertificates = pemCertificates;
    }
}