using Template.Api.Models.Foundation.Organization;

namespace Template.Api.Services.Foundations.Organizations;

public interface IOrganizationService
{
    ValueTask<Organization> AddOrganizationAsync(Organization org);
    ValueTask<Organization?> RetrieveOrganizationByIdAsync(Guid orgId);
    IQueryable<Organization> RetrieveAllOrganizations();
}