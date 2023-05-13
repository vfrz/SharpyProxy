namespace SharpyProxy.Models.Cluster.Destination;

public class UpdateClusterDestinationModel
{
    public Guid? Id { get; set; }
    
    public string Name { get; set; }
    
    public string Address { get; set; }
}