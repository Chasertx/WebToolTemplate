using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Template.Api.Brokers.Foundation.Storages
{
    /** Implementation of the generic base functions
    for the storage broker. **/
    internal partial class StorageBroker : DbContext, IStorageBroker
    {
        // Initializes the broker with the provided 
        // database configuration options.
        public StorageBroker(DbContextOptions<StorageBroker> options)
         : base(options) { }

        // Configures the database model mapping using
        // the ModelBuilder.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Executing base model creation logic.
            base.OnModelCreating(modelBuilder);
        }

        //Generic implementation inserting an entity into the DB.
        private async ValueTask<T> InsertAsync<T>(T @object)
        {
            if (@object is null) return default!;
            // Manually sets the entity state to Added for tracking.
            Entry(@object).State = EntityState.Added;
            // Persists the changes to the database async.
            await SaveChangesAsync();

            return @object;
        }

        //Generic implementation of selecting everything from storage.
        private IQueryable<T> SelectAll<T>() where T : class
        {
            // Returns the database set for the specified entity
            // type as a queryable.
            return Set<T>();
        }

        // '@object' is the instance of T being updated.
        // the @ prefix allows 'object' to be used as a
        // variable name.
        private async ValueTask<T> UpdateAsync<T>(T @object)
        {
            // 'default!' returns the default value of T
            if (@object is null) return default!;
            // 'Entry(@object) retrieves the EF core tracking entry
            // for this specific object.
            // EntityState.Modified = treat all fields as modified.
            Entry(@object).State = EntityState.Modified;
            //persists changes to database.
            await SaveChangesAsync();
            return @object;
        }

        private async ValueTask<T> DeleteAsync<T>(T @object)
        {
            if (@object is null) return default!;
            Entry(@object).State = EntityState.Deleted;
            await SaveChangesAsync();
            return @object;
        }
    }
}