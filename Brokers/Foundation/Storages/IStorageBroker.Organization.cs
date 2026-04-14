using Template.Api.Models.Foundation.Organization;

public partial interface IStorageBroker
{
    ValueTask<Organization> InsertOrganizationAsync(Organization org);
    ValueTask<Organization?> SelectOrganizationByIdAsync(Guid userId);
    IQueryable<Organization> SelectAllOrganizations();
    ValueTask<Organization> UpdateOrganizationAsync(Organization org);
    ValueTask<Organization> DeleteOrganizationAsync(Organization org);
}