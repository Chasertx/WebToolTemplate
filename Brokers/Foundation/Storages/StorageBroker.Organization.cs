using System.Linq;
using System.Threading.Tasks;
using Template.Api.Models.Foundation.Organization;
using Microsoft.EntityFrameworkCore;

namespace Template.Api.Brokers.Foundation.Storages
{
    /** Implementation of the book functions
    for the storage broker.
    All functions should delegate to generic 
    functionalities found in StorageBroker.cs **/
    internal partial class StorageBroker : DbContext, IStorageBroker
    {
        public DbSet<Organization> Organizations { get; set; }

        public IQueryable<Organization> SelectAllOrganizations() =>
            SelectAll<Organization>();

        public async ValueTask<Organization> InsertOrganizationAsync(Organization org) =>
            await InsertAsync(org);
    }
}