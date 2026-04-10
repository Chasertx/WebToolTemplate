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
        // Defines the database set for Organization entities.
        public DbSet<Organization> Organizations { get; set; }

        // Retrieves all organizations by calling the 
        // generic SelectAll method.
        public IQueryable<Organization> SelectAllOrganizations() =>
            SelectAll<Organization>();

        // Inserts a new organization record asynchronously
        // using the generic InsertAsync method.
        public async ValueTask<Organization> InsertOrganizationAsync(Organization org) =>
            await InsertAsync(org);

        public async ValueTask<Organization?> SelectOrganizationByIdAsync(Guid orgId)
        {
            return await Organizations.FindAsync(orgId);
        }
    }
}