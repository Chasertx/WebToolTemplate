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
    }
}