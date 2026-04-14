using Template.Api.Models.Foundation.Organization;

namespace Template.Api.Services.Foundations.Organizations;

// Interface for the Organization Service.
public interface IOrganizationService
{
    ValueTask<Organization> AddOrganizationAsync(Organization org);
    ValueTask<Organization?> RetrieveOrganizationByIdAsync(Guid orgId);
    IQueryable<Organization> RetrieveAllOrganizations();
    ValueTask<Organization> RemoveOrganizationByIdAsync(Guid orgId);
    ValueTask<Organization> ModifyOrganizationAsync(Organization org);
}