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

        /// <summary>
        /// Retrieves all entries in the Organization
        /// table.
        /// </summary>
        /// <returns></returns>
        public IQueryable<Organization> SelectAllOrganizations() =>
            SelectAll<Organization>();

        /// <summary>
        /// Inserts a new organization into the DB.
        /// </summary>
        /// <param name="org"></param>
        /// <returns></returns>
        public async ValueTask<Organization> InsertOrganizationAsync(Organization org) =>
            await InsertAsync(org);

        /// <summary>
        /// Selects an organization according to it's ID. 
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public async ValueTask<Organization?> SelectOrganizationByIdAsync(Guid orgId)
        {
            return await Organizations.FindAsync(orgId);
        }

        /// <summary>
        /// Applies updates to specified organization.
        /// </summary>
        /// <param name="org"></param>
        /// <returns></returns>
        public async ValueTask<Organization> UpdateOrganizationAsync(Organization org) =>
            await UpdateAsync(org);

        /// <summary>
        /// Deletes a specified organization from the 
        /// database.
        /// </summary>
        /// <param name="org"></param>
        /// <returns></returns>
        public async ValueTask<Organization> DeleteOrganizationAsync(Organization org) =>
            await DeleteAsync(org);
    }
}