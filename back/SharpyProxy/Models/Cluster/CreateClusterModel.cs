using SharpyProxy.Models.Cluster.Destination;

namespace SharpyProxy.Models.Cluster;

public class CreateClusterModel
{
    public string Name { get; set; }
    
    public CreateClusterDestinationModel[] Destinations { get; set; }
    
    public bool Enabled { get; set; }
}