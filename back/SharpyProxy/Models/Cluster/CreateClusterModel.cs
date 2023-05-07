using SharpyProxy.Models.Cluster.Destination;

namespace SharpyProxy.Models.Cluster;

public class CreateClusterModel
{
    public string Id { get; set; }
    
    public CreateClusterDestinationModel[] Destinations { get; set; }
}