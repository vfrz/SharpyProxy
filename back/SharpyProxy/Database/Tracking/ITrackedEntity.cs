namespace SharpyProxy.Database.Tracking;

public interface ITrackedEntity
{
    public DateTime CreatedDateUtc { get; set; }
    
    public DateTime UpdatedDateUtc { get; set; }
}