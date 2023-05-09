using Microsoft.EntityFrameworkCore;
using SharpyProxy.Database;
using SharpyProxy.Models.Core;

namespace SharpyProxy.Services;

public class CoreService
{
    private readonly AppDbContext _appDbContext;

    public CoreService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<StatsModel> GetStatsAsync()
    {
        var clusterCount = await _appDbContext.Clusters.CountAsync();
        var routeCount = await _appDbContext.Routes.CountAsync();
        var certificateCount = await _appDbContext.Certificates.CountAsync();

        return new StatsModel
        {
            ClusterCount = clusterCount,
            RouteCount = routeCount,
            CertificateCount = certificateCount
        };
    }
}