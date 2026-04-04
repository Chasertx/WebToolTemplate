using Template.Api.Models.Foundation.Organization;

namespace Template.Api.Services.Foundations.Organizations;

public interface IOrganizationService
{
    ValueTask<Organization> AddOrganizationAsync(Organization org);
    IQueryable<Organization> RetrieveAllOrganizations();
}