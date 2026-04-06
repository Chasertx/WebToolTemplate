using Template.Api.Models.Foundation.Organization;

public partial interface IStorageBroker
{
    ValueTask<Organization> InsertOrganizationAsync(Organization org);
    ValueTask<Organization?> SelectOrganizationByIdAsync(Guid userId);
    IQueryable<Organization> SelectAllOrganizations();
}