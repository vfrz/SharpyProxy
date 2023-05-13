using SharpyProxy.Models.Cluster.Destination;

namespace SharpyProxy.Models.Cluster;

public class ClusterModel
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public ClusterDestinationModel[] Destinations { get; set; }
    
    public bool Enabled { get; set; }
}