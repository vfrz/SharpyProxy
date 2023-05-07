using SharpyProxy.Models.Cluster.Destination;

namespace SharpyProxy.Models.Cluster;

public class ClusterModel
{
    public string Id { get; set; }
    
    public ClusterDestinationModel[] Destinations { get; set; }
}