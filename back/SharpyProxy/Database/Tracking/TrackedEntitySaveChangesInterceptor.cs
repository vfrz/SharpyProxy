using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace SharpyProxy.Database.Tracking;

public class TrackedEntitySaveChangesInterceptor : ISaveChangesInterceptor
{
    public InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        SetCreatedAndUpdatedDates(eventData);
        return result;
    }

    public ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = new())
    {
        SetCreatedAndUpdatedDates(eventData);
        return new ValueTask<InterceptionResult<int>>(result);
    }

    private static void SetCreatedAndUpdatedDates(DbContextEventData eventData)
    {
        if (eventData.Context is null)
            return;
        
        var entries = eventData.Context.ChangeTracker
            .Entries()
            .Where(e => e is {Entity: ITrackedEntity, State: EntityState.Added or EntityState.Modified});

        var now = DateTime.UtcNow;

        foreach (var entry in entries)
        {
            var entity = (ITrackedEntity) entry.Entity;
            entity.UpdatedDateUtc = now;
            if (entry.State is EntityState.Added)
                entity.CreatedDateUtc = now;
        }
    }
}