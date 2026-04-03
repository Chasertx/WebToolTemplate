using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Template.Api.Brokers.Foundation.Storages
{
    /** Implementation of the generic base functions
    for the storage broker. **/
    internal partial class StorageBroker : DbContext, IStorageBroker
    {
        public StorageBroker(DbContextOptions<StorageBroker> options)
         : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        //Generic implementation inserting an entity into the DB.
        private async ValueTask<T> InsertAsync<T>(T @object)
        {
            Entry(@object).State = EntityState.Added;
            await SaveChangesAsync();

            return @object;
        }

        //Generic implementation of selecting everything from storage.
        private IQueryable<T> SelectAll<T>() where T : class
        {
            return Set<T>();
        }
    }
}