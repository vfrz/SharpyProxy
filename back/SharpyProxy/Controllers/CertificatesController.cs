using Microsoft.AspNetCore.Mvc;
using SharpyProxy.Models.Certificate;
using SharpyProxy.Services;

namespace SharpyProxy.Controllers;

[Route("certificates")]
public class CertificatesController : ControllerBase
{
    [HttpPost]
    public async Task<Guid> Upload([FromBody] UploadCertificateModel model, [FromServices] CertificateService certificateService)
    {
        var id = await certificateService.UploadAsync(model);
        return id;
    }

    [HttpDelete("{certificateId:guid}")]
    public async Task Delete([FromRoute] Guid certificateId, [FromServices] CertificateService certificateService)
    {
        await certificateService.DeleteAsync(certificateId);
    }

    [HttpGet]
    public async Task<ListCertificateModel[]> List([FromServices] CertificateService certificateService)
    {
        var models = await certificateService.ListAsync();
        return models;
    }
}