using Template.Api.Models.Foundation.Organization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Template.Api.Services.Foundations.Organizations;

public partial class OrganizationService : IOrganizationService
{
    private readonly IStorageBroker storageBroker;

    /// <summary>
    /// Injecting and setting the storage broker.
    /// </summary>
    /// <param name="storageBroker"></param>
    public OrganizationService(IStorageBroker storageBroker) =>
        this.storageBroker = storageBroker;

    /// <summary>
    /// Calls the broker service to persist a
    /// organization to the database.
    /// </summary>
    /// <param name="org"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async ValueTask<Organization> AddOrganizationAsync(Organization org)
    {
        // Await the TryCatch and then tell the compiler the result won't be null
        return (await TryCatch(async () =>
        {
            return await this.storageBroker.InsertOrganizationAsync(org);
        }))!;
    }

    /// <summary>
    /// Retrieves a specific organization according
    /// to it's organizationId.
    /// </summary>
    /// <param name="orgId"></param>
    /// <returns></returns>
    public ValueTask<Organization?> RetrieveOrganizationByIdAsync(Guid orgId) =>
    TryCatch(async () =>
    {
        IQueryable<Organization?> organizations =
            this.storageBroker.SelectAllOrganizations();

        // Filters the query for the specific ID
        return await organizations.FirstOrDefaultAsync(org => org!.Id == orgId);
    });

    /// <summary>
    /// Retrieves all organizations.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public IQueryable<Organization> RetrieveAllOrganizations() =>
    TryCatch(() =>
    {
        return this.storageBroker.SelectAllOrganizations();
    });
}