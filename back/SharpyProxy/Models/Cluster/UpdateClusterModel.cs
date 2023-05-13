using SharpyProxy.Models.Cluster.Destination;

namespace SharpyProxy.Models.Cluster;

public class UpdateClusterModel
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public UpdateClusterDestinationModel[] Destinations { get; set; }
    
    public bool Enabled { get; set; }
}