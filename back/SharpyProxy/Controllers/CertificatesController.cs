using Microsoft.AspNetCore.Mvc;
using SharpyProxy.Models.Certificate;
using SharpyProxy.Services;

namespace SharpyProxy.Controllers;

[Route(".proxy-api/certificates")]
public class CertificatesController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<string>> Upload([FromBody] UploadCertificateModel model, [FromServices] CertificateService certificateService)
    {
        await certificateService.UploadAsync(model);
        return Ok();
    }

    [HttpDelete("{certificateId:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid certificateId, [FromServices] CertificateService certificateService)
    {
        await certificateService.DeleteAsync(certificateId);
        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<ListCertificateModel[]>> List([FromServices] CertificateService certificateService)
    {
        var models = await certificateService.ListAsync();
        return Ok(models);
    }
}