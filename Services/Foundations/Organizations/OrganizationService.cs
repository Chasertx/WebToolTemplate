using Template.Api.Models.Foundation.Organization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using Template.Api.Brokers.Logging;

namespace Template.Api.Services.Foundations.Organizations;

/// <summary>
/// This is the "Brain" of organization operations, it 
/// handles the sequence of the events.
/// </summary>
public partial class OrganizationService : IOrganizationService
{
    private readonly IStorageBroker storageBroker;
    private readonly ILoggingBroker loggingBroker;

    /// <summary>
    /// Injecting and setting the storage broker.
    /// </summary>
    /// <param name="storageBroker"></param>
    /// /// <param name="loggingBroker"></param>
    public OrganizationService(
        IStorageBroker storageBroker,
        ILoggingBroker loggingBroker)
    {
        this.storageBroker = storageBroker;
        this.loggingBroker = loggingBroker;
    }

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
            // All data should be validated before
            // they are persisted to the database.
            ValidateOrganizationOnAdd(org);
            await ValidateOrganizationDoesNotAlreadyExistAsync(org);

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
        return await this.storageBroker.SelectOrganizationByIdAsync(orgId);
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