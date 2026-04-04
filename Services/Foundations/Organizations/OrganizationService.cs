using Template.Api.Models.Foundation.Organization;

namespace Template.Api.Services.Foundations.Organizations;

public class OrganizationService : IOrganizationService
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
    public ValueTask<Organization> AddOrganizationAsync(Organization org)
    {
        return this.storageBroker.InsertOrganizationAsync(org);
    }

    /// <summary>
    /// Retrieves all organizations.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public IQueryable<Organization> RetrieveAllOrganizations() =>
        this.storageBroker.SelectAllOrganizations();
}